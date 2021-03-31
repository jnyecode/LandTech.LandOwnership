using System.Collections.Generic;

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

        public int OwnershipFor(string companyId)
        {
            return 0;
        }
    }
}