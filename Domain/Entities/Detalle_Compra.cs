
namespace ManejoInventario.Domain.Entities;

public class Detalle_Compra
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Producto_Id { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal Valor { get; set; }
        public int CompraId { get; set; }
        
        // Propiedades de navegación
        public Producto? Producto { get; set; }
        
        public decimal Subtotal => Valor * Cantidad;
        
        public override string ToString()
        {
            return $"ID: {Id}, Producto: {Producto_Id}, Cantidad: {Cantidad}, Valor: {Valor:C}, Subtotal: {Subtotal:C}";
        }
    }