using DapperASPNetCore.Contracts;
using DapperASPNetCore.Controllers;
using DapperASPNetCore.Dto;
using DapperASPNetCore.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.ControllerTests
{
    public class CompaniesControllerTests
    {
        private readonly Mock<ICompanyRepository> _companyRepositoryMock;
        private readonly CompaniesController _companiesController;

        public CompaniesControllerTests()
        {
            _companyRepositoryMock = new Mock<ICompanyRepository>();
            _companiesController = new CompaniesController(_companyRepositoryMock.Object);
        }

        [Fact]
        public async Task Index_should_return_list_of_companies()
        {
            //Arrange
            var companies = new List<Company> { new Company { Id = 1, Name = "Rimi" } }.AsEnumerable();
            _companyRepositoryMock.Setup(r => r.GetCompanies())
                                  .ReturnsAsync(companies);
            //Act
            var result = await _companiesController.GetCompanies() as ObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(result.Value, companies);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task Index_should_return_status_code_500()
        {
            //Arrange
            _companyRepositoryMock.Setup(r => r.GetCompanies())
                                  .Throws(new Exception());
            //Act
            var result = await _companiesController.GetCompanies() as ObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async Task GetCompaniesEmployeesMultipleMapping_should_return_list_of_companies()
        {
            //Arrange
            var companies = new List<Company> { new Company { Id = 1, Name = "Rimi" } };
            _companyRepositoryMock.Setup(r => r.GetCompaniesEmployeesMultipleMapping())
                                  .ReturnsAsync(companies);
            //Act
            var result = await _companiesController.GetCompaniesEmployeesMultipleMapping() as ObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(companies, result.Value);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task GetCompaniesEmployeesMultipleMapping_should_return_status_code_500()
        {
            //Arrange
            _companyRepositoryMock.Setup(r => r.GetCompaniesEmployeesMultipleMapping())
                                  .Throws(new Exception());
            //Act
            var result = await _companiesController.GetCompaniesEmployeesMultipleMapping() as ObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async Task GetCompanyForEmployee_should_return_company()
        {
            //Arrange
            int id = 1;
            var company = new Company { Id = id, Name = "Rimi" };
            _companyRepositoryMock.Setup(r => r.GetCompanyByEmployeeId(id))
                                  .ReturnsAsync(company);
            //Act
            var result = await _companiesController.GetCompanyForEmployee(id) as ObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(company, result.Value);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task GetCompanyForEmployee_should_return_status_code_500()
        {
            //Arrange
            _companyRepositoryMock.Setup(r => r.GetCompanyByEmployeeId(It.IsAny<int>()))
                                  .Throws(new Exception());
            //Act
            var result = await _companiesController.GetCompanyForEmployee(1) as ObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async Task GetCompanyEmployeesMultipleResult_should_return_company()
        {
            //Arrange
            int id = 1;
            var company = new Company { Id = id, Name = "Rimi" };
            _companyRepositoryMock.Setup(r => r.GetCompanyEmployeesMultipleResults(id))
                                  .ReturnsAsync(company);
            //Act
            var result = await _companiesController.GetCompanyEmployeesMultipleResult(id) as ObjectResult;
            //Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(company, result.Value);
        }

        [Fact]
        public async Task GetCompanyEmployeesMultipleResult_should_return_not_found()
        {
            //Arrange
            var nullCompany = (Company)null;
            _companyRepositoryMock.Setup(r => r.GetCompanyEmployeesMultipleResults(It.IsAny<int>()))
                                  .ReturnsAsync(nullCompany);
            //Act
            var result = await _companiesController.GetCompanyEmployeesMultipleResult(1) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetCompanyEmployeesMultipleResult_should_return_status_code_500()
        {
            //Arrange
            _companyRepositoryMock.Setup(r => r.GetCompanyEmployeesMultipleResults(It.IsAny<int>()))
                                  .Throws(new Exception());
            //Act
            var result = await _companiesController.GetCompanyEmployeesMultipleResult(1) as ObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async Task GetCompany_should_return_company()
        {
            //Arrange
            int id = 1;
            var company = new Company { Id = id, Name = "Rimi" };
            _companyRepositoryMock.Setup(r => r.GetCompany(id))
                                  .ReturnsAsync(company);
            //Act
            var result = await _companiesController.GetCompany(id) as ObjectResult;
            //Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(company, result.Value);
        }

        [Fact]
        public async Task GetCompany_should_return_not_found()
        {
            //Arrange
            var nullCompany = (Company)null;
            _companyRepositoryMock.Setup(r => r.GetCompany(It.IsAny<int>()))
                                  .ReturnsAsync(nullCompany);
            //Act
            var result = await _companiesController.GetCompany(1) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetCompany_should_return_status_code_500()
        {
            //Arrange
            _companyRepositoryMock.Setup(r => r.GetCompany(It.IsAny<int>()))
                                  .Throws(new Exception());
            //Act
            var result = await _companiesController.GetCompany(1) as ObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async Task CreateCompany_should_return_status_code_201()
        {
            //Arrange
            string routeName = "CompanyById";
            int id = 1;
            var company = new Company { Id = id, Name = "Rimi" };
            _companyRepositoryMock.Setup(r => r.CreateCompany(It.IsAny<CompanyForCreationDto>()))
                                  .ReturnsAsync(company);
            //Act
            var result = await _companiesController.CreateCompany(new CompanyForCreationDto()) as CreatedAtRouteResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(routeName, result.RouteName);
            Assert.Equal(company, result.Value);
        }

        [Fact]
        public async Task CreateCompany_should_return_status_code_500()
        {
            //Arrange
            _companyRepositoryMock.Setup(r => r.CreateCompany(It.IsAny<CompanyForCreationDto>()))
                                  .Throws(new Exception());
            //Act
            var result = await _companiesController.CreateCompany(new CompanyForCreationDto()) as ObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async Task CreateMultipleCompanies_should_return_status_code_201()
        {
            //Arrange
            _companyRepositoryMock.Setup(r => r.CreateMultipleCompanies(It.IsAny<List<CompanyForCreationDto>>()))
                                  .Verifiable();
            //Act
            var result = await _companiesController.CreateMultipleCompanies(new List<CompanyForCreationDto>()) as CreatedResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CreateMultipleCompanies_should_return_status_code_500()
        {
            //Arrange
            _companyRepositoryMock.Setup(r => r.CreateMultipleCompanies(It.IsAny<List<CompanyForCreationDto>>()))
                                  .Throws(new Exception());
            //Act
            var result = await _companiesController.CreateMultipleCompanies(new List<CompanyForCreationDto>()) as ObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async Task UpdateCompany_should_update_company()
        {
            //Arrange
            int id = 1;
            var company = new Company { Id = id, Name = "Rimi" };
            _companyRepositoryMock.Setup(r => r.GetCompany(id))
                      .ReturnsAsync(company);
            _companyRepositoryMock.Setup(r => r.UpdateCompany(id, It.IsAny<CompanyForUpdateDto>()))
                                  .Verifiable();

            //Act
            var result = await _companiesController.UpdateCompany(id, new CompanyForUpdateDto()) as NoContentResult;

            //Assert
            Assert.NotNull(result);
            _companyRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task UpdateCompany_should_return_not_found()
        {
            //Arrange
            var company = (Company)null;
            _companyRepositoryMock.Setup(r => r.GetCompany(It.IsAny<int>()))
                      .ReturnsAsync(company);

            //Act
            var result = await _companiesController.UpdateCompany(1, new CompanyForUpdateDto()) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteCompany_should_delete_company()
        {
            //Arrange
            int id = 1;
            var company = new Company { Id = id, Name = "Rimi" };
            _companyRepositoryMock.Setup(r => r.GetCompany(id))
                      .ReturnsAsync(company);
            _companyRepositoryMock.Setup(r => r.DeleteCompany(id))
                                  .Verifiable();

            //Act
            var result = await _companiesController.DeleteCompany(id) as NoContentResult;

            //Assert
            Assert.NotNull(result);
            _companyRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task DeleteCompany_should_return_not_found()
        {
            //Arrange
            var company = (Company)null;
            _companyRepositoryMock.Setup(r => r.GetCompany(It.IsAny<int>()))
                      .ReturnsAsync(company);

            //Act
            var result = await _companiesController.DeleteCompany(1) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }
    }
}
