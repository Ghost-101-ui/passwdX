using System;
using System.Collections.Generic;
using PasswordX.Models;

namespace PasswordX.Services
{
    public class MemeService
    {
        private static readonly Random _random = new Random();

        private static readonly Dictionary<string, List<(string Quote, string Emoji)>> Memes = new()
        {
            ["Weak"] = new()
            {
                ("Hackers won't even need coffee for this one ☕😂", "☕"),
                ("My dog typed a stronger password by stepping on the keyboard 🐶", "🐕"),
                ("This password offers less protection than a paper shield 🛡️", "📜"),
                ("BRUTE FORCE COMPLETE: Took 0.0001 seconds! ⚡", "⚡"),
                ("A toddler guessing '123' just cracked your mainframe 👶", "🍼")
            },
            ["Common"] = new()
            {
                ("'password123' called... it wants retirement 👵", "👴"),
                ("Found in 4,819 data breaches! You're famous! 📸", "🎉"),
                ("Dictionary bots are celebrating in binary right now 🤖", "👾"),
                ("This password is so popular it has its own fan club 🎟️", "🍿")
            },
            ["Moderate"] = new()
            {
                ("Getting warmer! Still needs a bit more chaos 🔥", "🌶️"),
                ("Decent defense, but a determined bot might sneak past 🕵️", "🕶️"),
                ("Not terrible, but not impenetrable yet 🏰", "⚔️")
            },
            ["Strong"] = new()
            {
                ("Even your future self might forget this one 😎", "😎"),
                ("Hackers are filing a union complaint over this complexity 👷", "🛡️"),
                ("Quantum computers are scratching their transistors 💻", "🔬"),
                ("Solid as digital granite! 🪨", "💎")
            },
            ["Generated"] = new()
            {
                ("Freshly baked password 🍪", "🍪"),
                ("Generated with 100% organic, non-predictable bits 🌾", "✨"),
                ("Hot off the cryptographic press! 🔥", "📜")
            },
            ["Copied"] = new()
            {
                ("Copied! Now don't paste it into Facebook 😅", "📋"),
                ("Saved to clipboard! Treat it like secret alien code 👽", "🛸"),
                ("Copied! Hide it under your digital mattress 🛏️", "🔐")
            },
            ["ChallengeUnlocked"] = new()
            {
                ("Achievement Unlocked: Hacker's Worst Nightmare 🏆", "🏆"),
                ("LEVEL MAXED OUT! Cyber God status achieved! ⚡🎮", "👑"),
                ("100/100 SCORE! The pixels are weeping tears of joy 👾✨", "🌟")
            }
        };

        public MemeResult GetMeme(string category)
        {
            if (!Memes.ContainsKey(category))
            {
                category = "Moderate";
            }

            var options = Memes[category];
            var item = options[_random.Next(options.Count)];

            return new MemeResult
            {
                Quote = item.Quote,
                Category = category,
                Emoji = item.Emoji,
                PixelIcon = category.ToLower()
            };
        }
    }
}
