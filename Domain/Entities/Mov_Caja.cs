

namespace ManejoInventario.Domain.Entities;

public class Mov_Caja
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int TipoMovimientoId { get; set; }
        public decimal Valor { get; set; }
        public string Concepto { get; set; } = string.Empty;
        public string TerceroId { get; set; } = string.Empty;
        
        public string? Tipo_MovimientoNom{ get; set; }
        public string? Tipo_Movimiento { get; set; } // Entrada o Salida
        public Tercero? Tercero { get; set; }
        
        public override string ToString()
        {
            return $"ID: {Id}, Fecha: {Fecha:d}, Tipo: {Tipo_MovimientoNom} ({Tipo_Movimiento}), Valor: {Valor:C}";
        }
    }