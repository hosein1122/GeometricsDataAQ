using System;
using System.Collections.Generic;
using System.Text;

namespace SeisCode
{
	public class DataTooLargeException : SeedFormatException
	{

		public DataTooLargeException()
		{
			// TODO Auto-generated constructor stub
		}

		public DataTooLargeException(string s) : base(s)
		{
			// TODO Auto-generated constructor stub
		}

		public DataTooLargeException(Exception cause) : base(cause)
		{
			// TODO Auto-generated constructor stub
		}

		public DataTooLargeException(string s, Exception cause) : base(s, cause)
		{
			// TODO Auto-generated constructor stub
		}

		public DataTooLargeException(string s, DataHeader header) : base(s, header)
		{
			// TODO Auto-generated constructor stub
		}

		public DataTooLargeException(Exception cause, DataHeader header) : base(cause, header)
		{
			// TODO Auto-generated constructor stub
		}

		public DataTooLargeException(string s, Exception cause, DataHeader header) : base(s, cause, header)
		{
			// TODO Auto-generated constructor stub
		}
	}

}
