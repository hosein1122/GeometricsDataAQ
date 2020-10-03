#include <stdio.h>
#include <stdint.h>
#include <string.h>
#include <float.h>
#include "libmseed.h"



void test_ms_doy2md(){
    int result;
    int month=1;
    int mday=1;
    result = ms_doy2md(2020,35,&month,&mday);
    printf("result = %d \n month =%d \n mday = %d \n",result,month,mday);

    
}

void test_ms_md2doy(){
    int jday=1;
    int result=0;
    result = ms_md2doy(2020,2,1,&jday);
    printf("result = %d \n jday =%d \n",result,jday);

}

void test_ms_btime2hptime(){
    BTime btime;
    btime.day=1;
    btime.hour=1;
    btime.fract=0;
    btime.min=1;
    btime.sec=1;
    btime.year=2019;
    hptime_t hpt;
    
    hpt = ms_btime2hptime(&btime);
       
    printf("result = %d \n",hpt);

}

void test_ms_btime2isotimestr(){
    char str[] = "2019/01-01T12:12:12.0";
    BTime btime;
    btime.day=1;
    btime.hour=1;
    btime.fract=12;
    btime.min=1;
    btime.sec=1;
    btime.year=2019;
    int result = 0;
    result = ms_btime2isotimestr(&btime,&str);
    printf("result = %s \n",result);

}

void test_ms_timestr2hptime(){
    char time_str[] = "2020-01-01 01:02:30.0000";
    hptime_t ep=0;
    
    //int ms_genfactmult (double samprate, int16_t *factor, int16_t *multiplier)
    ep = ms_timestr2hptime(&time_str);
    printf("eptime  = %d \n",ep);
   
}
void test_ms_hptime2btime(){

 BTime btime;
            btime.day = 10;
            btime.hour = 10;
            btime.fract = 12;
            btime.min = 23;
            btime.sec = 12;
            btime.year = 2020;
            hptime_t hpt=124555454555454453;
            int tt = ms_hptime2btime(hpt, &btime);

    
}

/*void test_ms_time2hptime_int(){
       hptime_t hpt = ms_time2hptime_int(2020, 1, 1, 1, 1, 1);
}*/
void test_ms_hptime2tomsusecoffset(){
    hptime_t ht1 = 1;
    hptime_t toms = 1;
    int8_t usecoffset = 1;
     int result = ms_hptime2tomsusecoffset(ht1, &toms,  &usecoffset);
}

void test_ms_genfactmult(){
    int dd;
    int16_t factor = 1;
    int16_t multiplier = 2;
    
    //int ms_genfactmult (double samprate, int16_t *factor, int16_t *multiplier)
    dd = ms_genfactmult(32768.0,&factor,&multiplier);
    printf("redce  = %d \n",dd);
    printf("f = %d \n",factor);
    printf("m = %d \n",multiplier);
}

void test_ms_reduce_rate(){
    int dd;
    int16_t factor1 = 1;
    int16_t factor2 = 2;
    
    //ms_reduce_rate (double samprate, int16_t *factor1, int16_t *factor2)
    dd = ms_reduce_rate(32768.0,&factor1,&factor2);
    printf("redce  = %d \n",dd);
    printf("f1 = %d \n",factor1);
    printf("f2 = %d \n",factor2);

}
void test_ms_ratapprox(){
    int dd;
    int num = 0;
    int den = 0;
    
    //(double real, int *num, int *den, int maxval, double precision)
    dd = ms_ratapprox(-0.225656565625,&num,&den,300,0.000001152342);
    printf("root sq = %d \n",dd);
    printf("den = %d \n",den);
    printf("num = %d \n",num);
}


void test_warp(){
    #define TM_WRAP(a, b, m) ((a) = ((a) < 0) ? ((b)--, (a) + (m)) : (a))
    int a=-1;
    int b=1;
    int m=30;
    TM_WRAP(a,b,m);
    
}


void test_TM_LEAP_CHECK()
{
    int num = 0;
   #define TM_LEAP_CHECK(n) ((     !(((n) + 1900) % 400) || (      !(((n) + 1900) % 4) && (((n) + 1900) % 100))    ) != 0)
   for (int i=1;i<3000;i++){
    int leap = TM_LEAP_CHECK(i);
    printf("year = %d , leap = %d \n",i+1900,leap);
    if (leap==0)
        num=num+1;
    else{
        if (num<3 || num>3){
            printf("year = %d , leap = %d , number of zero= %d \n",i+1900,leap,num);
        }
        num=0;
    }
   }

}