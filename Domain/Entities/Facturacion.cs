using System;

namespace ManejoInventario.Domain.Entities;

public class Facturacion
{
    public int Id { get; set; }
    public DateTime FechaResolucion { get; set; }
    public int NumInicio { get; set; }
    public int NumFin { get; set; }
    public int FactActual { get; set; }
}