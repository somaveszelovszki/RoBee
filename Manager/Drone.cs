using Microsoft.Azure.Devices;

namespace RoBee {
	namespace Manager {
		class Drone {
			public string Id { get; set; }

			public long TravelledDistance { get; set; }

			public static Drone FromDevice(Device device) {
				return new Drone(device.Id);
			}

			public Drone(string id = null, long travelledDistance = 0L) {
				Id = id;
				TravelledDistance = travelledDistance;
			}
		}
	}
}
