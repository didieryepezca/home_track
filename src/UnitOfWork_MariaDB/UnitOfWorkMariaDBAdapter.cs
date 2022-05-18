using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitOfWork_Interface;

namespace UnitOfWork_MariaDB
{
    public class UnitOfWorkMariaDBAdapter : IUnitOfWorkAdapter
    {
        private MySqlConnection _context { get; set; }

        private MySqlTransaction _transaction { get; set; }

        public IUnitOfWorkRepository? Repositories { get; set; }

        public UnitOfWorkMariaDBAdapter(string connectionString)
        {
            _context = new MySqlConnection(connectionString);
            _context.Open();

            _transaction = _context.BeginTransaction();

            Repositories = new UnitOfWorkMariaDBRepository(_context, _transaction);
        }

        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
            }

            if (_context != null)
            {
                _context.Close();
                _context.Dispose();
            }

            Repositories = null;
        }

        public void SaveChanges()
        {
            _transaction.Commit();
        }

    }
}
