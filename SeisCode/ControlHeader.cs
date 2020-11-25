using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SeisCode
{
    public class ControlHeader
    {

        protected internal int sequenceNum;

        protected internal byte typeCode;

        protected internal bool continuationCode;

        public static ControlHeader Read(BinaryReader @in)
        {
            byte[] seqBytes = @in.ReadBytes(6);
            if (seqBytes.Length == 0)
                throw new SeedFormatException("End of file, it is done?!!");
            string seqNumString = Encoding.ASCII.GetString(seqBytes);

            int sequenceNum = 0;
            // check for blank string, leave as zero if so
            if (!seqNumString.Equals("      "))
            {
                try
                {
                    sequenceNum = Convert.ToInt32(seqNumString);
                }
                catch (FormatException e)
                {
                    Console.Error.WriteLine("seq num unreadable, setting to 0 " + e.ToString());
                } // end of try-catch
            }
            byte typeCode = @in.ReadByte();

            int b = @in.ReadByte();

            bool continuationCode;
            if (b == 32)
            {
                // a space, so no continuation
                continuationCode = false;
            }
            else if (b == 42)
            {
                // an asterisk, so is a continuation
                continuationCode = true;
            }
            else
            {
                throw new SeedFormatException("ControlHeader, expected space or *, but got " + b);
            }

            if (typeCode == (byte)'D' || typeCode == (byte)'R' || typeCode == (byte)'Q' || typeCode == (byte)'M')
            {
                // Data Header is D, R, Q or M
                return DataHeader.read(@in, sequenceNum, (char)typeCode, continuationCode);
            }
            else
            {
                // Control header is V, A, S, or T
                return new ControlHeader(sequenceNum, typeCode, continuationCode);
            }
        }

        /// <summary>
        /// This method writes Control Header into the output stream
        /// While writing, it will conform to the format of MiniSeed
        /// </summary>

        protected internal virtual void Write(BinaryWriter dos)
        {
            string sequenceNumstring = SequenceNum.ToString("000000");

            byte[] sequenceNumByteArray = null;
            try
            {
                sequenceNumByteArray = Encoding.ASCII.GetBytes(sequenceNumstring);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }

            byte continuationCodeByte;
            if (continuationCode == true)
            {
                //if it is continuation,it is represented as asterix '*'
                continuationCodeByte = 42;
            }
            else
            {
                //if it continuationCode is false...it is represented as space ' '
                continuationCodeByte = 32;
            }
            try
            {
                dos.Write(sequenceNumByteArray);
                dos.Write((byte)typeCode);
                dos.Write(continuationCodeByte);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }

        }


        /// <summary>
        /// Writes an ASCII version of the record header. This is not meant to be a definitive ascii representation,
        /// merely to give something to print for debugging purposes. Ideally each field of the header should
        /// be printed in the order is appears in the header in a visually appealing way.
        /// </summary>
        /// <param name="out">
        ///            a Writer
        ///  </param>
        public virtual void WriteASCII(TextWriter @out)
        {
            WriteASCII(@out, "");
        }

        public virtual void WriteASCII(TextWriter @out, string indent)
        {
            @out.Write(indent + "seq=" + SequenceNum);
            @out.Write(" type=" + TypeCode);
            @out.WriteLine(" cont=" + Continuation);
        }

        public ControlHeader(int sequenceNum, byte typeCode, bool continuationCode)
        {
            this.sequenceNum = sequenceNum;
            this.typeCode = (byte)typeCode;
            this.continuationCode = continuationCode;
        }

        public ControlHeader(int sequenceNum, char typeCode, bool continuationCode) : this(sequenceNum, (byte)typeCode, continuationCode)
        {
        }

        public virtual short Size
        {
            get
            {
                return 8;
            }
        }

        public virtual int SequenceNum
        {
            get
            {
                return sequenceNum;
            }
        }

        public virtual char TypeCode
        {
            get
            {
                return (char)typeCode;
            }
        }

        public virtual bool Continuation
        {
            get
            {
                return continuationCode;
            }
        }

        public override string ToString()
        {
            return TypeCode + "  " + SequenceNum;
        }
        public static void Tester(string fileName)
        {
            try
            {
                BinaryWriter dos = new BinaryWriter(new FileStream(fileName, FileMode.Create, FileAccess.Write));
                ControlHeader controlHeaderObject = new ControlHeader(23, (byte)'D', true);
                controlHeaderObject.Write(dos);
                dos.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
        }
        public static void Main(string[] args)
        {
            Tester(args[0]);

        }
    }

}


