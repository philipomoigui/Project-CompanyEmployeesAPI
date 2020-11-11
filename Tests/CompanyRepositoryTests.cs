using Contracts;
using Entities.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class CompanyRepositoryTests
    {
        [Fact]
        public void GetAllCompaniesAsync_ReturnListOfCompanies_WithASingleCompany()
        {
            //Arrange
            var mockRepo = new Mock<ICompanyRepository>();
            mockRepo.Setup(repo => repo.GetAllCompaniesAsync(false))
                .Returns(Task.FromResult(GetCompanies()));

            //Act
            var result = mockRepo.Object.GetAllCompaniesAsync(false)
                .GetAwaiter()
                .GetResult();

            //Assert
            Assert.IsType<List<Company>>(result);
            Assert.Single(result);
        }

        public IEnumerable<Company> GetCompanies()
        {
            return new List<Company>
            {
                new Company
                {
                    Id = Guid.NewGuid(),
                    Name = "John Bull",
                    Address = "21, oak street",
                    Country = "Ng"
                }
            };
        }
    }
}

