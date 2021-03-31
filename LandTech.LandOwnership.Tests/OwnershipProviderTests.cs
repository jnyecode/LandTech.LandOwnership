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
            Assert.Equal(0, result.OwnedTotal);
        }

        [Fact]
        public void WhenRetrievingACompanyWithOnePlot_ReturnOwnership()
        {
            var companyId = _fixture.Create<string>();
            var ownershipProvider = new OwnershipProvider(new[] {new CompanyRelationship {CompanyId = companyId}},
                new[] {new LandOwnership {CompanyId = companyId, LandId = _fixture.Create<string>()}});
            
            var result = ownershipProvider.OwnershipFor(companyId);

            Assert.Equal(1, result.OwnedDirectly);
            Assert.Equal(1, result.OwnedTotal);
        }

        [Fact]
        public void WhenRetrievingACompanyWithChildCompanies_ReturnsDirectAndIndirectOwnership()
        {
            //Arrange
            var companyId = _fixture.Create<string>();
            var parentCompanyId = _fixture.Create<string>();
            var ownershipProvider = new OwnershipProvider(
                new[]
                {
                    new CompanyRelationship {CompanyId = companyId, ParentId = parentCompanyId}
                },
                new[]
                {
                    new LandOwnership {CompanyId = companyId, LandId = _fixture.Create<string>()},
                    new LandOwnership {CompanyId = parentCompanyId, LandId = _fixture.Create<string>()}
                });
            
            //Act
            var result = ownershipProvider.OwnershipFor(parentCompanyId);

            //Assert
            Assert.Equal(1, result.OwnedDirectly);
            Assert.Equal(1, result.OwnedIndirectly);
            Assert.Equal(2, result.OwnedTotal);
        }

        [Fact]
        public void WhenRetrievingACompanyWithMultiLevelChildCompanies_ReturnsOwnership()
        {
            //Arrange
            var companyId = _fixture.Create<string>();
            var parentCompanyId = _fixture.Create<string>();
            var grandParentCompanyId = _fixture.Create<string>();
            
            var ownershipProvider = new OwnershipProvider(
                new[]
                {
                    new CompanyRelationship {CompanyId = companyId, ParentId = parentCompanyId},
                    new CompanyRelationship {CompanyId = parentCompanyId, ParentId = grandParentCompanyId}
                },
                new[]
                {
                    new LandOwnership {CompanyId = companyId, LandId = _fixture.Create<string>()},
                    new LandOwnership {CompanyId = parentCompanyId, LandId = _fixture.Create<string>()},
                    new LandOwnership {CompanyId = grandParentCompanyId, LandId = _fixture.Create<string>()}
                });
            
            //Act
            var result = ownershipProvider.OwnershipFor(grandParentCompanyId);

            //Assert
            Assert.Equal(1, result.OwnedDirectly);
            Assert.Equal(2, result.OwnedIndirectly);
            Assert.Equal(3, result.OwnedTotal);
        }          
    }
}