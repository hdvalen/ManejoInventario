using ManejoInventario.UI;

namespace ManejoInventario.Application.UI
{
    public class MenuPrincipal
    {
        private readonly MenuProductos _menuProductos;
        private readonly MenuVentas _menuVentas;
        private readonly MenuCompras _menuCompras;
        private readonly MenuCaja _menuCaja;
        private readonly MenuPlanes _menuPlanes;
        private readonly MenuProveedor _menuProveedor;
        
        public MenuPrincipal()
        {
            _menuProductos = new MenuProductos();
            _menuVentas = new MenuVentas();
            _menuCompras = new MenuCompras();
            _menuCaja = new MenuCaja();
            _menuPlanes = new MenuPlanes();
            _menuProveedor = new MenuProveedor();
        }
        
        public void MostrarMenu()
        {
            bool salir = false;
            
            while (!salir)
            {
                Console.Clear();
                MostrarEncabezado("SISTEMA ZAIKO");
                
                Console.WriteLine("\nMENÚ PRINCIPAL:");
                Console.WriteLine("1. Manejo de Productos");
                Console.WriteLine("2. Manejo de Ventas");
                Console.WriteLine("3. Manejo de Compras");
                Console.WriteLine("4. Manejo de Proveedores");
                Console.WriteLine("5. Movimientos de Caja");
                Console.WriteLine("6. Manejo de Planes Promocionales");
                Console.WriteLine("0. Salir");
                
                Console.Write("\nSeleccione una opción: ");
                string opcion = Console.ReadLine() ?? "";
                
                switch (opcion)
                {
                    case "1":
                        _menuProductos.MostrarMenu();
                        break;
                    case "2":
                        _menuVentas.MostrarMenu();
                        break;
                    case "3":
                        _menuCompras.MostrarMenu();
                        break;
                    case "4":
                        _menuProveedor.MostrarMenu();
                        break;
                    case "5":
                        _menuCaja.MostrarMenu();
                        break;
                    case "6":
                        _menuPlanes.MostrarMenu();
                        break;
                    case "0":
                        salir = true;
                        break;
                    default:
                        MostrarMensaje("Opción no válida. Intente de nuevo.", ConsoleColor.DarkMagenta);
                        Console.ReadKey();
                        break;
                }
            }
            
            MostrarMensaje("\n¡Gracias por usar el Sistema Zaiko!", ConsoleColor.DarkGreen);
        }
        
        public static void MostrarEncabezado(string titulo)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            
            string borde = new string('=', titulo.Length + 4);
            Console.WriteLine(borde);
            Console.WriteLine($"| {titulo} |");
            Console.WriteLine(borde);
            
            Console.ResetColor();
        }
        
        public static void MostrarMensaje(string mensaje, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(mensaje);
            Console.ResetColor();
        }
        
        public static string LeerEntrada(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine() ?? "";
        }
        
        public static int LeerEnteroPositivo(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int valor) && valor >= 0)
                {
                    return valor;
                }
                
                MostrarMensaje("Error: Debe ingresar un número entero positivo.", ConsoleColor.Red);
            }
        }
        
        public static decimal LeerDecimalPositivo(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (decimal.TryParse(Console.ReadLine(), out decimal valor) && valor >= 0)
                {
                    return valor;
                }
                
                MostrarMensaje("Error: Debe ingresar un número decimal positivo.", ConsoleColor.Red);
            }
        }
        
        public static DateTime LeerFecha(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (DateTime.TryParse(Console.ReadLine(), out DateTime fecha))
                {
                    return fecha;
                }
                
                MostrarMensaje("Error: Formato de fecha incorrecto. Use DD/MM/AAAA.", ConsoleColor.Red);
            }
        }
    }
}