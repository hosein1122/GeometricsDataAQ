using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace libmseedNetCore
{
    public class gswap
    {




        public static void ms_gswap2a(ref Int16 data)
        {
            data = (short)(((data >> 8) & 0xff) | ((data & 0xff) << 8));
        }

        public static void ms_gswap2a(ref UInt16 data)
        {
            data = (ushort)(((data >> 8) & 0xff) | ((data & 0xff) << 8));
        }

        public static void ms_gswap2(ref Int16 data2)
        {
            byte temp;

            Byte[] dat = new Byte[2];
            var data2_bytes = BitConverter.GetBytes(data2);
            Array.Copy(data2_bytes, dat, 2);
            temp = dat[0];
            dat[0] = dat[1];
            dat[1] = temp;
            Array.Copy(dat, data2_bytes, 2);
            data2 = BitConverter.ToInt16(data2_bytes);

        }

        public static void ms_gswap2(ref UInt16 data2)
        {
            byte temp;

            Byte[] dat = new Byte[2];
            var data2_bytes = BitConverter.GetBytes(data2);
            Array.Copy(data2_bytes, dat, 2);
            temp = dat[0];
            dat[0] = dat[1];
            dat[1] = temp;
            Array.Copy(dat, data2_bytes, 2);
            data2 = BitConverter.ToUInt16(data2_bytes);

        }
      


        public static void ms_gswap4a(ref UInt32 data4)
        {
            uint data = data4;
            data4 = (uint)(((data >> 24) & 0xff) | ((data & 0xff) << 24) | ((data >> 8) & 0xff00) | ((data & 0xff00) << 8));
        }

        public static void ms_gswap4a(ref Int32 data4)
        {
            Int32 data = data4;
            data4 = (Int32)(((data >> 24) & 0xff) | ((data & 0xff) << 24) | ((data >> 8) & 0xff00) | ((data & 0xff00) << 8));
        }

        public static void ms_gswap4(ref Int32 data4)
        {
            byte temp;

            byte[] dat = BitConverter.GetBytes(data4);

            //memcpy(dat, data4, 4);
            temp = dat[0];
            dat[0] = dat[3];
            dat[3] = temp;
            temp = dat[1];
            dat[1] = dat[2];
            dat[2] = temp;
            //memcpy(data4, dat, 4);
            data4 = BitConverter.ToInt32(dat);
        }

        public static void ms_gswap4(ref UInt32 data4)
        {
            byte temp;

            byte[] dat = BitConverter.GetBytes(data4);

            //memcpy(dat, data4, 4);
            temp = dat[0];
            dat[0] = dat[3];
            dat[3] = temp;
            temp = dat[1];
            dat[1] = dat[2];
            dat[2] = temp;
            //memcpy(data4, dat, 4);
            data4 = BitConverter.ToUInt32(dat);
        }


        public static void ms_gswap8a(ref double data8)
        {
            //uint[] data4 = data8;
            byte[] data8_bytes = BitConverter.GetBytes(data8);
            byte[] h0_bytes = new byte[] { data8_bytes[0], data8_bytes[1], data8_bytes[2], data8_bytes[3] };
            byte[] h1_bytes = new byte[] { data8_bytes[4], data8_bytes[5], data8_bytes[6], data8_bytes[7] };

            uint h0;
            uint h1;

            //h0 = data4[0];
            h0 = BitConverter.ToUInt32(h0_bytes);
            h0 = (uint)(((h0 >> 24) & 0xff) | ((h0 & 0xff) << 24) | ((h0 >> 8) & 0xff00) | ((h0 & 0xff00) << 8));

            //h1 = data4[1];
            h1 = BitConverter.ToUInt32(h1_bytes);
            h1 = (uint)(((h1 >> 24) & 0xff) | ((h1 & 0xff) << 24) | ((h1 >> 8) & 0xff00) | ((h1 & 0xff00) << 8));

            h0_bytes = BitConverter.GetBytes(h0);
            h1_bytes = BitConverter.GetBytes(h1);
            data8_bytes = new byte[] { h1_bytes[0], h1_bytes[1], h1_bytes[2], h1_bytes[3], h0_bytes[0], h0_bytes[1], h0_bytes[2], h0_bytes[3] };
            data8 = BitConverter.ToDouble(data8_bytes);
            // data4[0] = h1;
            // data4[1] = h0;
        }

        public static void ms_gswap8(ref double data8)
        {
            byte temp;


            //memcpy(dat, data8, 8);
            byte[] dat = BitConverter.GetBytes(data8);


            temp = dat[0];
            dat[0] = dat[7];
            dat[7] = temp;

            temp = dat[1];
            dat[1] = dat[6];
            dat[6] = temp;

            temp = dat[2];
            dat[2] = dat[5];
            dat[5] = temp;

            temp = dat[3];
            dat[3] = dat[4];
            dat[4] = temp;
            //memcpy(data8, dat, 8);
            data8 = BitConverter.ToDouble(dat);
        }

        public static void Test_ms_gswap2a()
        {
            Console.WriteLine("Test gswap functions");
            Int16 data = 2150; //1,2,-1,-3000,2150,-50
            Console.WriteLine("data = " + data.ToString());
            gswap.ms_gswap2a(ref data);
            Console.WriteLine("swaped data = " + data.ToString());


        }

        public static void Test_ms_gswap4a()
        {
            Console.WriteLine("Test gswap functions");
            Int32 data = -50; //1,2,-1,-3000,2150,-50
            Console.WriteLine("data = " + data.ToString());
            gswap.ms_gswap4a(ref data);
            Console.WriteLine("swaped data = " + data.ToString());


        }

        public static void Test_ms_gswap8a()
        {
            Console.WriteLine("Test gswap functions");
            double data = -12345678901234567; //1,2,-1,-3000,2150,-50
            Console.WriteLine("data = " + data.ToString());
            gswap.ms_gswap8a(ref data);
            Console.WriteLine("swaped data = " + data.ToString());


        }

        public static void Test_ms_gswap2()
        {
            Console.WriteLine("Test gswap functions");
            Int16 data = 2150; //1,2,-1,-3000,2150,-50
            Console.WriteLine("data = " + data.ToString());
            gswap.ms_gswap2(ref data);
            Console.WriteLine("swaped data = " + data.ToString());


        }

        public static void Test_ms_gswap4()
        {
            Console.WriteLine("Test gswap functions");
            int data = -3000; //1,2,-1,-3000,2150,-50
            Console.WriteLine("data = " + data.ToString());
            gswap.ms_gswap4(ref data);
            Console.WriteLine("swaped data = " + data.ToString());


        }
        public static void Test_ms_gswap8()
        {
            Console.WriteLine("Test gswap functions");
            double data = -100000000000001.000000; //1,2,-1,-3000,2150,-50
            Console.WriteLine("data = " + data.ToString());
            gswap.ms_gswap8(ref data);
            Console.WriteLine("swaped data = " + data.ToString());


        }

    }

}
