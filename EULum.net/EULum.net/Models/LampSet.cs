namespace EuLum.net.Models
{
    public class LampSet
    {
        public LampSet(int count, string type, decimal luminousFlux, string colorTemperature, string colorRenderingIndex, decimal wattage)
        {
            Count = count;
            Type = type;
            LuminousFlux = luminousFlux;
            ColorTemperature = colorTemperature;
            ColorRenderingIndex = colorRenderingIndex;
            Wattage = wattage;
        }

        public int Count { get; private set; }

        public string Type { get; private set; }

        public decimal LuminousFlux { get; private set; }

        public string ColorTemperature { get; private set; }

        public string ColorRenderingIndex { get; private set; }

        public decimal Wattage { get; private set; }
    }
}