﻿//using libmseedNetCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using static libmseedNetCore.Constants;
//namespace libmseedNetCore
//{
//    /***************************************************************************
//     * pack.c:
//     *
//     * Generic routines to pack Mini-SEED records using an MSrecord as a
//     * header template and data source.
//     *
//     * Written by Chad Trabant,
//     *   IRIS Data Management Center
//     *
//     * modified: 2015.273
//     ***************************************************************************/
//    public class pack
//    {
//        /* Header and data byte order flags controlled by environment variables */
//        /* -2 = not checked, -1 = checked but not set, or 0 = LE and 1 = BE */
//        int packheaderbyteorder = -2;
//        int packdatabyteorder = -2;
//        /***************************************************************************
//         * msr_pack:
//         *
//         * Pack data into SEED data records.  Using the record header values
//         * in the MSRecord as a template the common header fields are packed
//         * into the record header, blockettes in the blockettes chain are
//         * packed and data samples are packed in the encoding format indicated
//         * by the MSRecord->encoding field.  A Blockette 1000 will be added if
//         * one is not present.
//         *
//         * The MSRecord->datasamples array and MSRecord->numsamples value will
//         * not be changed by this routine.  It is the responsibility of the
//         * calling routine to adjust the data buffer if desired.
//         *
//         * As each record is filled and finished they are passed to
//         * record_handler which expects 1) a char * to the record, 2) the
//         * length of the record and 3) a pointer supplied by the original
//         * caller containing optional private data (handlerdata).  It is the
//         * responsibility of record_handler to process the record, the memory
//         * will be re-used or freed when record_handler returns.
//         *
//         * If the flush flag != 0 all of the data will be packed into data
//         * records even though the last one will probably not be filled.
//         *
//         * Default values are: data record & quality indicator = 'D', record
//         * length = 4096, encoding = 11 (Steim2) and byteorder = 1 (MSBF).
//         * The defaults are triggered when the the msr->dataquality is 0 or
//         * msr->reclen, msr->encoding and msr->byteorder are -1 respectively.
//         *
//         * Returns the number of records created on success and -1 on error.
//         ***************************************************************************/
//        private delegate void record_handlerDelegate(ref string UnnamedParameter, int UnnamedParameter2, object UnnamedParameter3);

//        private int msr_pack(MSRecord msr, record_handlerDelegate record_handler, object handlerdata, ref long packedsamples, flag flush, flag verbose)
//        {
//            //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to value types:
//            //ORIGINAL LINE: ushort *HPnumsamples;
//            ushort HPnumsamples;
//            //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to value types:
//            //ORIGINAL LINE: ushort *HPdataoffset;
//            ushort HPdataoffset;
//            blkt_1001_s HPblkt1001 = null;

//            string rawrec;
//            string envvariable;
//            string srcname = "";

//            int headerswapflag = 0;
//            int dataswapflag = 0;

//            int samplesize;
//            int headerlen;
//            int dataoffset;
//            int maxdatabytes;
//            int maxsamples;
//            int recordcnt = 0;
//            int packsamples;
//            int packoffset;
//            long totalpackedsamples;
//            hptime_t segstarttime = new hptime_t();

//            if (msr == null)
//            {
//                return -1;
//            }

//            if (record_handler == null)
//            {
//                Logging.ms_log(2, "msr_pack(): record_handler() function pointer not set!\n");
//                return -1;
//            }

//            /* Allocate stream processing state space if needed */
//            if (msr.ststate == null)
//            {
//                msr.ststate = new StreamState();
//                if (msr.ststate == null)
//                {
//                    Logging.ms_log(2, "msr_pack(): Could not allocate memory for StreamState\n");
//                    return -1;
//                }
//                //C++ TO C# CONVERTER TODO TASK: The memory management function 'memset' has no equivalent in C#:
//                //memset(msr.ststate, 0, sizeof(StreamState));
//            }

//            /* Generate source name for MSRecord */
//            if (msrutils.msr_srcname(msr, ref srcname, 1) == "")
//            {
//                Logging.ms_log(2, "msr_unpack_data(): Cannot generate srcname\n");
//                return MS_GENERROR;
//            }

//            /* Track original segment start time for new start time calculation */
//            segstarttime = msr.starttime;

