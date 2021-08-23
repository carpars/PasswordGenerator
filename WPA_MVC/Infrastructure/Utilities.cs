using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPA_MVC.Infrastructure
{
    public static class SelectList
    {
        public static List<SelectListItem> Build(Dictionary<object, string> items, string selectedValue, bool emptyOption, bool placeholderOption, bool noneOption, bool allOption)
        {
            List<SelectListItem> toReturn = new List<SelectListItem>();
            Dictionary<string, string> itemsKeyString = new Dictionary<string, string>();

            if (items != null && items.Count > 0)
            {
                var typeNameOfKey = items.First().Key.GetType().Name;

                if (typeNameOfKey != null)
                {
                    if (typeNameOfKey.ToLower().Equals("string"))
                    {
                        foreach (var item in items)
                        {
                            itemsKeyString.Add((string)item.Key, item.Value);
                        }
                    }
                    else if (typeNameOfKey.ToLower().Equals("int32"))
                    {
                        foreach (var item in items)
                        {
                            itemsKeyString.Add(item.Key.ToString(), item.Value);
                        }
                    }
                }

                foreach (var item in itemsKeyString)
                {
                    SelectListItem selectedListItem = new SelectListItem()
                    {
                        Value = item.Key,
                        Text = item.Value
                    };
                    toReturn.Add(selectedListItem);
                }

                if (!String.IsNullOrWhiteSpace(selectedValue))
                {
                    SelectListItem selectListItem = toReturn.Where(i => i.Value.Equals(selectedValue)).FirstOrDefault();
                    if (selectListItem != null)
                    {
                        selectListItem.Selected = true;
                    }
                }
            }

            if (noneOption)
            {
                toReturn.Prepend(new SelectListItem()
                {
                    Value = ((int)SelectListExtraItems.selectListExtraItems.noneOption).ToString(),
                    Text = SelectListExtraItems.Get()[(int)SelectListExtraItems.selectListExtraItems.noneOption]
                });
            }

            if (emptyOption)
            {
                toReturn.Prepend(new SelectListItem()
                {
                    Value = ((int)SelectListExtraItems.selectListExtraItems.emptyOption).ToString(),
                    Text = SelectListExtraItems.Get()[(int)SelectListExtraItems.selectListExtraItems.emptyOption]
                });
            }
            else if (placeholderOption)
            {
                toReturn.Prepend(new SelectListItem()
                {
                    Value = ((int)SelectListExtraItems.selectListExtraItems.placeholderOption).ToString(),
                    Text = SelectListExtraItems.Get()[(int)SelectListExtraItems.selectListExtraItems.placeholderOption]
                });
            }

            if (allOption)
            {
                toReturn.Prepend(new SelectListItem()
                {
                    Value = ((int)SelectListExtraItems.selectListExtraItems.noneOption).ToString(),
                    Text = SelectListExtraItems.Get()[(int)SelectListExtraItems.selectListExtraItems.allOption]
                });
            }


            return toReturn;
        }

    }

    public class SelectListItem
    {
        public string Value { get; set; }

        public string Text { get; set; }

        public bool Selected { get; set; }
    }

    public static class SelectListExtraItems
    {
        public enum selectListExtraItems
        {
            emptyOption = -4,
            placeholderOption = -3,
            noneOption = -2,
            allOption = -1
        }

        public static Dictionary<int, string> Get()
        {
            Dictionary<int, string> toReturn = new Dictionary<int, string>();

            foreach (selectListExtraItems item in Enum.GetValues(typeof(selectListExtraItems)))
            {
                switch (item)
                {
                    case selectListExtraItems.emptyOption:
                        toReturn.Add((int)selectListExtraItems.emptyOption, String.Empty);
                        break;

                    case selectListExtraItems.placeholderOption:
                        toReturn.Add((int)selectListExtraItems.placeholderOption, "SELECT");
                        break;

                    case selectListExtraItems.noneOption:
                        toReturn.Add((int)selectListExtraItems.noneOption, "None");
                        break;

                    case selectListExtraItems.allOption:
                        toReturn.Add((int)selectListExtraItems.allOption, "All");
                        break;

                    case 0:
                        break;
                }
            };

            return toReturn;
        }
    }

    public class Base32Encode
    {
        public static byte[] ToBytes(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException("input");
            }

            input = input.TrimEnd('='); //remove padding characters
            int byteCount = input.Length * 5 / 8; //this must be TRUNCATED
            byte[] returnArray = new byte[byteCount];

            byte curByte = 0, bitsRemaining = 8;
            int mask = 0, arrayIndex = 0;

            foreach (char c in input)
            {
                int cValue = CharToValue(c);

                if (bitsRemaining > 5)
                {
                    mask = cValue << (bitsRemaining - 5);
                    curByte = (byte)(curByte | mask);
                    bitsRemaining -= 5;
                }
                else
                {
                    mask = cValue >> (5 - bitsRemaining);
                    curByte = (byte)(curByte | mask);
                    returnArray[arrayIndex++] = curByte;
                    curByte = (byte)(cValue << (3 + bitsRemaining));
                    bitsRemaining += 3;
                }
            }

            //if we didn't end with a full byte
            if (arrayIndex != byteCount)
            {
                returnArray[arrayIndex] = curByte;
            }

            return returnArray;
        }

        public static string ToString(byte[] input)
        {
            if (input == null || input.Length == 0)
            {
                throw new ArgumentNullException("input");
            }

            int charCount = (int)Math.Ceiling(input.Length / 5d) * 8;
            char[] returnArray = new char[charCount];

            byte nextChar = 0, bitsRemaining = 5;
            int arrayIndex = 0;

            foreach (byte b in input)
            {
                nextChar = (byte)(nextChar | (b >> (8 - bitsRemaining)));
                returnArray[arrayIndex++] = ValueToChar(nextChar);

                if (bitsRemaining < 4)
                {
                    nextChar = (byte)((b >> (3 - bitsRemaining)) & 31);
                    returnArray[arrayIndex++] = ValueToChar(nextChar);
                    bitsRemaining += 5;
                }

                bitsRemaining -= 3;
                nextChar = (byte)((b << bitsRemaining) & 31);
            }

            //if we didn't end with a full char
            if (arrayIndex != charCount)
            {
                returnArray[arrayIndex++] = ValueToChar(nextChar);
                while (arrayIndex != charCount) returnArray[arrayIndex++] = '='; //padding
            }

            return new string(returnArray);
        }

        private static int CharToValue(char c)
        {
            int value = (int)c;

            // TODO see if there are a different way to handle non-base32 chars                                
            // If not a letter
            if (value < 65 || (value > 90 && value < 95) || value > 122)
            {
                // if it's a number
                if ((value > 47 && value < 50) || (value > 55 && value <= 57))
                {
                    char newC = '2';
                    value = CharToValue(newC);
                }
                // if it's neither a number
                else
                {
                    char newC = 'a';
                    value = CharToValue(newC);
                }
            }

            //65-90 == uppercase letters
            if (value < 91 && value > 64)
            {
                return value - 65;
            }
            //50-55 == numbers 2-7
            if (value < 56 && value > 49)
            {
                return value - 24;
            }
            //97-122 == lowercase letters
            if (value < 123 && value > 96)
            {
                return value - 97;
            }

            throw new ArgumentException("Character is not a Base32 character.", c.ToString());
        }

        private static char ValueToChar(byte b)
        {
            if (b < 26)
            {
                return (char)(b + 65);
            }

            if (b < 32)
            {
                return (char)(b + 24);
            }

            throw new ArgumentException("Byte is not a value Base32 value.", "b");
        }
    }

    public sealed class Base32Enc
    {

        // the valid chars for the encoding
        private static string ValidChars = "QAZ2WSX3" + "EDC4RFV5" + "TGB6YHN7" + "UJM8K9LP";

        /// <summary>
        /// Converts an array of bytes to a Base32-k string.
        /// </summary>
        public static string ToBase32String(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();         // holds the base32 chars
            byte index;
            int hi = 5;
            int currentByte = 0;

            while (currentByte < bytes.Length)
            {
                // do we need to use the next byte?
                if (hi > 8)
                {
                    // get the last piece from the current byte, shift it to the right
                    // and increment the byte counter
                    index = (byte)(bytes[currentByte++] >> (hi - 5));
                    if (currentByte != bytes.Length)
                    {
                        // if we are not at the end, get the first piece from
                        // the next byte, clear it and shift it to the left
                        index = (byte)(((byte)(bytes[currentByte] << (16 - hi)) >> 3) | index);
                    }

                    hi -= 3;
                }
                else if (hi == 8)
                {
                    index = (byte)(bytes[currentByte++] >> 3);
                    hi -= 3;
                }
                else
                {

                    // simply get the stuff from the current byte
                    index = (byte)((byte)(bytes[currentByte] << (8 - hi)) >> 3);
                    hi += 5;
                }

                sb.Append(ValidChars[index]);
            }

            return sb.ToString();
        }


        /// <summary>
        /// Converts a Base32-k string into an array of bytes.
        /// </summary>
        /// <exception cref="System.ArgumentException">
        /// Input string <paramref name="s">s</paramref> contains invalid Base32-k characters.
        /// </exception>
        public static byte[] FromBase32String(string str)
        {
            int numBytes = str.Length * 5 / 8;
            byte[] bytes = new Byte[numBytes];

            // all UPPERCASE chars
            str = str.ToUpper();

            int bit_buffer;
            int currentCharIndex;
            int bits_in_buffer;

            if (str.Length < 3)
            {
                bytes[0] = (byte)(ValidChars.IndexOf(str[0]) | ValidChars.IndexOf(str[1]) << 5);
                return bytes;
            }

            bit_buffer = (ValidChars.IndexOf(str[0]) | ValidChars.IndexOf(str[1]) << 5);
            bits_in_buffer = 10;
            currentCharIndex = 2;
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)bit_buffer;
                bit_buffer >>= 8;
                bits_in_buffer -= 8;
                while (bits_in_buffer < 8 && currentCharIndex < str.Length)
                {
                    bit_buffer |= ValidChars.IndexOf(str[currentCharIndex++]) << bits_in_buffer;
                    bits_in_buffer += 5;
                }
            }

            return bytes;
        }
    }
}

