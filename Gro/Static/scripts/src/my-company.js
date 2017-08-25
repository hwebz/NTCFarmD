$(function () {

    var MyCompany = function () {

        var accordionLinks = $(".lm__accordion-wrapper .lm__accordion-link");
        var accordionContents = $(".lm__accordion-wrapper .lm__accordion-content");

        var addAccordionEvent = function () {
            accordionLinks.each(function () {
                $(this).click(function () {
                    if (!$(this).hasClass('active')) {
                        accordionContents.slideUp('fast', function () {
                            $(this).prev().removeClass("active");
                        });

                        $(this).next().slideDown('fast', function () {
                            $(this).prev().addClass("active");
                        });
                    } else {
                        $(this).next().slideUp('fast', function () {
                            $(this).prev().removeClass("active");
                        });
                    }
                });
            });
        };

        return {
            init: function () {
                addAccordionEvent();
                return false;
            }
        }
    };

    $(document).ready(function () {
        var myCom = new MyCompany();
        myCom.init();
    });

});