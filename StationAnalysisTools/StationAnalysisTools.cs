﻿using System;
using System.IO;
using StationAnalysisToolsNetCore;
using static StationAnalysisToolsNetCore.Definitions;
using FFTW.NET;
namespace StationAnalysisToolsNetCore
{
    public class StationAnalysisTools
    {

        //       //once we have the 9component cross and auto spectra computed, we can finish the sleeman noise
        //       private void finish_tri_noise( fftw_complex[] P11, fftw_complex[] P12, fftw_complex[] P13, fftw_complex[] P21, fftw_complex[] P22, fftw_complex[] P23, fftw_complex[] P31, fftw_complex[] P32, fftw_complex[] P33, int npts, float dt, fftw_complex[] N11, fftw_complex[] N22, fftw_complex[] N33)
        //       {

        //           //hij = Pik / Pjk
        //           //nii = Pii - Pji*hij
        //           //where nii is the noise for the ith sensor
        //           //Pik, Pjk are cross powers and Pii is auto-power

        //           int i;
        //           int nfreq;
        //           //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to value types:
        //           //ORIGINAL LINE: double *tmp;
        //           double tmp;
        //           nfreq = (int)Math.Floor(npts / 2 + 1.5);
        //           COMPLEX_RP tmp1 = new COMPLEX_RP();
        //           COMPLEX_RP tmp2 = new COMPLEX_RP();
        //           COMPLEX_RP tmp3 = new COMPLEX_RP();
        //           COMPLEX_RP tmp4 = new COMPLEX_RP();

        //           //C++ TO C# CONVERTER TODO TASK: The memory management function 'malloc' has no equivalent in C#:
        //           tmp = malloc(sizeof(double) * nfreq);

        //           for (i = 0; i < nfreq; i++)
        //           {
        //               //initialize output
        //               N11[i][0] = 0.0;
        //               N11[i][1] = 0.0;
        //               N22[i][0] = 0.0;
        //               N22[i][1] = 0.0;
        //               N33[i][0] = 0.0;
        //               N33[i][1] = 0.0;
        //               //start with N11: i=1, j=2, k=3
        //               tmp1.real = P13[i][0];
        //               tmp1.imag = P13[i][1];
        //               tmp2.real = P23[i][0];
        //               tmp2.imag = P23[i][1];
        //               tmp3 = complex_divide(tmp1, tmp2);
        //               tmp1.real = P11[i][0];
        //               tmp1.imag = 0.0;
        //               tmp2.real = P21[i][0];
        //               tmp2.imag = P21[i][1];
        //               tmp4 = complex_multiply(tmp2, tmp3);
        //               tmp3 = complex_subtract(tmp1, tmp4);
        //               if (10 * Math.Log10(complex_amplitude(tmp3) * 2 * dt / npts) > -999.9)
        //               {
        //                   N11[i][0] = 10 * Math.Log10(complex_amplitude(tmp3) * 2 * dt / npts);
        //               }
        //               else if (i != 0)
        //               {
        //                   N11[i][0] = N11[i - 1][0];
        //               }
        //               else
        //               {
        //                   N11[i][0] = -999.0;
        //               }


        //               //now N22: i=2, j=3, k=1
        //               tmp1.real = P21[i][0];
        //               tmp1.imag = P21[i][1];
        //               tmp2.real = P31[i][0];
        //               tmp2.imag = P31[i][1];
        //               tmp3 = complex_divide(tmp1, tmp2);
        //               tmp1.real = P22[i][0];
        //               tmp1.imag = 0.0;
        //               tmp2.real = P32[i][0];
        //               tmp2.imag = P32[i][1];
        //               tmp4 = complex_multiply(tmp2, tmp3);
        //               tmp3 = complex_subtract(tmp1, tmp4);
        //               if (10 * Math.Log10(complex_amplitude(tmp3) * 2 * dt / npts) > -999.9)
        //               {
        //                   N22[i][0] = 10 * Math.Log10(complex_amplitude(tmp3) * 2 * dt / npts);
        //               }
        //               else if (i != 0)
        //               {
        //                   N22[i][0] = N22[i - 1][0];
        //               }
        //               else
        //               {
        //                   N22[i][0] = -999.0;
        //               }

        //               //finish with N33: i=3, j=1, k=2
        //               //hij = Pik / Pjk
        //               //nii = Pii - Pji*hij
        //               tmp1.real = P32[i][0];
        //               tmp1.imag = P32[i][1];
        //               tmp2.real = P12[i][0];
        //               tmp2.imag = P12[i][1];
        //               tmp3 = complex_divide(tmp1, tmp2);
        //               tmp1.real = P33[i][0];
        //               tmp1.imag = 0.0;
        //               tmp2.real = P13[i][0];
        //               tmp2.imag = P13[i][1];
        //               tmp4 = complex_multiply(tmp2, tmp3);
        //               tmp3 = complex_subtract(tmp1, tmp4);
        //               if (10 * Math.Log10(complex_amplitude(tmp3) * 2 * dt / npts) > -999.9)
        //               {
        //                   N33[i][0] = 10 * Math.Log10(complex_amplitude(tmp3) * 2 * dt / npts);
        //               }
        //               else if (i != 0)
        //               {
        //                   N33[i][0] = N33[i - 1][0];
        //               }
        //               else
        //               {
        //                   N33[i][0] = -999.0;
        //               }
        //           }



        //           return;
        //       }


        //       //using the relation of Sleeman, 2006, we can compute a relative transfer function to look at noise in 3 co-located sensors
        //       private void compute_tri_noise(fftw_complex[] spect1, fftw_complex[] spect2, fftw_complex[] spect3, int npts, int j, fftw_complex[] P11, fftw_complex[] P12, fftw_complex[] P13, fftw_complex[] P21, fftw_complex[] P22, fftw_complex[] P23, fftw_complex[] P31, fftw_complex[] P32, fftw_complex[] P33)
        //       {

        //           //hij = Pik / Pjk
        //           //nii = Pii - Pji*hij
        //           //where nii is the noise for the ith sensor
        //           //Pik, Pjk are cross powers and Pii is auto-power
        //           //in here, we compute the average and standard deviation of all three auto-powers and cross-powers
        //           //after we have the mean and standard deviation, we finish the analysis after the loop by computing the transfer functions and applying the nii relation three times
        //           int i;
        //           int nfreq;
        //           double tmpR;
        //           double tmpI;
        //           nfreq = (int)Math.Floor(npts / 2 + 1.5);
        //           for (i = 0; i < nfreq; i++)
        //           {
        //               //easiest to start with the straight powers: P11, P22, P33
        //               tmpR = spect1[i][0] * spect1[i][0] + spect1[i][1] * spect1[i][1];
        //               P11[i][0] = recursive_mean(tmpR, P11[i][0], j);
        //               tmpR = spect2[i][0] * spect2[i][0] + spect2[i][1] * spect2[i][1];
        //               P22[i][0] = recursive_mean(tmpR, P22[i][0], j);
        //               tmpR = spect3[i][0] * spect3[i][0] + spect3[i][1] * spect3[i][1];
        //               P33[i][0] = recursive_mean(tmpR, P33[i][0], j);

