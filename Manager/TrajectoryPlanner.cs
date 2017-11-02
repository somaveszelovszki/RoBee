using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoBee.Utils;

namespace RoBee {
	namespace Manager {
		/// <summary>
		/// Calculates best trajectory plan for the drones.
		/// Minimizes the maximum time a drone has to fly during the mission.
		/// Takes into consideration the time the drones have spent in the air until this mission,
		/// drones with less air time will be matched to the longer trajectories.
		/// </summary>
		class TrajectoryPlanner {

			/// <summary>
			/// Stores current trajectories during the iterations.
			/// </summary>
			private Trajectory[] current;

			/// <summary>
			/// Stores the best trajectory set.
			/// </summary>
			private Trajectory[] best;

			/// <summary>
			/// Stores the maximum air time of the best trajectory set.
			/// This is the value that will be minimized during the iterations.
			/// </summary>
			private double bestMax;

			/// <summary>
			/// Contains the index of the current drone, while iteration through the drones and matching them to trajectories.
			/// </summary>
			private int droneIdx;

			/// <summary>
			/// Stores current trajectory durring the iterations.
			/// </summary>
			private Trajectory currentTraj = new Trajectory();

			/// <summary>
			/// Initializes calculation variables.
			/// </summary>
			private void initCalc() {
				current = new Trajectory[Database.Instance.Drones.Count];
				best = new Trajectory[Database.Instance.Drones.Count];
				bestMax = double.PositiveInfinity;
				droneIdx = 0;

				for(int i = 0; i < Database.Instance.Drones.Count; ++i) {
					current[i] = new Trajectory();
					best[i] = new Trajectory();
				}
			}

			/// <summary>
			/// Calculates best trajectory set.
			/// </summary>
			/// <returns>The best trajectory set</returns>
			public Dictionary<Drone, Trajectory> calculate() {
				initCalc();

				// creates all permutations of the flower fields,
				// distributes them among the drones in all possible ways,
				// checks which trajectory set is the best and stores it
				Permute_Distribute_CheckBest(Database.Instance.FlowerFields.ToList());

								Dictionary<Drone, Trajectory> trajectories = new Dictionary<Drone, Trajectory>();

				// orders drones ascending according to their air time
				List<Drone> orderedDroneList = Database.Instance.Drones.OrderBy(drone => drone.AirTime).ToList();

				// orders trajectories descending according to their execution time
				List<Trajectory> orderedTrajectoryList = new List<Trajectory>(best).OrderByDescending(traj => traj.getExecTime()).ToList();

				// matches drones to trajectories - drones with less air time will be matched to the longer trajectories
				for(int i = 0; i < orderedDroneList.Count; ++i)
					trajectories.Add(orderedDroneList.ElementAt(i), orderedTrajectoryList.ElementAt(i));

				return trajectories;
			}

			/// <summary>
			/// Creates all permutations of the flower fields (these will be the trajectories),
			/// distributes them among the drones in all possible ways,
			/// checks which trajectory set is the best and stores it.
			/// </summary>
			/// <param name="fields">The flower fields</param>
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

			/// <summary>
			/// Distributes trajectory among the drones in all possible ways,
			/// checks which trajectory set is the best and stores it.
			/// </summary>
			/// <param name="traj">The trajectory</param>

			private void Distribute_CheckBest(Trajectory traj) {
				int residualDronesCount = Math.Min(Database.Instance.Drones.Count - droneIdx, traj.FlowerFields.Count);
			
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

			/// <summary>
			/// Checks if current trajectory set is better than the best, and updates best if needed.
			/// </summary>
			private void CheckBest() {
				double currentMax = current.Max(traj => traj.getExecTime());

				if (currentMax < bestMax) {
					bestMax = currentMax;
					for(int i = 0; i < Database.Instance.Drones.Count; ++i)
						best[i].FlowerFields = current[i].FlowerFields.ToList();
				}
			}
		}
	}
}
