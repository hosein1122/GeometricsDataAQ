using System;
using System.Collections.Generic;
using System.Text;

namespace SeisCode
{
	public class MissingBlockette1000 : SeedFormatException
	{


		public MissingBlockette1000(ControlHeader header) : base(header)
		{
		}

		public MissingBlockette1000(string s, ControlHeader header) : base(s, header)
		{
		}

		public MissingBlockette1000(Exception cause, ControlHeader header) : base(cause, header)
		{
		}

		public MissingBlockette1000(string s, Exception cause, ControlHeader header) : base(s, cause, header)
		{
		}
	}

}
