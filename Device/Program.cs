using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Device;

namespace RoBee {
    namespace Device {
        public class MessageContent {

            /// <summary>
            /// allapotlekérdezéshez
            /// @droneType
            /// @deviceID
            /// @currentPosition
            /// 
            /// kommunikaciohoz
            /// @positionToReach
            /// @positionOfFlower
            /// </summary>

            public Location<double> positionToReach;
            public Location<double> positionOfFlower;

            public string droneType;
            public string deviceId;
            public Location<double> currentPosition;
        }

        class Program {
            static DeviceClient deviceClient;
            static string iotHubUri = "{IOT_HUB_URI}";
            static string deviceKey = "{DEVICE_KEY}";
            static string deviceId = "{DEVICE_ID}";
           
            static void Main(string[] args) {
                Location<double>startingLocation = new Location<double>();
                startingLocation.X = 0;
                startingLocation.Y = 0;

                Drone drone = new Drone(startingLocation,2,50,DroneType.Idle);

                //deviceClient =  DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                RunDeviceAsync(drone);
                ReceiveC2dAsync(drone);
                Console.ReadLine();
            }

            public static async void RunDeviceAsync(Drone d)
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
           
            //Todo üzenetek feldolgozása és átadása a drónnak
            private static async void ReceiveC2dAsync(Drone d)
            {
                Console.WriteLine("\nÜzenetek fogadása az IoT Hub-tól");
                while (true)
                {
                    Message receivedMessage = await deviceClient.ReceiveAsync();
                    if (receivedMessage == null) continue;


                    string data = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                    ///Itt felkéne okosan dolgozni ezt az üzenetet hogy kivehető legyen a virág pozíciója
                    ///vagy a felderítendő irány
                    ///beallit d.DroneType = kapott érték
                    ///beallit d.LocationToReach = kapott érték


                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Üzenet érkezett: {0}", Encoding.ASCII.GetString(receivedMessage.GetBytes()));
                    Console.ResetColor();
                    await deviceClient.CompleteAsync(receivedMessage);
                }
            }
            //TODO
            private static async void RunScoutingDrone(Drone d)
            {
                Console.WriteLine("Szimulalt Felderito Dron\n");
                while (DroneType.Scout.Equals(d.typeOfDrone))
                {

                }
                /// 1.Random generált vagy paraméterben kapott útvonal mentén feledríti a virágokat.
                /// 2.Ha virágot talált elküldi üzenetben a koordinátáit a kaptárba (1sec).
                /// 3.Folytatja a felderítést, szükség esetén feltölti magát.

            }
            //TODO
            private static async void RunPollinateDrone(Drone d)
            {
                Console.WriteLine("Szimulált Beporzo Dron\n");
                while (DroneType.Pollinate.Equals(d.typeOfDrone))
                {

                }
                /// 1.Paraméterben átadott virághoz indul koordinata alapján.
                /// 2.Ha oda ért vár 2sec-et (beporzás) és üzenetet küld  hogy megtörtént a beporzás.
                /// 3.Vissza indul a kaptárba.
            }
            //kész
            private static async void RunIdleDrone(Drone d)
            {
                /// 1.Ha a kaptárban (0;0) van, és nincs feladata akkor tölti magát .
                /// 2.Ha nincs a kaptárban akkor elindul az kaptár felé és tölt amíg feladatot nem kap.
                Console.WriteLine("Szimulált Piheno Dron\n");

                d.locationToReach.X = 0;
                d.locationToReach.Y = 0;
                
                    if ((d.locationToReach.X == d.locationOfDrone.X) && (d.locationToReach.Y == d.locationOfDrone.Y))
                    {
                        Console.WriteLine("A kaptarban voltam es toltodok\n");
                    }
                    if((d.locationToReach.X != d.locationOfDrone.X) || (d.locationToReach.Y != d.locationOfDrone.Y))
                    {
                        double waitTime = d.simulateMovementTime();
                        Console.WriteLine("A vissza indulok a kaptarba {0} masodperc mulva oda erek \n",waitTime);
                        await Task.Delay((int)waitTime * 1000);
                         Console.WriteLine("A kaptarba erkeztem toltodok\n");
                    }
            }

            //private static async void SendDeviceToCloudMessagesAsync() {
            //    Random rand = new Random();

            //    while (true) {
            //        double currentWindSpeed = 10 + rand.NextDouble() * 40;

            //        var messageContent = new MessageContent();
            //        messageContent.deviceId = deviceId;
            //        messageContent.windSpeed = currentWindSpeed;

            //        var messageString = JsonConvert.SerializeObject(messageContent);
            //        var message = new Message(Encoding.ASCII.GetBytes(messageString));

            //        await deviceClient.SendEventAsync(message);
            //        Console.WriteLine("{0} > Üzenet küldése: {1}", DateTime.Now, messageString);

            //        await Task.Delay(1000);
            //    }
            //}
        }
    }
}
