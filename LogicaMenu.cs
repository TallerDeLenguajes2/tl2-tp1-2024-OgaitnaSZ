using EspacioCadete;
using EspacioCadeteria;
using EspacioCliente;
using EspacioPedido;

namespace OpcionesMenu{
        class Opciones{
        public static void DarDeAltaPedido(Cadeteria cadeteria){
            cadeteria.AgregarPedido();
        }
        
        public static void CambiarEstadoDePedido(Cadeteria cadeteria){
            if(cadeteria.ListadoPedidos != null && cadeteria.ListadoPedidos.Count()>0){
                Mensajes.mostrarPedidos(cadeteria.ListadoPedidos);
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
            if(cadeteria.ListadoPedidos != null && cadeteria.ListadoPedidos.Count()>0){
                Mensajes.mostrarPedidos(cadeteria.ListadoPedidos);
                int numPedido;
                bool pedidoEncontrado = true;
                Console.Write("Ingrese el numero de pedido a reasignar: ");
                while(pedidoEncontrado){
                    if(int.TryParse(Console.ReadLine(), out numPedido)){
                        foreach(Pedido pedido in cadeteria.ListadoPedidos){
                            if(pedido.Nro == numPedido){
                                Mensajes.mostrarCadetes(cadeteria.ListadoCadetes);
                                Console.Write("Ingrese ID del cadete a asignar: ");
                                int idCadete;
                                bool cadeteEncontrado = true;
                                while(cadeteEncontrado){
                                    if(int.TryParse(Console.ReadLine(), out idCadete)){
                                        Cadete cadete = cadeteria.ObtenerCadetePorId(idCadete);
                                        if(cadete != null){
                                            cadeteria.ReasignarPedido(cadete.Id, pedido.Nro);
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
    }
    public static class Mensajes{
        public static void mostrarCadetes(List<Cadete> cadetes){
            foreach(Cadete cadete in cadetes){
                mostrarCadete(cadete);
            }
        }
        public static void mostrarCadete(Cadete cadete){
            Console.WriteLine("─────────────────────────────────────");
            Console.WriteLine($"ID: {cadete.Id}");
            Console.WriteLine($"Nombre: {cadete.Nombre}");
            Console.WriteLine("─────────────────────────────────────");
        }
        public static void mostrarPedidos(List<Pedido> pedidos){
            foreach(Pedido pedido in pedidos){
                mostrarPedido(pedido);
            }
        }
        public static void mostrarPedido(Pedido pedido){
            Console.WriteLine("─────────────────────────────────────");
            Console.WriteLine($"Numero de pedido: {pedido.Nro}");
            Console.WriteLine($"Observacion: {pedido.Obs}");
            Console.WriteLine($"Estado: {pedido.Estado}");
            Console.WriteLine("─────────────────────────────────────");
        }
    }
}