using System;

namespace ManejoInventario.Domain.Entities;

public class Empresa
{
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public DateTime FechaReg { get; set; }
    public int CiudadId { get; set; }
}