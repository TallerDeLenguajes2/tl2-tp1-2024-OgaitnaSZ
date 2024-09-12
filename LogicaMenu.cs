using EspacioCadete;
using EspacioCadeteria;
using EspacioCliente;
using EspacioPedido;

namespace OpcionesMenu{
        class Opciones{
        public static void DarDeAltaPedido(Cadeteria cadeteria){
            Console.Write("Ingrese el número del pedido: ");
            int nroPedido;
            bool control = true;

            do{
                if(int.TryParse(Console.ReadLine(), out nroPedido) && nroPedido > 0 && cadeteria.pedidoExiste(nroPedido)){
                    string obs = string.Empty;
                    while (string.IsNullOrWhiteSpace(obs)){
                        Console.Write("Ingrese la observación del pedido: ");
                        obs = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(obs)){
                            Console.WriteLine("La observación no puede estar vacía.");
                        }
                    }

                    string nombreCliente = string.Empty;
                    while (string.IsNullOrWhiteSpace(nombreCliente)){
                        Console.Write("Ingrese el nombre del cliente: ");
                        nombreCliente = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(nombreCliente)){
                            Console.WriteLine("El nombre del cliente no puede estar vacío.");
                        }
                    }

                    string direccionCliente = string.Empty;
                    while (string.IsNullOrWhiteSpace(direccionCliente)){
                        Console.Write("Ingrese la dirección del cliente: ");
                        direccionCliente = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(direccionCliente)){
                            Console.WriteLine("La dirección no puede estar vacía.");
                        }
                    }

                    string telefonoCliente = string.Empty;
                    while (string.IsNullOrWhiteSpace(telefonoCliente)){
                        Console.Write("Ingrese el teléfono del cliente: ");
                        telefonoCliente = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(telefonoCliente)){
                            Console.WriteLine("El teléfono no puede estar vacío.");
                        }
                    }

                    string datosReferencia = string.Empty;
                    while (string.IsNullOrWhiteSpace(datosReferencia)){
                        Console.Write("Ingrese los datos de referencia de la dirección: ");
                        datosReferencia = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(datosReferencia)){
                            Console.WriteLine("Los datos de referencia no pueden estar vacíos.");
                        }
                    }

                    Cliente cliente = new Cliente(nombreCliente, direccionCliente, telefonoCliente, datosReferencia);
                    
                    int idCadete;
                    bool controlCadete = true;
                    mostrarCadetes(cadeteria.ListadoCadetes);
                    while (controlCadete){
                        Console.Write("Ingrese el ID del cadete para asignar el pedido: ");
                        if (int.TryParse(Console.ReadLine(), out idCadete)){
                            Cadete cadete = cadeteria.ObtenerCadetePorId(idCadete);
                            if (cadete != null){
                                cadeteria.AgregarPedido(new Pedido(nroPedido, obs, cliente, cadete));
                                Console.WriteLine("Pedido creado con éxito.");
                                controlCadete = false;  // Salir del ciclo cuando se ha asignado un cadete válido.
                                control = false;  // Esto también asegura que se salga del ciclo principal.
                            }else{
                                Console.WriteLine("Debe ingresar un ID válido de cadete.");
                            }
                        }else{
                            Console.WriteLine("Debe ingresar un ID válido de cadete.");
                        }
                    }
                }else{
                    Console.WriteLine("El numero de pedido no es valido o ya existe.");
                }
            }while(control);
        }
        
        public static void CambiarEstadoDePedido(Cadeteria cadeteria){
            if(cadeteria.ListadoPedidos != null && cadeteria.contarPedidosPendientes()>0){
                mostrarPedidosPendientes(cadeteria.ListadoPedidos);
                Console.Write("Ingrese el numero del pedido: ");
                int numPedido;
                bool pedidoEncontrado = true;

                while(pedidoEncontrado){
                    if(int.TryParse(Console.ReadLine(), out numPedido)){
                        foreach(Pedido pedido in cadeteria.ListadoPedidos){
                            if(pedido.Nro == numPedido){
                                pedido.CambiarEstado("Entregado");
                                Console.WriteLine("Pedido actualizado con exito");
                                pedidoEncontrado = false;
                                break;
                            }
                        }
                        if(pedidoEncontrado){
                            Console.Write("Escriba un ID de pedido existente: ");
                        }
                    }else{
                        Console.Write("Escriba un numero: ");
                    }
                }
            }else{
                Console.WriteLine("No hay pedidos pendientes");
            }
        }

        public static void ReasignarPedidoAOtroCadete(Cadeteria cadeteria){
            if(cadeteria.ListadoPedidos != null && cadeteria.contarPedidosPendientes()>0){
                mostrarPedidosPendientes(cadeteria.ListadoPedidos);
                int numPedido;
                bool pedidoEncontrado = true;
                Console.Write("Ingrese el numero de pedido a reasignar: ");
                while(pedidoEncontrado){
                    if(int.TryParse(Console.ReadLine(), out numPedido)){
                        foreach(Pedido pedido in cadeteria.ListadoPedidos){
                            if(pedido.Nro == numPedido){
                                mostrarCadetes(cadeteria.ListadoCadetes);
                                Console.Write("Ingrese ID del cadete a asignar: ");
                                int idCadete;
                                bool cadeteEncontrado = true;
                                while(cadeteEncontrado){
                                    if(int.TryParse(Console.ReadLine(), out idCadete)){
                                        Cadete cadete = cadeteria.ObtenerCadetePorId(idCadete);
                                        if(cadete != null){
                                            cadeteria.ReasignarPedido(cadete, pedido);
                                            Console.WriteLine("Pedido reasignado con exito");
                                            cadeteEncontrado = false;
                                        }
                                        if(cadeteEncontrado){
                                            Console.Write("Escriba un ID de cadete existente: ");
                                        }
                                        pedidoEncontrado = false;
                                    }else{
                                        Console.Write("Escriba un numero: ");
                                    }
                                }
                            }
                        }
                        if(pedidoEncontrado){
                            Console.Write("Escriba un ID de pedido existente: ");
                        }
                    }else{
                        Console.Write("Escriba un numero: ");
                    }
                }
            }else{
                Console.WriteLine("No hay pedidos pendientes");
            }
        }
        
        //Metodos complementarios
        private static void mostrarCadetes(List<Cadete> cadetes){
            foreach(Cadete cadete in cadetes){
                mostrarCadete(cadete);
            }
        }
        private static void mostrarCadete(Cadete cadete){
            Console.WriteLine("─────────────────────────────────────");
            Console.WriteLine($"ID: {cadete.Id}");
            Console.WriteLine($"Nombre: {cadete.Nombre}");
            Console.WriteLine("─────────────────────────────────────");
        }
        private static void mostrarPedidosPendientes(List<Pedido> pedidos){
            foreach(Pedido pedido in pedidos){
                if(pedido.Estado == "Pendiente"){
                    mostrarPedido(pedido);
                }
            }
        }
        private static void mostrarPedido(Pedido pedido){
            Console.WriteLine("─────────────────────────────────────");
            Console.WriteLine($"Numero de pedido: {pedido.Nro}");
            Console.WriteLine($"Observacion: {pedido.Obs}");
            Console.WriteLine($"Estado: {pedido.Estado}");
            Console.WriteLine("─────────────────────────────────────");
        }
    }
}