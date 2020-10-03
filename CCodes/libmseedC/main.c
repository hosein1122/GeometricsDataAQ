#include <stdio.h>
#include <stdint.h>
#include <string.h>
#include <float.h>



int main(int argc, char **argv)
{
	printf("test gswap functions\n");
    //test_ms_ratapprox();
    //test_ms_reduce_rate();
    //test_ms_genfactmult();
    //test_ms_timestr2hptime();
    //test_ms_doy2md();
    //test_ms_md2doy();
    //test_ms_btime2hptime();
    //test_ms_btime2isotimestr();
    //test_ms_hptime2tomsusecoffset();
    //test_warp();
    //test_TM_LEAP_CHECK();
    //test_ms_hptime2btime();
    test_ms_time2hptime_int();
    /*
    test_ms_gswap8a();
    test_ms_gswap8();
    test_msr_encode_text();
    test_msr_encode_int16();
    test_msr_encode_int32();
    test_msr_encode_float32();
    test_msr_encode_float64();
    test_BITWIDTH();
    test_msr_encode_steim1();
    test_msr_encode_steim2();
     * */
    /*
    int16_t input[8]={-50,2,3,4,5,6,7,8};
    int32_t output[8];
    
    int dd = msr_decode_int16(input,8,output,8,1);
    for (int j = 0; j < dd; j++ ) {
      printf("Element[%d] = %d\n", j, output[j] );
   }
    printf("%d",dd);
   */
	return 0;
}



