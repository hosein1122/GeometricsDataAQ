using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using FFTW.NET;

namespace StationAnalysisToolsNetCore
{
    public class ComputePsd
    {
        int acc_flag = 2;
        int n_second_sub_record = 1000;
        int n_sec_shift = 100;

        //float[] temp_array_f;
        double[] signal;
        double[] temp_signal;
        double[] temp_psd;
        double[] mean_psd;
        double[] sd_psd;
        int n_psds = 0;
        Definitions.POLE_ZERO pz = new Definitions.POLE_ZERO();
        //Complex[] temp_spect;
        FftwArrayComplex temp_spect;
        double[] stga_signal;
        //FftwArrayComplex stga_fftw_dbl;
        PinnedArray<double> stga_fftw_dbl;
        FftwArrayComplex stga_fftw_cmplx;
        FftwArrayComplex stga_signal_spectrum;
        FftwArrayComplex stga_response_spectrum;
        FftwPlanRC stga_plan_forward, stga_plan_backward;

        //FftwPlan stga_plan_forward, stga_plan_backward;
        public ComputePsd()
        {
            //run some checks on the parameters
            //first check is for a reasonable unit to compute
            if (acc_flag != 0 && acc_flag != 1 && acc_flag != 2)
            {
                Console.Error.Write("Error, acc_flag must equal 0, 1, or 2. Determines the number of differentiations from the displacement response.\n");
                Environment.Exit(1);

            }
            //second check that the sub record and time shift are non-zero
            if (n_second_sub_record <= 0.0)
            {
                Console.Error.Write("Error, n_second_sub_record must be greater than 0.\n");
                Environment.Exit(1);
            }
            if (n_sec_shift <= 0.0)
            {
                Console.Error.Write("Error, n_sec_shift must be greater than 0.\n");
                Environment.Exit(1);
            }

            SAC.SacTimeSeries sacTimeSeries = new SAC.SacTimeSeries("d:\\tmp\\pnina.LHZ.SAC");

            signal = Array.ConvertAll<float, double>(sacTimeSeries.Y, x => x);

            //read the pole zero files
            SAC.SacPoleZero sacPoleZero = new SAC.SacPoleZero("d:\\tmp\\cmg3t.pz");
            pz.n_poles = sacPoleZero.Poles.Length;
            pz.n_zeroes = sacPoleZero.Zeros.Length;
            pz.poles = new Definitions.COMPLEX_RP[pz.n_poles];
            pz.zeroes = new Definitions.COMPLEX_RP[pz.n_zeroes];
            for (int i = 0; i < pz.n_poles; i++)
            {
                pz.poles[i] = new Definitions.COMPLEX_RP();
                pz.poles[i].real = sacPoleZero.Poles[i].Real;
                pz.poles[i].imag = sacPoleZero.Poles[i].Imaginary;
            }
            for (int i = 0; i < pz.n_zeroes; i++)
            {
                pz.zeroes[i] = new Definitions.COMPLEX_RP();
                pz.zeroes[i].real = sacPoleZero.Zeros[i].Real;
                pz.zeroes[i].imag = sacPoleZero.Zeros[i].Imaginary;
            }
                      
            pz.scale = sacPoleZero.Constant;

            //invert delta to samples per second
            var samples_per_second = Math.Floor(1.0 / sacTimeSeries.Header.Delta + 0.5);

            //hard wired for 8 hour data segments. (8*3600 = seconds in 8 hours)
            var temp_npts = (int)Math.Floor(n_second_sub_record * samples_per_second + 0.5);

            // var  stga_plan_forward = fftw_plan_dft_r2c_1d(temp_npts, stga_fftw_dbl, stga_fftw_cmplx, FFTW_ESTIMATE);
            temp_signal = new double[temp_npts];
            temp_psd = new double[temp_npts / 2 + 1];
            mean_psd = new double[temp_npts / 2 + 1];
            sd_psd = new double[temp_npts / 2 + 1];
            temp_spect = new FftwArrayComplex(temp_npts / 2 + 1);
            stga_signal = new double[temp_npts];
            stga_fftw_dbl = new PinnedArray<double>(temp_npts); //(temp_npts);
            stga_fftw_cmplx = new FftwArrayComplex(temp_npts / 2 + 1);
            stga_signal_spectrum = new FftwArrayComplex(temp_npts / 2 + 1);
            stga_response_spectrum = new FftwArrayComplex(temp_npts / 2 + 1);

            stga_plan_forward = FftwPlanRC.Create(stga_fftw_dbl, stga_fftw_cmplx, DftDirection.Forwards, PlannerFlags.Estimate, 1);
            stga_plan_backward = FftwPlanRC.Create(stga_fftw_dbl, stga_fftw_cmplx, DftDirection.Backwards, PlannerFlags.Estimate, 1);
            n_psds = (sacTimeSeries.Header.Npts - temp_npts) / (int)(n_sec_shift * (float)samples_per_second) + 1;

            //clear out the mean and sd...
            for (int ii = 0; ii < temp_npts / 2 + 1; ii++)
            {
                mean_psd[ii] = 0.0;
                sd_psd[ii] = 0.0;
            }

            for (int j = 0; j < n_psds; j++)
            {
                //hard wired to step 1 hour for each data segment
                var start_sample = (int)(j * n_sec_shift * samples_per_second);
                //make sure we don't go out of bounds
                if (start_sample + temp_npts < sacTimeSeries.Header.Npts)
                {
                    //fill with data from the current segment
                    for (int ii = 0; ii < temp_npts; ii++)
                    {
                        temp_signal[ii] = signal[start_sample + ii];
                    }
                    //convert to acceleration in m/s/s
                    StationAnalysisTools.Signal_to_ground_acceleration(temp_signal, temp_signal, temp_npts, sacTimeSeries.Header.Delta, (pz), acc_flag, stga_signal, stga_signal_spectrum, stga_response_spectrum, stga_fftw_dbl, stga_fftw_cmplx, stga_plan_forward, stga_plan_backward);

                    //fft
                    for (int ii = 0; ii < temp_npts; ii++)
                    {
                        stga_fftw_dbl[ii] = temp_signal[ii];
                    }
                    //fftw_execute(stga_plan_forward);
                    stga_plan_forward.Execute();

                    for (int ii = 0; ii < temp_npts / 2 + 1; ii++)
                    {
                        // temp_spect[ii].Real = stga_fftw_cmplx[ii][0];
                        // temp_spect[ii].Imaginary = stga_fftw_cmplx[ii][1];
                        temp_spect[ii] = new Complex(stga_fftw_cmplx[ii].Real, stga_fftw_cmplx[ii].Imaginary);
                    }
                    //compute the power 
                    StationAnalysisTools.Calculate_psd(temp_spect, temp_psd, sacTimeSeries.Header.Delta, temp_npts);
                    //for each period, compute the mean and standard deviation recursively
                    if (j == 0)
                    {
                        for (int ii = 0; ii < temp_npts / 2 + 1; ii++)
                        {
                            mean_psd[ii] = temp_psd[ii];
                            sd_psd[ii] = 0.0;
                        }
                        //				printf("%lf +/- %lf\n",mean_psd[20], sd_psd[20]);
                    }
                    else
                    {
                        for (int ii = 0; ii < temp_npts / 2 + 1; ii++)
                        {
                            mean_psd[ii] = StationAnalysisTools.recursive_mean(temp_psd[ii], mean_psd[ii], j + 1);
                            sd_psd[ii] = StationAnalysisTools.recursive_standard_deviation(temp_psd[ii], mean_psd[ii], sd_psd[ii], j + 1);
                        }
                        //				printf("%lf +/- %lf\n",mean_psd[20], sd_psd[20]);
                    }
                }

            } // done with each segment

            //simple message to tell the user how many psds were calculated
            Console.Out.Write("{0:D} psds computed and averaged\n", n_psds);

            //output
            string output_file = "d:\\tmp\\output.psd";
            List<string> fileCntent = new List<string>();
            for (int i = 1; i < temp_npts / 2 + 1; i++)
            {
                //printf("%3.5lf %3.5lf %3.5lf\n" , 1.0 / ( 1.0 / sac_header.delta * (i) / temp_npts), mean_psd[i], sd_psd[i]);
                // fprintf(output_file, "%3.5lf %3.5lf %3.5lf\n", 1.0 / (1.0 / sac_header.delta * (i) / temp_npts), mean_psd[i], sd_psd[i]);
                var str = (1.0 / (1.0 / sacTimeSeries.Header.Delta * (i) / temp_npts))+" , " + mean_psd[i].ToString() + " , " + sd_psd[i].ToString();

                fileCntent.Add(str);
            }
            File.WriteAllLines(output_file, fileCntent);
            


            
            


        }
    }
}
