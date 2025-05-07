using System;

namespace ManejoInventario.Domain.Entities;

public class Planes
{
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public int Dscto { get; set; }
}