//            /* Read possible environmental variables that force byteorder */
//            if (packheaderbyteorder == -2)
//            {
//                if ((envvariable = Environment.GetEnvironmentVariable("PACK_HEADER_BYTEORDER")) != null)
//                {
//                    if (envvariable != "0" && envvariable != "1")
//                    {
//                        Logging.ms_log(2, "Environment variable PACK_HEADER_BYTEORDER must be set to '0' or '1'\n");
//                        return -1;
//                    }
//                    else if (envvariable == "0")
//                    {
//                        packheaderbyteorder = 0;
//                        if (verbose > 2)
//                        {
//                            Logging.ms_log(1, "PACK_HEADER_BYTEORDER=0, packing little-endian header\n");
//                        }
//                    }
//                    else
//                    {
//                        packheaderbyteorder = 1;
//                        if (verbose > 2)
//                        {
//                            Logging.ms_log(1, "PACK_HEADER_BYTEORDER=1, packing big-endian header\n");
//                        }
//                    }
//                }
//                else
//                {
//                    packheaderbyteorder = -1;
//                }
//            }
//            if (packdatabyteorder == -2)
//            {
//                if ((envvariable = Environment.GetEnvironmentVariable("PACK_DATA_BYTEORDER")) != null)
//                {
//                    if (envvariable != "0" && envvariable != "1")
//                    {
//                        Logging.ms_log(2, "Environment variable PACK_DATA_BYTEORDER must be set to '0' or '1'\n");
//                        return -1;
//                    }
//                    else if (envvariable == "0")
//                    {
//                        packdatabyteorder = 0;
//                        if (verbose > 2)
//                        {
//                            Logging.ms_log(1, "PACK_DATA_BYTEORDER=0, packing little-endian data samples\n");
//                        }
//                    }
//                    else
//                    {
//                        packdatabyteorder = 1;
//                        if (verbose > 2)
//                        {
//                            Logging.ms_log(1, "PACK_DATA_BYTEORDER=1, packing big-endian data samples\n");
//                        }
//                    }
//                }
//                else
//                {
//                    packdatabyteorder = -1;
//                }
//            }

//            /* Set default indicator, record length, byte order and encoding if needed */
//            if (msr.dataquality == 0)
//            {
//                msr.dataquality = 'D';
//            }
//            if (msr.reclen == -1)
//            {
//                msr.reclen = 4096;
//            }
//            if (msr.byteorder == -1)
//            {
//                msr.byteorder = 1;
//            }
//            if (msr.encoding == -1)
//            {
//                msr.encoding = DE_STEIM2;
//            }

//            /* Cleanup/reset sequence number */
//            if (msr.sequence_number <= 0 || msr.sequence_number > 999999)
//            {
//                msr.sequence_number = 1;
//            }

//            if (msr.reclen < MINRECLEN || msr.reclen > MAXRECLEN)
//            {
//                Logging.ms_log(2, "msr_pack(%s): Record length is out of range: %d\n", srcname, msr.reclen);
//                return -1;
//            }

//            if (msr.numsamples <= 0)
//            {
//                Logging.ms_log(2, "msr_pack(%s): No samples to pack\n", srcname);
//                return -1;
//            }

//            samplesize = lookup.ms_samplesize(msr.sampletype);

//            if (samplesize == 0)
//            {
//                Logging.ms_log(2, "msr_pack(%s): Unknown sample type '%c'\n", srcname, msr.sampletype);
//                return -1;
//            }

//            /* Sanity check for msr/quality indicator */
//            if (!MS_ISDATAINDICATOR(msr.dataquality))
//            {
//                Logging.ms_log(2, "msr_pack(%s): Record header & quality indicator unrecognized: '%c'\n", srcname, msr.dataquality);
//                Logging.ms_log(2, "msr_pack(%s): Packing failed.\n", srcname);
//                return -1;
//            }

//            /* Allocate space for data record */
//            //C++ TO C# CONVERTER TODO TASK: The memory management function 'malloc' has no equivalent in C#:
//            rawrec = (string)malloc(msr.reclen);

//            if (rawrec == null)
//            {
//                Logging.ms_log(2, "msr_pack(%s): Cannot allocate memory\n", srcname);
//                return -1;
//            }

//            /* Set header pointers to known offsets into FSDH */
//            //HPnumsamples = (ushort)(rawrec + 30);
//            //HPdataoffset = (ushort)(rawrec + 44);
//            HPnumsamples = 30;
//            HPdataoffset = 44;

//            /* Check to see if byte swapping is needed */
//            if (msr.byteorder != genutils.ms_bigendianhost())
//            {
//                headerswapflag = dataswapflag = 1;
//            }

//            /* Check if byte order is forced */
//            if (packheaderbyteorder >= 0)
//            {
//                headerswapflag = (msr.byteorder != packheaderbyteorder) ? 1 : 0;
//            }

//            if (packdatabyteorder >= 0)
//            {
//                dataswapflag = (msr.byteorder != packdatabyteorder) ? 1 : 0;
//            }

//            if (verbose > 2)
//            {
//                if (headerswapflag == 1 && dataswapflag == 1)
//                {
//                    Logging.ms_log(1, "%s: Byte swapping needed for packing of header and data samples\n", srcname);
//                }
//                else if (headerswapflag == 1)
//                {
//                    Logging.ms_log(1, "%s: Byte swapping needed for packing of header\n", srcname);
//                }
//                else if (dataswapflag == 1)
//                {
//                    Logging.ms_log(1, "%s: Byte swapping needed for packing of data samples\n", srcname);
//                }
//                else
//                {
//                    Logging.ms_log(1, "%s: Byte swapping NOT needed for packing\n", srcname);
//                }
//            }
//            /* Add a blank 1000 Blockette if one is not present, the blockette values
//                 will be populated in msr_pack_header_raw()/msr_normalize_header() */
//            if (msr.Blkt1000 == null)
//            {
//                blkt_1000_s blkt1000 = new blkt_1000_s();
//                //C++ TO C# CONVERTER TODO TASK: The memory management function 'memset' has no equivalent in C#:
//                //memset(blkt1000, 0, sizeof(blkt_1000_s));

