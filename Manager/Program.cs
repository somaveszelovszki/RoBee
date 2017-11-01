using Manager;

using Microsoft.Azure.Devices;
using Microsoft.ServiceBus.Messaging;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RoBee {
	namespace Manager {
		class Program {
			private static string connectionString = "HostName=RoBee.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=BDaizyrfny0c4I/Z3C6mRnzGS9L6xoBYvWJDdK5h0gU=";
			private static string iotHubD2cEndpoint = "messages/events";

			private static EventHubClient eventHubClient;
			private static RegistryManager registryManager = RegistryManager.CreateFromConnectionString(connectionString);
			private static ServiceClient serviceClient = ServiceClient.CreateFromConnectionString(connectionString);


			private static void AddDevice(string deviceId) {
				Device device = registryManager.AddDeviceAsync(new Device(deviceId) { Status = DeviceStatus.Enabled }).Result;
				Console.WriteLine("Generated key: {0}", device.Authentication.SymmetricKey.PrimaryKey);
				Database.Instance.AddDrone(Drone.FromDevice(device));
			}

			private static void InitMessageReceiving() {
				eventHubClient = EventHubClient.CreateFromConnectionString(connectionString, iotHubD2cEndpoint);

				var d2cPartitions = eventHubClient.GetRuntimeInformation().PartitionIds;

				var tasks = new List<Task>();
				foreach(string partition in d2cPartitions)
					tasks.Add(ReceiveMessagesFromDeviceAsync(partition));

				Task.WaitAll(tasks.ToArray());
			}

			private static void SendMessage(string deviceId, Message message) {
				serviceClient.SendAsync(deviceId, message);
			}

			private static void SendDroneTrajectory(string droneId, List<FlowerField> flowerFields) {
				List<Location<double>> locations = new List<Location<double>>();
				foreach(FlowerField field in flowerFields)
					locations.Add(field.Loc);


				string json = JsonConvert.SerializeObject(locations);
				Console.WriteLine("Sending message to '{0}': '{1}'", droneId, json);
				SendMessage(droneId, new Message(Encoding.ASCII.GetBytes(json)));
			}

			private static async Task ReceiveMessagesFromDeviceAsync(string partition) {
				var eventHubReceiver = eventHubClient.GetDefaultConsumerGroup().CreateReceiver(partition, DateTime.UtcNow);
				while(true) {
					EventData eventData = await eventHubReceiver.ReceiveAsync();
					if(eventData == null) continue;

					string data = Encoding.UTF8.GetString(eventData.GetBytes());
					Console.WriteLine("New message. Partition: {0} Data: '{1}'", partition, data);

					// adds new flower field to the database
					Location<double> newLoc = JsonConvert.DeserializeObject<Location<double>>(data);
					Database.Instance.AddFlowerField(new FlowerField(newLoc));
				}
			}

			static void Main(string[] args) {
				foreach(KeyValuePair<Drone, List<FlowerField>> entry in Trajectories.calculate())
					SendDroneTrajectory(entry.Key.Id, entry.Value);

				InitMessageReceiving();
			}
		}
	}
}