        //               //ok, now the cross powers
        //               //1-2
        //               tmpR = spect1[i][0] * spect2[i][0] + spect1[i][1] * spect2[i][1];
        //               tmpI = spect1[i][1] * spect2[i][0] - spect2[i][1] * spect1[i][0];

        //               //means of 1-2
        //               P12[i][0] = recursive_mean(tmpR, P12[i][0], j);
        //               P12[i][1] = recursive_mean(tmpI, P12[i][1], j);

        //               //1-3
        //               tmpR = spect1[i][0] * spect3[i][0] + spect1[i][1] * spect3[i][1];
        //               tmpI = spect1[i][1] * spect3[i][0] - spect3[i][1] * spect1[i][0];

        //               //means of 1-3
        //               P13[i][0] = recursive_mean(tmpR, P13[i][0], j);
        //               P13[i][1] = recursive_mean(tmpI, P13[i][1], j);

        //               //2-1
        //               tmpR = spect2[i][0] * spect1[i][0] + spect2[i][1] * spect1[i][1];
        //               tmpI = spect2[i][1] * spect1[i][0] - spect1[i][1] * spect2[i][0];

        //               //means of 2-1
        //               P21[i][0] = recursive_mean(tmpR, P21[i][0], j);
        //               P21[i][1] = recursive_mean(tmpI, P21[i][1], j);

        //               //2-3
        //               tmpR = spect2[i][0] * spect3[i][0] + spect2[i][1] * spect3[i][1];
        //               tmpI = spect2[i][1] * spect3[i][0] - spect3[i][1] * spect2[i][0];

        //               //means of 2-3
        //               P23[i][0] = recursive_mean(tmpR, P23[i][0], j);
        //               P23[i][1] = recursive_mean(tmpI, P23[i][1], j);

        //               //3-1
        //               tmpR = spect3[i][0] * spect1[i][0] + spect3[i][1] * spect1[i][1];
        //               tmpI = spect3[i][1] * spect1[i][0] - spect1[i][1] * spect3[i][0];

        //               //means of 3-1
        //               P31[i][0] = recursive_mean(tmpR, P31[i][0], j);
        //               P31[i][1] = recursive_mean(tmpI, P31[i][1], j);

        //               //3-2
        //               tmpR = spect3[i][0] * spect2[i][0] + spect3[i][1] * spect2[i][1];
        //               tmpI = spect3[i][1] * spect2[i][0] - spect2[i][1] * spect3[i][0];

        //               //means of 2-3
        //               P32[i][0] = recursive_mean(tmpR, P32[i][0], j);
        //               P32[i][1] = recursive_mean(tmpI, P32[i][1], j);
        //           }

        //           return;
        //       }



        //       //does two psds for debugging output of coherence based measurements
        //       private void finish_two_psds(fftw_complex[] amps, int npts, float dt, double[] out1, double[] out2)
        //       {
        //           int i;
        //           int nfreq;
        //           nfreq = (int)Math.Floor(npts / 2 + 1.5);

        //           for (i = 0; i < nfreq; i++)
        //           {
        //               out1[i] = 10.0 * Math.Log10(Math.Sqrt(amps[i][0]) * 2 * dt / npts);
        //               out2[i] = 10.0 * Math.Log10(Math.Sqrt(amps[i][1]) * 2 * dt / npts);
        //           }
        //           running_mean_psd(out1, out1, nfreq);
        //           running_mean_psd(out2, out2, nfreq);

        //           return;
        //       }


        //       //similar idea as finish_coherence - using a coherence and substeps to get the incoherent component of the spectrum
        //       private void finish_incoherence(fftw_complex[] numerMean, fftw_complex[] numerSD, fftw_complex[] denom, int npts, float dt, double[] meanOut1, double[] sdOut1, double[] meanOut2, double[] sdOut2)
        //       {
        //           int i;
        //           int nfreq;
        //           double coh;
        //           nfreq = (int)Math.Floor(npts / 2 + 1.5);

        //           for (i = 0; i < nfreq; i++)
        //           {
        //               coh = (numerMean[i][0] * numerMean[i][0] + numerMean[i][1] * numerMean[i][1]) / (denom[i][0] * denom[i][1]);
        //               if (coh != 1.0)
        //               {
        //                   meanOut1[i] = 10 * Math.Log10((1.0 - coh) * (Math.Sqrt(denom[i][0]) * 2 * dt / npts));
        //                   meanOut2[i] = 10 * Math.Log10((1.0 - coh) * (Math.Sqrt(denom[i][1]) * 2 * dt / npts));
        //               }
        //               else
        //               {
        //                   meanOut1[i] = -999.0;
        //                   meanOut2[i] = -999.0;
        //               }
        //               sdOut1[i] = (numerSD[i][0] * numerSD[i][0] + numerSD[i][1] * numerSD[i][1]) / (denom[i][0] * denom[i][1]) * meanOut1[i];
        //               sdOut2[i] = (numerSD[i][0] * numerSD[i][0] + numerSD[i][1] * numerSD[i][1]) / (denom[i][0] * denom[i][1]) * meanOut2[i];
        //           }

        //           running_mean_psd(meanOut1, meanOut1, nfreq);
        //           running_mean_psd(sdOut1, sdOut1, nfreq);

        //           running_mean_psd(meanOut2, meanOut2, nfreq);
        //           running_mean_psd(sdOut2, sdOut2, nfreq);

        //           return;
        //       }

        //       //simply display the contents of a pole zero structure to standard out
        //       private void print_sac_pole_zero_file(POLE_ZERO pz)
        //       {
        //           int i;
        //           Console.Out.Write("Scalar Constant: {0:g}\n", pz.scale);
        //           Console.Out.Write("Poles:\n");
        //           for (i = 0; i < pz.n_poles; i++)
        //           {
        //               Console.Out.Write("{0:g} {1:g}\n", pz.poles[i].real, pz.poles[i].imag);
        //           }
        //           Console.Out.Write("Zeroes:\n");
        //           for (i = 0; i < pz.n_zeroes; i++)
        //           {
        //               Console.Out.Write("{0:g} {1:g}\n", pz.zeroes[i].real, pz.zeroes[i].imag);
        //           }
        //           return;
        //       }

        //       //compute the amplitude and divides by the total spectra
        //       private void finish_coherence(fftw_complex[] numerMean, fftw_complex[] numerSD, fftw_complex[] denom, int npts, double[] meanOut, double[] sdOut)
        //       {
        //           int i;
        //           int nfreq;
        //           nfreq = (int)Math.Floor(npts / 2 + 1.5);

        //           for (i = 0; i < nfreq; i++)
        //           {
        //               meanOut[i] = (numerMean[i][0] * numerMean[i][0] + numerMean[i][1] * numerMean[i][1]) / (denom[i][0] * denom[i][1]);
        //               sdOut[i] = (numerSD[i][0] * numerSD[i][0] + numerSD[i][1] * numerSD[i][1]) / (denom[i][0] * denom[i][1]);
        //           }

        //           running_mean_psd(meanOut, meanOut, nfreq);
        //           running_mean_psd(sdOut, sdOut, nfreq);

        //           return;
        //       }

