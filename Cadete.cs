namespace EspacioCadete{
public class Cadete{
    public int Id { get; private set; }
    public string Nombre { get; private set; }
    public string Direccion { get; private set; }
    public string Telefono { get; private set; }

    public Cadete(int id, string nombre, string direccion, string telefono){
        Id = id;
        Nombre = nombre;
        Direccion = direccion;
        Telefono = telefono;
    }
}
}