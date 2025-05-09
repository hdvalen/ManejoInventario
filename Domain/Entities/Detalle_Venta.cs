
namespace ManejoInventario.Domain.Entities;

public class Detalle_Venta
    {
        public int Id { get; set; }
        public int FacturaId { get; set; }
        public string Producto_Id { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal Valor { get; set; }
        public Producto? Producto { get; set; }
        
        public decimal Subtotal => Valor * Cantidad;
        
        public override string ToString()
        {
            return $"ID: {Id}, Producto: {Producto_Id}, Cantidad: {Cantidad}, Valor: {Valor:C}, Subtotal: {Subtotal:C}";
        }
    }