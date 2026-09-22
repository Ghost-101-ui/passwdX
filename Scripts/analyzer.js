$(document).ready(function () {
    $('#analyzeBtn').on('click', function () {
        performAnalysis();
    });

    $('#mainPasswordInput').on('keypress', function (e) {
        if (e.which === 13) {
            performAnalysis();
        }
    });

    // Auto-analyze on initial page load to eliminate layout blank space
    performAnalysis();
});

function performAnalysis() {
    const password = $('#mainPasswordInput').val() || "P@@ssw0rdX_2026!";
    const personalInfo = $('#personalInput').val();

    // Disable button & show pixel loading state
    const $btn = $('#analyzeBtn');
    const origText = $btn.html();
    $btn.html('ANALYZING ⏳').prop('disabled', true);

    $.ajax({
        url: '/Password/Analyze',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ password: password, personalInfo: personalInfo }),
        success: function (data) {
            $btn.html(origText).prop('disabled', false);

            // Populate Strength Overview
            $('#scoreVal').text(data.score);
            $('#strengthBadge').text(data.strengthLabel.toUpperCase());

            // Colorize badge and 8-bit pixel progress bar based on strength
            const badgeColors = {
                'WEAK': '#ef4444',
                'MODERATE': '#f59e0b',
                'STRONG': '#2563eb',
                'EXTREMELY STRONG': '#10b981'
            };
            const barColor = badgeColors[data.strengthLabel.toUpperCase()] || '#2563eb';

            $('#heroScoreFill').css({
                'width': data.score + '%',
                'background-color': barColor
            });

            $('#strengthBadge').css({
                'background-color': barColor + '15',
                'color': barColor,
                'border-color': barColor
            });

            // Quick Suggestions
            if (data.suggestions && data.suggestions.length > 0) {
                let itemsHtml = '';
                data.suggestions.forEach(sug => {
                    itemsHtml += `<li>${sug}</li>`;
                });
                $('#suggestionsList').html(itemsHtml);
                $('#quickSuggestionsContainer').slideDown(200);
            } else {
                $('#quickSuggestionsContainer').slideUp(200);
            }

            // Expand Dashboard
            $('#analysisDashboard').slideDown(300);

            // Trigger Humor Popup
            if (data.meme) {
                showHumorPopup(data.meme);
            }
        },
        error: function () {
            $btn.html(origText).prop('disabled', false);
        }
    });
}

// Module specific AJAX loaders
function loadDNAData(password) {
    password = password || "P@@ssw0rdX_2026!";
    $.ajax({
        url: '/Password/DNA',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ password: password }),
        success: function (dna) {
            $('#dnaSummaryText').text(dna.summaryText);
            const bars = [
                { label: 'Length Score', val: dna.lengthScore },
                { label: 'Complexity Score', val: dna.complexityScore },
                { label: 'Entropy Score (' + dna.entropyBits + ' bits)', val: dna.entropyScore },
                { label: 'Randomness Score', val: dna.randomnessScore },
                { label: 'Dictionary Safety', val: dna.dictionarySafetyScore },
                { label: 'Pattern Safety', val: dna.patternSafetyScore }
            ];

            let html = '';
            bars.forEach(b => {
                let color = '#10b981'; // Green
                if (b.val < 30) color = '#ef4444'; // Red
                else if (b.val < 50) color = '#f59e0b'; // Yellow
                else if (b.val < 80) color = '#2563eb'; // Blue

                html += `
                    <div class="dna-bar-container">
                        <div class="dna-bar-label">
                            <span>${b.label}</span>
                            <span style="font-family: var(--font-jetbrains); font-weight: 700; color: ${color};">${b.val}%</span>
                        </div>
                        <div class="pixel-progress-track">
                            <div class="pixel-progress-fill" style="width: ${b.val}%; background-color: ${color};"></div>
                        </div>
                    </div>
                `;
            });
            $('#dnaBarsContainer').html(html);
        }
    });
}

