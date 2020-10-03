using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq.Expressions;
using System.Text;

namespace libmseedNetCore
{
    public class packdata
    {
        /* Control for printing debugging information */
        public static bool encodedebug = false;

        public struct dword
        {
            public byte[] d8;
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

        public static int msr_encode_text(ref char[] input, int samplecount, ref char[] output, int outputlength)
        {
            int length;

            if (samplecount <= 0)
                return 0;

            if (input == null || output == null || outputlength <= 0)
                return -1;

            /* Determine minimum of input or output */
            length = (samplecount < outputlength) ? samplecount : outputlength;

            Array.Copy(input, 0, output, 0, length);

            outputlength -= length;

            if (outputlength > 0)
            {
                for (int j = 0; j < outputlength; j++)
                {
                    output[length + j] = (char)0;
                }
            }
            //memset(output + length, 0, outputlength);

            return length;
        } /* End of msr_encode_text() */

        public static void test_msr_encode_text()
        {
            char[] input = { '1', '2', '3', '4', '5' };
            char[] output = new char[8];
            int len = msr_encode_text(ref input, 5, ref output, 8);
            for (int j = 0; j < output.Length; j++)
            {
                Console.WriteLine("Element[" + j + "] = " + output[j] + "\n");
            }
        }


        public static Int32 msr_encode_int16(ref Int32[] input, Int32 samplecount, ref Int16[] output, Int32 outputlength, Int32 swapflag)
        {
            int idx;

            if (samplecount <= 0)
            {
                return 0;
            }

            if (input == null || output == null || outputlength <= 0)
            {
                return -1;
            }

            for (idx = 0; idx < samplecount && outputlength >= (int)sizeof(short); idx++)
            {
                output[idx] = (short)input[idx];

                if (swapflag != 0)
                {
                    gswap.ms_gswap2a(ref output[idx]);
                }

                outputlength -= sizeof(short);
            }

            if (outputlength != 0)
            {
                for (int j = 0; j < outputlength; j++)
                {
                    output[idx + j] = 0;
                }

                //memset(output[idx], 0, outputlength);
            }

            return idx;
        } // End of msr_encode_int16()

        public static void test_msr_encode_int16()
        {
            Int32[] input = { 100, 2, 3, 4, 50, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 9 }; //len=17
            Int16[] output = new short[100];
            int len = msr_encode_int16(ref input, 17, ref output, 100, 1);
            for (int j = 0; j < len/*output.Length*/; j++)
            {
                Console.WriteLine("Element[" + j + "] = " + output[j] + "\n");
            }
            Console.WriteLine("len = " + len);
        }

        public static int msr_encode_int32(Int32[] input, int samplecount, ref Int32[] output,
                  int outputlength, int swapflag)
        {
            int idx;

            if (samplecount <= 0)
                return 0;

            if (input == null || output == null || outputlength <= 0)
                return -1;

            for (idx = 0; idx < samplecount && outputlength >= (int)sizeof(Int32); idx++)
            {
                output[idx] = input[idx];

                if (swapflag == 1)
                    //ms_gswap4a(&output[idx]);

                    outputlength -= sizeof(Int32);
            }

            if (outputlength > 0)
            {
                // memset(&output[idx], 0, outputlength);
                for (int i = idx; i < outputlength; i++)
                {
                    output[i] = 0;
                }
            }

            return idx;
        } /* End of msr_encode_int32() */
        public static int msr_encode_float32(float[] input, int samplecount, ref float[] output,
                            int outputlength, int swapflag)
        {
            int idx;

            if (samplecount <= 0)
                return 0;

            if (input == null || output == null || outputlength <= 0)
                return -1;

            for (idx = 0; idx < samplecount && outputlength >= (int)sizeof(float); idx++)
            {
                output[idx] = input[idx];

                if (swapflag == 1)
                    //ms_gswap4a(&output[idx]);

                    outputlength -= sizeof(float);
            }

            if (outputlength > 0)
            {
                // memset(&output[idx], 0, outputlength);
                for (int i = idx; i < outputlength; i++)
                {
                    output[i] = 0;
                }

            }

            return idx;
        } /* End of msr_encode_float32() */

