using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoBee
{
    class Program
    {
        static string connectionString = "{connectionString}";
        static RegistryManager registryManager = RegistryManager.CreateFromConnectionString(connectionString);
        static string deviceId = "{deviceId}";
        static void Main(string[] args)
        {
            Device device = registryManager.AddDeviceAsync(new Device(deviceId) { Status = DeviceStatus.Enabled }).Result;
            Console.WriteLine("Generált kulcs: {0}", device.Authentication.SymmetricKey.PrimaryKey);
            Console.ReadLine();
        }
    }
}
