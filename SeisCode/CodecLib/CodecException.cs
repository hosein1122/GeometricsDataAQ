//using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;



namespace CodecLib
{
    public class CodecException : Exception
    {
        public CodecException() { }
        public CodecException(string reason) : base(reason) { }

    }// Codec
}
