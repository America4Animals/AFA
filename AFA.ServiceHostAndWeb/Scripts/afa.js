var AFA = AFA || {};

AFA.showSuccessMessage = function(msg) {
    $('#successMessage').html(msg).parent().show();
    scrollTop();
};

AFA.showErrorMessage = function(msg) {
    $('#errorMessage').html(msg).parent().show();
    scrollTop();
};

AFA.hideErrorDialog = function () {
    $('#errorMessage').parent().hide();
};

AFA.hideSuccessDialog = function () {
    $('#successMessage').parent().hide();
};

AFA.scrollTop = function() {
    $('html,body').animate({ scrollTop: 0 }, 'fast');
};
