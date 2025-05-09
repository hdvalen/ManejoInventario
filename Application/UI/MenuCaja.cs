using ManejoInventario.Domain.Entities;
using ManejoInventario.Repositories;

namespace ManejoInventario.Application.UI
{
    public class MenuCaja
    {
        private readonly MovimientoCajaRepository _movimientoRepository;
        
        public MenuCaja()
        {
            _movimientoRepository = new MovimientoCajaRepository();
        }
        
        public void MostrarMenu()
        {
            bool regresar = false;
            
            while (!regresar)
            {
                Console.Clear();
                MenuPrincipal.MostrarEncabezado("GESTIÓN DE CAJA");
                
                Console.WriteLine("\nOPCIONES:");
                Console.WriteLine("1. Apertura ");
                Console.WriteLine("2. Cierre ");
                Console.WriteLine("3. Registrar Movimiento");
                Console.WriteLine("4. Ver Movimientos por Fecha");
                Console.WriteLine("5. Ver Saldo ");
                Console.WriteLine("0. Regresar al Menú Principal");
                
                Console.Write("\nSeleccione una opción: ");
                string opcion = Console.ReadLine() ?? "";
                
                switch (opcion)
                {
                    case "1":
                        AperturaCaja().Wait();
                        break;
                    case "2":
                        CierreCaja().Wait();
                        break;
                    case "3":
                        RegistrarMovimiento().Wait();
                        break;
                    case "4":
                        VerMovimientosPorFecha().Wait();
                        break;
                    case "5":
                        VerSaldoCaja().Wait();
                        break;
                    case "0":
                        regresar = true;
                        break;
                    default:
                        MenuPrincipal.MostrarMensaje("Opción no válida. Intente nuevamente.", ConsoleColor.Red);
                        Console.ReadKey();
                        break;
                }
            }
        }
        
        private async Task AperturaCaja()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("APERTURA DE CAJA");
            
            try
            {
                DateTime fechaActual = DateTime.Today;
                
                // Verificar si ya hay apertura para hoy
                var movimientosDia = await _movimientoRepository.GetMovimientosByFechaAsync(fechaActual);
                bool yaAbierto = movimientosDia.Any(m => m.Concepto.Contains("Apertura de caja"));
                
                if (yaAbierto)
                {
                    MenuPrincipal.MostrarMensaje("\nYa se ha realizado la apertura de caja para hoy.", ConsoleColor.DarkMagenta);
                }
                else
                {
                    decimal montoInicial = MenuPrincipal.LeerDecimalPositivo("\nMonto inicial de la caja: ");
                    string terceroId = MenuPrincipal.LeerEntrada("ID del Tercero responsable: ");
                    
                    var movimiento = new Mov_Caja
                    {
                        Fecha = fechaActual,
                        TipoMovimientoId = 1, // Asumir que el tipo 1 es Apertura de Caja (entrada)
                        Valor = montoInicial,
                        Concepto = "Apertura de caja",
                        TerceroId = terceroId
                    };
                    
                    bool resultado = await _movimientoRepository.InsertAsync(movimiento);
                    
                    if (resultado)
                    {
                        MenuPrincipal.MostrarMensaje("\nApertura de caja completada.", ConsoleColor.Green);
                    }
                    else
                    {
                        MenuPrincipal.MostrarMensaje("\nNo se pudo completar la apertura de caja.", ConsoleColor.Red);
                    }
                }
            }
            catch (Exception ex)
            {
                MenuPrincipal.MostrarMensaje($"\nError al registrar apertura de caja: {ex.Message}", ConsoleColor.Red);
            }
            
