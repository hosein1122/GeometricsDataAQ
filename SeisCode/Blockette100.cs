﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SeisCode
{
    public class Blockette100 : DataBlockette
    {

        public Blockette100() : base(B100_SIZE)
        {
        }

        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
        //ORIGINAL LINE: public Blockette100(byte[] info, boolean swapBytes) throws SeedFormatException
        public Blockette100(byte[] info, bool swapBytes) : base(info, swapBytes)
        {
            trimToSize(B100_SIZE);
        }

        public virtual float ActualSampleRate
        {
            set
            {
                Utility.insertFloat(value, info, 4);
            }
            get
            {
                int bits = Utility.bytesToInt(info[4], info[5], info[6], info[7], false);
                //return Float.intBitsToFloat(bits);
                return bits;
            }
        }


        public override int Type
        {
            get
            {
                return 100;
            }
        }

        public override int Size
        {
            get
            {
                return B100_SIZE;
            }
        }

        public override string Name
        {
            get
            {
                return "Sample Rate Blockette";
            }
        }



        public override void WriteASCII(TextWriter @out)
        {
            @out.WriteLine("Blockette100 " + ActualSampleRate);
        }



        public const int B100_SIZE = 12;
    }
}
