
namespace ManejoInventario.Domain.Entities;

 public class Planes
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime Fecha_Inicio { get; set; }
        public DateTime Fecha_Fin { get; set; }
        public decimal Descuento { get; set; }
        
        public List<Producto> Productos { get; set; } = new List<Producto>();

        public bool EstaVigente()
        {
            DateTime hoy = DateTime.Today;
            return hoy >= Fecha_Inicio && hoy <= Fecha_Fin;
        }

        public override string ToString()
        {
            return $"ID: {Id}, Nombre: {Nombre}, Descuento: {Descuento:P}, Vigencia: {Fecha_Inicio:d} - {Fecha_Fin:d}";
        }
    }