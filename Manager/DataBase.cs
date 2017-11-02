using System;
using System.Collections.Generic;

using RoBee.Utils;

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

			public void TestInit() {
				Drones.Add(new Drone("poll_drone_1"));
				Drones.Add(new Drone("poll_drone_2"));
				Drones.Add(new Drone("poll_drone_3"));

				FlowerFields.Add(new FlowerField(new Location<double>(1000.0, 1500.0), 60));
				FlowerFields.Add(new FlowerField(new Location<double>(2000.0, 500.0), 20));
				FlowerFields.Add(new FlowerField(new Location<double>(3000.0, 3500.0), 30));
				FlowerFields.Add(new FlowerField(new Location<double>(4000.0, -2000.0), 25));
				FlowerFields.Add(new FlowerField(new Location<double>(5000.0, -500.0), 15));
			}
		}
	}

}