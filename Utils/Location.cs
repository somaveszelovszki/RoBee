namespace RoBee {
	namespace Utils {
		/// <summary>
		/// Stores a location given by its X and Y coordinates.
		/// </summary>
		/// <typeparam name="T">Numeric type of the data (defines accuracy).</typeparam>
		public class Location<T> {
			public T X { get; set; }
			public T Y { get; set; }

			public Location() { }

			public Location(T x, T y) {
				X = x;
				Y = y;
			}

			/// <summary>
			/// Adds two locations. (Adds the X and Y coordinates.)
			/// </summary>
			/// <param name="other">The other location.</param>
			/// <returns>The result of the addition.</returns>
			public Location<T> Add(Location<T> other) {
				return new Location<T>((dynamic) X + other.X, (dynamic) Y + other.Y);
			}

			/// <summary>
			/// Subtracts the other location from this one. (Subtracts the X and Y coordinates.)
			/// </summary>
			/// <param name="other">The other location.</param>
			/// <returns>The result of the subtraction.</returns>
			public Location<T> Sub(Location<T> other) {
				return new Location<T>((dynamic) X - other.X, (dynamic) Y - other.Y);
			}
		}
	}
}