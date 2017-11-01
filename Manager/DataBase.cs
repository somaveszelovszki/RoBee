using System.Collections.Generic;

namespace RoBee {
	namespace Manager {
		class Database {

			private static Database instance;

			public List<Drone> Drones { get; } = new List<Drone>();

			public List<FlowerField> FlowerFields { get; } = new List<FlowerField>();

			private Database() { }

			public static Database Instance {
				get {
					if(instance == null) instance = new Database();
					return instance;
				}
			}

			public void AddDrone(Drone drone) {
				Drones.Add(drone);
			}

			public void RemoveDrone(Drone drone) {
				Drones.Remove(drone);
			}

			public void AddFlowerField(FlowerField flowerField) {
				FlowerFields.Add(flowerField);
			}

			public void RemoveFlowerField(FlowerField flowerField) {
				FlowerFields.Remove(flowerField);
			}
		}
	}

}