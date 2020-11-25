using System;
using System.Collections.Generic;
using System.Text;

namespace SeisCode
{
	/// <summary>
	/// Convience class to hold begin and end Btime, along with simple methods to test if another
	/// Btime in inside the range.
	/// @author crotwell
	/// 
	/// Created on Mar 10, 2011
	/// </summary>
	public class BtimeRange
	{

		internal Btime begin;
		internal Btime end;

		public BtimeRange(Btime begin, Btime end) : base()
		{
			this.begin = begin;
			this.end = end;
		}

		public virtual Btime Begin
		{
			get
			{
				return begin;
			}
		}

		public virtual Btime End
		{
			get
			{
				return end;
			}
		}

		public virtual bool Overlaps(Btime btime)
		{
			return btime.AfterOrEquals(begin) && end.AfterOrEquals(btime);
		}

		public virtual bool Overlaps(BtimeRange range)
		{
			return !(range.Begin.After(End) || Begin.After(range.End));
		}
	}

}
