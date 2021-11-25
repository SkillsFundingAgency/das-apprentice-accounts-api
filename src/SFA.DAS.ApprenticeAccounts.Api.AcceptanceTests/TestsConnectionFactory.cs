using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.Infrastructure;
using System;
using System.Data.Common;

namespace SFA.DAS.ApprenticeAccounts.Api.AcceptanceTests
{
    public interface ITestsDbConnectionFactory : IConnectionFactory
    {
        string ConnectionString { get; }

        void EnsureCreated(ApprenticeAccountsDbContext dbContext);

        void EnsureDeleted(ApprenticeAccountsDbContext dbContext);
    }

    public class TestsDbConnectionFactory : IConnectionFactory
    {
        private static ITestsDbConnectionFactory factory;
        public static string ConnectionString => factory.ConnectionString;

        static TestsDbConnectionFactory()
        {
            //*
            factory = new SqliteTestsConnectionFactory();
            /*/
            factory = new SqlServerTestsConnectionFactory();
            //*/
        }

        internal void EnsureCreated(ApprenticeAccountsDbContext dbContext)
            => factory.EnsureCreated(dbContext);

        internal void EnsureDeleted(ApprenticeAccountsDbContext dbContext)
            => factory.EnsureDeleted(dbContext);

        public DbContextOptionsBuilder<TContext> AddConnection<TContext>(
            DbContextOptionsBuilder<TContext> builder) where TContext : DbContext
        {
            return factory.AddConnection(builder, ConnectionString);
        }

        DbContextOptionsBuilder<TContext> IConnectionFactory.AddConnection<TContext>(
            DbContextOptionsBuilder<TContext> builder, string connection)
        {
            return factory.AddConnection(builder, connection);
        }

        DbContextOptionsBuilder<TContext> IConnectionFactory.AddConnection<TContext>(
            DbContextOptionsBuilder<TContext> builder, DbConnection connection)
        {
            return factory.AddConnection(builder, connection);
        }

        DbConnection IConnectionFactory.CreateConnection(string connection)
        {
            return factory.CreateConnection(connection);
        }
    }

    internal class SqliteTestsConnectionFactory : IConnectionFactory, ITestsDbConnectionFactory
    {
        public string ConnectionString => $"Data Source={AcceptanceTestsData.AcceptanceTestsDatabaseName}";

        public void EnsureCreated(ApprenticeAccountsDbContext dbContext)
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }

        public void EnsureDeleted(ApprenticeAccountsDbContext dbContext)
        {
            dbContext.Database.EnsureDeleted();
        }

        DbContextOptionsBuilder<TContext> IConnectionFactory.AddConnection<TContext>(
            DbContextOptionsBuilder<TContext> builder, string connection)
        {
            return builder.UseSqlite(connection);
        }

        DbContextOptionsBuilder<TContext> IConnectionFactory.AddConnection<TContext>(
            DbContextOptionsBuilder<TContext> builder, DbConnection connection)
        {
            return builder.UseSqlite(connection);
        }

        DbConnection IConnectionFactory.CreateConnection(string connection)
        {
            return new SqliteConnection(connection);
        }
    }

    internal class SqlServerTestsConnectionFactory : IConnectionFactory, ITestsDbConnectionFactory
    {
        public string ConnectionString => "Data Source=.;Initial Catalog=SFA.DAS.ApprenticeAccounts.AcceptanceTests;Integrated Security=True;";

        public void EnsureCreated(ApprenticeAccountsDbContext dbContext)
        {
            if (!dbContext.Database.CanConnect())
            {
                throw new Exception(
@"SQL Server Acceptance Test database not found.

To ensure that we are testing against the real database schema used in production, do not rely on EFCore to create the database.

Instead, manually deploy the database using the `SFA.DAS.ApprenticeAccounts.Database` project, targetting `SFA.DAS.ApprenticeAccounts.AcceptanceTests`");
            }

            dbContext.Database.ExecuteSqlRaw("truncate table Revision");
            dbContext.Database.ExecuteSqlRaw("delete from Apprenticeship");
            dbContext.Database.ExecuteSqlRaw("truncate table ApprenticeEmailAddressHistory");
            dbContext.Database.ExecuteSqlRaw("truncate table Registration");
            dbContext.Database.ExecuteSqlRaw("delete from Apprentice");
        }

        public void EnsureDeleted(ApprenticeAccountsDbContext dbContext)
        {
        }

        DbContextOptionsBuilder<TContext> IConnectionFactory.AddConnection<TContext>(
            DbContextOptionsBuilder<TContext> builder, string connection)
        {
            return builder.UseSqlServer(connection);
        }

        DbContextOptionsBuilder<TContext> IConnectionFactory.AddConnection<TContext>(
            DbContextOptionsBuilder<TContext> builder, DbConnection connection)
        {
            return builder.UseSqlServer(connection);
        }

        DbConnection IConnectionFactory.CreateConnection(string connection)
        {
            return new SqliteConnection(connection);
        }
    }
}