namespace NFApp.Dtos
{
    public class SencerValues
    {
        public SHT30 SHT30Sencer { get; set; }
        public BMP280 BMP280Sencer { get; set; }


        public class SHT30
        {
            public float Temperature { get; set; }
            public float Humidity { get; set; }
        };

        public class BMP280
        {
            public float Temperature { get; set; }
            public float Pressure { get; set; }
            public float Altitude { get; set; }
        }
    }
}
