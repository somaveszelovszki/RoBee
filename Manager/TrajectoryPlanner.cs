using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoBee.Utils;

namespace RoBee {
	namespace Manager {
		class TrajectoryPlanner {

			private int dronesCount;
			private List<FlowerField> current = new List<FlowerField>();
			private List<FlowerField> best = new List<FlowerField>();

			public Dictionary<Drone, List<FlowerField>> calculate() {
				Dictionary<Drone, List<FlowerField>> trajectories = new Dictionary<Drone, List<FlowerField>>();

				dronesCount = Database.Instance.Drones.Count;



				return trajectories;
			}
		}
	}
}
