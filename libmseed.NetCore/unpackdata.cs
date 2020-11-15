using System;

namespace libmseedNetCore
{
    /* Extract bit range.  Byte order agnostic & defined when used with unsigned values */
    //C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
    //ORIGINAL LINE: #define EXTRACTBITRANGE(VALUE, STARTBIT, LENGTH) ((VALUE >> STARTBIT) & ((1U << LENGTH) - 1))


    internal static class DefineConstants
    {
        public const uint MAX12 = 0x7FFU; // maximum 12 bit positive #
        public const uint MAX14 = 0x1FFFU; // maximum 14 bit positive #
        public const uint MAX16 = 0x7FFFU; // maximum 16 bit positive #
        public const uint MAX24 = 0x7FFFFFU; // maximum 24 bit positive #
    }
    public struct dword
    {
        public sbyte[] d8;
        public Int16[] d16;
        public Int32 d32;

        /*
        public dword()
        {
            d8 = new byte[4];
            d16 = new short[2];
            d32 = 0;
        }*/
    }

    public class unpackdata
    {
        /* Control for printing debugging information */
        int decodedebug = 0;

        static void BITWIDTH(int VALUE, ref int RESULT)
        {
            if (VALUE >= -8 && VALUE <= 7)
                RESULT = 4;
            else if (VALUE >= -16 && VALUE <= 15)
                RESULT = 5;
            else if (VALUE >= -32 && VALUE <= 31)
                RESULT = 6;
            else if (VALUE >= -128 && VALUE <= 127)
                RESULT = 8;
            else if (VALUE >= -512 && VALUE <= 511)
                RESULT = 10;
            else if (VALUE >= -16384 && VALUE <= 16383)
                RESULT = 15;
            else if (VALUE >= -32768 && VALUE <= 32767)
                RESULT = 16;
            else if (VALUE >= -536870912 && VALUE <= 536870911)
                RESULT = 30;
            else
                RESULT = 32;
        }


        /************************************************************************
         * msr_decode_int16:
         *
         * Decode 16-bit integer data and place in supplied buffer as 32-bit
         * integers.
         *
         * Return number of samples in output buffer on success, -1 on error.
         ************************************************************************/
        private int msr_decode_int16(short[] input, int samplecount, int[] output, int outputlength, int swapflag)
        {
            short sample;
            int idx;

            if (samplecount <= 0)
            {
                return 0;
            }

            if (input == null || output == null || outputlength <= 0)
            {
                return -1;
            }

            for (idx = 0; idx < samplecount && outputlength >= (int)sizeof(int); idx++)
            {
                sample = input[idx];

                if (swapflag != 0)
                {
                    gswap.ms_gswap2a(ref sample);
                }

                output[idx] = (int)sample;

                outputlength -= sizeof(int);
            }

            return idx;
        } // End of msr_decode_int16()


        /************************************************************************
     * msr_decode_int32:
     *
     * Decode 32-bit integer data and place in supplied buffer as 32-bit
     * integers.
     *
     * Return number of samples in output buffer on success, -1 on error.
     ************************************************************************/
        private int msr_decode_int32(int[] input, int samplecount, int[] output, int outputlength, int swapflag)
        {
            int sample;
            int idx;

            if (samplecount <= 0)
            {
                return 0;
            }

            if (input == null || output == null || outputlength <= 0)
                return -1;


            for (idx = 0; idx < samplecount && outputlength >= (int)sizeof(int); idx++)
            {
                sample = input[idx];

                if (swapflag != 0)
                {
                    gswap.ms_gswap4a(ref sample);
                }

                output[idx] = sample;

                outputlength -= sizeof(int);
            }

            return idx;
        } // End of msr_decode_int32()

