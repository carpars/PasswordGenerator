using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using WPA_MVC.Infrastructure;

namespace WPA_MVC.Infrastructure
{
    /*
  * Derived from https://github.com/google/google-authenticator-android/blob/master/AuthenticatorApp/src/main/java/com/google/android/apps/authenticator/Base32String.java
  * 
  * Copyright (C) 2016 BravoTango86
  *
  * Licensed under the Apache License, Version 2.0 (the "License");
  * you may not use this file except in compliance with the License.
  * You may obtain a copy of the License at
  *
  *      http://www.apache.org/licenses/LICENSE-2.0
  *
  * Unless required by applicable law or agreed to in writing, software
  * distributed under the License is distributed on an "AS IS" BASIS,
  * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
  * See the License for the specific language governing permissions and
  * limitations under the License.
  */


    public class BaseConversion
    {
        private static char[] DIGITS;
        private static char[] DIGITS32;
        private static char[] DIGITS64;
        private static char[] DIGITSHEX;
        private static char[] DIGITSUTF8;
        private static char[] DIGITSDEC;
        private static int MASK;
        private static int SHIFT;       
        private static Dictionary<char, int> CHAR_MAP = new Dictionary<char, int>();        
        private const string SEPARATOR = "-";
        private static bool isLower = false;

        private BaseConversion(Constants.Codifications codification)
        {            
            // A base32 char has 5 bits
            DIGITS32 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567".ToCharArray();
            // An hex char has 4 bits
            DIGITSHEX = "0123456789ABCDEF".ToCharArray();            
            // An UTF-8 char has 8 bits
            DIGITSUTF8 = "0123456789ABCDEF".ToCharArray();
            // An Dec char has 4 bits
            DIGITSDEC = "0123456789".ToCharArray();

            switch (codification)
            {
                case Constants.Codifications.Hex:
                    SetInitVariables(DIGITSHEX.Length, codification, DIGITSHEX);
                    break;

                case Constants.Codifications.Base64:
                    SetInitVariables(DIGITS64.Length, codification, DIGITS64);
                    break;

                case Constants.Codifications.Base32:                   
                    SetInitVariables(DIGITS32.Length, codification, DIGITS32);                                        
                    break;

                case Constants.Codifications.UTF8:
                    SetInitVariables(DIGITSUTF8.Length, codification, DIGITSUTF8);
                    break;
                
                case Constants.Codifications.Dec:
                    SetInitVariables(DIGITSDEC.Length, codification, DIGITSDEC);
                    break;

                default:
                    break;
            }
            
                      
        }

        private string CheckOperator()
        {
            // The << operator shifts its left-hand operand left by the number of bits defined by its right-hand operand

            // The following operators perform bitwise or shift operations with operands of the integral numeric types or the char type:
            //      Unary ~(bitwise complement) operator
            //      Binary << (left shift) and >> (right shift) shift operators
            //      Binary & (logical AND), | (logical OR), and ^ (logical exclusive OR) operators

            string toReturn = String.Empty;

            string whole = "d4ertyu3";
            int y;
            int index = whole.Length - 1;
            int offset = 1;
            double charCode;
            toReturn+="Hello World; initial string: " + whole;
            for (int i = index; i >= 0;)
            {
                char thisChar = whole[i];
                i -= offset;
                y = i << offset;
                charCode = Char.GetNumericValue(whole[index]);

                toReturn += "\n\r" + "position=" + (i + 1) + ", thisChar=" + thisChar + ", y=" + y.ToString() + ", charCode=" + charCode.ToString();
            }
            return toReturn;
        }

        private static void SetInitVariables(int digitsLength, Constants.Codifications codification, char[] digits)
        {
            MASK = digitsLength;
            SHIFT = numberOfTrailingZeros(digitsLength, codification);
            for (int i = 0; i < digitsLength; i++) CHAR_MAP[digits[i]] = i;
        }

        private static int numberOfTrailingZeros(int i, Constants.Codifications codification)
        {
            // HD, Figure 5-14
            int y;
            if (i == 0) return 32;
            int n = 31;
            y = i << 16; if (y != 0) { n = n - 16; i = y; }
            y = i << 8; if (y != 0) { n = n - 8; i = y; }
            y = i << 4; if (y != 0) { n = n - 4; i = y; }
            y = i << 2; if (y != 0) { n = n - 2; i = y; }
            return n - (int)((uint)(i << 1) >> 31);
        }        

