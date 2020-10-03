using System;
using System.Collections.Generic;
using System.Text;
using static libmseedNetCore.Constants;

namespace libmseedNetCore
{


    public class genutils
    {


        /***************************************************************************
         * ms_recsrcname:
         *
         * Generate a source name string for a specified raw data record in
         * the format: 'NET_STA_LOC_CHAN' or, if the quality flag is true:
         * 'NET_STA_LOC_CHAN_QUAL'.  The passed srcname must have enough room
         * for the resulting string.
         *
         * Returns a pointer to the resulting string or NULL on error.
         ***************************************************************************/
        private string ms_recsrcname(ref string record, ref string srcname, bool? quality=null)
        {
            fsdh_s fsdh;
            string network = new string(new char[6]);
            string station = new string(new char[6]);
            string location = new string(new char[6]);
            string channel = new string(new char[6]);

            if (record==null)
            {
                return null;
            }

            //این خط پس از بررسی کدهای ارجاع داده شده به این تابع تبدیل خواهد شد
            //fsdh = (fsdh_s)record;
            fsdh = new fsdh_s();

            ms_strncpclean(ref network, fsdh.network, 2);
            ms_strncpclean(ref station, fsdh.station, 5);
            ms_strncpclean(ref location, fsdh.location, 2);
            ms_strncpclean(ref channel, fsdh.channel, 3);

            /* Build the source name string including the quality indicator*/
            if (quality != null)
            {
                srcname = string.Format("{0}_{1}_{2}_{3}_{4}", network, station, location, channel, fsdh.dataquality);
            }

            /* Build the source name string without the quality indicator*/
            else
            {
                srcname = string.Format("{0}_{1}_{2}_{3}", network, station, location, channel);
            }

            return srcname;
        } // End of ms_recsrcname()


        /***************************************************************************
         * ms_splitsrcname:
         *
         * Split srcname into separate components: "NET_STA_LOC_CHAN[_QUAL]".
         * Memory for each component must already be allocated.  If a specific
         * component is not desired set the appropriate argument to NULL.
         *
         * Returns 0 on success and -1 on error.
         ***************************************************************************/
        private int ms_splitsrcname(ref string srcname, ref string net, ref string sta, ref string loc, ref string chan, ref string qual)
        {
            var ids = srcname.Split('_');
            if (ids.Length <= 4)
                return -1;
            net = ids[0];
            sta = ids[1];
            loc = ids[2];
            chan = ids[3];
            if (ids.Length == 5)
                qual = ids[4];

            return 0;
        } // End of ms_splitsrcname()




        /***************************************************************************
         * ms_strncpclean:
         *
         * Copy up to 'length' characters from 'source' to 'dest' while
         * removing all spaces.  The result is left justified and always null
         * terminated.  The destination string must have enough room needed
         * for the non-space characters within 'length' and the null
         * terminator, a maximum of 'length + 1'.
         *
         * Returns the number of characters (not including the null terminator) in
         * the destination string.
         ***************************************************************************/
        private int ms_strncpclean(ref string dest, string source, int length)
        {
            int sidx;
            int didx;

            if (dest.Length == 0)
            {
                return 0;
            }

            if (source.Length == 0)
            {
                dest = "\0";
                return 0;
            }

            for (sidx = 0, didx = 0; sidx < length; sidx++)
            {
                if (source.Substring(sidx, 1) == "\0")
                {
                    break;
                }

                if (source.Substring(sidx, 1) != " ")
                {
                    dest.Insert(didx, source.Substring(sidx, 1));
                    didx++;
                }
            }

            dest.Insert(didx, "\0");

            return didx;
        } // End of ms_strncpclean()


        /***************************************************************************
         * ms_strncpcleantail:
         *
         * Copy up to 'length' characters from 'source' to 'dest' without any
         * trailing spaces.  The result is left justified and always null
         * terminated.  The destination string must have enough room needed
         * for the characters within 'length' and the null terminator, a
         * maximum of 'length + 1'.
         *
         * Returns the number of characters (not including the null terminator) in
         * the destination string.
         ***************************************************************************/
        private int ms_strncpcleantail(ref string dest, string source, int length)
        {
            int idx;
            int pretail;

            if (dest.Length == 0)
            {
                return 0;
            }

            if (source.Length == 0)
            {
                dest = "\0";
                return 0;
            }

            dest.Insert(length, "\0");

            pretail = 0;
            for (idx = length - 1; idx >= 0; idx--)
            {
                if (pretail == 0 && source.Substring(idx, 1) == " ")
                {
                    dest.Insert(idx, "\0");
                }
                else
                {
                    pretail++;
                    dest.Insert(idx, source.Substring(idx, 1));
                }
            }

            return pretail;
        } // End of ms_strncpcleantail()



        /***************************************************************************
         * ms_strncpopen:
         *
         * Copy 'length' characters from 'source' to 'dest', padding the right
         * side with spaces and leave open-ended.  The result is left
         * justified and *never* null terminated (the open-ended part).  The
         * destination string must have enough room for 'length' characters.
         *
         * Returns the number of characters copied from the source string.
         ***************************************************************************/
        private int ms_strncpopen(ref string dest, string source, int length)
        {
            int didx;
            int dcnt = 0;
            int term = 0;

            if (dest.Length == 0)
            {
                return 0;
            }

            if (source.Length != 0)
            {
                for (didx = 0; didx < length; didx++)
                {
                    dest.Insert(didx, " ");
                }

                return 0;
            }

            for (didx = 0; didx < length; didx++)
            {
                if (term == 0)
                {
                    if (source.Substring(didx, 1) == "\0")
                    {
                        term = 1;
                    }
                }

                if (term == 0)
                {
                    dest.Insert(didx, source.Substring(didx, 1));
                    dcnt++;
                }
                else
                {
                    dest.Insert(didx, " ");
                }
            }

            return dcnt;
        } // End of ms_strncpopen()


