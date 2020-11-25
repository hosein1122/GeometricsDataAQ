using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodecLib
{
    public class UnsupportedCompressionType : CodecException
    {
        public UnsupportedCompressionType()
        {

        }

        public UnsupportedCompressionType(string reason):base(reason)
        {
           // super(reason);
        }
    }
}
