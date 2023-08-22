using scada.Models;
using System.Xml.Serialization;

namespace scada.Data
{
    public class XmlSerializationHelper
    {

        public static List<T> LoadFromXml<T>()
        {
            String filePath = "Data/Config/config.xml";
            List<T> loadedObjects;
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>), new XmlRootAttribute("ArrayOfTag"));

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                loadedObjects = (List<T>)serializer.Deserialize(fileStream);
            }
            return loadedObjects;
        }

        public static void SaveToXml<T>(List<T> objects)
        {
            String filePath = "Data/Config/config.xml";
            // clearing xml file before writing anything to it
            File.WriteAllText(filePath, string.Empty);

            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            using (TextWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, objects);
            }
        }
    }
}


