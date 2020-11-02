using System;
using System.Collections.Generic;
using System.Text;

namespace SAC
{
    public class SacConstants
    {
        public static bool isUndef(float f)
        {
            return f == FLOAT_UNDEF;
        }

        public static bool isUndef(int i)
        {
            return i == INT_UNDEF;
        }

        public static bool isUndef(string s)
        {
            return (s.Length == 8 && s.Equals(STRING8_UNDEF)) || (s.Length == 16 && s.Equals(STRING16_UNDEF));
        }

        // undef values for sac
        public const float FLOAT_UNDEF = -12345.0f;

        public const int INT_UNDEF = -12345;

        public const string STRING8_UNDEF = "-12345  ";

        public const string STRING16_UNDEF = "-12345          ";

        public const int DEFAULT_NVHDR = 6;

        /* TRUE and FLASE defined for convenience. */
        public const int TRUE = 1;

        public const int FALSE = 0;

        /* Constants used by sac. This corresponds to utils/sac.h in sac 101.4 */
        /// <summary>
        /// Undocumented </summary>
        public const int IREAL = 0;

        /// <summary>
        /// Time series file </summary>
        public const int ITIME = 1;

        /// <summary>
        /// Spectral file-real/imag </summary>
        public const int IRLIM = 2;

        /// <summary>
        /// Spectral file-ampl/phase </summary>
        public const int IAMPH = 3;

        /// <summary>
        /// General x vs y file </summary>
        public const int IXY = 4;

        /// <summary>
        /// Unknown </summary>
        public const int IUNKN = 5;

        /// <summary>
        /// Displacement (NM) </summary>
        public const int IDISP = 6;

        /// <summary>
        /// Velocity (NM/SEC) </summary>
        public const int IVEL = 7;

        /// <summary>
        /// Acceleration (NM/SEC/SEC) </summary>
        public const int IACC = 8;

        /// <summary>
        /// Begin time </summary>
        public const int IB = 9;

        /// <summary>
        /// GMT day </summary>
        public const int IDAY = 10;

        /// <summary>
        /// Event origin time </summary>
        public const int IO = 11;

        /// <summary>
        /// First arrival time </summary>
        public const int IA = 12;

        /// <summary>
        /// User defined time pick 0 </summary>
        public const int IT0 = 13;

        /// <summary>
        /// User defined time pick 1 </summary>
        public const int IT1 = 14;

        /// <summary>
        /// User defined time pick 2 </summary>
        public const int IT2 = 15;

        /// <summary>
        /// User defined time pick 3 </summary>
        public const int IT3 = 16;

        /// <summary>
        /// User defined time pick 4 </summary>
        public const int IT4 = 17;

        /// <summary>
        /// User defined time pick 5 </summary>
        public const int IT5 = 18;

        /// <summary>
        /// User defined time pick 6 </summary>
        public const int IT6 = 19;

        /// <summary>
        /// User defined time pick 7 </summary>
        public const int IT7 = 20;
        /// <summary>
        /// User defined time pick 8 </summary>
        public const int IT8 = 21;

        /// <summary>
        /// User defined time pick 9 </summary>
        public const int IT9 = 22;

        /// <summary>
        /// Radial (NTS) </summary>
        public const int IRADNV = 23;

        /// <summary>
        /// Tangential (NTS) </summary>
        public const int ITANNV = 24;

        /// <summary>
        /// Radial (EVENT) </summary>
        public const int IRADEV = 25;

        /// <summary>
        /// Tangential (EVENT) </summary>
        public const int ITANEV = 26;

        /// <summary>
        /// North positive </summary>
        public const int INORTH = 27;

        /// <summary>
        /// East positive </summary>
        public const int IEAST = 28;

        /// <summary>
        /// Horizontal (ARB) </summary>
        public const int IHORZA = 29;

        /// <summary>
        /// Down positive </summary>
        public const int IDOWN = 30;

        /// <summary>
        /// Up positive </summary>
        public const int IUP = 31;

        /// <summary>
        /// LLL broadband </summary>
        public const int ILLLBB = 32;

        /// <summary>
        /// WWSN 15-100 </summary>
        public const int IWWSN1 = 33;

        /// <summary>
        /// WWSN 30-100 </summary>
        public const int IWWSN2 = 34;

        /// <summary>
        /// High-gain long-period </summary>
        public const int IHGLP = 35;

        /// <summary>
        /// SRO </summary>
        public const int ISRO = 36;

        /// <summary>
        /// Nuclear event </summary>
        public const int INUCL = 37;

        /// <summary>
        /// Nuclear pre-shot event </summary>
        public const int IPREN = 38;

        /// <summary>
        /// Nuclear post-shot event </summary>
        public const int IPOSTN = 39;

        /// <summary>
        /// Earthquake </summary>
        public const int IQUAKE = 40;

        /// <summary>
        /// Foreshock </summary>
        public const int IPREQ = 41;

        /// <summary>
        /// Aftershock </summary>
        public const int IPOSTQ = 42;

        /// <summary>
        /// Chemical explosion </summary>
        public const int ICHEM = 43;

        /// <summary>
        /// Other </summary>
        public const int IOTHER = 44;

        /// <summary>
        /// Good </summary>
        public const int IGOOD = 45;

        /// <summary>
        /// Gliches </summary>
        public const int IGLCH = 46;

        /// <summary>
        /// Dropouts </summary>
        public const int IDROP = 47;

