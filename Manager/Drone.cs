﻿using Microsoft.Azure.Devices;

using RoBee.Utils;

namespace RoBee {
	namespace Manager {
		class Drone {

			/// <summary>
			/// The speed of the drones. [m/sec]
			/// </summary>
			public static readonly double SPEED = 10.0;

			public string Id { get; set; }

			/// <summary>
			/// Stores the time the drone has spent in the air. [sec]
			/// </summary>
			public long AirTime { get; set; }

			/// <summary>
			/// Creates new Drone object from Device.
			/// </summary>
			/// <param name="device">The device.</param>
			/// <returns>The new Drone object.</returns>
			public static Drone FromDevice(Device device) {
				return new Drone(device.Id);
			}

			public Drone(string id = null, long airTime = 0L) {
				Id = id;
				AirTime = airTime;
			}

			/// <summary>
			/// Calculates travel time from two locations.
			/// </summary>
			/// <param name="start">The start location.</param>
			/// <param name="end">The end location.</param>
			/// <returns>The travel time.</returns>
			public double getTravelTime(Location<double> start, Location<double> end) {
				return end.Sub(start).distanceFrom(Location<double>.ORIGO) / SPEED;
			}
		}
	}
}
