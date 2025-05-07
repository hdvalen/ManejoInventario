using System;

namespace ManejoInventario.Domain.Entities;
public enum TipoMovimiento 
{
    Salida =0 ,
    Entrada = 1
}
public class Tipo_MovCaja
{
    public int Id { get; set; }
    public int Nombre { get; set; }
    public TipoMovimiento Tipo_Movimiento { get; set; }
}