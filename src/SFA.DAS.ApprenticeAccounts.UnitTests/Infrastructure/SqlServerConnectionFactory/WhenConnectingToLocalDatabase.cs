using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.Infrastructure;

namespace SFA.DAS.ApprenticeAccounts.UnitTests.Infrastructure.SqlServerConnectionFactory
{
    public class WhenConnectingToLocalDatabase
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
            _dbContextOptionsBuilder = new DbContextOptionsBuilder<ApprenticeAccountsDbContext>();
            _managedIdentityTokenProviderMock.Setup(x => x.GetSqlAccessTokenAsync()).ReturnsAsync("TOKEN");

            _sut = new ApprenticeAccounts.Infrastructure.SqlServerConnectionFactory(_configurationMock.Object, _managedIdentityTokenProviderMock.Object);
            _connectionString = "Data Source=(localdb);Initial Catalog=DummyDatabase;Integrated Security=True";
        }

        [TestCase("LOCAL")]
        [TestCase("ACCEPTANCE_TESTS")]
        [TestCase("DEV")]
        public void Then_CreateConnection_should_accept_a_local_connection_string(string environmentName)
        {
            _configurationMock.Setup(x => x[It.IsAny<string>()]).Returns(environmentName);

            var dbConnection = _sut.CreateConnection(_connectionString);
            dbConnection.Should().NotBeNull();
            _managedIdentityTokenProviderMock.Verify(x=>x.GetSqlAccessTokenAsync(), Times.Never);
        }

        [TestCase("LOCAL")]
        [TestCase("ACCEPTANCE_TESTS")]
        [TestCase("DEV")]
        public void Then_AddConnection_should_add_sqlConnection_to_builder(string environmentName)
        {
            _configurationMock.Setup(x => x[It.IsAny<string>()]).Returns(environmentName);

            var result = _sut.AddConnection(_dbContextOptionsBuilder, _connectionString);
            result.Should().Be(_dbContextOptionsBuilder);
            _managedIdentityTokenProviderMock.Verify(x => x.GetSqlAccessTokenAsync(), Times.Never);
        }

        [Test]
        public void Then_AddConnection_should_attach_to_existing_Connection_and_return_builder()
        {
            var existingConnection = new SqlConnection(_connectionString);

            var result = _sut.AddConnection(_dbContextOptionsBuilder, existingConnection);
            result.Should().Be(_dbContextOptionsBuilder);
            _managedIdentityTokenProviderMock.Verify(x => x.GetSqlAccessTokenAsync(), Times.Never);
        }
    }
}
