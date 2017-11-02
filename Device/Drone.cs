using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RoBee.Utils;

namespace RoBee {
	namespace Device {
		public class Drone {
			public Drone(Location<double> pos, double speed, double range, DroneType type) {
				typeOfDrone = type;
				rangeOfDrone = range;
				speedOfDrone = speed;
				locationOfDrone = pos;
				locationToReach = new Location<double>();
			}
			public Location<double> locationOfDrone;
			public double speedOfDrone;
			public double rangeOfDrone;

			public DroneType typeOfDrone;

			public Location<double> locationToReach;
			public double simulateMovementTime() {
				double distance = Math.Sqrt((locationToReach.X - locationOfDrone.X) * (locationToReach.X - locationOfDrone.X) + (locationToReach.Y - locationOfDrone.Y) * (locationToReach.Y - locationOfDrone.Y));
				double time = distance / speedOfDrone;
				locationOfDrone.X = locationToReach.X;
				locationOfDrone.Y = locationToReach.Y;
				return time;
			}
		}
	}
}