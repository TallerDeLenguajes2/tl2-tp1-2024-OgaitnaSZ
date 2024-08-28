using EspacioCadete;
using EspacioPedido;

namespace EspacioCadeteria{
class Cadeteria{
    public string Nombre { get; private set; }
    public string Telefono { get; private set; }
    public List<Cadete> ListadoCadetes { get; private set; }

    public Cadeteria(string nombre, string telefono){
        Nombre = nombre;
        Telefono = telefono;
        ListadoCadetes = new List<Cadete>();
    }

    public void AsignarPedidoACadete(Cadete cadete, Pedido pedido){
        cadete.AsignarPedido(pedido);
    }

    public void ReasignarPedido(Cadete cadeteActual, Cadete cadeteNuevo, Pedido pedido){
        cadeteActual.EliminarPedido(pedido);
        cadeteNuevo.AsignarPedido(pedido);
    }

    public void GenerarInformeDeActividad(){
        int totalPedidos = 0;
        double totalGanado = 0;

        foreach (var cadete in ListadoCadetes){
            Console.WriteLine($"Cadete: {cadete.Nombre}");
            Console.WriteLine($"Pedidos Entregados: {cadete.PedidosEntregados()}");
            Console.WriteLine($"Jornal: {cadete.JornalACobrar()}");
            Console.WriteLine("─────────────────────────────────────");
            totalPedidos += cadete.PedidosEntregados();
            totalGanado += cadete.JornalACobrar();
        }

        double promedioPedidos = totalPedidos / (double)ListadoCadetes.Count;

        Console.WriteLine($"Total de Pedidos: {totalPedidos}, Total Ganado: {totalGanado}, Promedio de Pedidos por Cadete: {promedioPedidos}");
    }

    public void AgregarCadete(Cadete cadete){
        ListadoCadetes.Add(cadete);
    }

    public void EliminarCadete(Cadete cadete){
        ListadoCadetes.Remove(cadete);
    }

    public Cadete ObtenerCadetePorId(int id){
        var cadeteEncontrado = new Cadete(0, "null", "null", "null");
        foreach (var cadete in ListadoCadetes){
            if(cadete.Id == id){
                cadeteEncontrado = cadete;
            }
        }
        return cadeteEncontrado;
    }
}
}