if (jQuery.validator) {
    if (jQuery.validator.regex == undefined) {
        jQuery.validator.addMethod("regex", function (value, element, regexpr) {
            return this.optional(element) || regexpr.test(value);
        }, 'Please enter a valid value.');
    }
}

var ContactForm = ContactForm || function () {
    var formValidator = undefined;

    function addValidation() {
        if ($('#ContactForm').length === 0) {
            return;
        }
        $(document).ready(function () {
            var rules = {
                subject: "required",
                message: "required"
            };

            var messages = {
                subject: "Du måste ange Ämne",
                message: "Du måste ange Meddelande"
            }
            if ($('#loginFlag').val().toString().toLowerCase !== "true") {
                rules.name = "required";
                rules.email = {
                    required: true,
                    email: true
                }
                rules.customerNumber = {
                    required: true,
                    regex: /^(\d+)/
                };

                messages.name = "Du måste ange Ditt namn";
                messages.email = {
                    required: "Du måste ange E-post",
                    email: "Ange en giltig e-postadress"
                };
                messages.customerNumber = {
                    required: "Du måste ange Kundnummer",
                    regex: "Ange en giltig Kundnummer"
                }
            }
            if ($().validate) {
                formValidator = $('#ContactForm').validate({
                    ignore: [],
                    errorElementClass: 'error',
                    errorClass: 'error-item',
                    errorElement: 'span',
                    rules: rules,
                    messages: messages,
                    highlight: function (element, errorClass, validClass) {
                        //console.log("jquery validation");
                        $('ul.errors-list').show();
                        $('li#li_' + $(element).attr('id')).show();
                        $(element).addClass(this.settings.errorElementClass).removeClass(errorClass);
                    },
                    unhighlight: function (element, errorClass, validClass) {
                        $(element).removeClass(this.settings.errorElementClass).removeClass(errorClass);
                        $('li#li_' + $(element).attr('id')).hide();
                        if ($('ul.errors-list').find('li[id^="li"]').is(":visible") === false) {
                            $('ul.errors-list').hide();
                        }
                    }
                });

                $("input[type=reset]").click(function() {
                    if (formValidator) {
                        formValidator.resetForm();

                        // hide all errors
                        $('ul.errors-list').hide();
                        $('.error').each(function() {
                            $(this).removeClass('error');
                        });
                    }
                });
            }
        });
    }


    function init() {
        addValidation();
    }
    return {
        init: init
    }

}();

$(function () {
    ContactForm.init();
});