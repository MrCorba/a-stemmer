using System;
using System.Collections.Generic;
using System.Text;
using Annytab.Stemmer;

namespace Annytab.Stemmer
{
    public class TestStemmer : Stemmer
    {
        private List<char> Vowels_WX;
        private List<char> AOU;
        private List<char> AIOU;

        public TestStemmer() : base()
        {
            this.vowels = new char[] { 'a', 'e', 'i', 'o', 'u', 'y' };
            this.Vowels_WX = new List<char> { 'a', 'e', 'i', 'o', 'u', 'y', 'w', 'x' };
            this.AOU = new List<char> { 'a', 'o', 'u' };
            this.AIOU = new List<char> { 'a', 'i', 'o', 'u' };
        }

        public override string[] GetSteamWords(string[] words)
        {
            throw new NotImplementedException();
        }

        public override string GetSteamWord(string word)
        {
            bool stemmed = false;

            word = word.ToLowerInvariant();
            char[] chars = word.ToCharArray();

            for (int i = 0; i < word.Length; i++)
            {
                if (chars[i] == 'y')
                    chars[i] = 'Y';
            }

            word = new string(chars);

            //measure
            Int32[] partIndexR = CalculateR1R2(chars);
            string strR1 = partIndexR[0] < word.Length ? word.Substring(partIndexR[0]) : "";
            string strR2 = partIndexR[1] < word.Length ? word.Substring(partIndexR[1]) : "";

            //Step 1
            if (word.EndsWith("'s"))
            {
                word = word.Remove(word.Length - 2);
                stemmed = true;
            }
            else if (word.EndsWith("ies"))
            {
                word = word.Remove(word.Length - 3);
                word += "ie";
                stemmed = true;
            }
            else if (word.EndsWith("es"))
            {
                if (strR1.EndsWith("ar"))
                {
                    word = word.Remove(word.Length - 4);
                    word = this.Lengthen_V(word);
                    stemmed = true;
                }
                else if (strR1.EndsWith("er"))
                {
                    word = word.Remove(word.Length - 4);
                    stemmed = true;
                }
                else
                {
                    if (IsNotVowelOrIJ(word, 3))
                    {
                        word = word.Remove(word.Length - 1);
                        stemmed = true;
                    }
                }
            }
            else if (word.EndsWith("aus"))
            {
                word = word.Remove(word.Length - 1);
                stemmed = true;
            }
            else if (word.EndsWith("s"))
            {
                if (word[word.Length - 2] != 't' && IsNotVowelOrIJ(word, 2))
                {
                    word = word.Remove(word.Length - 1);
                    stemmed = true;
                }
            }
            else if (word.EndsWith("en"))
            {
                if (strR1.EndsWith("heden"))
                {
                    word = word.Remove(word.Length - 5);
                    word += "heid";
                    stemmed = true;
                }
                else if (word.EndsWith("nden"))
                {
                    word = word.Remove(word.Length - 2);
                    stemmed = true;
                }
                else if (word.EndsWith("den") && word.Length > 3 && IsNotVowelOrIJ(word, 4) && IsConsonant(word[word.Length - 4]))
                {
                    word = word.Remove(word.Length - 3);
                    stemmed = true;
                }
                else if (word.EndsWith("ien") || word.EndsWith("jen"))
                {
                    if (IsVowel(word[word.Length - 4]))
                    {
                        word = word.Remove(word.Length - 2);
                        stemmed = true;
                    }
                }
                else
                {
                    if (strR1.Length < 3 || IsConsonant(strR1[strR1.Length - 3]))
                    {
                        word = word.Remove(word.Length - 2);
                        word = this.Lengthen_V(word);
                        stemmed = true;
                    }
                }
            }
            else if (word.EndsWith("nde"))
            {
                word = word.Remove(word.Length - 1);
                stemmed = true;
            }

            //Step 2
            strR1 = partIndexR[0] < word.Length ? word.Substring(partIndexR[0]) : "";
            strR2 = partIndexR[1] < word.Length ? word.Substring(partIndexR[1]) : "";

            if (word.EndsWith("je"))
            {
                if (word.EndsWith("'tje") || word.EndsWith("etje"))
                {
                    word = word.Remove(word.Length - 4);
                    stemmed = true;
                }
                else if (word.EndsWith("tje") || word.EndsWith("'je"))
                {
                    word = word.Remove(word.Length - 3);
                    stemmed = true;
                }
                else if (word.EndsWith("rnt") || word.EndsWith("mp"))
                {
                    word = word.Remove(word.Length - 1);
                    stemmed = true;
                }
                else if (word.EndsWith("ink"))
                {
                    word = word.Remove(word.Length - 3);
                    word += "ing";
                    stemmed = true;
                }
                else
                {
                    word = word.Remove(word.Length - 2);
                    stemmed = true;
                }
            }
            else if (word.EndsWith("e"))
            {
                if (word.EndsWith("ge") || word.EndsWith("lijke") || word.EndsWith("ische") || word.EndsWith("te") || word.EndsWith("se") || word.EndsWith("re"))
                {
                    word = word.Remove(word.Length - 1);
                    stemmed = true;
                }
                else if (word.EndsWith("de"))
                {
                    if (word.Length > 2 && IsConsonant(word[word.Length - 3]))
                    {
                        word = word.Remove(word.Length - 2);
                        stemmed = true;
                    }
                }
                else if (word.EndsWith("le") || word.EndsWith("ene"))
                {
                    word = word.Remove(word.Length - 1);
                    word = this.Lengthen_V(word);
                    stemmed = true;
                }
                else if (word.EndsWith("ieve"))
                {
                    word = word.Remove(word.Length - 4);
                    word += "ief";
                    stemmed = true;
                }
            }

            //Step 3
            strR1 = partIndexR[0] < word.Length ? word.Substring(partIndexR[0]) : "";
            strR2 = partIndexR[1] < word.Length ? word.Substring(partIndexR[1]) : "";

            if (word.EndsWith("atie"))
            {
                word = word.Remove(word.Length - 4);
                word += "eer";
                stemmed = true;
            }
            else if (word.EndsWith("iteit"))
            {
                word = word.Remove(word.Length - 5);
                word = this.Lengthen_V(word);
                stemmed = true;
            }
            else if (word.EndsWith("isme") || word.EndsWith("erij"))
            {
                word = word.Remove(word.Length - 4);
                word = this.Lengthen_V(word);
                stemmed = true;
            }
            else if (word.EndsWith("ing"))
            {
                word = word.Remove(word.Length - 3);
                word = this.Lengthen_V(word);
                stemmed = true;
            }
            else if (word.EndsWith("heid") || word.EndsWith("ster"))
            {
                word = word.Remove(word.Length - 4);
                stemmed = true;
            }
            else if (word.EndsWith("sel"))
            {
                word = word.Remove(word.Length - 3);
                stemmed = true;
            }
            else if (word.EndsWith("rder"))
            {
                word = word.Remove(word.Length - 3);
                stemmed = true;
            }
            else if (word.EndsWith("arij"))
            {
                word = word.Remove(word.Length - 4);
                word += "aar";
                stemmed = true;
            }
            else if (word.EndsWith("fie") || word.EndsWith("gie"))
            {
                word = word.Remove(word.Length - 2);
                word = this.Lengthen_V(word);
                stemmed = true;
            }
            else if (word.EndsWith("tst") || word.EndsWith("dst"))
            {
                word = word.Remove(word.Length - 2);
                stemmed = true;
            }

            // Step 4
            strR1 = partIndexR[0] < word.Length ? word.Substring(partIndexR[0]) : "";
            strR2 = partIndexR[1] < word.Length ? word.Substring(partIndexR[1]) : "";

            if (word.EndsWith("ioneel"))
            {
                word = word.Remove(word.Length - 6);
                word += "ie";
                stemmed = true;
            }
            else if (word.EndsWith("atief"))
            {
                word = word.Remove(word.Length - 5);
                word += "eer";
                stemmed = true;
            }
            else if (word.EndsWith("achtiger") || word.EndsWith("achtigst"))
            {
                word = word.Remove(word.Length - 8);
                stemmed = true;
            }
            else if (word.EndsWith("achtig") && word.Length > 6)
            {
                word = word.Remove(word.Length - 6);
                stemmed = true;
            }
            else if (word.EndsWith("baar"))
            {
                word = word.Remove(word.Length - 4);
                stemmed = true;
            }
            else if (word.EndsWith("naar") || word.EndsWith("raar") || word.EndsWith("laar"))
            {
                word = word.Remove(word.Length - 3);
                stemmed = true;
            }
            else if (word.EndsWith("tant"))
            {
                word = word.Remove(word.Length - 4);
                word += "teer";
                stemmed = true;
            }
            else if (word.EndsWith("lijker") || word.EndsWith("lijkst"))
            {
                word = word.Remove(word.Length - 2);
                stemmed = true;
            }
            else if (word.EndsWith("eriger") || word.EndsWith("erigst"))
            {
                word = word.Remove(word.Length - 6);
                word = this.Lengthen_V(word);
                stemmed = true;
            }
            else if (word.EndsWith("erig") || word.EndsWith("iger") || word.EndsWith("igst"))
            {
                word = word.Remove(word.Length - 4);
                word = this.Lengthen_V(word);
                stemmed = true;
            }
            else if (word.EndsWith("end"))
            {
                if (word.EndsWith("end") && word.Length > 3 && IsConsonant(word[word.Length - 4]))
                {
                    word = word.Remove(word.Length - 3);
                    word = this.Lengthen_V(word);
                    stemmed = true;
                }
            }
            else if (word.EndsWith("ig"))
            {
                if (strR1.EndsWith("ig") && IsConsonant(strR1[strR1.Length - 3]))
                {
                    word = word.Remove(word.Length - 2);
                    word = this.Lengthen_V(word);
                    stemmed = true;
                }
            }

            bool ge_removed = false;

            // Remove GE prefix
            if (word.Length > 3 && word.StartsWith("ge"))
            {
                word = word.Remove(0, 2);
                ge_removed = true;
            }

            // measure
            partIndexR = CalculateR1R2(chars);
            strR1 = partIndexR[0] < word.Length ? word.Substring(partIndexR[0]) : "";
            strR2 = partIndexR[1] < word.Length ? word.Substring(partIndexR[1]) : "";

            // Step 1c
            if (ge_removed)
            {
                if (word.EndsWith("d") && word.Length > 1)
                {
                    if (IsConsonant(word[word.Length - 2]) && word[word.Length - 2] != 'n')
                    {
                        word = word.Remove(word.Length - 1);
                    }
                }
                else if (word.EndsWith("t") && word.Length > 1)
                {
                    if (IsConsonant(word[word.Length - 2]) && word[word.Length - 2] != 'h')
                    {
                        word = word.Remove(word.Length - 1);
                    }
                }
            }

            ge_removed = false;

            // lose infix
            int index = word.IndexOf("ge");
            if (index > -1)
            {
                if (word.Length > index + 4)
                {
                    word = word.Remove(index, 2);
                    ge_removed = true;
                }
            }

            // measure
            partIndexR = CalculateR1R2(chars);
            strR1 = partIndexR[0] < word.Length ? word.Substring(partIndexR[0]) : "";
            strR2 = partIndexR[1] < word.Length ? word.Substring(partIndexR[1]) : "";

            // Step 1c
            if (ge_removed)
            {
                if (word.EndsWith("d") && word.Length > 1)
                {
                    if (IsConsonant(word[word.Length - 2]) && word[word.Length - 2] != 'n')
                    {
                        word = word.Remove(word.Length - 1);
                    }
                }
                else if (word.EndsWith("t") && word.Length > 1)
                {
                    if (IsConsonant(word[word.Length - 2]) && word[word.Length - 2] != 'h')
                    {
                        word = word.Remove(word.Length - 1);
                    }
                }
            }

            //Step 7
            strR1 = partIndexR[0] < word.Length ? word.Substring(partIndexR[0]) : "";
            strR2 = partIndexR[1] < word.Length ? word.Substring(partIndexR[1]) : "";

            if (word.EndsWith("kt") || word.EndsWith("ft") || word.EndsWith("pt"))
            {
                word = word.Remove(word.Length - 1);
                stemmed = true;
            }

            // Step 6
            if ((stemmed || ge_removed) && word.Length > 2)
            {
                strR1 = partIndexR[0] < word.Length ? word.Substring(partIndexR[0]) : "";
                strR2 = partIndexR[1] < word.Length ? word.Substring(partIndexR[1]) : "";

                if (IsConsonant(word[word.Length - 1]) && (word[word.Length - 1] == word[word.Length - 2]))
                {
                    word = word.Remove(word.Length - 1);
                }
                else if (word.EndsWith("v"))
                {
                    word = word.Remove(word.Length - 1);
                    word += "f";
                }
                else if (word.EndsWith("z"))
                {
                    word = word.Remove(word.Length - 1);
                    word += "s";
                }
            }

            return word.ToLowerInvariant();
        }

