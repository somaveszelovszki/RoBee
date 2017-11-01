using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoBee {
	public class MessageContent {
		public string deviceId;
		public double windSpeed;
	}

	class Program {
		static DeviceClient deviceClient;
		static string iotHubUri = "{IOT_HUB_URI}";
		static string deviceKey = "{DEVICE_KEY}";
		static string deviceId = "{DEVICE_ID}";


		static void Main(string[] args) {
			Console.WriteLine("Szimulált eszköz\n");
			deviceClient =
				DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

			SendDeviceToCloudMessagesAsync();
			ReceiveC2dAsync();
			Console.ReadLine();

		}

		private static async void SendDeviceToCloudMessagesAsync() {
			Random rand = new Random();

			while(true) {
				double currentWindSpeed = 10 + rand.NextDouble() * 40;

				var messageContent = new MessageContent();
				messageContent.deviceId = deviceId;
				messageContent.windSpeed = currentWindSpeed;

				var messageString = JsonConvert.SerializeObject(messageContent);
				var message = new Message(Encoding.ASCII.GetBytes(messageString));

				await deviceClient.SendEventAsync(message);
				Console.WriteLine("{0} > Üzenet küldése: {1}", DateTime.Now, messageString);

				await Task.Delay(1000);
			}
		}

		private static async void ReceiveC2dAsync() {
			Console.WriteLine("\nÜzenetek fogadása az IoT Hub-tól");
			while(true) {
				Message receivedMessage = await deviceClient.ReceiveAsync();
				if(receivedMessage == null) continue;

				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.WriteLine("Üzenet érkezett: {0}", Encoding.ASCII.GetString(receivedMessage.GetBytes()));
				Console.ResetColor();

				await deviceClient.CompleteAsync(receivedMessage);
			}
		}


	}
}