        //       //before calling this the first time, zero out the outputs (numer, denom1, denom2) as this appends to them
        //       //how about instead, I recursively compute the mean and standard deviation in here
        //       private void compute_coherence(fftw_complex[] spect1, fftw_complex[] spect2, int npts, int j, fftw_complex[] numerMean, fftw_complex[] numerSD, fftw_complex[] denom)
        //       {
        //           int i;
        //           int nfreq;
        //           double tmpNumerR;
        //           double tmpNumerI;
        //           double tmpDenomX;
        //           double tmpDenomY;
        //           nfreq = (int)Math.Floor(npts / 2 + 1.5);
        //           for (i = 0; i < nfreq; i++)
        //           {
        //               //cross power
        //               tmpNumerR = spect1[i][0] * spect2[i][0] + spect1[i][1] * spect2[i][1];
        //               tmpNumerI = spect1[i][1] * spect2[i][0] - spect2[i][1] * spect1[i][0];

        //               //straight powers
        //               tmpDenomX = spect1[i][0] * spect1[i][0] + spect1[i][1] * spect1[i][1];
        //               tmpDenomY = spect2[i][0] * spect2[i][0] + spect2[i][1] * spect2[i][1];

        //               //means of numerator
        //               numerMean[i][0] = recursive_mean(tmpNumerR, numerMean[i][0], j);
        //               numerMean[i][1] = recursive_mean(tmpNumerI, numerMean[i][1], j);

        //               //standard deviation of numerator
        //               numerSD[i][0] = recursive_standard_deviation(tmpNumerR, numerMean[i][0], numerSD[i][0], j);
        //               numerSD[i][1] = recursive_standard_deviation(tmpNumerI, numerMean[i][1], numerSD[i][1], j);

        //               //mean denominator
        //               denom[i][0] = recursive_mean(tmpDenomX, denom[i][0], j);
        //               denom[i][1] = recursive_mean(tmpDenomY, denom[i][1], j);
        //           }

        //           return;
        //       }


        //       //simply display the contents of a pole zero structure to standard out
        //       private void print_sac_pole_zero_file(POLE_ZERO pz)
        //       {
        //           int i;
        //           Console.Out.Write("Scalar Constant: {0:g}\n", pz.scale);
        //           Console.Out.Write("Poles:\n");
        //           for (i = 0; i < pz.n_poles; i++)
        //           {
        //               Console.Out.Write("{0:g} {1:g}\n", pz.poles[i].real, pz.poles[i].imag);
        //           }
        //           Console.Out.Write("Zeroes:\n");
        //           for (i = 0; i < pz.n_zeroes; i++)
        //           {
        //               Console.Out.Write("{0:g} {1:g}\n", pz.zeroes[i].real, pz.zeroes[i].imag);
        //           }
        //           return;
        //       }



        //       //linear interpolation onto log10 space
        //       private void log10_linear_interpolate(double[] freqs, int nfreqs, double[] input, double[] freqsOut, double[] @out, ref int nFreqsOut, double minPer, double maxPer)
        //       {
        //           int i;
        //           int j;
        //           int k;
        //           int l;
        //           int nspline;
        //           int khi;
        //           int klo;
        //           int splineMin;
        //           int splineMax;
        //           double slope;
        //           double df;
        //           double prev;
        //           //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to value types:
        //           //ORIGINAL LINE: double *splineFreqs, splineFreq;
        //           double splineFreqs;
        //           double splineFreq;
        //           double minval;
        //           double maxval;
        //           double minFreqIn;
        //           double maxFreqIn;

        //           //initialize output
        //           //	for (i=0; i<nfreqs; i++) {
        //           //		out[i] = 0.0;
        //           //		freqsOut[i] = 0.0;
        //           //	}

        //           //compute nodes
        //           nspline = compute_log10_nodes(maxPer, minPer);
        //           nFreqsOut = nspline;

        //           splineMin = Math.Floor(Math.Log10(minPer) + 0.5);
        //           splineMax = Math.Floor(Math.Log10(maxPer) + 0.5);

        //           /*
        //            if (freqs[0] < freqs[1]) {
        //            minFreqIn = freqs[0];
        //            maxFreqIn = freqs[nfreqs-1];
        //            minval = input[0];
        //            maxval = input[nfreqs-1];
        //            } else {
        //            maxFreqIn = freqs[0];
        //            minFreqIn = freqs[nfreqs-1];
        //            maxval = input[0];
        //            minval = input[nfreqs-1];
        //            }
        //            */
        //           prev = input[0];

        //           //loop out
        //           k = 0;
        //           klo = 0;
        //           khi = 1;
        //           slope = (input[khi] - input[klo]) / (freqs[khi] - freqs[klo]);
        //           for (i = splineMin; i <= splineMax; i++)
        //           {
        //               for (j = 1; j < 10; j++)
        //               {
        //                   for (l = 0; l < 10; l++)
        //                   {
        //                       freqsOut[k] = (double)j * Math.Pow(10.0, (double)i) + (double)l * Math.Pow(10.0, (double)i - 1);
        //                       //				if (freqsOut[k] >= minPer && freqsOut[k] <= maxPer) {
        //                       if (freqsOut[k] >= freqs[0] && freqsOut[k] <= freqs[nfreqs - 1])
        //                       {
        //                           while (freqsOut[k] > freqs[khi])
        //                           {
        //                               khi++;
        //                               if (khi == nfreqs - 1)
        //                               {
        //                                   break;
        //                               }
        //                           }
        //                           klo = khi - 1;
        //                           while (freqsOut[k] < freqs[klo])
        //                           {
        //                               klo--;
        //                               if (klo == 0)
        //                               {
        //                                   break;
        //                               }
        //                           }
        //                           df = freqsOut[k] - freqs[klo];
        //                           slope = (input[khi] - input[klo]) / (freqs[khi] - freqs[klo]);
        //                           @out[k] = input[klo] + df * slope;
        //                           prev = @out[k];
        //                           //				} else if (freqsOut[k] < minPer) {
        //                           //					out[k] = prev;
        //                           //				} else {
        //                           //					out[k] = prev;
        //                           //				}
        //                           k++;
        //                       }
        //                   }
        //               }
        //           }
        //           return;
        //       }


        //       //simple node computer
        //       private int compute_nodes(float max, float min, float inc)
        //       {
        //           return (int)Math.Floor((max - min) / inc + 1.5);
        //       }
        //       //END

        //       //primary interpolation routine (consider adjusting to use an output frequency array which is given as an argument)
        //       private void linear_interpolate_psd(float[] freqs, int nfreqs, float[] input, float[] @out, float minPer, float maxPer, float incPer)
        //       {

        //           int i;
        //           int klo;
        //           int khi;
        //           int nfreq;
        //           float slope;
        //           float F;
        //           float incr;
        //           float df;

        //           nfreq = compute_nodes(maxPer, minPer, incPer);
        //           F = minPer;
        //           incr = incPer;
        //           klo = 0;
        //           khi = 1;
        //           slope = (input[khi] - input[klo]) / (freqs[khi] - freqs[klo]);
        //           for (i = 0; i < nfreq; i++)
        //           {
        //               while (F > freqs[khi])
        //               {
        //                   khi++;
        //                   if (khi == nfreqs - 1)
        //                   {
        //                       break;
        //                   }
        //               }
        //               klo = khi - 1;
        //               while (F < freqs[klo])
        //               {
        //                   klo--;
        //                   if (klo == 0)
        //                   {
        //                       break;
        //                   }
        //               }
        //               df = F - freqs[klo];
        //               slope = (input[khi] - input[klo]) / (freqs[khi] - freqs[klo]);
        //               @out[i] = input[klo] + df * slope;
        //               F = F + incr;
        //           }

