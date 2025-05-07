using System;

namespace ManejoInventario.Domain.Entities;

public class Tercero
{
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public string? Email { get; set; }
    public int TipoDocumento_id { get; set; }
    public int TipoTercero_id { get; set; }
    public int Ciudad_id { get; set; }

}