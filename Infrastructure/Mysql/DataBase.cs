using MySql.Data.MySqlClient;
using ManejoInventario.Config;

namespace ManejoInventario.Data
{
    public class DataBase : IDisposable
    {
        private MySqlConnection _connection;
        private bool _disposed = false;

        public DataBase()
        {
            _connection = new MySqlConnection(AppSettings.ConnectionString);
        }

        public MySqlConnection Connection
        {
            get
            {
                if (_connection.State != System.Data.ConnectionState.Open)
                {
                    _connection.Open();
                }
                return _connection;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_connection != null && _connection.State == System.Data.ConnectionState.Open)
                    {
                        _connection.Close();
                        _connection.Dispose();
                        _connection = null!;
                    }
                }
                _disposed = true;
            }
        }

        ~DataBase()
        {
            Dispose(false);
        }
    }
}