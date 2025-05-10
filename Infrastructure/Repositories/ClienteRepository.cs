using MySql.Data.MySqlClient;
using ManejoInventario.Domain.Entities;
using ManejoInventario.Data;

namespace ManejoInventario.Repositories
{
    public class ClienteRepository : IRepository<Cliente>
    {
        // Obtener todos los clientes
        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            List<Cliente> clientes = new List<Cliente>();

            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand(@"
                    SELECT 
                        c.id AS cliente_id,
                        c.tercero_id,
                        c.fecha_nacimiento,
                        c.fecha_compra,
                        t.id AS tercero_id,
                        t.nombre,
                        t.email
                    FROM cliente c
                    JOIN tercero t ON c.tercero_id = t.id
                ", DataBase.Connection);

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    clientes.Add(new Cliente
                    {
                        Id = Convert.ToInt32(reader["cliente_id"]),
                        TerceroId = reader["tercero_id"].ToString()!,
                        Fecha_Nacimiento = Convert.ToDateTime(reader["fecha_nacimiento"]),
                        Fecha_Compra = reader["fecha_compra"] == DBNull.Value ? null : Convert.ToDateTime(reader["fecha_compra"]),
                        Tercero = new Tercero
                        {
                            Id = reader["tercero_id"].ToString()!,
                            Nombre = reader["nombre"].ToString()!,
                            Email = reader["email"].ToString()!
                        }
                    });
                }
            }

            return clientes;
        }

        // Obtener cliente por ID
        public async Task<Cliente?> GetByIdAsync(object id)
        {
            Cliente? cliente = null;

            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand("SELECT * FROM cliente WHERE id = @Id", DataBase.Connection);
                command.Parameters.AddWithValue("@Id", id);

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    cliente = new Cliente
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        TerceroId = reader["tercero_id"].ToString()!,
                        Fecha_Nacimiento = Convert.ToDateTime(reader["fecha_nacimiento"]),
                        Fecha_Compra = reader["fecha_compra"] == DBNull.Value ? null : Convert.ToDateTime(reader["fecha_compra"])
                    };
                }
            }

            return cliente;
        }

        // Crear cliente
        public async Task<bool> CreateAsync(Cliente entity)
        {
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand("INSERT INTO cliente (tercero_id, fecha_nacimiento, fecha_compra) VALUES (@TerceroId, @Fecha_Nacimiento, @Fecha_Compra)", DataBase.Connection);
                command.Parameters.AddWithValue("@TerceroId", entity.TerceroId);
                command.Parameters.AddWithValue("@Fecha_Nacimiento", entity.Fecha_Nacimiento);
                command.Parameters.AddWithValue("@Fecha_Compra", entity.Fecha_Compra);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        // Actualizar cliente
        public async Task<bool> UpdateAsync(Cliente entity)
        {
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand("UPDATE cliente SET tercero_id = @TerceroId, fecha_nacimiento = @Fecha_Nacimiento, fecha_compra = @Fecha_Compra WHERE id = @Id", DataBase.Connection);
                command.Parameters.AddWithValue("@Id", entity.Id);
                command.Parameters.AddWithValue("@TerceroId", entity.TerceroId);
                command.Parameters.AddWithValue("@Fecha_Nacimiento", entity.Fecha_Nacimiento);
                command.Parameters.AddWithValue("@Fecha_Compra", entity.Fecha_Compra);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        // Eliminar cliente
        public async Task<bool> DeleteAsync(object id)
        {
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand("DELETE FROM cliente WHERE id = @Id", DataBase.Connection);
                command.Parameters.AddWithValue("@Id", id);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        // Método sin implementar aún
        public Task<bool> InsertAsync(Cliente entity)
        {
            throw new NotImplementedException();
        }
    }
}
