using System;

namespace ManejoInventario.Domain.Entities;
public class Ciudad
{
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public int RegionId { get; set; }
}