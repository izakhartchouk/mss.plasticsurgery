(function ($) {
    'use strict';

    var $spinner = $('.spinner-border');
    var $submitText = $('.submit-text');
    var $submitFormButton = $('#submit-contact-form');
    var $alertSuccess = $('#send-success-alert');
    var $alertSuccessCloseButton = $('#send-success-alert-close-button');

    $submitFormButton.on('click', function () {
        $spinner.show();
        $submitText.hide();

        var dataToSend = $('#contact-form').serializeToObject();

        $.ajax({
            type: 'POST',
            url: '/Home/Contacts',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            processData: true,
            cache: false,
            data: JSON.stringify(dataToSend),
            success: function (result) {
                if (result === 'success') {
                    $spinner.hide();
                    $submitText.show();
                    $alertSuccess.show();
                }

                console.log("Email dispatch: " + result);
            }
        });

    });

    $alertSuccessCloseButton.on('click', function () {
        $alertSuccess.hide();
    });
})(jQuery);