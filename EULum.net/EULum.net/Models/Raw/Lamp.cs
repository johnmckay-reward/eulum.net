namespace EuLum.net.Models.Raw
{
    internal class Lamp
    {
        readonly int _iNl;
        readonly string _sTl;
        readonly double _dTlf;
        readonly string _sCa;
        readonly string _sCrg;
        readonly double _dWb;

        internal int Nl
        {
            get { return _iNl; }
        }

        internal string STl
        {
            get { return _sTl; }
        }

        internal double DTlf
        {
            get { return _dTlf; }
        }

        internal string SCa
        {
            get { return _sCa; }
        }

        internal string SCrg
        {
            get { return _sCrg; }
        }

        internal double DWb
        {
            get { return _dWb; }
        }

        internal Lamp(int iNl, string sTl, double dTlf, string sCa, string sCrg, double dWb)
        {
            _sTl = sTl;
            _dTlf = dTlf;
            _sCa = sCa;
            _sCrg = sCrg;
            _dWb = dWb;
            _iNl = iNl;
        }
    }
}