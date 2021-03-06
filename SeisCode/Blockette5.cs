﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SeisCode
{
	public class Blockette5 : ControlRecordLengthBlockette
	{

		public Blockette5(byte[] info) : base(info)
		{
		}

		public override int Type
		{
			get
			{
				return 5;
			}
		}

		public override string Name
		{
			get
			{
				return "Field Volume Identifier Blockette";
			}
		}

		public virtual string BeginningOfVolume
		{
			get
			{
				return Utility.extractVarString(info, 13, 22);
			}
		}

		public override void WriteASCII(TextWriter @out)
		{
			base.WriteASCIINoNewline(@out);
			@out.WriteLine(" beg vol=" + BeginningOfVolume);
		}
	}

}
