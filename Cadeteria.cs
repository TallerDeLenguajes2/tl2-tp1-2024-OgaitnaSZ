using EspacioCadete;
using EspacioPedido;

namespace EspacioCadeteria{
class Cadeteria{
    public string Nombre { get; private set; }
    public string Telefono { get; private set; }
    public List<Cadete> ListadoCadetes { get; private set; }
    public List<Pedido> ListadoPedidos { get; private set; }

    public Cadeteria(string nombre, string telefono){
        Nombre = nombre;
        Telefono = telefono;
        ListadoCadetes = new List<Cadete>();
    }

    public void ReasignarPedido(Cadete cadeteNuevo, Pedido pedido){
        pedido.cadete = cadeteNuevo;
    }

    public void GenerarInformeDeActividad(){
        double totalPedidos = 0;
        double totalGanado = 0;

        foreach (var cadete in ListadoCadetes){
            double jornal = JornalACobrar(cadete.Id);
            double numPedidos = jornal/500;
            Console.WriteLine($"Cadete: {cadete.Nombre}");
            Console.WriteLine($"Pedidos Entregados: {numPedidos}");
            Console.WriteLine($"Jornal: {jornal}");
            Console.WriteLine("─────────────────────────────────────");
            totalPedidos += numPedidos;
            totalGanado += jornal;
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
            if(pedido.cadete == cadete){
                jornal += 500;
            }
        }
        return jornal;
    }
    public void AsignarCadeteAPedido(int idCadete, int numPedido){   
        Cadete cadete = ObtenerCadetePorId(idCadete);
        foreach(Pedido pedido in ListadoPedidos){
            if(pedido.Nro == numPedido){
                pedido.cadete = cadete;
            }
        }
    }
    public void AgregarPedido(Pedido pedido){
        ListadoPedidos.Add(pedido);
    }
}
}