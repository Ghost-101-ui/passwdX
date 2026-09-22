using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PasswordX.Models;

namespace PasswordX.Services
{
    public class PasswordCoach
    {
        public CoachResult GetAdvice(string password)
        {
            var positive = new List<string>();
            var negative = new List<string>();
            var improvements = new List<string>();

            if (string.IsNullOrEmpty(password))
            {
                return new CoachResult
                {
                    NegativeFeedback = new List<string> { "No password provided to analyze!" },
                    ImprovementSuggestions = new List<string> { "Type or generate a password above." },
                    OverallVerdict = "Awaiting password input."
                };
            }

            int len = password.Length;

            // Positive callouts
            if (len >= 16) positive.Add("Excellent length! 16+ characters creates an exponential barrier against brute force.");
            else if (len >= 12) positive.Add("Good base length. 12+ characters meets modern security standards.");

            if (password.Any(char.IsUpper) && password.Any(char.IsLower))
                positive.Add("Great use of mixed-case casing (uppercase & lowercase letters).");

            if (password.Any(c => !char.IsLetterOrDigit(c)))
                positive.Add("Strong inclusion of special symbols to broaden search space.");

            if (password.Distinct().Count() > len * 0.7)
                positive.Add("High character diversity with minimal repetition.");

            // Negative callouts & Specific AI Coach advice
            if (len < 8)
            {
                negative.Add("Dangerously short length! Modern GPUs can crack sub-8 character passwords in seconds.");
                improvements.Add("Extend password length to at least 12 characters.");
            }

            if (Regex.IsMatch(password, @"(123|1234|2023|2024|2025|2026)$"))
            {
                negative.Add("Your password ends with a predictable numeric sequence or year suffix.");
                improvements.Add("Replace ending numbers like '123' or years with random symbols (e.g., '^k9#').");
            }

            if (Regex.IsMatch(password, @"^[A-Z][a-z]+[0-9]{1,3}[!@#$%^&*]?$"))
            {
                negative.Add("This follows the common 'Name123!' template. Attackers prioritize this exact pattern!");
                improvements.Add("Interleave symbols and numbers throughout the word instead of appending them at the end.");
            }

            if (Regex.IsMatch(password, @"(.)\1{2,}"))
            {
                negative.Add("Detected repeated character sequences (e.g. 'aaa' or '999').");
                improvements.Add("Remove repeating identical characters to maximize entropy.");
            }

            if (!password.Any(char.IsDigit))
            {
                negative.Add("Missing numeric digits.");
                improvements.Add("Scatter 2 or 3 random numbers within your password.");
            }

            if (!password.Any(c => !char.IsLetterOrDigit(c)))
            {
                negative.Add("Missing special characters.");
                improvements.Add("Add non-alphanumeric symbols like @, #, $, %, ^, &, *.");
            }

            // Overall Verdict
            string verdict;
            if (negative.Count == 0 && len >= 14)
            {
                verdict = "Outstanding! Your password follows elite cryptographic practices.";
            }
            else if (negative.Count <= 1)
            {
                verdict = "Solid password structure! Minor tweaks will make it virtually uncrackable.";
            }
            else
            {
                verdict = "Needs improvement. Follow the suggested tweaks below to fortify your defense.";
            }

            return new CoachResult
            {
                PositiveFeedback = positive,
                NegativeFeedback = negative,
                ImprovementSuggestions = improvements,
                OverallVerdict = verdict
            };
        }
    }
}
