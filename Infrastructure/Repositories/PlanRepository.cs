using ManejoInventario.Domain.Entities;
using MySql.Data.MySqlClient;
using ManejoInventario.Data;


namespace ManejoInventario.Repositories
{
    public class PlanRepository : IRepository<Planes>
    {
        public async Task<IEnumerable<Planes>> GetAllAsync()
        {
            List<Planes> Planes = new List<Planes>();
            
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand("SELECT * FROM Plan", DataBase.Connection);
                using var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    var Plan = new Planes
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Nombre = reader["nombre"].ToString()!,
                        Fecha_Inicio = Convert.ToDateTime(reader["fecha_inicio"]),
                        Fecha_Fin = Convert.ToDateTime(reader["fecha_fin"]),
                        Descuento = Convert.ToDecimal(reader["descuento"])
                    };
                    
                    Planes.Add(Plan);
                }
            }
            
            // Cargar los productos para cada Plan
            foreach (var Plan in Planes)
            {
                Plan.Productos = (await GetProductosByPlanAsync(Plan.Id)).ToList();
            }
            
            return Planes;
        }

        public async Task<Planes?> GetByIdAsync(object id)
        {
            Planes? Plan = null;
            
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand("SELECT * FROM Plan WHERE id = @Id", DataBase.Connection);
                command.Parameters.AddWithValue("@Id", id);
                
                using var reader = await command.ExecuteReaderAsync();
                
                if (await reader.ReadAsync())
                {
                    Plan = new Planes
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Nombre = reader["nombre"].ToString()!,
                        Fecha_Inicio = Convert.ToDateTime(reader["fecha_inicio"]),
                        Fecha_Fin = Convert.ToDateTime(reader["fecha_fin"]),
                        Descuento = Convert.ToDecimal(reader["descuento"])
                    };
                    
                    // Cargar los productos del Plan
                    Plan.Productos = (await GetProductosByPlanAsync(Plan.Id)).ToList();
                }
            }
            
            return Plan;
        }

        public async Task<bool> InsertAsync(Planes Plan)
        {
            using (var DataBase = new DataBase())
            {
                using var transaction = await DataBase.Connection.BeginTransactionAsync();
                
                try
                {
                    // Insertar el Plan
                    using var commandPlan = new MySqlCommand(
                        "INSERT INTO Plan (nombre, fecha_inicio, fecha_fin, descuento) " +
                        "VALUES (@Nombre, @FechaInicio, @FechaFin, @Descuento); " +
                        "SELECT LAST_INSERT_ID();",
                        DataBase.Connection);
                    
                    commandPlan.Parameters.AddWithValue("@Nombre", Plan.Nombre);
                    commandPlan.Parameters.AddWithValue("@FechaInicio", Plan.Fecha_Inicio);
                    commandPlan.Parameters.AddWithValue("@FechaFin", Plan.Fecha_Fin);
                    commandPlan.Parameters.AddWithValue("@Descuento", Plan.Descuento);
                    
                    // Obtener el ID del Plan insertado
                    var PlanId = Convert.ToInt32(await commandPlan.ExecuteScalarAsync());
                    Plan.Id = PlanId;
                    
                    // Insertar las relaciones Plan-producto
                    foreach (var producto in Plan.Productos)
                    {
                        using var commandRelacion = new MySqlCommand(
                            "INSERT INTO Plan_producto (Plan_id, producto_id) " +
                            "VALUES (@PlanId, @ProductoId)",
                            DataBase.Connection);
                        
                        commandRelacion.Parameters.AddWithValue("@PlanId", PlanId);
                        commandRelacion.Parameters.AddWithValue("@ProductoId", producto.Id);
                        
                        await commandRelacion.ExecuteNonQueryAsync();
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

        public async Task<bool> UpdateAsync(Planes Plan)
        {
            using (var DataBase = new DataBase())
            {
                using var transaction = await DataBase.Connection.BeginTransactionAsync();
                
                try
                {
                    // Actualizar el Plan
                    using var commandPlan = new MySqlCommand(
                        "UPDATE Plan SET nombre = @Nombre, fecha_inicio = @FechaInicio, " +
                        "fecha_fin = @FechaFin, descuento = @Descuento " +
                        "WHERE id = @Id",
                        DataBase.Connection);
                    
                    commandPlan.Parameters.AddWithValue("@Id", Plan.Id);
                    commandPlan.Parameters.AddWithValue("@Nombre", Plan.Nombre);
                    commandPlan.Parameters.AddWithValue("@FechaInicio", Plan.Fecha_Inicio);
                    commandPlan.Parameters.AddWithValue("@FechaFin", Plan.Fecha_Fin);
                    commandPlan.Parameters.AddWithValue("@Descuento", Plan.Descuento);
                    
                    await commandPlan.ExecuteNonQueryAsync();
                    
                    // Eliminar las relaciones anteriores
                    using var commandEliminar = new MySqlCommand(
                        "DELETE FROM Plan_producto WHERE Plan_id = @PlanId",
                        DataBase.Connection);
                    
                    commandEliminar.Parameters.AddWithValue("@PlanId", Plan.Id);
                    
                    await commandEliminar.ExecuteNonQueryAsync();
                    
                    // Insertar las nuevas relaciones
                    foreach (var producto in Plan.Productos)
                    {
                        using var commandRelacion = new MySqlCommand(
                            "INSERT INTO Plan_producto (Plan_id, producto_id) " +
                            "VALUES (@PlanId, @ProductoId)",
                            DataBase.Connection);
                        
                        commandRelacion.Parameters.AddWithValue("@PlanId", Plan.Id);
                        commandRelacion.Parameters.AddWithValue("@ProductoId", producto.Id);
                        
                        await commandRelacion.ExecuteNonQueryAsync();
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

        public async Task<bool> DeleteAsync(object id)
        {
            using (var DataBase = new DataBase())
            {
                using var transaction = await DataBase.Connection.BeginTransactionAsync();
                
                try
                {
                    // Eliminar primero las relaciones
                    using var commandRelaciones = new MySqlCommand(
                        "DELETE FROM Plan_producto WHERE Plan_id = @Id",
                        DataBase.Connection);
                    
                    commandRelaciones.Parameters.AddWithValue("@Id", id);
                    
                    await commandRelaciones.ExecuteNonQueryAsync();
                    
                    // Eliminar el Plan
                    using var commandPlan = new MySqlCommand(
                        "DELETE FROM Plan WHERE id = @Id",
                        DataBase.Connection);
                    
                    commandPlan.Parameters.AddWithValue("@Id", id);
                    
                    bool result = await commandPlan.ExecuteNonQueryAsync() > 0;
                    
                    await transaction.CommitAsync();
                    return result;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
        
        // Métodos específicos para Plan
        public async Task<IEnumerable<Producto>> GetProductosByPlanAsync(int PlanId)
        {
            List<Producto> productos = new List<Producto>();
            
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand(
                    "SELECT p.* FROM producto p " +
                    "JOIN Plan_producto pp ON p.id = pp.producto_id " +
                    "WHERE pp.Plan_id = @PlanId",
                    DataBase.Connection);
                
                command.Parameters.AddWithValue("@PlanId", PlanId);
                
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
        
        public async Task<IEnumerable<Planes>> GetPlanesVigentesAsync()
        {
            List<Planes> Planes = new List<Planes>();
            DateTime hoy = DateTime.Today;
            
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand(
                    "SELECT * FROM Plan WHERE @Hoy BETWEEN fecha_inicio AND fecha_fin",
                    DataBase.Connection);
                
                command.Parameters.AddWithValue("@Hoy", hoy);
                
                using var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    var Plan = new Planes
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Nombre = reader["nombre"].ToString()!,
                        Fecha_Inicio = Convert.ToDateTime(reader["fecha_inicio"]),
                        Fecha_Fin = Convert.ToDateTime(reader["fecha_fin"]),
                        Descuento = Convert.ToDecimal(reader["descuento"])
                    };
                    
                    Planes.Add(Plan);
                }
            }
            
            // Cargar los productos para cada Plan
            foreach (var Plan in Planes)
            {
                Plan.Productos = (await GetProductosByPlanAsync(Plan.Id)).ToList();
            }
            
            return Planes;
        }
    }
}