using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RoBee.Utils;

namespace RoBee {
    namespace Device {
        public class MessageContent {
            public FlowerField positionOfFlowerField;
            public string deviceId;
        }

        class Program {
            static DeviceClient deviceClient;
            static string iotHubUri = "RoBee.azure-devices.net";
            static string deviceKey = "E+UDcAQFQbTqXHCMxMX7kTFf1vSPynCzE4RPamf6JLA=";
            static string deviceId = "PollinateDrone1";
           
            static void Main(string[] args) {
                Location<double>startingLocation = new Location<double>();
                startingLocation.X = 0;
                startingLocation.Y = 0;
                Drone drone = new Drone(2,startingLocation,2,50,DroneType.Pollinate);
                deviceClient =  DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));
                RunDevice(drone);
                ReceiveC2dAsync(drone);
                Console.ReadLine();
            }

            public static void RunDevice(Drone d)
            {

                if (DroneType.Scout.Equals(d.typeOfDrone))
                {
                    RunScoutingDrone(d);
                }
                if (DroneType.Pollinate.Equals(d.typeOfDrone))
                {
                    RunPollinateDrone(d);
                }
                if (DroneType.Idle.Equals(d.typeOfDrone))
                {
                    RunIdleDrone(d);
                }
            }
            
            private static async void ReceiveC2dAsync(Drone d)
            {
                Console.WriteLine("\nUzenetek fogadasa az IoT Hub-tol");
                while (true)
                {
                    Message receivedMessage = await deviceClient.ReceiveAsync();
                    if (receivedMessage == null) continue;
                    string data = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                    FlowerField toReach = JsonConvert.DeserializeObject<FlowerField>(data);
                    d.locationToReach = toReach.Loc;
   
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Uzenet erkezett: {0}", Encoding.ASCII.GetString(receivedMessage.GetBytes()));
                    Console.ResetColor();
                    await deviceClient.CompleteAsync(receivedMessage);
                }
            }
            
            private static async void RunScoutingDrone(Drone d)
            {
                /// 1.Random generált útvonal mentén feledríti a virágokat.
                /// 2.Ha virágot talált elküldi üzenetben a koordinátáit a kaptárba (1sec).
                /// 3.Folytatja a felderítést.
                Console.WriteLine("Szimulalt Felderito Dron\n");
                while (DroneType.Scout.Equals(d.typeOfDrone))
                {
                   Console.WriteLine("{0} Viragmezot keresek", DateTime.Now);
                    bool flowerFound = d.lookForFlowerField();
                   if (flowerFound) {
                     var messageContent = new MessageContent();
                        messageContent.deviceId = deviceId;
                        messageContent.positionOfFlowerField = d.fF;
                        var messageString = JsonConvert.SerializeObject(messageContent);
                        var message = new Message(Encoding.ASCII.GetBytes(messageString));
                  
                        await deviceClient.SendEventAsync(message);
                        Console.WriteLine("{0} Viragmezot talaltam!", DateTime.Now);
                        Console.WriteLine("{0} > Uzenet kuldese: {1}", DateTime.Now, messageString);
                   }
                   flowerFound = false;
                   await Task.Delay(2000);
                   d.moveRandom(2000);
                   Console.WriteLine("{0} Nem talaltam mezot ezert tovabb kerestem x:{1} y:{2}", DateTime.Now, d.locationOfDrone.X,d.locationOfDrone.Y);
                }
            }
            
            private static async void RunPollinateDrone(Drone d)
            {
                /// 1.Paraméterben átadott virághoz indul koordinata alapján.
                /// 2.Ha oda ért vár 2sec-et (beporzás) és üzenetet küld  hogy megtörtént a beporzás.
                /// 3.Vissza indul a kaptárba.
                Console.WriteLine("Szimulált Beporzo Dron\n");
                if(DroneType.Pollinate.Equals(d.typeOfDrone))
                {
                    double movementTime= d.simulateMovementTime();
                    Console.WriteLine("Indulok a viraghoz, {0} masodperc\n", movementTime);
                    await Task.Delay((int)movementTime * 1000);
                    Console.WriteLine("Beporzok 2 masodperc\n");
                    await Task.Delay(2000);
                    Console.WriteLine("Vissza indulok a kaptarba \n");
                    d.setDestinationToHive();
                    movementTime = d.simulateMovementTime();
                    await Task.Delay((int)movementTime * 1000);
                    Console.WriteLine("A vissza indulok a kaptarba {0} masodperc mulva oda erek \n", movementTime);
                    Console.WriteLine("A kaptarba erkeztem toltodok\n");
                }
                
            }
            
            private static async void RunIdleDrone(Drone d)
            {
                /// 1.Ha a kaptárban (0;0) van, és nincs feladata akkor tölti magát .
                /// 2.Ha nincs a kaptárban akkor elindul az kaptár felé és tölt amíg feladatot nem kap.
                Console.WriteLine("Szimulált Piheno Dron\n");

                d.setDestinationToHive();
               
                    if ((d.locationToReach.X == d.locationOfDrone.X) && (d.locationToReach.Y == d.locationOfDrone.Y))
                    {
                        Console.WriteLine("A kaptarban voltam es toltodok\n");
                    }

                    if((d.locationToReach.X != d.locationOfDrone.X) || (d.locationToReach.Y != d.locationOfDrone.Y))
                    {
                        double movementTime = d.simulateMovementTime();
                        Console.WriteLine("A vissza indulok a kaptarba {0} masodperc mulva oda erek \n",movementTime);
                        await Task.Delay((int)movementTime * 1000);
                         Console.WriteLine("A kaptarba erkeztem toltodok\n");
                    }
            }

        }
    }
}
