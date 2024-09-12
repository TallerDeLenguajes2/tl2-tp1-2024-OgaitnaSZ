using EspacioCadete;
using EspacioPedido;
using EspacioManejoArchivos;
using EspacioCliente;
using OpcionesMenu;

namespace EspacioCadeteria{
public class Cadeteria{
    public string Nombre { get; private set; }
    public string Telefono { get; private set; }
    public List<Cadete> ListadoCadetes { get; private set; }
    public List<Pedido> ListadoPedidos { get; private set; }

    public Cadeteria(string nombre, string telefono){
        Nombre = nombre;
        Telefono = telefono;
        ListadoCadetes = new List<Cadete>();
        ListadoPedidos = new List<Pedido>();
    }

    public void ReasignarPedido(int idCadete, int numPedido){
        Cadete cadete = ObtenerCadetePorId(idCadete);
        foreach(Pedido pedido in ListadoPedidos){
            if(pedido.Nro == numPedido){
                pedido.Cadete = cadete;
            }
        }
    }

    public void GenerarInformeDeActividad(int opcionDatos){
        if(ListadoPedidos.Count()>0){
            AccesoADatos accesoADatos;
            if(opcionDatos == 1){
                accesoADatos = new AccesoCSV();
                accesoADatos.GuardarInforme(this);
            }else{
                accesoADatos = new AccesoJSON();
                accesoADatos.GuardarInforme(this);
            }
        }else{
            Console.WriteLine("No hay pedidos realizados");
        }
    }

    public void EliminarCadete(int idCadete){
        Cadete cadete = ObtenerCadetePorId(idCadete);
        ListadoCadetes.Remove(cadete);
    }

    public Cadete ObtenerCadetePorId(int id){
        foreach (var cadete in ListadoCadetes){
            if(cadete.Id == id){
                Cadete cadeteEncontrado = cadete;
                return cadeteEncontrado;
            }
        }
        return null;
    }

    public double JornalACobrar(int idCadete){
        Cadete cadete = ObtenerCadetePorId(idCadete);
        int jornal = 0;
        foreach(Pedido pedido in ListadoPedidos){
            if(pedido.Cadete == cadete){
                jornal += 500;
            }
        }
        return jornal;
    }
    public void AsignarCadeteAPedido(int idCadete, int numPedido){   
        Cadete cadete = ObtenerCadetePorId(idCadete);
        foreach(Pedido pedido in ListadoPedidos){
            if(pedido.Nro == numPedido){
                pedido.Cadete = cadete;
            }
        }
    }
    public void AgregarPedido(){
        Pedido pedido;
        Console.Write("Ingrese el número del pedido: ");
        int nroPedido;
        bool control = true;

        do{
            if(int.TryParse(Console.ReadLine(), out nroPedido) && nroPedido > 0){
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
                Mensajes.mostrarCadetes(ListadoCadetes);
                while (controlCadete){
                    Console.Write("Ingrese el ID del cadete para asignar el pedido: ");
                    if (int.TryParse(Console.ReadLine(), out idCadete)){
                        Cadete cadete = ObtenerCadetePorId(idCadete);
                        if (cadete != null){
                            ListadoPedidos.Add(new Pedido(nroPedido, obs, cliente, cadete));
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
                Console.WriteLine("Ingrese un ID valido.");
            }
        }while(control);
    }
}
}