            Console.Write("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        
        private async Task CierreCaja()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("CIERRE DE CAJA");
            
            try
            {
                DateTime fechaActual = DateTime.Today;
                
                // Verificar si ya hay cierre para hoy
                var movimientosDia = await _movimientoRepository.GetMovimientosByFechaAsync(fechaActual);
                bool yaCerrado = movimientosDia.Any(m => m.Concepto.Contains("Cierre de caja"));
                
                if (yaCerrado)
                {
                    MenuPrincipal.MostrarMensaje("\nYa se ha realizado el cierre de caja para hoy.", ConsoleColor.DarkMagenta);
                }
                else
                {
                    // Verificar si hay apertura
                    bool hayApertura = movimientosDia.Any(m => m.Concepto.Contains("Apertura de caja"));
                    
                    if (!hayApertura)
                    {
                        MenuPrincipal.MostrarMensaje("\nNo se ha registrado apertura de caja para hoy. Debe realizar la apertura primero.", ConsoleColor.DarkMagenta);
                    }
                    else
                    {
                        // Obtener saldo de la caja
                        decimal saldoCaja = await _movimientoRepository.GetSaldoCajaAsync(fechaActual);
                        
                        Console.WriteLine($"\nSaldo actual de caja: {saldoCaja:C}");
                        
                        // Mostrar movimientos del día
                        await MostrarMovimientosDia(fechaActual);
                        
                        decimal montoContado = MenuPrincipal.LeerDecimalPositivo("\nMonto contado físicamente: ");
                        
                        decimal diferencia = montoContado - saldoCaja;
                        if (diferencia != 0)
                        {
                            Console.WriteLine($"\nDiferencia detectada: {Math.Abs(diferencia):C} {(diferencia > 0 ? "sobrante" : "faltante")}");
                        }
                        
                        string terceroId = MenuPrincipal.LeerEntrada("ID del Tercero responsable: ");
                        
                        var movimiento = new Mov_Caja
                        {
                            Fecha = fechaActual,
                            TipoMovimientoId = 2, // Asumir que el tipo 2 es Cierre de Caja
                            Valor = montoContado,
                            Concepto = $"Cierre de caja. Saldo : {saldoCaja:C}, Diferencia: {diferencia:C}",
                            TerceroId = terceroId
                        };
                        
                        bool resultado = await _movimientoRepository.InsertAsync(movimiento);
                        
                        if (resultado)
                        {
                            MenuPrincipal.MostrarMensaje("\nCierre de caja completado.", ConsoleColor.Green);
                        }
                        else
                        {
                            MenuPrincipal.MostrarMensaje("\nNo se pudo completar el cierre de caja.", ConsoleColor.Red);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MenuPrincipal.MostrarMensaje($"\nError al registrar cierre de caja: {ex.Message}", ConsoleColor.Red);
            }
            
            Console.Write("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        
        private async Task RegistrarMovimiento()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("REGISTRAR MOVIMIENTO DE CAJA");
            
            try
            {
                Console.WriteLine("\nOpciones de movimiento:");
                Console.WriteLine("1. Entrada de efectivo");
                Console.WriteLine("2. Salida de efectivo");
                
                int tipoMovimientoId = MenuPrincipal.LeerEnteroPositivo("\nSeleccione una opción: ");
                
                if (tipoMovimientoId != 1 && tipoMovimientoId != 2)
                {
                    MenuPrincipal.MostrarMensaje("\nTipo de movimiento no válido.", ConsoleColor.DarkMagenta);
                    Console.ReadKey();
                    return;
                }
                
                string concepto = MenuPrincipal.LeerEntrada("Concepto del movimiento: ");
                decimal valor = MenuPrincipal.LeerDecimalPositivo("Valor: ");
                string terceroId = MenuPrincipal.LeerEntrada("ID del Tercero: ");
                
                var movimiento = new Mov_Caja
                {
                    Fecha = DateTime.Now,
                    TipoMovimientoId = tipoMovimientoId,
                    Valor = valor,
                    Concepto = concepto,
                    TerceroId = terceroId
                };
                
                bool resultado = await _movimientoRepository.InsertAsync(movimiento);
                
                if (resultado)
                {
                    MenuPrincipal.MostrarMensaje("\nMovimiento completado.", ConsoleColor.Green);
                }
                else
                {
                    MenuPrincipal.MostrarMensaje("\nNo se pudo completar el movimiento.", ConsoleColor.Red);
                }
            }
            catch (Exception ex)
            {
                MenuPrincipal.MostrarMensaje($"\nError al registrar movimiento: {ex.Message}", ConsoleColor.Red);
            }
            
            Console.Write("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        
        private async Task VerMovimientosPorFecha()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("MOVIMIENTOS POR FECHA");
            
            try
            {
                DateTime fecha = MenuPrincipal.LeerFecha("\nIngrese la fecha (DD/MM/AAAA): ");
                
                await MostrarMovimientosDia(fecha);
                
                // Mostrar saldo
                decimal saldo = await _movimientoRepository.GetSaldoCajaAsync(fecha);
                Console.WriteLine($"\nSaldo de caja para la fecha {fecha:dd/MM/yyyy}: {saldo:C}");
            }
            catch (Exception ex)
            {
                MenuPrincipal.MostrarMensaje($"\nError al obtener movimientos: {ex.Message}", ConsoleColor.Red);
            }
            
            Console.Write("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        
        private async Task VerSaldoCaja()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("SALDO DE CAJA");
            
            try
            {
                DateTime fecha = MenuPrincipal.LeerFecha("\nIngrese la fecha (DD/MM/AAAA): ");
                
                decimal saldo = await _movimientoRepository.GetSaldoCajaAsync(fecha);
                
                Console.WriteLine($"\nSaldo de caja para la fecha {fecha:dd/MM/yyyy}: {saldo:C}");
            }
            catch (Exception ex)
            {
                MenuPrincipal.MostrarMensaje($"\nError al obtener saldo: {ex.Message}", ConsoleColor.Red);
            }
            
            Console.Write("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        
        private async Task MostrarMovimientosDia(DateTime fecha)
        {
            var movimientos = await _movimientoRepository.GetMovimientosByFechaAsync(fecha);
            
            if (!movimientos.Any())
            {
                MenuPrincipal.MostrarMensaje("\nNo hay movimientos registrados en esta fecha.", ConsoleColor.DarkMagenta);
            }
            else
            {
                Console.WriteLine($"\nMovimientos en la fecha {fecha:dd/MM/yyyy}:");
                Console.WriteLine("\n{0,-5} {1,-12} {2,-20} {3,-10} {4,-15} {5,-20}", 
                    "ID", "Hora", "Tipo", "Mov.", "Valor", "Tercero");
                Console.WriteLine(new string('-', 90));
                
                foreach (var movimiento in movimientos)
                {
                    ConsoleColor color = movimiento.Tipo_Movimiento == "Entrada" ? ConsoleColor.Green : ConsoleColor.Red;
                    
                    Console.ForegroundColor = color;
                    Console.WriteLine("{0,-5} {1,-12} {2,-20} {3,-10} {4,-15} {5,-20}", 
                        movimiento.Id, 
                        movimiento.Fecha.ToString("HH:mm:ss"),
                        movimiento.Tipo_MovimientoNom,
                        movimiento.Tipo_Movimiento,
                        movimiento.Tipo_Movimiento == "Entrada" ? movimiento.Valor.ToString("C") : $"-{movimiento.Valor:C}",
                        movimiento.Tercero?.Nombre_Completo.Length > 17 
                            ? movimiento.Tercero.Nombre_Completo.Substring(0, 17) + "..." 
                            : movimiento.Tercero?.Nombre_Completo);
                    Console.ResetColor();
                }
                
                Console.WriteLine(new string('-', 90));
            }
        }
    }
}