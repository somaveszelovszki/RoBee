using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RoBee.Utils;

namespace RoBee {
	namespace Manager {
		class Trajectory {
			public List<FlowerField> FlowerFields = new List<FlowerField>();
			public Location<double> Hive = Location<double>.ORIGO;

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