        //           return;
        //       }
        //       //END


        //       //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to value types:
        //       //ORIGINAL LINE: int smooth_2d_array(double **dat, double **out, int n_x, int n_y, int n_smooth)
        //       private int smooth_2d_array(double[] dat, double[] @out, int n_x, int n_y, int n_smooth)
        //       {
        //           //schematic:
        //           /*--------------*/
        //           /*--------------*/
        //           /*------yxy-----*/
        //           /*------x1x-----*/
        //           /*------yxy-----*/
        //           /*--------------*/
        //           /*--------------*/
        //           //In the above, the 1 is the node being smoothed and its weight is 1. The x's are also included, but have a lower weight and
        //           //the y's have an lower weight than the x's. the '-' are other points in the model space. The example shows if n_smooth = 3. 
        //           //	returns 0 n_smooth is even, or less than 1

        //           /* check the smooth flag */
        //           if (n_smooth < 1)
        //           {
        //               Console.Error.Write("Error, n_smooth must be greater than 0. Input: {0:D}\n", n_smooth);
        //               return 0;
        //           }
        //           if (n_smooth % 2 == 0)
        //           {
        //               Console.Error.Write("Error, n_smooth must be odd. Input: {0:D}\n", n_smooth);
        //               return 0;
        //           }
        //           if (n_smooth == 1)
        //           {
        //               Console.Error.Write("Error, n_smooth must not be 1. Input: {0:D}\n", n_smooth);
        //               return 0;
        //           }

        //           /* local variable declaration */
        //           int x;
        //           int y;
        //           int x_start = 0;
        //           int x_end = 0;
        //           int y_start = 0;
        //           int y_end = 0;
        //           int xx;
        //           int yy;
        //           double count;
        //           double sum;
        //           double weight;


        //           /* loop over each point */
        //           for (x = 0; x < n_x; x++)
        //           {
        //               for (y = 0; y < n_y; y++)
        //               {
        //                   count = 0.0;
        //                   sum = 0.0;
        //                   weight = 0.0;
        //                   /* define the first x and y points in the smoothing window */
        //                   if (x - Math.Floor(n_smooth / 2) < 0)
        //                   {
        //                       x_start = 0;
        //                   }
        //                   else
        //                   {
        //                       x_start = x - Math.Floor(n_smooth / 2);
        //                   }
        //                   if (y - Math.Floor(n_smooth / 2) < 0)
        //                   {
        //                       y_start = 0;
        //                   }
        //                   else
        //                   {
        //                       y_start = y - Math.Floor(n_smooth / 2);
        //                   }
        //                   /* define the last x and y points in the smoothing window */
        //                   if (x + Math.Floor(n_smooth / 2) >= n_x)
        //                   {
        //                       x_end = n_x - 1;
        //                   }
        //                   else
        //                   {
        //                       x_end = x + Math.Floor(n_smooth / 2);
        //                   }
        //                   if (y + Math.Floor(n_smooth / 2) >= n_y)
        //                   {
        //                       y_end = n_y - 1;
        //                   }
        //                   else
        //                   {
        //                       y_end = y + Math.Floor(n_smooth / 2);
        //                   }
        //                   /* apply the smoothing window */
        //                   for (xx = x_start; xx <= x_end; xx++)
        //                   {
        //                       for (yy = y_start; yy <= y_end; yy++)
        //                       {
        //                           weight = (1.0 - Math.Sqrt((double)Math.Pow(((xx - x_start) - (x_end - x_start) / 2), 2) + (double)Math.Pow(((yy - y_start) - (y_end - y_start) / 2), 2)) / 10.0);
        //                           sum = sum + dat[xx][yy] * weight;
        //                           count = count + weight;
        //                       }
        //                   }
        //                   @out[x][y] = sum / count;
        //               }
        //           }
        //           return 1;
        //       }


        //       //running mean (general - previous function uses the "k" argument with a function of log10 to be specific to power spectral densities).
        //       //also new is the psd version is directly from david dolenc's code while this is more from rob's smoothing code, just simplified from the 2-d case to the 1-d case.
        //       private void smooth_1d_array(double[] arr_in, double[] arr_out, int npts, int n_smooth)
        //       {
        //           int i;
        //           int j;
        //           int start;
        //           int end;
        //           int count;
        //           double sum;
        //           if (n_smooth <= 0 || n_smooth >= npts)
        //           {
        //               return;
        //           }

        //           //odd
        //           if (n_smooth % 2 == 1)
        //           {
        //               for (i = 0; i < npts; i++)
        //               {
        //                   sum = 0.0;
        //                   count = 0;
        //                   if (i - Math.Floor(n_smooth / 2) < 0)
        //                   {
        //                       start = 0;
        //                   }
        //                   else
        //                   {
        //                       start = i - Math.Floor(n_smooth / 2);
        //                   }
        //                   if (i + Math.Floor(n_smooth / 2) > npts - 1)
        //                   {
        //                       end = npts - 1;
        //                   }
        //                   else
        //                   {
        //                       end = i + Math.Floor(n_smooth / 2);
        //                   }
        //                   for (j = start; j <= end; j++)
        //                   {
        //                       sum = sum + arr_in[j];
        //                       count = count + 1;
        //                   }
        //                   arr_out[i] = sum / (double)count;
        //               }
        //           }
        //           else
        //           {
        //               for (i = 0; i < npts; i++)
        //               {
        //                   sum = 0.0;
        //                   count = 0;
        //                   if (i - Math.Floor(n_smooth / 2) < 0)
        //                   {
        //                       start = 0;
        //                   }
        //                   else
        //                   {
        //                       start = i - Math.Floor(n_smooth / 2);
        //                   }
        //                   if (i + Math.Floor(n_smooth / 2) - 1 > npts - 1)
        //                   {
        //                       end = npts - 1;
        //                   }
        //                   else
        //                   {
        //                       end = i + Math.Floor(n_smooth / 2) - 1;
        //                   }
        //                   for (j = start; j <= end; j++)
        //                   {
        //                       sum = sum + (arr_in[j] + arr_in[j + 1]) / 2.0;
        //                       count = count + 1;
        //                   }
        //                   arr_out[i] = sum / (double)count;
        //               }
        //           }
        //           return;
        //       }
        //       //END

        //       //compute a mean value recursively
        //       private double recursive_mean(double new_value, double old_mean, int n)
        //       {
        //           if (n >= 1)
        //           {
        //               return (1 / (double)n) * (((double)n - 1) * old_mean + new_value);
        //           }
        //           else
        //           {
        //               return new_value;
        //           }
        //       }
        //       //END

