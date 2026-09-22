function loadChallengeData(password) {
    password = password || $('#mainPasswordInput').val() || "P@ssword123";
    $.ajax({
        url: '/Password/Challenge',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ password: password }),
        success: function (res) {
            $('#challengeScoreBadge').text(`SCORE: ${res.currentScore}/100`);

            let html = '';
            res.tasks.forEach(t => {
                const icon = t.isCompleted ? '☑️' : '☐';
                const style = t.isCompleted ? 'color: var(--color-success); font-weight: 600; text-decoration: line-through;' : 'color: var(--text-main);';
                html += `
                    <div style="display: flex; align-items: center; gap: 12px; padding: 12px; background: var(--bg-main); border-radius: 8px; border: 1px solid #e2e8f0;">
                        <span style="font-size: 1.2rem;">${icon}</span>
                        <span style="${style}">${t.title}</span>
                    </div>
                `;
            });
            $('#challengeTasksList').html(html);

            // If Master Unlocked, trigger Confetti & Trophy
            if (res.isMasterUnlocked) {
                $('#masterTrophyBox').slideDown(300);
                if (typeof confetti === 'function') {
                    confetti({
                        particleCount: 120,
                        spread: 70,
                        origin: { y: 0.6 }
                    });
                }
                if (res.unlockMeme) {
                    showHumorPopup(res.unlockMeme);
                }
            } else {
                $('#masterTrophyBox').slideUp(200);
            }
        }
    });
}
