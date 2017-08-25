if (jQuery.validator) {

    function isValidate(selector) {
        var isValid = $(selector).val() !== "True";
        if (!isValid) {
            $(selector).val('False');
        }
        return isValid;
    }

    jQuery.validator.addMethod("CustomerNumberNotMatch", function (value, element, selector) {
        return isValidate(selector);
    }, "Kundnumret du har angett finns inte registrerat hos Lantmännen för det organisationsnummer du har angett.");
    jQuery.validator.addMethod("CustomerNumberNotExist", function (value, element, selector) {
        return isValidate(selector);
    }, 'Kundnumret finns inte registrerat hos Lantmännen.');
    jQuery.validator.addMethod("CustomerNumberActivated", function (value, element, selector) {
        return isValidate(selector);
    }, 'Kundnumret och organisationsnumret  du har angett är redan aktiverat för ett konto i LM2.');
    jQuery.validator.addMethod("OrganisationNumberNotMatch", function (value, element, selector) {
        return isValidate(selector);
    }, 'Organisationsnumret du har angett stämmer inte överens med kundnumret du har angett.');
    jQuery.validator.addMethod("OrganisationNumberActivated", function (value, element, selector) {
        return isValidate(selector);
    }, 'Organisationsnumret och kundnumret du har angett är redan aktiverat för ett konto i LM2.');
}

jQuery(function () {
    ActiveFarmdayCustomer.init();
    ActiveFarmdayCustomer.initEvent();
    ActiveFarmdayCustomer.initValidateTermOfUseForm();
});

var ActiveFarmdayCustomer = ActiveFarmdayCustomer || (function () {
    var init = function() {
        if ($().validate) $('#verifyCustomer').validate({
            errorElementClass: 'error',
            errorClass: 'error-item',
            errorElement: 'span',
            rules: {
                CustomerNumber: {
                    required: true,
                    CustomerNumberNotMatch: '#CustomerNumberNotMatch',
                    CustomerNumberNotExist: '#CustomerNumberNotExist',
                    CustomerNumberActivated: '#CustomerNumberActivated'
                },
                OrganisationNumber: {
                    required: true,
                    regex: /^\d{6}[-]{0,1}\d{4}$/,
                    OrganisationNumberNotMatch: '#OrganisationNumberNotMatch',
                    OrganisationNumberActivated: '#OrganisationNumberActivated'
                }
            },
            messages: {
                CustomerNumber: {
                    required: "Du måste ange Kundnumret"
                },
                OrganisationNumber: {
                    required: "Du måste ange Organisationsnumret",
                    regex: "Organisationsnumret du har angett är av fel typ."
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
            },
            submitHandler: function(form) {
                $('#loader').show();
                $('#loader').parent().addClass('disabled');
                form.submit();
            }
        });

        if ($().validate) {
            var isInvalid = $('#CustomerNumberNotMatch').val() === "True" ||
                $('#CustomerNumberNotExist').val() === "True" ||
                $('#CustomerNumberActivated').val() === "True" ||
                $('#OrganisationNumberNotMatch').val() === "True" ||
                $('#OrganisationNumberActivated').val() === "True";
            if (isInvalid) {
                $("#verifyCustomer").valid();
            }
        }
    }

    var initEvent = function () {
        $('#expandTermOfUse').click(function () {
            $(this).toggleClass('expanded').next().toggle();
        });

        $(document).on('change', '#acceptTerm', function() {
            if ($(this).is(':checked')) {
                $('.success-icon').show().parent().next().html('Du har godkänt <a href="#" class="lm__link">användningsvillkoren</a> för LM<sup>2</sup>.');
                $('#submitBtn').removeClass('disabled-btn');
                $('#expandTermOfUse').toggleClass('expanded').next().toggle();
            } else {
                $('.success-icon').hide().parent().next().html('Du måste läsa och godkänna <a href="#" class="lm__link">användningsvillkoren</a> för att registrera ett LM<sup>2</sup>-konto');;
                $('#submitBtn').addClass('disabled-btn');
            }

        });
    }


    var initValidateTermOfUseForm = function() {
        if ($().validate)
            $('#registerCustomer').validate({
                errorElementClass: 'error',
                errorClass: 'error-item',
                errorElement: 'span',
                rules: {
                    ContractApplication: {
                        required: true
                    },
                    CopyOfIDDocuments: {
                        required: true
                    },
                    RegistrationCertificate: {
                        required: true
                    },
                    FirstName: {
                        required: true
                    },
                    SurName: {
                        required: true
                    },
                    Telephone: {
                        regex: /^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$/
                    },
                    Mobile: {
                        required: true,
                        regex: /^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$/
                    },
                    PersonNumber: {
                        required: true,
                        regex: /^[0-9]{12}$/
                    }
                },
                messages: {
                    ContractApplication: {
                        required: "Du måste bifoga ansökan om avtal för e-tjänster"
                    },
                    CopyOfIDDocuments: {
                        required: "Du måste bifoga Kopia på ID-handlingar"
                    },
                    RegistrationCertificate: {
                        required: "Du måste bifoga Registreringsbevis för ditt företag"
                    },
                    FirstName: {
                        required: "Förnamn är obligatorisk"
                    },
                    SurName: {
                        required: "Efternamn är obligatoriskt"
                    },
                    Telephone: {
                        regex: "Ange en giltig telefonnummer"
                    },
                    Mobile: {
                        required: "Mobilnr är obligatoriskt",
                        regex: "Ange ett giltigt mobilnummer"
                    },
                    PersonNumber: {
                        required: "Personnr är obligatoriskt",
                        regex: "Personnr måste bestå av 12 siffror"
                    }
                },
                highlight: function (element, errorClass, validClass) {
                    $('div.errors-list').hide();
                    $('ul.errors-list').show();
                    $('li#p_' + $(element).attr('id')).show();
                    var typeElement = $(element).attr('type');
                    if (typeElement === 'file') {
                        $(element).parent().addClass(this.settings.errorElementClass).removeClass(errorClass);
                    } else {
                        $(element).addClass(this.settings.errorElementClass).removeClass(errorClass);
                    }

                },
                unhighlight: function (element, errorClass, validClass) {
                    var typeElement = $(element).attr('type');
                    if (typeElement === 'file') {
                        $(element).parent().removeClass(this.settings.errorElementClass).removeClass(errorClass);
                    } else {
                        $(element).removeClass(this.settings.errorElementClass).removeClass(errorClass);
                    }

                    $('li#p_' + $(element).attr('id')).hide();
                    if ($('ul.errors-list').find('li[id^="p"]').is(":visible") === false) {
                        $('ul.errors-list').hide();
                    }
                },
                errorPlacement: function (error, element) {
                    var typeElement = element.attr('type');
                    if (typeElement === 'file') {
                        element.parent().after(error);
                    } else {
                        element.after(error);
                    }

                }
            });
    }

    return {
        init: init,
        initEvent: initEvent,
        initValidateTermOfUseForm: initValidateTermOfUseForm
    };
})();