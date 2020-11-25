using System;
using System.IO;
using System.Text;

namespace SeisCode
{
	public class Blockette200 : DataBlockette
	{

		public Blockette200(float signal, float period, float background, Btime signalOnset, string eventDetector) : base(B200_SIZE)
		{
			Utility.insertFloat(signal, info, SIGNAL);
			Utility.insertFloat(period, info, PERIOD);
			Utility.insertFloat(background, info, BACKGROUND);
			byte[] onsetBytes = signalOnset.Bytes;
			Array.Copy(onsetBytes, 0, info, SIGNAL_ONSET, onsetBytes.Length);
			if (eventDetector.Length > EVENT_DETECTOR_LENGTH)
			{
				throw new System.ArgumentException("The event detector can only be up to " + EVENT_DETECTOR_LENGTH + " characters in length");
			}
			byte[] detectorBytes;
			try
			{
				detectorBytes = Encoding.ASCII.GetBytes(eventDetector);
			}
			catch (Exception e)
			{
				throw new Exception("RuntimeException(Unable to find the US-ASCII character encoding) Error:" + e.Message);
			}
			if (detectorBytes.Length != eventDetector.Length)
			{
				throw new ArgumentException("The characters in event detector must be in the ASCII character set i.e. from 0-127");
			}
			detectorBytes = Utility.pad(detectorBytes, EVENT_DETECTOR_LENGTH, (byte)' ');
			Array.Copy(detectorBytes, 0, info, EVENT_DETECTOR, detectorBytes.Length);
		}

		
		public Blockette200(byte[] info, bool swapBytes) : base(info, swapBytes)
		{
			TrimToSize(Size);
		}

		public override string Name
		{
			get
			{
				return "Generic Event Detection Blockette";
			}
		}

		public override int Size
		{
			get
			{
				return B200_SIZE;
			}
		}

		public override int Type
		{
			get
			{
				return 200;
			}
		}

		/// <returns> - the signal amplitude field </returns>
		public virtual float Signal
		{
			get
			{
				return Utility.bytesToInt(info, SIGNAL, swapBytes);
			}
		}

		/// <returns> - the signal period field </returns>
		public virtual float Period
		{
			get
			{
				return Utility.bytesToInt(info, PERIOD, swapBytes);
			}
		}

		/// <returns> - the background estimate field </returns>
		public virtual float Background
		{
			get
			{
				//return Float.intBitsToFloat(Utility.bytesToInt(info, BACKGROUND, swapBytes));
				return Utility.bytesToInt(info, BACKGROUND, swapBytes);
			}
		}

		/// <returns> - the signal onset time field </returns>
		public virtual Btime SignalOnset
		{
			get
			{
				return new Btime(info, SIGNAL_ONSET);
			}
		}

		public virtual string EventDetector
		{
			get
			{
				//return new string(info, EVENT_DETECTOR, EVENT_DETECTOR_LENGTH);
				return new string(Encoding.UTF8.GetString(info).ToCharArray(), EVENT_DETECTOR, EVENT_DETECTOR_LENGTH);

			}
		}

		public override void WriteASCII(TextWriter @out)
		{
			@out.WriteLine("Blockette200 sig=" + Signal + " per=" + Period + " bkgrd=" + Background);
		}

		public override bool Equals(object o)
		{
			if (o == this)
			{
				return true;
			}
			if (o is Blockette200)
			{
				byte[] oinfo = ((Blockette200)o).info;
				if (info.Length != oinfo.Length)
				{
					return false;
				}
				for (int i = 0; i < oinfo.Length; i++)
				{
					if (info[i] != oinfo[i])
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        // Offsets for various fields
        private const int SIGNAL = 4;

		private const int PERIOD = 8;

		private const int BACKGROUND = 12;

		private const int SIGNAL_ONSET = 18;

		private const int EVENT_DETECTOR = 28;

		// Full size of blockette 200
		private const int B200_SIZE = 52;

		private const int EVENT_DETECTOR_LENGTH = 24;
	}
}
