using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SeisCode
{
    [Serializable]
    public abstract class DataBlockette : Blockette
    {

        public DataBlockette(byte[] info, bool swapBytes)
        {
            this.info = info;
            this.swapBytes = swapBytes;
        }

        public DataBlockette(int size)
        {
            this.info = new byte[size];
            Array.Copy(Utility.intToByteArray(Type), 2, info, 0, 2);
        }

        /// <summary>
        /// For use by subclasses that want to ensure that they are of a given size. </summary>
        /// <exception cref="SeedFormatException"> if the size is larger than the number of bytes </exception>
        protected internal virtual void CheckMinimumSize(int size)
        {
            if (info.Length < size)
            {
                throw new SeedFormatException("Blockette " + Type + " must have at least " + size + " bytes, but got " + info.Length);
            }
        }

        /// <summary>
        /// For use by subclasses that want to ensure that they are of a given size. </summary>
        /// <exception cref="SeedFormatException"> if the size is larger than the number of bytes </exception>
        protected internal virtual void TrimToSize(int size)
        {
            CheckMinimumSize(size);
            if (info.Length > size)
            {
                // must be extra junk at end, trim
                byte[] tmp = new byte[size];
                Array.Copy(info, 0, tmp, 0, size);
                info = tmp;
            }
        }

        public virtual void Write(BinaryWriter dos, short nextOffset)
        {
            dos.Write(ToBytes(nextOffset));
        }

        public virtual byte[] ToBytes(short nextOffset)
        {
            Array.Copy(Utility.intToByteArray(nextOffset), 2, info, 2, 2);
            return info;
        }

        public override byte[] ToBytes()
        {
            return ToBytes(0);
        }

        protected internal byte[] info;

        protected internal bool swapBytes;

    } // DataBlockette
}
