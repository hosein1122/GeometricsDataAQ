using System;
using System.Collections.Generic;
using System.Text;

namespace SeisCode
{
	public abstract class ControlBlockette : Blockette
	{

		public ControlBlockette(byte[] info)
		{
			this.info = info;
		}

		internal byte[] info;

		public override int Size
		{
			get
			{
				byte[] lengthBytes = new byte[4];
				Array.Copy(info, 3, lengthBytes, 0, 4);
				return int.Parse(Encoding.ASCII.GetString(lengthBytes));
			}
		}

		public override byte[] ToBytes()
		{
			return info;
		}
	}
}


