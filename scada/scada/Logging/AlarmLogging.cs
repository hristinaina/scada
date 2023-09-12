using scada.Database;
using scada.Models;

namespace scada.Logging
{
    public class AlarmLogging
    {
        public void Logging(Alarm alarm, string tagName)
        {
            string filePath = "alarmsLog.txt";
            string header = "ALARM ID|TYPE|LIMIT|PRIORITY|DATE|TAG NAME\n";
            string tekst = alarm.ToString() + "|" + DateTime.Now.ToString() + "|" + tagName + "\n";

            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, header);
            }

            File.AppendAllText(filePath, tekst);
        }
    }
}
