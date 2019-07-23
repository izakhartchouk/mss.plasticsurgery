(function ($) {
    'use strict';

    var $operationsTab = $('#tab-content-1');
    var $operationModal = $('#operation-modal');

    $(document).ready(function () {
        $operationsTab.load('/Administration/GetOperations');
    });

    $operationModal.on('show.bs.modal', function(event) {
        console.log('BS EVENT: show.bs.modal');
    });

    $('.btn-primary', $operationModal).on('click', function(event) {
        var formData = $('.modal-form', $operationModal).serialize();

        $.ajax({
            type: 'POST',
            url: '/Administration/CreateOperation',
            dataType: 'json',
            contentType: 'application/x-www-form-urlencoded; charset=utf-8',
            data: formData,
            success: function (result) {
                console.log(result);
            }
        });
    });
})(jQuery);