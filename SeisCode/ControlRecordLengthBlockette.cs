using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SeisCode
{
	public abstract class ControlRecordLengthBlockette : ControlBlockette, RecordLengthBlockette
	{

		public ControlRecordLengthBlockette(byte[] info) : base(info)
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


		public override void WriteASCII(TextWriter @out)
		{
			WriteASCIINoNewline(@out);
			@out.WriteLine();
		}

		public virtual void WriteASCIINoNewline(TextWriter @out)
		{
			@out.Write("Blockette" + Type + " record length=" + LogicalRecordLength + " (" + LogicalRecordLengthByte + ") " + Encoding.Default.GetString(info));
		}
	}

}
