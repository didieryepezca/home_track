using MySqlConnector;

namespace Repository_MariaDB
{
    public abstract class Repository
    {
        protected MySqlConnection _context;
        protected MySqlTransaction _transaction;

        protected MySqlCommand CreateCommand(string query)
        {
            return new MySqlCommand(query, _context, _transaction);
        }

        //protected MySqlBulkCopy CreateSqlBulkCopy()
        //{
        //    return new MySqlBulkCopy(_context, MySqlBulkCopyOptions.Default, _transaction);
        //}
    }
}