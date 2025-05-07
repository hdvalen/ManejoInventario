using System;

namespace ManejoInventario.Domain.Entities;

public enum TipoTelefono 
{
    Fijo = 0,
    Movil = 1
}
public class Tercero_Telefono
{
    public int Id { get; set; }
    public string? Numero { get; set; }
    public string? Tercero_Id { get; set; }
    public TipoTelefono Tipo_telefono { get; set; }
}