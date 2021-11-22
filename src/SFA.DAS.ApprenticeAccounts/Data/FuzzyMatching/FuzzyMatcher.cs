using F23.StringSimilarity;
using NinjaNye.SearchExtensions.Soundex;
using System;
using System.Linq;

namespace SFA.DAS.ApprenticeCommitments.Data.FuzzyMatching
{
    public class FuzzyMatcher
    {
        public static readonly FuzzyMatcher AlwaysMatcher = new FuzzyMatcher(0);

        private readonly int _similarityThreshold;

        public FuzzyMatcher(int similarityThreshold)
            => _similarityThreshold = similarityThreshold;

        public bool IsSimilar(string string1, string string2)
        {
            if (string1 == string2)
            {
                return true;
            }

            var allCombinations =
                from a in SplitByWord(string1)
                from b in SplitByWord(string2)
                select (a, b);

            if (allCombinations.Any(x => GetSimilarity(x.a, x.b) >= _similarityThreshold))
                return true;

            if (string1.ToSoundex() == string2.ToSoundex())
                return true;

            return false;
        }

        private static string[] SplitByWord(string stringList)
            => stringList.Trim().Split(new char[] { ' ', '-' });

        public double GetSimilarity(string string1, string string2)
        {
            var l = new NormalizedLevenshtein();
            return Math.Ceiling(l.Similarity(string1.ToUpper(), string2.ToUpper()) * 100);
        }
    }
}