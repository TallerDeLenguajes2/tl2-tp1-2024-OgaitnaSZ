using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EspacioCadete;
using EspacioCadeteria;
using EspacioCliente;
using EspacioPedido;
using EspacioManejoArchivos;

Cadeteria cadeteria = new Cadeteria("Cadeteria San Miguel", "381123123");
var cadetes = CargaDeArchivos.CargarCadetesDesdeCSV("cadetes.csv");

foreach (var cadete in cadetes){
    cadeteria.AgregarCadete(cadete);
}
bool continuar = true;
{
    Console.WriteLine("──────────────────────────────────");
    Console.WriteLine("Seleccione una opción:");
    Console.WriteLine("──────────────────────────────────");
    Console.WriteLine("1. Dar de alta pedidos");
    Console.WriteLine("2. Asignar pedido a cadete");
    Console.WriteLine("3. Cambiar estado de un pedido");
    Console.WriteLine("4. Reasignar pedido a otro cadete");
    Console.WriteLine("5. Generar informe de actividad");
    Console.WriteLine("6. Salir");
    Console.WriteLine("──────────────────────────────────");

    int opcion;
    if(int.TryParse(Console.ReadLine(), out opcion)){
        switch (opcion){
            case 1:
                Opciones.DarDeAltaPedido(cadeteria);
                break;
            case 2:
                //Opciones.AsignarPedido(cadeteria, pedidos);
                break;
            case 3: 
                Opciones.CambiarEstadoDePedido(cadeteria);
                break;
            case 4:
                Opciones.ReasignarPedidoAOtroCadete(cadeteria);
                break;
            case 5:
                cadeteria.GenerarInformeDeActividad();
                break;
            case 6:
                continuar = false;
                break;
            default:
                Console.WriteLine("Seleccione una opcion valida");
                break;
        }
    }else{
        Console.WriteLine("Seleccione una opcion valida");
    }
}