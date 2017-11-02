using System;
using System.Collections.Generic;

using RoBee.Utils;

namespace RoBee {
	namespace Manager {
		/// <summary>
		/// Contains a virtual database with drones and flower fields in it.
		/// This is a Singleton class - use Instance property to access it.
		/// </summary>
		class Database {

			/// <summary>
			/// The Singleton instance.
			/// </summary>
			private static Database instance;

			/// <summary>
			/// Virtual table containing drones.
			/// </summary>
			public List<Drone> Drones { get; } = new List<Drone>();

			/// <summary>
			/// Virtual table containing flower fields.
			/// </summary>
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

			/// <summary>
			/// Fills virtual tables with test objects.
			/// </summary>
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