using MySql.Data.MySqlClient;
using SGI.Data;
using SGI.Models;

namespace SGI.Repositories
{
    public class VentaRepository : IRepository<Venta>
    {
        private readonly Producto_Repository _productoRepository;
        
        public VentaRepository()
        {
            _productoRepository = new Producto_Repository();
        }
        
        public async Task<IEnumerable<Venta>> GetAllAsync()
        {
            List<Venta> ventas = new List<Venta>();
            
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand(
                    "SELECT v.*, " +
                    "tc.nombre as cliente_nombre, tc.apellidos as cliente_apellidos, " +
                    "te.nombre as empleado_nombre, te.apellidos as empleado_apellidos " +
                    "FROM venta v " +
                    "JOIN tercero tc ON v.terceroCliente_id = tc.id " +
                    "JOIN tercero te ON v.terceroEmpleado_id = te.id " +
                    "ORDER BY v.fecha DESC",
                    DataBase.Connection);
                
                using var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    var venta = new Venta
                    {
                        FacturaId = Convert.ToInt32(reader["factura_id"]),
                        Fecha = Convert.ToDateTime(reader["fecha"]),
                        Tercero_EmpleadoId = reader["terceroEmpleado_id"].ToString()!,
                        Tercero_ClienteId = reader["terceroCliente_id"].ToString()!,
                        Cliente = new Tercero
                        {
                            Id = reader["terceroCliente_id"].ToString()!,
                            Nombre = reader["cliente_nombre"].ToString()!,
                            Apellidos = reader["cliente_apellidos"].ToString()!
                        },
                        Empleado = new Tercero
                        {
                            Id = reader["terceroEmpleado_id"].ToString()!,
                            Nombre = reader["empleado_nombre"].ToString()!,
                            Apellidos = reader["empleado_apellidos"].ToString()!
                        }
                    };
                    
                    ventas.Add(venta);
                }
            }
            
            // Cargar los detalles para cada venta
            foreach (var venta in ventas)
            {
                venta.Detalles = (await GetDetallesVentaAsync(venta.FacturaId)).ToList();
            }
            