//                if (verbose > 2)
//                {
//                    Logging.ms_log(1, "%s: Adding 1000 Blockette\n", srcname);
//                }

//                if (!msr_addblockette(msr, (string)blkt1000, sizeof(blkt_1000_s), 1000, 0))
//                {
//                    Logging.ms_log(2, "msr_pack(%s): Error adding 1000 Blockette\n", srcname);
//                    rawrec = null;
//                    return -1;
//                }
//            }

//            headerlen = msr_pack_header_raw(msr, rawrec, msr.reclen, headerswapflag, 1, HPblkt1001, srcname, verbose);

//            if (headerlen == -1)
//            {
//                Logging.ms_log(2, "msr_pack(%s): Error packing header\n", srcname);
//                rawrec = null;
//                return -1;
//            }

//            /* Determine offset to encoded data */
//            if (msr.encoding == DE_STEIM1 || msr.encoding == DE_STEIM2)
//            {
//                dataoffset = 64;
//                while (dataoffset < headerlen)
//                {
//                    dataoffset += 64;
//                }

//                /* Zero memory between blockettes and data if any */
//                //C++ TO C# CONVERTER TODO TASK: The memory management function 'memset' has no equivalent in C#:
//                memset(rawrec + headerlen, 0, dataoffset - headerlen);
//            }
//            else
//            {
//                dataoffset = headerlen;
//            }

//            HPdataoffset = (ushort)dataoffset;
//            if (headerswapflag>0)
//            {
//                gswap.ms_gswap2(ref HPdataoffset);
//            }

//            /* Determine the max data bytes and sample count */
//            maxdatabytes = msr.reclen - dataoffset;

//            if (msr.encoding == DE_STEIM1)
//            {
//                maxsamples = (int)(maxdatabytes / 64) * STEIM1_FRAME_MAX_SAMPLES;
//            }
//            else if (msr.encoding == DE_STEIM2)
//            {
//                maxsamples = (int)(maxdatabytes / 64) * STEIM2_FRAME_MAX_SAMPLES;
//            }
//            else
//            {
//                maxsamples = maxdatabytes / samplesize;
//            }

//            /* Pack samples into records */
//            HPnumsamples = 0;
//            totalpackedsamples = 0;
//            packoffset = 0;
//            if (packedsamples>0)
//            {
//                packedsamples = 0;
//            }

//            while ((msr.numsamples - totalpackedsamples) > maxsamples || flush)
//            {
//                packsamples = msr_pack_data(rawrec + dataoffset, (string)msr.datasamples + packoffset, (int)(msr.numsamples - totalpackedsamples), maxdatabytes, msr.ststate.lastintsample, msr.ststate.comphistory, msr.sampletype, msr.encoding, dataswapflag, srcname, verbose);

//                if (packsamples < 0)
//                {
//                    Logging.ms_log(2, "msr_pack(%s): Error packing data samples\n", srcname);
//                    rawrec = null;
//                    return -1;
//                }

//                packoffset += packsamples * samplesize;

//                /* Update number of samples */
//                HPnumsamples = (ushort)packsamples;
//                if (headerswapflag>0)
//                {
//                    gswap.ms_gswap2(ref HPnumsamples);
//                }

//                if (verbose > 0)
//                {
//                    Logging.ms_log(1, "%s: Packed %d samples\n", srcname, packsamples);
//                }

//                /* Send record to handler */
//                record_handler(rawrec, msr.reclen, handlerdata);

//                totalpackedsamples += packsamples;
//                if (packedsamples>0)
//                {
//                    packedsamples = totalpackedsamples;
//                }
//                msr.ststate.packedsamples += packsamples;

//                /* Update record header for next record */
//                msr.sequence_number = (msr.sequence_number >= 999999) ? 1 : msr.sequence_number + 1;
//                if (msr.samprate > 0)
//                {
//                    msr.starttime = segstarttime + (hptime_t)(totalpackedsamples / msr.samprate * HPTMODULUS + 0.5);
//                }

//                msr_update_header(msr, rawrec, headerswapflag, HPblkt1001, srcname, verbose);

//                recordcnt++;
//                msr.ststate.packedrecords++;

//                /* Set compression history flag for subsequent records (Steim encodings) */
//                if (!msr.ststate.comphistory)
//                {
//                    msr.ststate.comphistory = 1;
//                }

//                if (totalpackedsamples >= msr.numsamples)
//                {
//                    break;
//                }
//            }

//            if (verbose > 2)
//            {
//                Logging.ms_log(1, "%s: Packed %d total samples\n", srcname, totalpackedsamples);
//            }

//            rawrec = null;

//            return recordcnt;
//        } // End of msr_pack()













//        /***************************************************************************
//         * msr_pack_header:
//         *
//         * Pack data header/blockettes into the SEED record at
//         * MSRecord->record.  Unlike msr_pack no default values are applied,
//         * the header structures are expected to be self describing and no
//         * Blockette 1000 will be added.  This routine is only useful for
//         * re-packing a record header.
//         *
//         * Returns the header length in bytes on success and -1 on error.
//         ***************************************************************************/
//        private int msr_pack_header(MSRecord msr, int normalize, int verbose)
//        {
//            string srcname = new string(new char[50]);
//            string envvariable;
//            int? headerswapflag = 0;
//            int headerlen;
//            int maxheaderlen;

