#include <stdio.h>
#include <stdint.h>
#include <string.h>
#include <float.h>

void ms_gswap2a (void *data2)
{
  uint16_t *data = data2;
     
  *data = (((*data >> 8) & 0xff) | ((*data & 0xff) << 8));
}


void ms_gswap4a (void *data4)
{
  uint32_t *data = data4;

  *data = (((*data >> 24) & 0xff) | ((*data & 0xff) << 24) |
           ((*data >> 8) & 0xff00) | ((*data & 0xff00) << 8));
}




void ms_gswap8a (void *data8)
{
  uint32_t *data4 = data8;
  uint32_t h0, h1;

  h0 = data4[0];
  h0 = (((h0 >> 24) & 0xff) | ((h0 & 0xff) << 24) |
        ((h0 >> 8) & 0xff00) | ((h0 & 0xff00) << 8));

  h1 = data4[1];
  h1 = (((h1 >> 24) & 0xff) | ((h1 & 0xff) << 24) |
        ((h1 >> 8) & 0xff00) | ((h1 & 0xff00) << 8));

  data4[0] = h1;
  data4[1] = h0;
}


void ms_gswap2 (void *data2)
{
  uint8_t temp;

  union {
    uint8_t c[2];
  } dat;

  memcpy (&dat, data2, 2);
  temp     = dat.c[0];
  dat.c[0] = dat.c[1];
  dat.c[1] = temp;
  memcpy (data2, &dat, 2);
}

void ms_gswap4 (void *data4)
{
  uint8_t temp;

  union {
    uint8_t c[4];
  } dat;

  memcpy (&dat, data4, 4);
  temp     = dat.c[0];
  dat.c[0] = dat.c[3];
  dat.c[3] = temp;
  temp     = dat.c[1];
  dat.c[1] = dat.c[2];
  dat.c[2] = temp;
  memcpy (data4, &dat, 4);
}


void ms_gswap8 (void *data8)
{
  uint8_t temp;

  union {
    uint8_t c[8];
  } dat;

  memcpy (&dat, data8, 8);
  temp     = dat.c[0];
  dat.c[0] = dat.c[7];
  dat.c[7] = temp;

  temp     = dat.c[1];
  dat.c[1] = dat.c[6];
  dat.c[6] = temp;

  temp     = dat.c[2];
  dat.c[2] = dat.c[5];
  dat.c[5] = temp;

  temp     = dat.c[3];
  dat.c[3] = dat.c[4];
  dat.c[4] = temp;
  memcpy (data8, &dat, 8);
}

void test_ms_gswap4a()
{
    int32_t sample= -50;
    printf("sample = %d \n", sample);
    ms_gswap4a (&sample);
    printf("swaped sample= %d \n", sample);
} 
void test_ms_gswap8a()
{
     
    double sample= -100000000000001 ;
    printf("sample = %f \n", sample);
    ms_gswap8a (&sample);
    printf("swaped sample= %f \n", sample);
} 
void test_ms_gswap2()
{
    double sample= 10;
    printf("sample = %f \n", sample);
    ms_gswap2 (&sample);
    printf("swaped sample= %f \n", sample);
} 

void test_ms_gswap8()
{
    double sample= -100000000000001;
    printf("sample = %f \n", sample);
    ms_gswap8 (&sample);
    printf("swaped sample= %f \n", sample);
} 