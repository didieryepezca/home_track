using Microsoft.Extensions.Configuration;
using UnitOfWork_Interface;

namespace UnitOfWork_MariaDB
{
    public class UnitOfWorkMariaDB : IUnitOfWork
    {
        public UnitOfWorkMariaDB(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private readonly IConfiguration _configuration;

        public IUnitOfWorkAdapter Create()
        {
            var connectionString = _configuration.GetConnectionString("MariaDBConnection");

            return new UnitOfWorkMariaDBAdapter(connectionString);
        }

    }
}