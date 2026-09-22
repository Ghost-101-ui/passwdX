using System;
using System.Linq;
using PasswordX.Models;

namespace PasswordX.Services
{
    public class ScorecardService
    {
        private readonly PatternDetector _patternDetector = new();

        public ScorecardResult Evaluate(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return new ScorecardResult();
            }

            int len = password.Length;
            bool hasUpper = password.Any(char.IsUpper);
            bool hasLower = password.Any(char.IsLower);
            bool hasNumber = password.Any(char.IsDigit);
            bool hasSymbol = password.Any(c => !char.IsLetterOrDigit(c));

            int compTypes = (hasUpper ? 1 : 0) + (hasLower ? 1 : 0) + (hasNumber ? 1 : 0) + (hasSymbol ? 1 : 0);
            int uniqueChars = password.Distinct().Count();
            double randRatio = (double)uniqueChars / len;

            var patterns = _patternDetector.DetectPatterns(password);
            bool hasDict = patterns.ChecklistItems.Any(i => i.Category == "Dictionary" && i.Detected);
            int patCount = patterns.TotalPatternsDetected;

            double charset = (hasUpper ? 26 : 0) + (hasLower ? 26 : 0) + (hasNumber ? 10 : 0) + (hasSymbol ? 32 : 0);
            if (charset == 0) charset = 26;
            double entropy = len * Math.Log2(charset);

            int lenScore = len >= 16 ? 95 : (len >= 12 ? 85 : (len >= 8 ? 65 : 30));
            int compScore = compTypes >= 4 ? 95 : (compTypes == 3 ? 75 : (compTypes == 2 ? 55 : 30));
            int randScore = randRatio > 0.8 ? 95 : (randRatio > 0.6 ? 75 : (randRatio > 0.4 ? 55 : 35));
            int patScore = patCount == 0 ? 95 : (patCount == 1 ? 70 : (patCount == 2 ? 50 : 25));
            int dictScore = !hasDict ? 95 : 30;
            int entScore = entropy >= 80 ? 95 : (entropy >= 60 ? 80 : (entropy >= 40 ? 60 : 35));

            int overall = (lenScore + compScore + randScore + patScore + dictScore + entScore) / 6;

            return new ScorecardResult
            {
                LengthGrade = ConvertToGrade(lenScore),
                ComplexityGrade = ConvertToGrade(compScore),
                RandomnessGrade = ConvertToGrade(randScore),
                PatternsGrade = ConvertToGrade(patScore),
                DictionaryGrade = ConvertToGrade(dictScore),
                EntropyGrade = ConvertToGrade(entScore),
                OverallGrade = ConvertToGrade(overall)
            };
        }

        private string ConvertToGrade(int score)
        {
            if (score >= 95) return "A+";
            if (score >= 88) return "A";
            if (score >= 80) return "B+";
            if (score >= 70) return "B";
            if (score >= 60) return "C";
            if (score >= 45) return "D";
            return "F";
        }
    }
}
