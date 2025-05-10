using MySql.Data.MySqlClient;
using ManejoInventario.Domain.Entities;
using ManejoInventario.Data;

namespace ManejoInventario.Repositories
{
    public class Proveedor_Repository : IRepository<Proveedor>
    {
        // Obtener todos los proveedores con JOIN a la tabla tercero
        public async Task<IEnumerable<Proveedor>> GetAllAsync()
        {
            List<Proveedor> proveedores = new List<Proveedor>();

            using (var DataBase = new DataBase())
            {
                // Se hace JOIN con la tabla tercero para obtener nombre y email
                using var command = new MySqlCommand(
                    @"SELECT p.id, p.tercero_id, p.descuento, p.dia_pago, 
                             t.nombre, t.email 
                      FROM proveedor p 
                      JOIN tercero t ON p.tercero_id = t.id", 
                    DataBase.Connection);

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    proveedores.Add(new Proveedor
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        TerceroId = reader["tercero_id"].ToString()!,
                        Descuento = Convert.ToDouble(reader["descuento"]),
                        Dia_Pago = Convert.ToInt32(reader["dia_pago"]),
                        Tercero = new Tercero
                        {
                            Id = reader["tercero_id"].ToString()!,
                            Nombre = reader["nombre"].ToString()!,
                            Email = reader["email"].ToString()!
                        },
                    });
                }
            }

            return proveedores;
        }

        // Obtener proveedor por ID
        public async Task<Proveedor?> GetByIdAsync(object id)
        {
            Proveedor? proveedor = null;

            using (var DataBase = new DataBase())
            {
                // Se hace JOIN con la tabla tercero para obtener los datos completos
                using var command = new MySqlCommand(
                    @"SELECT p.id, p.tercero_id, p.descuento, p.dia_pago, 
                             t.nombre, t.email 
                      FROM proveedor p 
                      JOIN tercero t ON p.tercero_id = t.id 
                      WHERE p.id = @Id", 
                    DataBase.Connection);

                command.Parameters.AddWithValue("@Id", id);

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    proveedor = new Proveedor
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        TerceroId = reader["tercero_id"].ToString()!,
                        Descuento = Convert.ToDouble(reader["descuento"]),
                        Dia_Pago = Convert.ToInt32(reader["dia_pago"]),
                        Tercero = new Tercero
                        {
                            Id = reader["tercero_id"].ToString()!,
                            Nombre = reader["nombre"].ToString()!,
                            Email = reader["email"].ToString()!
                        },
                    };
                }
            }

            return proveedor;
        }

        // Insertar proveedor
        public async Task<bool> InsertAsync(Proveedor proveedor)
        {
            using (var DataBase = new DataBase())
            {
                // Eliminamos la columna tercero (no existe como columna en la tabla)
                using var command = new MySqlCommand(
                    "INSERT INTO proveedor (id, tercero_id, descuento, dia_pago) VALUES (@Id, @TerceroId, @Descuento, @Dia_Pago)", 
                    DataBase.Connection);

                command.Parameters.AddWithValue("@Id", proveedor.Id);
                command.Parameters.AddWithValue("@TerceroId", proveedor.TerceroId);
                command.Parameters.AddWithValue("@Descuento", proveedor.Descuento);
                command.Parameters.AddWithValue("@Dia_Pago", proveedor.Dia_Pago);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        // Actualizar proveedor
        public async Task<bool> UpdateAsync(Proveedor proveedor)
        {
            using (var DataBase = new DataBase())
            {
                // Eliminamos la columna tercero (no existe como columna en la tabla)
                using var command = new MySqlCommand(
                    "UPDATE proveedor SET tercero_id = @TerceroId, descuento = @Descuento, dia_pago = @Dia_Pago WHERE id = @Id", 
                    DataBase.Connection);

                command.Parameters.AddWithValue("@Id", proveedor.Id);
                command.Parameters.AddWithValue("@TerceroId", proveedor.TerceroId);
                command.Parameters.AddWithValue("@Descuento", proveedor.Descuento);
                command.Parameters.AddWithValue("@Dia_Pago", proveedor.Dia_Pago);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        // Eliminar proveedor
        public async Task<bool> DeleteAsync(object id)
        {
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand("DELETE FROM proveedor WHERE id = @Id", DataBase.Connection);
                command.Parameters.AddWithValue("@Id", id);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        // Verificar si existe un proveedor por ID
        public async Task<bool> ExistAsync(object id)
        {
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand("SELECT COUNT(*) FROM proveedor WHERE id = @Id", DataBase.Connection);
                command.Parameters.AddWithValue("@Id", id);

                return Convert.ToInt32(await command.ExecuteScalarAsync()) > 0;
            }
        }
    }
}
