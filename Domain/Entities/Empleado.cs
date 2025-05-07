using System;

namespace ManejoInventario.Domain.Entities;

public class Empleado
{
    public int Id { get; set; }
    public string? Tercero_Id { get; set; }
    public DateTime Fecha_ingreso { get; set; }
    public double Salario_base { get; set; }
    public int Eps_id { get; set; }
    public int Arl_id { get; set; }
}