using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodecLib
{
    public class Codec:B1000Types {

    public Codec() {}

    public bool IsDecompressable(int type) {
        switch(type){
            case SHORT:
            case DWWSSN:
            case INT24:
            case INTEGER:
            case FLOAT:
            case DOUBLE:
            case STEIM1:
            case STEIM2:
            case CDSN:
            case SRO:
                return true;
            default:
                return false;
        }
    }
    
    /**
     * Decompresses the data into the best java primitive type for the given
     * compression and returns it.
     */
    
        /*
         * my change
         * 1- Double.doubleToLongBits ====> BitConverter.DoubleToInt64Bits
         * 2- delete throws CodecException, UnsupportedCompressionType
         * */
    public DecompressedData Decompress(int type,
                                       sbyte[] b,
                                       int numSamples,
                                       bool swapBytes)
             {
        // in case of record with no data points, ex detection blockette, which often have compression type
        // set to 0, which messes up the decompresser even though it doesn't matter since there is no data.
        if (numSamples == 0) {
            return new DecompressedData(new int[0]);
        }
        
        DecompressedData @out;
        int[] itemp;
        short[] stemp;
        float[] ftemp;
        double[] dtemp;
        int offset = 0;
        switch(type){
            case SHORT:
            case DWWSSN:
                // 16 bit values
                if(b.Length < 2 * numSamples) {
                    throw new CodecException("Not enough bytes for "
                            + numSamples + " 16 bit data points, only "
                            + b.Length + " bytes.");
                }
                stemp = new short[numSamples];
                for(int i = 0; i < stemp.Length; i++) {
                    stemp[i] = Utility.BytesToShort(b[offset],
                                                    b[offset + 1],
                                                    swapBytes);
                    offset += 2;
                }
                @out = new DecompressedData(stemp);
                break;
            case INT24:
                // 24 bit values
                if(b.Length < 3 * numSamples) {
                    throw new CodecException("Not enough bytes for "
                            + numSamples + " 24 bit data points, only "
                            + b.Length + " bytes.");
                }
                itemp = new int[numSamples];
                for(int i = 0; i < numSamples; i++) {
                    itemp[i] = Utility.BytesToInt(b[offset],
                                                  b[offset + 1],
                                                  b[offset + 2],
                                                  swapBytes);
                    offset += 3;
                }
                @out = new DecompressedData(itemp);
                break;
            case INTEGER:
                // 32 bit integers
                if(b.Length < 4 * numSamples) {
                    throw new CodecException("Not enough bytes for "
                            + numSamples + " 32 bit data points, only "
                            + b.Length + " bytes.");
                }
                itemp = new int[numSamples];
                for(int i = 0; i < numSamples; i++) {
                    itemp[i] = Utility.BytesToInt(b[offset],
                                                  b[offset + 1],
                                                  b[offset + 2],
                                                  b[offset + 3],
                                                  swapBytes);
                    offset += 4;
                }
                @out = new DecompressedData(itemp);
                break;
            case FLOAT:
                // 32 bit floats
                if(b.Length < 4 * numSamples) {
                    throw new CodecException("Not enough bytes for "
                            + numSamples + " 32 bit data points, only "
                            + b.Length + " bytes.");
                }
                ftemp = new float[numSamples];
                for(int i = 0; i < numSamples; i++) {
                    //Float.intBitsToFloat ======>  FloatToUInt32Bits

                    ftemp[i] = FloatToUInt32Bits(Utility.BytesToInt(b[offset],
                                                                       b[offset + 1],
                                                                       b[offset + 2],
                                                                       b[offset + 3],
                                                                       swapBytes));
                    offset += 4;
                }
                @out = new DecompressedData(ftemp);
                break;
            case DOUBLE:
                // 64 bit doubles
                if(b.Length < 8 * numSamples) {
                    throw new CodecException("Not enough bytes for "
                            + numSamples + " 64 bit data points, only "
                            + b.Length + " bytes.");
                }

                dtemp = new double[numSamples];
                for(int i = 0; i < numSamples; i++) {
                    dtemp[i] = BitConverter.DoubleToInt64Bits(Utility.BytesToLong(b[offset],
                                                                        b[offset + 1],
                                                                        b[offset + 2],
                                                                        b[offset + 3],
                                                                        b[offset + 4],
                                                                        b[offset + 5],
                                                                        b[offset + 6],
                                                                        b[offset + 7],
                                                                       swapBytes));
                    offset += 8;
                }
                @out = new DecompressedData(dtemp);
                break;
            case STEIM1:
                // steim 1
                itemp = Steim1.Decode(b, numSamples, swapBytes, 0);
                @out = new DecompressedData(itemp);
                break;
            case STEIM2:
                // steim 2
                itemp = Steim2.Decode(b, numSamples, swapBytes, 0);
                @out = new DecompressedData(itemp);
                break;
            case CDSN:
                itemp = Cdsn.Decode(b, numSamples, swapBytes);
                @out = new DecompressedData(itemp);
                break;
            case SRO:
                itemp = Sro.Decode(b, numSamples, swapBytes);
                @out = new DecompressedData(itemp);
                break;    
            default:
                // unknown format????
                throw new Exception( "UnsupportedCompressionType(Type " + type
                        + " is not supported at this time.");
        } // end of switch ()
        return @out;
    }

    /**
     * returns an integer that represent the java primitive that the data will
     * decompresses to. This is to allow for SEED types 4 and 5, float and
     * double, which cannot be represented as int without a loss of precision.
     */
    /*
     *1-  delete throws UnsupportedCompressionType 
     */
    public int GetDecompressedType(int type)  {
        if(type == INT24 || type == INTEGER || type == STEIM1 || type == STEIM2 || type == CDSN || type == SRO) {
            return INTEGER;
        } else if(type == SHORT || type == DWWSSN) {
            return SHORT;
        } else if(type == FLOAT) {
            return FLOAT;
        } else if(type == DOUBLE) {
            return DOUBLE;
        } // end of if ()
        // ????
        throw new Exception( "UnsupportedCompressionType; ( Type " + type
                + " is not supported at this time.");
    }
        
    /** encodes the short data as a byte array. This is the inverse operation to decompress() with seed type 1 - 16 bit integers. */
    public byte[] EncodeAsBytes(short[] data) {
        byte[] dataBytes = new byte[data.Length*2];
        for (int i = 0; i < data.Length; i++) {
            dataBytes[2 * i    ] = (byte)((data[i] & 0x0000ff00) >> 8);
            dataBytes[2 * i + 1] = (byte)((data[i] & 0x000000ff));
        }
        return dataBytes;
    }
    
    /** encodes the integer data as a byte array. This is the inverse operation to decompress() with seed type 3 - 32 bit integers. */
    public byte[] EncodeAsBytes(int[] data) {
        byte[] dataBytes = new byte[data.Length*4];
        for (int i = 0; i < data.Length; i++) {
            dataBytes[4 * i] = (byte)((data[i] & 0xff000000) >> 24);
            dataBytes[4 * i + 1] = (byte)((data[i] & 0x00ff0000) >> 16);
            dataBytes[4 * i + 2] = (byte)((data[i] & 0x0000ff00) >> 8);
            dataBytes[4 * i + 3] = (byte)((data[i] & 0x000000ff));
        }
        return dataBytes;
    }

    /** encodes the float data as a byte array. This is the inverse operation to decompress() with seed type 4 - 32 bit floats. */
    /*
     * 1- Float.floatToIntBits ======> FloatToUInt32Bits
     * */
    public byte[] EncodeAsBytes(float[] data) {
        int[] tmp = new int[data.Length];
        for (int i = 0; i < data.Length; i++) {
            tmp[i] = FloatToUInt32Bits(data[i]);
        }
        return EncodeAsBytes(tmp);
    }
    
    /** encodes the float data as a byte array. This is the inverse operation to decompress() with seed type 5 - 64 bit floats. */
    /*
     * 1- val======> (ulong)val
     * */
    public byte[] EncodeAsBytes(double[] data) {
        byte[] dataBytes = new byte[data.Length*8];
        int byteOffset = 0;
        for (int i = 0; i < data.Length; i++) {
            long val = BitConverter.DoubleToInt64Bits(data[i]);
            byteOffset = 8*i;
            dataBytes[byteOffset    ] = (byte)(((ulong)val & 0xff00000000000000L) >> 56);
            dataBytes[byteOffset + 1] = (byte)((val & 0x00ff000000000000L) >> 48);
            dataBytes[byteOffset + 2] = (byte)((val & 0x0000ff0000000000L) >> 40);
            dataBytes[byteOffset + 3] = (byte)((val & 0x000000ff00000000L) >> 32);
            dataBytes[byteOffset + 4] = (byte)((val & 0x00000000ff000000L) >> 24);
            dataBytes[byteOffset + 5] = (byte)((val & 0x0000000000ff0000L) >> 16);
            dataBytes[byteOffset + 6] = (byte)((val & 0x000000000000ff00L) >> 8);
            dataBytes[byteOffset + 7] = (byte)((val & 0x00000000000000ffL));
        }
        return dataBytes;
    }
        /// <summary>
        /// added by me!
         /// to simulate Float.floatToIntBits
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
    public static  int FloatToUInt32Bits(float f)
    {
            uint value = BitConverter.ToUInt32(BitConverter.GetBytes(f), 0);
            return (int)value;
    }
    
}// Codec
}