        //       //compute a standard deviation recursively
        //       private double recursive_standard_deviation(double new_value, double current_mean, double prev_value, int n)
        //       {
        //           double temp = 0.0;
        //           double new_sd = 0.0;
        //           if (n > 1)
        //           {
        //               temp = (prev_value * prev_value * ((double)n - 2)) + ((double)n / ((double)n - 1)) * (current_mean - new_value) * (current_mean - new_value);
        //               new_sd = Math.Sqrt(temp / ((double)n - 1));
        //           }
        //           else
        //           {
        //               //fprintf(stderr,"Recursive_standard_deviation error: n must be greater than 1! n: %d\n",n);
        //               new_sd = 0.0;
        //           }
        //           return new_sd;
        //       }
        //       //END


        //       //////running_mean
        //       private void running_mean_psd(double[] arr_in, double[] arr_out, int npts)
        //       {
        //           int i;
        //           int k;
        //           int j;
        //           double sum = 0.0;

        //           for (i = 1; i < npts; i++)
        //           {
        //               k = Math.Floor(2 * (Math.Log10(i)) * (Math.Log10(i)) + 0.5);
        //               if (i < 3)
        //               {
        //                   arr_out[i] = arr_in[i];
        //               }
        //               else if (i + k < npts)
        //               {
        //                   sum = 0.0;
        //                   for (j = -k; j <= k; j++)
        //                   {
        //                       if (i + j < npts)
        //                       {
        //                           sum = sum + arr_in[i + j];
        //                       }
        //                   }
        //                   arr_out[i] = sum / (2 * k + 1);
        //               }
        //               else
        //               {
        //                   arr_out[i] = arr_in[i];
        //               }
        //           }
        //           return;
        //       }
        //       ///END


        //       //calculate the power spectrum
        //       private void calculate_psd(fftw_complex[] spect, double[] psd_out, double dt, int npts)
        //       {
        //           int i;
        //           int nspec = (int)Math.Floor(npts / 2 + 1.5);
        //           for (i = 0; i < nspec; i++)
        //           {
        //               psd_out[i] = 10.0 * Math.Log10((spect[i][0] * spect[i][0] + spect[i][1] * spect[i][1]) * 2 * dt / (double)npts);
        //           }
        //           running_mean_psd(psd_out, psd_out, nspec);
        //           return;
        //       }
        //       //END


        //       //do everything in prep_sac_file....
        //       private void signal_to_ground_acceleration(ref double @in, double[] @out, int npts, double delta, POLE_ZERO pz, int acc_flag, double[] signal, fftw_complex[] signal_spectrum, fftw_complex[] response_spectrum, double[] fftw_dbl, fftw_complex[] fftw_cmplx, fftw_plan plan_forward, fftw_plan plan_backward)
        //       {
        //           int i;
        //           int nspec;
        //           double freq;
        //           COMPLEX_RP temp_crp = new COMPLEX_RP();
        //           nspec = (int)Math.Floor(npts / 2 + 1.5);

        //           /* some simple preprocessing removing a mean and linear trend to remove dc offsets after fft */
        //           //remove_mean(in,signal,npts);
        //           remove_trend(@in, signal, npts);


        //           /* apply a hann taper */
        //           //cos_taper(signal,signal,npts);
        //           hann_taper(signal, signal, npts);

        //           /* fft forward. and correct for energy loss in hann taper*/
        //           for (i = 0; i < npts; i++)
        //           {
        //               fftw_dbl[i] = signal[i] * Math.Sqrt(8.0 / 3.0);
        //           }
        //           fftw_execute(plan_forward);
        //           /* put the complex spectra into a safe format */
        //           for (i = 0; i < nspec; i++)
        //           {
        //               signal_spectrum[i][0] = fftw_cmplx[i][0];
        //               signal_spectrum[i][1] = fftw_cmplx[i][1];
        //           }

        //           /* Sequence to compute the frequency domain response of a given pole-zero structure */
        //           for (i = 0; i < nspec; i++)
        //           {
        //               freq = (double)i / (delta * (double)npts);
        //               temp_crp = generate_response(pz, freq);
        //               response_spectrum[i][0] = temp_crp.real;
        //               response_spectrum[i][1] = temp_crp.imag;
        //           }

        //           /* doing a deconvolution (complex division) of the response spectrum from the signal spectrum */
        //           decon_response_function(signal_spectrum, response_spectrum, signal_spectrum, nspec);

        //           /* return to the time domain for the differentiation */
        //           for (i = 0; i < nspec; i++)
        //           {
        //               fftw_cmplx[i][0] = signal_spectrum[i][0];
        //               fftw_cmplx[i][1] = signal_spectrum[i][1];
        //           }
        //           fftw_execute(plan_backward);
        //           for (i = 0; i < npts; i++)
        //           {
        //               signal[i] = fftw_dbl[i] / npts;
        //           }

        //           /* the response returns displacement. Therefore we differentiate twice to get acceleration */
        //           //	a single derivative seems to return acceleration when comparing with the noise model. Thus give the option for 0, 1, or 2 derivatives
        //           if (acc_flag == 0)
        //           {
        //               for (i = 0; i < npts; i++)
        //               {
        //                   @out[i] = signal[i];
        //               }
        //           }
        //           else if (acc_flag == 1)
        //           {
        //               time_derivative(signal, @out, delta, npts);
        //           }
        //           else
        //           {
        //               time_derivative(signal, signal, delta, npts);
        //               time_derivative(signal, @out, delta, npts);
        //           }

        //           return;
        //       }
        //       //END

        //       //phase_shift  
        private void phase_shift(double t_shift, double dt, int np, fftw_complex[] spect_in)
        {
            double tpi = 2 * PI;
            double t;
            double w;
            double phi;
            int i;
            int j;
            int nspec;
            COMPLEX_RP ca = new COMPLEX_RP();
            COMPLEX_RP cx = new COMPLEX_RP();
            COMPLEX_RP temp_complex = new COMPLEX_RP();
            COMPLEX_RP cb = new COMPLEX_RP();

            nspec = (int)Math.Floor(np / 2 + 1.5);
            t = (double)np * dt;

            //	for (i=1; i < np / 2+1; i++) {
            for (i = 0; i < nspec; i++)
            {
                temp_complex.real = spect_in[i][0];
                temp_complex.imag = spect_in[i][1];
                //		printf("i: %d\n",i);
                //		j = i - 1;
                j = i;
                w = tpi * (double)j / t;
                phi = w * t_shift;
                ca.real = 0.0;
                ca.imag = -1.0 * phi;
                cb = complex_exp(ca);
                cx = complex_multiply(temp_complex, cb);
                spect_in[i][0] = cx.real;
                spect_in[i][1] = cx.imag;
            }
            return;
        }
        //END


        //       ///FIND_TIME_SHIFT
        //       private double find_time_shift(int sec_1, int msec_1, int sec_2, int msec_2)
        //       {
        //           double time_shift;
        //           //	printf("sec_2: %d msec_2: %d, sec_1: %d, msec_1: %d\n", sec_2, msec_2, sec_1, msec_1);
        //           time_shift = ((double)sec_2 + (double)msec_2 / 1000.0) - ((double)sec_1 + (double)msec_1 / 1000.0);
        //           return time_shift;
        //       }
        //       ///END

