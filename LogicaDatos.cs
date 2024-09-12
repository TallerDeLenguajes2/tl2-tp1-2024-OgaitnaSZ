using System.Text;
using System.Text.Json;
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

    public abstract class AccesoADatos{
        public abstract Cadeteria CargarCadeteria();
        public abstract void GuardarInforme(Cadeteria cadeteria);
    }

    public class AccesoCSV : AccesoADatos{
        public override Cadeteria CargarCadeteria(){
            Cadeteria cadeteria = new Cadeteria("","");
            if(File.Exists("datos/cadetes.csv")){
                var lines = File.ReadAllLines("datos/cadeteria.csv");
                if(lines != null){
                    foreach (var line in lines){ 
                        var values = line.Split(',');
                        string nombre = values[0];
                        string telefono = values[1];
                        cadeteria = new Cadeteria(nombre,telefono);
                    }
                }else{
                    Console.WriteLine("El archivo esta danado.");
                }
            }else{
                Console.WriteLine("No existe el archivo o la ruta es incorrecta.");
            }

            //Agregar listado de cadetes
            if(File.Exists("datos/cadetes.csv")){
                var lines = File.ReadAllLines("datos/cadetes.csv");
                if(lines != null){
                    foreach (var line in lines){ 
                        var values = line.Split(',');
                        int id = int.Parse(values[0]);
                        string nombre = values[1];
                        string direccion = values[2];
                        string telefono = values[3];

                        cadeteria.ListadoCadetes.Add(new Cadete(id, nombre, direccion, telefono));
                    }
                }else{
                    Console.WriteLine("El archivo esta danado.");
                }
            }else{
                Console.WriteLine("No existe el archivo o la ruta es incorrecta");
            }
            return cadeteria;
        }

        public override void GuardarInforme(Cadeteria cadeteria){
            double totalPedidos = 0;
            double totalGanado = 0;
            DateTime fecha = DateTime.Today;
            float promedioPedidos = (float)cadeteria.ListadoPedidos.Count / (float)cadeteria.ListadoCadetes.Count;

            //Guardar informe de cadetes
            var informe = cadeteria.ListadoCadetes.Select(
                cadete =>{
                    double jornal = cadeteria.JornalACobrar(cadete.Id);
                    double numPedidos = jornal / 500;
                    totalPedidos += numPedidos;
                    totalGanado += jornal;

                    return new{
                        NombreCadete = cadete.Nombre,
                        PedidosEntregados = numPedidos,
                        Jornal = jornal
                    };
                }
            ).ToList();

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

            //Guardar informe de cadeteria
            var csvCadeteria = new StringBuilder();

            if (!File.Exists("informes/informe-de-cadeteria.csv")) {
                csvCadeteria.AppendLine("Fecha,Total Ganado,Pedidos relizados,Promedio de pedidos por cadete");
            }
            
            csvCadeteria.AppendLine($"{fecha.ToShortDateString()},{totalGanado},{totalPedidos},{promedioPedidos.ToString("F2", CultureInfo.InvariantCulture)}");  //Para convertir coma en punto
            File.AppendAllText("informes/informe-de-cadeteria.csv", csvCadeteria.ToString());
        }
    }

    public class AccesoJSON : AccesoADatos{
        double totalPedidos = 0;
        double totalGanado = 0;
        public override Cadeteria CargarCadeteria(){
            Cadeteria cadeteria = new Cadeteria("","");
            if(File.Exists("datos/cadeteria.json")){
                try{
                    string jsonExistente = File.ReadAllText("datos/cadeteria.json");
                    List<Cadeteria> ListaCadeteria = JsonSerializer.Deserialize<List<Cadeteria>>(jsonExistente);
                    cadeteria = ListaCadeteria[0];
                }catch(Exception ex){Console.WriteLine("Error al cargar datos de cadeteria: "+ex);}
            }else{
                Console.WriteLine("No existe el archivo o la ruta es incorrecta.");
            }

            //Cargar lista de cadetes

            List<Cadete> cadetes = new List<Cadete>();
            if(File.Exists("datos/cadetes.json")){
                try{
                    string jsonExistente = File.ReadAllText("datos/cadetes.json");
                    cadetes = JsonSerializer.Deserialize<List<Cadete>>(jsonExistente);
                }catch(Exception ex){Console.WriteLine("Error al cargar datos de cadeteria: "+ex);}
            }else{
                Console.WriteLine("No existe el archivo o la ruta es incorrecta.");
            }

            foreach(Cadete cadete in cadetes){
                cadeteria.ListadoCadetes.Add(cadete);
            }

            return cadeteria;
        }

        public override void GuardarInforme(Cadeteria cadeteria){
            List<InformeCadetes> listaCadetes = new();
            DateTime fecha = DateTime.Today;

            //Guardar informe de cadetes

            if (!Directory.Exists("informes")){
                Directory.CreateDirectory("informes");
            }

            if (File.Exists("informes/informe-de-cadetes.json")){
                try{
                    string jsonExistente = File.ReadAllText("informes/informe-de-cadetes.json");
                    listaCadetes = JsonSerializer.Deserialize<List<InformeCadetes>>(jsonExistente);
                }catch(Exception ex){Console.WriteLine("Error al cargar datos de cadetes: "+ex);}
            }

            var informe = cadeteria.ListadoCadetes.Select(
                cadete =>{
                    double jornal = cadeteria.JornalACobrar(cadete.Id);
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

            try{
                string jsonCadetes = JsonSerializer.Serialize(listaCadetes, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText("informes/informe-de-cadetes.json", jsonCadetes);
            }catch(Exception ex){Console.WriteLine("Error al guardar datos de cadetes: "+ex);}


            //Guardar informe de cadeteria en JSON
            List<InformeCadeteria> informeCadeteria = new();
            float promedioPedidos = (float)cadeteria.ListadoPedidos.Count / (float)cadeteria.ListadoCadetes.Count;

            if (File.Exists("informes/informe-de-cadeteria.json")){
                try{
                    string jsonExistente = File.ReadAllText("informes/informe-de-cadeteria.json");
                    informeCadeteria = JsonSerializer.Deserialize<List<InformeCadeteria>>(jsonExistente);
                }catch(Exception ex){Console.WriteLine("Error al cargar datos de cadeteria: "+ex);}
            }

            InformeCadeteria informeNuevo = new InformeCadeteria(
                fecha.ToShortDateString(),
                totalGanado,
                totalPedidos,
                promedioPedidos
            ); 

            informeCadeteria.Add(informeNuevo);

            try{
                var jsonCadeteria = JsonSerializer.Serialize(informeCadeteria, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText("informes/informe-de-cadeteria.json", jsonCadeteria);
            }catch(Exception ex){Console.WriteLine("Error al cargar datos de cadeteria: "+ex);}
        }
    }
}