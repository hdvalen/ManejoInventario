using ManejoInventario.Application.UI; 
using ManejoInventario.Domain.Entities;
using ManejoInventario.Repositories;

namespace ManejoInventario.UI
{
    public class MenuTercero
    {
        private readonly TerceroRepository _terceroRepository;
        public MenuTercero()
        {
            _terceroRepository = new TerceroRepository();
        }

        public void MostrarMenu()
        {
            bool regresar = false;
            while (!regresar)
            {
                Console.Clear();
                MenuPrincipal.MostrarEncabezado("GESTION DE TERCEROS");
                Console.WriteLine("Menu Tercero");
                Console.WriteLine("1. Listar Terceros");
                Console.WriteLine("2. Agregar Tercero");
                Console.WriteLine("3. Editar Tercero");
                Console.WriteLine("4. Eliminar Tercero");
                Console.WriteLine("0. Regresar al Menú Principal");
                Console.Write("Seleccione una opción: ");
                
                var opcion = Console.ReadLine() ?? "";
                
                switch (opcion)
                {
                    case "1":
                        ListarTerceros().Wait();
                        break;
                    case "2":
                        AgregarTercero().Wait();
                        break;
                    case "3":
                        EditarTercero().Wait();
                        break;
                    case "4":
                        EliminarTercero().Wait();
                        break;
                    case "0":
                        regresar = true;
                        break;
                    default:
                        MenuPrincipal.MostrarMensaje("Opción no válida. Intente de nuevo.", ConsoleColor.DarkMagenta);
                        Console.ReadKey();
                        break;
                }
            }    
        }
        private async Task ListarTerceros()
        {
            var terceros = await _terceroRepository.GetAllAsync();
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("LISTA DE TERCEROS");
            Console.WriteLine("ID\t\tNombre Completo\t\t\t\t Email");
            Console.WriteLine(new string('-', 80));
            foreach (var tercero in terceros)
            {
                Console.WriteLine($"{tercero.Id}\t\t{tercero.Nombre_Completo}\t\t{tercero.Email}");
            }
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        private async Task AgregarTercero()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("AGREGAR TERCERO");
            var tercero = new Tercero();
            Console.Write("ID: ");
            tercero.Id = Console.ReadLine() ?? "";
            Console.Write("Nombre: ");
            tercero.Nombre = Console.ReadLine() ?? "";
            Console.Write("Apellidos: ");
            tercero.Apellidos = Console.ReadLine() ?? "";
            Console.Write("Email: ");
            tercero.Email = Console.ReadLine() ?? "";
            Console.Write("id tipo de documento : ");
            if (int.TryParse(Console.ReadLine(), out int tipoDocumentoId))
            {
                tercero.Tipo_DocumentoId = tipoDocumentoId;
            }
            Console.Write("id tipo de tercero: ");
            if (int.TryParse(Console.ReadLine(), out int tipoTerceroId))
            {
                tercero.Tipo_TerceroId = tipoTerceroId;
            }
            Console.Write("id ciudad: ");
            if (int.TryParse(Console.ReadLine(), out int ciudadId))
            {
                tercero.CiudadId = ciudadId;
            }
            await _terceroRepository.CreateAsync(tercero);
            MenuPrincipal.MostrarMensaje("Tercero agregado exitosamente.", ConsoleColor.Green);
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        private async Task EditarTercero()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("EDITAR TERCERO");
            Console.Write("Ingrese el ID del tercero a editar: ");
            var id = Console.ReadLine() ?? "";
            var tercero = await _terceroRepository.GetByIdAsync(id);
            if (tercero == null)
            {
                MenuPrincipal.MostrarMensaje("Tercero no encontrado.", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }
            Console.Write("Nuevo Nombre: ");
            tercero.Nombre = Console.ReadLine() ?? "";
            Console.Write("Nuevos Apellidos: ");
            tercero.Apellidos = Console.ReadLine() ?? "";
            Console.Write("Nuevo Email: ");
            tercero.Email = Console.ReadLine() ?? "";
             Console.Write("id tipo de documento : ");
            if (int.TryParse(Console.ReadLine(), out int tipoDocumentoId))
            {
                tercero.Tipo_DocumentoId = tipoDocumentoId;
            }
            Console.Write("id tipo de tercero: ");
            if (int.TryParse(Console.ReadLine(), out int tipoTerceroId))
            {
                tercero.Tipo_TerceroId = tipoTerceroId;
            }
            Console.Write("id ciudad: ");
            if (int.TryParse(Console.ReadLine(), out int ciudadId))
            {
                tercero.CiudadId = ciudadId;
            }
           
            await _terceroRepository.UpdateAsync(tercero);
            MenuPrincipal.MostrarMensaje("Tercero editado exitosamente.", ConsoleColor.Green);
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        private async Task EliminarTercero()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("ELIMINAR TERCERO");
            Console.Write("Ingrese el ID del tercero a eliminar: ");
            var id = Console.ReadLine() ?? "";
            var tercero = await _terceroRepository.GetByIdAsync(id);
            if (tercero == null)
            {
                MenuPrincipal.MostrarMensaje("Tercero no encontrado.", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }
            await _terceroRepository.DeleteAsync(tercero);
            MenuPrincipal.MostrarMensaje("Tercero eliminado exitosamente.", ConsoleColor.Green);
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }
     }
}