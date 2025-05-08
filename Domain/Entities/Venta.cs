
namespace ManejoInventario.Domain.Entities;

public class Venta
    {
        public int FacturaId { get; set; }
        public DateTime Fecha { get; set; }
        public string Tercero_EmpleadoId { get; set; } = string.Empty;
        public string Tercero_ClienteId { get; set; } = string.Empty;
        
        public Tercero? Empleado { get; set; }
        public Tercero? Cliente { get; set; }
        public List<Detalle_Venta> Detalles { get; set; } = new List<Detalle_Venta>();
        
        public decimal Total => Detalles.Sum(d => d.Valor * d.Cantidad);
        
        public override string ToString()
        {
            return $"Factura: {FacturaId}, Fecha: {Fecha:d}, Total: {Total:C}";
        }
    }