using System;
using System.Collections.Generic;
using System.Text;
using static libmseedNetCore.Constants;


namespace libmseedNetCore
{
    public class msrutils
    {


        /***************************************************************************
         * msr_init:
         *
         * Initialize and return an MSRecord struct, allocating memory if
         * needed.  If memory for the fsdh and datasamples fields has been
         * allocated the pointers will be retained for reuse.  If a blockette
         * chain is present all associated memory will be released.
         *
         * Returns a pointer to a MSRecord struct on success or NULL on error.
         ***************************************************************************/
        /*        private MSRecord msr_init(MSRecord msr)
                {
                    object fsdh = null;
                    object datasamples = null;

                    if (msr == null)
                    {
                        msr = new MSRecord();
                    }
                    else
                    {
                        fsdh = msr.fsdh;
                        datasamples = msr.datasamples;

                        if (msr.blkts)
                        {
                            msr_free_blktchain(msr);
                        }

                        if (msr.ststate)
                        {
                            msr.ststate = null;
                        }
                    }

                    if (msr == null)
                    {
                        ms_log(2, "msr_init(): Cannot allocate memory\n");
                        return null;
                    }

                    //C++ TO C# CONVERTER TODO TASK: The memory management function 'memset' has no equivalent in C#:
                    memset(msr, 0, sizeof(MSRecord));

                    msr.fsdh = fsdh;
                    msr.datasamples = datasamples;

                    msr.reclen = -1;
                    msr.samplecnt = -1;
                    msr.byteorder = -1;
                    msr.encoding = -1;

                    return msr;
                } // End of msr_init()*/

        /***************************************************************************
         * msr_free:
         *
         * Free all memory associated with a MSRecord struct.
         ***************************************************************************/
        //    private void msr_free(MSRecord[] ppmsr)
        //     {
        //         if (ppmsr != null && ppmsr[0] != 0)
        //         {
        //             /* Free fixed section header if populated */
        //           if (ppmsr.fsdh)
        //             {
        //                 ppmsr.fsdh = null;
        //             }

        //             /* Free blockette chain if populated */
        //             if (ppmsr.blkts)
        //             {
        //                 msr_free_blktchainppmsr;
        //             }

        //             /* Free datasamples if present */
        //             if (ppmsr.datasamples)
        //             {
        //                 ppmsr.datasamples = null;
        //             }

        //             /* Free stream processing state if present */
        //             if (ppmsr.ststate)
        //             {
        //                 ppmsr.ststate = null;
        //             }

        //             freeppmsr;

        //             ppmsr[0] = null;
        //         }
        //     } // End of msr_free()


        //     /***************************************************************************
        //      * msr_free_blktchain:
        //      *
        //      * Free all memory associated with a blockette chain in a MSRecord
        //      * struct and set MSRecord->blkts to NULL.  Also reset the shortcut
        //      * blockette pointers.
        //      ***************************************************************************/
        //     private void msr_free_blktchain(MSRecord msr)
        //     {
        //         if (msr != null)
        //         {
        //             if (msr.blkts)
        //             {
        //                 BlktLink bc = msr.blkts;
        //                 BlktLink nb = null;

        //                 while (bc != null)
        //                 {
        //                     nb = bc.next;

        //                     if (bc.blktdata)
        //                     {
        //                         bc.blktdata = null;
        //                     }

        //                     bc = null;

        //                     bc = nb;
        //                 }

        //                 msr.blkts = 0;
        //             }

        //             msr.Blkt100 = 0;
        //             msr.Blkt1000 = 0;
        //             msr.Blkt1001 = 0;
        //         }
        //     } // End of msr_free_blktchain()

