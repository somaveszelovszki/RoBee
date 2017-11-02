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
				Drones.Add(new Drone("drone1", 1000L));
				Drones.Add(new Drone("drone2", 2000L));
				Drones.Add(new Drone("drone3", 3000L));

				FlowerFields.Add(new FlowerField(new Location<double>(100.0, 150.0)));
				FlowerFields.Add(new FlowerField(new Location<double>(200.0, 250.0)));
				FlowerFields.Add(new FlowerField(new Location<double>(300.0, 350.0)));
			}
		}
	}

}