function loadCoachData(password) {
    password = password || "P@@ssw0rdX_2026!";
    $.ajax({
        url: '/Password/Coach',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ password: password }),
        success: function (coach) {
            $('#coachVerdict').text(coach.overallVerdict);

            let posHtml = '';
            coach.positiveFeedback.forEach(p => posHtml += `<li>${p}</li>`);
            if (!posHtml) posHtml = '<li>No major strengths detected.</li>';
            $('#coachPositiveList').html(posHtml);

            let negHtml = '';
            coach.negativeFeedback.forEach(n => negHtml += `<li>${n}</li>`);
            coach.improvementSuggestions.forEach(s => negHtml += `<li>💡 ${s}</li>`);
            if (!negHtml) negHtml = '<li>No obvious weaknesses detected!</li>';
            $('#coachNegativeList').html(negHtml);
        }
    });
}

function loadPatternData(password) {
    password = password || "P@@ssw0rdX_2026!";
    $.ajax({
        url: '/Password/Patterns',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ password: password }),
        success: function (res) {
            let html = '';
            res.checklistItems.forEach(item => {
                const statusIcon = item.detected ? '❌' : '✅';
                const statusColor = item.detected ? 'var(--color-danger)' : 'var(--color-success)';
                html += `
                    <div style="display: flex; gap: 14px; align-items: center; padding: 12px; border-bottom: 1px solid #e2e8f0;">
                        <span style="font-size: 1.4rem;">${statusIcon}</span>
                        <div>
                            <div style="font-weight: 600; color: ${statusColor}">${item.name}</div>
                            <div style="font-size: 0.85rem; color: var(--text-muted);">${item.description}</div>
                        </div>
                    </div>
                `;
            });
            $('#patternChecklist').html(html);
        }
    });
}

function loadCrackData(password) {
    password = password || "P@@ssw0rdX_2026!";
    $.ajax({
        url: '/Password/CrackTime',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ password: password }),
        success: function (crack) {
            const cards = [
                { title: 'Online Attack (100 req/s)', time: crack.onlineAttackTime, icon: '🌐' },
                { title: 'Offline Attack (100k req/s)', time: crack.offlineAttackTime, icon: '💻' },
                { title: 'Dictionary Attack (10B req/s)', time: crack.dictionaryAttackTime, icon: '📚' },
                { title: 'GPU Brute Force (100B req/s)', time: crack.bruteForceAttackTime, icon: '⚡' }
            ];

            let html = '';
            cards.forEach(c => {
                html += `
                    <div style="background: var(--bg-main); border: 2px solid #e2e8f0; border-radius: 8px; padding: 16px;">
                        <div style="font-size: 1.5rem; margin-bottom: 4px;">${c.icon}</div>
                        <div style="font-size: 0.85rem; color: var(--text-muted);">${c.title}</div>
                        <div class="crack-time-val" style="font-size: 1.1rem; color: var(--primary-blue); margin-top: 6px;">${c.time}</div>
                    </div>
                `;
            });
            $('#crackGrid').html(html);
        }
    });
}

function loadScorecardData(password) {
    password = password || "P@@ssw0rdX_2026!";
    $.ajax({
        url: '/Password/Scorecard',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ password: password }),
        success: function (sc) {
            const items = [
                { label: 'Length', grade: sc.lengthGrade },
                { label: 'Complexity', grade: sc.complexityGrade },
                { label: 'Randomness', grade: sc.randomnessGrade },
                { label: 'Patterns', grade: sc.patternsGrade },
                { label: 'Dictionary', grade: sc.dictionaryGrade },
                { label: 'Overall Grade', grade: sc.overallGrade }
            ];

            let html = '';
            items.forEach(i => {
                html += `
                    <div style="text-align: center; background: var(--bg-main); border: 2px solid #e2e8f0; border-radius: 10px; padding: 20px;">
                        <div style="font-size: 0.85rem; color: var(--text-muted); margin-bottom: 4px;">${i.label}</div>
                        <div class="scorecard-grade" style="font-size: 2.2rem; color: var(--primary-blue);">${i.grade}</div>
                    </div>
                `;
            });
            $('#scorecardGrid').html(html);
        }
    });
}
