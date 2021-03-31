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
            var companyRelationship = ImportCompanyRelationships();
            var landOwnership = ImportLandOwnerships();

            Console.WriteLine("Please enter the Company Id you wish to retrieve ownership data for:");
            var companyId = Console.ReadLine();
            Console.WriteLine();
            
            var ownershipProvider = new OwnershipProvider(companyRelationship, landOwnership);
            var result = ownershipProvider.OwnershipFor(companyId);

            Console.WriteLine($"Ownership data for company: {companyId}");
            Console.WriteLine($"---------------------------------------");
            Console.WriteLine($"Land owned directly: {result.OwnedDirectly}");
            Console.WriteLine($"Land owned indirectly: {result.OwnedIndirectly}");
            Console.WriteLine($"Total land owned: {result.OwnedTotal}");
            
            Console.ReadLine();
        }

        private static IEnumerable<LandOwnership> ImportLandOwnerships()
        {
            using var reader = new StreamReader("Data/land_ownership.csv");
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            return csv.GetRecords<LandOwnership>().ToList();
        }

        private static IEnumerable<CompanyRelationship> ImportCompanyRelationships()
        {
            using var reader = new StreamReader("Data/company_relations.csv");
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            return csv.GetRecords<CompanyRelationship>().ToList();
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