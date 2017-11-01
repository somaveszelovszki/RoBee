using Manager;

using Microsoft.Azure.Devices;
using Microsoft.ServiceBus.Messaging;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RoBee {
	namespace Manager {
		class Program {
			static string connectionString = "{connectionString}";
			static RegistryManager registryManager = RegistryManager.CreateFromConnectionString(connectionString);
			static string deviceId = "{deviceId}";

			static string iotHubD2cEndpoint = "messages/events";
			static EventHubClient eventHubClient;

			static void AddDevice() {
				Device device = registryManager.AddDeviceAsync(new Device(deviceId) { Status = DeviceStatus.Enabled }).Result;
				Console.WriteLine("Generált kulcs: {0}", device.Authentication.SymmetricKey.PrimaryKey);
				Database.Instance.AddDrone(Drone.FromDevice(device));
			}

			static void InitMessageReceiving() {
				Console.WriteLine("Üzenetek fogadása. Kilépés: Ctrl + C");
				eventHubClient = EventHubClient.CreateFromConnectionString(connectionString, iotHubD2cEndpoint);

				var d2cPartitions = eventHubClient.GetRuntimeInformation().PartitionIds;

				var tasks = new List<Task>();
				foreach(string partition in d2cPartitions) {
					tasks.Add(ReceiveMessagesFromDeviceAsync(partition));
				}
				Task.WaitAll(tasks.ToArray());

			}

			private static async Task ReceiveMessagesFromDeviceAsync(string partition) {
				var eventHubReceiver = eventHubClient.GetDefaultConsumerGroup().CreateReceiver(partition, DateTime.UtcNow);
				while(true) {
					EventData eventData = await eventHubReceiver.ReceiveAsync();
					if(eventData == null) continue;

					string data = Encoding.UTF8.GetString(eventData.GetBytes());
					Console.WriteLine("Üzenet érkezett. Partíció: {0} Adat: '{1}'", partition, data);
				}
			}

			static void Main(string[] args) { }
		}
	}
}