        /// <summary>
        /// Low signal to noise ratio </summary>
        public const int ILOWSN = 48;

        /// <summary>
        /// Real data </summary>
        public const int IRLDTA = 49;

        /// <summary>
        /// Velocity (volts) </summary>
        public const int IVOLTS = 50;

        /// <summary>
        /// General XYZ (3-D) file </summary>
        public const int IXYZ = 51;

        /* These 18 added to describe magnitude type and source maf 970205 */
        /// <summary>
        /// Bodywave Magnitude </summary>
        public const int IMB = 52;

        /// <summary>
        /// Surface Magnitude </summary>
        public const int IMS = 53;
        /// <summary>
        /// Local Magnitude </summary>
        public const int IML = 54;

        /// <summary>
        /// Moment Magnitude </summary>
        public const int IMW = 55;

        /// <summary>
        /// Duration Magnitude </summary>
        public const int IMD = 56;

        /// <summary>
        /// User Defined Magnitude </summary>
        public const int IMX = 57;

        /// <summary>
        /// INEIC </summary>
        public const int INEIC = 58;

        /// <summary>
        /// IPDEQ </summary>
        public const int IPDEQ = 59;

        /// <summary>
        /// IPDEW </summary>
        public const int IPDEW = 60;

        /// <summary>
        /// IPDE </summary>
        public const int IPDE = 61;

        /// <summary>
        /// IISC </summary>
        public const int IISC = 62;

        /// <summary>
        /// IREB </summary>
        public const int IREB = 63;

        /// <summary>
        /// IUSGS </summary>
        public const int IUSGS = 64;

        /// <summary>
        /// IBRK </summary>
        public const int IBRK = 65;

        /// <summary>
        /// ICALTECH </summary>
        public const int ICALTECH = 66;

        /// <summary>
        /// ILLNL </summary>
        public const int ILLNL = 67;

        /// <summary>
        /// IEVLOC </summary>
        public const int IEVLOC = 68;

        /// <summary>
        /// IJSOP </summary>
        public const int IJSOP = 69;

        /// <summary>
        /// IUSER </summary>
        public const int IUSER = 70;

        /// <summary>
        /// IUNKNOWN </summary>
        public const int IUNKNOWN = 71;

        /* These 17 added for ievtyp. maf 970325 */
        /// <summary>
        /// Quarry or mine blast confirmed by quarry </summary>
        public const int IQB = 72;

        /// <summary>
        /// Quarry or mine blast with designed shot information-ripple fired </summary>
        public const int IQB1 = 73;

        /// <summary>
        /// Quarry or mine blast with observed shot information-ripple fired </summary>
        public const int IQB2 = 74;

        /// <summary>
        /// Quarry or mine blast - single shot </summary>
        public const int IQBX = 75;

        /// <summary>
        /// Quarry or mining-induced events: tremors and rockbursts </summary>
        public const int IQMT = 76;

        /// <summary>
        /// Earthquake </summary>
        public const int IEQ = 77;

        /// <summary>
        /// Earthquakes in a swarm or aftershock sequence </summary>
        public const int IEQ1 = 78;

        /// <summary>
        /// Felt earthquake </summary>
        public const int IEQ2 = 79;

        /// <summary>
        /// Marine explosion </summary>
        public const int IME = 80;

        /// <summary>
        /// Other explosion </summary>
        public const int IEX = 81;

        /// <summary>
        /// Nuclear explosion </summary>
        public const int INU = 82;

        /// <summary>
        /// Nuclear cavity collapse </summary>
        public const int INC = 83;

        /// <summary>
        /// Other source of known origin </summary>
        public const int IO_ = 84;

        /// <summary>
        /// Local event of unknown origin </summary>
        public const int IL = 85;
        /// <summary>
        /// Regional event of unknown origin </summary>
        public const int IR = 86;

        /// <summary>
        /// Teleseismic event of unknown origin </summary>
        public const int IT = 87;

        /// <summary>
        /// Undetermined or conflicting information </summary>
        public const int IU = 88;

        /* These 9 added for ievtype to keep up with database. maf 000530 */
        /// <summary>
        /// Damaging Earthquake </summary>
        public const int IEQ3 = 89;

        /// <summary>
        /// Probable earthquake </summary>
        public const int IEQ0 = 90;

        /// <summary>
        /// Probable explosion </summary>
        public const int IEX0 = 91;

        /// <summary>
        /// Mine collapse </summary>
        public const int IQC = 92;

        /// <summary>
        /// Probable Mine Blast </summary>
        public const int IQB0 = 93;

        /// <summary>
        /// Geyser </summary>
        public const int IGEY = 94;

        /// <summary>
        /// Light </summary>
        public const int ILIT = 95;

        /// <summary>
        /// Meteroic event </summary>
        public const int IMET = 96;

        /// <summary>
        /// Odors </summary>
        public const int IODOR = 97;

        public const int data_offset = 632;

        public const int NVHDR_OFFSET = 76 * 4;

        public const int NPTS_OFFSET = 79 * 4;

        public const bool SunByteOrder = true;

        public const bool IntelByteOrder = false;

        public const bool LITTLE_ENDIAN = IntelByteOrder;

        public const bool BIG_ENDIAN = SunByteOrder;

        //ORIGINAL LINE: private SacConstants()
        private SacConstants()
        {
        }

    }
}
