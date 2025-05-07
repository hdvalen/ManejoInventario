using System;

namespace ManejoInventario.Domain.Entities;

public class Venta
{
    public int Factura_id { get; set; }
    public DateTime Fecha { get; set; }
    public string? TerceroEmpleado_id { get; set; }
    public string? TerceroCliente_id { get; set; }
}