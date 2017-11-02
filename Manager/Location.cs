namespace Manager {
	/// <summary>
	/// Stores a location given by its X and Y coordinates.
	/// </summary>
	/// <typeparam name="T">Numeric type of the data (defines accuracy).</typeparam>
	class Location<T> {
		public T X { get; set; }
		public T Y { get; set; }

		public Location() { }

		public Location(T x, T y) {
			X = x;
			Y = y;
		}
	}
}
