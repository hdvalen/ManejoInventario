using MySql.Data.MySqlClient;
using ManejoInventario.Domain.Entities;
using ManejoInventario.Data;

namespace ManejoInventario.Repositories
{
    public class TerceroRepository : IRepository<Tercero>
    {
        // Obtener todos los terceros
        public async Task<IEnumerable<Tercero>> GetAllAsync()
        {
            List<Tercero> terceros = new List<Tercero>();
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand(
                    @"SELECT id, nombre, apellidos, email, tipo_documento_id, tipo_tercero_id, ciudad_id 
                      FROM tercero", 
                    DataBase.Connection);

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    terceros.Add(new Tercero
                    {
                        Id = reader["id"].ToString()!,
                        Nombre = reader["nombre"].ToString()!,
                        Apellidos = reader["apellidos"].ToString()!,
                        Email = reader["email"].ToString()!,
                        Tipo_DocumentoId = Convert.ToInt32(reader["tipo_documento_id"]),
                        Tipo_TerceroId = Convert.ToInt32(reader["tipo_tercero_id"]),
                        CiudadId = Convert.ToInt32(reader["ciudad_id"])
                    });
                }
            }
            return terceros;
        }
        // Obtener tercero por ID
        public async Task<Tercero?> GetByIdAsync(object id)
        {
            Tercero? tercero = null;
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand(
                    @"SELECT id, nombre, apellidos, email, tipo_documento_id, tipo_tercero_id, ciudad_id 
                      FROM tercero 
                      WHERE id = @Id", 
                    DataBase.Connection);
                command.Parameters.AddWithValue("@Id", id);

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    tercero = new Tercero
                    {
                        Id = reader["id"].ToString()!,
                        Nombre = reader["nombre"].ToString()!,
                        Apellidos = reader["apellidos"].ToString()!,
                        Email = reader["email"].ToString()!,
                        Tipo_DocumentoId = Convert.ToInt32(reader["tipo_documento_id"]),
                        Tipo_TerceroId = Convert.ToInt32(reader["tipo_tercero_id"]),
                        CiudadId = Convert.ToInt32(reader["ciudad_id"])
                    };
                }
            }
            return tercero;
        }
        // Crear nuevo tercero
        public async Task<Tercero> CreateAsync(Tercero entity)
        {
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand(
                    @"INSERT INTO tercero (id, nombre, apellidos, email, tipo_documento_id, tipo_tercero_id, ciudad_id) 
                      VALUES (@Id, @Nombre, @Apellidos, @Email, @Tipo_DocumentoId, @Tipo_TerceroId, @CiudadId)", 
                    DataBase.Connection);
                
                command.Parameters.AddWithValue("@Id", entity.Id);
                command.Parameters.AddWithValue("@Nombre", entity.Nombre);
                command.Parameters.AddWithValue("@Apellidos", entity.Apellidos);
                command.Parameters.AddWithValue("@Email", entity.Email);
                command.Parameters.AddWithValue("@Tipo_DocumentoId", entity.Tipo_DocumentoId);
                command.Parameters.AddWithValue("@Tipo_TerceroId", entity.Tipo_TerceroId);
                command.Parameters.AddWithValue("@CiudadId", entity.CiudadId);

                await command.ExecuteNonQueryAsync();
            }
            return entity;
        }
        // Actualizar tercero   
        public async Task<bool> UpdateAsync(Tercero entity)
        {
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand(
                    @"UPDATE tercero 
                      SET nombre = @Nombre, apellidos = @Apellidos, email = @Email, 
                          tipo_documento_id = @Tipo_DocumentoId, tipo_tercero_id = @Tipo_TerceroId, ciudad_id = @CiudadId 
                      WHERE id = @Id", 
                    DataBase.Connection);
                
                command.Parameters.AddWithValue("@Id", entity.Id);
                command.Parameters.AddWithValue("@Nombre", entity.Nombre);
                command.Parameters.AddWithValue("@Apellidos", entity.Apellidos);
                command.Parameters.AddWithValue("@Email", entity.Email);
                command.Parameters.AddWithValue("@Tipo_DocumentoId", entity.Tipo_DocumentoId);
                command.Parameters.AddWithValue("@Tipo_TerceroId", entity.Tipo_TerceroId);
                command.Parameters.AddWithValue("@CiudadId", entity.CiudadId);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }
        // Eliminar tercero
        public async Task<bool> DeleteAsync(object id)
        {
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand(
                    @"DELETE FROM tercero 
                      WHERE id = @Id", 
                    DataBase.Connection);
                
                command.Parameters.AddWithValue("@Id", id);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }
        // Obtener tercero por nombre
        public async Task<Tercero?> GetByNameAsync(string name)
        {
            Tercero? tercero = null;
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand(
                    @"SELECT id, nombre, apellidos, email, tipo_documento_id, tipo_tercero_id, ciudad_id 
                      FROM tercero 
                      WHERE nombre = @Nombre", 
                    DataBase.Connection);
                
                command.Parameters.AddWithValue("@Nombre", name);

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    tercero = new Tercero
                    {
                        Id = reader["id"].ToString()!,
                        Nombre = reader["nombre"].ToString()!,
                        Apellidos = reader["apellidos"].ToString()!,
                        Email = reader["email"].ToString()!,
                        Tipo_DocumentoId = Convert.ToInt32(reader["tipo_documento_id"]),
                        Tipo_TerceroId = Convert.ToInt32(reader["tipo_tercero_id"]),
                        CiudadId = Convert.ToInt32(reader["ciudad_id"])
                    };
                }
            }
            return tercero;
        }

        public Task<bool> InsertAsync(Tercero entity)
        {
            throw new NotImplementedException();
        }
    }
}