        public static byte[] Decode(string encoded)
        {
            try
            {
                // Remove whitespace and separators
                encoded = encoded.Trim().Replace(SEPARATOR, "");

                // Remove padding. Note: the padding is used as hint to determine how many
                // bits to decode from the last incomplete chunk (which is commented out
                // below, so this may have been wrong to start with).
                encoded = Regex.Replace(encoded, "[=]*$", "");

                string encodedBase32 = GetEncodedBase32(encoded);

                // Canonicalize to all upper case
                //encoded = encoded.ToUpper();
                if (encoded.Length == 0)
                {
                    return new byte[0];
                }
                int encodedLength = encoded.Length;
                int outLength = encodedLength * SHIFT / 8;
                byte[] result = new byte[outLength];
                int buffer = 0;
                int next = 0;
                int bitsLeft = 0;
                foreach (char c in encoded.ToCharArray())
                {
                    char cValue = c;
                    isLower = Char.IsLower(cValue);
                    // Canonicalize to all upper case
                    cValue = cValue.ToString().ToUpper()[0];
                    if (!CHAR_MAP32.ContainsKey(cValue))
                    {
                        throw new DecodingException("Illegal character: " + c);
                    }
                    buffer <<= SHIFT;
                    buffer |= CHAR_MAP32[cValue] & MASK;
                    bitsLeft += SHIFT;
                    if (bitsLeft >= 8)
                    {
                        result[next++] = (byte)(buffer >> (bitsLeft - 8));
                        bitsLeft -= 8;
                    }
                }
                // We'll ignore leftover bits for now.
                //
                // if (next != outLength || bitsLeft >= SHIFT) {
                //  throw new DecodingException("Bits left: " + bitsLeft);
                // }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static string GetEncodedBase32(string input)
        {
            string toReturn = String.Empty;

            foreach (char c in input.ToCharArray())
            {
                char cValue = c;
                isLower = Char.IsLower(cValue);
                // Canonicalize to all upper case
                cValue = cValue.ToString().ToUpper()[0];
                Random rnd = new Random();
                int nextRnd = rnd.Next(0, CHAR_MAP32.Count - 1);
                cValue = CHAR_MAP32.Where(cm => cm.Value == nextRnd).FirstOrDefault().Key.ToString()[0];
                toReturn.Append(cValue);
            }
            return toReturn;
        }

        public static string Encode(byte[] data, bool padOutput = false)
        {
            if (data.Length == 0)
            {
                return "";
            }

            // SHIFT is the number of bits per output character, so the length of the
            // output is the length of the input multiplied by 8/SHIFT, rounded up.
            if (data.Length >= (1 << 28))
            {
                // The computation below will fail, so don't do it.
                throw new ArgumentOutOfRangeException("data");
            }

            int outputLength = (data.Length * 8 + SHIFT - 1) / SHIFT;
            StringBuilder result = new StringBuilder(outputLength);

            int buffer = data[0];
            int next = 1;
            int bitsLeft = 8;
            while (bitsLeft > 0 || next < data.Length)
            {
                if (bitsLeft < SHIFT)
                {
                    if (next < data.Length)
                    {
                        buffer <<= 8;
                        buffer |= (data[next++] & 0xff);
                        bitsLeft += 8;
                    }
                    else
                    {
                        int pad = SHIFT - bitsLeft;
                        buffer <<= pad;
                        bitsLeft += pad;
                    }
                }
                int index = MASK & (buffer >> (bitsLeft - SHIFT));
                bitsLeft -= SHIFT;
                // TODO This is a CJP workaround for having lowercase letters too. This has to be made in the Decode() method, and in a different way
                char toAppend = DIGITS32[index];
                if (Char.IsLetter(toAppend))
                {
                    Random rnd = new Random();
                    int nextRnd = rnd.Next(0, 5);
                    bool isOdd = (nextRnd % 2) > 0;
                    toAppend = isOdd ? toAppend : toAppend.ToString().ToLower()[0];
                }
                result.Append(toAppend);
            }
            if (padOutput)
            {
                int padding = 8 - (result.Length % 8);
                if (padding > 0) result.Append(new string('=', padding == 8 ? 0 : padding));
            }

            return result.ToString();
        }

        private string ConvertToHexString(byte[] data, bool padOutput)
        {
            string toReturn;

            if (data.Length == 0)
            {
                return "";
            }

            // SHIFT is the number of bits per output character, so the length of the
            // output is the length of the input multiplied by 8/SHIFT, rounded up.
            if (data.Length >= (1 << 28))
            {
                // The computation below will fail, so don't do it.
                throw new ArgumentOutOfRangeException("data");
            }

            int outputLength = (data.Length * 8 + SHIFTHEX - 1) / SHIFTHEX;
            StringBuilder result = new StringBuilder(outputLength);

            int buffer = data[0];
            int next = 1;
            int bitsLeft = 8;
            while (bitsLeft > 0 || next < data.Length)
            {
                if (bitsLeft < SHIFTHEX)
                {
                    if (next < data.Length)
                    {
                        buffer <<= 8;
                        buffer |= (data[next++] & 0xff);
                        bitsLeft += 8;
                    }
                    else
                    {
                        int pad = SHIFTHEX - bitsLeft;
                        buffer <<= pad;
                        bitsLeft += pad;
                    }
                }
                int index = MASK & (buffer >> (bitsLeft - SHIFTHEX));
                bitsLeft -= SHIFTHEX;
                // TODO This is a CJP workaround for having lowercase letters too. This has to be made in the Decode() method, and in a different way
                char toAppend = DIGITS32[index];
                if (Char.IsLetter(toAppend))
                {
                    Random rnd = new Random();
                    int nextRnd = rnd.Next(0, 5);
                    bool isOdd = (nextRnd % 2) > 0;
                    toAppend = isOdd ? toAppend : toAppend.ToString().ToLower()[0];
                }
                result.Append(toAppend);
            }
            if (padOutput)
            {
                int padding = 8 - (result.Length % 8);
                if (padding > 0) result.Append(new string('=', padding == 8 ? 0 : padding));
            }

            return result.ToString();
        }

        private class DecodingException : Exception
        {
            public DecodingException(string message) : base(message)
            {
            }
        }
    }
}