        /***************************************************************************
         * msr_addblockette:
         *
         * Add a blockette to the blockette chain of an MSRecord.  'blktdata'
         * should be the body of the blockette type 'blkttype' of 'length'
         * bytes without the blockette header (type and next offsets).  The
         * 'chainpos' value controls which end of the chain the blockette is
         * added to.  If 'chainpos' is 0 the blockette will be added to the
         * end of the chain (last blockette), other wise it will be added to
         * the beginning of the chain (first blockette).
         *
         * Returns a pointer to the BlktLink added to the chain on success and
         * NULL on error.
         ***************************************************************************/
        private BlktLink msr_addblockette(MSRecord msr, ref string blktdata, int length, int blkttype, int chainpos)
        {
            BlktLink blkt;

            if (msr == null)
            {
                return null;
            }

            blkt = msr.blkts;

            if (blkt != null)
            {
                if (chainpos != 0)
                {
                    blkt = new BlktLink();

                    blkt.next = msr.blkts;
                    msr.blkts = blkt;
                }
                else
                {
                    /* Find the last blockette */
                    while (blkt != null && blkt.next)
                    {
                        blkt = blkt.next;
                    }

                    blkt.next = new BlktLink();

                    blkt = blkt.next;
                    blkt.next = 0;
                }

                if (blkt == null)
                {
                    Logging.ms_log(2, "msr_addblockette(): Cannot allocate memory\n");
                    return null;
                }
            }
            else
            {
                msr.blkts = new BlktLink();

                if (msr.blkts == null)
                {
                    Logging.ms_log(2, "msr_addblockette(): Cannot allocate memory\n");
                    return null;
                }

                blkt = msr.blkts;
                blkt.next = 0;
            }

            blkt.blktoffset = 0;
            blkt.blkt_type = (ushort)blkttype;
            blkt.next_blkt = 0;

            //C++ TO C# CONVERTER TODO TASK: The memory management function 'malloc' has no equivalent in C#:
            //blkt.blktdata = (string)malloc(length);

            if (blkt.blktdata == null)
            {
                Logging.ms_log(2, "msr_addblockette(): Cannot allocate memory\n");
                return null;
            }

            //C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
            //memcpy(blkt.blktdata, blktdata, length);
            blkt.blktdata = blktdata;
            blkt.blktdatalen = (ushort)length;

            /* Setup the shortcut pointer for common blockettes */
            switch (blkttype)
            {
                case 100:
                    msr.Blkt100 = blkt.blktdata;
                    break;
                case 1000:
                    msr.Blkt1000 = blkt.blktdata;
                    break;
                case 1001:
                    msr.Blkt1001 = blkt.blktdata;
                    break;
            }

            return blkt;
        } // End of msr_addblockette()


        //     /***************************************************************************
        //      * msr_normalize_header:
        //      *
        //      * Normalize header values between the MSRecord struct and the
        //      * associated fixed-section of the header and blockettes.  Essentially
        //      * this updates the SEED structured data in the MSRecord.fsdh struct
        //      * and MSRecord.blkts chain with values stored at the MSRecord level.
        //      *
        //      * Returns the header length in bytes on success or -1 on error.
        //      ***************************************************************************/
        //     private int msr_normalize_header(MSRecord msr, flag verbose)
        //     {
        //         blkt_link_s cur_blkt;
        //         hptime_t hptimems = new hptime_t();
        //         sbyte usecoffset;
        //         string seqnum = new string(new char[7]);
        //         int offset = 0;
        //         int blktcnt = 0;
        //         int reclenexp = 0;
        //         int reclenfind;

        //         if (msr == null)
        //         {
        //             return -1;
        //         }

        //         /* Get start time rounded to tenths of milliseconds and microsecond offset */
        //         ms_hptime2tomsusecoffset(msr.starttime, hptimems, usecoffset);

        //         /* Update values in fixed section of data header */
        //         if (msr.fsdh)
        //         {
        //             if (verbose > 2)
        //             {
        //                 ms_log(1, "Normalizing fixed section of data header\n");
        //             }

        //             /* Roll-over sequence number if necessary */
        //             if (msr.sequence_number > 999999)
        //             {
        //                 msr.sequence_number = 1;
        //             }

