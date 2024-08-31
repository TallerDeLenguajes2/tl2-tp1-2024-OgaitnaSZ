using System;
using System.IO;
using System.Linq;
using System.Text;
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
        ListadoPedidos = new List<Pedido>();
    }

    public void ReasignarPedido(Cadete cadeteNuevo, Pedido pedido){
        pedido.Cadete = cadeteNuevo;
    }

    public void GenerarInformeDeActividad(){
        if(ListadoPedidos.Count()>0){
            double totalPedidos = 0;
            double totalGanado = 0;

            var informe = ListadoCadetes.Select(
                cadete =>{
                    double jornal = JornalACobrar(cadete.Id);
                    double numPedidos = jornal / 500;
                    totalPedidos += numPedidos;
                    totalGanado += jornal;

                    return new{
                        NombreCadete = cadete.Nombre,
                        PedidosEntregados = numPedidos,
                        Jornal = jornal
                    };
                }).ToList();

            double promedioPedidos = totalPedidos / (double)ListadoCadetes.Count;

            DateTime fecha = DateTime.Today;
            var csvCadetes = new StringBuilder();
            csvCadetes.AppendLine("Cadete,Pedidos Entregados,Jornal,Fecha");

            foreach (var item in informe){
                csvCadetes.AppendLine($"{item.NombreCadete},{item.PedidosEntregados},{item.Jornal},{fecha.ToShortDateString()}");
            }

            File.WriteAllText("Informe-de-cadetes.csv", csvCadetes.ToString());

            var csvCadeteria = new StringBuilder();
            csvCadeteria.AppendLine("Fecha,Total Ganado,Pedidos relizados,Promedio de pedidos por cadete");
            csvCadeteria.AppendLine($"{fecha.ToShortDateString()},{totalGanado},{totalPedidos},{promedioPedidos}");
            File.WriteAllText("Informe-de-cadeteria.csv", csvCadeteria.ToString());

            Console.WriteLine($"Total de Pedidos: {totalPedidos}, Total Ganado: {totalGanado}, Promedio de Pedidos por Cadete: {promedioPedidos}");
        }else{
            Console.WriteLine("No hay pedidos");
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
}
}