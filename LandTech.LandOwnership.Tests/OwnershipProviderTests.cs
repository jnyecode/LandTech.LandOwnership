using System;
using System.Linq;
using AutoFixture;
using Xunit;

namespace LandTech.LandOwnership.Tests
{
    public class OwnershipProviderTests
    {
        private Fixture _fixture;

        public OwnershipProviderTests()
        {
            _fixture = new Fixture();
        }
        
        [Fact]
        public void WhenRetrievingANonExistingCompanyId_ReturnsEmptyResult()
        {
            //Arrange
            var ownershipProvider = new OwnershipProvider(
                Enumerable.Empty<CompanyRelationship>(),
                Enumerable.Empty<LandOwnership>());
            var companyId = _fixture.Create<string>();

            //Act
            var result = ownershipProvider.OwnershipFor(companyId);

            //Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void WhenRetrievingACompanyWithOnePlot_ReturnOwnership()
        {
            var companyId = _fixture.Create<string>();
            var ownershipProvider = new OwnershipProvider(new[] {new CompanyRelationship {CompanyId = companyId}},
                new[] {new LandOwnership {CompanyId = companyId, LandId = _fixture.Create<string>()}});
            
            var result = ownershipProvider.OwnershipFor(companyId);

            Assert.Equal(1, result);
            
        }
    }
}