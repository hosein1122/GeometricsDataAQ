using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SeisCode
{
	public class Blockette8 : ControlRecordLengthBlockette
	{

		public Blockette8(byte[] info) : base(info)
		{
		}

		public override int Type
		{
			get
			{
				return 8;
			}
		}

		public virtual string StationIdentifier
		{
			get
			{
				return Utility.extractString(info, 13, 5);
			}
		}

		public virtual string LocationIdentifier
		{
			get
			{
				return Utility.extractString(info, 18, 2);
			}
		}

		public virtual string ChannelIdentifier
		{
			get
			{
				return Utility.extractString(info, 20, 3);
			}
		}

		public virtual string NetworkIdentifier
		{
			get
			{
				return Utility.extractString(info, info.Length - 2, 2);
			}
		}

		public virtual string BeginningOfVolume
		{
			get
			{
				return Utility.extractVarString(info, 23, 22);
			}
		}

		public virtual string EndOfVolume
		{
			get
			{
				return Utility.extractVarString(info, 23 + BeginningOfVolume.Length + 1, 22);
			}
		}

		public virtual string StationInformationEffectiveDate
		{
			get
			{
				return Utility.extractVarString(info, 23 + BeginningOfVolume.Length + 1 + EndOfVolume.Length + 1, 22);
			}
		}

		public virtual string ChannelInformationEffectiveDate
		{
			get
			{
				return Utility.extractVarString(info, 23 + BeginningOfVolume.Length + 1 + EndOfVolume.Length + 1 + StationInformationEffectiveDate.Length + 1, 22);
			}
		}

		public override string Name
		{
			get
			{
				return "Telemetry Volume Identifier Blockette";
			}
		}

        public override void WriteASCII(TextWriter @out)
        {
			return;
        }
    }

}
