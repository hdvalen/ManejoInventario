
namespace ManejoInventario.Domain.Entities;

public class Producto
    {
        public string Id { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public int Stock { get; set; }
        public int StockMin { get; set; }
        public int StockMax { get; set; }
        public DateTime Fecha_Creacion { get; set; }
        public DateTime Fecha_Actualizacion { get; set; }
        public string Codigo_Barra { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"ID: {Id}, Nombre: {Nombre}, Stock: {Stock}, Código: {Codigo_Barra}";
        }
    }