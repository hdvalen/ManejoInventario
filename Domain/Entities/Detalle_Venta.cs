using System;

namespace ManejoInventario.Domain.Entities;

public class Detalle_Venta
{
    public int Id { get; set; }
    public int Factura_id { get; set; }
    public string? Producto_id { get; set; }
    public int Cantidad { get; set; }
    public decimal Valor { get; set; }
}