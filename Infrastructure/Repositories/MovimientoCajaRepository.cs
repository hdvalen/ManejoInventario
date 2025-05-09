using MySql.Data.MySqlClient;
using ManejoInventario.Data;
using ManejoInventario.Models;

namespace ManejoInventario.Repositories
{
    public class MovimientoCajaRepository : IRepository<Mov_Caja>
    {
        public async Task<IEnumerable<Mov_Caja>> GetAllAsync()
        {
            List<Mov_Caja> movimientos = new List<Mov_Caja>();
            
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand(
                    "SELECT m.*, tm.nombre as tipo_nombre, tm.tipoMovimiento, " +
                    "t.nombre as tercero_nombre, t.apellidos as tercero_apellidos " +
                    "FROM movimientoCaja m " +
                    "JOIN tipoMovCaja tm ON m.tipoMovimiento_id = tm.id " +
                    "JOIN tercero t ON m.tercero_id = t.id " +
                    "ORDER BY m.fecha DESC",
                    DataBase.Connection);
                
                using var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    movimientos.Add(new Mov_Caja
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Fecha = Convert.ToDateTime(reader["fecha"]),
                        TipoMovimientoId = Convert.ToInt32(reader["tipoMovimiento_id"]),
                        Valor = Convert.ToDecimal(reader["valor"]),
                        Concepto = reader["concepto"].ToString()!,
                        TerceroId = reader["tercero_id"].ToString()!,
                        Tipo_MovimientoNom = reader["tipo_nombre"].ToString(),
                        Tipo_Movimiento = reader["tipoMovimiento"].ToString(),
                        Tercero = new Tercero
                        {
                            Id = reader["tercero_id"].ToString()!,
                            Nombre = reader["tercero_nombre"].ToString()!,
                            Apellidos = reader["tercero_apellidos"].ToString()!
                        }
                    });
                }
            }
            
            return movimientos;
        }

        public async Task<Mov_Caja?> GetByIdAsync(object id)
        {
            Mov_Caja? movimiento = null;
            
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand(
                    "SELECT m.*, tm.nombre as tipo_nombre, tm.tipoMovimiento, " +
                    "t.nombre as tercero_nombre, t.apellidos as tercero_apellidos " +
                    "FROM movimientoCaja m " +
                    "JOIN tipoMovCaja tm ON m.tipoMovimiento_id = tm.id " +
                    "JOIN tercero t ON m.tercero_id = t.id " +
                    "WHERE m.id = @Id",
                    DataBase.Connection);
                
                command.Parameters.AddWithValue("@Id", id);
                
                using var reader = await command.ExecuteReaderAsync();
                
                if (await reader.ReadAsync())
                {
                    movimiento = new Mov_Caja
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Fecha = Convert.ToDateTime(reader["fecha"]),
                        TipoMovimientoId = Convert.ToInt32(reader["tipoMovimiento_id"]),
                        Valor = Convert.ToDecimal(reader["valor"]),
                        Concepto = reader["concepto"].ToString()!,
                        TerceroId = reader["tercero_id"].ToString()!,
                        Tipo_MovimientoNom = reader["tipo_nombre"].ToString(),
                        Tipo_Movimiento = reader["tipoMovimiento"].ToString(),
                        Tercero = new Tercero
                        {
                            Id = reader["tercero_id"].ToString()!,
                            Nombre = reader["tercero_nombre"].ToString()!,
                            Apellidos = reader["tercero_apellidos"].ToString()!
                        }
                    };
                }
            }
            
            return movimiento;
        }

        public async Task<bool> InsertAsync(Mov_Caja movimiento)
        {
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand(
                    "INSERT INTO movimientoCaja (fecha, tipoMovimiento_id, valor, concepto, tercero_id) " +
                    "VALUES (@Fecha, @TipoMovimientoId, @Valor, @Concepto, @TerceroId)",
                    DataBase.Connection);
                
                command.Parameters.AddWithValue("@Fecha", movimiento.Fecha);
                command.Parameters.AddWithValue("@TipoMovimientoId", movimiento.TipoMovimientoId);
                command.Parameters.AddWithValue("@Valor", movimiento.Valor);
                command.Parameters.AddWithValue("@Concepto", movimiento.Concepto);
                command.Parameters.AddWithValue("@TerceroId", movimiento.TerceroId);
                
                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> UpdateAsync(Mov_Caja movimiento)
        {
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand(
                    "UPDATE movimientoCaja SET fecha = @Fecha, tipoMovimiento_id = @TipoMovimientoId, " +
                    "valor = @Valor, concepto = @Concepto, tercero_id = @TerceroId " +
                    "WHERE id = @Id",
                    DataBase.Connection);
                
                command.Parameters.AddWithValue("@Id", movimiento.Id);
                command.Parameters.AddWithValue("@Fecha", movimiento.Fecha);
                command.Parameters.AddWithValue("@TipoMovimientoId", movimiento.TipoMovimientoId);
                command.Parameters.AddWithValue("@Valor", movimiento.Valor);
                command.Parameters.AddWithValue("@Concepto", movimiento.Concepto);
                command.Parameters.AddWithValue("@TerceroId", movimiento.TerceroId);
                
                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> DeleteAsync(object id)
        {
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand(
                    "DELETE FROM movimientoCaja WHERE id = @Id",
                    DataBase.Connection);
                
                command.Parameters.AddWithValue("@Id", id);
                
                return await command.ExecuteNonQueryAsync() > 0;
            }
        }
        
        // Métodos específicos para MovimientoCaja
        public async Task<decimal> GetSaldoCajaAsync(DateTime fecha)
        {
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand(
                    "SELECT SUM(CASE WHEN tm.tipoMovimiento = 'Entrada' THEN m.valor ELSE -m.valor END) as saldo " +
                    "FROM movimientoCaja m " +
                    "JOIN tipoMovCaja tm ON m.tipoMovimiento_id = tm.id " +
                    "WHERE DATE(m.fecha) = DATE(@Fecha)",
                    DataBase.Connection);
                
                command.Parameters.AddWithValue("@Fecha", fecha);
                
                var result = await command.ExecuteScalarAsync();
                
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToDecimal(result);
                }
                
                return 0;
            }
        }
        
        public async Task<IEnumerable<Mov_Caja>> GetMovimientosByFechaAsync(DateTime fecha)
        {
            List<Mov_Caja> movimientos = new List<Mov_Caja>();
            
            using (var DataBase = new DataBase())
            {
                using var command = new MySqlCommand(
                    "SELECT m.*, tm.nombre as tipo_nombre, tm.tipoMovimiento, " +
                    "t.nombre as tercero_nombre, t.apellidos as tercero_apellidos " +
                    "FROM movimientoCaja m " +
                    "JOIN tipoMovCaja tm ON m.tipoMovimiento_id = tm.id " +
                    "JOIN tercero t ON m.tercero_id = t.id " +
                    "WHERE DATE(m.fecha) = DATE(@Fecha) " +
                    "ORDER BY m.fecha DESC",
                    DataBase.Connection);
                
                command.Parameters.AddWithValue("@Fecha", fecha);
                
                using var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    movimientos.Add(new Mov_Caja
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Fecha = Convert.ToDateTime(reader["fecha"]),
                        TipoMovimientoId = Convert.ToInt32(reader["tipoMovimiento_id"]),
                        Valor = Convert.ToDecimal(reader["valor"]),
                        Concepto = reader["concepto"].ToString()!,
                        TerceroId = reader["tercero_id"].ToString()!,
                        Tipo_MovimientoNom = reader["tipo_nombre"].ToString(),
                        Tipo_Movimiento = reader["tipoMovimiento"].ToString(),
                        Tercero = new Tercero
                        {
                            Id = reader["tercero_id"].ToString()!,
                            Nombre = reader["tercero_nombre"].ToString()!,
                            Apellidos = reader["tercero_apellidos"].ToString()!
                        }
                    });
                }
            }
            
            return movimientos;
        }
    }
}