        //       //FIND_COMMON_SAMPLE_RATE
        //       private double find_common_sample_rate(double dt_1, double dt_2)
        //       {
        //           double out_rate;
        //           int sps_1;
        //           int sps_2;

        //           sps_1 = (int)Math.Floor(1 / dt_1 + .5);
        //           sps_2 = (int)Math.Floor(1 / dt_2 + .5);

        //           out_rate = 1.0 / gcd(sps_1, sps_2);

        //           return out_rate;
        //       }
        //       //END

        //       //Greatest Common Divisor
        //       private int gcd(int a, int b)
        //       {
        //           int @out;
        //           int c;

        //           if (a < b)
        //           {
        //               c = a;
        //               a = b;
        //               b = c;
        //           }

        //           c = 1;
        //           while (c != 0)
        //           {
        //               c = a % b;
        //               if (c != 0)
        //               {
        //                   a = b;
        //                   b = c;
        //               }
        //               else
        //               {
        //                   @out = b;
        //               }
        //           }

        //           return @out;
        //       }
        //       //END

        //       //decimate_signal
        //       private void decimate_signal(double[] input_signal, double[] arr_out, int npts, double dt_in, double dt_out)
        //       {
        //           int npts_out;
        //           int i;
        //           int j;

        //           npts_out = (int)npts * dt_in / dt_out;
        //           j = 0;

        //           for (i = 0; i < npts_out; i++)
        //           {
        //               j = (int)i * dt_out / dt_in;
        //               if (j < npts)
        //               {
        //                   arr_out[i] = input_signal[j];
        //               }
        //               else
        //               {
        //                   arr_out[i] = input_signal[npts - 1];
        //               }
        //           }

        //           return;
        //       }
        //       ///END


        //       //6pole low pass butterworth filter - translated from bob urhammer's fortran code
        //       private void bw6plp(double[] x, double[] arr_out, int np, double dt, double lpfc)
        //       {
        //           // 6 pole low-pass butterworth filter
        //           /* local variables */
        //           int i;
        //           int k;
        //           int nbpp;
        //           double wc;
        //           double hc;
        //           double dc;
        //           double cc11;
        //           double cc12;
        //           double cc13;
        //           double cc21;
        //           double cc22;
        //           double cc23;
        //           double cc31;
        //           double cc32;
        //           double cc33;
        //           double wim1;
        //           double xim1;
        //           double yim1;
        //           double zim1;
        //           double wi;
        //           double xi;
        //           double yi;
        //           double zi;
        //           double wim2;
        //           double xim2;
        //           double yim2;
        //           double zim2;

        //           /*initialize some variables. May be a wise decision to make these inputs */
        //           //dt = 1 / 200; /* samples per second ie 200Hz */
        //           //lpfc = 32.0;
        //           nbpp = 3;
        //           wc = Math.Tan(PI * lpfc * dt);

        //           k = 0;
        //           hc = Math.Sin(PI * (2.0 * (double)k + 1.0) / (2.0 * (double)nbpp));
        //           dc = wc * wc + 2.0 * wc * hc + 1.0;
        //           cc11 = wc * wc / dc;
        //           cc12 = (2.0 - 2.0 * wc * wc) / dc;
        //           cc13 = (-1 * wc * wc + 2.0 * wc * hc - 1.0) / dc;

        //           k = 1;
        //           hc = Math.Sin(PI * (2.0 * (double)k + 1.0) / (2.0 * (double)nbpp));
        //           dc = wc * wc + 2.0 * wc * hc + 1.0;
        //           cc21 = wc * wc / dc;
        //           cc22 = (2.0 - 2.0 * wc * wc) / dc;
        //           cc23 = (-1 * wc * wc + 2.0 * wc * hc - 1.0) / dc;

        //           k = 2;
        //           hc = Math.Sin(PI * (2.0 * (double)k + 1.0) / (2.0 * (double)nbpp));
        //           dc = wc * wc + 2.0 * wc * hc + 1.0;
        //           cc31 = wc * wc / dc;
        //           cc32 = (2.0 - 2.0 * wc * wc) / dc;
        //           cc33 = (-1 * wc * wc + 2.0 * wc * hc - 1.0) / dc;

        //           /* initialize temporary variables */
        //           wim1 = x[0];
        //           xim1 = x[0];
        //           yim1 = x[0];
        //           zim1 = x[0];
        //           wi = x[0];
        //           xi = x[0];
        //           yi = x[0];
        //           zi = x[0];

        //           /* apply recursive 6PLP filter */
        //           for (i = 0; i < np; i++)
        //           {
        //               wim2 = wim1;
        //               wim1 = wi;
        //               wi = x[i];
        //               xim2 = xim1;
        //               xim1 = xi;
        //               xi = cc11 * (wi + 2.0 * wim1 + wim2) + cc12 * xim1 + cc13 * xim2;
        //               yim2 = yim1;
        //               yim1 = yi;
        //               yi = cc21 * (xi + 2.0 * xim1 + xim2) + cc22 * yim1 + cc23 * yim2;
        //               zim2 = zim1;
        //               zim1 = zi;
        //               zi = cc31 * (yi + 2.0 * yim1 + yim2) + cc32 * zim1 + cc33 * zim2;
        //               x[i] = zi;
        //               arr_out[i] = x[i];
        //           }
        //           return;
        //       } // end bw6plp subprogram
        //       ///END


        //       //time derivative (as sac 2 point algorithm)
        //       private void time_derivative(double[] @in, double[] @out, double delta, int npts)
        //       {
        //           int i;
        //           for (i = 0; i < npts - 1; i++)
        //           {
        //               @out[i] = (@in[i + 1] - @in[i]) / delta;
        //           }
        //           //last value is not filled by sac. Here I fill it with the same as the previous value to have a minimal effect on future usage
        //           @out[npts - 1] = @out[npts - 2];

        //           return;
        //       }
        //       //END

        //       //get_amplitude_spectrum
        //       private void get_amplitude_spectrum(SPECTRUM spect_in, double[] arr_out)
        //       {
        //           int i;

        //           for (i = 0; i < spect_in.N; i++)
        //           {
        //               arr_out[i] = Math.Sqrt(Math.Pow(spect_in.re[i], 2.0) + Math.Pow(spect_in.im[i], 2.0));
        //           }

        //           return;
        //       }
        //       //END


        //       //to initailize a pole zero structure
        //       private void zero_pole_zero(POLE_ZERO pz)
        //       {
        //           int i;
        //           pz.scale = 0.0;
        //           pz.n_zeroes = 0;
        //           pz.n_poles = 0;
        //           for (i = 0; i < N_MAX_ZEROES; i++)
        //           {
        //               pz.zeroes[i].real = 0.0;
        //               pz.zeroes[i].imag = 0.0;
        //           }
        //           for (i = 0; i < N_MAX_POLES; i++)
        //           {
        //               pz.poles[i].real = 0.0;
        //               pz.poles[i].imag = 0.0;
        //           }

        //           return;
        //       }
        //       //END

