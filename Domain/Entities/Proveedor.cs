using System;

namespace ManejoInventario.Domain.Entities;

public class Proveedor
{
    public int Id { get; set; }
    public string? Tercero_id { get; set; }
    public double Descuento { get; set; }
    public int DiaPago { get; set; }

}