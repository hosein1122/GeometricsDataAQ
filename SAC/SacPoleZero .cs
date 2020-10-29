using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace SAC
{
    public class SacPoleZero
    {
        public SacPoleZero(StreamReader @in)
        {
            read(@in);
        }

        public SacPoleZero(string filename)
        {
            StreamReader @in = new StreamReader(filename);
            read(@in);
            @in.Close();
        }

        public SacPoleZero(Complex[] poles, Complex[] zeros, float constant)
        {
            this.poles = poles;
            this.zeros = zeros;
            this.constant = constant;
        }

        public virtual float Constant
        {
            get
            {
                return constant;
            }
        }

        public virtual Complex[] Poles
        {
            get
            {
                return poles;
            }
        }

        public virtual Complex[] Zeros
        {
            get
            {
                return zeros;
            }
        }

        public override string ToString()
        {
            DecimalFormat formatter = new DecimalFormat(" 0.0000;-0.0000", new DecimalFormatSymbols(Locale.US));
            DecimalFormat constantFormatter = new DecimalFormat("0.0#######E00", new DecimalFormatSymbols(Locale.US));
            string @out = ZEROS + " " + zeros.Length + "\n";
            for (int i = 0; i < zeros.Length; i++)
            {
                @out += formatter.format(zeros[i].Real) + " " + formatter.format(zeros[i].Imaginary) + "\n";
            }
            @out += POLES + " " + poles.Length + "\n";
            for (int i = 0; i < poles.Length; i++)
            {
                @out += formatter.format(poles[i].Real) + " " + formatter.format(poles[i].Imaginary) + "\n";
            }
            @out += CONSTANT + " " + constantFormatter.format(constant) + "\n";
            return @out;
        }

        protected internal virtual void read(StreamReader @in)
        {
            List<string> lines = new List<string>();
            string s;
            while (!string.ReferenceEquals((s = @in.ReadLine()), null))
            {
                lines.Add(s.Trim());
            }
            Complex[] poles = new Complex[0];
            Complex[] zeros = new Complex[0];
            float constant = 1;
            IEnumerator<string> it = lines.GetEnumerator();
            string line = nextLine(it);
            while (!line.Equals(""))
            {
                if (line.StartsWith(POLES, StringComparison.Ordinal))
                {
                    string num = line.Substring(POLES.Length).Trim();
                    int numPoles = int.Parse(num);
                    poles = initCmplx(numPoles);
                    line = nextLine(it);
                    //JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
                    for (int i = 0; i < poles.Length && it.hasNext(); i++)
                    {
                        if (line.matches("^-?\\d+\\.\\d+\\s+-?\\d+\\.\\d+"))
                        {
                            poles[i] = parseCmplx(line);
                            line = nextLine(it);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else if (line.StartsWith(ZEROS, StringComparison.Ordinal))
                {
                    string num = line.Substring(ZEROS.Length).Trim();
                    int numZeros = int.Parse(num);
                    zeros = initCmplx(numZeros);
                    line = nextLine(it);
                    //JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
                    for (int i = 0; i < zeros.Length && it.hasNext(); i++)
                    {
                        if (line.matches("^-?\\d+\\.\\d+\\s+-?\\d+\\.\\d+"))
                        {
                            zeros[i] = parseCmplx(line);
                            line = nextLine(it);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else if (line.StartsWith(CONSTANT, StringComparison.Ordinal))
                {
                    line = line.replaceAll("\\s+", " ");
                    string[] sline = line.Split(" ", true);
                    constant = float.Parse(sline[1]);
                    line = nextLine(it);
                }
                else
                {
                    throw new IOException("Unknown line in SAC polezero file: " + line);
                }
            }
            this.poles = poles;
            this.zeros = zeros;
            this.constant = constant;
        }

        private static string nextLine(System.Collections.IEnumerator it)
        {
            //JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
            if (it.hasNext())
            {
                //JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
                return (string)it.next();
            }
            else
            {
                return "";
            }
        }

        public static Complex[] initCmplx(int length)
        {
            Complex[] @out = new Complex[length];
            for (int i = 0; i < @out.Length; i++)
            {
                @out[i] = new Complex(0, 0);
            }
            return @out;
        }

        internal static Complex parseCmplx(string line)
        {
            line = line.Trim().replaceAll("\\s+", " ");
            string[] sline = line.Split(" ", true);
            return new Complex(float.Parse(sline[0]), float.Parse(sline[1]));
        }

        internal static Complex parseCmplx(string line)
        {
            line = line.Trim().replaceAll("\\s+", " ");
            string[] sline = line.Split(" ", true);
            return new Complex(float.Parse(sline[0]), float.Parse(sline[1]));
        }

        public virtual bool close(object obj)
        {
            if (base.Equals(obj))
            {
                return true;
            }
            if (obj is SacPoleZero)
            {
                SacPoleZero spz = (SacPoleZero)obj;
                if (!close(spz.constant, constant))
                {
                    Console.WriteLine("const not close");
                    return false;
                }
                else
                {
                    return closeButConstant(obj);
                }
            }
            else
            {
                return false;
            }
        }

        public virtual bool closeButConstant(object obj)
        {
            if (base.Equals(obj))
            {
                return true;
            }
            if (obj is SacPoleZero)
            {
                SacPoleZero spz = (SacPoleZero)obj;
                if (spz.poles.Length != poles.Length || spz.zeros.Length != zeros.Length)
                {
                    return false;
                }
                else
                {
                    for (int i = 0; i < poles.Length; i++)
                    {
                        if (!closeFourDigit(spz.poles[i], poles[i]))
                        {
                            Console.WriteLine("pole " + i + " not equal" + spz.poles[i].Imaginary + " " + poles[i].Imaginary + " " + spz.poles[i].Real + " " + poles[i].Real);
                            return false;
                        }
                    }
                    for (int i = 0; i < zeros.Length; i++)
                    {
                        if (!closeFourDigit(spz.zeros[i], zeros[i]))
                        {
                            Console.WriteLine("zero " + i + " not equal");
                            return false;
                        }
                    }
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        private static bool close(double a, double b)
        {
            if (Math.Abs(a - b) / a > 0.0001)
            {
                Console.WriteLine("fail close " + a + " " + b + " " + (Math.Abs(a - b) / a) + " ratio=" + (a / b));
                return false;
            }
            return true;
        }

        private static bool closeFourDigit(Complex a, Complex b)
        {
            return closeFourDigit(a.Real, b.Real) && closeFourDigit(a.Imaginary, b.Imaginary);
        }

        private static bool closeFourDigit(double a, double b)
        {
            if (Math.Abs(a - b) > 0.0001)
            {
                Console.WriteLine("fail closeFourDigit " + a + " " + b + " " + (Math.Abs(a - b)) + " ratio=" + (a / b));
                return false;
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                return true;
            }
            if (obj is SacPoleZero)
            {
                SacPoleZero spz = (SacPoleZero)obj;
                if ((Math.Abs(spz.constant - constant) / constant) > .001)
                {
                    return false;
                }
                else if (spz.poles.Length != poles.Length || spz.zeros.Length != zeros.Length)
                {
                    return false;
                }
                else
                {
                    for (int i = 0; i < poles.Length; i++)
                    {
                        if (spz.poles[i].Imaginary != poles[i].Imaginary || spz.poles[i].Real != poles[i].Real)
                        {
                            return false;
                        }
                    }
                    for (int i = 0; i < zeros.Length; i++)
                    {
                        if (spz.zeros[i].Imaginary != zeros[i].Imaginary || spz.zeros[i].Real != zeros[i].Real)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            int i = 17;
            i = 29 * i + poles.Length;
            i = 31 * i + zeros.Length;
            for (int j = 0; j < poles.Length; j++)
            {
                i = 37 * i + poles[j].GetHashCode();
            }
            for (int j = 0; j < zeros.Length; j++)
            {
                i = 43 * i + zeros[j].GetHashCode();
            }
            return i;
        }

        private Complex[] poles;

        private Complex[] zeros;

        private float constant;

        internal static string POLES = "POLES";

        internal static string ZEROS = "ZEROS";

        internal static string CONSTANT = "CONSTANT";

    }
}
