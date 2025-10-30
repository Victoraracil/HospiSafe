using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class User
{
    public string ID { get; set; }
    public string Nombre { get; set; }
    public string Apellidos { get; set; }
    public string Rol { get; set; }
    public User(){}
    public User(string id, string nombre, string apellidos, string rol)
    {
        ID = id;
        Nombre = nombre;
        Apellidos = apellidos;
        Rol = rol;
    }

    public override string ToString()
    {
        return $"ID: {ID}, Nombre: {Nombre}, Apellidos: {Apellidos}, Rol: {Rol}";
    }
}