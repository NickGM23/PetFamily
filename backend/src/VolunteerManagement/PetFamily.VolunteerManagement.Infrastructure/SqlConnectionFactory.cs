using Microsoft.Extensions.Configuration;
using Npgsql;
using PetFamily.Core.Database;
using System.Data;

namespace PetFamily.VolunteerManagement.Infrastructure
{
    internal class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public SqlConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection Create() =>
            new NpgsqlConnection(_configuration.GetConnectionString("Database"));
    }
}