        //       //generate response
        //       //generates the response for a specific frequency given in hertz
        //       private COMPLEX_RP generate_response(POLE_ZERO pz, double freq)
        //       {
        //           /* local variables */
        //           int i;
        //           double mod_squared;
        //           COMPLEX_RP omega = new COMPLEX_RP();
        //           COMPLEX_RP denom = new COMPLEX_RP();
        //           COMPLEX_RP num = new COMPLEX_RP();
        //           COMPLEX_RP temp = new COMPLEX_RP();
        //           COMPLEX_RP @out = new COMPLEX_RP();

        //           /* initializations */
        //           freq = freq * 2 * PI;
        //           omega.real = 0.0;
        //           omega.imag = freq;
        //           denom.real = 1.0;
        //           denom.imag = 1.0;
        //           num.real = 1.0;
        //           num.imag = 1.0;

        //           /* compute the complex laplacian */
        //           for (i = 0; i < pz.n_zeroes; i++)
        //           {
        //               temp = complex_subtract(omega, pz.zeroes[i]);
        //               num = complex_multiply(num, temp);
        //           }
        //           for (i = 0; i < pz.n_poles; i++)
        //           {
        //               temp = complex_subtract(omega, pz.poles[i]);
        //               denom = complex_multiply(denom, temp);
        //           }

        //           /* compute the final zeros / poles */
        //           temp = conjugate(denom);
        //           temp = complex_multiply(temp, num);
        //           mod_squared = denom.real * denom.real + denom.imag * denom.imag;
        //           temp.real = temp.real / mod_squared;
        //           temp.imag = temp.imag / mod_squared;
        //           @out.real = pz.scale * temp.real;
        //           @out.imag = pz.scale * temp.imag;

        //           return @out;
        //       }
        //       //END



        //       //decon_response_function
        //       private void decon_response_function(fftw_complex[] data_spect, fftw_complex[] response_spect, fftw_complex[] spect_out, int nspec)
        //       {
        //           int i;
        //           COMPLEX_RP temp_data = new COMPLEX_RP();
        //           COMPLEX_RP temp_response = new COMPLEX_RP();
        //           COMPLEX_RP temp_out = new COMPLEX_RP();

        //           for (i = 1; i < nspec; i++)
        //           {
        //               temp_data.real = data_spect[i][0];
        //               temp_data.imag = data_spect[i][1];
        //               temp_response.real = response_spect[i][0];
        //               temp_response.imag = response_spect[i][1];
        //               temp_out = complex_divide(temp_data, temp_response);
        //               spect_out[i][0] = temp_out.real;
        //               spect_out[i][1] = temp_out.imag;
        //           }

        //           return;

        //       }
        //       ///END


        //////average ratio between two spectrums
        ////private COMPLEX_RP AverageSpectralRatio(FftwArrayComplex[] signal_spectrum, FftwArrayComplex[] untapered_spectrum, int nspec)
        ////{
        ////    int i;
        ////    double sum_re = 0.0;
        ////    double sum_im = 0.0;
        ////    double temp;
        ////    COMPLEX_RP ratio = new COMPLEX_RP();

        ////    ratio.real = 0.0;
        ////    ratio.imag = 0.0;

        ////    for (i = 0; i < nspec; i++)
        ////    {
        ////        if (signal_spectrum[i][0] < 0.001 || untapered_spectrum[i][0] < 0.001)
        ////        {
        ////            temp = 1.0;
        ////        }
        ////        else
        ////        {
        ////            temp = Math.Abs(signal_spectrum[i][0] / untapered_spectrum[i][0]);
        ////        }
        ////        sum_re = sum_re + temp;
        ////        if (signal_spectrum[i][1] < 0.001 || untapered_spectrum[i][1] < 0.001)
        ////        {
        ////            temp = 1.0;
        ////        }
        ////        else
        ////        {
        ////            temp = Math.Abs(signal_spectrum[i][1] / untapered_spectrum[i][1]);
        ////        }
        ////        sum_im = sum_im + temp;

        ////    }

        ////    ratio.real = sum_re / (double)nspec;
        ////    ratio.imag = sum_im / (double)nspec;

        ////    return ratio;
        ////}
        //////END


        //calculate a 10% cosine taper
        private static void CcosTaper(double[] arr_in, ref double[] arr_out, int npts)
        {
            int i;
            int M;
            M = (int)Math.Floor(((npts - 1) / 10) / 2 + .5);

            for (i = 0; i < npts; i++)
            {
                if (i < M)
                {
                    arr_out[i] = arr_in[i] * (0.5 * (1 - Math.Cos(i * PI / (M + 1))));
                }
                else if (i < npts - M - 2)
                {
                    arr_out[i] = arr_in[i];
                }
                else if (i < npts)
                {
                    arr_out[i] = arr_in[i] * (0.5 * (1 - Math.Cos((npts - i - 1) * PI / (M + 1))));
                }
            }
            return;
            //http://www.cbi.dongnocchi.it/glossary/DataWindow.htm
        }
        /////END

        public static void test_CcosTaper()
        {
            double[] arr_in = new double[] { 1, 21, 3, 4, 5 };
            double[] arr_out = new double[] { 1, 2, 3, 4, 5 };
            CcosTaper(arr_in, ref arr_out, 5);
            foreach (double d in arr_out)
            {
                Console.WriteLine(d);
            }
        }


        //remove_mean
        private static void RemoveMean(double[] arr_in, ref double[] arr_out, int npts)
        {
            int i;
            double sum = 0.0;
            for (i = 0; i < npts; i++)
            {
                sum = sum + arr_in[i];
            }
            double ave = sum / npts;
            for (i = 0; i < npts; i++)
            {
                arr_out[i] = arr_in[i] - ave;
            }
            return;
        }
        ///END

        public static void test_RemoveMean()
        {
            double[] arr_in = new double[] { 1, 21, 3, 4, 5 };
            double[] arr_out = new double[] { 1, 2, 3, 4, 5 };
            RemoveMean(arr_in, ref arr_out, 5);
            foreach (double d in arr_out)
            {
                Console.WriteLine(d);
            }
        }


        private static void RemoveTrend(double[] arr_in, ref double[] arr_out, int npts)
        {
            int i;
            double ata11;
            double ata12;
            double ata21;
            double ata22;
            double atb1;
            double atb2;
            double fn;
            double di;
            double d;
            double a;
            double b;
            double ata11i;
            double ata12i;
            double ata21i;
            double ata22i;
            //	double mean, sum=0.0;

            /* removes trend with a linear least squares approach */
            ata11 = 0.0;
            ata12 = 0.0;
            ata21 = 0.0;
            ata22 = 0.0;
            atb1 = 0.0;
            atb2 = 0.0;
            fn = (double)npts * 1.0;

            for (i = 0; i < npts; i++)
            {
                di = (double)i;
                ata11 = ata11 + 1.0;
                ata12 = ata12 + di;
                ata21 = ata21 + di;
                ata22 = ata22 + di * di;
                atb1 = atb1 + arr_in[i];
                atb2 = atb2 + arr_in[i] * di;
                //	sum = sum + arr_in[i];
            }
            //mean = sum / ((double) npts);

            d = ata11 * ata22 - ata12 * ata21;
            ata11i = ata22 / d;
            ata12i = -1 * ata12 / d;
            ata21i = -1 * ata21 / d;
            ata22i = ata11 / d;
            a = ata11i * atb1 + ata12i * atb2;
            b = ata21i * atb1 + ata22i * atb2;

            //printf("a: %lf, b: %lf\nmean: %lf\n",a,b, mean);

            for (i = 0; i < npts; i++)
            {
                arr_out[i] = arr_in[i] - (a + b * (double)i);
            }
            return;
        } // end linear detrend
          ////END

