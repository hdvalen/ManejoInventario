namespace ManejoInventario.Application.Config
{
    public static class AppSettings
    {
        public static string ConnectionString = "Server=localhost;Database=gestionInventario;Uid=root;Pwd=root;";
        
        // Configuraciones adicionales de la aplicación
        public static int NumPaginacion = 10;
        public static bool RegistrosEliminados = false;
        public static int TiempoEspera = 30; // segundos
    }
}