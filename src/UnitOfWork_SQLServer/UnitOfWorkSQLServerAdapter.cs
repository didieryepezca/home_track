using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitOfWork_Interface;

namespace UnitOfWork_SQLServer
{
    public class UnitOfWorkSQLServerAdapter : IUnitOfWorkAdapter
    {
        private SqlConnection _context { get; set; }

        private SqlTransaction _transaction { get; set; }

        public IUnitOfWorkRepository Repositories { get; set; }

        public UnitOfWorkSQLServerAdapter(string connectionString)
        {
            _context = new SqlConnection(connectionString);
            _context.Open();

            _transaction = _context.BeginTransaction();

            Repositories = new UnitOfWorkSQLServerRepository(_context, _transaction);
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