//            if (msr == null)
//            {
//                return -1;
//            }

//            /* Generate source name for MSRecord */
//            if (msrutils.msr_srcname(msr, ref srcname, 1) == null)
//            {
//                Logging.ms_log(2, "msr_unpack_data(): Cannot generate srcname\n");
//                return MS_GENERROR;
//            }

//            /* Read possible environmental variables that force byteorder */
//            if (packheaderbyteorder == -2)
//            {
//                if ((envvariable = Environment.GetEnvironmentVariable("PACK_HEADER_BYTEORDER"))!=null)
//                {
//                    if (envvariable != "0" && envvariable != "1")
//                    {
//                        Logging.ms_log(2, "Environment variable PACK_HEADER_BYTEORDER must be set to '0' or '1'\n");
//                        return -1;
//                    }
//                    else if (envvariable == "0")
//                    {
//                        packheaderbyteorder = 0;
//                        if (verbose > 2)
//                        {
//                            Logging.ms_log(1, "PACK_HEADER_BYTEORDER=0, packing little-endian header\n");
//                        }
//                    }
//                    else
//                    {
//                        packheaderbyteorder = 1;
//                        if (verbose > 2)
//                        {
//                            Logging.ms_log(1, "PACK_HEADER_BYTEORDER=1, packing big-endian header\n");
//                        }
//                    }
//                }
//                else
//                {
//                    packheaderbyteorder = -1;
//                }
//            }

//            if (msr.reclen < MINRECLEN || msr.reclen > MAXRECLEN)
//            {
//                Logging.ms_log(2, "msr_pack_header(%s): record length is out of range: %d\n", srcname, msr.reclen);
//                return -1;
//            }

//            if (msr.byteorder != 0 && msr.byteorder != 1)
//            {
//                Logging.ms_log(2, "msr_pack_header(%s): byte order is not defined correctly: %d\n", srcname, msr.byteorder);
//                return -1;
//            }

//            if (msr.fsdh != null)
//            {
//                maxheaderlen = (msr.fsdh.data_offset > 0) ? msr.fsdh.data_offset : msr.reclen;
//            }
//            else
//            {
//                maxheaderlen = msr.reclen;
//            }

//            /* Check to see if byte swapping is needed */
//            if (msr.byteorder != genutils.ms_bigendianhost())
//            {
//                headerswapflag = 1;
//            }

//            /* Check if byte order is forced */
//            if (packheaderbyteorder >= 0)
//            {
//                headerswapflag = (msr.byteorder != packheaderbyteorder) ? 1 : 0;
//            }

//            if (verbose > 2)
//            {
//                if (headerswapflag != null)
//                {
//                    Logging.ms_log(1, "%s: Byte swapping needed for packing of header\n", srcname);
//                }
//                else
//                {
//                    Logging.ms_log(1, "%s: Byte swapping NOT needed for packing of header\n", srcname);
//                }
//            }

//            headerlen = msr_pack_header_raw(msr, ref msr.record, maxheaderlen, headerswapflag, normalize, null, ref srcname, verbose);

//            return headerlen;
//        } // End of msr_pack_header()








//        /***************************************************************************
//         * msr_pack_header_raw:
//         *
//         * Pack data header/blockettes into the specified SEED data record.
//         *
//         * Returns the header length in bytes on success or -1 on error.
//         ***************************************************************************/
//        private static int msr_pack_header_raw(MSRecord msr, ref string rawrec, int maxheaderlen, int? swapflag, int? normalize, blkt_1001_s[] blkt1001, ref string srcname, int verbose)
//        {
//            BlktLink cur_blkt;
//            fsdh_s fsdh;
//            short offset;
//            int blktcnt = 0;
//            int nextoffset;

//            if (msr == null || !rawrec)
//            {
//                return -1;
//            }

//            /* Make sure a fixed section of data header is available */
//            if (!msr.fsdh)
//            {
//                //C++ TO C# CONVERTER TODO TASK: The memory management function 'calloc' has no equivalent in C#:
//                msr.fsdh = (fsdh_s)calloc(1, sizeof(fsdh_s));

//                if (msr.fsdh == null)
//                {
//                    Logging.ms_log(2, "msr_pack_header_raw(%s): Cannot allocate memory\n", srcname);
//                    return -1;
//                }
//            }

//            /* Update the SEED structures associated with the MSRecord */
//            if (normalize != null)
//            {
//                if (msr_normalize_header(msr, verbose) < 0)
//                {
//                    Logging.ms_log(2, "msr_pack_header_raw(%s): error normalizing header values\n", srcname);
//                    return -1;
//                }
//            }

//            if (verbose > 2)
//            {
//                Logging.ms_log(1, "%s: Packing fixed section of data header\n", srcname);
//            }

//            if (maxheaderlen > msr.reclen)
//            {
//                Logging.ms_log(2, "msr_pack_header_raw(%s): maxheaderlen of %d is beyond record length of %d\n", srcname, maxheaderlen, msr.reclen);
//                return -1;
//            }

