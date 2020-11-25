using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodecLib
{
    
    public class B1000Types {

    /** ascii */
    public  const  int ASCII = 0;

    /** 16 bit integer, or java short */
    public  const int SHORT = 1;

    /** 24 bit integer */
    public  const int INT24 = 2;

    /** 32 bit integer, or java int */
    public  const int INTEGER = 3;

    /** ieee float */
    public  const int FLOAT = 4;

    /** ieee double*/
    public  const int DOUBLE = 5;

    /** Steim1 compression */
    public  const int STEIM1 = 10;

    /** Steim2 compression */
    public  const int STEIM2 = 11;
    
    /** CDSN 16 bit gain ranged */
    public  const int CDSN = 16;
        
    /** (A)SRO */
    public  const int SRO = 30;
    
    /** DWWSSN 16 bit */
    public  const int DWWSSN = 32;
}// B1000Types
}
