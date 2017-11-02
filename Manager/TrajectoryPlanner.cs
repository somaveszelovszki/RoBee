using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoBee.Manager;

namespace Manager {
	class TrajectoryPlanner {

		private List<FlowerField> current = new List<FlowerField>();
		private List<FlowerField> best = new List<FlowerField>();

		public Dictionary<Drone, List<FlowerField>> calculate() {
			Dictionary<Drone, List<FlowerField>> trajectories = new Dictionary<Drone, List<FlowerField>>();

			// adds all flower fields ->only for testing!!!
			foreach(Drone drone in Database.Instance.Drones) {
				trajectories.Add(drone, Database.Instance.FlowerFields);
			}

			return trajectories;
		}
	}
}