        public int msr_encode_float64(double[] input, int samplecount, ref double[] output,
                    int outputlength, int swapflag)
        {
            int idx;

            if (samplecount <= 0)
                return 0;

            if (input == null || output == null || outputlength <= 0)
                return -1;

            for (idx = 0; idx < samplecount && outputlength >= (int)sizeof(double); idx++)
            {
                output[idx] = input[idx];

                if (swapflag == 1)
                    //ms_gswap8a(&output[idx]);

                    outputlength -= sizeof(double);
            }

            if (outputlength > 0)
            {
                //memset(&output[idx], 0, outputlength);
                for (int i = idx; i < outputlength; i++)
                {
                    output[i] = 0;
                }
            }

            return idx;
        } /* End of msr_encode_float64() */

        private static int msr_encode_steim1(int[] input, int samplecount, ref int[] output, int outputlength, int diff0, int swapflag)
        {
            uint[] frameptr; // Frame pointer in output
            int frameptr_index;
            //int Xnp = null; // Reverse integration constant, aka last sample
            int? Xnp = null; // Reverse integration constant, aka last sample
            int[] diffs = new int[4];
            int[] bitwidth = new int[4];
            int diffcount = 0;
            int inputidx = 0;
            int outputsamples = 0;
            int maxframes = outputlength / 64;
            int packedsamples = 0;
            int frameidx;
            int startnibble;
            int widx;
            int idx;
            // to do
            dword word = new dword();
            word.d8 = new byte[4];
            word.d16 = new short[2];

            if (samplecount <= 0)
            {
                return 0;
            }

            if (input == null || output == null || outputlength <= 0)
            {
                return -1;
            }
            /*
			if (encodedebug)
			{
				ms_log(1, "Encoding Steim2 frames, samples: %d, max frames: %d, swapflag: %d\n", samplecount, maxframes, swapflag);
			}*/

            /* Add first difference to buffers */
            diffs[0] = diff0;
            BITWIDTH(diffs[0], ref bitwidth[0]);
            diffcount = 1;

            for (frameidx = 0; frameidx < maxframes && outputsamples < samplecount; frameidx++)
            {
                Console.WriteLine("frameidex=" + frameidx);
                //frameptr = (uint)output + (16 * frameidx);
                frameptr_index = 16 * frameidx;


                /* Set 64-byte frame to 0's   ===> 16 * int(4 byte) = 64*/
                //memset(frameptr, 0, 64);
                frameptr = new uint[64];
                for (int i = 0; i < 64; i++)
                    frameptr[i] = 0;

                /* Save forward integration constant (X0), pointer to reverse integration constant (Xn)
				 * and set the starting nibble index depending on frame. */
                if (frameidx == 0)
                {
                    frameptr[1] = (uint)input[0];
                    output[1] = (int)frameptr[1];

                    if (encodedebug)
                    {
                        //ms_log(1, "Frame %d: X0=%d\n", frameidx, frameptr[1]);
                    }

                    if (swapflag != 0)
                    {
                        gswap.ms_gswap4a(ref frameptr[1]);
                        gswap.ms_gswap4a(ref output[1]);

                    }

                    Xnp = (int)frameptr[2];

                    startnibble = 3; // First frame: skip nibbles, X0, and Xn
                }
                else
                {
                    startnibble = 1; // Subsequent frames: skip nibbles

                    if (encodedebug)
                    {
                        //ms_log(1, "Frame %d\n", frameidx);
                    }
                }

                for (widx = startnibble; widx < 16 && outputsamples < samplecount; widx++)
                {
                    if (diffcount < 4)
                    {
                        /* Shift diffs and related bit widths to beginning of buffers */
                        for (idx = 0; idx < diffcount; idx++)
                        {
                            diffs[idx] = diffs[packedsamples + idx];
                            bitwidth[idx] = bitwidth[packedsamples + idx];
                        }

                        /* Add new diffs and determine bit width needed to represent */
                        for (idx = diffcount; idx < 4 && inputidx < (samplecount - 1); idx++, inputidx++)
                        {
                            diffs[idx] = input[inputidx + 1] - input[inputidx];
                            BITWIDTH(diffs[idx], ref bitwidth[idx]);
                            diffcount++;
                        }
                    }


                    /* Determine optimal packing by checking, in-order:
					 * 7 x 4-bit differences
					 * 6 x 5-bit differences
					 * 5 x 6-bit differences
					 * 4 x 8-bit differences
					 * 3 x 10-bit differences
					 * 2 x 15-bit differences
					 * 1 x 30-bit difference */

                    packedsamples = 0;

                    /* 4 x 8-bit differences */
                    if (diffcount == 4 && bitwidth[0] <= 8 && bitwidth[1] <= 8 && bitwidth[2] <= 8 && bitwidth[3] <= 8)
                    {
                        if (encodedebug)
                        {
                            //ms_log(1, "  W%02d: 01=4x8b  %d  %d  %d  %d\n", widx, diffs[0], diffs[1], diffs[2], diffs[3]);
                        }

                        /*
                        word.d8[0] = (sbyte)diffs[0];
                        word.d8[1] = (sbyte)diffs[1];
                        word.d8[2] = (sbyte)diffs[2];
                        word.d8[3] = (sbyte)diffs[3];*/

                        word.d8[0] = (byte)diffs[0];
                        word.d8[1] = (byte)diffs[1];
                        word.d8[2] = (byte)diffs[2];
                        word.d8[3] = (byte)diffs[3];
                        frameptr[widx] = BitConverter.ToUInt32(word.d8);
                        output[widx + frameptr_index] = (int)BitConverter.ToUInt32(word.d8);

                        /* 2-bit nibble is 0b01 (0x1) */
                        frameptr[0] |= 0x1U << (30 - 2 * widx);
                        output[0 + frameptr_index] = (int)frameptr[0];
                        packedsamples = 4;
                    }
                    /* 2 x 16-bit differences */
                    else if (diffcount >= 2 && bitwidth[0] <= 16 && bitwidth[1] <= 16)
                    {
                        if (encodedebug)
                        {
                            // ms_log(1, "  W%02d: 2=2x16b  %d  %d\n", widx, diffs[0], diffs[1]);
                        }

                        word.d16[0] = (short)diffs[0];
                        word.d16[1] = (short)diffs[1];

                        frameptr[widx + 1] = (uint)((ushort)word.d16[0] << 16 | (ushort)word.d16[1]);
                        output[widx + frameptr_index] = (ushort)word.d16[0] << 16 | (ushort)word.d16[1];

                        if (swapflag == 1)
                        {
                            gswap.ms_gswap2a(ref word.d16[0]);
                            gswap.ms_gswap2a(ref word.d16[1]);
                        }

                        /* 2-bit nibble is 0b10 (0x2) */
                        frameptr[0] |= 0x2U << (30 - 2 * widx);
                        output[0 + frameptr_index] = (int)frameptr[0];
                        packedsamples = 2;
                    }
                    /* 1 x 32-bit difference */
                    else
                    {
                        if (encodedebug)
                        {
                            //ms_log(1, "  W%02d: 3=1x32b  %d\n", widx, diffs[0]);
                        }

                        frameptr[widx] = (uint)diffs[0];

                        if (swapflag != 0)
                        {
                            gswap.ms_gswap4a(ref frameptr[widx]);
                        }

                        /* 2-bit nibble is 0b11 (0x3) */
                        frameptr[0] |= 0x3U << (30 - 2 * widx);

                        output[widx + frameptr_index] = (int)frameptr[widx];
                        output[0 + frameptr_index] = (int)frameptr[0];
                        packedsamples = 1;
                    }


                    /* Swap encoded word except for 4x8-bit samples */
                    if (swapflag != 0 && packedsamples != 4)
                    {
                        gswap.ms_gswap4a(ref frameptr[widx]);
                        output[widx + frameptr_index] = (int)frameptr[widx];

                    }

                    diffcount -= packedsamples;
                    outputsamples += packedsamples;
                } // Done with words in frame

                /* Swap word with nibbles */
                if (swapflag != 0)
                {
                    gswap.ms_gswap4a(ref frameptr[0]);
                    output[0 + frameptr_index] = (int)frameptr[0];

                }
            } // Done with frames

            /* Set Xn (reverse integration constant) in first frame to last sample */
            //if (Xnp)
            if (Xnp != null)
            {

                // *Xnp = *(input + outputsamples - 1);
                Xnp = input[outputsamples - 1];
                output[2] = (int)Xnp;
            }
            if (swapflag != 0)
            {
                int xnp_int = (int)Xnp;
                gswap.ms_gswap4a(ref xnp_int);
                Xnp = (int?)xnp_int;
                output[2] = (int)Xnp;

            }

            /* Pad any remaining bytes */
            if ((frameidx * 64) < outputlength)
            {
                //memset(output + (frameidx * 16), 0, outputlength - (frameidx * 64));
                int start_index = frameidx * 16;
                int size = outputlength - (frameidx * 64);
                for (int i = start_index; i < size; i++)
                    output[i] = 0;
            }

            return (outputsamples);
        } // End of msr_encode_steim1()