        //             /* Update values in the MSRecord.fsdh struct */
        //             snprintf(seqnum, 7, "%06d", msr.sequence_number);
        //             //C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
        //             memcpy(msr.fsdh.sequence_number, seqnum, 6);
        //             msr.fsdh.dataquality = msr.dataquality;
        //             msr.fsdh.reserved = ' ';
        //             ms_strncpopen(msr.fsdh.network, msr.network, 2);
        //             ms_strncpopen(msr.fsdh.station, msr.station, 5);
        //             ms_strncpopen(msr.fsdh.location, msr.location, 2);
        //             ms_strncpopen(msr.fsdh.channel, msr.channel, 3);
        //             ms_hptime2btime(hptimems, (msr.fsdh.start_time));

        //             /* Determine the factor and multipler for sample rate */
        //             if (ms_genfactmult(msr.samprate, (msr.fsdh.samprate_fact), (msr.fsdh.samprate_mult)))
        //             {
        //                 if (verbose > 1)
        //                 {
        //                     ms_log(1, "Sampling rate out of range, cannot generate factor & multiplier: %g\n", msr.samprate);
        //                 }
        //                 msr.fsdh.samprate_fact = 0;
        //                 msr.fsdh.samprate_mult = 0;
        //             }

        //             offset += 48;

        //             if (msr.blkts)
        //             {
        //                 msr.fsdh.blockette_offset = offset;
        //             }
        //             else
        //             {
        //                 msr.fsdh.blockette_offset = 0;
        //             }
        //         }

        //         /* Traverse blockette chain and perform necessary updates */
        //         cur_blkt = msr.blkts;

        //         if (cur_blkt != null && verbose > 2)
        //         {
        //             ms_log(1, "Normalizing blockette chain\n");
        //         }

        //         while (cur_blkt != null)
        //         {
        //             offset += 4;

        //             if (cur_blkt.blkt_type == 100 && msr.Blkt100)
        //             {
        //                 msr.Blkt100.samprate = (float)msr.samprate;
        //                 offset += sizeof(blkt_100_s);
        //             }
        //             else if (cur_blkt.blkt_type == 1000 && msr.Blkt1000)
        //             {
        //                 msr.Blkt1000.byteorder = msr.byteorder;
        //                 msr.Blkt1000.encoding = msr.encoding;

        //                 /* Calculate the record length as an exponent of 2 */
        //                 for (reclenfind = 1, reclenexp = 1; reclenfind <= MAXRECLEN; reclenexp++)
        //                 {
        //                     reclenfind *= 2;
        //                     if (reclenfind == msr.reclen)
        //                     {
        //                         break;
        //                     }
        //                 }

        //                 if (reclenfind != msr.reclen)
        //                 {
        //                     ms_log(2, "msr_normalize_header(): Record length %d is not a power of 2\n", msr.reclen);
        //                     return -1;
        //                 }

        //                 msr.Blkt1000.reclen = reclenexp;

        //                 offset += sizeof(blkt_1000_s);
        //             }

        //             else if (cur_blkt.blkt_type == 1001)
        //             {
        //                 msr.Blkt1001.usec = usecoffset;
        //                 offset += sizeof(blkt_1001_s);
        //             }

        //             blktcnt++;
        //             cur_blkt = cur_blkt.next;
        //         }

        //         if (msr.fsdh)
        //         {
        //             msr.fsdh.numblockettes = blktcnt;
        //         }

        //         return offset;
        //     } // End of msr_normalize_header()


        //     /***************************************************************************
        //      * msr_duplicate:
        //      *
        //      * Duplicate an MSRecord struct including the fixed-section data
        //      * header and blockette chain.  If the datadup flag is true and the
        //      * source MSRecord has associated data samples copy them as well.
        //      *
        //      * Returns a pointer to a new MSRecord on success and NULL on error.
        //      ***************************************************************************/
        //     private MSRecord msr_duplicate(MSRecord msr, flag datadup)
        //     {
        //         MSRecord dupmsr = null;
        //         int samplesize = 0;

