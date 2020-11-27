using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodecLib
{
    public class DecompressedData:B1000Types {

    public DecompressedData(int[] data) {
        this.iData = data;
    }
    public DecompressedData(short[] data) {
        this.sData = data;
    }
    public DecompressedData(float[] data) {
        this.fData = data;
    }
    public DecompressedData(double[] data) {
        this.dData = data;
    }


    /** returns an integer that represent the java primitive that the data
     *  decompresses to. This is to allow for SEED types 4 and 5, float and
     *  double, which cannot be represented as int without a loss of precision.
     *  <p>see B1000Types.java for the values.</p>
     *  <p>see http://www.fdsn.org for the seed manual, blockette 1000, that defines these values.</p>
     */
    public int GetDataType() {
        if (iData != null) {
            return INTEGER;
        } else  if (sData != null) {
            return SHORT;
        } else  if (fData != null) {
            return FLOAT;
        } else {
            // assume double
            return DOUBLE;
        } // end of else
    }

    /** returns a string version of the type for printing in error messages. */
    public string GetTypeString() {
        if (iData != null) {
            return "INTEGER";
        } else  if (sData != null) {
            return "SHORT";
        } else  if (fData != null) {
            return "FLOAT";
        } else {
            // assume double
            return "DOUBLE";
        } // end of else
    }

    /** Converts the data to an int array if possible without loss. Otherwise
     *  returns null.
     */
    public int[] GetAsInt() {
        int[] temp;
        if (iData != null) {
            return iData;
        } else if (sData != null) {
            temp = new int[sData.Length];
            for (int i=0; i<sData.Length; i++) {
                temp[i] = sData[i];
            }
            return temp;
        }
        return null;
    }

    /** Converts the data to a short array if possible without loss. Otherwise
     *  returns null.
     */
    public short[] GetAsShort() {
        if (sData != null) {
            return sData;
        }
        return null;
    }

    /** Converts the data to a float array if possible without loss. Otherwise
     *  returns null.
     */
    public float[] GetAsFloat() {
        float[] temp;
        if (fData != null) {
            return fData;
        } else if (iData != null) {
            temp = new float[iData.Length];
            for (int i=0; i<iData.Length; i++) {
                temp[i] = iData[i];
            }
            return temp;
        } else if (sData != null) {
            temp = new float[sData.Length];
            for (int i=0; i<sData.Length; i++) {
                temp[i] = sData[i];
            }
            return temp;
        }
        return null;
    }

    /** Converts the data to a double array if possible without loss. Otherwise
     *  returns null.
     */
    public double[] GetAsDouble() {
        //double[] temp;
        if (dData != null) {
            return dData;
        } else if (fData != null) {
            dData = new double[fData.Length];
            for (int i=0; i<fData.Length; i++) {
                dData[i] = fData[i];
            }
            return dData;
        } else if (iData != null) {
            dData = new double[iData.Length];
            for (int i=0; i<iData.Length; i++) {
                dData[i] = iData[i];
            }
            return dData;
        } else if (sData != null) {
            dData = new double[sData.Length];
            for (int i=0; i<sData.Length; i++) {
                dData[i] = sData[i];
            }
            return dData;
        }
        return null;
    }

    /** holds a temp int array of the data elements.
     */
    protected int[] iData = null;

    /** holds a temp short array of the data elements.
     */
    protected short[] sData = null;

    /** holds a temp float array of the data elements.
     */
    protected float[] fData = null;

    /** holds a temp double array of the data elements.
     */
    protected double[] dData = null;

}// DecompressedData
}
