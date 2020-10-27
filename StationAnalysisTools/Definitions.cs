using System;
using System.Collections.Generic;
using System.Text;

namespace StationAnalysisToolsNetCore
{
 public class Definitions
    {
		public const double PI = 3.14159265358979311600;
		public const int NPTS_SAC_FILE_MAX = 17280000;
		public const int N_MAX_ZEROES = 25;
		public const int N_MAX_POLES = 25;
		public const double ZERO_MACHINE_VALUE = 0.00000001;
		public class SPECTRUM
		{
			public int N;
			public double[] re;
			public double[] im;
		}


		public class COMPLEX_RP
		{
			public double real;
			public double imag;
		}


		public class POLE_ZERO
		{
			public int n_zeroes;
			public int n_poles;
			public double scale;
			public COMPLEX_RP[] poles = Arrays.InitializeWithDefaultInstances<COMPLEX_RP>(N_MAX_POLES);
			public COMPLEX_RP[] zeroes = Arrays.InitializeWithDefaultInstances<COMPLEX_RP>(N_MAX_ZEROES);
		}

		internal static class DefineConstants
		{
			
		}

		//Helper class added by C++ to C# Converter:

		//----------------------------------------------------------------------------------------
		//	Copyright © 2006 - 2020 Tangible Software Solutions, Inc.
		//	This class can be used by anyone provided that the copyright notice remains intact.
		//
		//	This class provides the ability to initialize and delete array elements.
		//----------------------------------------------------------------------------------------
		internal static class Arrays
		{
			public static T[] InitializeWithDefaultInstances<T>(int length) where T : new()
			{
				T[] array = new T[length];
				for (int i = 0; i < length; i++)
				{
					array[i] = new T();
				}
				return array;
			}

			public static void DeleteArray<T>(T[] array) where T : System.IDisposable
			{
				foreach (T element in array)
				{
					if (element != null)
						element.Dispose();
				}
			}
		}

	}
}