        //         if (msr == null)
        //         {
        //             return null;
        //         }

        //         /* Allocate target MSRecord structure */
        //         if ((dupmsr = msr_init(null)) == null)
        //         {
        //             return null;
        //         }

        //         /* Copy MSRecord structure */
        //         //C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
        //         memcpy(dupmsr, msr, sizeof(MSRecord));

        //         /* Reset pointers to not alias memory held by other structures */
        //         dupmsr.fsdh = null;
        //         dupmsr.blkts = null;
        //         dupmsr.datasamples = null;
        //         dupmsr.ststate = null;

        //         /* Copy fixed-section data header structure */
        //         if (msr.fsdh)
        //         {
        //             /* Allocate memory for new FSDH structure */
        //             if ((dupmsr.fsdh = new fsdh_s()) == null)
        //             {
        //                 ms_log(2, "msr_duplicate(): Error allocating memory\n");
        //                 msr_free(dupmsr);
        //                 return null;
        //             }

        //             /* Copy the contents */
        //             //C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
        //             memcpy(dupmsr.fsdh, msr.fsdh, sizeof(fsdh_s));
        //         }

        //         /* Copy the blockette chain */
        //         if (msr.blkts)
        //         {
        //             BlktLink blkt = msr.blkts;
        //             BlktLink next = null;

        //             dupmsr.blkts = 0;
        //             while (blkt != null)
        //             {
        //                 next = blkt.next;

        //                 /* Add blockette to chain of new MSRecord */
        //                 if (msr_addblockette(dupmsr, blkt.blktdata, blkt.blktdatalen, blkt.blkt_type, 0) == null)
        //                 {
        //                     ms_log(2, "msr_duplicate(): Error adding blockettes\n");
        //                     msr_free(dupmsr);
        //                     return null;
        //                 }

        //                 blkt = next;
        //             }
        //         }

        //         /* Copy data samples if requested and available */
        //         if (datadup != null && msr.datasamples)
        //         {
        //             /* Determine size of samples in bytes */
        //             samplesize = ms_samplesize(msr.sampletype);

        //             if (samplesize == 0)
        //             {
        //                 ms_log(2, "msr_duplicate(): unrecognized sample type: '%c'\n", msr.sampletype);
        //                 msr_free(dupmsr);
        //                 return null;
        //             }

        //             /* Allocate memory for new data array */
        //             //C++ TO C# CONVERTER TODO TASK: The memory management function 'malloc' has no equivalent in C#:
        //             if ((dupmsr.datasamples = (object)malloc((uint)(msr.numsamples * samplesize))) == null)
        //             {
        //                 ms_log(2, "msr_duplicate(): Error allocating memory\n");
        //                 msr_free(dupmsr);
        //                 return null;
        //             }

        //             /* Copy the data array */
        //             //C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
        //             memcpy(dupmsr.datasamples, msr.datasamples, ((uint)(msr.numsamples * samplesize)));
        //         }
        //         /* Otherwise make sure the sample array and count are zero */
        //         else
        //         {
        //             dupmsr.datasamples = 0;
        //             dupmsr.numsamples = 0;
        //         }

        //         return dupmsr;
        //     } // End of msr_duplicate()


        //     /***************************************************************************
        //      * msr_samprate:
        //      *
        //      * Calculate and return a double precision sample rate for the
        //      * specified MSRecord.  If a Blockette 100 was included and parsed,
        //      * the "Actual sample rate" (field 3) will be returned, otherwise a
        //      * nominal sample rate will be calculated from the sample rate factor
        //      * and multiplier in the fixed section data header.
        //      *
        //      * Returns the positive sample rate on success and -1.0 on error.
        //      ***************************************************************************/
        //     private double msr_samprate(MSRecord msr)
        //     {
        //         if (msr == null)
        //         {
        //             return -1.0;
        //         }

        //         if (msr.Blkt100)
        //         {
        //             return (double)msr.Blkt100.samprate;
        //         }
        //         else
        //         {
        //             return msr_nomsamprate(msr);
        //         }
        //     } // End of msr_samprate()