        /************************************************************************
        * msr_decode_float32:
        *
        * Decode 32-bit float data and place in supplied buffer as 32-bit
        * floats.
        *
        * Return number of samples in output buffer on success, -1 on error.
        ************************************************************************/
        private int msr_decode_float32(float[] input, int samplecount, float[] output, int outputlength, int swapflag)
        {
            float sample;
            int idx;

            if (samplecount <= 0)
            {
                return 0;
            }

            if (input == null || output == null || outputlength <= 0)
                return -1;

            for (idx = 0; idx < samplecount && outputlength >= (int)sizeof(float); idx++)
            {
                //C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
                //memcpy(sample, input[idx], sizeof(float));
                sample = input[idx];

                if (swapflag != 0)
                {
                    //gswap.ms_gswap4a(ref sample);
                }

                output[idx] = sample;

                outputlength -= sizeof(float);
            }

            return idx;
        } // End of msr_decode_float32()

        /************************************************************************
         * msr_decode_float64:
         *
         * Decode 64-bit float data and place in supplied buffer as 64-bit
         * floats, aka doubles.
         *
         * Return number of samples in output buffer on success, -1 on error.
         ************************************************************************/
        private int msr_decode_float64(double[] input, int samplecount, double[] output, int outputlength, int swapflag)
        {
            double sample;
            int idx;

            if (samplecount <= 0)
            {
                return 0;
            }


            if (input == null || output == null || outputlength <= 0)
                return -1;

            for (idx = 0; idx < samplecount && outputlength >= (int)sizeof(double); idx++)
            {
                //C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
                //memcpy(sample, input[idx], sizeof(double));
                sample = input[idx];

                if (swapflag != 0)
                {
                    gswap.ms_gswap8a(ref sample);
                }

                output[idx] = sample;

                outputlength -= sizeof(double);
            }

            return idx;
        } // End of msr_decode_float64()



        /************************************************************************
         * msr_decode_steim1:
         *
         * Decode Steim1 encoded miniSEED data and place in supplied buffer
         * as 32-bit integers.
         *
         * Return number of samples in output buffer on success, -1 on error.
         ************************************************************************/


