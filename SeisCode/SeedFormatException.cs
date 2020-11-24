using System;
using System.Collections.Generic;
using System.Text;

namespace SeisCode
{
	public class SeedFormatException : SeisFileException
	{

		public SeedFormatException() : base()
		{
		}

		public SeedFormatException(ControlHeader header) : base()
		{
			this.header = header;
		}

		public SeedFormatException(string s) : base(s)
		{
		}

		public SeedFormatException(Exception cause) : base(cause)
		{
		}

		public SeedFormatException(string s, Exception cause) : base(s, cause)
		{
		}

		public SeedFormatException(string s, ControlHeader header) : base(s)
		{
			this.header = header;
		}

		public SeedFormatException(Exception cause, ControlHeader header) : base(cause)
		{
			this.header = header;
		}

		public SeedFormatException(string s, Exception cause, ControlHeader header) : base(s, cause)
		{
			this.header = header;
		}


		public virtual ControlHeader Header
		{
			get
			{
				return header;
			}
			set
			{
				this.header = value;
			}
		}



		public override string ToString()
		{
			if (header != null)
			{
				return base.ToString() + " " + header.ToString();
			}
			return base.ToString();
		}

		private ControlHeader header;
	}

}
