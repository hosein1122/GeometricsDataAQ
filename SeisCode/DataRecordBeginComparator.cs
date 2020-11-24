using System;
using System.Collections.Generic;
using System.Text;

namespace SeisCode
{
	public class DataRecordBeginComparator : IComparer<DataRecord>
	{

		public virtual int Compare(DataRecord o1, DataRecord o2)
		{
			Btime b1 = o1.Header.StartBtime;
			Btime b2 = o2.Header.StartBtime;
			return btimeComparator.compare(b1, b2);
		}

		internal BtimeComparator btimeComparator = new BtimeComparator();
	}

}
