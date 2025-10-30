using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class GestionFichero
{
    public static string Ruta { get; set; } = "../../../viajes.xml";
    public static List<User> Cargar()
    {
        if (!File.Exists(Ruta))
        {
            return new List<User>();
        }
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<User>),
            new Type[] { typeof(User)});
            Stream stream = new FileStream(Ruta, FileMode.Open);
            List<User> viajes = (List<User>)serializer.Deserialize(stream);
            stream.Close();
            return (viajes != null ? viajes : new List<User>());
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return new List<User>();
        }

    }
    public static void Guardar(List<User> viajes)
    {
        try
        {
            StreamWriter stream = new StreamWriter(Ruta);
            XmlSerializer Serializer = new XmlSerializer(typeof(List<User>),
            new Type[] { typeof(User)});
            Serializer.Serialize(stream, viajes);
            stream.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

    }
}