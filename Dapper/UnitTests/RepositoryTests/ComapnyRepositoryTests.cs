using DapperASPNetCore;
using DapperASPNetCore.Contracts;
using DapperASPNetCore.Data;
using DapperASPNetCore.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Xunit;
using DapperASPNetCore.Entities;
using System.Data.SqlClient;
using Moq.Dapper;
using Dapper;
using DapperASPNetCore.Dto;

namespace UnitTests.RepositoryTests
{
    public class ComapnyRepositoryTests
    {
        private readonly Mock<IDapperWrapper> _mockDapper;
        private readonly Mock<IDapperContext> _dapperContextMock;
        private readonly CompanyRepository _companyRepository;

        public ComapnyRepositoryTests()
        {
            _mockDapper = new Mock<IDapperWrapper>();
            _dapperContextMock = new Mock<IDapperContext>();

            _companyRepository = new CompanyRepository(_dapperContextMock.Object, _mockDapper.Object);

        }

        [Fact]
        public async Task GetCompanies_ReturnsCompanies()
        {
            //Arrange
            var query = "SELECT * FROM Companies";

            var expectedCompanies = new List<Company>() { new Company() { Id = 1, Name = "Google" } };

            var connection = new Mock<IDbConnection>();

            connection.SetupDapperAsync(c => c.QueryAsync<Company>(It.IsAny<string>(), null, null, null, null)).ReturnsAsync(expectedCompanies);

            //Act
            var actual = (await connection.Object.QueryAsync<Company>(query, null, null, null, null)).ToList();

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedCompanies.Count, actual.Count);
            Assert.IsType<List<Company>>(actual);
        }

        [Fact]
        public async Task GetCompanies_ReturnsCompanies2()
        {
            //Arrange
            var query = "SELECT * FROM Companies";

            var expectedCompanies = new List<Company>() { new Company() { Id = 1, Name = "Google" } };

            var connection = new Mock<IDbConnection>();

            _mockDapper.Setup(t => t.QueryAsync<Company>(It.IsAny<IDbConnection>(), query))
                .ReturnsAsync(() => expectedCompanies);

            //Act
            var result = await _companyRepository.GetCompanies();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<Company>>(result);
        }

        [Fact]
        public async Task GetCompany_ReturnsCompany()
        {
            //Arrange
            var query = "SELECT * FROM Companies WHERE Id = @Id";
            int id = 1;
            object obj = new { id };
            var expectedCompany = new Company(){ Id = id, Name = "Google" };

            _mockDapper.Setup(t => t.QuerySingleOrDefaultAsync<Company>(It.IsAny<IDbConnection>(), 
                        query, 
                        It.Is<object>(m => (int)m.GetType().GetProperty("id").GetValue(m) == id)))
                        .ReturnsAsync(() => expectedCompany);

            //Act
            var result = await _companyRepository.GetCompany(id);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Company>(result);
        }

        [Fact]
        public async Task CreateCompany_CreatesAndReturnsCreatedCompanyWithCorrectIdAndValues()
        {
            //Arrange
            var query = "INSERT INTO Companies (Name, Address, Country) VALUES (@Name, @Address, @Country)" +
                        "SELECT CAST(SCOPE_IDENTITY() as int)";
            int id = 1;

            var newCompany = new CompanyForCreationDto() { Name = "Selver", Address = "Torupilli", Country = "Estonia" };

            var parameters = new DynamicParameters();
            parameters.Add("Name", newCompany.Name, DbType.String);
            parameters.Add("Address", newCompany.Address, DbType.String);
            parameters.Add("Country", newCompany.Country, DbType.String);

            //Check if the dynamic parameters' values are equal to the dto values
            _mockDapper.Setup(t => t.QuerySingleAsync<int>(It.IsAny<IDbConnection>(), query, It.Is<DynamicParameters>(parameters => parameters.Get<string>("Name") == newCompany.Name 
              && parameters.Get<string>("Address") == newCompany.Address
              && parameters.Get<string>("Country") == newCompany.Country)))
                        .ReturnsAsync(() => id);

            var result = await _companyRepository.CreateCompany(newCompany);

            Assert.NotNull(result);
            Assert.IsType<Company>(result);
            Assert.True(result.Id == id);
        }

        [Fact]
        public async Task UpdateCompany_UpdatesCompany()
        {
            //Arrange
            var query = "UPDATE Companies SET Name = @Name, Address = @Address, Country = @Country WHERE Id = @Id";
            var companyDto = new CompanyForUpdateDto() {  Name = "Selver", Address = "Torupilli", Country = "Estonia" };
            var id = 1;
            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);
            parameters.Add("Name", companyDto.Name, DbType.String);
            parameters.Add("Address", companyDto.Address, DbType.String);
            parameters.Add("Country", companyDto.Country, DbType.String);

