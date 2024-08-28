using EspacioPedido;

namespace EspacioCadete{
class Cadete{
    public int Id { get; private set; }
    public string Nombre { get; private set; }
    public string Direccion { get; private set; }
    public string Telefono { get; private set; }
    public List<Pedido> ListadoPedidos { get; private set; }

    public Cadete(int id, string nombre, string direccion, string telefono){
        Id = id;
        Nombre = nombre;
        Direccion = direccion;
        Telefono = telefono;
        ListadoPedidos = new List<Pedido>();
    }

    public void AsignarPedido(Pedido pedido){
        ListadoPedidos.Add(pedido);
    }

    public void EliminarPedido(Pedido pedido){
        ListadoPedidos.Remove(pedido);
    }

    public double JornalACobrar(){
        int pedidosEntregados = 0;
        foreach (Pedido pedido in ListadoPedidos){
            if(pedido.Estado == "Entregado"){
                pedidosEntregados++;
            }
        }
        return pedidosEntregados * 500;
    }

    public void MostrarPedidos(){
        foreach (Pedido pedido in ListadoPedidos){
            Console.WriteLine($"Pedido Nro: {pedido.Nro} ───────────");
            Console.WriteLine($"Obs: {pedido.Obs}");
            Console.WriteLine($"Estado: {pedido.Estado}");
            Console.WriteLine("─────────────────────────────────────");
        }
    }
    public int PedidosEntregados(){
        int pedidosEntregados = 0;
        foreach(Pedido pedido in ListadoPedidos){
            if(pedido.Estado == "Entregado"){
                pedidosEntregados++;
            }
        }
        return pedidosEntregados;
    }
}
}