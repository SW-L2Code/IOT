using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Simulator
{
    internal class Program
    {
        private static DeviceClient deviceClient;
        private static string iotHubUri = "SmartCityHack.azure-devices.net";
        private static string deviceKey = "sxqo2ubOGztF9aWdz+NULVLdCGiwygn7wU50o5Y5hXY=";

        private static async void SendDeviceToCloudMessagesAsync()
        {
            double minCO2Emission = 5.0;
            double minPopulation = 1326801576;
            int messageId = 1;
            Random rand = new Random();

            while (true)
            {
                double currentCO2Emission = minCO2Emission + rand.NextDouble() * 8.91;
                double currentPopulation = minPopulation + rand.NextDouble() * 2.43;

                var telemetryDataPoint = new
                {
                    messageId = messageId++,
                    deviceId = "myFirstSensor",
                    C02Emission = currentCO2Emission,
                    Population = currentPopulation
                };
                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));
                message.Properties.Add("emissionAlert", (currentCO2Emission > 30) ? "true" : "false");

                await deviceClient.SendEventAsync(message);
                Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

                await Task.Delay(1000);
            }
        }

        private static void Main(string[] args)
        {
            Console.WriteLine("Simulated device\n");
            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("myFirstSensor", deviceKey), TransportType.Mqtt);

            SendDeviceToCloudMessagesAsync();
            Console.ReadLine();
        }
    }
}