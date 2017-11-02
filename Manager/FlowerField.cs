using Manager;

namespace RoBee {
	namespace Manager {
		/// <summary>
		/// Contains data of a flower field where the drones will fly for pollinating the flowers.
		/// </summary>
		class FlowerField {

			/// <summary>
			/// Location of the flower field (latitude, longitude).
			/// </summary>
			public Location<double> Loc { get; set; }

			public double Area { get; set; }

			public FlowerField(Location<double> loc = null, double area = 0.0) {
				Loc = loc;
				Area = area;
			}
		}
	}
}
