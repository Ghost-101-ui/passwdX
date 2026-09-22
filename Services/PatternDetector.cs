using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using PasswordX.Models;

namespace PasswordX.Services
{
    public class PatternDetector
    {
        private static readonly string[] KeyboardWalks = new[]
        {
            "qwerty", "asdfgh", "zxcvbn", "123456", "234567", "345678", "456789", "567890",
            "qazwsx", "edcrfv", "tgbyhn", "ujmkol", "abcdef", "qwertyuiop", "asdfghjkl"
        };

        private static readonly string[] PredictableWords = new[]
        {
            "password", "admin", "welcome", "master", "login", "iloveyou", "monkey",
            "dragon", "superman", "secret", "pass123", "letmein", "sunshine", "football"
        };

        public PatternResult DetectPatterns(string password)
        {
            var checklist = new List<PatternItem>();
            int totalDetected = 0;

            if (string.IsNullOrEmpty(password))
            {
                return new PatternResult { ChecklistItems = checklist, TotalPatternsDetected = 0 };
            }

            string lower = password.ToLower();

            // 1. Keyboard Walks
            bool foundKeyboard = false;
            string foundKw = "";
            foreach (var kw in KeyboardWalks)
            {
                if (lower.Contains(kw))
                {
                    foundKeyboard = true;
                    foundKw = kw;
                    break;
                }
            }
            if (foundKeyboard) totalDetected++;
            checklist.Add(new PatternItem
            {
                Name = "Keyboard Walks / Sequences",
                Category = "Sequential",
                Detected = foundKeyboard,
                Description = foundKeyboard ? $"Contains sequential keyboard pattern ('{foundKw}')" : "No obvious sequential keyboard patterns detected."
            });

            // 2. Repeated Characters
            bool foundRepeated = Regex.IsMatch(password, @"(.)\1{2,}");
            if (foundRepeated) totalDetected++;
            checklist.Add(new PatternItem
            {
                Name = "Repeated Characters (e.g. 'aaaa', '1111')",
                Category = "Repetition",
                Detected = foundRepeated,
                Description = foundRepeated ? "Contains 3 or more identical consecutive characters." : "No excessive character repetition detected."
            });

            // 3. Predictable Common Words
            bool foundPredictable = false;
            string foundWord = "";
            foreach (var word in PredictableWords)
            {
                if (lower.Contains(word))
                {
                    foundPredictable = true;
                    foundWord = word;
                    break;
                }
            }
            if (foundPredictable) totalDetected++;
            checklist.Add(new PatternItem
            {
                Name = "Common Predictable Words",
                Category = "Dictionary",
                Detected = foundPredictable,
                Description = foundPredictable ? $"Contains dictionary term ('{foundWord}')" : "No common dictionary words detected."
            });

            // 4. L33t / Simple Substitutions (e.g., P@ssword, pa$$word, 4dm1n, w3lc0m3)
            string deLeeted = lower
                .Replace("@", "a")
                .Replace("$", "s")
                .Replace("0", "o")
                .Replace("1", "i")
                .Replace("3", "e")
                .Replace("!", "i")
                .Replace("5", "s");

            bool foundLeet = false;
            string foundLeetWord = "";
            if (deLeeted != lower)
            {
                foreach (var word in PredictableWords)
                {
                    if (deLeeted.Contains(word))
                    {
                        foundLeet = true;
                        foundLeetWord = word;
                        break;
                    }
                }
            }
            if (foundLeet) totalDetected++;
            checklist.Add(new PatternItem
            {
                Name = "Simple L33t Substitutions (e.g. 'P@ssword')",
                Category = "Substitutions",
                Detected = foundLeet,
                Description = foundLeet ? $"Obfuscated common word detected ('{foundLeetWord}')" : "No obvious l33t character substitutions for common words."
            });

            // 5. Sequential Numbers / Alphabets
            bool foundSeq = Regex.IsMatch(lower, @"(123|234|345|456|567|678|789|890|abc|bcd|cde|def)");
            if (foundSeq) totalDetected++;
            checklist.Add(new PatternItem
            {
                Name = "Ascending Numbers / Alphabets (e.g. '123', 'abc')",
                Category = "Sequential",
                Detected = foundSeq,
                Description = foundSeq ? "Contains predictable ascending sequence." : "No simple ascending sequences found."
            });

            return new PatternResult
            {
                ChecklistItems = checklist,
                TotalPatternsDetected = totalDetected
            };
        }
    }
}