        private string Lengthen_V(string word)
        {
            int length = word.Length;

            if (length < 2)
            {
                return word;
            }

            bool doubleVowel = false;

            if (!Vowels_WX.Contains(word[length - 1]))
            {
                if (AOU.Contains(word[length - 2]))
                {
                    doubleVowel = length < 3 || IsConsonant(word[length - 3]);
                }
                else if (word[length - 2] == 'e')
                {
                    doubleVowel = length > 4 && IsConsonant(word[length - 3]);
                    doubleVowel = doubleVowel && !AIOU.Contains(word[length - 4]) && !(length > 6 && AIOU.Contains(word[length - 5]) && IsConsonant(word[length - 6]));
                }
            }

            if (doubleVowel)
            {
                word = word.Remove(word.Length - 1) + word[word.Length - 2] + word[word.Length - 1];
            }

            return word;
        }

        private Int32[] CalculateR1R2(char[] characters)
        {
            // Create the int array to return
            Int32 r1 = characters.Length;
            Int32 r2 = characters.Length;

            // Calculate R1
            for (int i = 1; i < characters.Length; i++)
            {
                if (IsConsonant(characters[i]) && IsVowel(characters[i - 1]))
                {
                    // Set the r1 index
                    r1 = i + 1;
                    break;
                }
            }

            // Calculate R2
            for (int i = r1; i < characters.Length; ++i)
            {
                if (IsConsonant(characters[i]) && IsVowel(characters[i - 1]))
                {
                    // Set the r2 index
                    r2 = i + 1;
                    break;
                }
            }

            // Adjust R1
            if (r1 < 3)
            {
                r1 = 3;
            }

            // Return the int array
            return new Int32[] { r1, r2 };

        } // End of the calculateR1R2 method

        public bool IsNotVowelOrIJ(string word, int reverseIndex)
        {
            int index = word.Length - reverseIndex;

            if (IsVowel(word[index]))
            {
                return false;
            }

            return !(word[index] == 'j' && word[index - 1] == 'i');
        }
    }
}
