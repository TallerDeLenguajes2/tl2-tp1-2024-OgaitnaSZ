using EspacioCadeteria;
using EspacioManejoArchivos;
using OpcionesMenu;
using EspacioManejoArchivos;
using EspacioCadete;

List<Cadete> cadetes = new();
Cadeteria cadeteria = new Cadeteria("","");

bool continuar = true;
int opcionDatos = 0;

Console.WriteLine("──────────────────────────────────");
Console.WriteLine("1. CSV");
Console.WriteLine("2. JSON");
Console.WriteLine("──────────────────────────────────");
Console.Write("Seleccione una opcion de datos para trabajar: ");
while(continuar){
    if(int.TryParse(Console.ReadLine(), out opcionDatos)){
        AccesoADatos accesoADatos;
        switch (opcionDatos){
            case 1:
                opcionDatos = 1;
                accesoADatos = new AccesoCSV();
                cadeteria = accesoADatos.CargarCadeteria();
                continuar = false;
                break;
            case 2: 
                opcionDatos = 2;
                accesoADatos = new AccesoJSON();
                cadeteria = accesoADatos.CargarCadeteria();
                continuar = false;
                break;
            default:
                Console.WriteLine("Seleccione una opcion valida");
                Thread.Sleep(1000);
                break;
        }
    }else{
        Console.WriteLine("Escriba un numero");
        Thread.Sleep(1000);
    }
}


continuar = true;
while(continuar){
    Console.WriteLine("──────────────────────────────────");
    Console.WriteLine("1. Dar de alta pedidos");
    Console.WriteLine("2. Cambiar estado de un pedido");
    Console.WriteLine("3. Reasignar pedido a otro cadete");
    Console.WriteLine("4. Generar informe de actividad");
    Console.WriteLine("5. Salir");
    Console.WriteLine("──────────────────────────────────");
    Console.Write("Seleccione una opción: ");

    int opcion;
    if(int.TryParse(Console.ReadLine(), out opcion)){
        switch (opcion){
            case 1:
                Opciones.DarDeAltaPedido(cadeteria);
                break;
            case 2: 
                Opciones.CambiarEstadoDePedido(cadeteria);
                break;
            case 3:
                Opciones.ReasignarPedidoAOtroCadete(cadeteria);
                break;
            case 4:
                cadeteria.GenerarInformeDeActividad(opcionDatos);
                break;
            case 5:
                continuar = false;
                break;
            default:
                Console.WriteLine("Seleccione una opcion valida");
                Thread.Sleep(1000);
                break;
        }
    }else{
        Console.WriteLine("Escriba un numero");
        Thread.Sleep(1000);
    }
}