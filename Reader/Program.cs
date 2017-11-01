using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RoBee
{
    class Program
    {
        static string connectionString = "connectionString";
        static string iotHubD2cEndpoint = "messages/events";
        static EventHubClient eventHubClient;

        static void Main(string[] args)
        {
            Console.WriteLine("Üzenetek fogadása. Kilépés: Ctrl + C");
            eventHubClient = EventHubClient.CreateFromConnectionString(connectionString, iotHubD2cEndpoint);

            var d2cPartitions = eventHubClient.GetRuntimeInformation().PartitionIds;

            var tasks = new List<Task>();
            foreach (string partition in d2cPartitions)
            {
                tasks.Add(ReceiveMessagesFromDeviceAsync(partition));
            }
            Task.WaitAll(tasks.ToArray());

        }

        private static async Task ReceiveMessagesFromDeviceAsync(string partition)
        {
            var eventHubReceiver =  eventHubClient.GetDefaultConsumerGroup().CreateReceiver(partition, DateTime.UtcNow);
            while (true)
            {
                EventData eventData = await eventHubReceiver.ReceiveAsync();
                if (eventData == null) continue;

                string data = Encoding.UTF8.GetString(eventData.GetBytes());
                Console.WriteLine("Üzenet érkezett. Partíció: {0} Adat: '{1}'", partition, data);
            }
        }

    }
}
