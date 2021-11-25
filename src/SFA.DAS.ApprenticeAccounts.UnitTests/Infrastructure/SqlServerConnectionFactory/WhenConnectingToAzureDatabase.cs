using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.Infrastructure;

namespace SFA.DAS.ApprenticeAccounts.UnitTests.Infrastructure.SqlServerConnectionFactory
{
    public class WhenConnectingToAzureDatabase
    {
        private ApprenticeAccounts.Infrastructure.SqlServerConnectionFactory _sut;
        private Mock<IConfiguration> _configurationMock;
        private Mock<IManagedIdentityTokenProvider> _managedIdentityTokenProviderMock;
        private DbContextOptionsBuilder<ApprenticeAccountsDbContext> _dbContextOptionsBuilder;
        private string _connectionString;

        [SetUp]
        public void Arrange()
        {
            _configurationMock = new Mock<IConfiguration>();
            _managedIdentityTokenProviderMock = new Mock<IManagedIdentityTokenProvider>();
            _managedIdentityTokenProviderMock.Setup(x => x.GetSqlAccessTokenAsync()).ReturnsAsync("TOKEN");
            _dbContextOptionsBuilder = new DbContextOptionsBuilder<ApprenticeAccountsDbContext>();

            _sut = new ApprenticeAccounts.Infrastructure.SqlServerConnectionFactory(_configurationMock.Object, _managedIdentityTokenProviderMock.Object);
            _connectionString = "Data Source=someserver;Initial Catalog=DummyDatabase;Integrated Security=False";
        }

        [Test]
        public void Then_CreateConnection_should_accept_a_connection_string()
        {
            _configurationMock.Setup(x => x[It.IsAny<string>()]).Returns("PROD");

            var dbConnection = _sut.CreateConnection(_connectionString);
            dbConnection.Should().NotBeNull();
            _managedIdentityTokenProviderMock.Verify(x=>x.GetSqlAccessTokenAsync());
        }

        [Test]
        public void Then_AddConnection_should_add_sqlConnection_to_builder()
        {
            _configurationMock.Setup(x => x[It.IsAny<string>()]).Returns("PROD");

            var result = _sut.AddConnection(_dbContextOptionsBuilder, _connectionString);
            result.Should().Be(_dbContextOptionsBuilder);
            _managedIdentityTokenProviderMock.Verify(x => x.GetSqlAccessTokenAsync());
        }
    }
}