        private static int msr_encode_steim2(int[] input, int samplecount, ref int[] output, int outputlength, int diff0, ref string srcname, int swapflag)
        {
            uint[] frameptr; // Frame pointer in output
            int frameptr_index;
            //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to value types:
            //ORIGINAL LINE: int *Xnp = NULL;

            //int Xnp = null; // Reverse integration constant, aka last sample
            int? Xnp = null; // Reverse integration constant, aka last sample
            int[] diffs = new int[7];
            int[] bitwidth = new int[7];
            int diffcount = 0;
            int inputidx = 0;
            int outputsamples = 0;
            int maxframes = outputlength / 64;
            int packedsamples = 0;
            int frameidx;
            int startnibble;
            int widx;
            int idx;
            // to do
            dword word = new dword();
            word.d8 = new byte[4];
            word.d16 = new short[2];

            if (samplecount <= 0)
            {
                return 0;
            }

            if (input == null || output == null || outputlength <= 0)
            {
                return -1;
            }
            /*
			if (encodedebug)
			{
				ms_log(1, "Encoding Steim2 frames, samples: %d, max frames: %d, swapflag: %d\n", samplecount, maxframes, swapflag);
			}*/

            /* Add first difference to buffers */
            diffs[0] = diff0;
            BITWIDTH(diffs[0], ref bitwidth[0]);
            diffcount = 1;

            for (frameidx = 0; frameidx < maxframes && outputsamples < samplecount; frameidx++)
            {
                Console.WriteLine("frameidex=" + frameidx);
                //frameptr = (uint)output + (16 * frameidx);
                frameptr_index = 16 * frameidx;


                /* Set 64-byte frame to 0's   ===> 16 * int(4 byte) = 64*/
                //memset(frameptr, 0, 64);
                frameptr = new uint[64];
                for (int i = 0; i < 64; i++)
                    frameptr[i] = 0;

                /* Save forward integration constant (X0), pointer to reverse integration constant (Xn)
				 * and set the starting nibble index depending on frame. */
                if (frameidx == 0)
                {
                    frameptr[1] = (uint)input[0];
                    output[1] = (int)frameptr[1];

                    if (encodedebug)
                    {
                        //ms_log(1, "Frame %d: X0=%d\n", frameidx, frameptr[1]);
                    }

                    if (swapflag != 0)
                    {
                        gswap.ms_gswap4a(ref frameptr[1]);
                        gswap.ms_gswap4a(ref output[1]);

                    }

                    Xnp = (int)frameptr[2];

                    startnibble = 3; // First frame: skip nibbles, X0, and Xn
                }
                else
                {
                    startnibble = 1; // Subsequent frames: skip nibbles

                    if (encodedebug)
                    {
                        //ms_log(1, "Frame %d\n", frameidx);
                    }
                }

                for (widx = startnibble; widx < 16 && outputsamples < samplecount; widx++)
                {
                    if (diffcount < 7)
                    {
                        /* Shift diffs and related bit widths to beginning of buffers */
                        for (idx = 0; idx < diffcount; idx++)
                        {
                            diffs[idx] = diffs[packedsamples + idx];
                            bitwidth[idx] = bitwidth[packedsamples + idx];
                        }

                        /* Add new diffs and determine bit width needed to represent */
                        for (idx = diffcount; idx < 7 && inputidx < (samplecount - 1); idx++, inputidx++)
                        {
                            //diffs[idx] = *(input + inputidx + 1) - *(input + inputidx);
                            diffs[idx] = input[inputidx + 1] - input[inputidx];
                            BITWIDTH(diffs[idx], ref bitwidth[idx]);
                            diffcount++;
                        }

                    }
                    /*
                    for (int i = 0; i < diffs.Length; i++)
                    {
                        Console.WriteLine("diff[" + i + "]=" + diffs[i]);
                        Console.WriteLine("bitwidth[" + i + "]=" + bitwidth[i]);

                    }
                    Console.WriteLine("diffcount = " + diffcount);*/
                    /* Determine optimal packing by checking, in-order:
					 * 7 x 4-bit differences
					 * 6 x 5-bit differences
					 * 5 x 6-bit differences
					 * 4 x 8-bit differences
					 * 3 x 10-bit differences
					 * 2 x 15-bit differences
					 * 1 x 30-bit difference */

                    packedsamples = 0;

                    /* 7 x 4-bit differences */
                    if (diffcount == 7 && bitwidth[0] <= 4 && bitwidth[1] <= 4 && bitwidth[2] <= 4 && bitwidth[3] <= 4 && bitwidth[4] <= 4 && bitwidth[5] <= 4 && bitwidth[6] <= 4)
                    {

                        if (encodedebug)
                        {
                            //ms_log(1, "  W%02d: 11,10=7x4b  %d  %d  %d  %d  %d  %d  %d\n", widx, diffs[0], diffs[1], diffs[2], diffs[3], diffs[4], diffs[5], diffs[6]);
                        }

                        /* Mask the values, shift to proper location and set in word */
                        frameptr[widx] = ((uint)diffs[6] & 0xFU);
                        frameptr[widx] |= ((uint)diffs[5] & 0xFU) << 4;
                        frameptr[widx] |= ((uint)diffs[4] & 0xFU) << 8;
                        frameptr[widx] |= ((uint)diffs[3] & 0xFU) << 12;
                        frameptr[widx] |= ((uint)diffs[2] & 0xFU) << 16;
                        frameptr[widx] |= ((uint)diffs[1] & 0xFU) << 20;
                        frameptr[widx] |= ((uint)diffs[0] & 0xFU) << 24;

                        /* 2-bit decode nibble is 0b10 (0x2) */
                        frameptr[widx] |= 0x2U << 30;

                        /* 2-bit nibble is 0b11 (0x3) */
                        frameptr[0] |= 0x3U << (30 - 2 * widx);

                        output[widx + frameptr_index] = (int)frameptr[widx];
                        output[0 + frameptr_index] = (int)frameptr[0];


                        packedsamples = 7;
                    }
                    /* 6 x 5-bit differences */
                    else if (diffcount >= 6 && bitwidth[0] <= 5 && bitwidth[1] <= 5 && bitwidth[2] <= 5 && bitwidth[3] <= 5 && bitwidth[4] <= 5 && bitwidth[5] <= 5)
                    {
                        if (encodedebug)
                        {
                            //ms_log(1, "  W%02d: 11,01=6x5b  %d  %d  %d  %d  %d  %d\n", widx, diffs[0], diffs[1], diffs[2], diffs[3], diffs[4], diffs[5]);
                        }

                        /* Mask the values, shift to proper location and set in word */
                        frameptr[widx] = ((uint)diffs[5] & 0x1FU);
                        frameptr[widx] |= ((uint)diffs[4] & 0x1FU) << 5;
                        frameptr[widx] |= ((uint)diffs[3] & 0x1FU) << 10;
                        frameptr[widx] |= ((uint)diffs[2] & 0x1FU) << 15;
                        frameptr[widx] |= ((uint)diffs[1] & 0x1FU) << 20;
                        frameptr[widx] |= ((uint)diffs[0] & 0x1FU) << 25;

                        /* 2-bit decode nibble is 0b01 (0x1) */
                        frameptr[widx] |= 0x1U << 30;

                        /* 2-bit nibble is 0b11 (0x3) */
                        frameptr[0] |= 0x3U << (30 - 2 * widx);

                        output[widx + frameptr_index] = (int)frameptr[widx];
                        output[0 + frameptr_index] = (int)frameptr[0];


                        packedsamples = 6;
                    }
                    /* 5 x 6-bit differences */
                    else if (diffcount >= 5 && bitwidth[0] <= 6 && bitwidth[1] <= 6 && bitwidth[2] <= 6 && bitwidth[3] <= 6 && bitwidth[4] <= 6)
                    {
                        if (encodedebug)
                        {
                            //ms_log(1, "  W%02d: 11,00=5x6b  %d  %d  %d  %d  %d\n", widx, diffs[0], diffs[1], diffs[2], diffs[3], diffs[4]);
                        }

                        /* Mask the values, shift to proper location and set in word */
                        frameptr[widx] = (uint)((uint)diffs[4] & 0x3FU);
                        frameptr[widx] |= ((uint)diffs[3] & 0x3FU) << 6;
                        frameptr[widx] |= ((uint)diffs[2] & 0x3FU) << 12;
                        frameptr[widx] |= ((uint)diffs[1] & 0x3FU) << 18;
                        frameptr[widx] |= ((uint)diffs[0] & 0x3FU) << 24;

                        /* 2-bit decode nibble is 0b00, nothing to set */

                        /* 2-bit nibble is 0b11 (0x3) */
                        frameptr[0] |= 0x3U << (30 - 2 * widx);

                        output[widx + frameptr_index] = (int)frameptr[widx];
                        output[0 + frameptr_index] = (int)frameptr[0];

                        packedsamples = 5;
                    }
                    /* 4 x 8-bit differences */
                    else if (diffcount >= 4 && bitwidth[0] <= 8 && bitwidth[1] <= 8 && bitwidth[2] <= 8 && bitwidth[3] <= 8)
                    {
                        if (encodedebug)
                        {
                            //ms_log(1, "  W%02d: 01=4x8b  %d  %d  %d  %d\n", widx, diffs[0], diffs[1], diffs[2], diffs[3]);
                        }


                        //word = (union dword) frameptr[widx];

                        word.d8[0] = (byte)diffs[0];
                        word.d8[1] = (byte)diffs[1];
                        word.d8[2] = (byte)diffs[2];
                        word.d8[3] = (byte)diffs[3];
                        frameptr[widx] = BitConverter.ToUInt32(word.d8);
                        output[widx + frameptr_index] = (int)BitConverter.ToUInt32(word.d8);


                        /* 2-bit nibble is 0b01, only need to set 2nd bit */
                        frameptr[0] |= 0x1U << (30 - 2 * widx);

                        output[0 + frameptr_index] = (int)frameptr[0];

                        packedsamples = 4;
                    }
                    /* 3 x 10-bit differences */
                    else if (diffcount >= 3 && bitwidth[0] <= 10 && bitwidth[1] <= 10 && bitwidth[2] <= 10)
                    {
                        if (encodedebug)
                        {
                            // ms_log(1, "  W%02d: 10,11=3x10b  %d  %d  %d\n", widx, diffs[0], diffs[1], diffs[2]);
                        }

                        /* Mask the values, shift to proper location and set in word */
                        frameptr[widx] = ((uint)diffs[2] & 0x3FFU);
                        frameptr[widx] |= ((uint)diffs[1] & 0x3FFU) << 10;
                        frameptr[widx] |= ((uint)diffs[0] & 0x3FFU) << 20;

                        /* 2-bit decode nibble is 0b11 (0x3) */
                        frameptr[widx] |= 0x3U << 30;

                        /* 2-bit nibble is 0b10 (0x2) */
                        frameptr[0] |= 0x2U << (30 - 2 * widx);

                        output[widx + frameptr_index] = (int)frameptr[widx];
                        output[0 + frameptr_index] = (int)frameptr[0];

                        packedsamples = 3;
                    }
                    /* 2 x 15-bit differences */
                    else if (diffcount >= 2 && bitwidth[0] <= 15 && bitwidth[1] <= 15)
                    {
                        Console.WriteLine("widx=" + widx);
                        if (encodedebug)
                        {
                            //ms_log(1, "  W%02d: 10,10=2x15b  %d  %d\n", widx, diffs[0], diffs[1]);
                        }
                        Console.WriteLine("ok");
                        /* Mask the values, shift to proper location and set in word */
                        frameptr[widx] = ((uint)diffs[1] & 0x7FFFU);

                        frameptr[widx] |= ((uint)diffs[0] & 0x7FFFU) << 15;

                        Console.WriteLine("frameptr[" + widx + "]=" + frameptr[widx]);


                        /* 2-bit decode nibble is 0b10 (0x2) */
                        frameptr[widx] |= 0x2U << 30;
                        //output[widx + frameptr_index] = (int)frameptr[widx + frameptr_index];


                        /* 2-bit nibble is 0b10 (0x2) */
                        frameptr[0] |= 0x2U << (30 - 2 * widx);

                        output[widx + frameptr_index] = (Int32)frameptr[widx];
                        output[0 + frameptr_index] = (Int32)frameptr[0];

                        packedsamples = 2;
                    }
                    /* 1 x 30-bit difference */
                    else if (diffcount >= 1 && bitwidth[0] <= 30)
                    {
                        if (encodedebug)
                        {
                            //ms_log(1, "  W%02d: 10,01=1x30b  %d\n", widx, diffs[0]);
                        }

                        /* Mask the value and set in word */
                        frameptr[widx] = ((uint)diffs[0] & 0x3FFFFFFFU);

                        /* 2-bit decode nibble is 0b01 (0x1) */
                        frameptr[widx] |= 0x1U << 30;

                        /* 2-bit nibble is 0b10 (0x2) */
                        frameptr[0] |= 0x2U << (30 - 2 * widx);


                        output[widx + frameptr_index] = (int)frameptr[widx];
                        output[0 + frameptr_index] = (int)frameptr[0];

                        packedsamples = 1;
                    }
                    else
                    {
                        //ms_log(2, "msr_encode_steim2(%s): Unable to represent difference in <= 30 bits\n", srcname);
                        return -1;
                    }

                    /* Swap encoded word except for 4x8-bit samples */
                    if (swapflag != 0 && packedsamples != 4)
                    {
                        //ms_gswap4a(frameptr[widx]);
                        gswap.ms_gswap4a(ref frameptr[widx]);
                        output[widx + frameptr_index] = (int)frameptr[widx];

                    }

                    diffcount -= packedsamples;
                    outputsamples += packedsamples;
                } // Done with words in frame

                /* Swap word with nibbles */
                if (swapflag != 0)
                {
                    //ms_gswap4a(frameptr[0]);
                    gswap.ms_gswap4a(ref frameptr[0]);
                    output[0 + frameptr_index] = (int)frameptr[0];
                }
            } // Done with frames

            /* Set Xn (reverse integration constant) in first frame to last sample */
            if (Xnp != null)
            {
                // *Xnp = *(input + outputsamples - 1);
                Xnp = input[outputsamples - 1];
                output[2] = (int)Xnp;
            }
            if (swapflag != 0)
            {
                // ms_gswap4a(Xnp);
                int xnp_int = (int)Xnp;
                gswap.ms_gswap4a(ref xnp_int);
                Xnp = (int?)xnp_int;
                output[2] = (int)Xnp;
            }

            /* Pad any remaining bytes */
            if ((frameidx * 64) < outputlength)
            {
                //memset(output + (frameidx * 16), 0, outputlength - (frameidx * 64));
                int start_index = frameidx * 16;
                int size = outputlength - (frameidx * 64);
                for (int i = start_index; i < size; i++)
                    output[i] = 0;
            }

            return (outputsamples);
        } // End of msr_encode_steim2()

