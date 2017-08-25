if (jQuery.validator) {
    jQuery.validator.addMethod("subItemRequired", function (value, element, method) {
        return method;
    }, '');
    jQuery.validator.addMethod("regex", function (value, element, regexpr) {
        return this.optional(element) || regexpr.test(value);
    }, 'Please enter a valid value.');
}

jQuery(function () {
    MyAccountPage.init();
    HandinSignInInformationPage.init();
    CompanyInformation.init();
    BusinessProfile.init();
    DeliveryAddress.init();
});

var MyAccountPage = MyAccountPage || (function () {

    var addUploadPopupEvent = function () {
        $(document).on("click", function () {
            $('.popup-sub').fadeOut();
        });

        //$(".popup-sub").click(function () { return false; });
        $(".lm__avatar .lm__avatar__upload-btn").click(function () {
            var popupSub = $(this).parent();
            if (popupSub.has('.popup-sub')) {
                popupSub.find('.popup-sub').fadeToggle();
            }
            return false;
        });
    };

    var init = function () {
        addUploadPopupEvent();
        //Init for Profile picture
        var userParamsForUpload = getSettingsForUserUpload();
        var userUploadImageModule = new UploadImageModule(userParamsForUpload);
        userUploadImageModule.initUploadFile();
        userUploadImageModule.initDeleteFile();

        //Init for Customer picture
        var companyUploadSettings = getSettingsForCompanyUpload();
        var companyUploadImageModule = new UploadImageModule(companyUploadSettings);
        companyUploadImageModule.initUploadFile();
        companyUploadImageModule.initDeleteFile();
    };
    function getSettingsForUserUpload() {
        return {
            fileSelector: "#user-avatar__file",
            previewImgSelector: "#user-avatar",
            uploadBtnSelector: "#user-uploadBtn",
            cancelUploadBtnSelector: "#user-cancelUpload",
            linkSelector: "#user-link",
            handleUrl: "/api/profile/user-upload-avatar",
            isNeedToUpdateHeader: true,
            deleteLinkSelector: "#user-deleteBtn",
            handleDeleteUrl: "/api/profile/user-delete-avatar",
            messages: {
                confirmationDialogSelector: "#dialog-user-confirmation"
            }

        };
    }

    function getSettingsForCompanyUpload() {
        return {
            fileSelector: "#company-avatar__file",
            previewImgSelector: "#company-avatar",
            uploadBtnSelector: "#company-uploadBtn",
            cancelUploadBtnSelector: "#company-cancelUpload",
            linkSelector: "#company-link",
            handleUrl: "/api/profile/company-upload-avatar",
            deleteLinkSelector: "#company-deleteBtn",
            handleDeleteUrl: "/api/profile/company-delete-avatar",
            messages: {
                confirmationDialogSelector: "#dialog-company-confirmation"
            }

        };
    }
    return {
        init: init
    };

})();

var HandinSignInInformationPage = HandinSignInInformationPage || (function () {
    var unsaved = false;
    var formSubmitting = false;


    var init = function () {
        var x = $('#hiddenSocialNumber').val();
        if (x && x !== "") {
            $('#hiddenSocialNumber, #SocialSecurityNumber').val('************');
            $('.lm__block .lm__checkbox').show();
        }

        $('#SocialSecurityNumber').change(function () {
            unsaved = true;
        });

        window.onload = function () {
            window.addEventListener("beforeunload", function (e) {
                if (formSubmitting || !unsaved) {
                    return undefined;
                }

                var confirmationMessage = 'It looks like you have been editing something. '
                    + 'If you leave before saving, your changes will be lost.';

                (e || window.event).returnValue = confirmationMessage; //Gecko + IE
                return confirmationMessage; //Gecko + Webkit, Safari, Chrome etc.
            });
        };
    };

    var validateForm = function () {
        var x = $('#SocialSecurityNumber').removeClass('error').val();
        $('#error-message').hide();
        if (x == null || x === "") {
            $('#SocialSecurityNumber').addClass('error');
            $('#error-message').show().html("Personnummer måste fyllas i");
            $('.lm__block .lm__checkbox').hide();
            return false;
        }
        var rex = /^([0-9]{12})$|^([0-9]{10})$/;
        if (!rex.test(x)) {
            $('#SocialSecurityNumber').addClass('error');
            $('#error-message').show().html("Du måste ange 12 siffror (ÅÅÅÅMMDDNNNN)");
            $('.lm__block .lm__checkbox').hide();
            return false;
        }

        formSubmitting = true;
    };

    return {
        init: init,
        validateForm: validateForm
    };

})();

