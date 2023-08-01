using MySql.Data.MySqlClient;

namespace scada.Database
{
    public class DataAccess
    {
        private readonly MySqlConnection _connection;

        public DataAccess(MySqlConnection connection)
        {
            _connection = connection;
        }

        public void GetDataFromDatabase()
        {
            try
            {
                _connection.Open();
                string query = "SELECT * FROM testable";
                MySqlCommand command = new MySqlCommand(query, _connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    // Process the data
                    Console.WriteLine(reader.GetString(0));
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine("error!!!");
            }
        }
    }
}