//            if (maxheaderlen < (int)sizeof(fsdh_s))
//            {
//                Logging.ms_log(2, "msr_pack_header_raw(%s): maxheaderlen of %d is too small, must be >= %d\n", srcname, maxheaderlen, sizeof(fsdh_s));
//                return -1;
//            }

//            fsdh = (fsdh_s)rawrec;
//            offset = 48;

//            /* Roll-over sequence number if necessary */
//            if (msr.sequence_number > 999999)
//            {
//                msr.sequence_number = 1;
//            }

//            /* Copy FSDH associated with the MSRecord into the record */
//            //C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
//            memcpy(fsdh, msr.fsdh, sizeof(fsdh_s));

//            /* Swap byte order? */
//            if (swapflag != null)
//            {
//                MS_SWAPBTIME(fsdh.start_time);
//                gswap.ms_gswap2(ref fsdh.numsamples);
//                gswap.ms_gswap2(ref fsdh.samprate_fact);
//                gswap.ms_gswap2(ref fsdh.samprate_mult);
//                gswap.ms_gswap4(ref fsdh.time_correct);
//                gswap.ms_gswap2(ref fsdh.data_offset);
//                gswap.ms_gswap2(ref fsdh.blockette_offset);
//            }

//            /* Traverse blockette chain and pack blockettes at 'offset' */
//            cur_blkt = msr.blkts;

//            while (cur_blkt != null && offset < maxheaderlen)
//            {
//                /* Check that the blockette fits */
//                if ((offset + 4 + cur_blkt.blktdatalen) > maxheaderlen)
//                {
//                    Logging.ms_log(2, "msr_pack_header_raw(%s): header exceeds maxheaderlen of %d\n", srcname, maxheaderlen);
//                    break;
//                }

//                /* Pack blockette type and leave space for next offset */
//                //C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
//                memcpy(rawrec.Substring(offset), cur_blkt.blkt_type, 2);
//                if (swapflag != null)
//                {
//                    gswap.ms_gswap2(ref rawrec.Substring(offset));
//                }
//                nextoffset = offset + 2;
//                offset += 4;
//                if (cur_blkt.blkt_type == 100)
//                {
//                    blkt_100_s blkt_100 = (blkt_100_s)(rawrec + offset);
//                    //C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
//                    memcpy(blkt_100, cur_blkt.blktdata, sizeof(blkt_100_s));
//                    offset += sizeof(blkt_100_s);

//                    if (swapflag>0)
//                    {
//                        gswap.ms_gswap4(ref blkt_100.samprate);
//                    }
//                }

//                else if (cur_blkt.blkt_type == 200)
//                {
//                    blkt_200_s blkt_200 = (blkt_200_s)(rawrec + offset);
//                    //C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
//                    memcpy(blkt_200, cur_blkt.blktdata, sizeof(blkt_200_s));
//                    offset += sizeof(blkt_200_s);

//                    if (swapflag>0)
//                    {
//                        gswap.ms_gswap4(blkt_200.amplitude);
//                        gswap.ms_gswap4(blkt_200.period);
//                        gswap.ms_gswap4(blkt_200.background_estimate);
//                        MS_SWAPBTIME(blkt_200.time);
//                    }
//                }

//                else if (cur_blkt.blkt_type == 201)
//                {
//                    blkt_201_s blkt_201 = (blkt_201_s)(rawrec + offset);
//                    //C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
//                    memcpy(blkt_201, cur_blkt.blktdata, sizeof(blkt_201_s));
//                    offset += sizeof(blkt_201_s);

//                    if (swapflag>0)
//                    {
//                        gswap.ms_gswap4(blkt_201.amplitude);
//                        gswap.ms_gswap4(blkt_201.period);
//                        gswap.ms_gswap4(blkt_201.background_estimate);
//                        MS_SWAPBTIME(blkt_201.time);
//                    }
//                }

//                else if (cur_blkt.blkt_type == 300)
//                {
//                    blkt_300_s blkt_300 = (blkt_300_s)(rawrec + offset);
//                    //C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
//                    memcpy(blkt_300, cur_blkt.blktdata, sizeof(blkt_300_s));
//                    offset += sizeof(blkt_300_s);

//                    if (swapflag>0)
//                    {
//                        MS_SWAPBTIME(blkt_300.time);
//                        gswap.ms_gswap4(blkt_300.step_duration);
//                        gswap.ms_gswap4(blkt_300.interval_duration);
//                        gswap.ms_gswap4(blkt_300.amplitude);
//                        gswap.ms_gswap4(blkt_300.reference_amplitude);
//                    }
//                }

//                else if (cur_blkt.blkt_type == 310)
//                {
//                    blkt_310_s blkt_310 = (blkt_310_s)(rawrec + offset);
//                    //C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
//                    memcpy(blkt_310, cur_blkt.blktdata, sizeof(blkt_310_s));
//                    offset += sizeof(blkt_310_s);

