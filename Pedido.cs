using EspacioCadete;
using EspacioCliente;

namespace EspacioPedido{
class Pedido{
    public int Nro { get; private set; }
    public string Obs { get; private set; }
    public Cliente Cliente { get; private set; }
    public string Estado { get; private set; }
    public Cadete cadete { get; set; }

    public Pedido(int nro, string obs, Cliente cliente){
        Nro = nro;
        Obs = obs;
        Cliente = cliente;
        Estado = "Pendiente";
    }

    public void VerDireccionCliente(){
        Console.WriteLine($"Direccion del cliente: {Cliente.Direccion}");
    }

    public void VerDatosCliente(){
        Console.WriteLine($"Nombre: {Cliente.Nombre}, Telefono: {Cliente.Telefono}");
    }

    public void CambiarEstado(string nuevoEstado){
        Estado = nuevoEstado;
    }

    public static Pedido ObtenerPedidoPorNum(List<Pedido> pedidos, int num){
        foreach(Pedido pedido in pedidos){
            if(pedido.Nro == num){
                return pedido;
            }
        }
        return null;
    }
}
}