using ManejoInventario.Domain.Entities;
using ManejoInventario.Repositories;

namespace ManejoInventario.Application.UI
{
    public class MenuCompras
    {
        private readonly Compra_Repository _compraRepository;
        private readonly Producto_Repository _productoRepository;
        
        public MenuCompras()
        {
            _compraRepository = new Compra_Repository();
            _productoRepository = new Producto_Repository();
        }
        
        public void MostrarMenu()
        {
            bool regresar = false;
            
            while (!regresar)
            {
                Console.Clear();
                MenuPrincipal.MostrarEncabezado("GESTIÓN DE COMPRAS");
                
                Console.WriteLine("\nOPCIONES:");
                Console.WriteLine("1. Mostrar Compras");
                Console.WriteLine("2. Ver Detalle de Compra");
                Console.WriteLine("3. Registrar Nueva Compra");
                Console.WriteLine("0. Regresar al Menú Principal");
                
                Console.Write("\nSeleccione una opción: ");
                string opcion = Console.ReadLine() ?? "";
                
                switch (opcion)
                {
                    case "1":
                        ListarCompras().Wait();
                        break;
                    case "2":
                        VerDetalleCompra().Wait();
                        break;
                    case "3":
                        RegistrarCompra().Wait();
                        break;
                    case "0":
                        regresar = true;
                        break;
                    default:
                        MenuPrincipal.MostrarMensaje("Opción no válida. Intente nuevamente.", ConsoleColor.DarkMagenta);
                        Console.ReadKey();
                        break;
                }
            }
        }
        
        private async Task ListarCompras()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("LISTA DE COMPRAS");
            
            try
            {
                var compras = await _compraRepository.GetAllAsync();
                
                if (!compras.Any())
                {
                    MenuPrincipal.MostrarMensaje("\nNo hay compras registradas.", ConsoleColor.DarkMagenta);
                }
                else
                {
                    Console.WriteLine("\n{0,-5} {1,-12} {2,-20} {3,-20} {4,-15}", 
                        "ID", "Fecha", "Proveedor", "Empleado", "Total");
                    Console.WriteLine(new string('-', 80));
                    
                    foreach (var compra in compras)
                    {
                        Console.WriteLine("{0,-5} {1,-12} {2,-20} {3,-20} {4,-15}", 
                            compra.Id, 
                            compra.Fecha.ToString("dd/MM/yyyy"),
                            compra.Proveedor?.Nombre_Completo.Length > 17 
                                ? compra.Proveedor.Nombre_Completo.Substring(0, 17) + "..." 
                                : compra.Proveedor?.Nombre_Completo,
                            compra.Empleado?.Nombre_Completo.Length > 17 
                                ? compra.Empleado.Nombre_Completo.Substring(0, 17) + "..." 
                                : compra.Empleado?.Nombre_Completo,
                            compra.Total.ToString("C"));
                    }
                }
            }
            catch (Exception ex)
            {
                MenuPrincipal.MostrarMensaje($"\n ⚠ Error al listar compras: {ex.Message}", ConsoleColor.Red);
            }
            
