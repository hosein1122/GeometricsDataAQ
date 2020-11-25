using System;
using System.Collections.Generic;
using System.Text;

namespace SeisCode
{
    public class DataRecordBeginComparator
    {

        public int compare(DataRecord o1, DataRecord o2)
        {
            Btime b1 = o1.Header.StartBtime;
            Btime b2 = o2.Header.StartBtime;
            if (b1.DateTime < b2.DateTime)
                return -1;
            if (b1.DateTime > b2.DateTime)
                return 1;
            else
                return 0;
        }

    }
}
