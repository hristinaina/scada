using System.Xml.Serialization;

namespace scada.Data
{
    public class XmlSerializationHelper
    {

        public static List<T> LoadFromXml<T>(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));

            using (TextReader reader = new StreamReader(filePath))
            {
                return (List<T>)serializer.Deserialize(reader);
            }
        }

        public static void SaveToXml<T>(List<T> objects, string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));

            using (TextWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, objects);
                //writer.Flush();
            }
        }
    }
}


