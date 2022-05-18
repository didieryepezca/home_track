using System.Data.SqlClient;

namespace Repository_SQLServer
{
    public abstract class Repository
    {
        protected SqlConnection _context;
        protected SqlTransaction _transaction;

        protected SqlCommand CreateCommand(string query)
        {
            return new SqlCommand(query, _context, _transaction);
        }

        protected SqlBulkCopy CreateSqlBulkCopy()
        {
            return new SqlBulkCopy(_context, SqlBulkCopyOptions.Default, _transaction);
        }
    }
}