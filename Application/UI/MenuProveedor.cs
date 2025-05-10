using ManejoInventario.Application.UI;
using ManejoInventario.Domain.Entities;
using ManejoInventario.Repositories;

namespace ManejoInventario.UI
{
    public class MenuProveedor
    {
        private readonly Proveedor_Repository _proveedorRepository;

        public MenuProveedor()
        {
            _proveedorRepository = new Proveedor_Repository();
        }

        public void MostrarMenu()
        {
            bool regresar = false;
            while (!regresar)
            {
                Console.Clear();
                MenuPrincipal.MostrarEncabezado("GESTION DE PROVEEDORES");
                Console.WriteLine("========================================");
                Console.WriteLine("Menu Proveedor");
                Console.WriteLine("1. Listar Proveedores");
                Console.WriteLine("2. Agregar Proveedor");
                Console.WriteLine("3. Editar Proveedor");
                Console.WriteLine("4. Eliminar Proveedor");
                Console.WriteLine("0. Regresar al Menú Principal");
                Console.Write("Seleccione una opción: ");
                
                var opcion = Console.ReadLine() ?? "";
                
                switch (opcion)
                {
                    case "1":
                        ListarProveedores().Wait();
                        break;
                    case "2":
                        AgregarProveedor().Wait();
                        break;
                    case "3":
                        EditarProveedor().Wait();
                        break;
                    case "4":
                        EliminarProveedor().Wait();
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
        private async Task ListarProveedores()
        {
            var proveedores = await _proveedorRepository.GetAllAsync();
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("LISTA DE PROVEEDORES");
            
            try{
                var proveedor = await _proveedorRepository.GetAllAsync(); 
                if(!proveedor.Any())
                {
                    Console.WriteLine("No hay proveedores registrados.", ConsoleColor.DarkMagenta);
                    return;
                }
              else{
                Console.WriteLine("ID\t TerceroId\t Descuento\t Día Pago\t Nombre");
                Console.WriteLine(new string('-', 80));
                foreach (var item in proveedores)
            {
                Console.WriteLine($"{item.Id}\t\t{item.TerceroId}\t\t{item.Descuento}\t\t{item.Dia_Pago}\t\t{item.Tercero?.Nombre}");
            }
              }
            }
            catch (Exception ex)
            {
               MenuPrincipal.MostrarMensaje($"\nError al listar productos: {ex.Message}", ConsoleColor.Red);
            }
            Console.Write("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        private async Task AgregarProveedor()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("AGREGAR PROVEEDOR");
            var proveedor = new Proveedor();

            Console.Write("Ingrese el ID del proveedor: ");
            proveedor.Id = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("Ingrese el id de Tercero: ");
            proveedor.TerceroId = Console.ReadLine() ?? "";

            Console.Write("Ingrese el Descuento: ");
            proveedor.Descuento = double.Parse(Console.ReadLine() ?? "0");

            Console.Write("Ingrese el Día de Pago: ");
            proveedor.Dia_Pago = int.Parse(Console.ReadLine() ?? "0");

            try
            {
                await _proveedorRepository.InsertAsync(proveedor);
                MenuPrincipal.MostrarMensaje("Proveedor agregado exitosamente.", ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                MenuPrincipal.MostrarMensaje($"Error al agregar proveedor: {ex.Message}", ConsoleColor.Red);
            }

            Console.Write("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        private async Task EditarProveedor()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("EDITAR PROVEEDOR");
            Console.Write("Ingrese el ID del proveedor a editar: ");
            var id = int.Parse(Console.ReadLine() ?? "0");

            var proveedor = await _proveedorRepository.GetByIdAsync(id);
            if (proveedor == null)
            {
                MenuPrincipal.MostrarMensaje("Proveedor no encontrado.", ConsoleColor.DarkMagenta);
                Console.ReadKey();
                return;
            }

            Console.Write("Ingrese el nuevo id de Tercero: ");
            proveedor.TerceroId = Console.ReadLine() ?? "";

            Console.Write("Ingrese el nuevo Descuento: ");
            proveedor.Descuento = double.Parse(Console.ReadLine() ?? "0");

            Console.Write("Ingrese el nuevo Día de Pago: ");
            proveedor.Dia_Pago = int.Parse(Console.ReadLine() ?? "0");

            try
            {
                await _proveedorRepository.UpdateAsync(proveedor);
                MenuPrincipal.MostrarMensaje("Proveedor editado exitosamente.", ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                MenuPrincipal.MostrarMensaje($"Error al editar proveedor: {ex.Message}", ConsoleColor.Red);
            }

            Console.Write("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        private async Task EliminarProveedor()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("ELIMINAR PROVEEDOR");
            Console.Write("Ingrese el ID del proveedor a eliminar: ");
            var id = int.Parse(Console.ReadLine() ?? "0");

            try
            {
                await _proveedorRepository.DeleteAsync(id);
                MenuPrincipal.MostrarMensaje("Proveedor eliminado exitosamente.", ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                MenuPrincipal.MostrarMensaje($"Error al eliminar proveedor: {ex.Message}", ConsoleColor.Red);
            }

            Console.Write("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        }
    }