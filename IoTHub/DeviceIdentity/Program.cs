using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;
using System;
using System.Threading.Tasks;

namespace DeviceIdentity
{
    internal class Program
    {
        private static RegistryManager registryManager;
        private static string connectionString = "HostName=SmartCityHack.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=GOkuUw/NCHJFBT6LO08cA1rPh75CtYlkXEDNvN3V46A=";

        private static async Task AddDeviceAsync()

        {
            string deviceId = "myFirstSensor";
            Device device;
            try
            {
                device = await registryManager.AddDeviceAsync(new Device(deviceId));
            }
            catch (DeviceAlreadyExistsException)
            {
                device = await registryManager.GetDeviceAsync(deviceId);
            }
            Console.WriteLine("Generated device key: {0}", device.Authentication.SymmetricKey.PrimaryKey);
        }

        private static void Main(string[] args)
        {
            registryManager = RegistryManager.CreateFromConnectionString(connectionString);
            AddDeviceAsync().Wait();
            Console.ReadLine();
        }
    }
}