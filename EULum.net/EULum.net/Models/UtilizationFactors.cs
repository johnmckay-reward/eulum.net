namespace EuLum.net.Models
{
    public class UtilizationFactors
    {
        public UtilizationFactors(decimal k060, decimal k080, decimal k1, decimal k125, decimal k15, decimal k2, decimal k25, decimal k3, decimal k4, decimal k5)
        {
            K060 = k060;
            K080 = k080;
            K1 = k1;
            K125 = k125;
            K15 = k15;
            K2 = k2;
            K25 = k25;
            K3 = k3;
            K4 = k4;
            K5 = k5;
        }

        public decimal K060 { get; private set; }

        public decimal K080 { get; private set; }

        public decimal K1 { get; private set; }

        public decimal K125 { get; private set; }

        public decimal K15 { get; private set; }

        public decimal K2 { get; private set; }

        public decimal K25 { get; private set; }

        public decimal K3 { get; private set; }

        public decimal K4 { get; private set; }

        public decimal K5 { get; private set; }
    }
}