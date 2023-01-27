namespace NFApp.Dtos
{
    public class SencerValues
    {
        public SHT30 SHT30Sencer { get; set; }
        public BMP280 BMP280Sencer { get; set; }


        public class SHT30
        {
            public double Temperature { get; set; }
            public double Humidity { get; set; }
        };

        public class BMP280
        {
            public double Temperature { get; set; }
            public double Pressure { get; set; }
            public double Altitude { get; set; }
        }
    }
}