        public static void Test_Steim1_decode()
        {
            //Int32[] input = { 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 65888778, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8 };
            //Int32[] input = { 123565, 223565, 323565, 423565, 523565, 623565, 723565, 823565, 123565, 223565, 323565, 423565, 523565, 623565, 723565, 823565, 123565, 223565, 323565, 423565, 523565, 623565, 723565, 823565, 123565, 223565, 323565, 423565, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 123565, 223565, 323565, 423565, 523565, 623565, 723565, 823565, 123565, 223565, 323565, 423565, 523565, 623565, 723565, 823565, 123565, 223565, 323565, 423565, 523565, 623565, 723565, 823565, 123565, 223565, 323565, 423565, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 123565, 223565, 323565, 423565, 523565, 623565, 723565, 823565, 123565, 223565, 323565, 423565, 523565, 623565, 723565, 823565, 123565, 223565, 323565, 423565, 523565, 623565, 723565, 823565, 123565, 223565, 323565, 423565, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 123565, 223565, 323565, 423565, 523565, 623565, 723565, 823565, 123565, 223565, 323565, 423565, 523565, 623565, 723565, 823565, 123565, 223565, 323565, 423565, 523565, 623565, 723565, 823565, 123565, 223565, 323565, 423565, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8 };  256
            Int32[] input = { 154000, 255, 300, 4888, 5888, 688888, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8 };
            Int32[] output = new int[16];
            int diff0 = 0;
            var len = msr_decode_steim1(ref input, 64, 16, ref output, 16, 1);
            for (int j = 0; j < len/*output.Length*/; j++)
            {
                Console.WriteLine("Element[" + j + "] = " + output[j] + "\n");
            }
            Console.WriteLine("len = " + len);

        }
        private static int msr_decode_steim1(ref int[] input, int inputlength, int samplecount, ref int[] output, int outputlength, int swapflag)
        {
            //C++ TO C# CONVERTER TODO TASK: Pointer arithmetic is detected on this variable, so pointers on this variable are left unchanged:
            int outputptr_index = 0; //= output; // Pointer to next output sample location
            uint[] frame = new uint[16]; // Frame, 16 x 32-bit quantities = 64 bytes
            int X0 = 0; // Forward integration constant, aka first sample
            int Xn = 0; // Reverse integration constant, aka last sample
            int maxframes = inputlength / 64;
            int frameidx;
            int startnibble;
            int nibble = 0;
            int widx;
            int diffcount;
            int idx;

            dword word;
            word.d8 = new sbyte[4];
            word.d16 = new short[2];

            if (inputlength <= 0)
            {
                return 0;
            }


            if (samplecount <= 0)
                return 0;

            if (input == null || output == null || outputlength <= 0)
                return -1;

            //if (decodedebug)
            //{
            //    ms_log(1, "Decoding %d Steim1 frames, swapflag: %d, srcname: %s\n", maxframes, swapflag, (srcname) ? srcname : "");
            //}

            for (frameidx = 0; frameidx < maxframes && samplecount > 0; frameidx++)
            {
                outputptr_index = 16 * frameidx;

                /* Copy frame, each is 16x32-bit quantities = 64 bytes */
                //memcpy(frame, input + (16 * frameidx), 64);
                for (int i = (16 * frameidx); i < ((16 * frameidx) + 16); i++)
                    frame[i] = (uint)input[i];

                /* Save forward integration constant (X0) and reverse integration constant (Xn)
                   and set the starting nibble index depending on frame. */
                if (frameidx == 0)
                {
                    if (swapflag != 0)
                    {
                        gswap.ms_gswap4a(ref frame[1]);
                        gswap.ms_gswap4a(ref frame[2]);
                    }

                    X0 = (int)frame[1];
                    Xn = (int)frame[2];

                    startnibble = 3; // First frame: skip nibbles, X0, and Xn

                    //if (decodedebug)
                    //{
                    //    ms_log(1, "Frame %d: X0=%d  Xn=%d\n", frameidx, X0, Xn);
                    //}
                }
                else
                {
                    startnibble = 1; // Subsequent frames: skip nibbles

                    //if (decodedebug)
                    //{
                    //    ms_log(1, "Frame %d\n", frameidx);
                    //}
                }

                /* Swap 32-bit word containing the nibbles */
                if (swapflag != 0)
                {
                    gswap.ms_gswap4a(ref frame[0]);
                }

                /* Decode each 32-bit word according to nibble */
                for (widx = startnibble; widx < 16 && samplecount > 0; widx++)
                {
                    /* W0: the first 32-bit contains 16 x 2-bit nibbles for each word */
                    // nibble = EXTRACTBITRANGE(frame[0], (30 - (2 * widx)), 2);
                    nibble = (int)((frame[0] >> (30 - (2 * widx))) & ((1U << 2) - 1));


                    // word = (union dword) frame[widx];
                    var bytes = BitConverter.GetBytes(frame[widx]);
                    word.d8[0] = (sbyte)bytes[0];
                    word.d8[1] = (sbyte)bytes[1];
                    word.d8[2] = (sbyte)bytes[2];
                    word.d8[3] = (sbyte)bytes[3];
                    word.d16[0] = BitConverter.ToInt16(bytes, 0);
                    word.d16[1] = BitConverter.ToInt16(bytes, 2);
                    word.d32 = (int)frame[widx];

                    diffcount = 0;

                    switch (nibble)
                    {
                        case 0: // 00: Special flag, no differences
                                //if (decodedebug)
                                //{
                                //    ms_log(1, "  W%02d: 00=special\n", widx);
                                //}
                            break;

                        case 1: // 01: Four 1-byte differences
                            diffcount = 4;

                            //if (decodedebug)
                            //{
                            //    ms_log(1, "  W%02d: 01=4x8b  %d  %d  %d  %d\n", widx, word.d8[0], word.d8[1], word.d8[2], word.d8[3]);
                            //}
                            break;

                        case 2: // 10: Two 2-byte differences
                            diffcount = 2;

                            if (swapflag != 0)
                            {
                                gswap.ms_gswap2a(ref word.d16[0]);
                                gswap.ms_gswap2a(ref word.d16[1]);
                            }

                            //if (decodedebug)
                            //{
                            //    ms_log(1, "  W%02d: 10=2x16b  %d  %d\n", widx, word.d16[0], word.d16[1]);
                            //}
                            break;

                        case 3: // 11: One 4-byte difference
                            diffcount = 1;
                            if (swapflag != 0)
                            {
                                //todo
                                // gswap.ms_gswap4a(ref word.d32);
                            }

                            //if (decodedebug)
                            //{
                            //    ms_log(1, "  W%02d: 11=1x32b  %d\n", widx, word.d32);
                            //}
                            break;
                    } // Done with decoding 32-bit word based on nibble

                    /* Apply accumulated differences to calculate output samples */


                    if (diffcount > 0)
                    {
                        for (idx = 0; idx < diffcount && samplecount > 0; idx++, outputptr_index++)
                        {

                            if (outputptr_index == 0) // Ignore first difference, instead store X0
                            {
                                // outputptr_index = X0;
                                output[outputptr_index] = X0;
                            }

                            else if (diffcount == 4) // Otherwise store difference from previous sample
                            {
                                output[outputptr_index] = output[(outputptr_index - 1)] + word.d8[idx];
                            }
                            else if (diffcount == 2)
                            {
                                output[outputptr_index] = output[(outputptr_index - 1)] + word.d16[idx];
                            }
                            else if (diffcount == 1)
                            {
                                output[outputptr_index] = output[(outputptr_index - 1)] + word.d32;
                            }

                            samplecount--;
                        }
                    }
                } // Done looping over nibbles and 32-bit words
            } // Done looping over frames

            /* Check data integrity by comparing last sample to Xn (reverse integration constant) */
            //todo
            //if (outputptr != output && (outputptr - 1) != Xn)
            //{
            //ms_log(1, "%s: Warning: Data integrity check for Steim1 failed, Last sample=%d, Xn=%d\n", srcname, *(outputptr - 1), Xn);
            //}
            //todo
            // return (outputptr - output);
            return outputptr_index;

        } // End of msr_decode_steim1()


