using ManejoInventario.Domain.Entities;
using ManejoInventario.Repositories;

namespace ManejoInventario.Application.UI
{
    public class MenuProductos
    {
        private readonly Producto_Repository _productoRepository;
        
        public MenuProductos()
        {
            _productoRepository = new Producto_Repository();
        }
        
        public void MostrarMenu()
        {
            bool regresar = false;
            
            while (!regresar)
            {
                Console.Clear();
                MenuPrincipal.MostrarEncabezado("GESTIÓN DE PRODUCTOS");
                Console.WriteLine("1. Mostrar Productos");
                Console.WriteLine("2. Buscar Producto");
                Console.WriteLine("3. Nuevo Producto");
                Console.WriteLine("4. Actualizar Producto");
                Console.WriteLine("5. Eliminar Producto");
                Console.WriteLine("6. Productos con Stock Bajo");
                Console.WriteLine("0. Regresar al Menú Principal");
                
                Console.Write("\nSeleccione una opción: ");
                string opcion = Console.ReadLine() ?? "";
                
                switch (opcion)
                {
                    case "1":
                        ListarProductos().Wait();
                        break;
                    case "2":
                        BuscarProducto().Wait();
                        break;
                    case "3":
                        NuevoProducto().Wait();
                        break;
                    case "4":
                        ActualizarProducto().Wait();
                        break;
                    case "5":
                        EliminarProducto().Wait();
                        break;
                    case "6":
                        ProductosStockBajo().Wait();
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
        
        private async Task ListarProductos()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("LISTA DE PRODUCTOS");
            
            try
            {
                var productos = await _productoRepository.GetAllAsync();
                
                if (!productos.Any())
                {
                    MenuPrincipal.MostrarMensaje("\nNo hay productos registrados.", ConsoleColor.DarkMagenta);
                }
                else
                {
                    Console.WriteLine("\n{0,-10} {1,-30} {2,-8} {3,-15}", "ID", "Nombre", "Stock", "Código de Barras");
                    Console.WriteLine(new string('-', 70));
                    
                    foreach (var producto in productos)
                    {
                        Console.WriteLine("{0,-10} {1,-30} {2,-8} {3,-15}", 
                            producto.Id, 
                            producto.Nombre.Length > 27 ? producto.Nombre.Substring(0, 27) + "..." : producto.Nombre, 
                            producto.Stock, 
                            producto.Codigo_Barra);
                    }
                }
            }
            catch (Exception ex)
            {
                MenuPrincipal.MostrarMensaje($"\n ⚠ Error al listar productos: {ex.Message}", ConsoleColor.Red);
            }
            
            Console.Write("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        
        private async Task BuscarProducto()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("BUSCAR PRODUCTO");
            
            string id = MenuPrincipal.LeerEntrada("\nIngrese el ID del producto: ");
            
            try
            {
                var producto = await _productoRepository.GetByIdAsync(id);
                
                if (producto == null)
                {
                    MenuPrincipal.MostrarMensaje("\nEl producto no existe.", ConsoleColor.DarkMagenta);
                }
                else
                {
                    Console.WriteLine("\nINFORMACIÓN DEL PRODUCTO:");
                    Console.WriteLine($"ID: {producto.Id}");
                    Console.WriteLine($"Nombre: {producto.Nombre}");
                    Console.WriteLine($"Stock: {producto.Stock}");
                    Console.WriteLine($"Stock Mínimo: {producto.StockMin}");
                    Console.WriteLine($"Stock Máximo: {producto.StockMax}");
                    Console.WriteLine($"Código de Barras: {producto.Codigo_Barra}");
                    Console.WriteLine($"Fecha de Creación: {producto.Fecha_Creacion:d}");
                    Console.WriteLine($"Fecha de Actualización: {producto.Fecha_Actualizacion:d}");
                    
                    // Alerta de stock
                    if (producto.Stock <= producto.StockMin)
                    {
                        MenuPrincipal.MostrarMensaje("\n¡ALERTA! Stock por debajo del mínimo permitido.", ConsoleColor.Red);
                    }
                    else if (producto.Stock >= producto.StockMax)
                    {
                        MenuPrincipal.MostrarMensaje("\n¡ALERTA! Stock por encima del máximo recomendado.", ConsoleColor.DarkMagenta);
                    }
                }
            }
            catch (Exception ex)
            {
                MenuPrincipal.MostrarMensaje($"\n ⚠ Error al buscar el producto: {ex.Message}", ConsoleColor.Red);
            }
            
            Console.Write("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        
        private async Task NuevoProducto()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("NUEVO PRODUCTO");
            
            try
            {
                string id = MenuPrincipal.LeerEntrada("\nIngrese el ID del producto: ");
                
                // Verificar si ya existe
                var existe = await _productoRepository.GetByIdAsync(id);
                if (existe != null)
                {
                    MenuPrincipal.MostrarMensaje("\n ⚠ Error: Producto ya existente.", ConsoleColor.Red);
                    Console.ReadKey();
                    return;
                }
                
                string nombre = MenuPrincipal.LeerEntrada("Ingrese el nombre del producto: ");
                string codigoBarra = MenuPrincipal.LeerEntrada("Ingrese el código de barras: ");
                int stock = MenuPrincipal.LeerEnteroPositivo("Ingrese el stock inicial: ");
                int stockMin = MenuPrincipal.LeerEnteroPositivo("Ingrese el stock mínimo: ");
                int stockMax = MenuPrincipal.LeerEnteroPositivo("Ingrese el stock máximo: ");
                
                var producto = new Producto
                {
                    Id = id,
                    Nombre = nombre,
                    Stock = stock,
                    StockMin = stockMin,
                    StockMax = stockMax,
                    Fecha_Creacion = DateTime.Now,
                    Fecha_Actualizacion = DateTime.Now,
                    Codigo_Barra = codigoBarra
                };
                
                bool resultado = await _productoRepository.InsertAsync(producto);
                
                if (resultado)
                {
                    MenuPrincipal.MostrarMensaje("\n ✔ Producto registrado correctamente.", ConsoleColor.Green);
                }
                else
                {
                    MenuPrincipal.MostrarMensaje("\nNo se pudo registrar el producto.", ConsoleColor.Red);
                }
            }
            catch (Exception ex)
            {
                MenuPrincipal.MostrarMensaje($"\n ⚠ Error al registrar el producto: {ex.Message}", ConsoleColor.Red);
            }
            
            Console.Write("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        
        private async Task ActualizarProducto()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("ACTUALIZAR PRODUCTO");
            
            string id = MenuPrincipal.LeerEntrada("\nIngrese el ID del producto : ");
            
            try
            {
                var producto = await _productoRepository.GetByIdAsync(id);
                
                if (producto == null)
                {
                    MenuPrincipal.MostrarMensaje("\nEl producto no existe.", ConsoleColor.DarkMagenta);
                }
                else
                {
                    Console.WriteLine($"\nProducto actual: {producto.Nombre}");
                    
                    string nombre = MenuPrincipal.LeerEntrada($"Ingrese el nuevo nombre ({producto.Nombre}): ");
                    if (!string.IsNullOrWhiteSpace(nombre))
                    {
                        producto.Nombre = nombre;
                    }
                    
                    string codigoBarra = MenuPrincipal.LeerEntrada($"Ingrese el nuevo código de barras ({producto.Codigo_Barra}): ");
                    if (!string.IsNullOrWhiteSpace(codigoBarra))
                    {
                        producto.Codigo_Barra = codigoBarra;
                    }
                    
                    Console.Write($"Ingrese el nuevo stock ({producto.Stock}): ");
                    string stockStr = Console.ReadLine() ?? "";
                    if (!string.IsNullOrWhiteSpace(stockStr) && int.TryParse(stockStr, out int stock) && stock >= 0)
                    {
                        producto.Stock = stock;
                    }
                    
                    Console.Write($"Ingrese el nuevo stock mínimo ({producto.StockMin}): ");
                    string stockMinStr = Console.ReadLine() ?? "";
                    if (!string.IsNullOrWhiteSpace(stockMinStr) && int.TryParse(stockMinStr, out int stockMin) && stockMin >= 0)
                    {
                        producto.StockMin = stockMin;
                    }
                    
                    Console.Write($"Ingrese el nuevo stock máximo ({producto.StockMax}): ");
                    string stockMaxStr = Console.ReadLine() ?? "";
                    if (!string.IsNullOrWhiteSpace(stockMaxStr) && int.TryParse(stockMaxStr, out int stockMax) && stockMax >= 0)
                    {
                        producto.StockMax = stockMax;
                    }
                    
                    producto.Fecha_Actualizacion = DateTime.Now;
                    
                    bool resultado = await _productoRepository.UpdateAsync(producto);
                    
                    if (resultado)
                    {
                        MenuPrincipal.MostrarMensaje("\n ✔ Producto actualizado correctamente.", ConsoleColor.Green);
                    }
                    else
                    {
                        MenuPrincipal.MostrarMensaje("\nNo se pudo actualizar el producto.", ConsoleColor.Red);
                    }
                }
            }
            catch (Exception ex)
            {
                MenuPrincipal.MostrarMensaje($"\n ⚠ Error al actualizar el producto: {ex.Message}", ConsoleColor.Red);
            }
            
            Console.Write("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        
        private async Task EliminarProducto()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("ELIMINAR PRODUCTO");
            
            string id = MenuPrincipal.LeerEntrada("\nIngrese el ID del producto a eliminar: ");
            
            try
            {
                var producto = await _productoRepository.GetByIdAsync(id);
                
                if (producto == null)
                {
                    MenuPrincipal.MostrarMensaje("\nEl producto no existe.", ConsoleColor.DarkMagenta);
                }
                else
                {
                    Console.WriteLine($"\nProducto a eliminar: {producto.Nombre}");
                    
                    string confirmacion = MenuPrincipal.LeerEntrada("\n¿Está seguro de eliminar este producto? (S/N): ");
                    
                    if (confirmacion.ToUpper() == "S")
                    {
                        bool resultado = await _productoRepository.DeleteAsync(id);
                        
                        if (resultado)
                        {
                            MenuPrincipal.MostrarMensaje("\n ✔ Producto eliminado correctamente.", ConsoleColor.Green);
                        }
                        else
                        {
                            MenuPrincipal.MostrarMensaje("\nNo se pudo eliminar el producto.", ConsoleColor.Red);
                        }
                    }
                    else
                    {
                        MenuPrincipal.MostrarMensaje("\nOperación cancelada.", ConsoleColor.DarkMagenta);
                    }
                }
            }
            catch (Exception ex)
            {
                MenuPrincipal.MostrarMensaje($"\n ⚠ Error al eliminar el producto: {ex.Message}", ConsoleColor.Red);
            }
            
            Console.Write("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        
        private async Task ProductosStockBajo()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("PRODUCTOS CON STOCK BAJO");
            
            try
            {
                var productos = await _productoRepository.GetProductosBajoStockAsync();
                
                if (!productos.Any())
                {
                    MenuPrincipal.MostrarMensaje("\nNo hay productos con stock bajo.", ConsoleColor.Green);
                }
                else
                {
                    MenuPrincipal.MostrarMensaje($"\nSe encontraron {productos.Count()} productos con stock bajo o crítico.", ConsoleColor.DarkMagenta);
                    
                    Console.WriteLine("\n{0,-10} {1,-30} {2,-8} {3,-8} {4,-8}", "ID", "Nombre", "Stock", "Mínimo", "Estado");
                    Console.WriteLine(new string('-', 70));
                    
                    foreach (var producto in productos)
                    {
                        string estado = producto.Stock == 0 ? "CRÍTICO" : "BAJO";
                        ConsoleColor color = producto.Stock == 0 ? ConsoleColor.Red : ConsoleColor.Yellow;
                        
                        Console.ForegroundColor = color;
                        Console.WriteLine("{0,-10} {1,-30} {2,-8} {3,-8} {4,-8}", 
                            producto.Id, 
                            producto.Nombre.Length > 27 ? producto.Nombre.Substring(0, 27) + "..." : producto.Nombre, 
                            producto.Stock, 
                            producto.StockMin,
                            estado);
                        Console.ResetColor();
                    }
                }
            }
            catch (Exception ex)
            {
                MenuPrincipal.MostrarMensaje($"\n ⚠ Error al buscar productos con stock bajo: {ex.Message}", ConsoleColor.Red);
            }
            
            Console.Write("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}