//                    if (swapflag>0)
//                    {
//                        MS_SWAPBTIME(blkt_310.time);
//                        gswap.ms_gswap4(blkt_310.duration);
//                        gswap.ms_gswap4(blkt_310.period);
//                        gswap.ms_gswap4(blkt_310.amplitude);
//                        gswap.ms_gswap4(blkt_310.reference_amplitude);
//                    }
//                }

//                else if (cur_blkt.blkt_type == 320)
//                {
//                    blkt_320_s blkt_320 = (blkt_320_s)(rawrec + offset);
//                    //C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
//                    memcpy(blkt_320, cur_blkt.blktdata, sizeof(blkt_320_s));
//                    offset += sizeof(blkt_320_s);

//                    if (swapflag>0)
//                    {
//                        MS_SWAPBTIME(blkt_320.time);
//                        gswap.ms_gswap4(blkt_320.duration);
//                        gswap.ms_gswap4(blkt_320.ptp_amplitude);
//                        gswap.ms_gswap4(blkt_320.reference_amplitude);
//                    }
//                }

//                else if (cur_blkt.blkt_type == 390)
//                {
//                    blkt_390_s blkt_390 = (blkt_390_s)(rawrec + offset);
//                    //C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
//                    memcpy(blkt_390, cur_blkt.blktdata, sizeof(blkt_390_s));
//                    offset += sizeof(blkt_390_s);

//                    if (swapflag>0)
//                    {
//                        MS_SWAPBTIME(blkt_390.time);
//                        gswap.ms_gswap4(blkt_390.duration);
//                        gswap.ms_gswap4(blkt_390.amplitude);
//                    }
//                }
//                else if (cur_blkt.blkt_type == 395)
//                {
//                    blkt_395_s blkt_395 = (blkt_395_s)(rawrec + offset);
//                    //C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
//                    memcpy(blkt_395, cur_blkt.blktdata, sizeof(blkt_395_s));
//                    offset += sizeof(blkt_395_s);

//                    if (swapflag>0)
//                    {
//                        MS_SWAPBTIME(blkt_395.time);
//                    }
//                }

//                else if (cur_blkt.blkt_type == 400)
//                {
//                    blkt_400_s blkt_400 = (blkt_400_s)(rawrec + offset);
//                    //C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
//                    memcpy(blkt_400, cur_blkt.blktdata, sizeof(blkt_400_s));
//                    offset += sizeof(blkt_400_s);

//                    if (swapflag>0)
//                    {
//                        gswap.ms_gswap4(blkt_400.azimuth);
//                        gswap.ms_gswap4(blkt_400.slowness);
//                        gswap.ms_gswap2(blkt_400.configuration);
//                    }
//                }

//                else if (cur_blkt.blkt_type == 405)
//                {
//                    blkt_405_s blkt_405 = (blkt_405_s)(rawrec + offset);
//                    //C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
//                    memcpy(blkt_405, cur_blkt.blktdata, sizeof(blkt_405_s));
//                    offset += sizeof(blkt_405_s);

//                    if (swapflag>0)
//                    {
//                        gswap.ms_gswap2(blkt_405.delay_values);
//                    }

//                    if (verbose > 0)
//                    {
//                        Logging.ms_log(1, "msr_pack_header_raw(%s): WARNING Blockette 405 cannot be fully supported\n", srcname);
//                    }
//                }

//                else if (cur_blkt.blkt_type == 500)
//                {
//                    blkt_500_s blkt_500 = (blkt_500_s)(rawrec + offset);
//                    //C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
//                    memcpy(blkt_500, cur_blkt.blktdata, sizeof(blkt_500_s));
//                    offset += sizeof(blkt_500_s);

//                    if (swapflag>0)
//                    {
//                        gswap.ms_gswap4(blkt_500.vco_correction);
//                        MS_SWAPBTIME(blkt_500.time);
//                        gswap.ms_gswap4(blkt_500.exception_count);
//                    }
//                }

//                else if (cur_blkt.blkt_type == 1000)
//                {
//                    blkt_1000_s blkt_1000 = (blkt_1000_s)(rawrec + offset);
//                    //C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
//                    memcpy(blkt_1000, cur_blkt.blktdata, sizeof(blkt_1000_s));
//                    offset += sizeof(blkt_1000_s);

//                    /* This guarantees that the byte order is in sync with msr_pack() */
//                    if (packdatabyteorder >= 0)
//                    {
//                        blkt_1000.byteorder = packdatabyteorder;
//                    }
//                }

//                else if (cur_blkt.blkt_type == 1001)
//                {
//                    blkt_1001_s blkt_1001 = (blkt_1001_s)(rawrec + offset);
//                    //C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
//                    memcpy(blkt_1001, cur_blkt.blktdata, sizeof(blkt_1001_s));
//                    offset += sizeof(blkt_1001_s);

//                    /* Track location of Blockette 1001 if requested */
//                    if (blkt1001)
//                    {
//                        *blkt1001 = blkt_1001;
//                    }
//                }

//                else if (cur_blkt.blkt_type == 2000)
//                {
//                    blkt_2000_s blkt_2000 = (blkt_2000_s)(rawrec + offset);
//                    //C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
//                    memcpy(blkt_2000, cur_blkt.blktdata, cur_blkt.blktdatalen);
//                    offset += cur_blkt.blktdatalen;