            Console.Write("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        
        private async Task VerDetalleCompra()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("DETALLE DE COMPRA");
            
            try
            {
                int id = MenuPrincipal.LeerEnteroPositivo("\nIngrese el ID de la compra: ");
                
                var compra = await _compraRepository.GetByIdAsync(id);
                
                if (compra == null)
                {
                    MenuPrincipal.MostrarMensaje("\nLa compra no existe.", ConsoleColor.DarkMagenta);
                }
                else
                {
                    Console.WriteLine("\nINFORMACIÓN DE LA COMPRA:");
                    Console.WriteLine($"ID: {compra.Id}");
                    Console.WriteLine($"Documento: {compra.Doc_Compra}");
                    Console.WriteLine($"Fecha: {compra.Fecha:dd/MM/yyyy}");
                    Console.WriteLine($"Proveedor: {compra.Proveedor?.Nombre_Completo}");
                    Console.WriteLine($"Empleado: {compra.Empleado?.Nombre_Completo}");
                    
                    Console.WriteLine("\nDETALLES DE PRODUCTOS:");
                    Console.WriteLine("{0,-20} {1,-10} {2,-15} {3,-15}", 
                        "Producto", "Cantidad", "Valor Unit.", "Subtotal");
                    Console.WriteLine(new string('-', 65));
                    
                    foreach (var detalle in compra.Detalles)
                    {
                        Console.WriteLine("{0,-20} {1,-10} {2,-15} {3,-15}", 
                            detalle.Producto?.Nombre.Length > 17 
                                ? detalle.Producto.Nombre.Substring(0, 17) + "..." 
                                : detalle.Producto?.Nombre,
                            detalle.Cantidad,
                            detalle.Valor.ToString("C"),
                            detalle.Subtotal.ToString("C"));
                    }
                    
                    Console.WriteLine(new string('-', 65));
                    Console.WriteLine("{0,-47} {1,-15}", "TOTAL:", compra.Total.ToString("C"));
                }
            }
            catch (Exception ex)
            {
                MenuPrincipal.MostrarMensaje($"\n ⚠ Error al obtener detalle de la compra: {ex.Message}", ConsoleColor.Red);
            }
            
            Console.Write("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        
        private async Task RegistrarCompra()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("REGISTRAR NUEVA COMPRA");
            
            try
            {
                // Capturar información general de la compra
                string proveedorId = MenuPrincipal.LeerEntrada("\nID del Proveedor: ");
                string empleadoId = MenuPrincipal.LeerEntrada("ID del Empleado: ");
                string docCompra = MenuPrincipal.LeerEntrada("Documento de compra: ");
                
                // Crear la compra
                var compra = new Compra
                {
                    Tercero_ProveedorId = proveedorId,
                    Tercero_EmpleadoId = empleadoId,
                    Fecha = DateTime.Now,
                    Doc_Compra = docCompra,
                    Detalles = new List<Detalle_Compra>()
                };
                
                // Capturar detalles de la compra (productos)
                bool agregarMasProductos = true;
                while (agregarMasProductos)
                {
                    Console.Clear();
                    MenuPrincipal.MostrarEncabezado("AGREGAR PRODUCTO A LA COMPRA");
                    
                    string productoId = MenuPrincipal.LeerEntrada("\nID del Producto: ");
                    
                    // Verificar si el producto existe
                    var producto = await _productoRepository.GetByIdAsync(productoId);
                    if (producto == null)
                    {
                        MenuPrincipal.MostrarMensaje("\nEl producto no existe.", ConsoleColor.DarkMagenta);
                        Console.ReadKey();
                        continue;
                    }
                    
                    Console.WriteLine($"Producto: {producto.Nombre}");
                    
                    int cantidad = MenuPrincipal.LeerEnteroPositivo("Cantidad: ");
                    decimal valorUnitario = MenuPrincipal.LeerDecimalPositivo("Valor unitario: ");
                    
                    // Agregar detalle a la compra
                    compra.Detalles.Add(new Detalle_Compra
                    {
                        Producto_Id = productoId,
                        Cantidad = cantidad,
                        Valor = valorUnitario,
                        Fecha = compra.Fecha,
                        Producto = producto
                    });
                    
                    string respuesta = MenuPrincipal.LeerEntrada("\n¿Desea agregar otro producto? (S/N): ");
                    agregarMasProductos = respuesta.ToUpper() == "S";
                }
                
                // Mostrar resumen de la compra
                Console.Clear();
                MenuPrincipal.MostrarEncabezado("RESUMEN DE LA COMPRA");
                
                Console.WriteLine($"\nProveedor: {proveedorId}");
                Console.WriteLine($"Empleado: {empleadoId}");
                Console.WriteLine($"Documento: {docCompra}");
                Console.WriteLine($"Fecha: {compra.Fecha:dd/MM/yyyy}");
                
                Console.WriteLine("\nPRODUCTOS:");
                Console.WriteLine("{0,-20} {1,-10} {2,-15} {3,-15}", 
                    "Producto", "Cantidad", "Valor Unit.", "Subtotal");
                Console.WriteLine(new string('-', 65));
                
                decimal total = 0;
                foreach (var detalle in compra.Detalles)
                {
                    decimal subtotal = detalle.Cantidad * detalle.Valor;
                    total += subtotal;
                    
                    Console.WriteLine("{0,-20} {1,-10} {2,-15} {3,-15}", 
                        detalle.Producto?.Nombre.Length > 17 
                            ? detalle.Producto?.Nombre.Substring(0, 17) + "..." 
                            : detalle.Producto?.Nombre,
                        detalle.Cantidad,
                        detalle.Valor.ToString("C"),
                        subtotal.ToString("C"));
                }
                
                Console.WriteLine(new string('-', 65));
                Console.WriteLine("{0,-47} {1,-15}", "TOTAL:", total.ToString("C"));
                
                // Confirmar registro
                string confirmar = MenuPrincipal.LeerEntrada("\n¿Desea registrar esta compra? (S/N): ");
                
                if (confirmar.ToUpper() == "S")
                {
                    bool resultado = await _compraRepository.InsertAsync(compra);
                    
                    if (resultado)
                    {
                        MenuPrincipal.MostrarMensaje("\n ✔ Compra completada correctamente.", ConsoleColor.Green);
                    }
                    else
                    {
                        MenuPrincipal.MostrarMensaje("\nNo se pudo completar la compra.", ConsoleColor.Red);
                    }
                }
                else
                {
                    MenuPrincipal.MostrarMensaje("\nOperación cancelada.", ConsoleColor.DarkMagenta);
                }
            }
            catch (Exception ex)
            {
                MenuPrincipal.MostrarMensaje($"\n ⚠ Error al registrar la compra: {ex.Message}", ConsoleColor.Red);
            }
            
            Console.Write("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}