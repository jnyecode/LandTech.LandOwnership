using System.Collections.Generic;
using System.Linq;

namespace LandTech.LandOwnership
{
    public class OwnershipProvider
    {
        private readonly IEnumerable<CompanyRelationship> _companyRelationship;
        private readonly IEnumerable<LandOwnership> _landOwnership;

        public OwnershipProvider(IEnumerable<CompanyRelationship> companyRelationship, IEnumerable<LandOwnership> landOwnership)
        {
            _companyRelationship = companyRelationship;
            _landOwnership = landOwnership;
        }

        public OwnershipResult OwnershipFor(string companyId)
        {
            var directlyOwned = _landOwnership.Count(lo => lo.CompanyId == companyId);
            var companiesInChain = GetChildCompanies(companyId);
            var indirectlyOwned = _landOwnership.Count(lo => companiesInChain.Contains(lo.CompanyId));

            return new OwnershipResult {OwnedDirectly = directlyOwned, OwnedIndirectly = indirectlyOwned };
        }
        
        private IEnumerable<string> GetChildCompanies(string companyId)
        {
            if (!_companyRelationship.Any(cr => cr.ParentId == companyId))
            {
                return Enumerable.Empty<string>();
            }

            var result = new List<string>();
            foreach (var company in _companyRelationship.Where(cr => cr.ParentId == companyId))
            {
                result.Add(company.CompanyId);
                result.AddRange(GetChildCompanies(company.CompanyId));
            }

            return result;
        }        
    }

    public class OwnershipResult
    {
        public int OwnedDirectly { get; init; }
        public int OwnedIndirectly { get; init; }
        public int OwnedTotal => OwnedDirectly + OwnedIndirectly;
    }
}