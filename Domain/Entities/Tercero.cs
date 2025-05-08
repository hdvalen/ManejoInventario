
namespace ManejoInventario.Domain.Entities;

public class Tercero
    {
        public string Id { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Tipo_DocumentoId { get; set; }
        public int Tipo_TerceroId { get; set; }
        public int CiudadId { get; set; }
        
        public string Nombre_Completo => $"{Nombre} {Apellidos}";
        
        public override string ToString()
        {
            return $"ID: {Id}, Nombre: {Nombre_Completo}, Email: {Email}";
        }
    }