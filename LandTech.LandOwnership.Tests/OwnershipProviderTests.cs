using System;
using System.Linq;
using AutoFixture;
using Xunit;

namespace LandTech.LandOwnership.Tests
{
    public class OwnershipProviderTests
    {
        [Fact]
        public void WhenRetrievingANonExistingCompanyId_ReturnsEmptyResult()
        {
            //Arrange
            var fixture = new Fixture();
            var ownershipProvider = new OwnershipProvider(
                Enumerable.Empty<CompanyRelationship>(),
                Enumerable.Empty<LandOwnership>());
            var companyId = fixture.Create<string>();
            
            
            //Act
            var result = ownershipProvider.OwnershipFor(companyId);

            //Assert
            Assert.Equal(0, result);
        }
    }
}