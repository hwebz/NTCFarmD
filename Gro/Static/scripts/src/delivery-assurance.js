jQuery(function () {
    DeliveryAssuranceModule.init();
});

var DeliveryAssuranceModule = DeliveryAssuranceModule || (function () {
    var reqDate, startDate, endDate, harvestPeriodStart, harvestPeriodEnd;
    var existDepaAvtal, choosenSpontanAvtal;
    var isInternal = 0;
    var enabledWarehouse = 0;

    var getCalculateObject = function () {
        return {
            supplier: $('#dialog_CustomerNumber').val(),
            lorryType: $('#dialog_LorryType').val(),
            deliveryAddressId: $('#dialog_DeliveryAddress').val(),
            quantity: $('#dialog_Quantity').val(),
            deliveryDate: $('#dialog_DeliveryDate').val(),
            itemId: $('#dialog_Article').val()
        };
    }

    var setSelectedForCustomSelectBox = function (targeSelector, sourceSelector) {
        var selectedValue = $(sourceSelector).val();
        $(targeSelector).prev().find('li').each(function (index, item) {
            var matchValue = $(item).attr('data-value');
            if (matchValue === selectedValue) {
                $(item).find('a').trigger('click');
            }
        });
    }

    var fillCalculateForm = function () {
        $('#dialog_Quantity').val($('#Quantity').val());
        $('#dialog_DeliveryDate').val($('#DeliveryDate').val());
        setSelectedForCustomSelectBox('#dialog_LorryType', '#LorryType');
        setSelectedForCustomSelectBox('#dialog_DeliveryAddress', '#DeliveryAddress');
        setSelectedForCustomSelectBox('#dialog_Article', '#Article');
    }

    var confirmDialog = function (yesCallback, noCallback) {
        $('#confirmDelete').removeClass('hidden');
        $('#btnYes').unbind().click(function () {
            $('#confirmDelete').addClass('hidden');
            yesCallback();
        });
        $('#btnNo').unbind().click(function () {
            $('#confirmDelete').addClass('hidden');
            noCallback();
        });
    }

    var showInformationDialog = function (title, message) {
        $('#informationDialog').find('#dialogTitle').html(title);
        $('#informationDialog').find('#dialogContent').html(message);
        $('#informationDialog').removeClass('hidden');
    }

    var showChangeNotAvailableDiglog = function () {
        $('#changeNotAvailable').removeClass('hidden');
    }

    var deleteDeliveryAssurance = function (self, totalSelector, ioNumber, lineNumber) {
        confirmDialog(function () {
            var loader = $(self).parents('.deliveryTableContainer').addClass('disabled').find('.loader-wrapper');
            loader.show();
            $.ajax({
                dataType: "json",
                type: 'post',
                url: '/api/delivery-assurance/delete',
                data: { ioNumber: ioNumber, lineNumber: lineNumber },
                cache: false,
                success: function (data) {
                    if (data.success) {
                        var total = parseInt($(totalSelector).val());
                        if (!isNaN(total) && total >= 1) {
                            total = total - 1;
                            $(totalSelector).val(total);
                        }
                        $(self).parents('tr').remove();
                    } else {
                        showInformationDialog('', data.message);
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    showInformationDialog('', errorThrown.toString());
                },
                complete: function (jqXHR) {
                    loader.hide().parents('.deliveryTableContainer').removeClass('disabled');
                }
            });
        }, function () {

        });
    }

    var printMultiLines = function (cbxClassName) {
        var ioNumbers = "";
        var lineNumbers = "";
        var rowSelected = 0;

        $('.' + cbxClassName).each(function () {
            if ($(this).is(':checked')) {
                rowSelected++;
                // Get the value. Value contains of string with this format "IOnumber;lineNumber".
                var cbxValue = $(this).val();

                if (cbxValue) {
                    var valSplit = cbxValue.split(';');
                    if (valSplit != null) {

                        ioNumbers += valSplit[0] + "|";
                        lineNumbers += valSplit[1] + "|";
                    }
                }
            }
        });

        if (rowSelected === 0) {
            alert("Du måste först välja rader");
            return;
        }
        // Remove the last pipe line |
        if (ioNumbers !== "")
            ioNumbers = ioNumbers.slice(0, -1);

        if (lineNumbers !== "")
            lineNumbers = lineNumbers.slice(0, -1);

        // Create link to pdfHandler.
        var hrefLink = "/api/pdfhandler/generatemultipdf?a={ioNumber}&l={lineNumber}&resync=true";

        hrefLink = hrefLink.replace("{ioNumber}", ioNumbers); //ex: ioNumbers = "4022316|4023510"
        hrefLink = hrefLink.replace("{lineNumber}", lineNumbers); // ex: lineNumber =  "3|5"

        // Invoke now pdfHandler to create the pdf file.
        window.open(hrefLink, '_blank');
    }

    var getWarehouseList = function (deliveryDateSelector, article) {
        $.ajax({
            dataType: "json",
            type: 'post',
            url: '/api/delivery-assurance/get-warehouse',
            data: { action: $('#Action').val(), customerNumber: $('#dialog_CustomerNumber').val(), itemName: article, deliveryDateStr: $(deliveryDateSelector).val() },
            cache: false,
            success: function (data) {
                if (data.warehouses) {
                    $('.warehouseDropdown').html('');
                    $.each(data.warehouses, function (index, item) {
                        $('.warehouseDropdown')
                            .append('<li data-value="' + item.Key + '"><a href="javascript:void(0)">' + item.Value + '</a></li>');

                    });

                    setDropdown($('.warehouseDropdown').parents('ul.lm__form-dropdown'), 'type-3');

                    $('#warehouseList').find(".showcase .dropdown li a").click(function () {
                        var inputElement = $(this).parents(".showcase").find("input.form-element");
                        //console.log(inputElement.val());
                        var inputSelector = inputElement.attr('id');
                        $("#deliveryAssuranceForm").validate().element('#' + inputSelector);
                    });
                }

                if (data.depaAvtals) {

                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                showInformationDialog('', errorThrown.toString());
            },
            complete: function (jqXHR) {
            }
        });
    }

    var validateDeliveryAssurance = function () {
        if ($().validate) $('#deliveryAssuranceForm').validate({
            ignore: [],
            errorElementClass: 'error',
            errorClass: 'error-item',
            errorElement: 'span',
            rules: {
                Quantity: {
                    required: true,
                    max: 39,
                    min: 1
                },
                Torkat: {
                    required: true
                },
                Red: {
                    required: true
                },
                Straforkortat: {
                    required: true
                },
                Slam: {
                    required: true
                },
                HarvestYear: {
                    required: true
                },
                DeliveryType: {
                    required: true
                },
                DeliveryAddress: {
                    required: true
                },
                DeliveryDate: {
                    required: true,
                    date:true
                },
                DepaAvtal: {
                    required: function (element) {
                        return $('input[name="DeliveryType"]:checked').val() === "Depa";
                    }
                },
                Article: {
                    required: true
                },
                Warehouse: {
                    required: function (element) {
                        return enabledWarehouse == "1" && !$('#warehouseList').hasClass('hidden') && !$('#Gardshamtning').is(':checked');
                    }
                },
                Gardshamtning: {
                    required: function (element) {
                        return $('input[name="TermAndCondition"]:checked').length === 0;
                    }
                },
                TermAndCondition: {
                    required: function (element) {
                        return $('input[name="Gardshamtning"]:checked').length === 0;
                    }
                }
            },
            messages: {
                Quantity: {
                    required: "Du måste ange Kvantitet (ton)",
                    max: "Vänligen ange ett värde mellan 1 och 39",
                    min: "Vänligen ange ett värde mellan 1 och 39"
                },
                Torkat: {
                    required: "Du måste ange Varmluftstorkat"
                },
                Red: {
                    required: "Du måste ange RED"
                },
                Straforkortat: {
                    required: "Du måste ange Stråförkortning"
                },
                Slam: {
                    required: "Du måste ange Slammat"
                },
                HarvestYear: {
                    required: "Du måste ange Skördeår"
                },
                DeliveryType: {
                    required: "Du måste ange Överleverans"
                },
                DeliveryAddress: {
                    required: "Du måste ange Leveransadress"
                },
                DeliveryDate: {
                    required: "Du måste ange Leveransdatum",
                    date: "Var vänlig ange ett giltigt datum"
                },
                DepaAvtal: {
                    required: "Inget depåavtal valt. Om vald leveranstyp är Depåavtal behöver ett depåavtal också väljas från listan."
                },
                Article: {
                    required: "Du måste ange Artikel"
                },
                Warehouse: {
                    required: "Du måste ange Mottagningsplats"
                },
                Gardshamtning: {
                    required: "Du måste ange Transportsätt"
                },
                TermAndCondition: {
                    required: ""
                }
            },
            highlight: function (element, errorClass, validClass) {
                $(element).addClass(this.settings.errorElementClass).removeClass(errorClass);
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).removeClass(this.settings.errorElementClass).removeClass(errorClass);
            },
            errorPlacement: function (error, element) {
                var typeElement = $(element).attr('type');
                if (typeElement === 'radio') {
                    element.parent('div.lm__radio').parent().before(error);
                } else {
                    element.after(error);
                }
            },
            submitHandler: function (form) {
                $('#loader').show();
                $('#loader').parent().addClass('disabled');
                if (existDepaAvtal === 1 && choosenSpontanAvtal === 1) {
                    alert("Det finns depåavtal på vald artikel. Ändra Överleverans till Depåavtal eller bekräfta ditt val av spontanleverans genom att klicka på knappen Nästa en gång till.");
                }

                form.submit();
            }
        });

        if ($().validate) $('#deliveryAssuranceFormStep2').validate({
            ignore: [],
            errorElementClass: 'error',
            errorClass: 'error-item',
            errorElement: 'span',
            rules: {
                noOfNew: {
                    required: function (element) {
                        return $('input[name="createNew"]:checked').val() === "1";
                    },
                    digits: true
                }
            },
            messages: {
                noOfNew: {
                    required: "Du måste ange en siffra",
                    digits: "Ange endast siffror"
                }
            },
            highlight: function (element, errorClass, validClass) {
                $(element).addClass(this.settings.errorElementClass).removeClass(errorClass);
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).removeClass(this.settings.errorElementClass).removeClass(errorClass);
            }
        });
    }

    var initEvent = function () {
        $('#Gardshamtning').change(function () {
            if ($(this).is(':checked')) {
                $('input[name="TermAndCondition"]:radio').prop('checked', false);
                $('#lorryType').removeClass('hidden');
                if (isInternal != "1") {
                    $('#warehouseList').addClass('hidden');
                }
                $("#deliveryAssuranceForm").validate().element("#Warehouse");
                $("#deliveryAssuranceForm").validate().element("#DeliveryDate");
            }
        });

        $('input[name="TermAndCondition"]:radio').change(function () {
            if ($(this).is(':checked')) {
                $('#Gardshamtning').prop('checked', false);
                $('#lorryType').addClass('hidden');
                if (isInternal != "1") {
                    $('#warehouseList').removeClass('hidden');
                }
                $("#deliveryAssuranceForm").validate().element('#Gardshamtning');
                $("#deliveryAssuranceForm").validate().element("#DeliveryDate");
                $("#deliveryAssuranceForm").validate().element("#Warehouse");
            }
        });

        $('input[name="DeliveryType"]:radio').change(function () {
            var value = $('input[name="DeliveryType"]:checked').val();
            if (value === "Depa") {
                $('#depaAvtal').removeClass('hidden');
                if (choosenSpontanAvtal !== 2) {
                    choosenSpontanAvtal = 0;
                }
            } else if (value === "Spon") {
                $('#depaAvtal').addClass('hidden');
                //Clear selected value for depaAvtal
                if (existDepaAvtal === 1 && choosenSpontanAvtal !== 2) {
                    choosenSpontanAvtal = 1;
                }
            }
        });

        $('input[name="createNew"]:checkbox').change(function () {
            $('#noOfNew').val('');
            if ($(this).is(':checked')) {
                $('#noOfNew').parent().show();
            } else {
                $('#noOfNew').parent().hide();
                $('#createPdf').prop('disabled', false);
            }
        });

        $('#approveTerm').change(function () {
            if ($(this).is(':checked')) {
                $('#btnSubmitDelivery').addClass('reverse-state').removeClass('disabled');
            } else {
                $('#btnSubmitDelivery').addClass('disabled').removeClass('reverse-state');
            }
        });

        $('.cbxApproveDelAss').change(function () {
            if ($('.cbxApproveDelAss').is(':checked')) {
                $('#btnApprove').addClass('reverse-state').removeClass('disabled');
            } else {
                $('#btnApprove').addClass('disabled').removeClass('reverse-state');
            }
        });

        $('.cbxToPrintDeliveried').change(function () {
            if ($('.cbxToPrintDeliveried').is(':checked')) {
                $('#btnToPrintDeliveried').removeClass('disabled');
            } else {
                $('#btnToPrintDeliveried').addClass('disabled');
            }
        });

        $('.cbxToPrintConfirmed').change(function () {
            if ($('.cbxToPrintConfirmed').is(':checked')) {
                $('#btnToPrintConfirmed').removeClass('disabled');
            } else {
                $('#btnToPrintConfirmed').addClass('disabled');
            }
        });

        $('#noOfNew').blur(function () {
            var noOfNew = parseInt($(this).val());
            if (!isNaN(noOfNew) && noOfNew > 0) {
                var cb = $('#createPdf');
                cb.prop('checked', false);
                cb.prop('disabled', true);
            }
        });

        $('#openCalculateFee').click(function () {
            fillCalculateForm();
            $('#calculateFeeDialog').removeClass('hidden');
        });

        $('#calculateFree').click(function () {
            var calculateObject = getCalculateObject();
            $('#calculateFeeDialog').find('.loader-wrapper').show();
            $.ajax({
                url: '/api/deliveryfee/caluculate',
                type: 'POST',
                data: {
                    supplier: calculateObject.supplier,
                    lorryType: calculateObject.lorryType,
                    deliveryAddressId: calculateObject.deliveryAddressId,
                    quantity: calculateObject.quantity,
                    deliveryDate: calculateObject.deliveryDate,
                    itemId: calculateObject.itemId
                },
                success: function (data) {
                    $('#calculateResult').html(data);
                },
                complete: function (jqXHR) {
                    $('#calculateFeeDialog').find('.loader-wrapper').hide();
                }
            });
        });

        $('#clearBtn').click(function() {
            //clear delivery address
            $('#a_deliveryAddress').html('-- Välj leveransadress --');
            $('#DeliveryAddress').val('');
            //clear date
            $('#DeliveryDate').val('');
            //clear article
            $('#a_article').html('-- Välj artikel --');
            $('#Article').val('');
            //clear quantity
            $("#Quantity").val('');
            //clear year
            $('#a_HarvestYear').html('');
            $('#HarvestYear').val('');
            //clear Mottagningsplats
            $('#a_Warehouse').html('-- Välj mottagningsplats --');
            $('#Warehouse').val('');
            $('.warehouseDropdown').html('');
            //clear Leveranssätt
            $('#a_LorryType').html('-- Välj transportsätt --');
            $('#LorryType').val('');

            //clear delivery type
            $('input[name="DeliveryType"]').prop('checked', false);
            //clear Varmluftstorkat
            $("input[name='Torkat']").prop('checked', false);
            //clear RED
            $("input[name='Red']").prop('checked', false);
            //clear Straforkortat
            $("input[name='Straforkortat']").prop('checked', false);
            //clear Slam
            $("input[name='Slam']").prop('checked', false);

            $("input[name='TermAndCondition']").prop('checked', false);
            $('#Gardshamtning').prop('checked', false);

            $("textarea[name='OtherInfo']").val("");

            $("#deliveryAssuranceForm").validate().resetForm();
            //$('#btnSubmitDelivery').addClass('disabled').removeClass('reverse-state');
            $('#depaAvtal').addClass('hidden');
            $('#lorryType').addClass('hidden');
            if (isInternal != "1") {
                $('#warehouseList').addClass('hidden');
            }
        });

        $('#deliveryAssuranceForm').find(".showcase .dropdown li a").click(function () {
            var inputElement = $(this).parents(".showcase").find("input.form-element");
            //console.log(inputElement.val());
            var inputSelector = inputElement.attr('id');
            $("#deliveryAssuranceForm").validate().element('#' + inputSelector);
        });
    }

    var initVar = function () {

        var preDaysDelivery = parseInt($('#preDaysDelivery').val());
        var postDaysDelivery = parseInt($('#postDaysDelivery').val());
        if (isNaN(preDaysDelivery) || isNaN(postDaysDelivery)) return;

        reqDate = GroCommon.dateFromISO($('#reqDate').val());
        startDate = GroCommon.dateFromISO($('#reqDate').val());
        startDate.setHours(0, 0, 0, 0);
        startDate.setDate(reqDate.getDate() + preDaysDelivery);

        endDate = GroCommon.dateFromISO($('#reqDate').val());
        endDate.setHours(23, 59, 59, 999);
        endDate.setDate(reqDate.getDate() + postDaysDelivery);

        harvestPeriodStart = GroCommon.dateFromISO($('#harvestPeriodStart').val());
        harvestPeriodEnd = GroCommon.dateFromISO($('#harvestPeriodEnd').val());

        isInternal = $('#IsInternal').val();
        enabledWarehouse = $('#EnabledWarehouse').val();

        var numberOfDepaAvtal = parseInt($('#numberOfDepaAvtal').val());
        if (!isNaN(preDaysDelivery)) {
            if (numberOfDepaAvtal > 1) {
                $('input[name="DeliveryType"]:radio').each(function (index, item) {
                    var value = $(item).val();
                    if (value === "Depa") {
                        $(item).prop('checked', true);
                    }
                });
                existDepaAvtal = 1;
            } else {
                $('input[name="DeliveryType"]:radio').each(function (index, item) {
                    var value = $(item).val();
                    if (value === "Depa") {
                        $(item).prop('checked', false);
                        $(item).prop('disabled', true);
                        //$(item).parent().addClass('disabled');
                    }
                    if (value === "Spon") {
                        $(item).prop('checked', true);
                    }
                });
                existDepaAvtal = 0;
            }
        }

        $('#deliveryAssuranceForm .dropdown li.selected, #calculateFeeDialog .dropdown li.selected').each(function (index, item) {
            var description = $(item).find('a').html();
            $(item).parent('ul').prev('a').html(description);
        });
    }

    var initDeliveryDate = function () {
        if ($.fn.datepicker) $(".delivery-datepicker").datepicker({
            defaultDate: new Date(),
            onSelect: function (newText) {
                // compare the new value to the previous one
                if ($(this).data('previous') !== newText) {
                    if ($("#deliveryAssuranceForm").validate().element("#DeliveryDate")) {
                        getWarehouseList(this, $("#Article").val());
                    };
                }
            },
            monthNames :["Januari", "Februari", "Mars", "April", "Maj", "Juni", "Juli", "Augusti", "September", "Oktober", "November", "December"],
            dayNamesMin: ["Sö", "Må", "Ti", "On", "To", "Fr", "Lö"],
            firstDay: 1,
        })
        .on('change', function () {
            var $this = $(this);
            var validDate = !/Invalid|NaN/.test(new Date($this.val()).toString());
            var validDateRegex = /^\d\d\d\d-(0?[1-9]|1[0-2])-(0?[1-9]|[12][0-9]|3[01])$/ig.test($this.val());

            $("#deliveryAssuranceForm").validate().element("#DeliveryDate");

            if (!validDateRegex || !validDate) {
                //$this.addClass("error").next().show();
                //$this.next().show();
            } else {
                //$this.removeClass("error").next().hide();
                //$this.next().hide();
                getWarehouseList(this, $("#Article").val());
            }

            });
    }

    var addValidation = function () {
        if (jQuery.validator) {
            jQuery.validator.addMethod("dateHigherThanToday", function (value, element) {
                //If false, the validation fails and the message below is displayed
                var currentDate = new Date();

                var valGardshamtning = $('input[id="Gardshamtning"]:checked').val();
                if (valGardshamtning === '1') {
                    if (!element)
                        return false;

                    var selectedDate = GroCommon.dateFromISO(element.value);  // new Date(element.value); funkar bara med IE9+

                    selectedDate.setHours(0, 0, 0, 0);

                    var currentHours = currentDate.getHours();
                    currentDate.setHours(0, 0, 0, 0);

                    if (selectedDate < currentDate)
                        return false;

                    if ((selectedDate.getFullYear() === currentDate.getFullYear()) &&
                        (selectedDate.getMonth() === currentDate.getMonth()) &&
                        (selectedDate.getDate() === currentDate.getDate()) && (currentHours > 9))
                        return false;

                }
                return true;
            }, "Leveransdatum kan inte vara dagens datum eller tidigare.");

            if (startDate && reqDate) {
                jQuery.validator.addMethod('dateHarvest206', function (value, element) {
                    if (this.optional(element)) {
                        return true;
                    }
                    var enteredDate = GroCommon.dateFromISO(element.value); //new Date(element.value);
                    var tomorrow = new Date().setDate(new Date().getDate() + 1);

                    return ((tomorrow <= enteredDate) && (enteredDate <= reqDate));

                }, $.validator.format("Vänligen ange ett datum mellan {0} och {1}.", startDate ? startDate.toShortDateString() : "", reqDate ? reqDate.toShortDateString() : ""));
            }

            if (harvestPeriodStart && harvestPeriodEnd) {
                jQuery.validator.addMethod('dateHarvestRange', function (value, element) {
                    if (this.optional(element)) {
                        return true;
                    }

                    var enteredDate = GroCommon.dateFromISO(element.value); //new Date(element.value);

                    return ((enteredDate >= harvestPeriodStart) && (enteredDate <= harvestPeriodEnd));

                }, $.validator.format("Vänligen ange ett datum inom skördeperioden, dvs {0}/{1} - {2}/{3}.", harvestPeriodStart.getDate().toString(), (harvestPeriodStart.getMonth() + 1).toString(), harvestPeriodEnd.getDate().toString(), (harvestPeriodEnd.getMonth() + 1).toString()));
            }

            if (startDate && endDate) {
                jQuery.validator.addMethod('daterange', function (value, element) {
                    if (this.optional(element)) {
                        return true;
                    }
                    var enteredDate = GroCommon.dateFromISO(element.value); //new Date(element.value);

                    return ((startDate <= enteredDate) && (enteredDate <= endDate));

                }, $.validator.format("Vänligen ange ett datum mellan {0} och {1}.", startDate ? startDate.toShortDateString() : '', endDate ? endDate.toShortDateString() : ''));
            }
        }
    }

    var init = function () {
        initDeliveryDate();
        initVar();
        initEvent();
        addValidation();
        validateDeliveryAssurance();
    }

    return {
        init: init,
        deleteDeliveryAssurance: deleteDeliveryAssurance,
        showChangeNotAvailableDiglog: showChangeNotAvailableDiglog,
        printMultiLines: printMultiLines,
        getWarehouseList: getWarehouseList
    };
})();