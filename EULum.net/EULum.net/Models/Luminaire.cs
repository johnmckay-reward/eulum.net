namespace EuLum.net.Models
{
    public class Luminaire
    {
        public Luminaire(decimal length, decimal width, decimal height, decimal areaLength, decimal areaWidth, decimal c0PlaneHeight, decimal c90PlaneHeight, decimal c180PlaneHeight, decimal c270PlaneHeight, decimal downwardFlux, decimal lightOutput, decimal intensityConversionFactor, decimal tilt)
        {
            Length = length;
            Width = width;
            Height = height;
            AreaLength = areaLength;
            AreaWidth = areaWidth;
            C0PlaneHeight = c0PlaneHeight;
            C90PlaneHeight = c90PlaneHeight;
            C180PlaneHeight = c180PlaneHeight;
            C270PlaneHeight = c270PlaneHeight;
            DownwardFlux = downwardFlux;
            LightOutput = lightOutput;
            IntensityConversionFactor = intensityConversionFactor;
            Tilt = tilt;
        }

        public decimal Length { get; private set; }

        public decimal Width { get; private set; }

        public decimal Height { get; private set; }

        public decimal AreaLength { get; private set; }

        public decimal AreaWidth { get; private set; }

        public decimal C0PlaneHeight { get; private set; }

        public decimal C90PlaneHeight { get; private set; }

        public decimal C180PlaneHeight { get; private set; }

        public decimal C270PlaneHeight { get; private set; }

        public decimal DownwardFlux { get; private set; }

        public decimal LightOutput { get; private set; }

        public decimal IntensityConversionFactor { get; private set; }

        public decimal Tilt { get; private set; }
    }
}