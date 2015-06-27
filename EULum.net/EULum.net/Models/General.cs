using System.Globalization;
using EuLum.net.Models.Enums;

namespace EuLum.net.Models
{
    public class General
    {
        private readonly decimal _distanceBetweenCPlanes;
        private readonly decimal _distanceBetweenLuminousIntensitiesPerCPlane;

        public General(string identification, TypeIndicator typeIndicator, SymmetryIndicator symmetryIndicator, int numberOfCPlanes, decimal distanceBetweenCPlanes, int luminousIntensitiesInEachCPlane, decimal distanceBetweenLuminousIntensitiesPerCPlane, string measurementReportNumber, string luminaireName, string luminaireNumber, string fileName, string dateUser)
        {
            Identification = identification;
            TypeIndicator = typeIndicator;
            SymmetryIndicator = symmetryIndicator;
            NumberOfCPlanes = numberOfCPlanes;
            _distanceBetweenCPlanes = distanceBetweenCPlanes;
            LuminousIntensitiesInEachCPlane = luminousIntensitiesInEachCPlane;
            _distanceBetweenLuminousIntensitiesPerCPlane = distanceBetweenLuminousIntensitiesPerCPlane;
            MeasurementReportNumber = measurementReportNumber;
            LuminaireName = luminaireName;
            LuminaireNumber = luminaireNumber;
            FileName = fileName;
            DateUser = dateUser;
        }

        public string Identification { get; private set; }

        public TypeIndicator TypeIndicator { get; private set; }

        public SymmetryIndicator SymmetryIndicator { get; private set; }

        public int NumberOfCPlanes { get; private set; }

        public string DistanceBetweenCPlanes
        {
            get
            {
                return _distanceBetweenCPlanes != 0.0M ? _distanceBetweenCPlanes.ToString(CultureInfo.InvariantCulture) : "non-equidistantly";
            }
        }

        public int LuminousIntensitiesInEachCPlane { get; private set; }

        public string DistanceBetweenLuminousIntensitiesPerCPlane
        {
            get
            {
                return _distanceBetweenLuminousIntensitiesPerCPlane != 0.0M ? _distanceBetweenLuminousIntensitiesPerCPlane.ToString(CultureInfo.InvariantCulture) : "non-equidistantly";
            }
        }

        public string MeasurementReportNumber { get; private set; }

        public string LuminaireName { get; private set; }

        public string LuminaireNumber { get; private set; }

        public string FileName { get; private set; }

        public string DateUser { get; private set; }
    }
}