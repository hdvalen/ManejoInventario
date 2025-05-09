using ManejoInventario.Domain.Entities;
using ManejoInventario.Repositories;

namespace ManejoInventario.Application.UI
{
    public class MenuVentas
    {
        private readonly VentaRepository _ventaRepository;
        private readonly Producto_Repository _productoRepository;
        private readonly PlanRepository _planRepository;
        
        public MenuVentas()
        {
            _ventaRepository = new VentaRepository();
            _productoRepository = new Producto_Repository();
            _planRepository = new PlanRepository();
        }
        
        public void MostrarMenu()
        {
            bool regresar = false;
            
            while (!regresar)
            {
                Console.Clear();
                MenuPrincipal.MostrarEncabezado("GESTIÓN DE VENTAS");
                
                Console.WriteLine("\nOPCIONES:");
                Console.WriteLine("1. Mostrar Ventas");
                Console.WriteLine("2. Ver Detalle de Venta");
                Console.WriteLine("3. Registrar Nueva Venta");
                Console.WriteLine("0. Regresar al Menú Principal");
                
                Console.Write("\nSeleccione una opción: ");
                string opcion = Console.ReadLine() ?? "";
                
                switch (opcion)
                {
                    case "1":
                        ListarVentas().Wait();
                        break;
                    case "2":
                        VerDetalleVenta().Wait();
                        break;
                    case "3":
                        RegistrarVenta().Wait();
                        break;
                    case "0":
                        regresar = true;
                        break;
                    default:
                        MenuPrincipal.MostrarMensaje("Opción inválida. Intente nuevamente.", ConsoleColor.DarkMagenta);
                        Console.ReadKey();
                        break;
                }
            }
        }
        
        private async Task ListarVentas()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("LISTA DE VENTAS");
            
            try
            {
                var ventas = await _ventaRepository.GetAllAsync();
                
                if (!ventas.Any())
                {
                    MenuPrincipal.MostrarMensaje("\nNo hay ventas registradas.", ConsoleColor.DarkMagenta);
                }
                else
                {
                    Console.WriteLine("\n{0,-10} {1,-12} {2,-20} {3,-20} {4,-15}", 
                        "Factura", "Fecha", "Cliente", "Empleado", "Total");
                    Console.WriteLine(new string('-', 80));
                    
                    foreach (var venta in ventas)
                    {
                        Console.WriteLine("{0,-10} {1,-12} {2,-20} {3,-20} {4,-15}", 
                            venta.FacturaId, 
                            venta.Fecha.ToString("dd/MM/yyyy"),
                            venta.Cliente?.Nombre_Completo.Length > 17 
                                ? venta.Cliente.Nombre_Completo.Substring(0, 17) + "..." 
                                : venta.Cliente?.Nombre_Completo,
                            venta.Empleado?.Nombre_Completo.Length > 17 
                                ? venta.Empleado.Nombre_Completo.Substring(0, 17) + "..." 
                                : venta.Empleado?.Nombre_Completo,
                            venta.Total.ToString("C"));
                    }
                }
            }
            catch (Exception ex)
            {
                MenuPrincipal.MostrarMensaje($"\nError al listar ventas: {ex.Message}", ConsoleColor.Red);
            }
            