//                    if (swapflag>0)
//                    {
//                        gswap.ms_gswap2(blkt_2000.length);
//                        gswap.ms_gswap2(blkt_2000.data_offset);
//                        gswap.ms_gswap4(blkt_2000.recnum);
//                    }

//                    /* Nothing done to pack the opaque headers and data, they should already
//                       be packed into the blockette payload */
//                }

//                else
//                {
//                    //C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
//                    memcpy(rawrec + offset, cur_blkt.blktdata, cur_blkt.blktdatalen);
//                    offset += cur_blkt.blktdatalen;
//                }

//                /* Pack the offset to the next blockette */
//                if (cur_blkt.next)
//                {
//                    //C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
//                    memcpy(rawrec + nextoffset, offset, 2);
//                    if (swapflag>0)
//                    {
//                        gswap.ms_gswap2(rawrec + nextoffset);
//                    }
//                }
//                else
//                {
//                    //C++ TO C# CONVERTER TODO TASK: The memory management function 'memset' has no equivalent in C#:
//                    memset(rawrec + nextoffset, 0, 2);
//                }

//                blktcnt++;
//                cur_blkt = cur_blkt.next;
//            }

//            fsdh.numblockettes = blktcnt;

//            if (verbose > 2)
//            {
//                Logging.ms_log(1, "%s: Packed %d blockettes\n", srcname, blktcnt);
//            }

//            return new return (offset);
//        } // End of msr_pack_header_raw()















//        /***************************************************************************
//         * msr_update_header:
//         *
//         * Update the header values that change between records: start time,
//         * sequence number, etc.
//         *
//         * Returns 0 on success or -1 on error.
//         ***************************************************************************/
//        private static int msr_update_header(MSRecord msr, ref string rawrec, flag swapflag, blkt_1001_s blkt1001, ref string srcname, flag verbose)
//        {
//            fsdh_s fsdh;
//            hptime_t hptimems = new hptime_t();
//            sbyte usecoffset;
//            string seqnum = new string(new char[7]);

//            if (msr == null || !rawrec)
//            {
//                return -1;
//            }

//            if (verbose > 2)
//            {
//                Logging.ms_log(1, "%s: Updating fixed section of data header\n", srcname);
//            }

//            fsdh = (fsdh_s)rawrec;

//            /* Pack values into the fixed section of header */
//            snprintf(seqnum, 7, "%06d", msr.sequence_number);
//            //C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
//            memcpy(fsdh.sequence_number, seqnum, 6);

//            /* Get start time rounded to tenths of milliseconds and microsecond offset */
//            ms_hptime2tomsusecoffset(msr.starttime, hptimems, usecoffset);

//            /* Update fixed-section start time */
//            genutils.ms_hptime2btime(hptimems, (fsdh.start_time));

//            /* Swap byte order? */
//            if (swapflag != null)
//            {
//                MS_SWAPBTIME(fsdh.start_time);
//            }

//            /* Update microsecond offset value if Blockette 1001 is present */
//            if (msr.Blkt1001 && blkt1001 != null)
//            {
//                /* Update microseconds offset in blockette chain entry */
//                msr.Blkt1001.usec = usecoffset;

//                /* Update microseconds offset in packed header */
//                blkt1001.usec = usecoffset;
//            }

//            return 0;
//        } // End of msr_update_header()



//        /************************************************************************
//         *  msr_pack_data:
//         *
//         *  Pack Mini-SEED data samples.  The input data samples specified as
//         *  'src' will be packed with 'encoding' format and placed in 'dest'.
//         *
//         *  If a pointer to a 32-bit integer sample is provided in the
//         *  argument 'lastintsample' and 'comphistory' is true the sample
//         *  value will be used to seed the difference buffer for Steim1/2
//         *  encoding and provide a compression history.  It will also be
//         *  updated with the last sample packed in order to be used with a
//         *  subsequent call to this routine.
//         *
//         *  Return number of samples packed on success and a negative on error.
//         ************************************************************************/
//        private static int msr_pack_data(ref object[] dest, object[] src, int maxsamples, int maxdatabytes, ref int lastintsample, int? comphistory, char sampletype, int encoding, int swapflag, ref string srcname, int? verbose)
//        {
//            int nsamples;
//            int[] intbuff;

//            int d0;

//            /* Check for encode debugging environment variable */
//            if (Environment.GetEnvironmentVariable("ENCODE_DEBUG") != null)
//            {
//                encodedebug = 1;
//            }

//            /* Decide if this is a format that we can encode */
//            switch (encoding)
//            {
//                case DE_ASCII:
//                    if (sampletype != 'a')
//                    {
//                        Logging.ms_log(2, "%s: Sample type must be ascii (a) for ASCII text encoding not '%c'\n", srcname, sampletype);
//                        return -1;
//                    }

//                    if (verbose > 1)
//                    {
//                        Logging.ms_log(1, "%s: Packing ASCII data\n", srcname);
//                    }

//                    nsamples = packdata.msr_encode_text(src, maxsamples, dest, maxdatabytes);

//                    break;