        public static void Test_Steim1()
        {
            //Int32[] input = { 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 65888778, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8 };
            //Int32[] input = { 123565, 223565, 323565, 423565, 523565, 623565, 723565, 823565, 123565, 223565, 323565, 423565, 523565, 623565, 723565, 823565, 123565, 223565, 323565, 423565, 523565, 623565, 723565, 823565, 123565, 223565, 323565, 423565, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 123565, 223565, 323565, 423565, 523565, 623565, 723565, 823565, 123565, 223565, 323565, 423565, 523565, 623565, 723565, 823565, 123565, 223565, 323565, 423565, 523565, 623565, 723565, 823565, 123565, 223565, 323565, 423565, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 123565, 223565, 323565, 423565, 523565, 623565, 723565, 823565, 123565, 223565, 323565, 423565, 523565, 623565, 723565, 823565, 123565, 223565, 323565, 423565, 523565, 623565, 723565, 823565, 123565, 223565, 323565, 423565, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 123565, 223565, 323565, 423565, 523565, 623565, 723565, 823565, 123565, 223565, 323565, 423565, 523565, 623565, 723565, 823565, 123565, 223565, 323565, 423565, 523565, 623565, 723565, 823565, 123565, 223565, 323565, 423565, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8 };  256
            Int32[] input = { 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8 };
            Int32[] output = new int[256];
            int diff0 = 0;
            var len = msr_encode_steim1(input, 16, ref output, 256, diff0, 1);
            for (int j = 0; j < len/*output.Length*/; j++)
            {
                Console.WriteLine("Element[" + j + "] = " + output[j] + "\n");
            }
            Console.WriteLine("len = " + len);

        }

