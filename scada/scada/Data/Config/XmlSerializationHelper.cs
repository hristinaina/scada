using System.Xml.Serialization;

namespace scada.Data
{
    public class XmlSerializationHelper
    {

        private static String _filePath = "Data/Config/config.xml";
        private static object fileLock = new object(); // shared lock object


        public static List<T> LoadFromXml<T>()
        {
            List<T> loadedObjects;
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>), new XmlRootAttribute("ArrayOfTag"));

            lock (fileLock)
            {
                using (FileStream fileStream = new FileStream(_filePath, FileMode.Open))
                {
                    loadedObjects = (List<T>)serializer.Deserialize(fileStream);
                }
            }
            return loadedObjects;
        }

        public static void SaveToXml<T>(List<T> objects)
        {
            lock (fileLock)
            {
                // clearing xml file before writing anything to it
                File.WriteAllText(_filePath, string.Empty);

                XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
                using (TextWriter writer = new StreamWriter(_filePath))
                {
                    serializer.Serialize(writer, objects);
                }
            }
        }
    }
}


