using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration.Attributes;

namespace LandTech.LandOwnership
{
    class Program
    {
        static void Main(string[] args)
        {
            //TODO

            // import Company Relationship
            IEnumerable<CompanyRelationship> companyRelationship;
            using (var reader = new StreamReader("Data/company_relations.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                companyRelationship = csv.GetRecords<CompanyRelationship>().ToList();
                Console.WriteLine($"Company Relationships: {companyRelationship.Count()}");
            }

            // import Land Ownership
            IEnumerable<LandOwnership> landOwnership;
            using (var reader = new StreamReader("Data/land_ownership.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                landOwnership = csv.GetRecords<LandOwnership>().ToList();
                Console.WriteLine($"Land Ownerships: {landOwnership.Count()}");
            }
            
            // landownership = lo ctor(customer rel, land ownership)
            var ownershipProvider = new OwnershipProvider(companyRelationship, landOwnership);
            // landownership.LandFor(companyId);

            Console.ReadLine();
        }
    }

    public class LandOwnership
    {
        [Index(0)]
        public string LandId { get; init; }
        [Index(1)]
        public string CompanyId  { get; init; }
    }

    public class CompanyRelationship
    {
        [Index(0)]
        public string CompanyId { get; init; }
        [Index(1)]
        public string Name { get; init; }
        [Index(2)]
        public string ParentId { get; init; }
    }        
    
}