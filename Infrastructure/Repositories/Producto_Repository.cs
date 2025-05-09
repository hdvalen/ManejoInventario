using MySql.Data.MySqlClient;
using ManejoInventario.Domain.Entities;
using ManejoInventario.Data;

namespace ManejoInventario.Repositories
{
    public class Producto_Repository : IRepository<Producto>
    {
        public async Task<IEnumerable<Producto>> GetAllAsync()
        {
            List<Producto> productos = new List<Producto>();
            
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand("SELECT * FROM producto", DataBase.Connection);
                using var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    productos.Add(new Producto
                    {
                        Id = reader["id"].ToString()!,
                        Nombre = reader["nombre"].ToString()!,
                        Stock = Convert.ToInt32(reader["stock"]),
                        StockMin = Convert.ToInt32(reader["stockMin"]),
                        StockMax = Convert.ToInt32(reader["stockMax"]),
                        Fecha_Creacion = Convert.ToDateTime(reader["fecha_creacion"]),
                        Fecha_Actualizacion = Convert.ToDateTime(reader["fecha_actualizacion"]),
                        Codigo_Barra = reader["codigo_barra"].ToString()!
                    });
                }
            }
            
            return productos;
        }

        public async Task<Producto?> GetByIdAsync(object id)
        {
            Producto? producto = null;
            
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand("SELECT * FROM producto WHERE id = @Id", DataBase.Connection);
                command.Parameters.AddWithValue("@Id", id);
                
                using var reader = await command.ExecuteReaderAsync();
                
                if (await reader.ReadAsync())
                {
                    producto = new Producto
                    {
                        Id = reader["id"].ToString()!,
                        Nombre = reader["nombre"].ToString()!,
                        Stock = Convert.ToInt32(reader["stock"]),
                        StockMin = Convert.ToInt32(reader["stockMin"]),
                        StockMax = Convert.ToInt32(reader["stockMax"]),
                        Fecha_Creacion = Convert.ToDateTime(reader["fecha_creacion"]),
                        Fecha_Actualizacion = Convert.ToDateTime(reader["fecha_actualizacion"]),
                        Codigo_Barra = reader["codigo_barra"].ToString()!
                    };
                }
            }
            
            return producto;
        }

        public async Task<bool> InsertAsync(Producto producto)
        {
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand(
                    "INSERT INTO producto (id, nombre, stock, stockMin, stockMax, fecha_creacion, fecha_actualizacion, codigo_barra) " +
                    "VALUES (@Id, @Nombre, @Stock, @StockMin, @StockMax, @FechaCreacion, @FechaActualizacion, @CodigoBarra)",
                    DataBase.Connection);
                
                command.Parameters.AddWithValue("@Id", producto.Id);
                command.Parameters.AddWithValue("@Nombre", producto.Nombre);
                command.Parameters.AddWithValue("@Stock", producto.Stock);
                command.Parameters.AddWithValue("@StockMin", producto.StockMin);
                command.Parameters.AddWithValue("@StockMax", producto.StockMax);
                command.Parameters.AddWithValue("@FechaCreacion", producto.Fecha_Creacion);
                command.Parameters.AddWithValue("@FechaActualizacion", producto.Fecha_Actualizacion);
                command.Parameters.AddWithValue("@CodigoBarra", producto.Codigo_Barra);
                
                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> UpdateAsync(Producto producto)
        {
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand(
                    "UPDATE producto SET nombre = @Nombre, stock = @Stock, stockMin = @StockMin, " +
                    "stockMax = @StockMax, fecha_actualizacion = @FechaActualizacion, codigo_barra = @CodigoBarra " +
                    "WHERE id = @Id",
                    DataBase.Connection);
                
                command.Parameters.AddWithValue("@Id", producto.Id);
                command.Parameters.AddWithValue("@Nombre", producto.Nombre);
                command.Parameters.AddWithValue("@Stock", producto.Stock);
                command.Parameters.AddWithValue("@StockMin", producto.StockMin);
                command.Parameters.AddWithValue("@StockMax", producto.StockMax);
                command.Parameters.AddWithValue("@FechaActualizacion", DateTime.Now);
                command.Parameters.AddWithValue("@CodigoBarra", producto.Codigo_Barra);
                
                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> DeleteAsync(object id)
        {
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand("DELETE FROM producto WHERE id = @Id", DataBase.Connection);
                command.Parameters.AddWithValue("@Id", id);
                
                return await command.ExecuteNonQueryAsync() > 0;
            }
        }
        
        // Métodos específicos para Producto
        public async Task<bool> ActualizarStockAsync(string productoId, int cantidad, MySqlConnection connection)
        {
            using var command = new MySqlCommand(
                "UPDATE producto SET stock = stock + @Cantidad, fecha_actualizacion = @FechaActualizacion " +
                "WHERE id = @Id",
                connection);
            
            command.Parameters.AddWithValue("@Id", productoId);
            command.Parameters.AddWithValue("@Cantidad", cantidad);
            command.Parameters.AddWithValue("@FechaActualizacion", DateTime.Now);
            
            return await command.ExecuteNonQueryAsync() > 0;
        }
        
        public async Task<IEnumerable<Producto>> GetProductosBajoStockAsync()
        {
            List<Producto> productos = new List<Producto>();
            
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand(
                    "SELECT * FROM producto WHERE stock <= stockMin",
                    DataBase.Connection);
                
                using var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    productos.Add(new Producto
                    {
                        Id = reader["id"].ToString()!,
                        Nombre = reader["nombre"].ToString()!,
                        Stock = Convert.ToInt32(reader["stock"]),
                        StockMin = Convert.ToInt32(reader["stockMin"]),
                        StockMax = Convert.ToInt32(reader["stockMax"]),
                        Fecha_Creacion = Convert.ToDateTime(reader["fecha_creacion"]),
                        Fecha_Actualizacion = Convert.ToDateTime(reader["fecha_actualizacion"]),
                        Codigo_Barra = reader["codigo_barra"].ToString()!
                    });
                }
            }
            
            return productos;
        }
    }
}