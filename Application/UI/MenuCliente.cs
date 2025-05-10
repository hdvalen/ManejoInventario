using ManejoInventario.Application.UI; 
using ManejoInventario.Domain.Entities;
using ManejoInventario.Repositories;

namespace ManejoInventario.UI
{
    public class MenuCliente
    {
        private readonly ClienteRepository _clienteRepository;
        public MenuCliente()
        {
            _clienteRepository = new ClienteRepository();
        }

        public void MostrarMenu()
        {
            bool regresar = false;
            while (!regresar)
            {
                Console.Clear();
                MenuPrincipal.MostrarEncabezado("GESTION DE CLIENTES");
                Console.WriteLine("Menu clientes");
                Console.WriteLine("1. Listar clientes");
                Console.WriteLine("2. Agregar clientes");
                Console.WriteLine("3. Editar clientes");
                Console.WriteLine("4. Eliminar clientes");
                Console.WriteLine("0. Regresar al Menú Principal");
                Console.Write("Seleccione una opción: ");
                
                var opcion = Console.ReadLine() ?? "";
                
                switch (opcion)
                {
                    case "1":
                        ListarClientes().Wait();
                        break;
                    case "2":
                        AgregarClientes().Wait();
                        break;
                    case "3":
                        EditarClientes().Wait();
                        break;
                    case "4":
                        EliminarClientes().Wait();
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
        //Listar clientes
        private async Task ListarClientes()
        {
            var clientes = await _clienteRepository.GetAllAsync();
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("LISTA DE CLIENTES");
            Console.WriteLine(new string('-', 80));
            Console.WriteLine("ID\tidTercero\tFecha Nacimiento\tFecha Compra");
            foreach (var cliente in clientes)
            {
                Console.WriteLine($"{cliente.Id}	{cliente.TerceroId}	\t {cliente.Fecha_Nacimiento.ToShortDateString()}\t\t{cliente.Fecha_Compra}");
            }
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        //Agregar clientes
        private async Task AgregarClientes()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("AGREGAR CLIENTE");
            var cliente = new Cliente();
            Console.Write("Ingrese el ID del cliente: ");
            cliente.Id = Convert.ToInt32(Console.ReadLine());
            Console.Write("Ingrese el ID del tercero: ");
            cliente.TerceroId = Console.ReadLine() ?? "";
            Console.Write("Ingrese la fecha de nacimiento (yyyy-mm-dd): ");
            cliente.Fecha_Nacimiento = Convert.ToDateTime(Console.ReadLine());
            Console.Write("Ingrese la fecha de compra (yyyy-mm-dd): ");
            cliente.Fecha_Compra = Convert.ToDateTime(Console.ReadLine());
            await _clienteRepository.CreateAsync(cliente);
            MenuPrincipal.MostrarMensaje("Cliente agregado exitosamente.", ConsoleColor.Green);
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        //Editar clientes
        private async Task EditarClientes()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("EDITAR CLIENTE");
            Console.Write("Ingrese el ID del cliente a editar: ");
            var id = Convert.ToInt32(Console.ReadLine());
            var cliente = await _clienteRepository.GetByIdAsync(id);
            if (cliente == null)
            {
                MenuPrincipal.MostrarMensaje("Cliente no encontrado.", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }
            Console.Write("Ingrese el nuevo ID del cliente: ");
            cliente.Id = Convert.ToInt32(Console.ReadLine());
            Console.Write("Ingrese el nuevo ID del tercero: ");
            cliente.TerceroId = Console.ReadLine() ?? "";
            Console.Write("Ingrese la nueva fecha de nacimiento (yyyy-mm-dd): ");
            cliente.Fecha_Nacimiento = Convert.ToDateTime(Console.ReadLine());
            Console.Write("Ingrese la nueva fecha de compra (yyyy-mm-dd): ");
            cliente.Fecha_Compra = Convert.ToDateTime(Console.ReadLine());

            await _clienteRepository.UpdateAsync(cliente);
            MenuPrincipal.MostrarMensaje("Cliente editado exitosamente.", ConsoleColor.Green);
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        //Eliminar clientes
        private async Task EliminarClientes()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("ELIMINAR CLIENTE");
            Console.Write("Ingrese el ID del cliente a eliminar: ");
            var id = Convert.ToInt32(Console.ReadLine());
            var cliente = await _clienteRepository.GetByIdAsync(id);
            if (cliente == null)
            {
                MenuPrincipal.MostrarMensaje("Cliente no encontrado.", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }
            await _clienteRepository.DeleteAsync(cliente);
            MenuPrincipal.MostrarMensaje("Cliente eliminado exitosamente.", ConsoleColor.Green);
            Console.ReadKey();
        }
    }
}