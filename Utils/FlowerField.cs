namespace RoBee {
	namespace Utils {
		/// <summary>
		/// Contains data of a flower field where the drones will fly for pollinating the flowers.
		/// </summary>
		public class FlowerField {

			/// <summary>
			/// Location of the flower field (latitude, longitude).
			/// </summary>
			public Location<double> Loc { get; set; }
			public double Area { get; set; }

			/// <summary>
			/// Defines how much time is needed to pollinate 1 m^2 of flower field. [sec/m^2]
			/// </summary>
			public static readonly long POLLINATION_RATE = 180;

			public FlowerField(Location<double> loc = null, double area = 0.0) {
				Loc = loc;
				Area = area;
			}

			public long getPollinationTime() {
				return (long) (Area * POLLINATION_RATE);
			}

			public override string ToString() {
				return "FlowerField(" + "Loc: " + Loc.ToString() + ", " + "Area:" + Area.ToString() + ")";
			}
		}
	}
}

