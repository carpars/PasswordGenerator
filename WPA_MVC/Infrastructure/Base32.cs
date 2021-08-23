﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

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


    public static class Base32
    {

        private static readonly char[] DIGITS;        
        private static readonly int MASK;
        private static readonly int SHIFT;
        private static Dictionary<char, int> CHAR_MAP = new Dictionary<char, int>();
        private const string SEPARATOR = "-";
        private static bool isLower = false;

        static Base32()
        {
            DIGITS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567".ToCharArray();            
            MASK = DIGITS.Length - 1;
            SHIFT = numberOfTrailingZeros(DIGITS.Length);
            for (int i = 0; i < DIGITS.Length; i++) CHAR_MAP[DIGITS[i]] = i;
        }

        private static int numberOfTrailingZeros(int i)
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
                    if (!CHAR_MAP.ContainsKey(cValue))
                    {
                        // TODO CJP: WOrkaround made by me: replace for a legal char. This shouldn't be done, since it's expected to receive a valid base32 string, and to throw an Except if not                                                
                        // The illegarl char is replaced by a valid random char
                        Random rnd = new Random();
                        int nextRnd = rnd.Next(0, CHAR_MAP.Count - 1);
                        cValue = CHAR_MAP.Where(cm => cm.Value == nextRnd).FirstOrDefault().Key.ToString()[0];

                        //throw new DecodingException("Illegal character: " + c);
                    }
                    buffer <<= SHIFT;
                    buffer |= CHAR_MAP[cValue] & MASK;
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
                char toAppend = DIGITS[index];
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
