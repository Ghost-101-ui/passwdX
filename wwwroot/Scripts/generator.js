$(document).ready(function () {
    // Generate initially when generator view is accessed or controls change
    $('#genLengthSlider').on('input', function () {
        $('#genLengthVal').text($(this).val());
        requestGeneratedPassword();
    });

    $('#genStyleSelect, #chkUpper, #chkLower, #chkNumbers, #chkSymbols').on('change', function () {
        requestGeneratedPassword();
    });

    $('#refreshGenBtn').on('click', function () {
        requestGeneratedPassword();
    });

    $('#copyGenBtn').on('click', function () {
        const text = $('#generatedOutput').val();
        if (!text) return;

        navigator.clipboard.writeText(text).then(function () {
            showHumorPopup({
                quote: "Copied! Now don't paste it into Facebook 😅",
                emoji: "📋",
                category: "Copied"
            });
        });
    });
});

function requestGeneratedPassword() {
    const req = {
        length: parseInt($('#genLengthSlider').val()),
        useUpper: $('#chkUpper').is(':checked'),
        useLower: $('#chkLower').is(':checked'),
        useNumbers: $('#chkNumbers').is(':checked'),
        useSymbols: $('#chkSymbols').is(':checked'),
        style: $('#genStyleSelect').val()
    };

    $.ajax({
        url: '/Password/Generate',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(req),
        success: function (res) {
            $('#generatedOutput').val(res.generatedPassword);
            
            // Auto update main analyzer input if empty
            if (!$('#mainPasswordInput').val()) {
                $('#mainPasswordInput').val(res.generatedPassword).trigger('input');
            }
        }
    });
}