        /***************************************************************************
         * ms_md2doy:
         *
         * Compute the day-of-year from a year, month and day-of-month.
         *
         * Year is expected to be in the range 1800-5000, month is expected to
         * be in the range 1-12, mday is expected to be in the range 1-31 and
         * jday will be in the range 1-366.
         *
         * Returns 0 on success and -1 on error.
         ***************************************************************************/
        //C++ TO C# CONVERTER TODO TASK: Pointer arithmetic is detected on the parameter 'jday', so pointers on this parameter are left unchanged:
        private static int ms_md2doy(int year, int month, int mday, ref int jday)
        {
            int idx;
            int leap;
            int[] days = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

            /* Sanity check for the supplied parameters */
            if (year < 1800 || year > 5000)
            {
                //ms_log(2, "ms_md2doy(): year (%d) is out of range\n", year);
                return -1;
            }
            if (month < 1 || month > 12)
            {
                //ms_log(2, "ms_md2doy(): month (%d) is out of range\n", month);
                return -1;
            }
            if (mday < 1 || mday > 31)
            {
                //ms_log(2, "ms_md2doy(): day-of-month (%d) is out of range\n", mday);
                return -1;
            }

            /* Test for leap year */
            leap = (((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0)) ? 1 : 0;

            /* Add a day to February if leap year */
            if (leap != 0)
            {
                days[1]++;
            }

            /* Check that the day-of-month jives with specified month */
            if (mday > days[month - 1])
            {
                //ms_log(2, "ms_md2doy(): day-of-month (%d) is out of range for month %d\n", mday, month);
                return -1;
            }

            jday = 0;
            month--;

            for (idx = 0; idx < 12; idx++)
            {
                if (idx == month)
                {
                    jday += mday;
                    break;
                }

                jday += days[idx];
            }

            return 0;
        } // End of ms_md2doy()
        public static void test_ms_md2doy()
        {
            int result = 0;
            int jday = 1;
            result = ms_md2doy(2020, 2, 1, ref jday);
            Console.WriteLine("result = " + result + " \n jday = " + jday + "\n");
        }

        /***************************************************************************
        * ms_doy2md:
        *
        * Compute the month and day-of-month from a year and day-of-year.
        *
        * Year is expected to be in the range 1800-5000, jday is expected to
        * be in the range 1-366, month will be in the range 1-12 and mday
        * will be in the range 1-31.
        *
        * Returns 0 on success and -1 on error.
        ***************************************************************************/
        private static int ms_doy2md(int year, int jday, ref int month, ref int mday)
        {
            int idx;
            int leap;
            int[] days = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

            /* Sanity check for the supplied year */
            if (year < 1800 || year > 5000)
            {
                //ms_log(2, "ms_doy2md(): year (%d) is out of range\n", year);
                return -1;
            }

            /* Test for leap year */
            leap = (((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0)) ? 1 : 0;

            /* Add a day to February if leap year */
            if (leap != 0)
            {
                days[1]++;
            }

            if (jday > 365 + leap || jday <= 0)
            {
                //ms_log(2, "ms_doy2md(): day-of-year (%d) is out of range\n", jday);
                return -1;
            }

            for (idx = 0; idx < 12; idx++)
            {
                jday -= days[idx];

                if (jday <= 0)
                {
                    month = idx + 1;
                    mday = days[idx] + jday;
                    break;
                }
            }

            return 0;
        } // End of ms_doy2md()

        public static void test_ms_doy2md()
        {
            int result = 0;
            int month = 1;
            int mday = 1;
            result = ms_doy2md(2020, 35, ref month, ref mday);
            Console.WriteLine("result = " + result + " \n month = " + month + "\n mday= " + mday + "\n");
        }

        /***************************************************************************
         * ms_btime2hptime:
         *
         * Convert a binary SEED time structure to a high precision epoch time
         * (1/HPTMODULUS second ticks from the epoch).  The algorithm used is
         * a specific version of a generalized function in GNU glibc.
         *
         * Returns a high precision epoch time on success and HPTERROR on
         * error.
         ***************************************************************************/
        private static hptime_t ms_btime2hptime(ref BTime btime)
        {
            hptime_t hptime;
            int shortyear;
            int a4, a100, a400;
            int intervening_leap_days;
            int days;

            if (btime == null)
            {
                return (hptime_t)HPTERROR;

            }


            shortyear = btime.year - 1900;
            // a4 = (shortyear >> 2) + 475 - !(shortyear & 3);
            if ((shortyear & 3) == 0)
                a4 = (shortyear >> 2) + 475 - 1;
            else
                a4 = (shortyear >> 2) + 475;

            // a100 = a4 / 25 - (a4 % 25 < 0);
            if (a4 % 25 < 0)
                a100 = a4 / 25 - 1;
            else
                a100 = a4 / 25;


            a400 = a100 >> 2;
            intervening_leap_days = (a4 - 492) - (a100 - 19) + (a400 - 4);

            days = (365 * (shortyear - 70) + intervening_leap_days + (btime.day - 1));


            hptime = (hptime_t)(60 * (60 * ((hptime_t)24 * days + btime.hour) + btime.min) + btime.sec) * HPTMODULUS + (btime.fract * (HPTMODULUS / 10000));

            return hptime;

        } /* End of ms_btime2hptime() */

        public static void test_ms_btime2hptime()
        {
            BTime btime = new BTime();
            btime.day = 1;
            btime.hour = 1;
            btime.fract = 0;
            btime.min = 1;
            btime.sec = 1;
            btime.year = 2019;
            hptime_t hpt;

            hpt = ms_btime2hptime(ref btime);
            Console.WriteLine("result = " + hpt * 1 + "\n");
        }


        /***************************************************************************
         * ms_btime2isotimestr:
         *
         * Build a time string in ISO recommended format from a BTime struct.
         *
         * The provided isostimestr must have enough room for the resulting time
         * string of 25 characters, i.e. '2001-07-29T12:38:00.0000' + NULL.
         *
         * Returns a pointer to the resulting string or NULL on error.
         ***************************************************************************/
        private static string ms_btime2isotimestr(BTime btime, ref string isotimestr)
        {
            int month = 0;
            int mday = 0;
            string ret;

            if (isotimestr.Length == 0)
            {
                return null;
            }

            if (ms_doy2md(btime.year, btime.day, ref month, ref mday) == 1)
            {
                //ms_log(2, "ms_btime2isotimestr(): Error converting year %d day %d\n", btime.year, btime.day);
                return null;
            }


            //ret = snprintf(isotimestr, 25, "%4d-%02d-%02dT%02d:%02d:%02d.%04d", btime.year, month, mday, btime.hour, btime.min, btime.sec, btime.fract);
            ret = btime.year + "-" + string.Format("{00:00}", month) + "-" + string.Format("{00:00}", mday) + "T" + string.Format("{00:00}", btime.hour) + ":" + string.Format("{00:00}", btime.min) + ":" + string.Format("{00:00}", btime.sec) + "." + string.Format("{0000:####}", btime.fract);


            //if (ret != 24)
            if (ret.Length != 24)
            {
                return null;
            }
            else
            {
                isotimestr = ret;
                return isotimestr;
            }
        } // End of ms_btime2isotimestr()

        public static void test_btime2isotimestr()
        {
            BTime btime = new BTime();
            btime.day = 1;
            btime.hour = 1;
            btime.fract = 1212;
            btime.min = 1;
            btime.sec = 1;
            btime.year = 2019;
            string str = "1";

            var result = ms_btime2isotimestr(btime, ref str);
            Console.WriteLine("result = " + str + "\n");
        }

        /***************************************************************************
         * ms_btime2mdtimestr:
         *
         * Build a time string in month-day format from a BTime struct.
         *
         * The provided isostimestr must have enough room for the resulting time
         * string of 25 characters, i.e. '2001-07-29 12:38:00.0000' + NULL.
         *
         * Returns a pointer to the resulting string or NULL on error.
         ***************************************************************************/
        private string ms_btime2mdtimestr(BTime btime, ref string mdtimestr)
        {
            int month = 0;
            int mday = 0;
            string ret;

            if (mdtimestr.Length == 0)
            {
                return null;
            }

            if (ms_doy2md(btime.year, btime.day, ref month, ref mday) == 1)
            {
                //ms_log(2, "ms_btime2mdtimestr(): Error converting year %d day %d\n", btime.year, btime.day);
                return null;
            }

            //ret = snprintf(mdtimestr, 25, "%4d-%02d-%02d %02d:%02d:%02d.%04d", btime.year, month, mday, btime.hour, btime.min, btime.sec, btime.fract);
             ret = btime.year + "-" + string.Format("{00:00}", month) + "-" + string.Format("{00:00}", mday) + " " + string.Format("{00:00}", btime.hour) + ":" + string.Format("{00:00}", btime.min) + ":" + string.Format("{00:00}", btime.sec) + "." + string.Format("{0000:####}", btime.fract);


            //if (ret != 24)
            if (ret.Length != 24)
            {
                return null;
            }
            else
            {
                mdtimestr = ret;
                return mdtimestr;
            }
        } // End of ms_btime2mdtimestr()

        /***************************************************************************
         * ms_btime2seedtimestr:
         *
         * Build a SEED time string from a BTime struct.
         *
         * The provided seedtimestr must have enough room for the resulting time
         * string of 23 characters, i.e. '2001,195,12:38:00.0000' + NULL.
         *
         * Returns a pointer to the resulting string or NULL on error.
         ***************************************************************************/
        private string ms_btime2seedtimestr(BTime btime, ref string seedtimestr)
        {
            string ret;

            if (seedtimestr.Length == 0)
            {
                return null;
            }

            //ret = snprintf(seedtimestr, 23, "%4d,%03d,%02d:%02d:%02d.%04d", btime.year, btime.day, btime.hour, btime.min, btime.sec, btime.fract);
            ret = btime.year + "," + string.Format("{000:000}", btime.day) + "," + string.Format("{00:00}", btime.hour) + "," + string.Format("{00:00}", btime.min) + "," + string.Format("{00:00}", btime.sec) + "." + string.Format("{0000:####}", btime.fract);


            if (ret.Length != 21)
            {
                return null;
            }
            else
            {
                seedtimestr = ret;
                return seedtimestr;
            }
        } // End of ms_btime2seedtimestr()


        /***************************************************************************
         * ms_hptime2tomsusecoffset:
         *
         * Convert a high precision epoch time to a time value in tenths of
         * milliseconds (aka toms) and a microsecond offset (aka usecoffset).
         *
         * The tenths of milliseconds value will be rounded to the nearest
         * value having a microsecond offset value between -50 to +49.
         *
         * Returns 0 on success and -1 on error.
         ***************************************************************************/
        private static int ms_hptime2tomsusecoffset(hptime_t hptime, ref hptime_t? toms, ref sbyte? usecoffset)
        {
            if (toms == null || usecoffset == null)
            {
                return -1;
            }

            /* Split time into tenths of milliseconds and microseconds */
            toms = hptime / (HPTMODULUS / 10000);
            usecoffset = (sbyte)(hptime - (toms * (HPTMODULUS / 10000)));

            /* Round tenths and adjust microsecond offset to -50 to +49 range */
            if (usecoffset > 49 && usecoffset < 100)
            {
                toms += 1;
                usecoffset -= 100;
            }
            else if (usecoffset < -50 && usecoffset > -100)
            {
                toms -= 1;
                usecoffset += 100;
            }

            /* Convert tenths of milliseconds to be in hptime_t (HPTMODULUS) units */
            toms *= (HPTMODULUS / 10000);

            return 0;
        } // End of ms_hptime2tomsusecoffset()

        public static void test_ms_hptime2tomsusecoffset()
        {
            hptime_t ht1 = 1;
            hptime_t? toms = 1;
            sbyte? usecoffset = 1;
            var result = ms_hptime2tomsusecoffset(ht1, ref toms, ref usecoffset);


        }




        /***************************************************************************
         * ms_hptime2btime:
         *
         * Convert a high precision epoch time to a SEED binary time
         * structure.  The microseconds beyond the 1/10000 second range are
         * truncated and *not* rounded, this is intentional and necessary.
         *
         * Returns 0 on success and -1 on error.
         ***************************************************************************/
        private static int ms_hptime2btime(hptime_t hptime, ref BTime btime)
        {
            tm tms = new tm();
            long isec;
            int ifract;
            int bfract;

            if (btime == null)
            {
                return -1;
            }

            /* Reduce to Unix/POSIX epoch time and fractional seconds */
            isec = MS_HPTIME2EPOCH(hptime);
            ifract = (int)(hptime - (isec * HPTMODULUS));

            /* BTime only has 1/10000 second precision */
            // bfract = ifract / (HPTMODULUS / 10000);
            bfract = (int)(ifract / (HPTMODULUS / 10000));

            /* Adjust for negative epoch times, round back when needed */
            if (hptime < 0 && ifract != 0)
            {
                /* Isolate microseconds between 1e-4 and 1e-6 precision and adjust bfract if not zero */
                if ((ifract - bfract * (HPTMODULUS / 10000)) == 1)
                {
                    bfract -= 1;
                }

                isec -= 1;
                bfract = 10000 - (-bfract);
            }

            if (ms_gmtime_r(ref isec, ref tms) == null)
            {
                return -1;
            }

            btime.year = (ushort)(tms.tm_year + 1900);
            btime.day = (ushort)(tms.tm_yday + 1);
            btime.hour = (ushort)tms.tm_hour;
            btime.min = (ushort)tms.tm_min;
            btime.sec = (ushort)tms.tm_sec;
            btime.unused = 0;
            btime.fract = (ushort)bfract;

            return 0;
        } // End of ms_hptime2btime()


        public static void test_ms_hptime2btime()
        {
            BTime btime = new BTime();
            btime.day = 10;
            btime.hour = 10;
            btime.fract = 12;
            btime.min = 23;
            btime.sec = 12;
            btime.year = 2020;
            hptime_t hpt = 124555454555454453;
            var tt = ms_hptime2btime(hpt, ref btime);
        }



        /***************************************************************************
         * ms_hptime2isotimestr:
         *
         * Build a time string in ISO recommended format from a high precision
         * epoch time.
         *
         * The provided isostimestr must have enough room for the resulting time
         * string of 27 characters, i.e. '2001-07-29T12:38:00.000000' + NULL.
         *
         * The 'subseconds' flag controls whenther the sub second portion of the
         * time is included or not.
         *
         * Returns a pointer to the resulting string or NULL on error.
         ***************************************************************************/
        private string ms_hptime2isotimestr(hptime_t hptime, ref string isotimestr, bool? subseconds=null)
        {
            tm tms = new tm();
            long isec;
            int ifract;
            string ret;

            if (isotimestr == null)
            {
                return null;
            }

            /* Reduce to Unix/POSIX epoch time and fractional seconds */
            isec = MS_HPTIME2EPOCH(hptime);
            ifract = (int)(hptime - (isec * HPTMODULUS));

            /* Adjust for negative epoch times */
            if (hptime < 0 && ifract != 0)
            {
                isec -= 1;
                ifract = (int)(HPTMODULUS - (-ifract));
            }

            if ((ms_gmtime_r(ref isec, ref tms)) == null)
            {
                return null;
            }

            if (subseconds != null)
            {
                /* Assuming ifract has at least microsecond precision */
                // ret = snprintf(isotimestr, 27, "%4d-%02d-%02dT%02d:%02d:%02d.%06d", tms.tm_year + 1900, tms.tm_mon + 1, tms.tm_mday, tms.tm_hour, tms.tm_min, tms.tm_sec, ifract);
                ret = tms.tm_year + 1900 + "-" + string.Format("{00:00}", tms.tm_mon + 1) + "-" + string.Format("{00:00}", tms.tm_mday) + "T" + string.Format("{00:00}", tms.tm_hour) + ":" + string.Format("{00:00}", tms.tm_min) + ":" + string.Format("{00:00}", tms.tm_sec) + "." + string.Format("{000000:######}", ifract);

            }
            else
            {
                //ret = snprintf(isotimestr, 20, "%4d-%02d-%02dT%02d:%02d:%02d", tms.tm_year + 1900, tms.tm_mon + 1, tms.tm_mday, tms.tm_hour, tms.tm_min, tms.tm_sec);
                ret = tms.tm_year + 1900 + "-" + string.Format("{00:00}", tms.tm_mon + 1) + "-" + string.Format("{00:00}", tms.tm_mday) + "T" + string.Format("{00:00}", tms.tm_hour) + ":" + string.Format("{00:00}", tms.tm_min) + ":" + string.Format("{00:00}", tms.tm_sec);

            }

            if (ret.Length != 25 && ret.Length != 18)
            {
                return null;
            }
            else
            {
                isotimestr = ret;
                return isotimestr;
            }
        } // End of ms_hptime2isotimestr()


        /***************************************************************************
         * ms_hptime2seedtimestr:
         *
         * Build a SEED time string from a high precision epoch time.
         *
         * The provided seedtimestr must have enough room for the resulting time
         * string of 25 characters, i.e. '2001,195,12:38:00.000000\n'.
         *
         * The 'subseconds' flag controls whenther the sub second portion of the
         * time is included or not.
         *
         * Returns a pointer to the resulting string or NULL on error.
         ***************************************************************************/
        private string ms_hptime2seedtimestr(hptime_t hptime, ref string seedtimestr, bool? subseconds=null)
        {
            tm tms = new tm();
            long isec;
            int ifract;
            string ret;

            if (seedtimestr == null)
            {
                return null;
            }

            /* Reduce to Unix/POSIX epoch time and fractional seconds */
            isec = MS_HPTIME2EPOCH(hptime);
            ifract = (int)(hptime - (isec * HPTMODULUS));

            /* Adjust for negative epoch times */
            if (hptime < 0 && ifract != 0)
            {
                isec -= 1;
                ifract = (int)(HPTMODULUS - (-ifract));
            }

            if ((ms_gmtime_r(ref isec, ref tms)) == null)
            {
                return null;
            }

            if (subseconds != null)
            {
                /* Assuming ifract has at least microsecond precision */
                //ret = snprintf(seedtimestr, 25, "%4d,%03d,%02d:%02d:%02d.%06d", tms.tm_year + 1900, tms.tm_yday + 1, tms.tm_hour, tms.tm_min, tms.tm_sec, ifract);
                ret = tms.tm_year + 1900 + "," + string.Format("{000:000}", tms.tm_yday + 1) + "," + string.Format("{00:00}", tms.tm_hour) + "," + string.Format("{00:00}", tms.tm_min) + "," + string.Format("{00:00}", tms.tm_sec) + "." + string.Format("{000000:######}", ifract);

            }
            else
            {
                //ret = snprintf(seedtimestr, 18, "%4d,%03d,%02d:%02d:%02d", tms.tm_year + 1900, tms.tm_yday + 1, tms.tm_hour, tms.tm_min, tms.tm_sec);
                ret = tms.tm_year + 1900 + "," + string.Format("{000:000}", tms.tm_yday + 1) + "," + string.Format("{00:00}", tms.tm_hour) + "," + string.Format("{00:00}", tms.tm_min) + "," + string.Format("{00:00}", tms.tm_sec);
            }

            if (ret.Length != 23 && ret.Length != 16)
            {
                return null;
            }
            else
            {
                return seedtimestr;
            }
        } // End of ms_hptime2seedtimestr()

        /***************************************************************************
         * ms_time2hptime_int:
         *
         * Convert specified time values to a high precision epoch time.  This
         * is an internal version which does no range checking, it is assumed
         * that checking the range for each value has already been done.
         *
         * Returns epoch time on success and HPTERROR on error.
         ***************************************************************************/
        private static hptime_t ms_time2hptime_int(int year, int day, int hour, int min, int sec, int usec)
        {
            BTime btime = new BTime();
            hptime_t hptime = new hptime_t();

            //memset(btime, 0, sizeof(BTime));
            btime.day = 1;

            /* Convert integer seconds using ms_btime2hptime */
            btime.year = (ushort)year;
            btime.day = (ushort)day;
            btime.hour = (byte)hour;
            btime.min = (byte)min;
            btime.sec = (byte)sec;
            btime.fract = 0;

            hptime = ms_btime2hptime(ref btime);

            if (hptime == HPTERROR)
            {
                //ms_log(2, "ms_time2hptime(): Error converting with ms_btime2hptime()\n");
                return HPTERROR;
            }

            /* Add the microseconds */
            hptime += (hptime_t)usec * (1000000 / HPTMODULUS);

            return hptime;
        } // End of ms_time2hptime_int()

        public static void test_ms_time2hptime_int()
        {
            var hpt = ms_time2hptime_int(2020, 1, 1, 1, 1, 1);
        }

        /***************************************************************************
         * ms_hptime2mdtimestr:
         *
         * Build a time string in month-day format from a high precision
         * epoch time.
         *
         * The provided mdtimestr must have enough room for the resulting time
         * string of 27 characters, i.e. '2001-07-29 12:38:00.000000' + NULL.
         *
         * The 'subseconds' flag controls whenther the sub second portion of the
         * time is included or not.
         *
         * Returns a pointer to the resulting string or NULL on error.
         ***************************************************************************/
        private string ms_hptime2mdtimestr(hptime_t hptime, ref string mdtimestr, bool? subseconds=null)
        {
            tm tms = new tm();
            long isec;
            int ifract;
            string ret;

            if (mdtimestr == null)
            {
                return null;
            }

            /* Reduce to Unix/POSIX epoch time and fractional seconds */
            isec = MS_HPTIME2EPOCH(hptime);
            ifract = (int)(hptime - (isec * HPTMODULUS));

            /* Adjust for negative epoch times */
            if (hptime < 0 && ifract != 0)
            {
                isec -= 1;
                ifract = (int)(HPTMODULUS - (-ifract));
            }

            if ((ms_gmtime_r(ref isec, ref tms)) == null)
            {
                return null;
            }

            if (subseconds != null)
            {
                /* Assuming ifract has at least microsecond precision */
                //ret = snprintf(mdtimestr, 27, "%4d-%02d-%02d %02d:%02d:%02d.%06d", tms.tm_year + 1900, tms.tm_mon + 1, tms.tm_mday, tms.tm_hour, tms.tm_min, tms.tm_sec, ifract);
                ret = tms.tm_year + 1900 + "-" + string.Format("{00:00}", tms.tm_mon + 1) + "-" + string.Format("{00:00}", tms.tm_mday) + " " + string.Format("{00:00}", tms.tm_hour) + ":" + string.Format("{00:00}", tms.tm_min) + ":" + string.Format("{00:00}", tms.tm_sec) + "." + string.Format("{000000:######}", ifract);

            }
            else
            {
                //ret = snprintf(mdtimestr, 20, "%4d-%02d-%02d %02d:%02d:%02d", tms.tm_year + 1900, tms.tm_mon + 1, tms.tm_mday, tms.tm_hour, tms.tm_min, tms.tm_sec);
                ret = tms.tm_year + 1900 + "-" + string.Format("{00:00}", tms.tm_mon + 1) + "-" + string.Format("{00:00}", tms.tm_mday) + " " + string.Format("{00:00}", tms.tm_hour) + ":" + string.Format("{00:00}", tms.tm_min) + ":" + string.Format("{00:00}", tms.tm_sec);

            }

            if (ret.Length != 25 && ret.Length != 18)
            {
                return null;
            }
            else
            {
                return mdtimestr;
            }
        } // End of ms_hptime2mdtimestr()


        /***************************************************************************
         * ms_timestr2hptime:
         *
         * Convert a generic time string to a high precision epoch time.  The
         * time format expected is "YYYY[/MM/DD HH:MM:SS.FFFF]", the delimiter
         * can be a dash [-], comma[,], slash [/], colon [:], or period [.].
         * Additionally a 'T' or space may be used between the date and time
         * fields.  The fractional seconds ("FFFFFF") must begin with a period
         * [.] if present.
         *
         * The time string can be "short" in which case the omitted values are
         * assumed to be zero (with the exception of month and day which are
         * assumed to be 1): "YYYY/MM/DD" assumes HH, MM, SS and FFFF are 0.
         * The year is required, otherwise there wouldn't be much for a date.
         *
         * Ranges are checked for each value.
         *
         * Returns epoch time on success and HPTERROR on error.
         ***************************************************************************/
        private static hptime_t ms_timestr2hptime(ref string timestr)
        {
            string[] fields;
            int year = 0;
            int mon = 1;
            int mday = 1;
            int day = 1;
            int hour = 0;
            int min = 0;
            int sec = 0;
            float fusec = 0.0F;
            int usec = 0;
            hptime_t hptime_T = new hptime_t();
            //fields = sscanf(timestr, "%d%*[-,/:.]%d%*[-,/:.]%d%*[-,/:.Tt ]%d%*[-,/:.]%d%*[-,/:.]%d%f", year, mon, mday, hour, min, sec, fusec);
            fields = timestr.Split('/', '-', ' ', ':', 't', '.');


            if (!(fields.Length <= 7 && fields.Length >= 6))
            {
                //ms_log(2, "ms_timestr2hptime(): Error converting time string: %s\n", timestr);
                return HPTERROR;
            }
            year = Convert.ToInt32(fields[0]);
            mon = Convert.ToInt32(fields[1]);
            day = Convert.ToInt32(fields[2]);
            hour = Convert.ToInt32(fields[3]);
            min = Convert.ToInt32(fields[4]);
            sec = Convert.ToInt32(fields[5]);
            if (fields.Length == 7)
                fusec = Convert.ToInt32(fields[6]);



            /* Convert fractional seconds to microseconds */
            if (fusec != 0.0F)
            {
                usec = (int)(fusec * 1000000.0 + 0.5);
            }



            if (year < 1800 || year > 5000)
            {
                //ms_log(2, "ms_timestr2hptime(): Error with year value: %d\n", year);
                return HPTERROR;
            }

            if (mon < 1 || mon > 12)
            {
                //ms_log(2, "ms_timestr2hptime(): Error with month value: %d\n", mon);
                return HPTERROR;
            }

            if (mday < 1 || mday > 31)
            {
                //ms_log(2, "ms_timestr2hptime(): Error with day value: %d\n", mday);
                return HPTERROR;
            }

            /* Convert month and day-of-month to day-of-year */
            /*
            if (ms_md2doy(year, mon, mday, day))
            {
                return HPTERROR;
            }
            */

            if (hour < 0 || hour > 23)
            {
                //ms_log(2, "ms_timestr2hptime(): Error with hour value: %d\n", hour);
                return HPTERROR;
            }

            if (min < 0 || min > 59)
            {
                //ms_log(2, "ms_timestr2hptime(): Error with minute value: %d\n", min);
                return HPTERROR;
            }

            if (sec < 0 || sec > 60)
            {
                //ms_log(2, "ms_timestr2hptime(): Error with second value: %d\n", sec);
                return HPTERROR;
            }

            if (usec < 0 || usec > 999999)
            {
                //ms_log(2, "ms_timestr2hptime(): Error with fractional second value: %d\n", usec);
                return HPTERROR;
            }

            //return ms_time2hptime_int(year, day, hour, min, sec, usec);
            DateTimeOffset dtof = new DateTimeOffset(year, mon, day, hour, min, sec, usec, TimeSpan.Zero);
            hptime_T = dtof.ToUnixTimeMilliseconds();
            return hptime_T;
        } // End of ms_timestr2hptime()

        public static void test_ms_timestr2hptime()
        {
            string time = "2020-01-01 01:2:30.0";
            hptime_t tt = ms_timestr2hptime(ref time);
            Console.WriteLine("time = " + tt);
        }
        /***************************************************************************
         * ms_nomsamprate:
         *
         * Calculate a sample rate from SEED sample rate factor and multiplier
         * as stored in the fixed section header of data records.
         *
         * Returns the positive sample rate.
         ***************************************************************************/
        private static double ms_nomsamprate(int factor, int multiplier)
        {
            double samprate = 0.0;

            if (factor > 0)
            {
                samprate = (double)factor;
            }
            else if (factor < 0)
            {
                samprate = -1.0 / (double)factor;
            }
            if (multiplier > 0)
            {
                samprate = samprate * (double)multiplier;
            }
            else if (multiplier < 0)
            {
                samprate = -1.0 * (samprate / (double)multiplier);
            }

            return samprate;
        } // End of ms_nomsamprate()


        /***************************************************************************
         * ms_reduce_rate:
         *
         * Reduce the specified sample rate into two "factors" (in some cases
         * the second factor is actually a divisor).
         *
         * Integer rates between 1 and 32767 can be represented exactly.
         *
         * Integer rates higher than 32767 will be matched as closely as
         * possible with the deviation becoming larger as the integers reach
         * (32767 * 32767).
         *
         * Non-integer rates between 32767.0 and 1.0/32767.0 are represented
         * exactly when possible and approximated otherwise.
         *
         * Non-integer rates greater than 32767 or less than 1/32767 are not supported.
         *
         * Returns 0 on success and -1 on error.
         ***************************************************************************/
        //C++ TO C# CONVERTER TODO TASK: Pointer arithmetic is detected on the parameter 'factor1', so pointers on this parameter are left unchanged:
        private static int ms_reduce_rate(double samprate, ref short? factor1, ref short? factor2)
        {
            int num;
            int den;
            int intsamprate = (int)(samprate + 0.5);

            int searchfactor1;
            int searchfactor2;
            int closestfactor;
            int closestdiff;
            int diff;

            /* Handle case of integer sample values. */
            if (ms_dabs(samprate - intsamprate) < 0.0000001)
            {
                /* If integer sample rate is less than range of 16-bit int set it directly */
                if (intsamprate <= 32767)
                {
                    factor1 = (short)intsamprate;
                    factor2 = 1;
                    return 0;
                }
                /* If integer sample rate is within the maximum possible nominal rate */
                else if (intsamprate <= (32767 * 32767))
                {
                    /* Determine the closest factors that represent the sample rate.
                     * The approximation gets worse as the values increase. */
                    searchfactor1 = (int)(1.0 / ms_rsqrt64(samprate));
                    closestdiff = searchfactor1;
                    closestfactor = searchfactor1;

                    while ((intsamprate % searchfactor1) != 0)
                    {
                        searchfactor1 -= 1;

                        /* Track the factor that generates the closest match */
                        searchfactor2 = intsamprate / searchfactor1;
                        diff = intsamprate - (searchfactor1 * searchfactor2);
                        if (diff < closestdiff)
                        {
                            closestdiff = diff;
                            closestfactor = searchfactor1;
                        }

                        /* If the next iteration would create a factor beyond the limit
                         * we accept the closest factor */
                        if ((intsamprate / (searchfactor1 - 1)) > 32767)
                        {
                            searchfactor1 = closestfactor;
                            break;
                        }
                    }

                    searchfactor2 = intsamprate / searchfactor1;

                    if (searchfactor1 <= 32767 && searchfactor2 <= 32767)
                    {
                        factor1 = (short)searchfactor1;
                        factor2 = (short)searchfactor2;
                        return 0;
                    }
                }
            }
            /* Handle case of non-integer less than 16-bit int range */
            else if (samprate <= 32767.0)
            {
                num = 0; den = 0;
                /* For samples/seconds, determine, potentially approximate, numerator and denomiator */
                ms_ratapprox(samprate, ref num, ref den, 32767, 1e-8);

                /* Negate the factor2 to denote a division operation */
                factor1 = (short)num;
                factor2 = (short)-den;
                return 0;
            }

            return -1;
        } // End of ms_reduce_rate()
        public static void test_reduce_rate()
        {
            //  int ms_reduce_rate(double samprate, ref short? factor1, ref short? factor2)
            short? factor1 = 30;
            short? factor2 = 2;

            var dd = ms_reduce_rate(32768, ref factor1, ref factor2);
            Console.WriteLine("ms_reduce_rate = " + dd + "\n");
            Console.WriteLine("factor1 = " + factor1 + "\n");
            Console.WriteLine("factor2 = " + factor2 + "\n");


        }


        /***************************************************************************
         * ms_genfactmult:
         *
         * Generate an appropriate SEED sample rate factor and multiplier from
         * a double precision sample rate.
         *
         * If the samplerate > 0.0 it is expected to be a rate in SAMPLES/SECOND.
         * If the samplerate < 0.0 it is expected to be a period in SECONDS/SAMPLE.
         *
         * Results use SAMPLES/SECOND notation when sample rate >= 1.0
         * Results use SECONDS/SAMPLE notation when samles rates < 1.0
         *
         * Returns 0 on success and -1 on error or calculation not possible.
         ***************************************************************************/
        private static int ms_genfactmult(double samprate, ref short? factor, ref short? multiplier)
        {
            short? factor1 = 0;
            short? factor2 = 0;

            if (factor == 0 || multiplier == 0)
            {
                return -1;
            }

            /* Convert sample period to sample rate */
            if (samprate < 0.0)
            {
                samprate = -1.0 / samprate;
            }

            /* Handle special case of zero */
            if (samprate == 0.0)
            {
                factor = null;
                multiplier = null;
                return 0;
            }
            /* Handle sample rates >= 1.0 with the SAMPLES/SECOND representation */
            else if (samprate >= 1.0)
            {
                if (ms_reduce_rate(samprate, ref factor1, ref factor2) == 0)
                {
                    factor = factor1;
                    multiplier = factor2;
                    return 0;
                }
            }
            /* Handle sample rates < 1 with the SECONDS/SAMPLE representation */
            else
            {
                /* Reduce rate as a sample period and invert factor/multiplier */
                if (ms_reduce_rate(1.0 / samprate, ref factor1, ref factor2) == 0)
                {
                    factor = (short)(-factor1);
                    multiplier = (short)(-factor2);
                    return 0;
                }
            }

            return -1;
        } // End of ms_genfactmult()

        public static void test_ms_genfactmult()
        {
            // ms_genfactmult(double samprate, ref short ? factor, ref short ? multiplier)
            short? factor = 1;
            short? multiplier = 2;

            //int ms_genfactmult (double samprate, int16_t *factor, int16_t *multiplier)
            var dd = ms_genfactmult(32768.0, ref factor, ref multiplier);
            Console.WriteLine("ms_genfactmult = " + dd + "\n");
            Console.WriteLine("factor = " + factor + "\n");
            Console.WriteLine("multiplier = " + multiplier + "\n");
        }

        /***************************************************************************
         * ms_ratapprox:
         *
         * Find an approximate rational number for a real through continued
         * fraction expansion.  Given a double precsion 'real' find a
         * numerator (num) and denominator (den) whose absolute values are not
         * larger than 'maxval' while trying to reach a specified 'precision'.
         *
         * Returns the number of iterations performed.
         ***************************************************************************/
        private static int ms_ratapprox(double real, ref int num, ref int den, int maxval, double precision)
        {
            double realj;
            double preal;
            bool pos;
            int pnum;
            int pden;
            int iterations = 1;
            int Aj1;
            int Aj2;
            int Bj1;
            int Bj2;
            int bj = 0;
            int Aj = 0;
            int Bj = 1;

            if (real >= 0.0)
            {
                pos = true;
                realj = real;
            }
            else
            {
                pos = false;
                realj = -real;
            }

            preal = realj;

            bj = (int)(realj + precision);
            realj = 1 / (realj - bj);
            Aj = bj;
            Aj1 = 1;
            Bj = 1;
            Bj1 = 0;
            num = pnum = Aj;
            den = pden = Bj;
            if (!pos)
            {
                num = -num;
            }

            while (ms_dabs(preal - (double)Aj / (double)Bj) > precision && Aj < maxval && Bj < maxval)
            {
                Aj2 = Aj1;
                Aj1 = Aj;
                Bj2 = Bj1;
                Bj1 = Bj;
                bj = (int)(realj + precision);
                realj = 1 / (realj - bj);
                Aj = bj * Aj1 + Aj2;
                Bj = bj * Bj1 + Bj2;
                num = pnum;
                den = pden;
                if (!pos)
                {
                    num = -num;
                }
                pnum = Aj;
                pden = Bj;

                iterations++;
            }

            if (pnum < maxval && pden < maxval)
            {
                num = pnum;
                den = pden;
                if (!pos)
                {
                    num = -num;
                }
            }

            return iterations;
        }

        public static void test_ms_ratapprox()
        {
            int num = 10;
            int den = 5;

            //(double real, int *num, int *den, int maxval, double precision)
            var dd = ms_ratapprox(-0.225656565625, ref num, ref den, 300, 0.000001152342);
            Console.WriteLine("ratapprox = " + dd + "\n");
            Console.WriteLine("den = " + den + "\n");
            Console.WriteLine("num = " + num + "\n");
        }

        /***************************************************************************
         * ms_bigendianhost:
         *
         * Determine the byte order of the host machine.  Due to the lack of
         * portable defines to determine host byte order this run-time test is
         * provided.  The code below actually tests for little-endianess, the
         * only other alternative is assumed to be big endian.
         *
         * Returns 0 if the host is little endian, otherwise 1.
         ***************************************************************************/
        private int ms_bigendianhost()
        {
            if (BitConverter.IsLittleEndian)
            {
                return 0;
            }
            else
                return 1;

        } // End of ms_bigendianhost()


        /***************************************************************************
         * ms_dabs:
         *
         * Determine the absolute value of an input double, actually just test
         * if the input double is positive multiplying by -1.0 if not and
         * return it.
         *
         * Returns the positive value of input double.
        ***************************************************************************/
        private static double ms_dabs(double val)
        {
            if (val < 0.0)
            {
                val *= -1.0;
            }
            return val;
        } // End of ms_dabs()


        /***************************************************************************
         * ms_rsqrt64:
         *
         * An optimized reciprocal square root calculation from:
         *   Matthew Robertson (2012). "A Brief History of InvSqrt"
         *   https://cs.uwaterloo.ca/~m32rober/rsqrt.pdf
         *
         * Further reference and description:
         *   https://en.wikipedia.org/wiki/Fast_inverse_square_root
         *
         * Modifications:
         * Add 2 more iterations of Newton's method to increase accuracy,
         * specifically for large values.
         * Use memcpy instead of assignment through differing pointer types.
         *
         * Returns 0 if the host is little endian, otherwise 1.
         ***************************************************************************/

        public static double ms_rsqrt64(double val)
        {
            ulong i;
            double x2;
            double y;

            x2 = val * 0.5;
            y = val;
            //memcpy(i, y, sizeof(ulong));
            var y_bytes = BitConverter.GetBytes(y);
            i = BitConverter.ToUInt64(y_bytes);


            i = 0x5fe6eb50c7b537a9UL - (i >> 1);
            //memcpy(y, i, sizeof(double));
            var i_bytes = BitConverter.GetBytes(i);
            y = BitConverter.ToDouble(i_bytes);

            y = y * (1.5 - (x2 * y * y));
            y = y * (1.5 - (x2 * y * y));
            y = y * (1.5 - (x2 * y * y));

            return y;
        } // End of ms_rsqrt64()


        public static void test_ms_rsqrt64()
        {
            double val = 12;  //output = 0.288675
            Console.WriteLine("rsqrt64 = " + ms_rsqrt64(val) + "\n");

        }


        /***************************************************************************
         * ms_gmtime_r:
         *
         * An internal version of gmtime_r() that is 64-bit compliant and
         * works with years beyond 2038.
         *
         * The original was called pivotal_gmtime_r() by Paul Sheer, all
         * required copyright and other hoohas are below.  Modifications were
         * made to integrate the original to this code base, avoid name
         * collisions and formatting so I could read it.
         *
         * Returns a pointer to the populated tm struct on success and NULL on error.
         ***************************************************************************/

        /* pivotal_gmtime_r - a replacement for gmtime/localtime/mktime
                              that works around the 2038 bug on 32-bit
                              systems. (Version 4)

           Copyright (C) 2009  Paul Sheer

           Redistribution and use in source form, with or without modification,
           is permitted provided that the above copyright notice, this list of
           conditions, the following disclaimer, and the following char array
           are retained.

           Redistribution and use in binary form must reproduce an
           acknowledgment: 'With software provided by http://2038bug.com/' in
           the documentation and/or other materials provided with the
           distribution, and wherever such acknowledgments are usually
           accessible in Your program.

           This software is provided "AS IS" and WITHOUT WARRANTY, either
           express or implied, including, without limitation, the warranties of
           NON-INFRINGEMENT, MERCHANTABILITY or FITNESS FOR A PARTICULAR
           PURPOSE. THE ENTIRE RISK AS TO THE QUALITY OF THIS SOFTWARE IS WITH
           YOU. Under no circumstances and under no legal theory, whether in
           tort (including negligence), contract, or otherwise, shall the
           copyright owners be liable for any direct, indirect, special,
           incidental, or consequential damages of any character arising as a
           result of the use of this software including, without limitation,
           damages for loss of goodwill, work stoppage, computer failure or
           malfunction, or any and all other commercial damages or losses. This
           limitation of liability shall not apply to liability for death or
           personal injury resulting from copyright owners' negligence to the
           extent applicable law prohibits such limitation. Some jurisdictions
           do not allow the exclusion or limitation of incidental or
           consequential damages, so this exclusion and limitation may not apply
           to You.

        */

        private const string pivotal_gmtime_r_stamp_lm = "pivotal_gmtime_r. Copyright (C) 2009  Paul Sheer. Terms and " + "conditions apply. Visit http://2038bug.com/ for more info.";

        private static int[][] tm_days =
        {
            new int[] {31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31, 0},
            new int[] {31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31, 0},
            new int[] {0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334, 365},
            new int[] {0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335, 366}
        };


        private static tm? ms_gmtime_r(ref long timep, ref tm result)
        {
            int v_tm_sec;
            int v_tm_min;
            int v_tm_hour;
            int v_tm_mon;
            int v_tm_wday;
            int v_tm_tday;
            int leap;
            long m;
            long tv;

            if (timep == 0 || result.isNull())
            {
                //return null;
            }

            tv = timep;

            v_tm_sec = (int)((long)tv % (long)60);
            tv /= 60;
            v_tm_min = (int)((long)tv % (long)60);
            tv /= 60;
            v_tm_hour = (int)((long)tv % (long)24);
            tv /= 24;
            v_tm_tday = (int)tv;


            //TM_WRAP(a, b, m)((a) = ((a) < 0) ? ((b)--, (a) + (m)) : (a))


            TM_WRAP(ref v_tm_sec, ref v_tm_min, 60);
            TM_WRAP(ref v_tm_min, ref v_tm_hour, 60);
            TM_WRAP(ref v_tm_hour, ref v_tm_tday, 24);





            if ((v_tm_wday = (v_tm_tday + 4) % 7) < 0)
            {
                v_tm_wday += 7;
            }

            m = (int)v_tm_tday;

            if (m >= 0)
            {
                result.tm_year = 70;

                leap = TM_LEAP_CHECK(result.tm_year);

                while (m >= (long)tm_days[leap + 2][12])
                {
                    m -= (long)tm_days[leap + 2][12];
                    result.tm_year++;
                    leap = TM_LEAP_CHECK(result.tm_year);
                }

                v_tm_mon = 0;

                while (m >= (long)tm_days[leap][v_tm_mon])
                {
                    m -= (long)tm_days[leap][v_tm_mon];
                    v_tm_mon++;
                }
            }
            else
            {
                result.tm_year = 69;
                leap = TM_LEAP_CHECK(result.tm_year);

                while (m < (long)-tm_days[leap + 2][12])
                {
                    m += (long)tm_days[leap + 2][12];
                    result.tm_year--;
                    leap = TM_LEAP_CHECK(result.tm_year);
                }

                v_tm_mon = 11;

                while (m < (long)-tm_days[leap][v_tm_mon])
                {
                    m += (long)tm_days[leap][v_tm_mon];
                    v_tm_mon--;
                }

                m += (long)tm_days[leap][v_tm_mon];
            }




            result.tm_mday = (int)m + 1;
            result.tm_yday = (int)(tm_days[leap + 2][v_tm_mon] + m);
            result.tm_sec = v_tm_sec;
            result.tm_min = v_tm_min;
            result.tm_hour = v_tm_hour;
            result.tm_mon = v_tm_mon;
            result.tm_wday = v_tm_wday;

            return result;
        } // End of ms_gmtime_r()

        private static void TM_WRAP(ref int a, ref int b, int m)
        {
            //((a) = ((a) < 0) ? ((b)--, (a) + (m)) : (a))}
            if (a >= 0)
                return;
            else
            {
                a = a + m;
                b = b - 1;
            }

        }
        private static int TM_LEAP_CHECK(int n)
        {
            // TM_LEAP_CHECK(n)((!(((n) + 1900) % 400) || (!(((n) + 1900) % 4) && (((n) + 1900) % 100))) != 0)
            if ((n + 1900) % 400 == 0)
                return 1;
            if (((n + 1900) % 4 == 0) && ((n + 1900) % 100 != 0))
                return 1;
            return 0;
        }

    }





    //Helper class added by C++ to C# Converter:

    //----------------------------------------------------------------------------------------
    //	Copyright © 2006 - 2020 Tangible Software Solutions, Inc.
    //	This class can be used by anyone provided that the copyright notice remains intact.
    //
    //	This class provides the ability to replicate various classic C string functions
    //	which don't have exact equivalents in the .NET Framework.
    //----------------------------------------------------------------------------------------


}