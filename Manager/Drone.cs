using Microsoft.Azure.Devices;

namespace RoBee {
	namespace Manager {
		class Drone {
			public string Id { get; set; }

			/// <summary>
			/// Stores the time the drone has spent in the air. [sec]
			/// </summary>
			public long AirTime { get; set; }

			public static Drone FromDevice(Device device) {
				return new Drone(device.Id);
			}

			public Drone(string id = null, long airTime = 0L) {
				Id = id;
				AirTime = airTime;
			}
		}
	}
}