var CompanyInformation = CompanyInformation || (function () {
    var init = function () {
        $(document).ready(function () {
            if ($().validate) $('#editCompanyInformationFrom').validate({
                errorElementClass: 'error',
                errorClass: 'error-item',
                errorElement: 'span',
                rules: {
                    CompanyName: "required",
                    Address: "required",
                    ZipCode: {
                        required: true,
                        zipCode: true
                    },
                    City: "required",
                    PhoneMobile: {
                        required: true,
                        mobileSE: true
                    },
                    PhoneWork: {
                        phoneSE: true
                    },
                    Email: {
                        required: true,
                        email: true
                    }
                },
                messages: {
                    CompanyName: "Du måste ange Företag/namn",
                    Address: "Du måste ange Gatuadress",
                    ZipCode: {
                        required: "Du måste ange Postnummer",
                        zipCode: window["validationMessage"].zipCode.valid
                    },
                    City: {
                        required: "Du måste ange Ort"
                    },
                    PhoneMobile: {
                        required: window["validationMessage"].mobileSE.required,
                        mobileSE: window["validationMessage"].mobileSE.valid
                    },
                    PhoneWork: {
                        phoneSE: window["validationMessage"].phoneSE.valid
                    },
                    Email: {
                        required: "Du måste ange E-post",
                        email: "Ange en giltig e-postadress"
                    }
                },
                highlight: function (element, errorClass, validClass) {
                    $('div.errors-list').hide();
                    $('ul.errors-list').show();
                    $('li#p_' + $(element).attr('id')).show();
                    $(element).addClass(this.settings.errorElementClass).removeClass(errorClass);
                },
                unhighlight: function (element, errorClass, validClass) {
                    $(element).removeClass(this.settings.errorElementClass).removeClass(errorClass);
                    $('li#p_' + $(element).attr('id')).hide();
                    if ($('ul.errors-list').find('li[id^="p"]').is(":visible") === false) {
                        $('ul.errors-list').hide();
                    }
                }
            });
        });
    };
    return {
        init: init
    };
})();

var BusinessProfile = BusinessProfile || (function () {
    var init = function () {
        $(document).ready(function () {
            if ($().validate) $('#editBusinessProfileForm').validate({
                errorElementClass: 'error',
                errorClass: 'error-item',
                errorElement: 'span',
                rules: {
                    "BusinessProfile.Name": "required"
                },
                messages: {
                    "BusinessProfile.Name": "Du måste ange Typ av verksamhet"
                },
                highlight: function (element, errorClass, validClass) {
                    $('div.errors-list').hide();
                    $('ul.errors-list').show();
                    $('li#p_' + $(element).attr('id')).show();
                    $(element).addClass(this.settings.errorElementClass).removeClass(errorClass);
                },
                unhighlight: function (element, errorClass, validClass) {
                    $(element).removeClass(this.settings.errorElementClass).removeClass(errorClass);
                    $('li#p_' + $(element).attr('id')).hide();
                    if ($('ul.errors-list').find('li[id^="p"]').is(":visible") === false) {
                        $('ul.errors-list').hide();
                    }
                }
            });
        });
    };

    return {
        init: init
    };
})();

