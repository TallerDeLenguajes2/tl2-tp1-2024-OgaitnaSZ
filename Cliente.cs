namespace EspacioCliente{
public class Cliente{
    public string Nombre { get; private set; }
    public string Direccion { get; private set; }
    public string Telefono { get; private set; }
    public string DatosReferenciaDireccion { get; private set; }

    public Cliente(string nombre, string direccion, string telefono, string datosReferenciaDireccion){
        Nombre = nombre;
        Direccion = direccion;
        Telefono = telefono;
        DatosReferenciaDireccion = datosReferenciaDireccion;
    }
}
}