if (!Array.prototype.find) {
    Array.prototype.find = function (predicate) {
        'use strict';
        if (this == null) {
            throw new TypeError('Array.prototype.find called on null or undefined');
        }
        if (typeof predicate !== 'function') {
            throw new TypeError('predicate must be a function');
        }
        var list = Object(this);
        var length = list.length >>> 0;
        var thisArg = arguments[1];
        var value;

        for (var i = 0; i < length; i++) {
            value = list[i];
            if (predicate.call(thisArg, value, i, list)) {
                return value;
            }
        }
        return undefined;
    };
}

if (!Date.prototype.toShortDateString) {
    Date.prototype.toShortDateString = function () {
        return this.getFullYear() +
            "-" +
            ("0" + (this.getMonth() + 1)).slice(-2) +
            "-" +
            ("0" + this.getDate()).slice(-2);
    }
}

var ImagePlaceHolder = function () {
    var elements = $("img[data-img-source]");
    elements.each(function () {
        var element = this;
        $(element).attr("src", element.dataset["imgSource"]);
        $(element).on("error",
            function (event) {
                $(event.target).on("error", null);
                if (event.target.dataset["placeholder"]) {
                    $(event.target).attr("src", event.target.dataset["placeholder"]);
                }
            });
    });
}

var UploadImageModule = function (options) {
    var settings = {
        fileSelector: "",
        previewImgSelector: "",
        uploadBtnSelector: "",
        linkSelector: "",
        handleUrl: "",
        cancelUploadBtnSelector: "",
        deleteLinkSelector: "",
        handleDeleteUrl: "",
        errorMessage: "",
        prevImageData: "",
        isNeedToUpdateHeader: false,

        messages: {
            confirmationDialogSelector: "#dialog-confirmation",
            delConfirmationMess: "Är du säker på att du vill ta bort bilden?",
            yesDelBtnText: "Ja, ta bort",
            noDelBtnText: "Nej, spara den",

            uploadingErrorMessage: "Något gick fel när du försökte ladda upp bilden. Vänligen försök igen.",
            uploadingErrorTitle: "Bilden kunde inte laddas upp",

            informationDialogSelector: "#informationDialog"
        }
        //addition for dialogs
        //confirmationDialogSelector: "#dialog-confirmation",
        //delConfirmationMess: "Är du säker på att du vill ta bort bilden?",
        //yesDelBtnText: "Ja, ta bort",
        //noDelBtnText: "Nej, spara den"
    };
    var _prevImageData;
    var _data = []; //add params to request following format [{"key": "id", "value": 1},{"key": "name", "value": "abc"}]

    $.extend(settings, options);

    var buildParamsForRequest = function () {
        var data = new FormData();
        if (_data && _data.length > 0) {
            for (var i = 0; i < _data.length; i++) {
                var param = _data[i];
                if (param.key && param.value) {
                    data.append(param.key, param.value);
                }
            }
        }
        return data;
    }

    var attachFileOnChangeEvent = function () {
        $(settings.fileSelector).change(function () {
            var file = this.files[0];
            var imagefile = file.type;
            var match = ["image/jpeg", "image/png", "image/jpg"];
            if (!((imagefile === match[0]) || (imagefile === match[1]) || (imagefile === match[2]))) {
                $(settings.previewImgSelector).attr('src', _prevImageData);
                return false;
            } else {
                var reader = new FileReader();
                reader.onload = function (event) {
                    $(settings.previewImgSelector).attr('src', event.target.result);
                    //show Cancel/Upload buttons
                    $(settings.uploadBtnSelector).parent().show();

                };
                reader.readAsDataURL(this.files[0]);
            }
        });
    };

    var attachOpenFileDialog = function () {
        $(settings.linkSelector).on('click',
            function (e) {
                $('.popup-sub, .list-action-item').hide();
                e.preventDefault();
                $(settings.fileSelector).trigger('click');
            });
    }

    var attachUploadFileClickEvent = function () {
        $(settings.uploadBtnSelector).on('click', function () {
            var data = buildParamsForRequest();
            var files = $(settings.fileSelector).get(0).files;

            // Add the uploaded image content to the form data collection
            if (files.length > 0) {
                var fileSize = files[0].size;
                if (fileSize > 4096000) {
                    showInformationDialog("", "Filen är för stor. Maximal filstorlek 4MB.");
                    return;
                }
                data.append("UploadedImage", files[0]);
            }
            $(settings.previewImgSelector).prev().show();
            $(settings.uploadBtnSelector).parent().hide();
            $.ajax({
                url: settings.handleUrl,
                type: "POST",
                data: data,
                contentType: false, // The content type used when sending data to the server.
                cache: false,
                processData: false, // To send DOMDocument or non processed data file it is set to false
                success: function (result) {
                    $(settings.previewImgSelector).prev().hide();

                    if (result.success) {
                        $(settings.uploadBtnSelector).parent().hide();
                        $(settings.deleteLinkSelector).show();

                        _prevImageData = $(settings.previewImgSelector).attr("src");
                        if (settings.isNeedToUpdateHeader) {
                            $('.lm__user-avatar-wrapper .header_user_avatar').attr('src', _prevImageData);
                        }

                        MachineDetailPage.autoFitImages();
                    } else {
                        $(settings.previewImgSelector).attr("src", _prevImageData);
                        //var message = settings.errorMessage!=="" ? settings.errorMessage : "Det uppstod ett fel när bilden skulle laddas upp. Var god försök igen!";
                        showInformationDialog(settings.messages.uploadingErrorTitle, settings.messages.uploadingErrorMessage);
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $(settings.previewImgSelector).prev().hide();
                    $(settings.previewImgSelector).attr("src", _prevImageData);
                    var message = "Det uppstod ett fel när bilden skulle laddas upp: " + errorThrown.toString();
                    showInformationDialog(this.settings.uploadingErrorTitle, message);
                }
            });
        });
    }

    var attactCancelUploadEvent = function () {
        $(settings.cancelUploadBtnSelector).on('click', function () {
            //$(_fileSelector).val('');
            $(settings.fileSelector).val('');
            $(settings.uploadBtnSelector).parent().hide();
            $(settings.previewImgSelector).attr("src", _prevImageData);
        });
    }
    var deleteImage = function () {
        $(settings.previewImgSelector).prev().show();
        $('.popup-sub, .list-action-item').hide();

        var data = buildParamsForRequest();

        $.ajax({
            url: settings.handleDeleteUrl,
            type: "POST",
            data: data,
            contentType: false, // The content type used when sending data to the server.
            cache: false,
            processData: false, // To send DOMDocument or non processed data file it is set to false
            success: function (result) {
                $(settings.previewImgSelector).prev().hide();
                if (result.success) {
                    _prevImageData = result.imageUrl;

                    $(settings.deleteLinkSelector).hide();
                    $(settings.previewImgSelector).attr("src", _prevImageData);

                    if (this.settings.isNeedToUpdateHeader) { //only using for update user avatar on header
                        $('.lm__user-avatar-wrapper .header_user_avatar').attr('src', _prevImageData);
                    }
                } else {
                    $(settings.previewImgSelector).attr("src", _prevImageData);
                    //alert("Det uppstod ett fel när bilden skulle laddas upp. Var god försök igen!");
                    showInformationDialog("", "Det uppstod ett fel när bilden skulle laddas upp. Var god försök igen!");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $(settings.previewImgSelector).prev().hide();
                $(settings.previewImgSelector).attr("src", _prevImageData);
                //alert("Det uppstod ett fel när bilden skulle tas bort: " + errorThrown.toString());
                var message = "Det uppstod ett fel när bilden skulle tas bort: " + errorThrown.toString();
                showInformationDialog(message);
            }
        });
    }

    var attachDeleteFileClickEvent = function () {
        $(settings.deleteLinkSelector).on('click', function () {
            showDelConfirmationDialog(deleteImage);
        });
    }


    function showDelConfirmationDialog(callback) {
        var $dialog = $(settings.messages.confirmationDialogSelector);
        if ($dialog.length === 0) {
            return;
        }
        $(".success-header-title", $dialog).html(settings.delConfirmationMess);
        var $yesBtn = $(".success-confirm-inform.yes", $dialog);
        var $noBtn = $(".success-confirm-inform.no", $dialog);
        $noBtn.html(settings.noDelBtnText);
        $yesBtn.html(settings.yesDelBtnText);

        $yesBtn.click(function () {
            callback();
        });

        $dialog.fadeIn();
    }
    var initDataForRequest = function (data) {
        _data = data;
    }

    var initUploadFile = function (options) {
        if (!options) {
            $.extend(settings, options);
        }
        _prevImageData = $(settings.previewImgSelector).attr("src");

        attachOpenFileDialog();
        attachFileOnChangeEvent();
        attachUploadFileClickEvent();
        attactCancelUploadEvent();
    };

    var initDeleteFile = function (options) {
        if (!options) {
            $.extend(settings, options);
        }

        attachDeleteFileClickEvent();
    }

    function showInformationDialog(title, message) {
        var $informationDialog = $(settings.messages.informationDialogSelector);
        $informationDialog.find('#dialogTitle').html(title);
        $informationDialog.find('#dialogContent').html(message);
        if ($informationDialog.hasClass('hidden')) {
            $informationDialog.removeClass('hidden');
        } else {
            $informationDialog.fadeIn();
        }
    }

    return {
        initDataForRequest: initDataForRequest,
        initUploadFile: initUploadFile,
        initDeleteFile: initDeleteFile
    };
};

var GroCommon = GroCommon ||
(function () {

    var dateFromISO = function (s) {
        s = s.split(/\D/);
        return new Date(Date.UTC(s[0], --s[1] || '', s[2] || '', s[3] || '', s[4] || '', s[5] || '', s[6] || ''));
    };

    var parseInt32 = function (s) {
        var value = parseInt(s);
        if (isNaN(value)) value = 0;

        return value;
    }

    var blockUI = function () {
        //Block UI
    };

    var unblockUI = function () {
        //Unblock UI
    };
    var handleExternalLink = function () {
        var $a = $('a');
        $a.each(function () {
            var href = $(this).attr('href');
            var target = $(this).attr("target");

            if (!!target) {
                return;
            }

            var isExternalLink = href != undefined && href.trim().indexOf("/") !== 0 && href.trim() !== "#" && href.trim().indexOf("javascript:void") && href.trim().indexOf("javascript:window.print()") < 0;
            if (isExternalLink) {
                $(this).attr("target", "_blank");
            }
        });
    }

    var getIdxOfElement = function ($element, $array) {
        return $array.index($element);
    };

    var disableOrEnableElement = function (elementSelector, $currentInput) {
        var agreementElements = $(elementSelector);
        var $currentElement = $currentInput.closest(elementSelector);
        if (agreementElements.length > 0) {
            var idx = getIdxOfElement($currentElement, $(agreementElements));
            if (idx >= 0 && idx < agreementElements.length - 1) {
                // enable the element right after the current element.
                var $nextElement = $(agreementElements[idx + 1]);
                if ($nextElement.hasClass('disabled')) {
                    $nextElement.removeClass('disabled');
                }
            }
        }
    }

    return {
        blockUI: blockUI,
        unblockUI: unblockUI,
        handleExternalLink: handleExternalLink,
        dateFromISO: dateFromISO,
        parseInt: parseInt32,
        disableOrEnableElement: disableOrEnableElement
    };
})();

$(function () {
    ImagePlaceHolder();
});

$(function () {
    $("input[type=button].submit").click(function() {
        var form = $(event.target).closest("form");
        if (form) {
            $(form).submit();
        }
    });

    $("form").keyup(function (event) {
        if (event.which === 13) {
            // key enter
            var form = $(event.target).closest("form");
            if (form) {
                var submitBtn = getSubmitBtn(form);
                if (submitBtn !== undefined) {
                    $(submitBtn).click();
                }
            }
        }
    });
    function getSubmitBtn(form) {
        var btnSubmit = $("button.submit.trigger-on-enter", $(form));
        if (btnSubmit.length > 0) {
            return btnSubmit;
        }

        var btnTypeSubmit = $("button[type=submit].trigger-on-enter", $(form));
        if (btnTypeSubmit.length > 0) {
            return btnTypeSubmit;
        }

        var inputSubmit = $("input[type=submit].trigger-on-enter", $(form));
        if (inputSubmit.length > 0) {
            return inputSubmit;
        }
        return undefined;
    }

});