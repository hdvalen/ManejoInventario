
namespace ManejoInventario.Domain.Entities;

public class Empleado
    {
        public int Id { get; set; }
        public string TerceroId { get; set; } = string.Empty;
        public DateTime Fecha_Ingreso { get; set; }
        public double Salario_Base { get; set; }
        public int EpsId { get; set; }
        public int ArlId { get; set; }

        public Tercero? Tercero { get; set; }
        
        public override string ToString()
        {
            return $"ID: {Id}, TerceroID: {TerceroId}, Fecha ingreso: {Fecha_Ingreso:d}, Salario: {Salario_Base:C}";
        }
    }