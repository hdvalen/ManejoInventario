using System;

namespace ManejoInventario.Domain.Entities;

public class Movimiento_Caja
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public int TipoMovimiento_id { get; set; }
    public decimal Valor { get; set; }
    public string? Concepto { get; set; }
    public string? Tercero_Id { get; set; }
}