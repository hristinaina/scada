using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var httpClient = new HttpClient();
        var sensorData = new { email = "pera@gmail.com", password = "123" }; // Example sensor data

        try
        {
            var response = await httpClient.PostAsJsonAsync("http://localhost:5083/api/user/login", sensorData); // Replace with your API endpoint
            response.EnsureSuccessStatusCode();
            Console.WriteLine("Sensor data sent successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending sensor data: {ex.Message}");
        }
    }
}
