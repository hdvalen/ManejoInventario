
using ManejoInventario.Data;


namespace ManejoInventario.Repositories
{
    public class Compra_Repository : IRepository<Compra>
    {
        private readonly Producto_Repository _producto_Repository;
        
        public Compra_Repository()
        {
            _producto_Repository = new Producto_Repository();
        }
        
        public async Task<IEnumerable<Compra>> GetAllAsync()
        {
            List<Compra> compras = new List<Compra>();
            
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand(
                    "SELECT c.*, " +
                    "tp.nombre as proveedor_nombre, tp.apellidos as proveedor_apellidos, " +
                    "te.nombre as empleado_nombre, te.apellidos as empleado_apellidos " +
                    "FROM compra c " +
                    "JOIN tercero tp ON c.terceroProveedor_id = tp.id " +
                    "JOIN tercero te ON c.terceroEmpleado_id = te.id " +
                    "ORDER BY c.fecha DESC", 
                    DataBase.Connection);
                
                using var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    var compra = new Compra
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Tercero_ProveedorId = reader["terceroProveedor_id"].ToString()!,
                        Fecha = Convert.ToDateTime(reader["fecha"]),
                        Tercero_EmpleadoId = reader["terceroEmpleado_id"].ToString()!,
                        Doc_Compra = reader["DocCompra"].ToString()!,
                        Proveedor = new Tercero
                        {
                            Id = reader["terceroProveedor_id"].ToString()!,
                            Nombre = reader["proveedor_nombre"].ToString()!,
                            Apellidos = reader["proveedor_apellidos"].ToString()!
                        },
                        Empleado = new Tercero
                        {
                            Id = reader["terceroEmpleado_id"].ToString()!,
                            Nombre = reader["empleado_nombre"].ToString()!,
                            Apellidos = reader["empleado_apellidos"].ToString()!
                        }
                    };
                    
                    compras.Add(compra);
                }
            }
            foreach (var compra in compras)
            {
                compra.Detalles = (await GetDetalles_CompraAsync(compra.Id)).ToList();
            }
            
            return compras;
        }

        public async Task<Compra?> GetByIdAsync(object id)
        {
            Compra? compra = null;
            
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand(
                    "SELECT c.*, " +
                    "tp.nombre as proveedor_nombre, tp.apellidos as proveedor_apellidos, " +
                    "te.nombre as empleado_nombre, te.apellidos as empleado_apellidos " +
                    "FROM compra c " +
                    "JOIN tercero tp ON c.terceroProveedor_id = tp.id " +
                    "JOIN tercero te ON c.terceroEmpleado_id = te.id " +
                    "WHERE c.id = @Id", 
                    DataBase.Connection);
                
                command.Parameters.AddWithValue("@Id", id);
                
                using var reader = await command.ExecuteReaderAsync();
                
                if (await reader.ReadAsync())
                {
                    compra = new Compra
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Tercero_ProveedorId = reader["terceroProveedor_id"].ToString()!,
                        Fecha = Convert.ToDateTime(reader["fecha"]),
                        Tercero_EmpleadoId = reader["terceroEmpleado_id"].ToString()!,
                        Doc_Compra = reader["DocCompra"].ToString()!,
                        Proveedor = new Tercero
                        {
                            Id = reader["terceroProveedor_id"].ToString()!,
                            Nombre = reader["proveedor_nombre"].ToString()!,
                            Apellidos = reader["proveedor_apellidos"].ToString()!
                        },
                        Empleado = new Tercero
                        {
                            Id = reader["terceroEmpleado_id"].ToString()!,
                            Nombre = reader["empleado_nombre"].ToString()!,
                            Apellidos = reader["empleado_apellidos"].ToString()!
                        }
                    };
                    
                    // Cargar los detalles de la compra
                    compra.Detalles = (await GetDetalles_CompraAsync(compra.Id)).ToList();
                }
            }
            
            return compra;
        }

        public async Task<bool> InsertAsync(Compra compra)
        {
            using (var DataBase = new DataBase())
            {
                using var transaction = await DataBase.Connection.BeginTransactionAsync();
                
                try
                {
                    // Insertar la compra
                    using var commandCompra = new MySqlCommand(
                        "INSERT INTO compra (terceroProveedor_id, fecha, terceroEmpleado_id, DocCompra) " +
                        "VALUES (@TerceroProveedorId, @Fecha, @TerceroEmpleadoId, @DocCompra); " +
                        "SELECT LAST_INSERT_ID();",
                        DataBase.Connection);
                    
                    commandCompra.Parameters.AddWithValue("@TerceroProveedorId", compra.Tercero_ProveedorId);
                    commandCompra.Parameters.AddWithValue("@Fecha", compra.Fecha);
                    commandCompra.Parameters.AddWithValue("@TerceroEmpleadoId", compra.Tercero_EmpleadoId);
                    commandCompra.Parameters.AddWithValue("@DocCompra", compra.Doc_Compra);
                    
                    // Obtener el ID de la compra insertada
                    var compraId = Convert.ToInt32(await commandCompra.ExecuteScalarAsync());
                    compra.Id = compraId;
                    
                    // Insertar los detalles de la compra
                    foreach (var detalle in compra.Detalles)
                    {
                        using var commandDetalle = new MySqlCommand(
                            "INSERT INTO detalle_compra (fecha, producto_id, cantidad, valor, compra_id) " +
                            "VALUES (@Fecha, @ProductoId, @Cantidad, @Valor, @CompraId)",
                            DataBase.Connection);
                        
                        commandDetalle.Parameters.AddWithValue("@Fecha", compra.Fecha);
                        commandDetalle.Parameters.AddWithValue("@ProductoId", detalle.Producto_Id);
                        commandDetalle.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                        commandDetalle.Parameters.AddWithValue("@Valor", detalle.Valor);
                        commandDetalle.Parameters.AddWithValue("@CompraId", compraId);
                        
                        await commandDetalle.ExecuteNonQueryAsync();
                        
                        // Actualizar el stock del producto
                        await _producto_Repository.ActualizarStockAsync(detalle.Producto_Id, detalle.Cantidad, DataBase.Connection);
                    }
                    
                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<bool> UpdateAsync(Compra compra)
        {
            return await Task.FromResult(false);
        }

        public async Task<bool> DeleteAsync(object id)
        {
            return await Task.FromResult(false);
        }
        
        // Métodos específicos para Compra
        public async Task<IEnumerable<Detalle_Compra>> GetDetalles_CompraAsync(int compraId)
        {
            List<Detalle_Compra> detalles = new List<Detalle_Compra>();
            
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand(
                    "SELECT dc.*, p.nombre as producto_nombre " +
                    "FROM detalle_compra dc " +
                    "JOIN producto p ON dc.producto_id = p.id " +
                    "WHERE dc.compra_id = @CompraId",
                    DataBase.Connection);
                
                command.Parameters.AddWithValue("@CompraId", compraId);
                
                using var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    detalles.Add(new Detalle_Compra
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Fecha = Convert.ToDateTime(reader["fecha"]),
                        Producto_Id = reader["producto_id"].ToString()!,
                        Cantidad = Convert.ToInt32(reader["cantidad"]),
                        Valor = Convert.ToDecimal(reader["valor"]),
                        CompraId = compraId,
                        Producto = new Producto
                        {
                            Id = reader["producto_id"].ToString()!,
                            Nombre = reader["producto_nombre"].ToString()!
                        }
                    });
                }
            }
            
            return detalles;
        }
    }
}