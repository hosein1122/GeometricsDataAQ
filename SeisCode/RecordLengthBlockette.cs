using System;
using System.Collections.Generic;
using System.Text;

namespace SeisCode
{
	public interface RecordLengthBlockette
	{

		int LogicalRecordLengthByte { get; }

		int LogicalRecordLength { get; }
	}

}
