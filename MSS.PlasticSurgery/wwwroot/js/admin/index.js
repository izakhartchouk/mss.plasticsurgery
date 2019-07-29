(function ($) {
    'use strict';

    var $operationsTab = $('#tab-content-1');
    var $operationModal = $('#operation-modal');
    var $modalFormWithUploader = $('#modal-form-with-uploader', $operationModal);
    var $uploader = $('#images-field', $modalFormWithUploader);
    var $modalFormSaveButton = $('#modal-form-save', $operationModal);
    var $modalFormCloseButton = $('#modal-form-close', $operationModal);

    var formData = {
        Images: []
    };

    $(document).ready(function () {
        updateOperationsTab();

        $uploader.dmUploader({
            auto: true,
            queue: true,
            url: '/Administration/UploadFile',
            dataType: 'json',
            maxFileSize: 20971520, // 20 MB Max
            multiple: true,
            allowedTypes: 'image/*',
            extFilter: ['jpg', 'jpeg', 'png', 'gif'],
            onDragEnter: function () {
                // Happens when dragging something over the DnD area
                this.addClass('active');
            },
            onDragLeave: function () {
                // Happens when dragging something OUT of the DnD area
                this.removeClass('active');
            },
            onInit: function () {
                // Plugin is ready to use
            },
            onComplete: function () {
                // All files in the queue are processed (success or error)
            },
            onNewFile: function (id, file) {
                // When a new file is added using the file selector or the DnD area
                console.log('New file added #' + id);
                uiMultiAddFile(id, file);

                if (typeof FileReader !== "undefined") {
                    var reader = new FileReader();
                    var img = $('#uploaderFile' + id).find('img');

                    reader.onload = function (e) {
                        img.attr('src', e.target.result);
                    }

                    reader.readAsDataURL(file);
                }
            },
            onBeforeUpload: function (id) {
                // about tho start uploading a file
                console.log('Starting the upload of #' + id);
                uiMultiUpdateFileProgress(id, 0, '', true);
                uiMultiUpdateFileStatus(id, 'uploading', 'Uploading...');
            },
            onUploadProgress: function (id, percent) {
                // Updating file progress
                uiMultiUpdateFileProgress(id, percent);
            },
            onUploadSuccess: function (id, data) {
                // A file was successfully uploaded
                console.log('Server Response for file #' + id + ': ' + JSON.stringify(data));
                console.log('Upload of file #' + id + ' COMPLETED');

                formData.Images.push(data.filePath);

                uiMultiUpdateFileStatus(id, 'success', 'Upload Complete');
                uiMultiUpdateFileProgress(id, 100, 'success', false);
            },
            onUploadError: function (id, xhr, status, message) {
                uiMultiUpdateFileStatus(id, 'danger', message);
                uiMultiUpdateFileProgress(id, 0, 'danger', false);
            },
            onFallbackMode: function () {
                // When the browser doesn't support this plugin :(
                console.log('Plugin cant be used here, running Fallback callback');
            },
            onFileSizeError: function (file) {
                console.log('File \'' + file.name + '\' cannot be added: size excess limit');
            },
            onFileTypeError: function (file) {
                console.log('File \'' + file.name + '\' cannot be added: must be an image (type error)');
            },
            onFileExtError: function (file) {
                console.log('File \'' + file.name + '\' cannot be added: must be an image (extension error)');
            }
        });
    });

    $operationModal.on('show.bs.modal', function (event) {
        console.log('BS EVENT: show.bs.modal');
    });

    $operationModal.on('hidden.bs.modal', function (event) {
        console.log('BS EVENT: hidden.bs.modal');

        $modalFormWithUploader.trigger('reset');

        uiMultiResetFileList();
        $uploader.dmUploader('reset');
        formData.Images.length = 0;
    });

    $modalFormSaveButton.on('click', function(event) {
        $.extend(formData, $modalFormWithUploader.serializeToObject());

        $.ajax({
            type: 'POST',
            url: '/Administration/CreateOperation',
            dataType: 'json',
            contentType: 'application/x-www-form-urlencoded; charset=utf-8',
            data: formData,
            success: function (result) {
                updateOperationsTab();
                formData.Images.length = 0;
            }
        });
    });

    $(document).on('click', '.edit-operation-button', function (event) {
        var operationId = $(event.target).attr('data-operation-id');

        $.ajax({
            type: 'POST',
            url: '/Administration/GetOperation',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: operationId,
            success: function (result) {
                setFormWithData(result);

                $operationModal.modal('show');
            }
        });

        function setFormWithData (data) {
            $('#title-field', $modalFormWithUploader).val(data.title);
            $('#subtitle-field', $modalFormWithUploader).val(data.subtitle);
            $('#description-field', $modalFormWithUploader).val(data.description);

            $.each(data.images, function (index, filePath) {
                uiMultiAddFile(index, { name: filePath }, filePath);
                uiMultiUpdateFileStatus(index, 'success', 'Upload Complete');
                uiMultiUpdateFileProgress(index, 100, 'success', false);
            });
        }
    });

    $(document).on('click', '.delete-operation-button', function (event) {
        var operationId = $(event.target).attr('data-operation-id');

        $.ajax({
            type: 'POST',
            url: '/Administration/DeleteOperation',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: operationId,
            success: function (result) {
                updateOperationsTab();
            }
        });
    });

    function updateOperationsTab() {
        $operationsTab.load('/Administration/GetOperations');
    }

    // Creates a new file and add it to our list
    function uiMultiAddFile(id, file, src) {
        var template = $('#files-template').text();
        template = template.replace('%%filename%%', file.name);

        template = $(template);
        template.prop('id', 'uploaderFile' + id);
        template.data('file-id', id);

        $('#files').find('li.empty').fadeOut(); // remove the 'no files yet'
        $('#files').prepend(template);

        if (src) {
            $('#uploaderFile' + id, '#files').find('img').attr('src', src);
        }
    }

    function uiMultiResetFileList() {
        $('#files').find('li:not(.empty)').remove();
        $('#files').find('li.empty').fadeIn();
    }

    // Changes the status messages on our list
    function uiMultiUpdateFileStatus(id, status, message) {
        $('#uploaderFile' + id).find('span').html(message).prop('class', 'status text-' + status);
    }

    // Updates a file progress, depending on the parameters it may animate it or change the color.
    function uiMultiUpdateFileProgress(id, percent, color, active) {
        color = (typeof color === 'undefined' ? false : color);
        active = (typeof active === 'undefined' ? true : active);

        var bar = $('#uploaderFile' + id).find('div.progress-bar');

        bar.width(percent + '%').attr('aria-valuenow', percent);
        bar.toggleClass('progress-bar-striped progress-bar-animated', active);

        if (percent === 0) {
            bar.html('');
        } else {
            bar.html(percent + '%');
        }

        if (color !== false) {
            bar.removeClass('bg-success bg-info bg-warning bg-danger');
            bar.addClass('bg-' + color);
        }
    }
})(jQuery);