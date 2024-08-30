using EspacioCadete;
using EspacioCadeteria;
using EspacioCliente;
using EspacioPedido;
using EspacioManejoArchivos;

namespace EspacioManejoArchivos{
    class Opciones{
        public static void DarDeAltaPedido(Cadeteria cadeteria){
            Console.WriteLine("Ingrese el número del pedido:");
            int nroPedido;
            bool control = true;
            while(control){
                if(int.TryParse(Console.ReadLine(), out nroPedido)){
                    Console.WriteLine("Ingrese la observación del pedido:");
                    string obs = Console.ReadLine();
                    Console.WriteLine("Ingrese el nombre del cliente:");
                    string nombreCliente = Console.ReadLine();
                    Console.WriteLine("Ingrese la dirección del cliente:");
                    string direccionCliente = Console.ReadLine();
                    Console.WriteLine("Ingrese el teléfono del cliente:");
                    string telefonoCliente = Console.ReadLine();
                    Console.WriteLine("Ingrese los datos de referencia de la dirección:");
                    string datosReferencia = Console.ReadLine();
                    Cliente cliente = new Cliente(nombreCliente, direccionCliente, telefonoCliente, datosReferencia);
                    Pedido pedido = new Pedido(nroPedido, obs, cliente);
                    
                    int idCadete;
                    Console.WriteLine("Ingrese el ID del cadete para asignar el pedido:");
                    if(int.TryParse(Console.ReadLine(), out idCadete)){
                        Cadete cadete = cadeteria.ObtenerCadetePorId(idCadete);
                        pedido.cadete = cadete;
                        cadeteria.AgregarPedido(pedido);
                        Console.WriteLine("Pedido creado con éxito.");
                        control = false;
                    }else{
                        Console.WriteLine("Ingrese un ID valido");
                    }
                }else{
                    Console.WriteLine("Debe escribir un numero");
                }
            }
        }
        public static void CambiarEstadoDePedido(Cadeteria cadeteria){
            if(cadeteria.ListadoPedidos != null && cadeteria.ListadoPedidos.Count()>0){
                mostrarPedidos(cadeteria.ListadoPedidos);
                Console.WriteLine("Ingrese el numero del pedido:");
                int numPedido;

                if(int.TryParse(Console.ReadLine(), out numPedido)){
                    int encontrado = 0;
                    foreach(Pedido pedido in cadeteria.ListadoPedidos){
                        if(pedido.Nro == numPedido){
                            pedido.CambiarEstado("Entregado");
                            encontrado = 1;
                            break;
                        }
                    }
                    if(encontrado == 1){
                        Console.WriteLine("Pedido actualizado con exito");
                    }else{
                        Console.WriteLine("No se encontro el pedido");
                    }
                }else{
                    Console.WriteLine("Seleccione una opcion valida");
                }
            }else{
                Console.WriteLine("No hay pedidos pendientes");
            }
        }

        public static void ReasignarPedidoAOtroCadete(Cadeteria cadeteria){
            mostrarPedidos(cadeteria.ListadoPedidos);
            Console.WriteLine("Ingrese el numero de pedido a reasignar: ");
            int numPedido;
            if(int.TryParse(Console.ReadLine(), out numPedido)){
                int encontrado = 0;
                foreach(Pedido pedido in cadeteria.ListadoPedidos){
                    if(pedido.Nro == numPedido){
                        mostrarCadetes(cadeteria.ListadoCadetes);
                        Console.WriteLine("Ingrese ID del cadete a asignar: ");
                        int idCadete;
                        if(int.TryParse(Console.ReadLine(), out idCadete)){
                            Cadete cadete = cadeteria.ObtenerCadetePorId(idCadete);
                            cadeteria.ReasignarPedido(cadete, pedido);
                        }else{
                            Console.WriteLine("Seleccione una opcion valida");
                        }
                    }
                }
                if(encontrado == 1){
                    Console.WriteLine("Pedido reasignado con exito");
                }else{
                    Console.WriteLine("Error al reasignar pedido");
                }
            }else{
                Console.WriteLine("Seleccione una opcion valida");
            }
        }

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
        private static void mostrarPedidos(List<Pedido> pedidos){
            foreach(Pedido pedido in pedidos){
                mostrarPedido(pedido);
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

    class CargaDeArchivos{
        public static List<Cadete> CargarCadetesDesdeCSV(string ruta){
            List<Cadete> cadetes = new List<Cadete>();
            if(File.Exists(ruta)){
                var lines = File.ReadAllLines(ruta);
                foreach (var line in lines){ 
                    var values = line.Split(',');
                    int id = int.Parse(values[0]);
                    string nombre = values[1];
                    string direccion = values[2];
                    string telefono = values[3];

                    cadetes.Add(new Cadete(id, nombre, direccion, telefono));
                }
            }else{
                Console.WriteLine("No existe el archivo o la ruta es incorrecta");
            }
            return cadetes;
        }
    }
}