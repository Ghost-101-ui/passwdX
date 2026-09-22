using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PasswordX.Models;

namespace PasswordX.Services
{
    public class PasswordChecker
    {
        private readonly PatternDetector _patternDetector = new();
        private readonly MemeService _memeService = new();

        public AnalysisResult Analyze(string password, string personalInfo = "")
        {
            if (string.IsNullOrEmpty(password))
            {
                return new AnalysisResult
                {
                    Score = 0,
                    StrengthLabel = "Weak",
                    Suggestions = new List<string> { "Please enter a password to analyze." },
                    Meme = _memeService.GetMeme("Weak")
                };
            }

            int score = 0;
            var suggestions = new List<string>();

            int length = password.Length;
            bool hasUpper = password.Any(char.IsUpper);
            bool hasLower = password.Any(char.IsLower);
            bool hasNumber = password.Any(char.IsDigit);
            bool hasSymbol = password.Any(c => !char.IsLetterOrDigit(c));

            // Length Score (up to 35 points)
            if (length >= 16) score += 35;
            else if (length >= 12) score += 25;
            else if (length >= 8) score += 15;
            else score += 5;

            // Character Diversity (up to 35 points)
            if (hasUpper) score += 8;
            if (hasLower) score += 7;
            if (hasNumber) score += 10;
            if (hasSymbol) score += 10;

            // Entropy & Randomness bonus (up to 15 points)
            int uniqueChars = password.Distinct().Count();
            double uniqueRatio = (double)uniqueChars / length;
            if (uniqueRatio > 0.7) score += 15;
            else if (uniqueRatio > 0.5) score += 8;

            // Pattern & Dictionary Deductions (up to -40 points)
            var patternResult = _patternDetector.DetectPatterns(password);
            var dictWords = new List<string>();
            var kbPatterns = new List<string>();
            var repeatedChars = new List<string>();
            var seqChars = new List<string>();

            foreach (var item in patternResult.ChecklistItems)
            {
                if (item.Detected)
                {
                    if (item.Category == "Dictionary" || item.Category == "Substitutions")
                    {
                        score -= 15;
                        dictWords.Add(item.Description);
                    }
                    else if (item.Category == "Sequential")
                    {
                        score -= 10;
                        if (item.Name.Contains("Keyboard")) kbPatterns.Add(item.Description);
                        else seqChars.Add(item.Description);
                    }
                    else if (item.Category == "Repetition")
                    {
                        score -= 10;
                        repeatedChars.Add(item.Description);
                    }
                }
            }

            // Personal Info Check
            bool personalInfoFound = false;
            if (!string.IsNullOrWhiteSpace(personalInfo))
            {
                var terms = personalInfo.Split(new[] { ' ', ',', ';', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var term in terms)
                {
                    if (term.Length >= 3 && password.ToLower().Contains(term.ToLower()))
                    {
                        personalInfoFound = true;
                        score -= 20;
                        suggestions.Add($"Avoid using personal information ('{term}') in your password.");
                    }
                }
            }

            // Clamp score between 0 and 100
            score = Math.Clamp(score, 0, 100);

            // Strength Label
            string label;
            string memeCategory;
            if (score < 40)
            {
                label = "Weak";
                memeCategory = dictWords.Any() ? "Common" : "Weak";
            }
            else if (score < 70)
            {
                label = "Moderate";
                memeCategory = "Moderate";
            }
            else if (score < 90)
            {
                label = "Strong";
                memeCategory = "Strong";
            }
            else
            {
                label = "Extremely Strong";
                memeCategory = "Strong";
            }

            // Construct Actionable Suggestions
            if (length < 12) suggestions.Add("Increase password length to at least 12–16 characters.");
            if (!hasUpper) suggestions.Add("Include at least one uppercase letter (A-Z).");
            if (!hasLower) suggestions.Add("Include at least one lowercase letter (a-z).");
            if (!hasNumber) suggestions.Add("Include at least one numeric digit (0-9).");
            if (!hasSymbol) suggestions.Add("Include at least one special symbol (!@#$%^&*).");
            if (patternResult.TotalPatternsDetected > 0) suggestions.Add("Remove predictable keyboard sequences and character repetitions.");

            return new AnalysisResult
            {
                Score = score,
                StrengthLabel = label,
                Suggestions = suggestions,
                Length = length,
                HasUpper = hasUpper,
                HasLower = hasLower,
                HasNumber = hasNumber,
                HasSymbol = hasSymbol,
                DictionaryWords = dictWords,
                KeyboardPatterns = kbPatterns,
                RepeatedChars = repeatedChars,
                SequentialChars = seqChars,
                PersonalInfoFound = personalInfoFound,
                Meme = _memeService.GetMeme(memeCategory)
            };
        }
    }
}