            return ventas;
        }

        public async Task<Venta?> GetByIdAsync(object id)
        {
            Venta? venta = null;
            
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand(
                    "SELECT v.*, " +
                    "tc.nombre as cliente_nombre, tc.apellidos as cliente_apellidos, " +
                    "te.nombre as empleado_nombre, te.apellidos as empleado_apellidos " +
                    "FROM venta v " +
                    "JOIN tercero tc ON v.terceroCliente_id = tc.id " +
                    "JOIN tercero te ON v.terceroEmpleado_id = te.id " +
                    "WHERE v.factura_id = @FacturaId",
                    DataBase.Connection);
                
                command.Parameters.AddWithValue("@FacturaId", id);
                
                using var reader = await command.ExecuteReaderAsync();
                
                if (await reader.ReadAsync())
                {
                    venta = new Venta
                    {
                        FacturaId = Convert.ToInt32(reader["factura_id"]),
                        Fecha = Convert.ToDateTime(reader["fecha"]),
                        Tercero_EmpleadoId = reader["terceroEmpleado_id"].ToString()!,
                        Tercero_ClienteId = reader["terceroCliente_id"].ToString()!,
                        Cliente = new Tercero
                        {
                            Id = reader["terceroCliente_id"].ToString()!,
                            Nombre = reader["cliente_nombre"].ToString()!,
                            Apellidos = reader["cliente_apellidos"].ToString()!
                        },
                        Empleado = new Tercero
                        {
                            Id = reader["terceroEmpleado_id"].ToString()!,
                            Nombre = reader["empleado_nombre"].ToString()!,
                            Apellidos = reader["empleado_apellidos"].ToString()!
                        }
                    };
                    
                    // Cargar los detalles de la venta
                    venta.Detalles = (await GetDetallesVentaAsync(venta.FacturaId)).ToList();
                }
            }
            
            return venta;
        }

        public async Task<bool> InsertAsync(Venta venta)
        {
            using (var DataBase = new DataBase())
            {
                using var transaction = await DataBase.Connection.BeginTransactionAsync();
                
                try
                {
                    // Obtener el próximo número de factura
                    int facturaId = await GetNextFacturaIdAsync(DataBase);
                    venta.FacturaId = facturaId;
                    facturaId++;
                    
                    // Insertar la venta
                    using var commandVenta = new MySqlCommand(
                        "INSERT INTO venta (factura_id, fecha, terceroEmpleado_id, terceroCliente_id) " +
                        "VALUES (@FacturaId, @Fecha, @TerceroEmpleadoId, @TerceroClienteId)",
                        DataBase.Connection);
                    
                    commandVenta.Parameters.AddWithValue("@FacturaId", facturaId);
                    commandVenta.Parameters.AddWithValue("@Fecha", venta.Fecha);
                    commandVenta.Parameters.AddWithValue("@TerceroEmpleadoId", venta.Tercero_EmpleadoId);
                    commandVenta.Parameters.AddWithValue("@TerceroClienteId", venta.Tercero_ClienteId);
                    
                    await commandVenta.ExecuteNonQueryAsync();
                    
                    // Insertar los detalles de la venta
                    foreach (var detalle in venta.Detalles)
                    {
                        using var commandDetalle = new MySqlCommand(
                            "INSERT INTO detalle_venta (factura_id, producto_id, cantidad, valor) " +
                            "VALUES (@FacturaId, @ProductoId, @Cantidad, @Valor)",
                            DataBase.Connection);
                        
                        commandDetalle.Parameters.AddWithValue("@FacturaId", facturaId);
                        commandDetalle.Parameters.AddWithValue("@ProductoId", detalle.Producto_Id);
                        commandDetalle.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                        commandDetalle.Parameters.AddWithValue("@Valor", detalle.Valor);
                        
                        await commandDetalle.ExecuteNonQueryAsync();
                        
                        // Actualizar el stock del producto (restar)
                        await _productoRepository.ActualizarStockAsync(detalle.Producto_Id, -detalle.Cantidad, DataBase.Connection);
                    }
                    
                    // Actualizar el contador de facturas
                    await UpdateFacturacionAsync(DataBase, facturaId);
                    
                    // Actualizar la fecha de compra del cliente
                    await UpdateClienteFechaCompraAsync(DataBase, venta.Tercero_ClienteId, venta.Fecha);
                    
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

        public async Task<bool> UpdateAsync(Venta venta)
        {
            // La actualización de ventas no se implementará por simplicidad
            // ya que implica manejar la reversión de stock y otros detalles complejos
            return await Task.FromResult(false);
        }

        public async Task<bool> DeleteAsync(object id)
        {
            // La eliminación de ventas no se implementará por simplicidad
            // ya que implica manejar la reversión de stock y otros detalles complejos
            return await Task.FromResult(false);
        }
        
        // Métodos específicos para Venta
        private async Task<IEnumerable<Detalle_Venta>> GetDetallesVentaAsync(int facturaId)
        {
            List<Detalle_Venta> detalles = new List<Detalle_Venta>();
            
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand(
                    "SELECT dv.*, p.nombre as producto_nombre " +
                    "FROM detalle_venta dv " +
                    "JOIN producto p ON dv.producto_id = p.id " +
                    "WHERE dv.factura_id = @FacturaId",
                    DataBase.Connection);
                
                command.Parameters.AddWithValue("@FacturaId", facturaId);
                
                using var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    detalles.Add(new Detalle_Venta
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        FacturaId = facturaId,
                        Producto_Id = reader["producto_id"].ToString()!,
                        Cantidad = Convert.ToInt32(reader["cantidad"]),
                        Valor = Convert.ToDecimal(reader["valor"]),
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
        
        private async Task<int> GetNextFacturaIdAsync(DataBase DataBase)
        {
            using var command = new MySqlCommand(
                "SELECT factura_actual FROM facturacion LIMIT 1",
                DataBase.Connection);
            
            var result = await command.ExecuteScalarAsync();
            
            if (result != null && result != DBNull.Value)
            {
                return Convert.ToInt32(result) + 1;
            }
            else
            {
                // Si no hay configuración de facturación, crear una por defecto
                using var createCommand = new MySqlCommand(
                    "INSERT INTO facturacion (fechaResolucion, numInicio, numFinal, factura_actual) " +
                    "VALUES (NOW(), 1, 1000, 1)",
                    DataBase.Connection);
                
                await createCommand.ExecuteNonQueryAsync();
                return 1;
            }
        }
        
        private async Task UpdateFacturacionAsync(DataBase DataBase, int facturaId)
        {
            using var command = new MySqlCommand(
                "UPDATE facturacion SET factura_actual = @FacturaId",
                DataBase.Connection);
            
            command.Parameters.AddWithValue("@FacturaId", facturaId);
            
            await command.ExecuteNonQueryAsync();
        }
        
        private async Task UpdateClienteFechaCompraAsync(DataBase DataBase, string terceroId, DateTime fechaCompra)
        {
            // Buscar el registro de cliente asociado al tercero
            using var findCommand = new MySqlCommand(
                "SELECT id FROM cliente WHERE tercero_id = @TerceroId",
                DataBase.Connection);
            
            findCommand.Parameters.AddWithValue("@TerceroId", terceroId);
            
            var clienteId = await findCommand.ExecuteScalarAsync();
            
            if (clienteId != null && clienteId != DBNull.Value)
            {
                // Actualizar la fecha de compra
                using var updateCommand = new MySqlCommand(
                    "UPDATE cliente SET fecha_compra = @FechaCompra WHERE id = @ClienteId",
                    DataBase.Connection);
                
                updateCommand.Parameters.AddWithValue("@ClienteId", clienteId);
                updateCommand.Parameters.AddWithValue("@FechaCompra", fechaCompra);
                
                await updateCommand.ExecuteNonQueryAsync();
            }
        }
    }
}