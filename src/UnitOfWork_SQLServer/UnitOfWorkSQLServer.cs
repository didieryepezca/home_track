using Microsoft.Extensions.Configuration;
using UnitOfWork_Interface;

namespace UnitOfWork_SQLServer
{
    public class UnitOfWorkSQLServer : IUnitOfWork
    {
        public UnitOfWorkSQLServer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private readonly IConfiguration _configuration;

        public IUnitOfWorkAdapter Create()
        {
            var connectionString = _configuration.GetConnectionString("SQLServerConnection");

            return new UnitOfWorkSQLServerAdapter(connectionString);
        }

    }
}