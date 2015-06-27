using System.Collections.Generic;

namespace EuLum.net.Models
{
    public class Elumdat
    {
        public Elumdat(General general, Luminaire luminaire, IEnumerable<LampSet> lampSets, UtilizationFactors utilizationFactors, int[,] luminousIntensity)
        {
            LuminousIntensity = luminousIntensity;
            UtilizationFactors = utilizationFactors;
            Lamps = lampSets;
            Luminaire = luminaire;
            General = general;
        }

        public General General { get; private set; }
        public Luminaire Luminaire { get; private set; }
        public IEnumerable<LampSet> Lamps { get; private set; }
        public UtilizationFactors UtilizationFactors { get; private set; }
        public int[,] LuminousIntensity { get; private set; }
    }
}