        //     /***************************************************************************
        //      * msr_nomsamprate:
        //      *
        //      * Calculate a double precision nominal sample rate from the sample
        //      * rate factor and multiplier in the FSDH struct of the specified
        //      * MSRecord.
        //      *
        //      * Returns the positive sample rate on success and -1.0 on error.
        //      ***************************************************************************/
        //     private double msr_nomsamprate(MSRecord msr)
        //     {
        //         if (msr == null)
        //         {
        //             return -1.0;
        //         }

        //         return ms_nomsamprate(msr.fsdh.samprate_fact, msr.fsdh.samprate_mult);
        //     } // End of msr_nomsamprate()

        /***************************************************************************
         * msr_starttime:
         *
         * Convert a btime struct of a FSDH struct of a MSRecord (the record
         * start time) into a high precision epoch time and apply time
         * corrections if any are specified in the header and bit 1 of the
         * activity flags indicates that it has not already been applied.  If
         * a Blockette 1001 is included and has been parsed the microseconds
         * of field 4 are also applied.
         *
         * Returns a high precision epoch time on success and HPTERROR on
         * error.
         ***************************************************************************/
        // private hptime_t msr_starttime(MSRecord msr)
        // {
        //     hptime_t starttime = msr_starttime_uc(msr);

        //     if (msr == null || starttime == HPTERROR)
        //     {
        //         return HPTERROR;
        //     }

        //     /* Check if a correction is included and if it has been applied,
        //        bit 1 of activity flags indicates if it has been appiled */

        //     if (msr.fsdh.time_correct != 0 && (msr.fsdh.act_flags & 0x02) == 0)
        //     {
        //         starttime += (hptime_t)msr.fsdh.time_correct * (HPTMODULUS / 10000);
        //     }

        //     /* Apply microsecond precision in a parsed Blockette 1001 */
        //     if (msr.Blkt1001)
        //     {
        //         starttime += (hptime_t)msr.Blkt1001.usec * (HPTMODULUS / 1000000);
        //     }

        //     return new hptime_t(starttime);
        // } // End of msr_starttime()


        // /***************************************************************************
        //  * msr_starttime_uc:
        //  *
        //  * Convert a btime struct of a FSDH struct of a MSRecord (the record
        //  * start time) into a high precision epoch time.  This time has no
        //  * correction(s) applied to it.
        //  *
        //  * Returns a high precision epoch time on success and HPTERROR on
        //  * error.
        //  ***************************************************************************/
        // private hptime_t msr_starttime_uc(MSRecord msr)
        // {
        //     if (msr == null)
        //     {
        //         return HPTERROR;
        //     }

        //     if (!msr.fsdh)
        //     {
        //         return HPTERROR;
        //     }

        //     return ms_btime2hptime(msr.fsdh.start_time);
        // } // End of msr_starttime_uc()


        // /***************************************************************************
        //  * msr_endtime:
        //  *
        //  * Calculate the time of the last sample in the record; this is the
        //  * actual last sample time and *not* the time "covered" by the last
        //  * sample.
        //  *
        //  * On the epoch time scale the value of a leap second is the same as
        //  * the second following the leap second, without external information
        //  * the values are ambiguous.
        //  *
        //  * Leap second handling: when a record completely contains a leap
        //  * second, starts before and ends after, the calculated end time will
        //  * be adjusted (reduced) by one second.
        //  *
        //  * Returns the time of the last sample as a high precision epoch time
        //  * on success and HPTERROR on error.
        //  ***************************************************************************/
        // private hptime_t msr_endtime(MSRecord msr)
        // {
        //     hptime_t span = 0;
        //     LeapSecond lslist = leapsecondlist;

        //     if (msr == null)
        //     {
        //         return HPTERROR;
        //     }

