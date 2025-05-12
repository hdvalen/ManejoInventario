using MySql.Data.MySqlClient;
using ManejoInventario.Domain.Entities;
using ManejoInventario.Data;

namespace ManejoInventario.Repositories
{
    public class EmpleadoRepository : IRepository<Empleado>
    {
        // Obtener todos los empleados
        public async Task<IEnumerable<Empleado>> GetAllAsync()
        {
            List<Empleado> empleados = new List<Empleado>();

            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand(@"
                    SELECT 
                        e.id AS empleado_id,
                        e.tercero_id,
                        e.fecha_ingreso,
                        e.salario_base,
                        t.id AS tercero_id,
                        t.nombre,
                        t.email
                    FROM empleado e
                    JOIN tercero t ON e.tercero_id = t.id
                ", DataBase.Connection);

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    empleados.Add(new Empleado
                    {
                        Id = Convert.ToInt32(reader["empleado_id"]),
                        TerceroId = reader["tercero_id"].ToString()!,
                        Fecha_Ingreso = Convert.ToDateTime(reader["fecha_ingreso"]),
                        Salario_Base = Convert.ToDouble(reader["salario_base"]),
                        Tercero = new Tercero
                        {
                            Id = reader["tercero_id"].ToString()!,
                            Nombre = reader["nombre"].ToString()!,
                            Email = reader["email"].ToString()!
                        }
                    });
                }
            }

            return empleados;
        }

        // Obtener empleado por ID
        public async Task<Empleado?> GetByIdAsync(object id)
        {
            Empleado? empleado = null;

            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand("SELECT * FROM empleado WHERE id = @Id", DataBase.Connection);
                command.Parameters.AddWithValue("@Id", id);

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    empleado = new Empleado
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        TerceroId = reader["tercero_id"].ToString()!,
                        Fecha_Ingreso = Convert.ToDateTime(reader["fecha_ingreso"]),
                        Salario_Base = Convert.ToDouble(reader["salario_base"])
                    };
                }
            }

            return empleado;
        }

        // Crear empleado
        public async Task<bool> CreateAsync(Empleado entity)
        {
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand("INSERT INTO empleado (tercero_id, fecha_ingreso, salario_base) VALUES (@TerceroId, @Fecha_ingreso, @Salario_base)", DataBase.Connection);
                command.Parameters.AddWithValue("@TerceroId", entity.TerceroId);
                command.Parameters.AddWithValue("@Fecha_ingreso", entity.Fecha_Ingreso);
                command.Parameters.AddWithValue("@Salario_base", entity.Salario_Base);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        // Actualizar empleado
        public async Task<bool> UpdateAsync(Empleado entity)
        {
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand("UPDATE empleado SET tercero_id = @TerceroId, fecha_ingreso = @Fecha_ingreso, salario_base = @Salario WHERE id = @Id", DataBase.Connection);
                command.Parameters.AddWithValue("@Id", entity.Id);
                command.Parameters.AddWithValue("@TerceroId", entity.TerceroId);
                command.Parameters.AddWithValue("@Fecha_ingreso", entity.Fecha_Ingreso);
                command.Parameters.AddWithValue("@Salario_base", entity.Salario_Base);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        // Eliminar empleado
        public async Task<bool> DeleteAsync(object id)
        {
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand("DELETE FROM empleado WHERE id = @Id", DataBase.Connection);
                command.Parameters.AddWithValue("@Id", id);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        // Método sin implementar aún
        public Task<bool> InsertAsync(Empleado entity)
        {
            throw new NotImplementedException();
        }
    }
}