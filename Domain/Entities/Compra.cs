
namespace ManejoInventario.Domain.Entities;

public class Compra
    {
        public int Id { get; set; }
        public string Tercero_ProveedorId { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string Tercero_EmpleadoId { get; set; } = string.Empty;
        public string Doc_Compra { get; set; } = string.Empty;
        
        public Tercero? Proveedor { get; set; }
        public Tercero? Empleado { get; set; }
        public List<Detalle_Compra> Detalles { get; set; } = new List<Detalle_Compra>();
        
        public decimal Total => Detalles.Sum(d => d.Valor * d.Cantidad);
        
        public override string ToString()
        {
            return $"ID: {Id}, Fecha: {Fecha:d}, Doc: {Doc_Compra}, Total: {Total:C}";
        }
    }