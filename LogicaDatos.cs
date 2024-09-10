using System;
using System.Text;
using System.Text.Json;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using EspacioCadete;
using EspacioCadeteria;
using System.Globalization;

namespace EspacioManejoArchivos{
    public class InformeCadetes{
        public string NombreCadete{ get; set; }
        public double PedidosEntregados{ get; set; }
        public double JornalACobrar{ get; set; }
        public string Fecha{ get; set; }
        public InformeCadetes(string nombreCadete, double pedidosEntregados, double jornalACobrar, string fecha){
            NombreCadete = nombreCadete;
            PedidosEntregados = pedidosEntregados;
            JornalACobrar = jornalACobrar;
            Fecha = fecha;
        }
    }
    public class InformeCadeteria{
        public string Fecha{ get; set; }
        public double TotalGanado{ get; set; }
        public double TotalPedidos{ get; set; }
        public float PromedioPedidos{ get; set; }
        public InformeCadeteria(string fecha, double totalGanado, double totalPedidos, float promedioPedidos){
            Fecha = fecha;
            TotalGanado = totalGanado;
            TotalPedidos = totalPedidos;
            PromedioPedidos = promedioPedidos;
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

    public abstract class AccesoADatos{
        public abstract List<Cadete> CargarDatos();
        public abstract void GuardarInforme(List<Cadete> cadetes);
    }

    public class AccesoCSV : AccesoADatos{

        public override List<Cadete> CargarDatos(){
            List<Cadete> cadetes = new List<Cadete>();
            if(File.Exists("datos/cadetes.csv")){
                var lines = File.ReadAllLines("datos/cadetes.csv");
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

        public override void GuardarInforme(List<Cadete> cadetes){
            double totalPedidos = 0;
            double totalGanado = 0;
            DateTime fecha = DateTime.Today;
            float promedioPedidos = (float)Cadeteria.ListadoPedidos.Count / (float)Cadeteria.ListadoCadetes.Count;

            var informe = cadetes.Select(
                cadete =>{
                    double jornal = Cadeteria.JornalACobrar(cadete.Id);
                    double numPedidos = jornal / 500;
                    totalPedidos += numPedidos;
                    totalGanado += jornal;

                    return new{
                        NombreCadete = cadete.Nombre,
                        PedidosEntregados = numPedidos,
                        Jornal = jornal
                    };
                }).ToList();

            var csvCadetes = new StringBuilder();

            if (!Directory.Exists("informes")){
                Directory.CreateDirectory("informes");
            }

            if (!File.Exists("informes/informe-de-cadetes.csv")){
                csvCadetes.AppendLine("Cadete,Pedidos Entregados,Jornal,Fecha");
            }
            
            foreach (var item in informe){
                csvCadetes.AppendLine($"{item.NombreCadete},{item.PedidosEntregados},{item.Jornal},{fecha.ToShortDateString()}");
            }
            File.AppendAllText("informes/informe-de-cadetes.csv", csvCadetes.ToString());

            var csvCadeteria = new StringBuilder();

            if (!File.Exists("informes/informe-de-cadeteria.csv")) {
                csvCadeteria.AppendLine("Fecha,Total Ganado,Pedidos relizados,Promedio de pedidos por cadete");
            }

            Console.WriteLine(promedioPedidos);
            
            csvCadeteria.AppendLine($"{fecha.ToShortDateString()},{totalGanado},{totalPedidos},{promedioPedidos.ToString("F2", CultureInfo.InvariantCulture)}");  //Para convertir coma en punto
            File.AppendAllText("informes/informe-de-cadeteria.csv", csvCadeteria.ToString());
        }
    }

    public class AccesoJSON : AccesoADatos{
        double totalPedidos = 0;
        double totalGanado = 0;

        public override List<Cadete> CargarDatos(){
            List<Cadete> cadetes = new List<Cadete>();
            if(File.Exists("datos/cadetes.json")){
                string jsonExistente = File.ReadAllText("datos/cadetes.json");
                cadetes = JsonSerializer.Deserialize<List<Cadete>>(jsonExistente);
            }else{
                Console.WriteLine("No existe el archivo o la ruta es incorrecta");
            }
            return cadetes;
        }

        public override void GuardarInforme(List<Cadete> cadetes){
            List<InformeCadetes> listaCadetes = new();
            DateTime fecha = DateTime.Today;

            if (!Directory.Exists("informes")){
                Directory.CreateDirectory("informes");
            }

            if (File.Exists("informes/informe-de-cadeteria.json")){
                string jsonExistente = File.ReadAllText("informes/informe-de-cadeteria.json");
                listaCadetes = JsonSerializer.Deserialize<List<InformeCadetes>>(jsonExistente);
            }

            var informe = cadetes.Select(
                cadete =>{
                    double jornal = Cadeteria.JornalACobrar(cadete.Id);
                    double numPedidos = jornal / 500;
                    totalPedidos += numPedidos;
                    totalGanado += jornal;

                    return new{
                        NombreCadete = cadete.Nombre,
                        PedidosEntregados = numPedidos,
                        Jornal = jornal
                    };
                }).ToList();


            foreach (var item in informe){
                InformeCadetes nuevoInformeCadete = new InformeCadetes(
                    item.NombreCadete,
                    item.PedidosEntregados,
                    item.Jornal,
                    fecha.ToShortDateString()
                );
                
                listaCadetes.Add(nuevoInformeCadete);
            }

            string jsonCadetes = JsonSerializer.Serialize(listaCadetes, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("informes/informe-de-cadetes.json", jsonCadetes);


            //Guardar datos de cadeteria en JSON
            List<InformeCadeteria> informeCadeteria = new();
            float promedioPedidos = (float)Cadeteria.ListadoPedidos.Count / (float)Cadeteria.ListadoCadetes.Count;

            if (File.Exists("informes/informe-de-cadeteria.json")){
                string jsonExistente = File.ReadAllText("informes/informe-de-cadeteria.json");
                informeCadeteria = JsonSerializer.Deserialize<List<InformeCadeteria>>(jsonExistente);
            }

            InformeCadeteria informeNuevo = new InformeCadeteria(
                fecha.ToShortDateString(),
                totalGanado,
                totalPedidos,
                promedioPedidos
            ); 

            informeCadeteria.Add(informeNuevo);

            var jsonCadeteria = JsonSerializer.Serialize(informeCadeteria, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("informes/informe-de-cadeteria.json", jsonCadeteria);
        }
    }
}