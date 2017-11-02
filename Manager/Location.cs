namespace Manager {
	class Location<T> {
		public T Lat { get; set; }
		public T Lon { get; set; }

		public Location() { }

		public Location(T lat, T lon) {
			Lat = lat;
			Lon = lon;
		}
	}
}
