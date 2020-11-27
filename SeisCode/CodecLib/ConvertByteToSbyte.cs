using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodecLib
{
    public class ConvertByteToSbyte
    {
        public static sbyte[] ByteToSbyte(byte[] byte_array)
        {
            sbyte[] java_byte = new sbyte[byte_array.Length];
            //var byte_array = byte_array.ToArray();
            for (int i = 0; i < byte_array.Length; i++)
                java_byte[i] = (sbyte)(byte_array[i]);
            return java_byte;
        }

    }
}
