using System;

namespace ManejoInventario.Domain.Entities;

public class Cliente
{
    public int Id { get; set; }
    public string? Tercero_id { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public DateTime FechaCompra { get; set; }

}