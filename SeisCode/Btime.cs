using System;
using System.Collections.Generic;
using System.Text;

namespace SeisCode
{
	public class Btime
	{

		public static readonly TimeZone UTC = TimeZone.getTimeZone("UTC");

		public Btime(DateTime date)
		{
			DateTime cal = DateTime.getInstance(UTC);
			cal = new DateTime(date);
			FieldsFromCalendar = cal;
		}

		private DateTime FieldsFromCalendar
		{
			set
			{
				if (!value.TimeZone.Equals(UTC))
				{
					throw new System.ArgumentException("Calendar time zone is not UTC: " + value.TimeZone);
				}
				tenthMilli = (int)(value.Millisecond * 10);
				year = value.Year;
				jday = value.DayOfYear;
				hour = value.Hour;
				min = value.Minute;
				sec = value.Second;
			}
		}

		public Btime()
		{
		}

		public Btime(int year, int jday, int hour, int min, int sec, int tenthMilli)
		{
			this.year = year;
			this.jday = jday;
			this.hour = hour;
			this.min = min;
			this.sec = sec;
			this.tenthMilli = tenthMilli;
		}

		public Btime(sbyte[] bytes) : this(bytes, 0)
		{
		}

		public Btime(sbyte[] bytes, int offset)
		{
			bool byteSwapFlag = shouldSwapBytes(bytes, offset);
			year = Utility.uBytesToInt(bytes[offset], bytes[offset + 1], byteSwapFlag);
			jday = Utility.uBytesToInt(bytes[offset + 2], bytes[offset + 3], byteSwapFlag);
			hour = bytes[offset + 4] & 0xff;
			min = bytes[offset + 5] & 0xff;
			sec = bytes[offset + 6] & 0xff;
			// bytes[7] is unused (alignment)
			tenthMilli = Utility.uBytesToInt(bytes[offset + 8], bytes[offset + 9], byteSwapFlag);
		}

		/// <summary>
		/// Create with seconds since epoch (1970) </summary>
		public Btime(double d)
		{
			long millis = (long)Math.Round(Math.Floor(d * 1000), MidpointRounding.AwayFromZero); // milliseconds
			DateTime cal = DateTime.getInstance(UTC);
			cal.TimeInMillis = millis;
			FieldsFromCalendar = cal;
			tenthMilli = (int)((long)Math.Round(d * 10000, MidpointRounding.AwayFromZero) % 10000);
		}

		public override int GetHashCode()
		{
			const int PRIME = 31;
			int result = 1;
			result = PRIME * result + hour;
			result = PRIME * result + jday;
			result = PRIME * result + min;
			result = PRIME * result + sec;
			result = PRIME * result + tenthMilli;
			result = PRIME * result + year;
			return result;
		}

		public override bool Equals(object o)
		{
			if (o == this)
			{
				return true;
			}
			if (o is Btime)
			{
				Btime oBtime = (Btime)o;
				return oBtime.year == year && oBtime.jday == jday && oBtime.hour == hour && oBtime.min == min && oBtime.sec == sec && oBtime.tenthMilli == tenthMilli;
			}
			return false;
		}

		public virtual bool before(Btime other)
		{
			return comparator.compare(this, other) == -1;
		}

		public virtual bool after(Btime other)
		{
			return comparator.compare(this, other) == 1;
		}

		public virtual bool afterOrEquals(Btime other)
		{
			return comparator.compare(this, other) >= 0;
		}

		public virtual DateTime convertToCalendar()
		{
			DateTime cal = GregorianCalendar.getInstance(UTC);
			cal.set(DateTime.MILLISECOND, TenthMilli / 10); // loose precision here
			cal.set(DateTime.SECOND, Sec);
			cal.set(DateTime.MINUTE, Min);
			cal.set(DateTime.HOUR_OF_DAY, Hour);
			cal.set(DateTime.DAY_OF_YEAR, DayOfYear);
			cal.set(DateTime.YEAR, Year);
			return cal;
		}

		public override string ToString()
		{
			return "BTime(" + year + ":" + jday + ":" + hour + ":" + min + ":" + sec + "." + tenthMilli + ")";
		}

		public int year = 1960;

		public int jday = 1;

		public int hour = 0;

		public int min = 0;

		public int sec = 0;

		public int tenthMilli = 0;


		public virtual int Year
		{
			get
			{
				return year;
			}
		}

		public virtual int DayOfYear
		{
			get
			{
				return JDay;
			}
		}

		public virtual int JDay
		{
			get
			{
				return jday;
			}
		}


		public virtual int Hour
		{
			get
			{
				return hour;
			}
		}

		public virtual int Min
		{
			get
			{
				return min;
			}
		}


		public virtual int Sec
		{
			get
			{
				return sec;
			}
		}


		public virtual int TenthMilli
		{
			get
			{
				return tenthMilli;
			}
		}

		/// <summary>
		/// Expects btime to be a byte array pointing at the beginning of a btime
		/// segment
		/// </summary>
		/// <returns> - true if the bytes need to be swapped to get a valid year </returns>
		public static bool shouldSwapBytes(sbyte[] btime)
		{
			return shouldSwapBytes(btime, 0);
		}

		/// <summary>
		/// Expects btime to be a byte array pointing at the beginning of a btime
		/// segment.
		/// 
		/// Time capsule: note that year 2056 as a short byte swaps to itself, so whomever
		/// is maintaining this code off in the distant future, 49 years from now as
		/// I write this in 2007, should find some other header to use for byte swap checking!
		/// 
		/// Using the jday or tenthmilli doesn't help much as 1 byte swaps to 256, 256 to 1 and 257 to itself.
		/// 
		/// If mseed was going to support little endian headers they should have put in a damn flag!
		///  - HPC
		/// </summary>
		/// <returns> - true if the bytes need to be swapped to get a valid year </returns>
		public static bool shouldSwapBytes(sbyte[] btime, int offset)
		{
			int year = Utility.uBytesToInt(btime[0 + offset], btime[1 + offset], false);
			return year < 1960 || year > 2055;
		}

		private static BtimeComparator comparator = new BtimeComparator();

		public virtual sbyte[] AsBytes
		{
			get
			{
				sbyte[] bytes = new sbyte[10];
				Array.Copy(Utility.intToByteArray(year), 2, bytes, 0, 2);
				Array.Copy(Utility.intToByteArray(jday), 2, bytes, 2, 2);
				Array.Copy(Utility.intToByteArray(hour), 3, bytes, 4, 1);
				Array.Copy(Utility.intToByteArray(min), 3, bytes, 5, 1);
				Array.Copy(Utility.intToByteArray(sec), 3, bytes, 6, 1);
				Array.Copy(Utility.intToByteArray(tenthMilli), 2, bytes, 8, 2);
				return bytes;
			}
		}


	}
