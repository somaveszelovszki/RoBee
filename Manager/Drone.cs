using Microsoft.Azure.Devices;

namespace RoBee {
	namespace Manager {
		class Drone {
			public string Id { get; set; }

			public long TravelledDistance { get; set; }

			public static Drone FromDevice(Device device) {
				Drone drone = new Drone();
				drone.Id = device.Id;
				drone.TravelledDistance = 0L;
				return drone;
			}
		}
	}
}
