using Microsoft.AspNetCore.Mvc;
using PasswordX.Models;
using PasswordX.Services;

namespace PasswordX.Controllers
{
    public class PasswordController : Controller
    {
        private readonly PasswordChecker _checker = new();
        private readonly PasswordGenerator _generator = new();
        private readonly PasswordCoach _coach = new();
        private readonly PatternDetector _patternDetector = new();
        private readonly CrackEstimator _crackEstimator = new();
        private readonly ScorecardService _scorecardService = new();
        private readonly ChallengeService _challengeService = new();
        private readonly MemeService _memeService = new();

        public IActionResult Index()
        {
            return View();
        }

        private string EnsurePassword(AnalysisRequest? request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Password))
            {
                return "P@ssword123";
            }
            return request.Password;
        }

        [HttpPost]
        public IActionResult Analyze([FromBody] AnalysisRequest? request)
        {
            string password = EnsurePassword(request);
            string personalInfo = request?.PersonalInfo ?? "";
            var result = _checker.Analyze(password, personalInfo);
            return Json(result);
        }

        [HttpPost]
        public IActionResult Generate([FromBody] GeneratorRequest? request)
        {
            request ??= new GeneratorRequest();
            var result = _generator.Generate(request);
            return Json(result);
        }

        [HttpPost]
        public IActionResult DNA([FromBody] AnalysisRequest? request)
        {
            string password = EnsurePassword(request);
            string personalInfo = request?.PersonalInfo ?? "";

            var analysis = _checker.Analyze(password, personalInfo);
            var crack = _crackEstimator.Estimate(password);
            var pattern = _patternDetector.DetectPatterns(password);

            int lenScore = Math.Min(100, password.Length * 6);
            int compScore = (analysis.HasUpper ? 25 : 0) + (analysis.HasLower ? 25 : 0) + (analysis.HasNumber ? 25 : 0) + (analysis.HasSymbol ? 25 : 0);
            int patSafety = Math.Max(0, 100 - (pattern.TotalPatternsDetected * 25));
            int dictSafety = analysis.DictionaryWords.Count > 0 ? 20 : 95;
            double entropyBits = Math.Round(password.Length * (compScore > 50 ? 5.5 : 3.5), 1);
            int entropyScore = Math.Min(100, (int)(entropyBits * 1.2));
            int randScore = Math.Min(100, (int)(analysis.Score * 0.9 + compScore * 0.1));

            var result = new DnaResult
            {
                LengthScore = lenScore,
                ComplexityScore = compScore,
                EntropyScore = entropyScore,
                RandomnessScore = randScore,
                DictionarySafetyScore = dictSafety,
                PatternSafetyScore = patSafety,
                EntropyBits = entropyBits,
                SummaryText = $"DNA Profile: {analysis.StrengthLabel} complexity with {entropyBits} bits of entropy."
            };

            return Json(result);
        }

        [HttpPost]
        public IActionResult Coach([FromBody] AnalysisRequest? request)
        {
            string password = EnsurePassword(request);
            var result = _coach.GetAdvice(password);
            return Json(result);
        }

        [HttpPost]
        public IActionResult Patterns([FromBody] AnalysisRequest? request)
        {
            string password = EnsurePassword(request);
            var result = _patternDetector.DetectPatterns(password);
            return Json(result);
        }

        [HttpPost]
        public IActionResult CrackTime([FromBody] AnalysisRequest? request)
        {
            string password = EnsurePassword(request);
            var result = _crackEstimator.Estimate(password);
            return Json(result);
        }

        [HttpPost]
        public IActionResult Scorecard([FromBody] AnalysisRequest? request)
        {
            string password = EnsurePassword(request);
            var result = _scorecardService.Evaluate(password);
            return Json(result);
        }

        [HttpPost]
        public IActionResult Challenge([FromBody] AnalysisRequest? request)
        {
            string password = EnsurePassword(request);
            var result = _challengeService.GetChallengeState(password);
            return Json(result);
        }
    }
}
