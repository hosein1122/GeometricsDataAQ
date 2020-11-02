using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Text.RegularExpressions;

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
            string @out = ZEROS + " " + zeros.Length + "\n";
            for (int i = 0; i < zeros.Length; i++)
            {
                // @out += formatter.format(zeros[i].Real) + " " + formatter.format(zeros[i].Imaginary) + "\n";
                @out += (zeros[i].Real).ToString(" 0.0000;-0.0000") + " " + (zeros[i].Imaginary).ToString(" 0.0000; -0.0000") + "\n";
            }
            @out += POLES + " " + poles.Length + "\n";
            for (int i = 0; i < poles.Length; i++)
            {
                @out += (poles[i].Real).ToString(" 0.0000;-0.0000") + " " + (poles[i].Imaginary).ToString(" 0.0000;-0.0000") + "\n";
            }
            @out += CONSTANT + " " + (constant).ToString("0.0#######E00") + "\n";
            return @out;
        }

        protected internal virtual void read(StreamReader @in)
        {
            List<string> lines = new List<string>();
            string s;
            var reg = new Regex("^-?\\d+\\.\\d+\\s+-?\\d+\\.\\d+");
            while ((s = @in.ReadLine()) is object)
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
                    for (int i = 0; i < poles.Length && it.MoveNext(); i++)
                    {
                        if (reg.IsMatch(line))
                        {
                            poles[i] = parseCmplx(line);
                            line = it.Current;
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
                    for (int i = 0; i < zeros.Length && it.MoveNext(); i++)
                    {
                        if (reg.IsMatch(line))
                        {
                            zeros[i] = parseCmplx(line);
                            line = it.Current;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else if (line.StartsWith(CONSTANT, StringComparison.Ordinal))
                {
                    line = line.Replace("\\s+", " ");
                    string[] sline = line.Split(' ');
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
            if (it.MoveNext())
            {
                return (string)it.Current;
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
            line = line.Trim().Replace("\\s+", " ");
            string[] sline = line.Split(' ');
            var d1 = Convert.ToDouble(sline[0], System.Globalization.CultureInfo.GetCultureInfo("en-US"));
            var d2 = Convert.ToDouble(sline[2], System.Globalization.CultureInfo.GetCultureInfo("en-US"));
            return new Complex(d1, d2);
        }



        public virtual bool Close(object obj)
        {
            if (base.Equals(obj))
            {
                return true;
            }
            if (obj is SacPoleZero)
            {
                SacPoleZero spz = (SacPoleZero)obj;
                if (!Close(spz.constant, constant))
                {
                    Console.WriteLine("const not close");
                    return false;
                }
                else
                {
                    return CloseButConstant(obj);
                }
            }
            else
            {
                return false;
            }
        }

        public virtual bool CloseButConstant(object obj)
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
                        if (!CloseFourDigit(spz.poles[i], poles[i]))
                        {
                            Console.WriteLine("pole " + i + " not equal" + spz.poles[i].Imaginary + " " + poles[i].Imaginary + " " + spz.poles[i].Real + " " + poles[i].Real);
                            return false;
                        }
                    }
                    for (int i = 0; i < zeros.Length; i++)
                    {
                        if (!CloseFourDigit(spz.zeros[i], zeros[i]))
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

        private static bool Close(double a, double b)
        {
            if (Math.Abs(a - b) / a > 0.0001)
            {
                Console.WriteLine("fail close " + a + " " + b + " " + (Math.Abs(a - b) / a) + " ratio=" + (a / b));
                return false;
            }
            return true;
        }

        private static bool CloseFourDigit(Complex a, Complex b)
        {
            return CloseFourDigit(a.Real, b.Real) && CloseFourDigit(a.Imaginary, b.Imaginary);
        }

        private static bool CloseFourDigit(double a, double b)
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
