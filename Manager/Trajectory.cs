using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RoBee.Utils;

namespace RoBee {
	namespace Manager {
		/// <summary>
		/// Contains a trajectory object (an ordered list of flower fields).
		/// </summary>
		class Trajectory {
			/// <summary>
			/// List of flower fields in the trajectory.
			/// </summary>
			public List<FlowerField> FlowerFields = new List<FlowerField>();

			/// <summary>
			/// The Hive is where drones are stored. It is the starting and finishing point for all missions.
			/// </summary>
			public Location<double> Hive = Location<double>.ORIGO;

			/// <summary>
			/// Calculates execution time of the trajectory for a drone. It is based on the flower field distances, their area and the drones' speed.
			/// </summary>
			/// <returns>The calculated execution time.</returns>
			public long getExecTime() {
				long time = 0;
				Location<double> prevLoc = Hive;

				for(int i = 0; i < FlowerFields.Count; ++i) {
					FlowerField currentField = FlowerFields.ElementAt(i);
					time += Drone.getTravelTime(prevLoc, currentField.Loc) + currentField.getPollinationTime();
					prevLoc = currentField.Loc;
				}

				time += Drone.getTravelTime(prevLoc, Hive);

				return time;
			}

			public override string ToString() {
				return "Trajectory(\n"
					+ "\tFlowerFields:\n\t\t"
					+ string.Join(",\n\t\t", FlowerFields) + "\n"
					+ ")";
			}
		}
	}
}
