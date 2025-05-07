using System;

namespace ManejoInventario.Domain.Entities;

public class Compra
{
    public int Id { get; set; }
    public string? TerceroProveedor_id { get; set; }
    public DateTime Fecha { get; set; }
    public string? TerceroEmpleado_id { get; set; }
    public string? DocCompra { get; set; }
}