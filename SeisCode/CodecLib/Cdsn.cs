using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodecLib
{
    public class Cdsn {

	/* mask for mantissa */
	public static readonly int MANTISSA_MASK = 0x3fff;

	/* mask for gainrange factor */
	public static readonly int GAINRANGE_MASK = 0x0003;

	/* # bits in mantissa */
	public static readonly int SHIFT = 14;

	/* maximum 14 bit positive # */
	public static readonly int MAX14 = 0x1fff;

	public static int[] Decode(sbyte[] b, int numSamples, bool swapBytes)
			 {
		if (b.Length < numSamples * 2) {
			throw new CodecException("Not enough bytes for " + numSamples
					+ " samples, need " + (2 * numSamples) + " but only have "
					+ b.Length);
		}
		int[] out_1 = new int[numSamples];
		for (int j = 0; j < out_1.Length; j++) {

			int mult = 0;
			int temp = Utility.BytesToShort(b[2 * j], b[2 * j + 1], swapBytes);
			int mantissa = temp & MANTISSA_MASK;
			int gainrange = (temp >> SHIFT ) & GAINRANGE_MASK;

			/* determine multiplier from gain range factor and format definition */
			/* because shift operator is used later, these are powers of two */
			if (gainrange == 0) {
				mult = 0;
			} else if (gainrange == 1) {
				mult = 2;
			} else if (gainrange == 2) {
				mult = 4;
			} else if (gainrange == 3) {
				mult = 7;
			}

			/* unbias the mantissa */
			mantissa -= MAX14;

			/* calculate sample from mantissa and multiplier using left shift */
			/* mantissa << mult is equivalent to mantissa * (2 exp (mult)) */
			out_1[j] = (mantissa << mult);

		}
		return out_1;
	}

}
}
