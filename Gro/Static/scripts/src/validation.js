window.validation = {
    phoneSE: function (phoneNumber) {
        phoneNumber = phoneNumber.replace(/\s+/g, "");
        return phoneNumber.match(/^(\+46)\d{6,}$/);
    },

    mobileSE: function (phoneNumber) {
        phoneNumber = phoneNumber.replace(/\s+/g, "");
        return phoneNumber.match(/^(\+46)\d{9}$/);
    },

    zipCode: function (zipCode) {
        zipCode = zipCode.replace(/\s+/g, "");
        return zipCode.match(/^(\d{5}$)/);
    },

    personNumber: function (personNumber) {
        personNumber = personNumber.replace(/\s+/g, "");
        return personNumber.match(/^\d{12}$/);
    },
    swedishDate:function(swedishDate) {
        var swedishDateRegex = /^(\d{4})(\/|-)(\d{1,2})(\/|-)(\d{1,2})$/;
        return swedishDate.match(swedishDateRegex) && ((new Date(swedishDate)) !="Invalid Date");
    }
};

window.validationMessage = {
    firstName: {
        required: "Du måste ange Förnamn"
    },
    lastName: {
        required: "Du måste ange Efternamn"
    },
    phoneSE: {
        valid: "Ange ett korrekt telefonnummer"
    },
    mobileSE: {
        required: "Du måste ange Mobilnr",
        valid: "Ange ett korrekt mobilnummer"
    },
    zipCode: {
        required: "Postnumret ska bestå av 5 siffror",
        valid: "Postnumret ska bestå av 5 siffror"
    },
    email: {
        required: "Du måste ange E-post",
        valid: "Ange en giltig e-postadress"
    },
    personNumber: {
        valid: "Personnumret ska bestå av 12 siffror"
    },
    swedishDate: {
        required:"Du måste ange ett giltigt datum",
        valid: "Du måste ange ett giltigt datum"
    }
};

$(function () {
    if (!jQuery.validator) {
        return;
    }

    $.validator.addMethod("phoneSE",
        function (phoneNumber, element) {
            return this.optional(element) || window.validation.phoneSE(phoneNumber);
        },
        window.validationMessage.phoneSE.valid);

    $.validator.addMethod("mobileSE",
        function (phoneNumber, element) {
            return window.validation.mobileSE(phoneNumber);
        },
        window.validationMessage.mobileSE.valid);

    $.validator.addMethod("zipCode",
        function (zipCode, element) {
            return this.optional(element) || window.validation.zipCode(zipCode);
        },
        window.validationMessage.zipCode.valid);

    $.validator.addMethod("personNumber",
        function (pn, element) {
            return this.optional(element) || window.validation.personNumber(pn);
        },
        window.validationMessage.personNumber.valid);

    $.validator.addMethod("swedishDate",
       function (pn, element) {
           return window.validation.swedishDate(pn);
       },
       window.validationMessage.swedishDate.valid);
});
