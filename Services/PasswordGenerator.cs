using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using PasswordX.Models;

namespace PasswordX.Services
{
    public class PasswordGenerator
    {
        private static readonly string UpperChars = "ABCDEFGHJKLMNPQRSTUVWXYZ"; // default excludes ambiguous I, O if needed
        private static readonly string LowerChars = "abcdefghijkmnopqrstuvwxyz";
        private static readonly string NumberChars = "23456789";
        private static readonly string SymbolChars = "!@#$%^&*()_+-=[]{}|;:,.<>?";

        private static readonly string[] Syllables = new[]
        {
            "Cyber", "Pixel", "Vortex", "Matrix", "Neon", "Retro", "Shield", "Vault",
            "Shadow", "Quantum", "Hyper", "Astral", "Echo", "Falcon", "Titan", "Zenith"
        };

        private static readonly string[] GamerPrefixes = new[] { "Xx_", "iNf_", "V3x_", "Gl1tch_", "P1x3l_", "Zyph_" };
        private static readonly string[] GamerSuffixes = new[] { "_xX", "_Vip", "_RtX", "_Gg", "_99", "_Pro" };

        private readonly MemeService _memeService = new();

        public GeneratorResult Generate(GeneratorRequest request)
        {
            int length = Math.Clamp(request.Length, 6, 64);
            string password = "";
            string description = "";

            switch (request.Style)
            {
                case "Easy to Remember":
                    password = GenerateEasyToRemember(length, request.UseNumbers, request.UseSymbols);
                    description = "Pronounceable word combinations with digits and special symbols.";
                    break;

                case "Maximum Security":
                    password = GenerateMaxSecurity(length);
                    description = "High-entropy cryptographic string using all character sets.";
                    break;

                case "Gaming":
                    password = GenerateGamingStyle(length);
                    description = "Synthwave retro gamer tag aesthetic with numbers and symbols.";
                    break;

                case "Corporate":
                    password = GenerateCorporateStyle(length);
                    description = "Compliant with enterprise security standards without ambiguous characters.";
                    break;

                case "Professional":
                default:
                    password = GenerateStandard(length, request.UseUpper, request.UseLower, request.UseNumbers, request.UseSymbols);
                    description = "Balanced, clean password tailored to your specified options.";
                    break;
            }

            double entropy = CalculateEntropy(password);
            int score = (int)Math.Min(100, Math.Max(20, entropy * 1.25));

            return new GeneratorResult
            {
                GeneratedPassword = password,
                StrengthScore = score,
                EntropyBits = Math.Round(entropy, 1),
                StyleDescription = description,
                Meme = _memeService.GetMeme("Generated")
            };
        }

        private string GenerateStandard(int length, bool upper, bool lower, bool num, bool sym)
        {
            var charPool = new StringBuilder();
            var required = new List<char>();

            if (upper) { charPool.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ"); required.Add(GetRandomChar("ABCDEFGHIJKLMNOPQRSTUVWXYZ")); }
            if (lower) { charPool.Append("abcdefghijklmnopqrstuvwxyz"); required.Add(GetRandomChar("abcdefghijklmnopqrstuvwxyz")); }
            if (num) { charPool.Append("0123456789"); required.Add(GetRandomChar("0123456789")); }
            if (sym) { charPool.Append("!@#$%^&*()_+-="); required.Add(GetRandomChar("!@#$%^&*()_+-=")); }

            if (charPool.Length == 0) charPool.Append("abcdefghijklmnopqrstuvwxyz0123456789");

            var result = new char[length];
            int i = 0;

            for (; i < required.Count && i < length; i++)
            {
                result[i] = required[i];
            }

            for (; i < length; i++)
            {
                result[i] = GetRandomChar(charPool.ToString());
            }

            // Shuffle
            return ShuffleString(new string(result));
        }

        private string GenerateEasyToRemember(int length, bool num, bool sym)
        {
            string word1 = Syllables[RandomNumberGenerator.GetInt32(Syllables.Length)];
            string word2 = Syllables[RandomNumberGenerator.GetInt32(Syllables.Length)];
            string digit = num ? RandomNumberGenerator.GetInt32(100, 999).ToString() : "";
            string symbol = sym ? GetRandomChar("!@#$%^&*").ToString() : "";

            string raw = $"{word1}{symbol}{word2}{digit}";
            if (raw.Length < length)
            {
                raw += GetRandomChar("!@#$%^&*") + RandomNumberGenerator.GetInt32(10, 99).ToString();
            }
            return raw.Length > length ? raw.Substring(0, length) : raw;
        }

        private string GenerateMaxSecurity(int length)
        {
            string fullPool = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+-=[]{}|;:,.<>?";
            var result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = GetRandomChar(fullPool);
            }
            return new string(result);
        }

        private string GenerateGamingStyle(int length)
        {
            string prefix = GamerPrefixes[RandomNumberGenerator.GetInt32(GamerPrefixes.Length)];
            string suffix = GamerSuffixes[RandomNumberGenerator.GetInt32(GamerSuffixes.Length)];
            string word = Syllables[RandomNumberGenerator.GetInt32(Syllables.Length)]
                .Replace("a", "4").Replace("e", "3").Replace("i", "1").Replace("o", "0").Replace("s", "$");

            string result = $"{prefix}{word}{suffix}";
            return result.Length > length ? result.Substring(0, length) : result;
        }

        private string GenerateCorporateStyle(int length)
        {
            int targetLen = Math.Max(12, length);
            string pool = UpperChars + LowerChars + NumberChars + SymbolChars;
            var result = new char[targetLen];

            result[0] = GetRandomChar(UpperChars);
            result[1] = GetRandomChar(LowerChars);
            result[2] = GetRandomChar(NumberChars);
            result[3] = GetRandomChar(SymbolChars);

            for (int i = 4; i < targetLen; i++)
            {
                result[i] = GetRandomChar(pool);
            }
            return ShuffleString(new string(result));
        }

        private char GetRandomChar(string pool)
        {
            return pool[RandomNumberGenerator.GetInt32(pool.Length)];
        }

        private string ShuffleString(string str)
        {
            char[] array = str.ToCharArray();
            int n = array.Length;
            while (n > 1)
            {
                int k = RandomNumberGenerator.GetInt32(n--);
                (array[n], array[k]) = (array[k], array[n]);
            }
            return new string(array);
        }

        private double CalculateEntropy(string password)
        {
            if (string.IsNullOrEmpty(password)) return 0;
            int charsetSize = 0;
            if (password.Any(char.IsLower)) charsetSize += 26;
            if (password.Any(char.IsUpper)) charsetSize += 26;
            if (password.Any(char.IsDigit)) charsetSize += 10;
            if (password.Any(c => !char.IsLetterOrDigit(c))) charsetSize += 32;

            if (charsetSize == 0) charsetSize = 26;
            return password.Length * Math.Log2(charsetSize);
        }
    }
}
