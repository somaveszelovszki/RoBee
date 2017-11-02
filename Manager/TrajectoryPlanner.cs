using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoBee.Utils;

namespace RoBee {
	namespace Manager {
		class TrajectoryPlanner {

			private Trajectory[] current;
			private Trajectory[] best;
			private double bestMax;

			private int droneIdx, dronesCount;

			private void initCalc() {
				dronesCount = Database.Instance.Drones.Count;

				current = new Trajectory[dronesCount];
				best = new Trajectory[dronesCount];
				bestMax = double.PositiveInfinity;
				droneIdx = 0;

				for(int i = 0; i < dronesCount; ++i) {
					current[i] = new Trajectory();
					best[i] = new Trajectory();
				}
			}

			public Dictionary<Drone, Trajectory> calculate() {
				initCalc();

				Permute_Distribute_CheckBest(Database.Instance.FlowerFields.ToList());

				Dictionary<Drone, Trajectory> trajectories = new Dictionary<Drone, Trajectory>();

				List<Drone> orderedDroneList = Database.Instance.Drones.OrderBy(drone => drone.AirTime).ToList();

				List<Trajectory> orderedTrajectoryList = new List<Trajectory>(best).OrderByDescending(traj => traj.getExecTime()).ToList();

				for(int i = 0; i < orderedDroneList.Count; ++i)
					trajectories.Add(orderedDroneList.ElementAt(i), orderedTrajectoryList.ElementAt(i));

				return trajectories;
			}

			private Trajectory currentTraj = new Trajectory();

			private void Permute_Distribute_CheckBest(List<FlowerField> fields) {

				if (fields.Count > 0) {
					for(int i = 0; i < fields.Count; ++i) {
						FlowerField field = fields.ElementAt(i);
						fields.RemoveAt(i);
						currentTraj.FlowerFields.Add(field);

						Permute_Distribute_CheckBest(fields);

						fields.Insert(i, field);
						currentTraj.FlowerFields.Remove(field);
					}
				} else
					Distribute_CheckBest(currentTraj);		// check trajectory (runs for each permutation)
			}
						 
			private void Distribute_CheckBest(Trajectory traj) {
				int residualDronesCount = Math.Min(dronesCount - droneIdx, traj.FlowerFields.Count);
			
				if (residualDronesCount > 1) {
					int min = (traj.FlowerFields.Count - (residualDronesCount - 2) + 1) / 2,
					max = traj.FlowerFields.Count - (residualDronesCount - 1);

					for(int i = min; i <= max; ++i) {
						List<FlowerField> range = traj.FlowerFields.GetRange(0, i);
						traj.FlowerFields.RemoveRange(0, i);
						current[droneIdx++].FlowerFields.AddRange(range);

						Distribute_CheckBest(traj);

						traj.FlowerFields.InsertRange(0, range);
						current[--droneIdx].FlowerFields.RemoveRange(0, i);
					}
				} else {
					current[droneIdx].FlowerFields = traj.FlowerFields;
					CheckBest();
				}	
			}

			private void CheckBest() {
				double currentMax = current.Max(traj => traj.getExecTime());

				if (currentMax < bestMax) {
					bestMax = currentMax;
					for(int i = 0; i < dronesCount; ++i)
						best[i].FlowerFields = current[i].FlowerFields.ToList();
				}
			}
		}
	}
}
