using System.Collections.Generic;

namespace Lucy
{
    public static class StringHelper
    {
        public static string CleanString(string input)
        {
            string clean = string.Empty;
            clean = TrimAndRemovePunctuation(input);

            return clean.Trim();
        }

        public static string[] CleanLines(string[] fileLines)
        {
            List<string> cleanLines = new List<string>();

            foreach (string line in fileLines)
                if (!string.IsNullOrEmpty(line))
                {
                    string trimmedLine = line.Trim();
                    if (trimmedLine[0] != '#')
                        cleanLines.Add(trimmedLine);
                }

            return cleanLines.ToArray();
        }

        public static string InsertSpacesBAF(string input)
        {
            string str = input.Insert(0, " ");
            return str.Insert(str.Length, " ");
        }

        private static string TrimAndRemovePunctuation(string input)
        {
            string trimmed = string.Empty;
            string upperInput = input.Trim().ToUpper();

            foreach (char chr in upperInput)
                if (!char.IsPunctuation(chr))
                    trimmed += chr.ToString();

            return trimmed;
        }
    }
}
