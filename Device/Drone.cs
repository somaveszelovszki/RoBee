using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RoBee.Utils;

namespace RoBee {
	namespace Device {
		public class Drone {
			public Drone(double sight,Location<double> pos, double speed, double range, DroneType type) {
				typeOfDrone = type;
				rangeOfDrone = range;
				speedOfDrone = speed;
                angleOfSight = sight;
				locationOfDrone = pos;
				locationToReach = new Location<double>();
                fF = new FlowerField(null,0.0);
			}
            public FlowerField fF { get; set; }
			public Location<double> locationOfDrone { get; set; }
			public double speedOfDrone { get; set; }
			public double rangeOfDrone { get; set; }
            public double angleOfSight { get; set; }
			public DroneType typeOfDrone { get; set; }
			public Location<double> locationToReach { get; set; }


			public double simulateMovementTime() {
				double distance = Math.Sqrt((locationToReach.X - locationOfDrone.X) * (locationToReach.X - locationOfDrone.X) + (locationToReach.Y - locationOfDrone.Y) * (locationToReach.Y - locationOfDrone.Y));
				double time = distance / speedOfDrone;
				locationOfDrone.X = locationToReach.X;
				locationOfDrone.Y = locationToReach.Y;
				return time;
			}
            public bool lookForFlowerField()
            {
                bool flowerFieldIsFound = false;
                Random rand = new Random();
                int findRate = 5;
                if (findRate > rand.Next(100))
                {
                    flowerFieldIsFound = true;
                    double flowerX = rand.NextDouble()*angleOfSight + locationOfDrone.X;
                    double flowerY = rand.NextDouble()*angleOfSight + locationOfDrone.Y;

                    Location<double> flowerFieldLocation = new Location<double>(flowerX, flowerY);
                    double flowerFieldArea = rand.NextDouble() * 2;

                    fF.Area = flowerFieldArea;
                    fF.Loc = flowerFieldLocation;

                }
                    return flowerFieldIsFound;
            }
            public void moveRandom(double timeMilisec)
            {
                Random rand = new Random();
                double distance = (timeMilisec / 1000) * speedOfDrone;
                double moveOnX = Math.Sqrt( (distance * distance) - rand.Next(0, (int)(distance * distance)) );
                double moveOnY = Math.Sqrt( (distance * distance) - (moveOnX * moveOnX) );
                int upDown = rand.Next(4);
                if( upDown == 0)
                {
                    locationOfDrone.X = locationOfDrone.X + moveOnX;
                    locationOfDrone.Y = locationOfDrone.Y + moveOnY;
                }
                if(upDown == 1)
                {
                    locationOfDrone.X = locationOfDrone.X - moveOnX;
                    locationOfDrone.Y = locationOfDrone.Y + moveOnY;
                }
                if (upDown == 2)
                {
                    locationOfDrone.X = locationOfDrone.X - moveOnX;
                    locationOfDrone.Y = locationOfDrone.Y - moveOnY;
                }
                if (upDown == 3)
                {
                    locationOfDrone.X = locationOfDrone.X + moveOnX;
                    locationOfDrone.Y = locationOfDrone.Y - moveOnY;
                }
            }
            public void setDestinationToHive()
            {
                locationToReach.X = 0;
                locationToReach.Y = 0;
            }
		}
	}
}