            Console.Write("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        
        private async Task VerDetalleVenta()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("DETALLE DE VENTA");
            
            try
            {
                int id = MenuPrincipal.LeerEnteroPositivo("\nIngrese el número de factura: ");
                
                var venta = await _ventaRepository.GetByIdAsync(id);
                
                if (venta == null)
                {
                    MenuPrincipal.MostrarMensaje("\nLa venta no existe.", ConsoleColor.DarkMagenta);
                }
                else
                {
                    Console.WriteLine("\nINFORMACIÓN DE LA VENTA:");
                    Console.WriteLine($"Factura: {venta.FacturaId}");
                    Console.WriteLine($"Fecha: {venta.Fecha:dd/MM/yyyy}");
                    Console.WriteLine($"Cliente: {venta.Cliente?.Nombre_Completo}");
                    Console.WriteLine($"Empleado: {venta.Empleado?.Nombre_Completo}");
                    
                    Console.WriteLine("\nDETALLES DE PRODUCTOS:");
                    Console.WriteLine("{0,-20} {1,-10} {2,-15} {3,-15}", 
                        "Producto", "Cantidad", "Valor Unit.", "Subtotal");
                    Console.WriteLine(new string('-', 65));
                    
                    foreach (var detalle in venta.Detalles)
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
                    Console.WriteLine("{0,-47} {1,-15}", "TOTAL:", venta.Total.ToString("C"));
                }
            }
            catch (Exception ex)
            {
                MenuPrincipal.MostrarMensaje($"\nError al obtener detalle de la venta: {ex.Message}", ConsoleColor.Red);
            }
            
            Console.Write("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        
        private async Task RegistrarVenta()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("REGISTRAR NUEVA VENTA");
            
            try
            {
                // Obtener planes promocionales vigentes
                var planesVigentes = await _planRepository.GetPlanesVigentesAsync();
                Dictionary<string, decimal> descuentosPorProducto = new Dictionary<string, decimal>();
                
                if (planesVigentes.Any())
                {
                    MenuPrincipal.MostrarMensaje("\nHay planes promocionales vigentes:", ConsoleColor.Green);
                    foreach (var plan in planesVigentes)
                    {
                        Console.WriteLine($"- {plan.Nombre}: {plan.Descuento:P}");
                        
                        // Registrar descuentos por producto
                        foreach (var producto in plan.Productos)
                        {
                            descuentosPorProducto[producto.Id] = plan.Descuento;
                        }
                    }
                }
                else
                {
                    MenuPrincipal.MostrarMensaje("\nNo hay planes promocionales vigentes.", ConsoleColor.DarkMagenta);
                }
                
                // Capturar información general de la venta
                string clienteId = MenuPrincipal.LeerEntrada("\nID del Cliente: ");
                string empleadoId = MenuPrincipal.LeerEntrada("ID del Empleado: ");
                
                // Crear la venta
                var venta = new Venta
                {
                    Tercero_ClienteId = clienteId,
                    Tercero_EmpleadoId = empleadoId,
                    Fecha = DateTime.Now,
                    Detalles = new List<Detalle_Venta>()
                };
                
                // Capturar detalles de la venta (productos)
                bool agregarMasProductos = true;
                while (agregarMasProductos)
                {
                    Console.Clear();
                    MenuPrincipal.MostrarEncabezado("AGREGAR PRODUCTO A LA VENTA");
                    
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
                    Console.WriteLine($"Stock disponible: {producto.Stock}");
                    
                    // Verificar si el producto está en promoción
                    bool enPromocion = descuentosPorProducto.ContainsKey(productoId);
                    if (enPromocion)
                    {
                        MenuPrincipal.MostrarMensaje($"¡Producto en promoción! Descuento: {descuentosPorProducto[productoId]:P}", ConsoleColor.Green);
                    }
                    
                    int cantidad = MenuPrincipal.LeerEnteroPositivo("Cantidad: ");
                    
                    // Verificar que haya suficiente stock
                    if (cantidad > producto.Stock)
                    {
                        MenuPrincipal.MostrarMensaje($"\nError: Stock insuficiente. Solo hay {producto.Stock} unidades disponibles.", ConsoleColor.Red);
                        Console.ReadKey();
                        continue;
                    }
                    
                    decimal valorUnitario = MenuPrincipal.LeerDecimalPositivo("Valor unitario: ");
                    
                    // Aplicar descuento si está en promoción
                    if (enPromocion)
                    {
                        decimal descuento = descuentosPorProducto[productoId];
                        decimal valorConDescuento = valorUnitario * (1 - descuento);
                        
                        Console.WriteLine($"Valor con descuento: {valorConDescuento:C}");
                        valorUnitario = valorConDescuento;
                    }
                    
                    // Agregar detalle a la venta
                    venta.Detalles.Add(new Detalle_Venta
                    {
                        Producto_Id = productoId,
                        Cantidad = cantidad,
                        Valor = valorUnitario,
                        Producto = producto
                    });
                    
                    string respuesta = MenuPrincipal.LeerEntrada("\n¿Desea agregar otro producto? (S/N): ");
                    agregarMasProductos = respuesta.ToUpper() == "S";
                }
                
                // Mostrar resumen de la venta
                Console.Clear();
                MenuPrincipal.MostrarEncabezado("RESUMEN DE LA VENTA");
                
                Console.WriteLine($"\nCliente: {clienteId}");
                Console.WriteLine($"Empleado: {empleadoId}");
                Console.WriteLine($"Fecha: {venta.Fecha:dd/MM/yyyy}");
                
                Console.WriteLine("\nPRODUCTOS:");
                Console.WriteLine("{0,-20} {1,-10} {2,-15} {3,-15}", 
                    "Producto", "Cantidad", "Valor Unit.", "Subtotal");
                Console.WriteLine(new string('-', 65));
                
                decimal total = 0;
                foreach (var detalle in venta.Detalles)
                {
                    decimal subtotal = detalle.Cantidad * detalle.Valor;
                    total += subtotal;
                    
                    // Mostrar indicador de promoción
                    string nombreProducto = detalle.Producto?.Nombre ?? "";
                    if (nombreProducto.Length > 17)
                        nombreProducto = nombreProducto.Substring(0, 17) + "...";
                    
                    if (descuentosPorProducto.ContainsKey(detalle.Producto_Id))
                    {
                        nombreProducto += " (P)";
                    }
                    
                    Console.WriteLine("{0,-20} {1,-10} {2,-15} {3,-15}", 
                        nombreProducto,
                        detalle.Cantidad,
                        detalle.Valor.ToString("C"),
                        subtotal.ToString("C"));
                }
                
                Console.WriteLine(new string('-', 65));
                Console.WriteLine("{0,-47} {1,-15}", "TOTAL:", total.ToString("C"));
                
                // Confirmar registro
                string confirmar = MenuPrincipal.LeerEntrada("\n¿Desea registrar esta venta? (S/N): ");
                
                if (confirmar.ToUpper() == "S")
                {
                    bool resultado = await _ventaRepository.InsertAsync(venta);
                    
                    if (resultado)
                    {
                        MenuPrincipal.MostrarMensaje("\nVenta registrada correctamente.", ConsoleColor.Green);
                        Console.WriteLine($"Número de factura: {venta.FacturaId}");
                    }
                    else
                    {
                        MenuPrincipal.MostrarMensaje("\nNo se pudo registrar la venta.", ConsoleColor.Red);
                    }
                }
                else
                {
                    MenuPrincipal.MostrarMensaje("\nOperación cancelada.", ConsoleColor.DarkMagenta);
                }
            }
            catch (Exception ex)
            {
                MenuPrincipal.MostrarMensaje($"\nError al registrar la venta: {ex.Message}", ConsoleColor.Red);
            }
            
            Console.Write("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}