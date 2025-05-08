
namespace ManejoInventario.Domain.Entities;

public class Cliente
    {
        public int Id { get; set; }
        public string TerceroId { get; set; } = string.Empty;
        public DateTime Fecha_Nacimiento { get; set; }
        public DateTime? Fecha_Compra { get; set; }
        
        public Tercero? Tercero { get; set; }
        
        public override string ToString()
        {
            return $"ID: {Id}, TerceroID: {TerceroId}, Última compra: {Fecha_Compra?.ToString("d") ?? "Sin compras"}";
        }
    }