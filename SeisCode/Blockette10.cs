using System;
using System.Collections.Generic;
using System.Text;

namespace SeisCode
{
	public class Blockette10 : ControlRecordLengthBlockette
	{

		public Blockette10(sbyte[] info) : base(info)
		{
		}

		public override int Type
		{
			get
			{
				return 10;
			}
		}

		public override string Name
		{
			get
			{
				return "Volume Identifier Blockette";
			}
		}

		public virtual string BeginningTime
		{
			get
			{
				return Utility.extractVarString(info, 23, 22);
			}
		}

		public virtual string EndTime
		{
			get
			{
				return Utility.extractVarString(info, 23 + BeginningTime.Length + 1, 22);
			}
		}

		public virtual string VolumeTime
		{
			get
			{
				return Utility.extractVarString(info, 23 + BeginningTime.Length + 1 + EndTime.Length + 1, 22);
			}
		}

		public virtual string OriginatingOrganization
		{
			get
			{
				return Utility.extractVarString(info, 23 + BeginningTime.Length + 1 + EndTime.Length + 1 + VolumeTime.Length + 1, 80);
			}
		}

		public virtual string Label
		{
			get
			{
				return Utility.extractVarString(info, 23 + BeginningTime.Length + 1 + EndTime.Length + 1 + VolumeTime.Length + 1 + OriginatingOrganization.Length + 1, 80);
			}
		}

	}

}