            _mockDapper.Setup(d => d.ExecuteAsync(It.IsAny<IDbConnection>(), query, It.Is<DynamicParameters>(parameters => parameters.Get<int>("Id") == id 
              && parameters.Get<string>("Name") == companyDto.Name
              && parameters.Get<string>("Address") == companyDto.Address
              && parameters.Get<string>("Country") == companyDto.Country)))
                        .Verifiable();

            //Act
            await _companyRepository.UpdateCompany(id, companyDto);

            //Assert
            _mockDapper.VerifyAll();
        }

        [Fact]
        public async Task DeleteCompany_DeletesCompany()
        {
            //Arrange
            var query = "DELETE FROM Companies WHERE Id = @Id";
            int id = 1;
            _mockDapper.Setup(d => d.ExecuteAsync(It.IsAny<IDbConnection>(), query, It.Is<object>(m => (int)m.GetType().GetProperty("id").GetValue(m) == id)))
                       .Verifiable();

            //Act
            await _companyRepository.DeleteCompany(id);

            //Assert
            _mockDapper.VerifyAll();
        }

        [Fact]
        public async Task GetCompanyByEmployeeId_ReturnCorrectCompany()
        {
            var procedureName = "ShowCompanyForProvidedEmployeeId";
            int employeeId = 1;
            int companyId = 5;
            var company = new Company { Id = companyId, Name = "Selver", Address = "Torupilli", Country = "Estonia" };
            var parameters = new DynamicParameters();
            parameters.Add("Id", employeeId, DbType.Int32, ParameterDirection.Input);

            _mockDapper.Setup(d => d.QueryFirstOrDefaultAsync<Company>(It.IsAny<IDbConnection>(), procedureName, It.Is<DynamicParameters>(parameters => parameters.Get<int>("Id") == employeeId), It.Is<CommandType>(command => command == CommandType.StoredProcedure)))
                       .ReturnsAsync(() => company)
                       .Verifiable();

            var result = await _companyRepository.GetCompanyByEmployeeId(employeeId);

            Assert.NotNull(result);
            Assert.IsType<Company>(result);
            _mockDapper.VerifyAll();
            
        }

        [Fact]
        public async Task GetCompanyEmployeesMultipleResults_ReturnsCompanyWithEmployees()
        {
            int companyId = 1;
            var company = new Company { Id = companyId, Name = "Selver", Address = "Torupilli", Country = "Estonia" };
            var employees = new List<Employee> { 
                new Employee { Id = 1, Age = 20, Name = "John Doe", CompanyId = companyId },
                new Employee {Id = 2, Age = 21, Name = "Mark Rober", CompanyId = companyId}
            };

            _mockDapper.Setup(d => d.ReadSingleOrDefaultAsync<Company>(It.IsAny<SqlMapper.GridReader>()))
                       .ReturnsAsync(() => company);
            _mockDapper.Setup(d => d.ReadAsync<Employee>(It.IsAny<SqlMapper.GridReader>()))
                       .ReturnsAsync(() => employees);

            //Act
            var result = await _companyRepository.GetCompanyEmployeesMultipleResults(companyId);

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Employees);
            Assert.Equal(employees, result.Employees);
                       
        }

        [Fact]
        public async Task GetCompaniesEmployeesMultipleMapping_ReturnsListOfCompaniesWithEmployees()
        {
            var query = "SELECT * FROM Companies c JOIN Employees e ON c.Id = e.CompanyId";
            int companyId = 1;
            var employees = new List<Employee> {
                new Employee { Id = 1, Age = 20, Name = "John Doe", CompanyId = companyId },
                new Employee {Id = 2, Age = 21, Name = "Mark Rober", CompanyId = companyId}
            };
            var company = new List<Company> { new Company { Id = companyId, Name = "Selver", Address = "Torupilli", Country = "Estonia", Employees = employees } }.AsEnumerable();

            _mockDapper.Setup(d => d.QueryAsync<Company, Employee, Company>(It.IsAny<IDbConnection>(), query, It.IsAny<Func<Company, Employee, Company>>())).ReturnsAsync(company);

            //Act
            var result = await _companyRepository.GetCompaniesEmployeesMultipleMapping();

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.FirstOrDefault());
            Assert.True(result.FirstOrDefault().Employees.Count > 0);

        }
    }
}
