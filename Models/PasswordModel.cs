using System.Collections.Generic;

namespace PasswordX.Models
{
    public class AnalysisRequest
    {
        public string Password { get; set; } = string.Empty;
        public string PersonalInfo { get; set; } = string.Empty;
    }

    public class AnalysisResult
    {
        public int Score { get; set; }
        public string StrengthLabel { get; set; } = string.Empty;
        public List<string> Suggestions { get; set; } = new();
        public int Length { get; set; }
        public bool HasUpper { get; set; }
        public bool HasLower { get; set; }
        public bool HasNumber { get; set; }
        public bool HasSymbol { get; set; }
        public List<string> DictionaryWords { get; set; } = new();
        public List<string> KeyboardPatterns { get; set; } = new();
        public List<string> RepeatedChars { get; set; } = new();
        public List<string> SequentialChars { get; set; } = new();
        public bool PersonalInfoFound { get; set; }
        public MemeResult Meme { get; set; } = new();
    }

    public class GeneratorRequest
    {
        public int Length { get; set; } = 16;
        public bool UseUpper { get; set; } = true;
        public bool UseLower { get; set; } = true;
        public bool UseNumbers { get; set; } = true;
        public bool UseSymbols { get; set; } = true;
        public string Style { get; set; } = "Professional";
    }

    public class GeneratorResult
    {
        public string GeneratedPassword { get; set; } = string.Empty;
        public int StrengthScore { get; set; }
        public double EntropyBits { get; set; }
        public string StyleDescription { get; set; } = string.Empty;
        public MemeResult Meme { get; set; } = new();
    }

    public class DnaResult
    {
        public int LengthScore { get; set; }
        public int ComplexityScore { get; set; }
        public int EntropyScore { get; set; }
        public int RandomnessScore { get; set; }
        public int DictionarySafetyScore { get; set; }
        public int PatternSafetyScore { get; set; }
        public double EntropyBits { get; set; }
        public string SummaryText { get; set; } = string.Empty;
    }

    public class CoachResult
    {
        public List<string> PositiveFeedback { get; set; } = new();
        public List<string> NegativeFeedback { get; set; } = new();
        public List<string> ImprovementSuggestions { get; set; } = new();
        public string OverallVerdict { get; set; } = string.Empty;
    }

    public class PatternItem
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public bool Detected { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class PatternResult
    {
        public List<PatternItem> ChecklistItems { get; set; } = new();
        public int TotalPatternsDetected { get; set; }
    }

    public class CrackTimeResult
    {
        public string OnlineAttackTime { get; set; } = string.Empty;
        public string OfflineAttackTime { get; set; } = string.Empty;
        public string DictionaryAttackTime { get; set; } = string.Empty;
        public string BruteForceAttackTime { get; set; } = string.Empty;
        public int Score { get; set; }
    }

    public class ScorecardResult
    {
        public string LengthGrade { get; set; } = "F";
        public string ComplexityGrade { get; set; } = "F";
        public string RandomnessGrade { get; set; } = "F";
        public string PatternsGrade { get; set; } = "F";
        public string DictionaryGrade { get; set; } = "F";
        public string EntropyGrade { get; set; } = "F";
        public string OverallGrade { get; set; } = "F";
    }

    public class ChallengeTask
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public int Points { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class ChallengeResult
    {
        public int CurrentScore { get; set; }
        public int TargetScore { get; set; } = 100;
        public List<ChallengeTask> Tasks { get; set; } = new();
        public bool IsMasterUnlocked { get; set; }
        public MemeResult? UnlockMeme { get; set; }
    }

    public class MemeResult
    {
        public string Quote { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Emoji { get; set; } = "👾";
        public string PixelIcon { get; set; } = "bubble-chat";
    }
}
