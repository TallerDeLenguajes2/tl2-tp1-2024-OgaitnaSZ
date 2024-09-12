using EspacioCadete;
using EspacioPedido;
using EspacioManejoArchivos;

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

    public void ReasignarPedido(Cadete cadeteNuevo, Pedido pedido){
        pedido.Cadete = cadeteNuevo;
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

    public void AgregarCadete(Cadete cadete){
        ListadoCadetes.Add(cadete);
    }

    public void EliminarCadete(Cadete cadete){
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

    //TP 2
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
    public void AgregarPedido(Pedido pedido){
        ListadoPedidos.Add(pedido);
    }

    public int contarPedidosPendientes(){
        int pedidosPendientes = 0;
        foreach(Pedido pedido in ListadoPedidos){
            if(pedido.Estado == "Pendiente"){
                pedidosPendientes++;
            }
        }
        return pedidosPendientes;
    }

    public bool pedidoExiste(int numPedido){
        bool existe = true;
        foreach(Pedido pedido in ListadoPedidos){
            if(pedido.Nro == numPedido){
                existe = false;
            }
        }
        return existe;
    }
}
}