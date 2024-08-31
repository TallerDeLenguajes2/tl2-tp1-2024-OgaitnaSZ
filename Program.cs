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
                cadeteria.GenerarInformeDeActividad();
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