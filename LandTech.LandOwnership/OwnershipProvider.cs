using System.Collections.Generic;
using System.Linq;

namespace LandTech.LandOwnership
{
    public class OwnershipProvider
    {
        private readonly Dictionary<string, string[]> _companyRelationship;
        private readonly Dictionary<string, string[]> _landOwnership;

        public OwnershipProvider(IEnumerable<CompanyRelationship> companyRelationship, IEnumerable<LandOwnership> landOwnership)
        {
            _companyRelationship = BuildCompanyRelationships(companyRelationship);
            _landOwnership = BuildLandOwnershipByCompany(landOwnership);
        }
        
        public OwnershipResult OwnershipFor(string companyId)
        {
            var directlyOwned = _landOwnership.ContainsKey(companyId) ? _landOwnership[companyId].Length : 0;
            var companiesInChain = GetChildCompanies(companyId).ToHashSet();
            var indirectlyOwned = _landOwnership.Where(lo => companiesInChain.Contains(lo.Key)).SelectMany(x => x.Value).Count();

            return new OwnershipResult {OwnedDirectly = directlyOwned, OwnedIndirectly = indirectlyOwned };
        }
        
        private IEnumerable<string> GetChildCompanies(string companyId)
        {
            if (!_companyRelationship.ContainsKey(companyId))
            {
                return Enumerable.Empty<string>();
            }

            var result = new List<string>();
            foreach (var company in _companyRelationship[companyId])
            {
                result.Add(company);
                result.AddRange(GetChildCompanies(company));
            }

            return result;
        }        
        
        private static Dictionary<string, string[]> BuildLandOwnershipByCompany(IEnumerable<LandOwnership> landOwnership)
        {
            return landOwnership.GroupBy(lo => lo.CompanyId, lo => lo.LandId)
                .ToDictionary(x => x.Key, x => x.ToArray());
        }

        private static Dictionary<string, string[]> BuildCompanyRelationships(IEnumerable<CompanyRelationship> companyRelationship)
        {
            return companyRelationship.Where(cr => cr.ParentId != null)
                .GroupBy(cr => cr.ParentId, cr => cr.CompanyId)
                .ToDictionary(x => x.Key, x => x.ToArray());
        }
    }

    public class OwnershipResult
    {
        public int OwnedDirectly { get; init; }
        public int OwnedIndirectly { get; init; }
        public int OwnedTotal => OwnedDirectly + OwnedIndirectly;
    }
}