        public static void Test_Steim2()
        {
            //Int32[] input = { 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 65888778, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8 };
            //Int32[] input = { 123565, 223565, 323565, 423565, 523565, 623565, 723565, 823565, 123565, 223565, 323565, 423565, 523565, 623565, 723565, 823565, 123565, 223565, 323565, 423565, 523565, 623565, 723565, 823565, 123565, 223565, 323565, 423565, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 123565, 223565, 323565, 423565, 523565, 623565, 723565, 823565, 123565, 223565, 323565, 423565, 523565, 623565, 723565, 823565, 123565, 223565, 323565, 423565, 523565, 623565, 723565, 823565, 123565, 223565, 323565, 423565, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 123565, 223565, 323565, 423565, 523565, 623565, 723565, 823565, 123565, 223565, 323565, 423565, 523565, 623565, 723565, 823565, 123565, 223565, 323565, 423565, 523565, 623565, 723565, 823565, 123565, 223565, 323565, 423565, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 123565, 223565, 323565, 423565, 523565, 623565, 723565, 823565, 123565, 223565, 323565, 423565, 523565, 623565, 723565, 823565, 123565, 223565, 323565, 423565, 523565, 623565, 723565, 823565, 123565, 223565, 323565, 423565, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8 };256
            Int32[] input = { 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8 };

            Int32[] output = new int[256];
            int diff0 = 0;
            string srcname = "a";
            var len = msr_encode_steim2(input, 16, ref output, 256, diff0, ref srcname, 1);
            for (int j = 0; j < len/*output.Length*/; j++)
            {
                Console.WriteLine("Element[" + j + "] = " + output[j] + "\n");
            }
            Console.WriteLine("len = " + len);

        }

    }
}
