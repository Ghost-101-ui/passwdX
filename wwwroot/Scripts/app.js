// Global State
window.PasswordXState = {
    currentPassword: "",
    personalInfo: "",
    currentModule: "analyzer"
};

$(document).ready(function () {
    // Theme Toggle Handler
    $('#themeToggle').on('click', function () {
        const currentTheme = $('html').attr('data-theme');
        const newTheme = currentTheme === 'dark' ? 'light' : 'dark';
        $('html').attr('data-theme', newTheme);
        $(this).text(newTheme === 'dark' ? '☀️' : '🌙');
    });

    // Show/Hide Password Toggle
    $('#togglePasswordBtn').on('click', function () {
        const $input = $('#mainPasswordInput');
        const isPass = $input.attr('type') === 'password';
        $input.attr('type', isPass ? 'text' : 'password');
        $(this).text(isPass ? '🙈' : '👁️');
    });

    // Personal Info Toggle
    $('#togglePersonalInfo').on('click', function (e) {
        e.preventDefault();
        $('#personalInfoContainer').slideToggle(200);
    });

    // Character Counter & Live State Sync
    $('#mainPasswordInput').on('input', function () {
        const val = $(this).val();
        window.PasswordXState.currentPassword = val;
        $('#charCountLabel').text(val.length + " characters");

        // Live refresh active module if viewing DNA, Coach, Patterns, Crack, Scorecard, or Challenge
        const activeMod = window.PasswordXState.currentModule;
        if (activeMod !== 'analyzer' && activeMod !== 'generator' && activeMod !== 'about') {
            switchModule(activeMod);
        }
    });

    $('#personalInput').on('input', function () {
        window.PasswordXState.personalInfo = $(this).val();
    });
});

// Module Navigation Function with Fade Animation
function switchModule(moduleName) {
    window.PasswordXState.currentModule = moduleName;

    // Update Sidebar Active Class
    $('.sidebar-item').removeClass('active');
    $(`#nav-${moduleName}`).addClass('active');

    // Fade Out Current View & Load Selected View
    $('.module-section').fadeOut(150, function () {
        $(`#${moduleName}View`).fadeIn(200);
    });

    // Trigger Module Specific AJAX Data Load with fallback
    const currentPass = $('#mainPasswordInput').val() || "P@ssword123";

    if (moduleName === 'dna') loadDNAData(currentPass);
    else if (moduleName === 'coach') loadCoachData(currentPass);
    else if (moduleName === 'patterns') loadPatternData(currentPass);
    else if (moduleName === 'crack') loadCrackData(currentPass);
    else if (moduleName === 'scorecard') loadScorecardData(currentPass);
    else if (moduleName === 'challenge') loadChallengeData(currentPass);
}

// Global Humor Popup Speech Bubble Manager
function showHumorPopup(meme) {
    if (!meme || !meme.quote) return;

    const popupHtml = `
        <div class="humor-popup" id="activeHumorPopup">
            <span class="popup-emoji">${meme.emoji || '👾'}</span>
            <div class="popup-text">${meme.quote}</div>
        </div>
    `;

    $('#humorPopupContainer').html(popupHtml);

    // Auto-dismiss after 3.5 seconds
    setTimeout(function () {
        $('#activeHumorPopup').fadeOut(300, function () {
            $(this).remove();
        });
    }, 3500);
}
