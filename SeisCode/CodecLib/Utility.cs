using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodecLib
{
    public class Utility
    {
        //###################################################################
        //###################################################################
        //###################################################################
        //###################################################################
        // i converted all bytes keywords to sbytes to match results by java version
        // m.rahmati
        //###################################################################
        //###################################################################
        //###################################################################
        //###################################################################
        //###################################################################

        /**
         * Concatenate two bytes to a short integer value.  Accepts a high and low byte to be
         * converted to a 16-bit integer.  if swapBytes is true, then <b>b</b> becomes
         * the high order byte.
         * @param a high order byte
         * @param b low order byte
         * @param swapBytes reverse the roles of the first two parameters
         * @return short integer representation of the concatenated bytes
         */
        public static short BytesToShort(sbyte a, sbyte b, bool swapBytes)
        {
            if (swapBytes)
            {
                return (short)((a & 0xff) + (b & 0xff) << 8);
            }
            else
            {
                return (short)(((a & 0xff) << 8) + (b & 0xff));
            }
        }


        // convert signed bytes to an integer value

        /**
         * Convert a single byte to a 32-bit int, with sign extension.
         * @param a signed byte value
         * @return 32-bit integer
         */
        public static int BytesToInt(sbyte a)
        {
            return (int)a;  // whatever the high-order bit is set to is extended into integer 32-bit space
        }

        /**
         * Concatenate two bytes to a 32-bit int value.  <b>a</b> is the high order
         * byte in the resulting int representation, unless swapBytes is true, in
         * which <b>b</b> is the high order byte.
         * @param a high order byte
         * @param b low order byte
         * @param swapBytes byte order swap flag
         * @return 32-bit integer
         */
        public static int BytesToInt(sbyte a, sbyte b, bool swapBytes)
        {
            // again, high order bit is expressed left into 32-bit form
            if (swapBytes)
            {
                return (a & 0xff) + ((int)b << 8);
            }
            else
            {
                return ((int)a << 8) + (b & 0xff);
            }
        }

        /**
         * Concatenate three bytes to a 32-bit int value.  Byte order is <b>a,b,c</b>
         * unless swapBytes is true, in which case the order is <b>c,b,a</b>.
         * @param a highest order byte
         * @param b second-highest order byte
         * @param c lowest order byte
         * @param swapBytes byte order swap flag
         * @return 32-bit integer
         */
        public static int BytesToInt(sbyte a, sbyte b, sbyte c, bool swapBytes)
        {
            if (swapBytes)
            {
                return (a & 0xff) + ((b & 0xff) << 8) + ((int)c << 16);
            }
            else
            {
                return ((int)a << 16) + ((b & 0xff) << 8) + (c & 0xff);
            }
        }


        /**
         * Concatenate four bytes to a 32-bit int value.  Byte order is <b>a,b,c,d</b>
         * unless swapBytes is true, in which case the order is <b>d,c,b,a</b>.
         * <i>Note:</i> This method will accept unsigned and signed byte
         * representations, since high bit extension is not a concern here.
         * Java does not support unsigned integers, so the maximum value is not as
         * high as would be the case with an unsigned integer.  To hold an unsigned
         * 32-bit value, use uBytesToLong().
         * @param a highest order byte
         * @param b second-highest order byte
         * @param c second-lowest order byte
         * @param d lowest order byte
         * @param swapBytes byte order swap flag
         * @return 32-bit integer
         * see edu.iris.Fissures.seed.util.Utility#uBytesToLong(byte,byte,byte,byte,boolean)
         */
        public static int BytesToInt(sbyte a, sbyte b, sbyte c, sbyte d, bool swapBytes)
        {
            if (swapBytes)
            {
                return ((a & 0xff)) +
                    ((b & 0xff) << 8) +
                    ((c & 0xff) << 16) +
                    ((d & 0xff) << 24);
            }
            else
            {
                return ((a & 0xff) << 24) +
                    ((b & 0xff) << 16) +
                    ((c & 0xff) << 8) +
                    ((d & 0xff));
            }
        }

        /**
         * Concatenate eight bytes to a 64-bit int value.  Byte order is <b>a,b,c,d,e,f,g,h</b>
         * unless swapBytes is true, in which case the order is <b>h,g,f,e,d,c,b,a</b>.
         * <i>Note:</i> This method will accept unsigned and signed byte
         * representations, since high bit extension is not a concern here.
         * Java does not support unsigned long integers, so the maximum value is not as
         * high as would be the case with an unsigned integer.  
         * @param a highest order byte
         * @param b second-highest order byte
         * @param c next order byte
         * @param d next order byte
         * @param e next order byte
         * @param f next order byte
         * @param g next order byte
         * @param h lowest order byte
         * @param swapBytes byte order swap flag
         * @return 64-bit long
         * see edu.iris.Fissures.seed.util.Utility#uBytesToLong(byte,byte,byte,byte,boolean)
         */
        public static long BytesToLong(sbyte a,
                                     sbyte b,
                                     sbyte c,
                                     sbyte d,
                                     sbyte e,
                                     sbyte f,
                                     sbyte g,
                                     sbyte h,
                                     bool swapBytes)
        {
            if (swapBytes)
            {
                return ((a & 0xffL)) + ((b & 0xffL) << 8) + ((c & 0xffL) << 16)
                        + ((d & 0xffL) << 24) + ((e & 0xffL) << 32) + ((f & 0xffL) << 40) + ((g & 0xffL) << 48)
                        + ((h & 0xffL) << 56);
            }
            else
            {
                return ((a & 0xffL) << 56) + ((b & 0xffL) << 48) + ((c & 0xffL) << 40)
                        + ((d & 0xffL) << 32) + ((e & 0xffL) << 24) + ((f & 0xffL) << 16) + ((g & 0xffL) << 8)
                        + ((h & 0xffL));
            }
        }


        // convert unsigned byte representations to an integer value

        /**
         * Treat byte value as an unsigned value and convert to a 32-bit int value.
         * @param a unsigned byte value
         * @return positive 32-bit integer
         */
        public static int UBytesToInt(sbyte a)
        {
            // we "and" with 0xff in order to get the sign correct (pos)
            // this extends zeroes left into 32-bit	space
            return a & 0xff;
        }

        /**
         * Conatenate two unsigned byte values into a 32-bit integer.
         * @param a high order unsigned byte
         * @param b low order unsigned byte
         * @param swapBytes if true, <b>b</b> becomes the high order byte
         * @return positive 32-bit integer
         */
        public static int UBytesToInt(sbyte a, sbyte b, bool swapBytes)
        {
            // we "and" with 0xff to get the sign correct (pos)
            if (swapBytes)
            {
                return (a & 0xff) + ((b & 0xff) << 8);
            }
            else
            {
                return ((a & 0xff) << 8) + (b & 0xff);
            }
        }

        /**
         * Conacatenate four unsigned byte values into a long integer.
         * This method puts out a long value because a large unsigned 32-bit value would
         * exceed the capacity of an int, which is considered signed in Java.
         * @param a highest-order byte
         * @param b second-highest order byte
         * @param c second-lowest order byte
         * @param d lowest order byte
         * @param swapBytes if true, byte order is <b>d,c,b,a</b>, else order is
         * <b>a,b,c,d</b>
         * @return positive long integer
         */
        public static long UBytesToLong(sbyte a, sbyte b, sbyte c, sbyte d, bool swapBytes)
        {
            if (swapBytes)
            {
                return ((a & 0xffL)) +
                    ((b & 0xffL) << 8) +
                    ((c & 0xffL) << 16) +
                    ((d & 0xffL) << 24);
            }
            else
            {
                return ((a & 0xffL) << 24) +
                    ((b & 0xffL) << 16) +
                    ((c & 0xffL) << 8) +
                    ((d & 0xffL));
            }
        }

        /**
         * Convert a long value to a 4-byte array.
         * @param a long integer
         * @return byte[4] array
         */
        public static sbyte[] LongToIntBytes(long a)
        {
            sbyte[] returnByteArray = new sbyte[4]; //int is 4 bytes
            returnByteArray[0] = (sbyte)((a & 0xff000000) >> 24);
            returnByteArray[1] = (sbyte)((a & 0x00ff0000) >> 16);
            returnByteArray[2] = (sbyte)((a & 0x0000ff00) >> 8);
            returnByteArray[3] = (sbyte)((a & 0x000000ff));
            return returnByteArray;
        }

        /**
         * Convert an int value to a 2-byte array.
         * @param a int value
         * @return byte[2] array
         */
        public static sbyte[] IntToShortBytes(int a)
        {
            sbyte[] returnByteArray = new sbyte[2];  //short is 2 bytes
            returnByteArray[0] = (sbyte)((a & 0x0000ff00) >> 8);
            returnByteArray[1] = (sbyte)((a & 0x000000ff));
            return returnByteArray;
        }


        // miscellaneous utilities

        /**
         * Return a byte array of Length <b>requiredBytes</b> that contains the
         * contents of <b>source</b> and is padded on the end with <b>paddingByte</b>.
         * If <b>requiredBytes</b> is less than or equal to the Length of
         * <b>source</b>, then <b>source</b> will simply be returned.
         * @param source byte array to have <b>paddingByte</b>(s) appended to
         * @param requiredBytes the Length in bytes of the returned byte array
         * @param paddingByte the byte value that will be appended to the array to
         * fill out the required byte size of the return array
         * @return byte array of size <b>requiredBytes</b>
         */
        public static sbyte[] Pad(sbyte[] source, int requiredBytes, sbyte paddingByte)
        {
            if (source.Length >= requiredBytes)
            {
                return source;
            }
            else
            {
                sbyte[] returnByteArray = new sbyte[requiredBytes];
                //System.arraycopy(source, 0, returnByteArray, 0, source.Length);
               Array.Copy(source, 0, returnByteArray, 0, source.Length);
                for (int i = source.Length; i < requiredBytes; i++)
                {
                    returnByteArray[i] = (sbyte)paddingByte;
                }
                return returnByteArray;
            }
        }

        /**
         * Return a byte array which is a subset of bytes from <b>source</b>
         * beginning with index <b>start</b> and stopping just before index
         * <b>end</b>.
         * @param source source byte array
         * @param start starting index, inclusive
         * @param end ending index, exclusive
         * @return byte array of Length <b>start</b>-<b>end</b>
         */
        public static sbyte[] Format(sbyte[] source, int start, int end)
        {
            sbyte[] returnByteArray = new sbyte[start - end + 1];
            int j = 0;
            for (int i = start; i < end; i++, j++)
            {
                returnByteArray[j] = source[i];
            }
            return returnByteArray;
        }


        /**
         * Test method.
         * @param args not used.
         */
        public static void main(string[] args)
	{
		int a = 256;
		sbyte a1 = (sbyte)((a & 0xff000000)>>24);
		sbyte a2 = (sbyte)((a & 0x00ff0000)>>16);
		sbyte a3 = (sbyte)((a & 0x0000ff00)>>8);
		sbyte a4 = (sbyte) ((a & 0x000000ff));
		//System.out.println("first byte is " + a1);
		//System.out.println("2 byte is " + a2);
		//System.out.println("3 byte is " + a3);
		//System.out.println("4  byte is " + a4);

        Console.WriteLine("first byte is " + a1);
		 Console.WriteLine("2 byte is " + a2);
		 Console.WriteLine("3 byte is " + a3);
		 Console.WriteLine("4  byte is " + a4);
		sbyte[] source = new sbyte[5];
		for(int i=0; i< 5; i++)
			source[i] = (sbyte)10;
		sbyte[] output = Utility.Pad(source, 5, (sbyte)32);
		for(int k=output.Length-1; k > -1; k--)
		{
			//System.out.println("byte" + k +" " + output[k]);
             Console.WriteLine("byte" + k +" " + output[k]);
		}
	}

    }
}
