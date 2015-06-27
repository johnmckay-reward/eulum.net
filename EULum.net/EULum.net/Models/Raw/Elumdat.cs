using System;
using System.Collections.Generic;

namespace EuLum.net.Models.Raw
{
    internal class Elumdat
    {
        readonly string _sIden;
        readonly int _iItyp;
        readonly int _iIsym;
        readonly int _iNc;
        readonly double _dDc;
        readonly int _iNg;
        readonly double _dDg;
        readonly string _sMrn;
        readonly string _sLnam;
        readonly string _sLnum;
        readonly string _sFnam;
        readonly string _sDate;   // 12
        readonly double _dL;
        readonly double _dB;
        readonly double _dH;
        readonly double _dLa;
        readonly double _dB1;
        readonly double _dHc0;
        readonly double _dHc90;
        readonly double _dHc180;
        readonly double _dHc270;
        readonly double _dDff; // 22
        readonly double _dLorl;
        readonly double _dCfli;
        readonly double _dTilt;
        readonly int _iN;
        readonly private IList<Lamp> _lamps;
        readonly double[] _dDr;
        readonly double[] _dC;
        readonly double[] _dG;
        readonly int[,] _dLcd;

        internal Elumdat(IList<string> fileData)
        {
            _sIden = fileData[0];
            _iItyp = Convert.ToInt32(fileData[1]);
            _iIsym = Convert.ToInt32(fileData[2]);
            _iNc = Convert.ToInt32(fileData[3]);
            _dDc = Convert.ToDouble(fileData[4]);
            _iNg = Convert.ToInt32(fileData[5]);
            _dDg = Convert.ToDouble(fileData[6]);
            _sMrn = fileData[7];
            _sLnam = fileData[8];
            _sLnum = fileData[9];
            _sFnam = fileData[10];
            _sDate = fileData[11];
            _dL = Convert.ToDouble(fileData[12]);
            _dB = Convert.ToDouble(fileData[13]);
            _dH = Convert.ToDouble(fileData[14]);
            _dLa = Convert.ToDouble(fileData[15]);
            _dB1 = Convert.ToDouble(fileData[16]);
            _dHc0 = Convert.ToDouble(fileData[17]);
            _dHc90 = Convert.ToDouble(fileData[18]);
            _dHc180 = Convert.ToDouble(fileData[19]);
            _dHc270 = Convert.ToDouble(fileData[20]);
            _dDff = Convert.ToDouble(fileData[21]);
            _dLorl = Convert.ToDouble(fileData[22]);
            _dCfli = Convert.ToDouble(fileData[23]);
            _dTilt = Convert.ToDouble(fileData[24]);

            _iN = Convert.ToInt32(fileData[25]);

            var currentCount = 25;

            _lamps = new List<Lamp>();

            for (int i = 0; i < _iN; i++)
            {
                var a = Convert.ToInt32(fileData[currentCount + 1]);
                var b = fileData[currentCount + 2];
                var c = Convert.ToDouble(fileData[currentCount + 3]);
                var d = fileData[currentCount + 4];
                var e = fileData[currentCount + 5];
                var f = Convert.ToDouble(fileData[currentCount + 6]);

                var lamp = new Lamp(a,b,c,d,e,f);

                currentCount += 7;
                _lamps.Add(lamp);
            }

            _dDr = new double[10];

            for (var i = 0; i < 10; i++)
            {
                _dDr[i] = Convert.ToDouble(fileData[currentCount]);
                currentCount++;
            }

            _dC = new double[_iNc];
            _dG = new double[_iNg];

            for (var i = 0; i < _iNc; i++)
            {
                _dC[i] = Convert.ToDouble(fileData[currentCount]);
                currentCount++;
            }
            for (var i = 0; i < _iNg; i++)
            {
                _dG[i] = Convert.ToDouble(fileData[currentCount]);
                currentCount++;
            }

            CalcMc();

            _dLcd = new int[Mc, _iNg];

            for (var i = 0; i < Mc; i++)
            {
                for (var j = 0; j < _iNg; j++)
                {
                    _dLcd[i, j] = Convert.ToInt32(fileData[currentCount]);
                    currentCount++;
                }
            }
        }

        internal string SIden
        {
            get { return _sIden; }
        }

        internal int Ityp
        {
            get { return _iItyp; }
        }

        internal int Isym
        {
            get { return _iIsym; }
        }

        internal int Nc
        {
            get { return _iNc; }
        }

        internal double DDc
        {
            get { return _dDc; }
        }

        internal int Ng
        {
            get { return _iNg; }
        }

        internal double DDg
        {
            get { return _dDg; }
        }

        internal string SMrn
        {
            get { return _sMrn; }
        }

        internal string SLnam
        {
            get { return _sLnam; }
        }

        internal string SLnum
        {
            get { return _sLnum; }
        }

        internal string SFnam
        {
            get { return _sFnam; }
        }

        internal string SDate
        {
            get { return _sDate; }
        }

        internal double Dl
        {
            get { return _dL; }
        }

        internal double Db
        {
            get { return _dB; }
        }

        internal double Dh
        {
            get { return _dH; }
        }

        internal double DLa
        {
            get { return _dLa; }
        }

        internal double Db1
        {
            get { return _dB1; }
        }

        internal double DHc0
        {
            get { return _dHc0; }
        }

        internal double DHc90
        {
            get { return _dHc90; }
        }

        internal double DHc180
        {
            get { return _dHc180; }
        }

        internal double DHc270
        {
            get { return _dHc270; }
        }

        internal double DDff
        {
            get { return _dDff; }
        }

        internal double DLorl
        {
            get { return _dLorl; }
        }

        internal double DCfli
        {
            get { return _dCfli; }
        }

        internal double DTilt
        {
            get { return _dTilt; }
        }

        internal int In
        {
            get { return _iN; }
        }

        internal IList<Lamp> Lamps
        {
            get { return _lamps; }
        }

        internal double[] DDr
        {
            get { return _dDr; }
        }

        internal double[] DC
        {
            get { return _dC; }
        }

        internal double[] DG
        {
            get { return _dG; }
        }

        internal int[,] DLcd
        {
            get { return _dLcd; }
        }

        internal int Mc { get; private set; }

        void CalcMc()
        {
            switch (_iIsym)
            {
                case 0: Mc = _iNc;
                    break;
                case 1: Mc = 1;
                    break;
                case 2:
                case 3: Mc = _iNc / 2 + 1;
                    break;
                case 4: Mc = _iNc / 4 + 1;
                    break;
            }
        }
    }
}