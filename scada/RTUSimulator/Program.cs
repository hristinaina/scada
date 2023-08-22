using RTUSimulator;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Random timeRandom = new Random();
        List<RTU> rtuData = new List<RTU>();
        List<Task> tasks = new List<Task>();

        rtuData.Add(new RTU("a1", 0, 100));
        rtuData.Add(new RTU("a2", 0, 100));
        rtuData.Add(new RTU("a3", -100, 50));
        rtuData.Add(new RTU("a4", -100, 50));
        rtuData.Add(new RTU("a5", -100, 50));

        rtuData.Add(new RTU("d1", 0, 1));
        rtuData.Add(new RTU("d2", 0, 1));
        rtuData.Add(new RTU("d3", 0, 1));
        rtuData.Add(new RTU("d4", 0, 1));
        rtuData.Add(new RTU("d5", 0, 1));


        foreach (RTU rtu in rtuData)
        {
            int awaitTime = timeRandom.Next(1, 4);
            tasks.Add(Task.Run(() => SimulateSensorAsync(rtu, awaitTime)));
        }

        await Task.WhenAll(tasks);
    }

    static async Task SimulateSensorAsync(RTU rtu, double awaitTime)
    {
        Random random = new Random();
        HttpClient httpClient = new HttpClient();

        while (true)
        {
            rtu.Value = random.Next(rtu.LowLimit, rtu.HighLimit + 1);

            try
            {
                var response = await httpClient.PostAsJsonAsync("http://localhost:5083/api/tag/rtu", rtu);
                response.EnsureSuccessStatusCode();
                Console.WriteLine($"Sensor data sent successfully - Sensor: {rtu.Address}, Value: {rtu.Value}, Await Time: {awaitTime}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending sensor data - Sensor: {rtu.Address}, Value: {rtu.Value}, Error: {ex.Message}");
            }

            await Task.Delay(TimeSpan.FromSeconds(awaitTime));
        }
    }
}
