﻿using Microsoft.Azure.Devices;
using Microsoft.ServiceBus.Messaging;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using RoBee.Utils;

namespace RoBee {
	namespace Manager {
		/// <summary>
		/// The main program. The program is able to communicate with the drones:
		/// sends them trajectory plans (ordered list of flower fields) and receives locations of new flower fields.
		/// </summary>
		class Program {

			private static string connectionString = "HostName=RoBee.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=BDaizyrfny0c4I/Z3C6mRnzGS9L6xoBYvWJDdK5h0gU=";
			private static string iotHubD2cEndpoint = "messages/events";

			private static EventHubClient eventHubClient;
			private static RegistryManager registryManager = RegistryManager.CreateFromConnectionString(connectionString);
			private static ServiceClient serviceClient = ServiceClient.CreateFromConnectionString(connectionString);


			/// <summary>
			/// Adds a device (typically a drone) to the registry.
			/// NOTE: This function is not used during the execution of the main program!
			/// </summary>
			/// <param name="deviceId">The device id</param>
			private static void AddDevice(string deviceId) {
				Device device = registryManager.AddDeviceAsync(new Device(deviceId) { Status = DeviceStatus.Enabled }).Result;
				Console.WriteLine("Generated key: {0}", device.Authentication.SymmetricKey.PrimaryKey);
				Database.Instance.AddDrone(Drone.FromDevice(device));
			}

			/// <summary>
			/// Initializes message receiving from the Event Hub.
			/// </summary>
			private static void InitMessageReceiving() {
				eventHubClient = EventHubClient.CreateFromConnectionString(connectionString, iotHubD2cEndpoint);

				var d2cPartitions = eventHubClient.GetRuntimeInformation().PartitionIds;

				var tasks = new List<Task>();
				foreach(string partition in d2cPartitions)
					tasks.Add(ReceiveMessagesFromDeviceAsync(partition));

				Task.WaitAll(tasks.ToArray());
			}

			/// <summary>
			/// Sends message to a given device.
			/// </summary>
			/// <param name="deviceId">The device id</param>
			/// <param name="message">The message to send</param>
			private static void SendMessage(string deviceId, Message message) {
				serviceClient.SendAsync(deviceId, message);
			}

			/// <summary>
			/// Sends trajectory plan (list of flower fields) to a given drone.
			/// </summary>
			/// <param name="droneId">The drone id</param>
			/// <param name="flowerFields">The list of flower fields</param>
			private static void SendDroneTrajectory(string droneId, List<FlowerField> flowerFields) {
				List<Location<double>> locations = new List<Location<double>>();
				foreach(FlowerField field in flowerFields)
					locations.Add(field.Loc);

				// sends JSON-serialized object
				string json = JsonConvert.SerializeObject(locations);
				Console.WriteLine("Sending message to '{0}': '{1}'", droneId, json);
				SendMessage(droneId, new Message(Encoding.ASCII.GetBytes(json)));
			}

			/// <summary>
			/// Receives messages from ther drones.
			/// These messages contain new flower fields that need to be added to the database.
			/// </summary>
			/// <param name="partition">The partition id.</param>
			/// <returns>A task.</returns>
			private static async Task ReceiveMessagesFromDeviceAsync(string partition) {
				var eventHubReceiver = eventHubClient.GetDefaultConsumerGroup().CreateReceiver(partition, DateTime.UtcNow);
				while(true) {
					EventData eventData = await eventHubReceiver.ReceiveAsync();
					if(eventData == null) continue;

					string data = Encoding.UTF8.GetString(eventData.GetBytes());
					Console.WriteLine("New message. Partition: {0} Data: '{1}'", partition, data);

					// deserializes flower field and adds it to the database
					Location<double> newLoc = JsonConvert.DeserializeObject<Location<double>>(data);
					Database.Instance.AddFlowerField(new FlowerField(newLoc));
				}
			}

			/// <summary>
			/// Sends trajectory plans to drones and initializes message receiving.
			/// </summary>
			/// <param name="args"></param>
			static void Main(string[] args) {
				Database.Instance.TestInit();
				foreach(KeyValuePair<Drone, List<FlowerField>> entry in new TrajectoryPlanner().calculate())
					SendDroneTrajectory(entry.Key.Id, entry.Value);

				InitMessageReceiving();
			}
		}
	}
}