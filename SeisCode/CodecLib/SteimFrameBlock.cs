using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CodecLib
{
    public class SteimFrameBlock
    {

        // *** constructors *** 

        /**
         * Create a new block of Steim frames for a particular version of Steim
         * copression.
         * Instantiate object with the number of 64-byte frames
         * that this block will contain (should connect to data
         * record header such that a proper power of 2 boundary is
         * formed for the data record) AND the version of Steim
         * compression used (1 and 2 currently)
         * the number of frames remains static...frames that are
         * not filled with data are simply full of nulls.
         * @param numFrames the number of frames in this Steim record
         * @param steimVersion which version of Steim compression is being used
         * (1,2,3).
         */
        public SteimFrameBlock(int numFrames, int steimVersion)
        {
            steimFrame = new SteimFrame[numFrames]; // array of frames
            for (int i = 0; i < steimFrame.Length; i++)
            {
                // initialize the SteimFrame array
                steimFrame[i] = new SteimFrame();
            }
            this.numFrames = numFrames;
            this.steimVersion = steimVersion;

            // initialize the first frame properly
            currentFrame = 0;            // sanity
            AddEncodingNibble(0); // first nibble always 00
            this.steimFrame[currentFrame].pos++;  // increment position in frame to W1
        }


        // *** public methods ***

        /**
         * Return the number of data samples represented by this frame block
         * @return integer value indicating number of samples
         */
        public int GetNumSamples()
        {
            return numSamples;
        }

        /**
         * Return the version of Steim compression used
         * @return integer value representing the Steim version (1,2,3)
         */
        public int GetSteimVersion()
        {
            return steimVersion;
        }

        public SteimFrame[] GetSteimFrames()
        {
            return steimFrame;
        }

        public int NumNonEmptyFrames()
        {
            int i = steimFrame.Length - 1;
            while (i >= 0 && steimFrame[i].isEmpty())
            {
                i--;
            }
            i++;
            return i;
        }

        public void TrimEmptyFrames()
        {
            int i = NumNonEmptyFrames();
            SteimFrame[] tmp = new SteimFrame[i];
            //System.arraycopy(steimFrame, 0, tmp, 0, i);
            Array.Copy(steimFrame, 0, tmp, 0, i);
            steimFrame = tmp;
            numFrames = steimFrame.Length;
        }

        /**
         * Return the compressed byte representation of the data for inclusion
         * in a data record.
         * @return byte array containing the encoded, compressed data
         * @throws IOException from called method(s)
         */

        //###################################################################
        //###################################################################
        //###################################################################
        //###################################################################
        //see https://social.msdn.microsoft.com/Forums/en-US/453a4b01-fe58-4787-9c68-c962b63ebd23/bytearrayoutputstream-and-dataoutputstream-in-c?forum=netfxcompact
        public sbyte[] GetEncodedData()
        {
            // set up a byte array to write int words to

            //ByteArrayOutputStream encodedData = 
            //       new ByteArrayOutputStream(numFrames * 64);

            System.IO.MemoryStream encodedData =
                new System.IO.MemoryStream(numFrames * 64);


            // set up interface to the array for writing the ints
            BinaryWriter intSerializer =
                new BinaryWriter(encodedData);




            for (int i = 0; i < numFrames; i++)
            {  // for each frame
                for (int j = 0; j < 16; j++)
                {     // for each word
                    //convert  litle endian to big by swap   =====by me!
                    intSerializer.Write(Swap(steimFrame[i].word[j]));
                }
            }
            // convert c# byte to java byte array    =====by me!
            // sbyte = signed byte;    
            sbyte[] java_encodedData = new sbyte[encodedData.Length];
            var encod_array = encodedData.ToArray();
            for (int i = 0; i < encodedData.Length; i++)
                java_encodedData[i] = (sbyte)(encod_array[i]);

            return java_encodedData;//encodedData.ToArray(); // return byte stream as array
        }
        public static int Swap(int value)
        {
            int b1 = (value >> 0) & 0xff;
            int b2 = (value >> 8) & 0xff;
            int b3 = (value >> 16) & 0xff;
            int b4 = (value >> 24) & 0xff;
            return b1 << 24 | b2 << 16 | b3 << 8 | b4 << 0;

        }
        //###################################################################
        //###################################################################
        //###################################################################
        //###################################################################
        //###################################################################

        /**
         * Return the number of frames in this frame block
         * @return integer value indicating number of frames
         */
        public int GetNumFrames()
        {
            return numFrames;
        }


        // *** private and protected methods ***

        /**
         * Add a single 32-bit word to current frame.
         * @param samples the number of sample differences in the word
         * @param nibble a value of 0 to 3 that reflects the W0 encoding
         * for this word
         * @return boolean indicating true if the block is full (ie: the
         * calling app should not add any more to this object)
         */
        protected internal bool AddEncodedWord(int word, int samples, int nibble)
        {
            int pos = steimFrame[currentFrame].pos; // word position
            steimFrame[currentFrame].word[pos] = word; // add word
            AddEncodingNibble(nibble);                     // add nibble
            numSamples += samples;
            pos++;     // increment position in frame
            if (pos > 15)
            {  // need next frame?
                currentFrame++;
                if (currentFrame >= numFrames)
                {  // exceeded frame limit?
                    return true;  // block is full
                }
                AddEncodingNibble(0); // first nibble always 00
            }
            steimFrame[currentFrame].pos++;  // increment position in frame
            return false;  // block is not yet full
        }

        /**
         * Set the reverse integration constant X(N) explicitly to the
         * provided word value.
         * This method is typically used to reset X(N) should the compressor
         * fill the frame block before all samples have been read.
         * @param word integer value to be placed in X(N)
         */
        protected internal void SetXsubN(int word)
        {
            steimFrame[0].word[2] = word;
            return;
        }

        /**
        * Add encoding nibble to W0.
        * @param bitFlag a value 0 to 3 representing an encoding nibble
        */
        private void AddEncodingNibble(int bitFlag)
        {
            int offset = steimFrame[currentFrame].pos; // W0 nibble offset - determines Cn in W0
            int shift = (15 - offset) * 2;  // how much to shift bitFlag
            steimFrame[currentFrame].word[0] |= (bitFlag << shift);
            return;
        }




        // *** inner classes ***

        /**
         * This represents a single Steim compression frame.  It stores values
         * as an int array and keeps track of it's current position in the frame.
         */
        public class SteimFrame
        {
            public int[] word = new int[16];  // 16 32-byte words
            public int pos = 0;  // word position in frame (pos: 0 = W0, 1 = W1, etc...)
            public bool isEmpty()
            {
                if (word[0] == 0)
                {
                    return true;
                }
                else { return false; }
            }
        }


        // *** instance variables ***

        private int numFrames = 0;        // number of frames this object contains
        private int numSamples = 0;      // number of samples represented
        private int steimVersion = 0;    // Steim version number
        private int currentFrame = 0;     // number of current frame being built
        private SteimFrame[] steimFrame = null;  // array of frames;



    }
}