var DeliveryAddress = DeliveryAddress || function () {

    var addDeleteAddressHandler = function () {
        var $delElement = $("#del-delivery-address");
        if ($delElement.length > 0) {
            $delElement.click(function (e) {
                e.preventDefault();
                var addressNumber = $("#addressNumber").val().toString();
                var handlePageId = $("#handler-delivery-page-id").val().toString();
                var apiUrl = "/api/handle-delivery-address/delete/" + addressNumber + "/" + handlePageId;
                $.ajax({
                    dataType: "html",
                    url: apiUrl,
                    cache: false,
                    success: function (data) {
                        if (!!data && data.toString().trim() !== "") {
                            window.location = data;
                        }
                    }
                });
            });
        }
    };

    function generateSiloRow(rowIdx) {
        var siloBox = "<div class='author-inform-form__column author-inform-form__column-full silos-box gray-background'> </div>";
        var delBtn = "<a href='#' class='silos-box__delete-btn'></a>";
        var silorDesc = " <span>Silo nr*</span> <input type='text' name='Silos[" + rowIdx + "].Number' class='silos silo-number' placeholder='12' /><span>Framkomlighet* </span>";
        var siloAccessibility = " <ul class='lm__form-dropdown type-3'>" +
            "<li class='showcase' data-value='10M'>" +
            "<a href='#' id='selected-silor" + rowIdx + "'>10 meter</a>" +
            "<ul class='dropdown'>" +
            "<li data-value='10M' class='selected'><a href='#'>10 meter</a></li>" +
            "<li data-value='12M'><a href='#'>12 meter</a></li>" +
            "<li data-value='24M'><a href='#'>24 meter</a></li>" +
            "</ul>" +
            "<input type='hidden' class='form-element silo-accessibility' name='Silos[" + rowIdx + "].Accessibility' value='10M' />" +
            "</li>" +
            "</ul>";
        return $(siloBox).append(delBtn, silorDesc, siloAccessibility);
    };

    function setSelectedDropdown() {
        var selectedValue = $("#editDeliveryAddress .lm__form-dropdown .showcase").attr("data-value");
        if (!selectedValue) {
            return;
        }

        var liItems = $(".lm__form-dropdown .dropdown li");
        // reset selected state
        liItems.removeClass("selected");
        liItems.each(function () {
            var data = $(this).attr("data-value");

            if (data == selectedValue) {
                $(this).addClass("selected");
            }
        });
    }

    function addSilo() {
        $('#btn-add-silo').click(function (e) {
            var rowCount = $("div.silos-box").length;
            var silorContainer = $('.silo-container');
            var newRow = generateSiloRow(rowCount);
            silorContainer.append(newRow);
            setDropdown($(".lm__form-dropdown"), 'type-3');

            addRemoveSiloHandler();

            e.preventDefault();
        });
    }

    function addRemoveSiloHandler() {
        $(".silos-box__delete-btn").click(function (e) {
            e.preventDefault();
            $(this).closest('div.silos-box').remove();
            $('input.silo-number').each(function (idx, item) {
                $(this).attr('name', 'Silos[' + idx + '].Number');
            });
            $('input.silo-accessibility').each(function (idx, item) {
                $(this).attr('name', 'Silos[' + idx + '].Accessibility');
            });
        });
    }
    function showOrHideDialog(name) {
        var indicatorId = "#has-" + name + "-indicator";
        var $itemIndicator = $(indicatorId);
        if ($itemIndicator && $itemIndicator.length > 0) {
            var updateSuccess = $itemIndicator.val().toString().toLowerCase() === "true";
            if (updateSuccess) {
                //show success dialog
                var modalId = "#modal-" + name + ".lm__information-modal__wrapper";
                var modal = $(modalId);
                modal.removeClass("hidden");
            }
        }
    }
    function addNotificationReceiverHandler() {
        var $receiverCheckBox = $('input.notification-receiver-handler');
        if (!!$receiverCheckBox && $receiverCheckBox.length > 0) {
            $receiverCheckBox.click(function () {
                $(this).val($(this).is(":checked"));
            });
        }
    }

    function init() {
        showOrHideDialog("added");
        showOrHideDialog("updated");
        showOrHideDialog("deleted");

        if (!$("#editDeliveryAddress") || $("#editDeliveryAddress").length == 0) {
            return;
        }

        setSelectedDropdown();
        addSilo();
        addRemoveSiloHandler();
        addDeleteAddressHandler();
        addNotificationReceiverHandler();
    }
    return {
        init: init
    }

}();

