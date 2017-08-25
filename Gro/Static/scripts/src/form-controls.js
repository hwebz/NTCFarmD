function setDropdown(obj, type) {
    obj.find(".showcase > a").unbind('click').click(function () {
        var sub = $(this).next();
        $(".dropdown").not(sub).hide();
        if (sub.css('display') == 'none') {
            sub.show();
        } else {
            sub.hide();
        }
        return false;
    });

    obj.find(".showcase .dropdown li a").click(function () {
        var data = $(this).parent().attr("data-value");
        var dataText = $(this).text();
        $(".dropdown li").removeClass("selected");
        $(this).parent().addClass("selected");
        $(this).parents(".showcase").find(">a").html((type === "type-2") ? data : dataText);
        $(this).parents(".showcase").find(">a").html(dataText);
        $(this).parents(".showcase").attr("data-value", data);
        //$(this).parents(".showcase").find("input.form-element").val(data);
        $(this).parents(".showcase").find(">input[type=hidden]").val(data);
        $(this).parents(".dropdown").hide();

        if ($(this).parent().data('value') === 'Annan') {
            $(this).parents('.model-block').find('input[type="text"]').show();
            $(this).parents('ul.lm__form-dropdown.model-dropdown').hide();
        }
        return false;
    });
}

$(function () {
    function addDataPickerHandler() {
        if ($.fn.datepicker) {
            // Set default options for datepicker
            $.datepicker.setDefaults({
                monthNamesShort: ["Jan", "Feb", "Mar", "Apr", "Maj", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov", "Dec"],
                monthNames: ['Januari', 'Februari', 'Mars', 'April', 'Maj', 'Juni', 'Juli', 'Augusti', 'September', 'Oktober', 'November', 'December'],
                dayNamesMin: ["Sö", "Må", "Ti", "On", "To", "Fr", "Lö"],
                firstDay: 0,
                showOtherMonths: true,
                selectOtherMonths: true,
                dateFormat: "yy-mm-dd"
            });

            $(".datepicker").datepicker({
                defaultDate: new Date(),
                onSelect: function (newText) {
                    // compare the new value to the previous one
                    if ($(this).data('previous') != newText) {
                        // do whatever has to be done, e.g. log it to console
                        //console.log('changed to: ' + newText);
                    }
                },
            monthNames: ["Januari", "Februari", "Mars", "April", "Maj", "Juni", "Juli", "Augusti", "September", "Oktober", "November", "December"],
            dayNamesMin: ["Sö", "Må", "Ti", "On", "To", "Fr", "Lö"],
            firstDay: 1
            }).datepicker("setDate", new Date())
            .on('change', function () {
                var $this = $(this);
                var validDate = !/Invalid|NaN/.test(new Date($this.val()).toString());
                var validDateRegex = /^\d\d\d\d-(0?[1-9]|1[0-2])-(0?[1-9]|[12][0-9]|3[01])$/ig.test($this.val());

                if (!validDateRegex || !validDate) {
                    //$this.addClass("error").next().show();
                    $this.next().show();
                } else {
                    //$this.removeClass("error").next().hide();
                    $this.next().hide();
                }
            });
        }
    }

    function addDropDownHandler() {
        var dropdown = $(".lm__form-dropdown");

        $(document).on('click touchstart', function (e) {
            var $this = $(e.target);
            if ($this.attr('class') == 'dropdown' || $($this).parents('.lm__form-dropdown').length > 0) {
                return;
            } else {
                $(".dropdown").hide();
            }
        });

        dropdown.each(function () {
            $(this).find(".showcase > a").click(function () {
                var sub = $(this).next();
                $(".dropdown").not(sub).hide();
                if (sub.css('display') == 'none') {
                    sub.show();
                } else {
                    sub.hide();
                }
                return false;
            });

            setDropdown($(this), 'type-2');
            setDropdown($(this), 'type-3');
        });
    }

    addDataPickerHandler();
    addDropDownHandler();
});
