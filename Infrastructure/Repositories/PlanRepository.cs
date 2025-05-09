
using ManejoInventario.Data;

namespace ManejoInventario.Repositories
{
    public class PlanRepository : IRepository<Plan>
    {
        public async Task<IEnumerable<Plan>> GetAllAsync()
        {
            List<Plan> planes = new List<Plan>();
            
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand("SELECT * FROM plan", DataBase.Connection);
                using var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    var plan = new Plan
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Nombre = reader["nombre"].ToString()!,
                        Fecha_Inicio = Convert.ToDateTime(reader["fecha_inicio"]),
                        Fecha_Fin = Convert.ToDateTime(reader["fecha_fin"]),
                        Descuento = Convert.ToDecimal(reader["descuento"])
                    };
                    
                    planes.Add(plan);
                }
            }
            
            // Cargar los productos para cada plan
            foreach (var plan in planes)
            {
                plan.Productos = (await GetProductosByPlanAsync(plan.Id)).ToList();
            }
            
            return planes;
        }

        public async Task<Plan?> GetByIdAsync(object id)
        {
            Plan? plan = null;
            
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand("SELECT * FROM plan WHERE id = @Id", DataBase.Connection);
                command.Parameters.AddWithValue("@Id", id);
                
                using var reader = await command.ExecuteReaderAsync();
                
                if (await reader.ReadAsync())
                {
                    plan = new Plan
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Nombre = reader["nombre"].ToString()!,
                        Fecha_Inicio = Convert.ToDateTime(reader["fecha_inicio"]),
                        Fecha_Fin = Convert.ToDateTime(reader["fecha_fin"]),
                        Descuento = Convert.ToDecimal(reader["descuento"])
                    };
                    
                    // Cargar los productos del plan
                    plan.Productos = (await GetProductosByPlanAsync(plan.Id)).ToList();
                }
            }
            
            return plan;
        }

        public async Task<bool> InsertAsync(Plan plan)
        {
            using (var DataBase = new DataBase())
            {
                using var transaction = await DataBase.Connection.BeginTransactionAsync();
                
                try
                {
                    // Insertar el plan
                    using var commandPlan = new MySqlCommand(
                        "INSERT INTO plan (nombre, fecha_inicio, fecha_fin, descuento) " +
                        "VALUES (@Nombre, @FechaInicio, @FechaFin, @Descuento); " +
                        "SELECT LAST_INSERT_ID();",
                        DataBase.Connection);
                    
                    commandPlan.Parameters.AddWithValue("@Nombre", plan.Nombre);
                    commandPlan.Parameters.AddWithValue("@FechaInicio", plan.Fecha_Inicio);
                    commandPlan.Parameters.AddWithValue("@FechaFin", plan.Fecha_Fin);
                    commandPlan.Parameters.AddWithValue("@Descuento", plan.Descuento);
                    
                    // Obtener el ID del plan insertado
                    var planId = Convert.ToInt32(await commandPlan.ExecuteScalarAsync());
                    plan.Id = planId;
                    
                    // Insertar las relaciones plan-producto
                    foreach (var producto in plan.Productos)
                    {
                        using var commandRelacion = new MySqlCommand(
                            "INSERT INTO plan_producto (plan_id, producto_id) " +
                            "VALUES (@PlanId, @ProductoId)",
                            DataBase.Connection);
                        
                        commandRelacion.Parameters.AddWithValue("@PlanId", planId);
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

        public async Task<bool> UpdateAsync(Plan plan)
        {
            using (var DataBase = new DataBase())
            {
                using var transaction = await DataBase.Connection.BeginTransactionAsync();
                
                try
                {
                    // Actualizar el plan
                    using var commandPlan = new MySqlCommand(
                        "UPDATE plan SET nombre = @Nombre, fecha_inicio = @FechaInicio, " +
                        "fecha_fin = @FechaFin, descuento = @Descuento " +
                        "WHERE id = @Id",
                        DataBase.Connection);
                    
                    commandPlan.Parameters.AddWithValue("@Id", plan.Id);
                    commandPlan.Parameters.AddWithValue("@Nombre", plan.Nombre);
                    commandPlan.Parameters.AddWithValue("@FechaInicio", plan.Fecha_Inicio);
                    commandPlan.Parameters.AddWithValue("@FechaFin", plan.Fecha_Fin);
                    commandPlan.Parameters.AddWithValue("@Descuento", plan.Descuento);
                    
                    await commandPlan.ExecuteNonQueryAsync();
                    
                    // Eliminar las relaciones anteriores
                    using var commandEliminar = new MySqlCommand(
                        "DELETE FROM plan_producto WHERE plan_id = @PlanId",
                        DataBase.Connection);
                    
                    commandEliminar.Parameters.AddWithValue("@PlanId", plan.Id);
                    
                    await commandEliminar.ExecuteNonQueryAsync();
                    
                    // Insertar las nuevas relaciones
                    foreach (var producto in plan.Productos)
                    {
                        using var commandRelacion = new MySqlCommand(
                            "INSERT INTO plan_producto (plan_id, producto_id) " +
                            "VALUES (@PlanId, @ProductoId)",
                            DataBase.Connection);
                        
                        commandRelacion.Parameters.AddWithValue("@PlanId", plan.Id);
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
                        "DELETE FROM plan_producto WHERE plan_id = @Id",
                        DataBase.Connection);
                    
                    commandRelaciones.Parameters.AddWithValue("@Id", id);
                    
                    await commandRelaciones.ExecuteNonQueryAsync();
                    
                    // Eliminar el plan
                    using var commandPlan = new MySqlCommand(
                        "DELETE FROM plan WHERE id = @Id",
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
        public async Task<IEnumerable<Producto>> GetProductosByPlanAsync(int planId)
        {
            List<Producto> productos = new List<Producto>();
            
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand(
                    "SELECT p.* FROM producto p " +
                    "JOIN plan_producto pp ON p.id = pp.producto_id " +
                    "WHERE pp.plan_id = @PlanId",
                    DataBase.Connection);
                
                command.Parameters.AddWithValue("@PlanId", planId);
                
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
        
        public async Task<IEnumerable<Plan>> GetPlanesVigentesAsync()
        {
            List<Plan> planes = new List<Plan>();
            DateTime hoy = DateTime.Today;
            
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand(
                    "SELECT * FROM plan WHERE @Hoy BETWEEN fecha_inicio AND fecha_fin",
                    DataBase.Connection);
                
                command.Parameters.AddWithValue("@Hoy", hoy);
                
                using var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    var plan = new Plan
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Nombre = reader["nombre"].ToString()!,
                        Fecha_Inicio = Convert.ToDateTime(reader["fecha_inicio"]),
                        Fecha_Fin = Convert.ToDateTime(reader["fecha_fin"]),
                        Descuento = Convert.ToDecimal(reader["descuento"])
                    };
                    
                    planes.Add(plan);
                }
            }
            
            // Cargar los productos para cada plan
            foreach (var plan in planes)
            {
                plan.Productos = (await GetProductosByPlanAsync(plan.Id)).ToList();
            }
            
            return planes;
        }
    }
}