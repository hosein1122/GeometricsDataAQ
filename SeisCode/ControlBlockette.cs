using System;
using System.Collections.Generic;
using System.Text;

namespace SeisCode
{
	public abstract class ControlBlockette : Blockette
	{

		public ControlBlockette(sbyte[] info)
		{
			this.info = info;
		}

		internal sbyte[] info;

		public override int Size
		{
			get
			{
				sbyte[] lengthBytes = new sbyte[4];
				Array.Copy(info, 3, lengthBytes, 0, 4);
				return int.Parse(StringHelper.NewString(lengthBytes));
			}
		}

		public override sbyte[] toBytes()
		{
			return info;
		}
	}
}


