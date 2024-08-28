# ¿Cuál de estas relaciones considera que se realiza por composición y cuál por agregación?
La relación entre **Pedidos** y **Cliente** es de composición porque un **Cliente** no existe sin un Pedido. 
Si un **Pedido** se elimina, el **Cliente** correspondiente también debe eliminarse.

La relación entre **Cadete** y **Pedidos** es de agregación, porque un **Cadete** puede existir independientemente de los **Pedidos** que tenga asignados. 
Los **Pedidos** pueden ser reasignados a otros cadetes sin que el **Cadete** sea eliminado.

La relación entre **Cadete** y **Cadeteria** es de composicion porque un **Cadete** pertenece al listado de cadetes de la **Cadeteria**.

# ¿Qué métodos considera que debería tener la clase Cadetería y la clase Cadete?
## Clase Cadete:
- **JornalACobrar():** Calcula el total a cobrar según la cantidad de pedidos entregados.
- **AsignarPedido(Pedido pedido):** Asigna un pedido al cadete.
- **EliminarPedido(Pedido pedido):** Elimina un pedido del cadete.
- **MostrarPedidos():** Muestra todos los pedidos asignados al cadete (pendientes y entregados).
- **PedidosEntregados():** Muestra la cantidad de pedidos entregados.
## Clase Cadeteria:
- **AsignarPedidoACadete(Cadete cadete, Pedido pedido):** Asigna un pedido a un cadete.
- **ReasignarPedido(Cadete cadeteActual, Cadete cadeteNuevo, Pedido pedido):** Reasigna un pedido de un cadete a otro.
- **GenerarInformeDeActividad():** Genera un informe de los pedidos realizados y el jornal de cada cadete.
- **AgregarCadete(Cadete cadete):** Añade un nuevo cadete a la cadetería.
- **EliminarCadete(Cadete cadete):** Elimina un cadete de la cadetería.

# Teniendo en cuenta los principios de abstracción y ocultamiento, que atributos, propiedades y métodos deberían ser públicos y cuáles privados.
## Públicos:
Los métodos que interactúan con otras clases o que son parte de la interfaz pública, como VerDireccionCliente(), JornalACobrar(), AsignarPedidoACadete(), y GenerarInformeDeActividad().
Los atributos que describen la identidad de las clases, como Nombre, Teléfono, Dirección.

## Privados:
Los atributos que deben ser manipulados internamente por la misma clase, como listas (listadoPedidos, listadoCadetes) y datos sensibles (DatosReferenciaDireccion).
Los métodos internos de manipulación de datos que no deberían ser expuestos, como la reasignación de pedidos entre cadetes.

# ¿Cómo diseñaría los constructores de cada una de las clases?
```
public Pedido(int nro, string obs, Cliente cliente){
    Nro = nro;
    Obs = obs;
    Cliente = cliente;
    Estado = "Pendiente";
}
public Cliente(string nombre, string direccion, string telefono, string datosReferenciaDireccion){
    Nombre = nombre;
    Direccion = direccion;
    Telefono = telefono;
    DatosReferenciaDireccion = datosReferenciaDireccion;
}
public Cadete(int id, string nombre, string direccion, string telefono){
    Id = id;
    Nombre = nombre;
    Direccion = direccion;
    Telefono = telefono;
    ListadoPedidos = new List<Pedido>();
}
public Cadeteria(string nombre, string telefono){
    Nombre = nombre;
    Telefono = telefono;
    ListadoCadetes = new List<Cadete>();
}
```
# ¿Se le ocurre otra forma que podría haberse realizado el diseño de clases?
Agregar una clase **Persona**, para que **Cadete** y **Cliente** hereden los atributos 'Nombre', 'Direccion' y 'Telefono', que tienen en común.
