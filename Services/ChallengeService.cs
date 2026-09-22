using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PasswordX.Models;

namespace PasswordX.Services
{
    public class ChallengeService
    {
        private readonly PatternDetector _patternDetector = new();
        private readonly MemeService _memeService = new();

        public ChallengeResult GetChallengeState(string password)
        {
            password ??= "";
            string lower = password.ToLower();

            bool hasUpper = password.Any(char.IsUpper);
            bool hasSymbol = password.Any(c => !char.IsLetterOrDigit(c));
            bool isLongEnough = password.Length >= 14;
            bool noRepeated = !Regex.IsMatch(password, @"(.)\1{2,}");
            
            var patternRes = _patternDetector.DetectPatterns(password);
            bool noCommonWord = !patternRes.ChecklistItems.Any(i => i.Category == "Dictionary" && i.Detected);
            bool noSeqNumbers = !Regex.IsMatch(lower, @"(123|234|345|456|567|678|789|890)");

            var tasks = new List<ChallengeTask>
            {
                new ChallengeTask
                {
                    Id = "t_upper",
                    Title = "Add one uppercase letter (+10)",
                    Points = 10,
                    IsCompleted = hasUpper
                },
                new ChallengeTask
                {
                    Id = "t_symbol",
                    Title = "Add one special character (+10)",
                    Points = 10,
                    IsCompleted = hasSymbol
                },
                new ChallengeTask
                {
                    Id = "t_len14",
                    Title = "Increase length to 14+ characters (+8)",
                    Points = 8,
                    IsCompleted = isLongEnough
                },
                new ChallengeTask
                {
                    Id = "t_repeat",
                    Title = "Remove repeated characters (+7)",
                    Points = 7,
                    IsCompleted = noRepeated
                },
                new ChallengeTask
                {
                    Id = "t_word",
                    Title = "Remove common dictionary word (+10)",
                    Points = 10,
                    IsCompleted = noCommonWord
                },
                new ChallengeTask
                {
                    Id = "t_seq",
                    Title = "Avoid sequential numbers (+8)",
                    Points = 8,
                    IsCompleted = noSeqNumbers
                }
            };

            int baseScore = 47; // Base score for starting challenge
            int currentScore = baseScore + tasks.Where(t => t.IsCompleted).Sum(t => t.Points);
            currentScore = Math.Min(100, currentScore);

            bool isMasterUnlocked = tasks.All(t => t.IsCompleted) || currentScore >= 100;

            return new ChallengeResult
            {
                CurrentScore = isMasterUnlocked ? 100 : currentScore,
                TargetScore = 100,
                Tasks = tasks,
                IsMasterUnlocked = isMasterUnlocked,
                UnlockMeme = isMasterUnlocked ? _memeService.GetMeme("ChallengeUnlocked") : null
            };
        }
    }
}
