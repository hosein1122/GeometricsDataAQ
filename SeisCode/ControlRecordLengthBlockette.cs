using System;
using System.Collections.Generic;
using System.Text;

namespace SeisCode
{
	public abstract class ControlRecordLengthBlockette : ControlBlockette, RecordLengthBlockette
	{

		public ControlRecordLengthBlockette(sbyte[] info) : base(info)
		{
		}

		public virtual string VersionOfFormat
		{
			get
			{
				return Utility.extractString(info, 7, 4);
			}
		}

		public virtual int LogicalRecordLengthByte
		{
			get
			{
				return Utility.extractInteger(info, 11, 2);
			}
		}

		public virtual int LogicalRecordLength
		{
			get
			{
				if (LogicalRecordLengthByte < 31)
				{
					return (0x01 << LogicalRecordLengthByte);
				}
				else
				{
					throw new Exception("Data Record Length exceeds size of int");
				}
			}
		}


		public virtual void writeASCII(PrintWriter @out)
		{
			writeASCIINoNewline(@out);
			@out.println();
		}

		public virtual void writeASCIINoNewline(PrintWriter @out)
		{
			@out.print("Blockette" + Type + " record length=" + LogicalRecordLength + " (" + LogicalRecordLengthByte + ") " + new string(info));
		}
	}

}
