(function ($) {
    'use strict';

    var $operationsTab = $('#tab-content-1');
    var $operationModal = $('#operation-modal');
    var $modalFormWithUploader = $('#modal-form-with-uploader', $operationModal);
    var $uploader = $('#images-field', $modalFormWithUploader);
    var $modalFormSaveButton = $('#modal-form-save', $operationModal);
    var $modalFormCloseButton = $('#modal-form-close', $operationModal);

    var pendingFormData = {
        Images: []
    };
    var actionType = 'discard';

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
                uiMultiUpdateFileProgress(id, 0, '', true);
                uiMultiUpdateFileStatus(id, 'uploading', 'Uploading...');
            },
            onUploadProgress: function (id, percent) {
                // Updating file progress
                uiMultiUpdateFileProgress(id, percent);
            },
            onUploadSuccess: function (id, data) {
                // A file was successfully uploaded
                pendingFormData.Images.push({ path: data.filePath, shouldSave: true });
                uiMultiSetFilePath(id, data.filePath);

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

    $operationModal.on('hide.bs.modal', function (event) {
        var imagesArray = pendingFormData.Images
            .filter(function(item) { return item.shouldSave; })
            .map(function (item) { return item.path; });

        if (imagesArray.length > 0) {
            $.ajax({
                type: 'POST',
                url: '/Administration/DiscardFiles',
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(imagesArray),
                success: function (result) {
                    actionType = 'discard';
                }
            });
        }

        $modalFormWithUploader.trigger('reset');
        uiMultiResetFileList();
        $uploader.dmUploader('reset');
        $('.modal-backdrop').remove();
        pendingFormData.Images.length = 0;
    });

    $modalFormSaveButton.on('click', function (event) {
        if (actionType === 'update') {
            var deleteImagesArray = pendingFormData.Images
                .filter(function (item) { return item.shouldDelete; })
                .map(function (item) { return item.path; });

            if (deleteImagesArray.length > 0) {
                $.ajax({
                    type: 'POST',
                    url: '/Administration/DiscardFiles',
                    dataType: 'json',
                    contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                    data: { filePaths: deleteImagesArray, shouldPersist: true }
                });
            }

            var saveImagesArray = pendingFormData.Images
                .filter(function (item) { return item.shouldSave; })
                .map(function (item) { return item.path; });

            var operationId = $('.edit-operation-button').attr('data-operation-id');
            var saveDataToSend = $.extend(
                {
                    Id: operationId,
                    Images: saveImagesArray
                },
                $modalFormWithUploader.serializeToObject());

            $.ajax({
                type: 'POST',
                url: '/Administration/UpdateOperation',
                dataType: 'json',
                contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                data: saveDataToSend,
                success: function (result) {
                    updateOperationsTab();

                    actionType = 'discard';
                    pendingFormData.Images.length = 0;
                    $operationModal.modal('hide');
                }
            });
        } else if(actionType === 'create') {
            var imagesArray = pendingFormData.Images
                .filter(function (item) { return item.shouldSave; })
                .map(function (item) { return item.path; });

            var dataToSend = $.extend({ Images: imagesArray }, $modalFormWithUploader.serializeToObject());

            $.ajax({
                type: 'POST',
                url: '/Administration/CreateOperation',
                dataType: 'json',
                contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                data: dataToSend,
                success: function (result) {
                    updateOperationsTab();

                    actionType = 'discard';
                    pendingFormData.Images.length = 0;
                    $operationModal.modal('hide');
                }
            });
        }
    });

    $(document).on('click', '#create-operation-button', function (event) {
        actionType = 'create';
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
                actionType = 'update';
                populateFormWithData(result);
                $operationModal.modal('show');
            }
        });

        function populateFormWithData (data) {
            $('#title-field', $modalFormWithUploader).val(data.title);
            $('#subtitle-field', $modalFormWithUploader).val(data.subtitle);
            $('#description-field', $modalFormWithUploader).val(data.description);

            $.each(data.images, function (index, filePath) {
                uiMultiAddFile(index, { name: filePath }, filePath);
                uiMultiSetFilePath(index, filePath);
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
                actionType = 'discard';
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

        var uploaderFileId = 'uploaderFile' + id;
        template = $(template);
        template.prop('id', uploaderFileId);
        template.data('file-id', id);

        $('#files').find('li.empty').fadeOut(); // remove the 'no files yet'
        $('#files').prepend(template);

        $('#files ' + '#' + uploaderFileId).on('click', '.close', function (event) {
            var $li = $(event.target).closest('.media');
            var fileId = $li.attr('id');
            var filePaths = [];
            var filePath = $li.data('file-path');
            filePaths.push(filePath);

            var index = pendingFormData.Images.findIndex(function (item) {
                return item.path === filePath;
            });

            if (actionType === 'create') {
                $.ajax({
                    type: 'POST',
                    url: '/Administration/DiscardFiles',
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify(filePaths)
                });

                pendingFormData.Images.splice(index, 1);
            } else if (actionType === 'update') {
                if (index > -1) {
                    pendingFormData.Images[index].shouldSave = false;
                    pendingFormData.Images[index].shouldDelete = true;
                } else {
                    pendingFormData.Images.push({ path: filePath, shouldDelete: true });
                }
            }

            uiMultiRemoveFile(fileId);
        });

        if (src) {
            $('#uploaderFile' + id, '#files').find('img').attr('src', src);
        }
    }

    function uiMultiSetFilePath(id, filePath) {
        $('#uploaderFile' + id, '#files').data('file-path', filePath);
    }

    function uiMultiRemoveFile (id) {
        $('#' + id, '#files').remove();

        if ($('#files').find('li:not(.empty)').length === 0) {
            $('#files').find('li.empty').fadeIn();
        }
    }

    function uiMultiResetFileList() {
        $('#files').find('li:not(.empty)').remove();
        $('#files').find('li.empty').fadeIn();
    }

    // Changes the status messages on our list
    function uiMultiUpdateFileStatus(id, status, message) {
        $('#uploaderFile' + id).find('span[data-id="status-label"]').html(message).prop('class', 'status text-' + status);
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