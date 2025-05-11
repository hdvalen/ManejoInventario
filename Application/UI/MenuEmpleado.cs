using ManejoInventario.Application.UI;
using ManejoInventario.Domain.Entities;
using ManejoInventario.Repositories;

namespace ManejoInventario.UI
{
    public class MenuEmpleado
    {
        private readonly EmpleadoRepository _empleadoRepository;

        public MenuEmpleado()
        {
            _empleadoRepository = new EmpleadoRepository();
        }

        public void MostrarMenu()
        {
            bool regresar = false;
            while (!regresar)
            {
                Console.Clear();
                MenuPrincipal.MostrarEncabezado("GESTION DE EMPLEADOS");
                Console.WriteLine("Menu empleados");
                Console.WriteLine("1. Listar empleados");
                Console.WriteLine("2. Agregar empleado");
                Console.WriteLine("3. Editar empleado");
                Console.WriteLine("4. Eliminar empleado");
                Console.WriteLine("0. Regresar al Menú Principal");
                Console.Write("Seleccione una opción: ");

                var opcion = Console.ReadLine() ?? "";

                switch (opcion)
                {
                    case "1":
                        ListarEmpleados().Wait();
                        break;
                    case "2":
                        AgregarEmpleado().Wait();
                        break;
                    case "3":
                        EditarEmpleado().Wait();
                        break;
                    case "4":
                        EliminarEmpleado().Wait();
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

        // Listar empleados
        private async Task ListarEmpleados()
        {
            var empleados = await _empleadoRepository.GetAllAsync();
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("LISTA DE EMPLEADOS");
            Console.WriteLine(new string('-', 80));
            Console.WriteLine("ID\tidTercero\tFecha Contratación\tSalario");
            foreach (var empleado in empleados)
            {
                Console.WriteLine($"{empleado.Id}\t{empleado.TerceroId}\t{empleado.Fecha_Ingreso.ToShortDateString()}\t{empleado.Salario_Base}");
            }
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        // Agregar empleado
        private async Task AgregarEmpleado()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("AGREGAR EMPLEADO");
            var empleado = new Empleado();
            Console.Write("Ingrese el ID del empleado: ");
            empleado.Id = Convert.ToInt32(Console.ReadLine());
            Console.Write("Ingrese el ID del tercero: ");
            empleado.TerceroId = Console.ReadLine() ?? "";
            Console.Write("Ingrese la fecha de contratación (yyyy-mm-dd): ");
            empleado.Fecha_Ingreso = Convert.ToDateTime(Console.ReadLine());
            Console.Write("Ingrese el salario: ");
            empleado.Salario_Base = (double)Convert.ToDecimal(Console.ReadLine());
            await _empleadoRepository.CreateAsync(empleado);
            MenuPrincipal.MostrarMensaje("Empleado agregado exitosamente.", ConsoleColor.Green);
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        // Editar empleado
        private async Task EditarEmpleado()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("EDITAR EMPLEADO");
            Console.Write("Ingrese el ID del empleado a editar: ");
            var id = Convert.ToInt32(Console.ReadLine());
            var empleado = await _empleadoRepository.GetByIdAsync(id);
            if (empleado == null)
            {
                MenuPrincipal.MostrarMensaje("Empleado no encontrado.", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }
            Console.Write("Ingrese el nuevo ID del tercero: ");
            empleado.TerceroId = Console.ReadLine() ?? "";
            Console.Write("Ingrese la nueva fecha de contratación (yyyy-mm-dd): ");
            empleado.Fecha_Ingreso = Convert.ToDateTime(Console.ReadLine());
            Console.Write("Ingrese el nuevo salario: ");
            empleado.Salario_Base = (double)Convert.ToDecimal(Console.ReadLine());
            await _empleadoRepository.UpdateAsync(empleado);
            MenuPrincipal.MostrarMensaje("Empleado editado exitosamente.", ConsoleColor.Green);
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        // Eliminar empleado
        private async Task EliminarEmpleado()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("ELIMINAR EMPLEADO");
            Console.Write("Ingrese el ID del empleado a eliminar: ");
            var id = Convert.ToInt32(Console.ReadLine());
            var empleado = await _empleadoRepository.GetByIdAsync(id);
            if (empleado == null)
            {
                MenuPrincipal.MostrarMensaje("Empleado no encontrado.", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }
            await _empleadoRepository.DeleteAsync(id);
            MenuPrincipal.MostrarMensaje("Empleado eliminado exitosamente.", ConsoleColor.Green);
            Console.ReadKey();
        }
    }
}