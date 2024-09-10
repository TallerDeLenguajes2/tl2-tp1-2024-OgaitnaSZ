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
        public abstract void GuardarDatos(List<Cadete> cadetes);
    }

    public class AccesoCSV : AccesoADatos{
        private string RutaCadetes;
        private string RutaCadeteria;

        public AccesoCSV(string rutaCadetes, string rutaCadeteria){
            RutaCadetes = rutaCadetes;
            RutaCadeteria = rutaCadeteria;
        }

        public override List<Cadete> CargarDatos(){
            var cadetes = new List<Cadete>();

            if (File.Exists(RutaCadetes)){
                var lines = File.ReadAllLines(RutaCadetes);

                foreach (var line in lines.Skip(1)){
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

        public override void GuardarDatos(List<Cadete> cadetes){
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

            if (!Directory.Exists("datos")){
                Directory.CreateDirectory("datos");
            }

            if (!File.Exists(RutaCadetes+".csv")){
                csvCadetes.AppendLine("Cadete,Pedidos Entregados,Jornal,Fecha");
            }
            
            foreach (var item in informe){
                csvCadetes.AppendLine($"{item.NombreCadete},{item.PedidosEntregados},{item.Jornal},{fecha.ToShortDateString()}");
            }
            File.AppendAllText(RutaCadetes+".csv", csvCadetes.ToString());

            var csvCadeteria = new StringBuilder();

            if (!File.Exists(RutaCadeteria+".csv")) {
                csvCadeteria.AppendLine("Fecha,Total Ganado,Pedidos relizados,Promedio de pedidos por cadete");
            }

            Console.WriteLine(promedioPedidos);
            
            csvCadeteria.AppendLine($"{fecha.ToShortDateString()},{totalGanado},{totalPedidos},{promedioPedidos.ToString("F2", CultureInfo.InvariantCulture)}");  //Para convertir coma en punto
            File.AppendAllText(RutaCadeteria+".csv", csvCadeteria.ToString());
        }
    }

    public class AccesoJSON : AccesoADatos{
        private string RutaCadetes;
        private string RutaCadeteria;
        DateTime fecha = DateTime.Today;
        double totalPedidos = 0;
        double totalGanado = 0;

        public AccesoJSON(string rutaCadetes, string rutaCadeteria){
            RutaCadetes = rutaCadetes;
            RutaCadeteria = rutaCadeteria;
        }
        public override List<Cadete> CargarDatos(){
            if (File.Exists(RutaCadetes)){
                var jsonData = File.ReadAllText(RutaCadetes);
                return JsonSerializer.Deserialize<List<Cadete>>(jsonData);
            }else{
                Console.WriteLine("No se encontraron cadetes guardados");
            }

            return new List<Cadete>();
        }

        public override void GuardarDatos(List<Cadete> cadetes){
            List<InformeCadetes> listaCadetes = new();
            DateTime fecha = DateTime.Today;

            if (!Directory.Exists("datos")){
                Directory.CreateDirectory("datos");
            }

            if (File.Exists(RutaCadetes + ".json")){
                string jsonExistente = File.ReadAllText(RutaCadetes + ".json");
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
            File.WriteAllText(RutaCadetes+ ".json", jsonCadetes);


            //Guardar datos de cadeteria en JSON
            List<InformeCadeteria> informeCadeteria = new();
            float promedioPedidos = (float)Cadeteria.ListadoPedidos.Count / (float)Cadeteria.ListadoCadetes.Count;

            if (File.Exists(RutaCadeteria + ".json")){
                string jsonExistente = File.ReadAllText(RutaCadeteria + ".json");
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
            File.WriteAllText(RutaCadeteria +".json", jsonCadeteria);
        }
    }
}