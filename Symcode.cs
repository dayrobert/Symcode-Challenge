/**
 * Code Challenge Symcode
 * 
 * Author: Robert Day <day.c.robert@gmail.com>(p: 617 967 5901)
 * Challenge site: http://yetanotherwhatever.io/tp/60F66F5C-3357-4AA8-B0BF-04FD2DECD8E1.html * 
 * 
 */
 
 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace symantec.challenge.symcode
{
    /// <summary>
    /// Class handled Symcode encoding routines
    /// </summary>
    class Symcode
    {
        protected const string SYMCODE = "symantec";
        protected const int ENCODE_CHUNK_SIZE = 3;

        /// <summary>
        /// Encode the passed string. 
        /// </summary>
        /// <param name="text">The text to be encoded</param>
        /// <returns>The SYMCODE encoded string</returns>
        public static string EncodeLine(String text)
        {
            string ret = "";

            // clean the input and if the string is empty then return a empty string
            text = text.Trim();
            if (text.Length == 0)
                return "";

            // process the line ENCODE_CHUNK_SIZE characters at a time
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            while (text.Length > 0)
            {
                string subtext = text.Length > ENCODE_CHUNK_SIZE ? text.Substring(0, ENCODE_CHUNK_SIZE) : text;
                byte[] buf = encoding.GetBytes(subtext);
                ret += EncodeBuffer(buf);
                text = text.Substring(subtext.Length);
            }

            return ret;
        }

        /// <summary>
        /// Function will zip through the passed buffer and encode every 3 bits against the SYMCODE encoding standard.
        /// The returned string will be padded with '$' characters
        /// </summary>
        /// <param name="buf">BYTE array to be encoded. The length of the buffer should be evenly divisible by 3 to avoid padding</param>
        /// <returns>SYMCODE encoded string</returns>
        protected static string EncodeBuffer(byte[] buf)
        {
            const int BITSPERBYTE = 8;
            const int MASK_SIZE = 3;

            string ret = "";

            // important validation check
            if (null == buf || buf.Length > ENCODE_CHUNK_SIZE)
                throw new Exception("Invalid buffer passed");

            // quick check for anything to do
            if (buf.Length == 0)
                return "";

            // load the encoding buffer with the passed buffer values
            UInt32 encodingBuffer = 0;
            for (int x = 0; x < ENCODE_CHUNK_SIZE; ++x)
            {
                encodingBuffer += x < buf.Length ? buf[x] : (byte)0;
                encodingBuffer <<= BITSPERBYTE;
            }

            // zip through the encoding buffer and ... encode
            UInt32 mask = 0xe0000000;
            int encodeRunLength = (buf.Length * BITSPERBYTE) / MASK_SIZE + (buf.Length % MASK_SIZE == 0 ? 0 : 1);
            int totalRunLength = 8; /*ENCODE_CHUNK_SIZE * BITSPERBYTE / MASK_SIZE*/
            int runIdx = 0;
            for (; runIdx < encodeRunLength; ++runIdx)
            {
                UInt32 encIndex = encodingBuffer & mask;

                // move the encIndex to the right to convert to actual value
                encIndex >>= ((totalRunLength - runIdx) * MASK_SIZE) + (BITSPERBYTE - MASK_SIZE);
                ret += SYMCODE[(int)encIndex];

                // update the mask
                mask >>= MASK_SIZE;
            }

            // end by adding padding
            for (; runIdx < totalRunLength; ++runIdx)
                ret += "$";

            return ret;
        }
    }
}
