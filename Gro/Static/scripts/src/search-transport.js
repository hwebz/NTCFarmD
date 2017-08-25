$(function () {
    if ($.fn.datepicker) {
        $(".datepicker-blank-default").datepicker({
            onSelect: function(newText) {
                if ($(this).is("#selectedDate")) {
                    if ($(this).data("previous") !== newText) {
                        var timeBooking = new TimeBookingPage();
                        timeBooking.searchItems("", $("#resourceDrp").attr("data-value"));
                        timeBooking.searchAvalibleSlotAction();
                    }

                }
                var error = $("#error-fromDate");
                if ($(this).is("#fromDateTxt")) {
                    var toDateValue = $("#toDateTxt").val();
                    if (validateDateValue(newText, toDateValue) === 1) {
                        error.show();
                    } else if (validateDateValue(newText, toDateValue) === -1) {
                        $("#toDateTxt").val(newText);
                        error.hide();
                    } else {
                        error.hide();
                    }
                }
                if ($(this).is("#toDateTxt")) {
                    var fromDateValue = $("#fromDateTxt").val();
                    if (validateDateValue(fromDateValue, newText) === 1) {
                        error.show();
                    } else {
                        error.hide();
                    }
                }
            },
            monthNamesShort: ["Jan", "Feb", "Mar", "Apr", "Maj", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov", "Dec"],
            dayNamesMin: ["Sö", "Må", "Ti", "On", "To", "Fr", "Lö"],
            firstDay: 1,
            dateFormat: "yy-mm-dd",
            beforeShow: function () {
                setTimeout(function () {
                    $('.ui-datepicker').css('z-index', 5);
                }, 0);
                if ($(this).is("#toDateTxt")) {
                    var fromDateValue = $("#fromDateTxt").val();
                    var toDateValue = $("#toDateTxt").val();
                    $(this).datepicker('option', 'minDate', new Date(fromDateValue));
                    $("#toDateTxt").val(toDateValue);
                }
                if ($(this).is("#selectedDate")) {
                    $(this).datepicker('option', 'minDate', new Date());
                }
            }
        }).on("change", function () {
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

        $(".datepicker-blank-dialog").datepicker({
            onSelect: function (newText) {
                if ($(this).is("#selectedDate")) {
                    if ($(this).data("previous") !== newText) {
                        var timeBooking = new TimeBookingPage();
                        timeBooking.searchItems("", $("#resourceDrp").attr("data-value"));
                        timeBooking.searchAvalibleSlotAction();
                    }

                }
                var error = $("#error-fromDate");
                if ($(this).is("#fromDateTxt")) {
                    var toDateValue = $("#toDateTxt").val();
                    if (validateDateValue(newText, toDateValue) === 1) {
                        error.show();
                    } else if (validateDateValue(newText, toDateValue) === -1) {
                        $("#toDateTxt").val(newText);
                        error.hide();
                    } else {
                        error.hide();
                    }
                }
                if ($(this).is("#toDateTxt")) {
                    var fromDateValue = $("#fromDateTxt").val();
                    if (validateDateValue(fromDateValue, newText) === 1) {
                        error.show();
                    } else {
                        error.hide();
                    }
                }
            },
            monthNamesShort: ["Jan", "Feb", "Mar", "Apr", "Maj", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov", "Dec"],
            dayNamesMin: ["Sö", "Må", "Ti", "On", "To", "Fr", "Lö"],
            firstDay: 1,
            dateFormat: "yy-mm-dd",
            beforeShow: function () {
                setTimeout(function () {
                    $('.ui-datepicker').css('z-index', 9);
                }, 0);
                if ($(this).is("#toDateTxt")) {
                    var fromDateValue = $("#fromDateTxt").val();
                    var toDateValue = $("#toDateTxt").val();
                    $(this).datepicker('option', 'minDate', new Date(fromDateValue));
                    $("#toDateTxt").val(toDateValue);
                }
                if ($(this).is("#selectedDate")) {
                    $(this).datepicker('option', 'minDate', new Date());
                }
            }
        }).on("change", function () {
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

    $(document).ready(function () {
        $(".expand-info").click(function () {
            var currentRow = $($(this).parent().parent());
            var hiddenRow = currentRow.next();
            hiddenRow.slideToggle(100);
            if ($(this).hasClass("expand-icon-plus")) {
                $(this).removeClass("expand-icon-plus");
                $(this).addClass("expand-icon-minus");
            }
            else {
                $(this).removeClass("expand-icon-minus");
                $(this).addClass("expand-icon-plus");
            }
        });
        TransportTypeChangeEvent();
    });
    $(".showDialog").click(function (e) {
        getAnnoncements($(this));
        e.preventDefault();
    });
    $(".table-transport > tbody > tr > td > a").click(function (e) {
        submitForm($(this).html(), $(this).attr('data-value'));
        e.preventDefault();
    });

    $(".search-customerNr").click(function(e) {
        submitForm($(this).attr('data-customerNr'), $(this).attr('data-value'));
        e.preventDefault();
    });

    $(document).on("click", ".datepicker-container", function () {
        console.dir("parent clicked");
        $(this).find("input").focus();
    });
});

var validateDateValue = function (fromData, toDate) {
    var myFromDate = new Date(fromData);
    var myToDate = new Date(toDate);
    var timeOfSevenDay = (24 * 60 * 60 * 1000) * 7;
    if (myToDate.getTime() - myFromDate.getTime() > timeOfSevenDay) {
        return 1;
    }
    else if (myToDate.getTime() - myFromDate.getTime() < 0) {
        return -1;
    }
    return 0;
};

var setToDateByFromDate = function(fromDateValue, toDateValue) {
    var myFromDate = new Date(fromDateValue);
    var myToDate = new Date(toDateValue);
    if (myFromDate.getTime() > myToDate.getTime()) {
        return 
    }
}
function submitForm(search, category) {
    var searchVal = search;
    var catVal = category;
    $("input[name= 'SearchText']").val(searchVal);
    $("#search-transport-type > ul > li").each(function() {
        if ($(this).attr("data-value") === catVal) {
            $("input[name= 'Category']").val(catVal);
        }
    });
    $("#form-search-transport").submit();
}

function getAnnoncements(row) {
    var orderRowId = $(row).parents(".lm__collapse-row").prev().find("td > a").html();
    var annoncementTable = $(".tblAnnoncements tbody");
    var modal = $(".lm__information-modal__wrapper");

    $.ajax({
        type: "POST",
        url: "/api/search-transport/get-annoncements",
        contentType: "application/json; charset=utf-8",
        data: "{'orderRowId':'" + orderRowId + "'}",

        success: function (data) {
            annoncementTable.html("");
            annoncementTable.html(data);
            modal.removeClass("hidden");
        },
        error: function () {
            annoncementTable.html("");
            modal.removeClass("hidden");
        }
    });
}

var TransportTypeChangeEvent = function () {
    $("#search-transport-type > ul > li > a").click(function () {
        var dataValueType = $(this).parent().attr("data-value");
        var placeholderData = "Ange " + dataValueType.toLowerCase();
        $("input[name='Category']").val(dataValueType);
        $("input[name= 'SearchText']").attr('placeholder', placeholderData);
        var inputFromDate = $("input[name= 'FromDate']");
        var inputToDate = $("input[name= 'ToDate']");
        if (dataValueType === "Kundnummer" || dataValueType === "Lastbil") {
            inputFromDate.prop("disabled", false);
            inputToDate.prop("disabled", false);
            if (inputFromDate.parent().hasClass("disabled")) {
                inputFromDate.parent().removeClass("disabled");
            }
            if (inputToDate.parent().hasClass("disabled")) {
                inputToDate.parent().removeClass("disabled");
            }
        } else {
            inputFromDate.prop("disabled", true);
            inputToDate.prop("disabled", true);
            inputFromDate.val("");
            inputToDate.val("");
            inputFromDate.parent().addClass("disabled");
            inputToDate.parent().addClass("disabled");

        }
    });
}
