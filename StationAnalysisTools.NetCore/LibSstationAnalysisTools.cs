///get_phase_spectrum
void get_phase_spectrum(SPECTRUM *spect_in, double *arr_out) {
        int i;
         
        for (i=0; i<spect_in->N; i++) {
                arr_out[i] = atan2(spect_in->im[i],spect_in->re[i]);
        }       
          
       return;
}       
//END   