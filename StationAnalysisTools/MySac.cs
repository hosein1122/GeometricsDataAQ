using System;
using System.Collections.Generic;
using System.Text;

namespace StationAnalysisToolsNetCore
{
    public class MySAC
    {
        /* all the sac header variables to be used in type SAC_HD */
        public class SAC
        {
            public float delta = (float)-12345.0; // 1
            public float depmin = (float)-12345.0;
            public float depmax = (float)-12345.0;
            public float scale = (float)-12345.0;
            public float odelta = (float)-12345.0;
            public float b = (float)-12345.0; // 2
            public float e = (float)-12345.0;
            public float o = (float)-12345.0;
            public float a = (float)-12345.0;
            public float internal1 = (float)-12345.0;
            public float t0 = (float)-12345.0; // 3
            public float t1 = (float)-12345.0;
            public float t2 = (float)-12345.0;
            public float t3 = (float)-12345.0;
            public float t4 = (float)-12345.0;
            public float t5 = (float)-12345.0; // 4
            public float t6 = (float)-12345.0;
            public float t7 = (float)-12345.0;
            public float t8 = (float)-12345.0;
            public float t9 = (float)-12345.0;
            public float f = (float)-12345.0; // 5
            public float resp0 = (float)-12345.0;
            public float resp1 = (float)-12345.0;
            public float resp2 = (float)-12345.0;
            public float resp3 = (float)-12345.0;
            public float resp4 = (float)-12345.0; // 6
            public float resp5 = (float)-12345.0;
            public float resp6 = (float)-12345.0;
            public float resp7 = (float)-12345.0;
            public float resp8 = (float)-12345.0;
            public float resp9 = (float)-12345.0; // 7
            public float stla = (float)-12345.0;
            public float stlo = (float)-12345.0;
            public float stel = (float)-12345.0;
            public float stdp = (float)-12345.0;
            public float evla = (float)-12345.0; // 8
            public float evlo = (float)-12345.0;
            public float evel = (float)-12345.0;
            public float evdp = (float)-12345.0;
            public float mag = (float)-12345.0;
            public float user0 = (float)-12345.0; // 9
            public float user1 = (float)-12345.0;
            public float user2 = (float)-12345.0;
            public float user3 = (float)-12345.0;
            public float user4 = (float)-12345.0;
            public float user5 = (float)-12345.0; // 10
            public float user6 = (float)-12345.0;
            public float user7 = (float)-12345.0;
            public float user8 = (float)-12345.0;
            public float user9 = (float)-12345.0;
            public float dist = (float)-12345.0; // 11
            public float az = (float)-12345.0;
            public float baz = (float)-12345.0;
            public float gcarc = (float)-12345.0;
            public float internal2 = (float)-12345.0;
            public float internal3 = (float)-12345.0; // 12
            public float depmen = (float)-12345.0;
            public float cmpaz = (float)-12345.0;
            public float cmpinc = (float)-12345.0;
            public float xminimum = (float)-12345.0;
            public float xmaximum = (float)-12345.0; // 13
            public float yminimum = (float)-12345.0;
            public float ymaximum = (float)-12345.0;
            public float unused1 = (float)-12345.0;
            public float unused2 = (float)-12345.0;
            public float unused3 = (float)-12345.0; // 14
            public float unused4 = (float)-12345.0;
            public float unused5 = (float)-12345.0;
            public float unused6 = (float)-12345.0;
            public float unused7 = (float)-12345.0;
            public int nzyear = -12345; // 15
            public int nzjday = -12345;
            public int nzhour = -12345;
            public int nzmin = -12345;
            public int nzsec = -12345;
            public int nzmsec = -12345; // 16
            public int nvhdr = -12345;
            public int norid = -12345;
            public int nevid = -12345;
            public int npts = -12345;
            public int internal4 = -12345; // 17
            public int nwfid = -12345;
            public int nxsize = -12345;
            public int nysize = -12345;
            public int unused8 = -12345;
            public int iftype = -12345; // 18
            public int idep = -12345;
            public int iztype = -12345;
            public int unused9 = -12345;
            public int iinst = -12345;
            public int istreg = -12345; // 19
            public int ievreg = -12345;
            public int ievtyp = -12345;
            public int iqual = -12345;
            public int isynth = -12345;
            public int imagtyp = -12345; // 20
            public int imagsrc = -12345;
            public int unused10 = -12345;
            public int unused11 = -12345;
            public int unused12 = -12345;
            public int unused13 = -12345; // 21
            public int unused14 = -12345;
            public int unused15 = -12345;
            public int unused16 = -12345;
            public int unused17 = -12345;
            public int leven = -12345; // 22
            public int lpspol = -12345;
            public int lovrok = -12345;
            public int lcalda = -12345;
            public int unused18 = -12345;
            public string kstnm = new string(new char[] { '-', '1', '2', '3', '4', '5', ' ', ' ' }); // 23
            public string kevnm = new string(new char[] { '-', '1', '2', '3', '4', '5', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' });
            public string khole = new string(new char[] { '-', '1', '2', '3', '4', '5', ' ', ' ' }); // 24
            public string ko = new string(new char[] { '-', '1', '2', '3', '4', '5', ' ', ' ' });
            public string ka = new string(new char[] { '-', '1', '2', '3', '4', '5', ' ', ' ' });
            public string kt0 = new string(new char[] { '-', '1', '2', '3', '4', '5', ' ', ' ' }); // 25
            public string kt1 = new string(new char[] { '-', '1', '2', '3', '4', '5', ' ', ' ' });
            public string kt2 = new string(new char[] { '-', '1', '2', '3', '4', '5', ' ', ' ' });
            public string kt3 = new string(new char[] { '-', '1', '2', '3', '4', '5', ' ', ' ' }); // 26
            public string kt4 = new string(new char[] { '-', '1', '2', '3', '4', '5', ' ', ' ' });
            public string kt5 = new string(new char[] { '-', '1', '2', '3', '4', '5', ' ', ' ' });
            public string kt6 = new string(new char[] { '-', '1', '2', '3', '4', '5', ' ', ' ' }); // 27
            public string kt7 = new string(new char[] { '-', '1', '2', '3', '4', '5', ' ', ' ' });
            public string kt8 = new string(new char[] { '-', '1', '2', '3', '4', '5', ' ', ' ' });
            public string kt9 = new string(new char[] { '-', '1', '2', '3', '4', '5', ' ', ' ' }); // 28
            public string kf = new string(new char[] { '-', '1', '2', '3', '4', '5', ' ', ' ' });
            public string kuser0 = new string(new char[] { '-', '1', '2', '3', '4', '5', ' ', ' ' });
            public string kuser1 = new string(new char[] { '-', '1', '2', '3', '4', '5', ' ', ' ' }); // 29
            public string kuser2 = new string(new char[] { '-', '1', '2', '3', '4', '5', ' ', ' ' });
            public string kcmpnm = new string(new char[] { '-', '1', '2', '3', '4', '5', ' ', ' ' });
            public string knetwk = new string(new char[] { '-', '1', '2', '3', '4', '5', ' ', ' ' }); // 30
            public string kdatrd = new string(new char[] { '-', '1', '2', '3', '4', '5', ' ', ' ' });
            public string kinst = new string(new char[] { '-', '1', '2', '3', '4', '5', ' ', ' ' });
            public SAC()
            {

            }
        }

        private SAC SAC_HEADER = new SAC();

        /* initial values for all the sac header variables */
        //private static sac sac_null = new sac(-12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345.0, -12345, -12345, -12345, -12345, -12345, -12345, -12345, -12345, -12345, -12345, -12345, -12345, -12345, -12345, -12345, -12345, -12345, -12345, -12345, -12345, -12345, -12345, -12345, -12345, -12345, -12345, -12345, -12345, -12345, -12345, -12345, -12345, -12345, -12345, -12345, -12345, -12345, -12345, -12345, -12345, {'-', '1', '2', '3', '4', '5', ' ', ' '}, {'-','1','2','3','4','5',' ',' ',' ',' ',' ',' ',' ',' ',' ',' '}, {'-','1','2','3','4','5',' ',' '}, {'-','1','2','3','4','5',' ',' '}, {'-','1','2','3','4','5',' ',' '}, {'-','1','2','3','4','5',' ',' '}, {'-','1','2','3','4','5',' ',' '}, {'-','1','2','3','4','5',' ',' '}, {'-','1','2','3','4','5',' ',' '}, {'-','1','2','3','4','5',' ',' '}, {'-','1','2','3','4','5',' ',' '}, {'-','1','2','3','4','5',' ',' '}, {'-','1','2','3','4','5',' ',' '}, {'-','1','2','3','4','5',' ',' '}, {'-','1','2','3','4','5',' ',' '}, {'-','1','2','3','4','5',' ',' '}, {'-','1','2','3','4','5',' ',' '}, {'-','1','2','3','4','5',' ',' '}, {'-','1','2','3','4','5',' ',' '}, {'-','1','2','3','4','5',' ',' '}, {'-','1','2','3','4','5',' ',' '}, {'-','1','2','3','4','5',' ',' '}, {'-','1','2','3','4','5',' ',' '});

        internal static class DefineConstants
        {
            public const int ITIME = 1;
            public const int IRLIM = 2;
            public const int IAMPH = 3;
            public const int IXY = 4;
            public const int IUNKN = 5;
            public const int IDISP = 6;
            public const int IVEL = 7;
            public const int IACC = 8;
            public const int IB = 9;
            public const int IDAY = 10;
            public const int IO = 11;
            public const int IA = 12;
            public const int IT0 = 13;
            public const int IT1 = 14;
            public const int IT2 = 15;
            public const int IT3 = 16;
            public const int IT4 = 17;
            public const int IT5 = 18;
            public const int IT6 = 19;
            public const int IT7 = 20;
            public const int IT8 = 21;
            public const int IT9 = 22;
            public const int IRADNV = 23;
            public const int ITANNV = 24;
            public const int IRADEV = 25;
            public const int ITANEV = 26;
            public const int INORTH = 27;
            public const int IEAST = 28;
            public const int IHORZA = 29;
            public const int IDOWN = 30;
            public const int IUP = 31;
            public const int ILLLBB = 32;
            public const int IWWSN1 = 33;
            public const int IWWSN2 = 34;
            public const int IHGLP = 35;
            public const int ISRO = 36;
            public const int INUCL = 37;
            public const int IPREN = 38;
            public const int IPOSTN = 39;
            public const int IQUAKE = 40;
            public const int IPREQ = 41;
            public const int IPOSTQ = 42;
            public const int ICHEM = 43;
            public const int IOTHER = 44;
            public const int IGOOD = 45;
            public const int IGLCH = 46;
            public const int IDROP = 47;
            public const int ILOWSN = 48;
            public const int IRLDTA = 49;
            public const int IVOLTS = 50;
            public const int IXYZ = 51;
            public const int IMB = 52;
            public const int IMS = 53;
            public const int IML = 54;
            public const int IMW = 55;
            public const int IMD = 56;
            public const int IMX = 57;
            public const int INEIC = 58;
            public const int IPDE = 59;
            public const int IISC = 60;
            public const int IREB = 61;
            public const int IUSGS = 62;
            public const int IBRK = 63;
            public const int ICALTECH = 64;
            public const int ILLNL = 65;
            public const int IEVLOC = 66;
            public const int IJSOP = 67;
            public const int IUSER = 68;
            public const int IUNKNOWN = 69;
            public const int IQB = 70;
            public const int IQB1 = 71;
            public const int IQB2 = 72;
            public const int IQBX = 73;
            public const int IQMT = 74;
            public const int IEQ = 75;
            public const int IEQ1 = 76;
            public const int IEQ2 = 77;
            public const int IME = 78;
            public const int IEX = 79;
            public const int INU = 80;
            public const int INC = 81;
            public const int IO_ = 82;
            public const int IL = 83;
            public const int IR = 84;
            public const int IT = 85;
            public const int IU = 86;
            public const string FCS = "%15.7f%15.7f%15.7f%15.7f%15.7f\n";
            public const string ICS = "%10d%10d%10d%10d%10d\n";
            public const string CCS1 = "%-8.8s%-8.8s%-8.8s\n";
            public const string CCS2 = "%-8.8s%-16.16s\n";



        }

    }
}