        //     if (msr.samprate > 0.0 && msr.samplecnt > 0)
        //     {
        //         //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
        //         //ORIGINAL LINE: span = (hptime_t)(((double)(msr->samplecnt - 1) / msr->samprate * HPTMODULUS) + 0.5);
        //         span.CopyFrom((hptime_t)(((double)(msr.samplecnt - 1) / msr.samprate * HPTMODULUS) + 0.5));
        //     }

        //     /* Check if the record contains a leap second, if list is available */
        //     if (lslist != null)
        //     {
        //         while (lslist != null)
        //         {
        //             if (lslist.leapsecond > msr.starttime && lslist.leapsecond <= (msr.starttime + span - HPTMODULUS))
        //             {
        //                 span -= HPTMODULUS;
        //                 break;
        //             }

        //             lslist = lslist.next;
        //         }
        //     }
        //     else
        //     {
        //         /* If a positive leap second occurred during this record as denoted by
        //          * bit 4 of the activity flags being set, reduce the end time to match
        //          * the now shifted UTC time. */
        //         if (msr.fsdh)
        //         {
        //             if ((msr.fsdh.act_flags & 0x10) != 0)
        //             {
        //                 span -= HPTMODULUS;
        //             }
        //         }
        //     }

        //     return (msr.starttime + span);
        // } // End of msr_endtime()



        /***************************************************************************
         * msr_srcname:
         *
         * Generate a source name string for a specified MSRecord in the
         * format: 'NET_STA_LOC_CHAN' or, if the quality flag is true:
         * 'NET_STA_LOC_CHAN_QUAL'.  The passed srcname must have enough room
         * for the resulting string.
         *
         * Returns a pointer to the resulting string or NULL on error.
         ***************************************************************************/
        public static string msr_srcname(MSRecord msr, ref string srcname, int quality)
        {
            if (msr == null)
            {
                return null;
            }

            /* Build the source name string */
            if (msr.network != "")
                srcname += msr.network;

            srcname += '_';

            if (msr.station != "")
                srcname += msr.station;

            srcname += '_';

            if (msr.location != "")
                srcname += msr.location;

            srcname += '_';

            if (msr.channel != "")
                srcname += msr.channel;

            if (quality == 1)
            {
                srcname += '_';
                srcname += msr.dataquality;
            }

            return srcname;
        }


        /***************************************************************************
         * msr_print:
         *
         * Prints header values in an MSRecord struct, if 'details' is greater
         * than 0 then detailed information about each blockette is printed.
         * If 'details' is greater than 1 very detailed information is
         * printed.  If no FSDH (msr->fsdh) is present only a single line with
         * basic information is printed.
         ***************************************************************************/
        void msr_print(MSRecord msr, bool? details = null)
        {
        }

        /***************************************************************************
         * msr_host_latency:
         *
         * Calculate the latency based on the host time in UTC accounting for
         * the time covered using the number of samples and sample rate; in
         * other words, the difference between the host time and the time of
         * the last sample in the specified Mini-SEED record.
         *
         * Double precision is returned, but the true precision is dependent
         * on the accuracy of the host system clock among other things.
         *
         * Returns seconds of latency or 0.0 on error (indistinguishable from
         * 0.0 latency).
         ***************************************************************************/
        private double msr_host_latency(MSRecord msr)
        {
            double span = 0.0; // Time covered by the samples
            double epoch; // Current epoch time
            double latency = 0.0;
            // time_t tv = new time_t();

            if (msr == null)
            {
                return 0.0;
            }

            /* Calculate the time covered by the samples */
            if (msr.samprate > 0.0 && msr.samplecnt > 0)
            {
                span = (1.0 / msr.samprate) * (msr.samplecnt - 1);
            }

            /* Grab UTC time according to the system clock */
            // time(time_t) ==> This function returns the time since 00:00:00 UTC, January 1, 1970(Unix timestamp) in seconds.
            // epoch = (double)time(tv);
            epoch = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;



            /* Now calculate the latency */
            latency = epoch - ((double)msr.starttime / HPTMODULUS) - span;

            return latency;
        } // End of msr_host_latency()

    }
}
