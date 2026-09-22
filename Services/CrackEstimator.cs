using System;
using System.Linq;
using PasswordX.Models;

namespace PasswordX.Services
{
    public class CrackEstimator
    {
        public CrackTimeResult Estimate(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return new CrackTimeResult
                {
                    OnlineAttackTime = "Instant (0 seconds)",
                    OfflineAttackTime = "Instant (0 seconds)",
                    DictionaryAttackTime = "Instant (0 seconds)",
                    BruteForceAttackTime = "Instant (0 seconds)",
                    Score = 0
                };
            }

            // Estimate total combinations
            int charsetSize = 0;
            if (password.Any(char.IsLower)) charsetSize += 26;
            if (password.Any(char.IsUpper)) charsetSize += 26;
            if (password.Any(char.IsDigit)) charsetSize += 10;
            if (password.Any(c => !char.IsLetterOrDigit(c))) charsetSize += 32;
            if (charsetSize == 0) charsetSize = 26;

            double combinations = Math.Pow(charsetSize, password.Length);

            // Penalize for common patterns
            string lower = password.ToLower();
            if (lower.Contains("password") || lower.Contains("123456") || lower.Contains("admin"))
            {
                combinations /= 10000;
            }

            // Attack Rates (Attacks per second)
            double onlineRate = 100.0;             // 100 guesses/sec (rate limited API)
            double offlineRate = 100_000.0;         // 100,000 guesses/sec (fast hash online)
            double dictRate = 10_000_000_000.0;     // 10 Billion guesses/sec (precomputed hash cat)
            double bruteRate = 100_000_000_000.0;   // 100 Billion guesses/sec (GPU cluster)

            double onlineSeconds = combinations / onlineRate;
            double offlineSeconds = combinations / offlineRate;
            double dictSeconds = combinations / dictRate;
            double bruteSeconds = combinations / bruteRate;

            int score = (int)Math.Min(100, Math.Max(5, Math.Log10(Math.Max(1, combinations)) * 5));

            return new CrackTimeResult
            {
                OnlineAttackTime = FormatTime(onlineSeconds),
                OfflineAttackTime = FormatTime(offlineSeconds),
                DictionaryAttackTime = FormatTime(dictSeconds),
                BruteForceAttackTime = FormatTime(bruteSeconds),
                Score = score
            };
        }

        private string FormatTime(double seconds)
        {
            if (seconds < 1) return "Instant (< 1 sec)";
            if (seconds < 60) return $"{Math.Round(seconds)} seconds";
            double minutes = seconds / 60;
            if (minutes < 60) return $"{Math.Round(minutes)} minutes";
            double hours = minutes / 60;
            if (hours < 24) return $"{Math.Round(hours)} hours";
            double days = hours / 24;
            if (days < 365) return $"{Math.Round(days)} days";
            double years = days / 365;
            if (years < 100) return $"{Math.Round(years)} years";
            double centuries = years / 100;
            if (centuries < 1000) return $"{Math.Round(centuries)} centuries";
            return "Millions of Years (Uncrackable)";
        }
    }
}
