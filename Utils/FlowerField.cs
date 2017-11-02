namespace RoBee {
	namespace Utils {
		/// <summary>
		/// Contains data of a flower field where the drones will fly for pollinating the flowers.
		/// </summary>
		class FlowerField {

			/// <summary>
			/// Location of the flower field (latitude, longitude).
			/// </summary>
			public Location<double> Loc { get; set; }
			public double Area { get; set; }

			/// <summary>
			/// Defines how much time is needed to pollinate 1 m^2 of flower field. [sec/m^2]
			/// </summary>
			public static readonly double POLLINATION_RATE = 180.0;

			public FlowerField(Location<double> loc = null, double area = 0.0) {
				Loc = loc;
				Area = area;
			}
		}
	}
}