        public static void test_RemoveTrend()
        {
            double[] arr_in = new double[] { 1, 21, 3, 4, 5 };
            double[] arr_out = new double[] { 1, 2, 3, 4, 5 };
            RemoveTrend(arr_in, ref arr_out, 5);
            foreach (double d in arr_out)
            {
                Console.WriteLine(d);
            }
        }

        //complex math
        private COMPLEX_RP Conjugate(COMPLEX_RP comp)
        {
            COMPLEX_RP comp_out = new COMPLEX_RP();
            comp_out.real = comp.real;
            comp_out.imag = -1.0 * comp.imag;
            return comp_out;
        }

        private COMPLEX_RP ComplexMultiply(COMPLEX_RP comp1, COMPLEX_RP comp2)
        {
            COMPLEX_RP comp_out = new COMPLEX_RP();
            //(a+bi)(c+di):
            //ac+adi+bci-bd
            comp_out.real = comp1.real * comp2.real - comp1.imag * comp2.imag;
            comp_out.imag = comp1.real * comp2.imag + comp2.real * comp1.imag;
            return comp_out;
        }

        private COMPLEX_RP ComplexDivide(COMPLEX_RP comp1, COMPLEX_RP comp2)
        {
            COMPLEX_RP comp_out = new COMPLEX_RP();
            double denom;
            denom = comp2.real * comp2.real + comp2.imag * comp2.imag; // + 1.2e-32;
            comp_out.real = (comp1.real * comp2.real + comp1.imag * comp2.imag) / (denom);
            comp_out.imag = (comp1.imag * comp2.real - comp1.real * comp2.imag) / (denom);
            return comp_out;
        }


        private COMPLEX_RP ComplexAdd(COMPLEX_RP comp1, COMPLEX_RP comp2)
        {
            COMPLEX_RP comp_out = new COMPLEX_RP();
            comp_out.real = comp1.real + comp2.real;
            comp_out.imag = comp1.imag + comp2.imag;
            return comp_out;
        }

        private COMPLEX_RP ComplexSubtract(COMPLEX_RP comp1, COMPLEX_RP comp2)
        {
            COMPLEX_RP comp_out = new COMPLEX_RP();
            comp_out.real = comp1.real - comp2.real;
            comp_out.imag = comp1.imag - comp2.imag;
            return comp_out;
        }

        private static COMPLEX_RP ComplexExp(COMPLEX_RP comp)
        {
            COMPLEX_RP comp_out = new COMPLEX_RP();
            comp_out.real = Math.Exp(comp.real) * Math.Cos(comp.imag);
            comp_out.imag = Math.Exp(comp.real) * Math.Sin(comp.imag);
            return comp_out;
        }

        public static void test_ComplexExp()
        {
            COMPLEX_RP cOMPLEX_RP = new COMPLEX_RP();
            cOMPLEX_RP.real = 1;
            cOMPLEX_RP.imag = 2;

            COMPLEX_RP r = ComplexExp(cOMPLEX_RP);

            Console.WriteLine("real = " + r.real + " img = " + r.imag);

        }

        private static double ComplexAmplitude(COMPLEX_RP comp)
        {
            return Math.Sqrt(comp.real * comp.real + comp.imag * comp.imag);
        }

        public static void test_ComplexAmplitude()
        {
            COMPLEX_RP cOMPLEX_RP = new COMPLEX_RP();
            cOMPLEX_RP.real = 1;
            cOMPLEX_RP.imag = 2;

            double r = ComplexAmplitude(cOMPLEX_RP);
            
                Console.WriteLine(r);
            
        }


        //initialize_spectrum
        private void InitializeSpectrum(SPECTRUM spect, int npts)
        {
            // C# does not have, or use malloc in any way, shape or form.  
            //.NET applications are Managed: which means they have their own memory management 
            //built in (via the new keyword) and which is tied into the Garbage Collection system. 
            //They do not use malloc, or free as the memory is handled by the system for you
            spect.N = npts / 2 + 1;
            spect.im = new double[spect.N];
            spect.re = new double[spect.N];

            /*
            //C++ TO C# CONVERTER TODO TASK: The memory management function 'malloc' has no equivalent in C#:
            if ((spect.re = malloc(sizeof(*spect.re) * spect.N)) == null)
 {
                Console.Write("Error initializing real component of spectrum\n");
                Environment.Exit(1);
            }
            //C++ TO C# CONVERTER TODO TASK: The memory management function 'malloc' has no equivalent in C#:
            if ((spect.im = malloc(sizeof(*spect.im) * spect.N)) == null)
 {
                Console.Write("Error initializing imaginary component of spectrum\n");
                Environment.Exit(1);
            }*/

            return;
        }


        ///hann taper
        private static void HannTaper(double[] arr_in, ref double[] arr_out, int npts)
        {
            int i;
            for (i = 0; i < npts; i++)
            {
                arr_out[i] = arr_in[i] * (0.5 * (1 - Math.Cos(2 * i * PI / (npts))));
            }

            return;
        }
        ///END

        public static void test_HannTaper()
        {
            double[] arr_in = new double[] { 1, 2, 3, 4, 5 };
            double[] arr_out = new double[] { 1, 2, 3, 4, 5 };
            HannTaper(arr_in, ref arr_out, 5);
            foreach (double d in arr_out)
            {
                Console.WriteLine(d);
            }
        }



        //hamming_taper
        private static void HammingTaper(double[] arr_in, ref double[] arr_out, int npts)
        {
            int i;
            for (i = 0; i < npts; i++)
            {
                arr_out[i] = arr_in[i] * (0.54 - 0.46 * Math.Cos(2 * i * PI / (npts - 1)));
            }
            return;
        }
        ///END

        public static void test_HammingTaper()
        {
            double[] arr_in = new double[] { 1, 2, 3, 4, 5 };
            double[] arr_out = new double[] { 1, 2, 3, 4, 5 };
            HammingTaper(arr_in, ref arr_out,5);
            foreach (double d in arr_out)
            {
                Console.WriteLine(d);
            }
        }


        ///get_phase_spectrum
        public static void GetPhaseSpectrum(SPECTRUM spect_in, ref double[] arr_out)
        {
            int i;

            for (i = 0; i < spect_in.N; i++)
            {
                arr_out[i] = Math.Atan2(spect_in.im[i], spect_in.re[i]);
            }

            return;
        }
        //END   

        public static void test_GetPhaseSpectrum()
        {
            Definitions.SPECTRUM sPECTRUM = new Definitions.SPECTRUM();
            sPECTRUM.N = 4;
            sPECTRUM.re = new double[] { 1, 2, 3, 4 };
            sPECTRUM.im = new double[] { 1, 2, 3, 4 };
            double[] result = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            StationAnalysisTools.GetPhaseSpectrum(sPECTRUM, ref result);
            foreach (double d in result)
            {
                Console.WriteLine(d);
            }
        }

    }
}