        /************************************************************************
 * msr_decode_steim2:
 *
 * Decode Steim2 encoded miniSEED data and place in supplied buffer
 * as 32-bit integers.
 *
 * Return number of samples in output buffer on success, -1 on error.
 ************************************************************************/
        //C++ TO C# CONVERTER TODO TASK: Unions are not supported in C#:
        //union dword
        //{
        //  sbyte d8[4];
        //  int d32;
        //}
        private int msr_decode_steim2(ref int[] input, int inputlength, int samplecount, ref int[] output, int outputlength, ref string srcname, int swapflag)
        {
            //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to value types:
            //ORIGINAL LINE: int *outputptr = output;
            int outputptr = output; // Pointer to next output sample location
            uint[] frame = new uint[16]; // Frame, 16 x 32-bit quantities = 64 bytes
            int X0 = 0; // Forward integration constant, aka first sample
            int Xn = 0; // Reverse integration constant, aka last sample
            int[] diff = new int[7];
            int semask;
            int maxframes = inputlength / 64;
            int frameidx;
            int startnibble;
            int nibble;
            int widx;
            int diffcount;
            int dnib;
            int idx;

            dword word;

            if (inputlength <= 0)
            {
                return 0;
            }

            if (input == 0 || output == 0 || outputlength <= 0 || maxframes <= 0)
            {
                return -1;
            }

            //if (decodedebug)
            //{
            //    ms_log(1, "Decoding %d Steim2 frames, swapflag: %d, srcname: %s\n", maxframes, swapflag, (srcname) ? srcname : "");
            //}

            for (frameidx = 0; frameidx < maxframes && samplecount > 0; frameidx++)
            {
                /* Copy frame, each is 16x32-bit quantities = 64 bytes */
                //C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
                //memcpy(frame, input + (16 * frameidx), 64);
                for (int i = (16 * frameidx); i < ((16 * frameidx) + 16); i++)
                    frame[i] = (uint)input[i];


                /* Save forward integration constant (X0) and reverse integration constant (Xn)
                   and set the starting nibble index depending on frame. */
                if (frameidx == 0)
                {
                    if (swapflag != 0)
                    {
                        gswap.ms_gswap4a(ref frame[1]);
                        gswap.ms_gswap4a(ref frame[2]);
                    }

                    X0 = (int)frame[1];
                    Xn = (int)frame[2];

                    startnibble = 3; // First frame: skip nibbles, X0, and Xn

                    //if (decodedebug)
                    //{
                    //    ms_log(1, "Frame %d: X0=%d  Xn=%d\n", frameidx, X0, Xn);
                    //}
                }
                else
                {
                    startnibble = 1; // Subsequent frames: skip nibbles

                    //if (decodedebug)
                    //{
                    //    ms_log(1, "Frame %d\n", frameidx);
                    //}
                }

                /* Swap 32-bit word containing the nibbles */
                if (swapflag != 0)
                {
                    gswap.ms_gswap4a(ref frame[0]);
                }

                /* Decode each 32-bit word according to nibble */
                for (widx = startnibble; widx < 16 && samplecount > 0; widx++)
                {
                    /* W0: the first 32-bit quantity contains 16 x 2-bit nibbles */
                    nibble = EXTRACTBITRANGE(frame[0], (30 - (2 * widx)), 2);
                    diffcount = 0;

                    switch (nibble)
                    {
                        case 0: // nibble=00: Special flag, no differences
                            //if (decodedebug)
                            //{
                            //    ms_log(1, "  W%02d: 00=special\n", widx);
                            //}

                            break;
                        case 1: // nibble=01: Four 1-byte differences
                            diffcount = 4;

                            word = (union dword) frame[widx];
            for (idx = 0; idx < diffcount; idx++)
            {
                diff[idx] = word.d8[idx];
            }

            //if (decodedebug)
            //{
            //    ms_log(1, "  W%02d: 01=4x8b  %d  %d  %d  %d\n", widx, diff[0], diff[1], diff[2], diff[3]);
            //}
            break;

	  case 2: // nibble=10: Must consult dnib, the high order two bits
		if (swapflag != 0)
            {
                gswap.ms_gswap4a(frame[widx]);
            }
            dnib = EXTRACTBITRANGE(frame[widx], 30, 2);

            switch (dnib)
            {
                case 0: // nibble=10, dnib=00: Error, undefined value
                    //ms_log(2, "%s: Impossible Steim2 dnib=00 for nibble=10\n", srcname);

                    return -1;
                    break;

                case 1: // nibble=10, dnib=01: One 30-bit difference
                    diffcount = 1;
                    semask = 1U << (30 - 1); // Sign extension from bit 30
                    diff[0] = EXTRACTBITRANGE(frame[widx], 0, 30);
                    diff[0] = (diff[0] ^ semask) - semask;

                    //if (decodedebug)
                    //{
                    //    ms_log(1, "  W%02d: 10,01=1x30b  %d\n", widx, diff[0]);
                    //}
                    break;

                case 2: // nibble=10, dnib=10: Two 15-bit differences
                    diffcount = 2;
                    semask = 1U << (15 - 1); // Sign extension from bit 15
                    for (idx = 0; idx < diffcount; idx++)
                    {
                        diff[idx] = EXTRACTBITRANGE(frame[widx], (15 - idx * 15), 15);
                        diff[idx] = (diff[idx] ^ semask) - semask;
                    }

                    //if (decodedebug)
                    //{
                    //    ms_log(1, "  W%02d: 10,10=2x15b  %d  %d\n", widx, diff[0], diff[1]);
                    //}
                    break;

                case 3: // nibble=10, dnib=11: Three 10-bit differences
                    diffcount = 3;
                    semask = 1U << (10 - 1); // Sign extension from bit 10
                    for (idx = 0; idx < diffcount; idx++)
                    {
                        diff[idx] = EXTRACTBITRANGE(frame[widx], (20 - idx * 10), 10);
                        diff[idx] = (diff[idx] ^ semask) - semask;
                    }

                    //if (decodedebug)
                    //{
                    //    ms_log(1, "  W%02d: 10,11=3x10b  %d  %d  %d\n", widx, diff[0], diff[1], diff[2]);
                    //}
                    break;
            }

            break;

            case 3: // nibble=11: Must consult dnib, the high order two bits
		if (swapflag > 0)
            {
                gswap.ms_gswap4a(ref frame[widx]);
            }
            dnib = EXTRACTBITRANGE(frame[widx], 30, 2);

            switch (dnib)
            {
                case 0: // nibble=11, dnib=00: Five 6-bit differences
                    diffcount = 5;
                    semask = 1U << (6 - 1); // Sign extension from bit 6
                    for (idx = 0; idx < diffcount; idx++)
                    {
                        diff[idx] = EXTRACTBITRANGE(frame[widx], (24 - idx * 6), 6);
                        diff[idx] = (diff[idx] ^ semask) - semask;
                    }

                    //if (decodedebug)
                    //{
                    //    ms_log(1, "  W%02d: 11,00=5x6b  %d  %d  %d  %d  %d\n", widx, diff[0], diff[1], diff[2], diff[3], diff[4]);
                    //}
                    break;

                case 1: // nibble=11, dnib=01: Six 5-bit differences
                    diffcount = 6;
                    semask = 1U << (5 - 1); // Sign extension from bit 5
                    for (idx = 0; idx < diffcount; idx++)
                    {
                        diff[idx] = EXTRACTBITRANGE(frame[widx], (25 - idx * 5), 5);
                        diff[idx] = (diff[idx] ^ semask) - semask;
                    }

                    //if (decodedebug)
                    //{
                    //    ms_log(1, "  W%02d: 11,01=6x5b  %d  %d  %d  %d  %d  %d\n", widx, diff[0], diff[1], diff[2], diff[3], diff[4], diff[5]);
                    //}
                    break;

                case 2: // nibble=11, dnib=10: Seven 4-bit differences
                    diffcount = 7;
                    semask = 1U << (4 - 1); // Sign extension from bit 4
                    for (idx = 0; idx < diffcount; idx++)
                    {
                        diff[idx] = EXTRACTBITRANGE(frame[widx], (24 - idx * 4), 4);
                        diff[idx] = (diff[idx] ^ semask) - semask;
                    }

                    //if (decodedebug)
                    //{
                    //    ms_log(1, "  W%02d: 11,10=7x4b  %d  %d  %d  %d  %d  %d  %d\n", widx, diff[0], diff[1], diff[2], diff[3], diff[4], diff[5], diff[6]);
                    //}
                    break;

                case 3: // nibble=11, dnib=11: Error, undefined value
                    //ms_log(2, "%s: Impossible Steim2 dnib=11 for nibble=11\n", srcname);

                    return -1;
                    break;
            }

            break;
        } // Done with decoding 32-bit word based on nibble

	  /* Apply differences to calculate output samples */
	  if (diffcount > 0)
	  {
		for (idx = 0; idx<diffcount && samplecount> 0; idx++, outputptr++)
		{
		  if (outputptr == output) // Ignore first difference, instead store X0
		  {
			*outputptr = X0;
		  }
		  else // Otherwise store difference from previous sample
		  {

            * outputptr = *(outputptr - 1) + diff[idx];
}

samplecount--;
		}
	  }
	} // Done looping over nibbles and 32-bit words
  } // Done looping over frames
/* Check data integrity by comparing last sample to Xn (reverse integration constant) */
  if (outputptr != output && *(outputptr - 1) != Xn)
{
    ms_log(1, "%s: Warning: Data integrity check for Steim2 failed, Last sample=%d, Xn=%d\n", srcname, *(outputptr - 1), Xn);
}

return (outputptr - output);
} // End of msr_decode_steim2()



        }



    }