//                case DE_INT16:
//                    if (sampletype != 'i')
//                    {
//                        Logging.ms_log(2, "%s: Sample type must be integer (i) for INT16 encoding not '%c'\n", srcname, sampletype);
//                        return -1;
//                    }

//                    if (verbose > 1)
//                    {
//                        Logging.ms_log(1, "%s: Packing INT16 data samples\n", srcname);
//                    }

//                    nsamples = packdata.msr_encode_int16(ref src, maxsamples, ref dest, maxdatabytes, swapflag);

//                    break;

//                case DE_INT32:
//                    if (sampletype != 'i')
//                    {
//                        Logging.ms_log(2, "%s: Sample type must be integer (i) for INT32 encoding not '%c'\n", srcname, sampletype);
//                        return -1;
//                    }

//                    if (verbose > 1)
//                    {
//                        Logging.ms_log(1, "%s: Packing INT32 data samples\n", srcname);
//                    }

//                    nsamples = packdata.msr_encode_int32(src, maxsamples, dest, maxdatabytes, swapflag);

//                    break;

//                case DE_FLOAT32:
//                    if (sampletype != 'f')
//                    {
//                        Logging.ms_log(2, "%s: Sample type must be float (f) for FLOAT32 encoding not '%c'\n", srcname, sampletype);
//                        return -1;
//                    }

//                    if (verbose > 1)
//                    {
//                        Logging.ms_log(1, "%s: Packing FLOAT32 data samples\n", srcname);
//                    }

//                    nsamples = packdata.msr_encode_float32(src, maxsamples, dest, maxdatabytes, swapflag);

//                    break;

//                case DE_FLOAT64:
//                    if (sampletype != 'd')
//                    {
//                        Logging.ms_log(2, "%s: Sample type must be double (d) for FLOAT64 encoding not '%c'\n", srcname, sampletype);
//                        return -1;
//                    }

//                    if (verbose > 1)
//                    {
//                        Logging.ms_log(1, "%s: Packing FLOAT64 data samples\n", srcname);
//                    }

//                    nsamples = packdata.msr_encode_float64(src, maxsamples, dest, maxdatabytes, swapflag);

//                    break;

//                case DE_STEIM1:
//                    if (sampletype != 'i')
//                    {
//                        Logging.ms_log(2, "%s: Sample type must be integer (i) for Steim1 compression not '%c'\n", srcname, sampletype);
//                        return -1;
//                    }

//                    // intbuff = (int)src;
//                    intbuff = src.Cast<int>().ToArray();
//                    var outbuff = dest.Cast<int>().ToArray();

//                    /* If a previous sample is supplied use it for compression history otherwise cold-start */
//                    d0 = (lastintsample != 0 && comphistory != null) ? (intbuff[0] - lastintsample) : 0;

//                    if (verbose > 1)
//                    {
//                        Logging.ms_log(1, "%s: Packing Steim1 data frames\n", srcname);
//                    }

//                    nsamples = packdata.msr_encode_steim1(intbuff, maxsamples, ref outbuff, maxdatabytes, d0, swapflag);
//                    dest = outbuff.Cast<object>().ToArray();

//                    /* If a previous sample is supplied update it with the last sample value */
//                    if (lastintsample != 0 && nsamples > 0)
//                    {
//                        lastintsample = intbuff[nsamples - 1];
//                    }

//                    break;

//                case DE_STEIM2:
//                    if (sampletype != 'i')
//                    {
//                        Logging.ms_log(2, "%s: Sample type must be integer (i) for Steim2 compression not '%c'\n", srcname, sampletype);
//                        return -1;
//                    }

//                    //intbuff = (int)src;
//                    intbuff = src.Cast<int>().ToArray();
//                    outbuff = dest.Cast<int>().ToArray();

//                    /* If a previous sample is supplied use it for compression history otherwise cold-start */
//                    d0 = (lastintsample != 0 && comphistory != null) ? (intbuff[0] - lastintsample) : 0;

//                    if (verbose > 1)
//                    {
//                        Logging.ms_log(1, "%s: Packing Steim2 data frames\n", srcname);
//                    }

//                    nsamples = packdata.msr_encode_steim2(src, maxsamples, dest, maxdatabytes, d0, srcname, swapflag);

//                    dest = outbuff.Cast<object>().ToArray();

//                    /* If a previous sample is supplied update it with the last sample value */
//                    if (lastintsample != 0 && nsamples > 0)
//                    {
//                        lastintsample = intbuff[nsamples - 1];
//                    }

//                    break;

//                default:
//                    Logging.ms_log(2, "%s: Unable to pack format %d\n", srcname, encoding);

//                    return -1;
//            }

//            return nsamples;
//        } // End of msr_pack_data()

//        private bool MS_ISDATAINDICATOR(char X)
//        {
//            //#define MS_ISDATAINDICATOR(X) (X=='D' || X=='R' || X=='Q' || X=='M')
//            if (X == 'D' || X == 'R' || X == 'Q' || X == 'M')
//                return true;
//            return false;

//        }


//        private static void MS_SWAPBTIME(BTime x) {
//            gswap.ms_gswap2(ref x.year);
//            gswap.ms_gswap2(ref x.day);
//            gswap.ms_gswap2(ref x.fract); 
//        }

//    }
//}
