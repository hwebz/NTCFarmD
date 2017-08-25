jQuery(function () {
    PurchaseAgreement.init();
    PriceAlertPage.init();
    DryingAgreement.init();
    GrainPricePage.init();
    PoolAndDepaAgreement.init();
});

var PurchaseAgreement = PurchaseAgreement || (function () {
    var agreementElements = [];
    var disabledClass = "disabled";
    var formSelector = "#agreementForm";
    
    var addValidation = function () {
        if (jQuery.validator) {
            jQuery.validator.addMethod("exact_date", function (value, element) {
                var result = true;
                var dates = value.split('-');

                if (dates.length == 3) {
                    //var d = new Date(dates[0] + "-" + dates[1] + "-" + dates[2] + " 00:00:00");
                    var d = new Date(value);
                    if (d.getFullYear() == dates[0] && d.getMonth() == (parseInt(dates[1]) - 1) && d.getDate() == dates[2]) {
                        result = true;
                    } else {
                        result = false;
                    }
                }

                return this.optional(element) || result;

            }, "Var vänlig ange ett giltigt datum");

            jQuery.validator.addMethod("lowerPriceMustLessUpperPrice", function (value, element) {
                //If false, the validation fails and the message below is displayed
                var upperPrice = parseInt($('#UpperPrice').val());
                var lowerPrice = parseInt($('#LowerPrice').val());
                return this.optional(element) || (!isNaN(upperPrice) && !isNaN(lowerPrice) && upperPrice >= lowerPrice);

            }, "Undre pris måste vara lägre än Övre pris");

            jQuery.validator.addMethod("atLeastOnePriceMustBeInput", function (value, element) {
                //If false, the validation fails and the message below is displayed
                var upperPrice = parseInt($('#UpperPrice').val());
                var lowerPrice = parseInt($('#LowerPrice').val());
                if (isNaN(lowerPrice)) lowerPrice = 0;
                return this.optional(element) || (!isNaN(upperPrice) && !isNaN(lowerPrice) && (upperPrice !== 0 || lowerPrice !== 0));

            }, "Något pris kanske ska anges?");

            jQuery.validator.addMethod("optinalLowerPriceMustBeGreatMinValue", function (value, element) {
                //If false, the validation fails and the message below is displayed
                var minValue = parseInt($('#PriceLow').val());
                var lowerPrice = parseInt($('#LowerPrice').val());
                if ($('#AgreementType').val() === "PrissakringDepaavtal") {
                    return this.optional(element) || (!isNaN(minValue) && !isNaN(lowerPrice) && !(lowerPrice >= 0 && lowerPrice < minValue));
                }
                return this.optional(element) || (!isNaN(minValue) && !isNaN(lowerPrice) && !(lowerPrice > 0 && lowerPrice < minValue));

            }, "Undre pris understiger. Pris anges i kr/ton");

            jQuery.validator.addMethod("agreementDateMin", function (value, element) {
                //If false, the validation fails and the message below is displayed
                var minDate = new Date();
                var enteredDate = GroCommon.dateFromISO(element.value); //new Date(element.value);
                enteredDate.setHours(enteredDate.getHours() + 23);

                return this.optional(element) || (enteredDate >= minDate);

            }, "Du kan inte ange datum som är lägre än dagens datum");

            jQuery.validator.addMethod("agreementDateMax", function (value, element) {
                //If false, the validation fails and the message below is displayed
                var minDate = new Date();
                var maxDate = new Date();
                maxDate.setDate(new Date().getDate() + 60);
                var enteredDate = GroCommon.dateFromISO(element.value); //new Date(element.value);
                enteredDate.setHours(enteredDate.getHours() + 23);

                return this.optional(element) || (enteredDate <= maxDate);

            }, "Uppdraget får ej vara giltigt i mer än 60 dagar");
        }
    }

    var validatePriceHedge = function () {
        var lowPrice = GroCommon.parseInt($('#PriceLow').val());
        var minQuantity = GroCommon.parseInt($('#CommitQuantityMin').val());

        if ($().validate) $(formSelector).validate({
            ignore: [],
            errorElementClass: 'error',
            errorClass: 'error-item',
            errorElement: 'span',
            onkeyup: false,
            rules: {
                UpperPrice: {
                    required: true,
                    digits: true,
                    atLeastOnePriceMustBeInput: true,
                    min: lowPrice
                },
                LowerPrice: {
                    required: function (element) {
                        return $('#AgreementType').val() === "PrissakringDepaavtal";
                    },
                    digits: true,
                    lowerPriceMustLessUpperPrice: true,
                    optinalLowerPriceMustBeGreatMinValue: true
                },
                CommitQuantity: {
                    required: true,
                    digits: true,
                    min: minQuantity
                },
                DeliveryMode: {
                    required: true
                },
                PriceWatchEndDate: {
                    date: true,
                    exact_date: true,
                    agreementDateMin: true,
                    agreementDateMax: true,
                    //required:true
                },
                TargetAction: {
                    required: true
                }
            },
            messages: {
                UpperPrice: {
                    required: "Du måste ange en siffra",
                    digits: "Ange endast siffror",
                    atLeastOnePriceMustBeInput: "Något pris kanske ska anges?",
                    min: "Övre pris understiger " + lowPrice + " kr. Pris anges i kr/ton"
                },
                LowerPrice: {
                    required: "Du måste ange en siffra",
                    digits: "Ange endast siffror",
                    lowerPriceMustLessUpperPrice: "Undre pris måste vara lägre än Övre pris",
                    optinalLowerPriceMustBeGreatMinValue: "Undre pris understiger " + lowPrice + " kr. Pris anges i kr/ton."
                },
                CommitQuantity: {
                    required: "Kvantitet ton måste anges",
                    digits: "Kvantitet ton anges numeriskt",
                    min: "Du kan inte ange kvantitet som är mindre än " + minQuantity
                },
                DeliveryMode: {
                    required: "Leveranssätt måste väljas"
                },
                PriceWatchEndDate: {
                    date: "Var vänlig ange ett giltigt datum"
                },
                TargetAction: {
                    required: "Du måste ange Resultat av prisbevakning"
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
                if ($('#approveAgreementTerm').is(':checked')) {
                    form.submit();
                }
            }
        });
    }

    var toggleDisableNextButton = function () {
        var commitQuantity = parseInt($('#CommitQuantity').val());
        var upperPrice = parseInt($('#UpperPrice').val());
        var lowerPrice = parseInt($('#LowerPrice').val());

        var isValid = $('#AgreementAvtal').val() != '' &&
            ($('input[name="DeliveryMode"]:checked').length > 0 || $('input[name="DeliveryMode"]:radio').length === 0) &&
            ($('input[name="TargetAction"]:checked').length > 0 || $('input[name="TargetAction"]:radio').length === 0) &&
            $('#AgreementPeriod').val() != '' &&
            $('#PriceWatchEndDate').val() != '' &&
            ((!isNaN(commitQuantity) && commitQuantity >= 12) || $('#CommitQuantity').length === 0) &&
            !isNaN(upperPrice) && upperPrice >= 500 &&
            !isNaN(lowerPrice) && lowerPrice <= upperPrice && lowerPrice >= 500;

        if (isValid)
            $('#move-to-step2').addClass('reverse-state').removeClass('disabled');
        else
            $('#move-to-step2').addClass('disabled').removeClass('reverse-state');
    }

    var validateForDropdown = function (self) {
        var inputElement = $(self).parents(".showcase").find("input.form-element");
        var inputSelector = inputElement.attr('id');
        $(formSelector).validate().element('#' + inputSelector);
        toggleDisableNextButton();
    }
    
    var initPriceWatchEndDate = function () {
        if ($.fn.datepicker) $("#PriceWatchEndDate").datepicker({
            defaultDate: new Date(),
            onSelect: function (newText) {
                // compare the new value to the previous one
                if ($(this).data('previous') !== newText) {
                    $(formSelector).validate().element("#PriceWatchEndDate");
                    toggleDisableNextButton();
                }
            },
            monthNames: ["Januari", "Februari", "Mars", "April", "Maj", "Juni", "Juli", "Augusti", "September", "Oktober", "November", "December"],
            dayNamesMin: ["Sö", "Må", "Ti", "On", "To", "Fr", "Lö"],
            firstDay: 1,
            showOtherMonths: true,
            selectOtherMonths: true,
            dateFormat: 'yy-mm-dd',
            beforeShow: function () {
                setTimeout(function () {
                    $('.ui-datepicker').css('z-index', 5);
                }, 0);
            }
        })
        .on('change', function () {
            $(formSelector).validate().element("#PriceWatchEndDate");
            toggleDisableNextButton();
        });
    }

    var getProtectAgreement = function (agreementId, productItemId, priceAreaId, grainType, productItemName, callback) {
        if (!agreementId) {
            return;
        }
        $('#agreement-loader').show();
        $("#AgreementId").val(agreementId);
        $("#product-item-name").html(productItemName);
        $("#grain-type").html(grainType);

        var apiUrl = "/api/agreement/get-protect-agreement";
        $.ajax({
            dataType: "json",
            type: 'get',
            url: apiUrl,
            data: {
                productItemId: productItemId,
                priceAreaId: priceAreaId,
                grainType: grainType
            },
            cache: false,
            success: function (data) {
                $("#agreement-detail").show();
                if (data) {
                    $("#price-periods").html($.parseHTML(data.periodsView));
                    setDropdown($("#price-periods"), "type-2");
                    $("#price-periods .showcase a.period-item").each(function (idx, item) {
                        $(item).click(function () {
                            GroCommon.disableOrEnableElement('#agreementForm .agreement-element', $(item));
                            toggleDisableNextButton();
                        });
                    });
                    callback();
                }

            },
            error: function (jqXHR, textStatus, errorThrown) {
                
            },
            complete: function (jqXHR) {
                $('#agreement-loader').hide();
            }
        });
    }

    var getDisplayValueInDropdown = function ($dropdown, selectedValue) {
        var result;
        $dropdown.find('li').each(function (index, item) {
            var matchValue = $(item).attr('data-value');
            if (matchValue === selectedValue) {
                result = $(item).find('a').html();
                return false;
            }
        });

        return result;
    }

    var getHarvestYear = function (periodInfo) {
        var startIndex = periodInfo.indexOf(';');
        var datePeriod = periodInfo.substring(startIndex + 1, startIndex + 9);
        var year = parseInt(datePeriod.substring(0, 4));
        var date = parseInt(datePeriod.substring(4));
        if (isNaN(year) || isNaN(date)) return "";

        return date < 701 ? year - 1 : year;
    }

    var getGrainTypesForSpotAgreement = function (priceAreaId, productItemId) {
        $('#agreement-loader').show();
        $('#product-item-name').html(getDisplayValueInDropdown($('.productDropdown .dropdown'), productItemId));
        $.ajax({
            dataType: "json",
            type: 'get',
            url: '/api/agreement/get-grain-type-spot-agreement',
            data: { priceAreaId: priceAreaId, productItemId: productItemId },
            cache: false,
            success: function (data) {
                $('#agreementForm .grainTypeDropdown .dropdown').html('');
                $('#agreementForm .grainTypeDropdown > .showcase > a').html('Välj sort');
                $('#agreementForm #GrainType').val('');
                $('#grain-type').html('');
                if (data.length > 0) {
                    $.each(data, function (index, item) {
                        $('.grainTypeDropdown .dropdown').append('<li data-value="' + item.GrainName + '"><a href="javascript:void(0)">' + item.GrainName + '</a></li>');
                    });

                    setDropdown($('.grainTypeDropdown'), 'type-3');

                    $('#agreementForm .grainTypeDropdown').find(".showcase .dropdown li a").click(function () {
                        validateForDropdown(this);
                        GroCommon.disableOrEnableElement('#agreementForm .agreement-element', $(this));
                        $('#grain-type').html(getDisplayValueInDropdown($('.grainTypeDropdown .dropdown'), $(this).parent().attr('data-value')));
                        if ($('#AgreementPeriod').val() != '') {
                            $('.agreement-detail').removeClass('hidden');
                        }
                    });

                    $('#agreementForm .grainTypeDropdown').parent().parent().removeClass('disabled');
                }
                $('#agreementForm .periodDropdown').parent().parent().addClass('disabled');
                $('.agreement-detail').addClass('hidden');
                toggleDisableNextButton();
            },
            error: function (jqXHR, textStatus, errorThrown) {
            },
            complete: function (jqXHR) {
                $('#agreement-loader').hide();
            }
        });
    }

    
    var initEvent = function () {
        $('#UpperPrice').blur(function () {
            $(formSelector).validate().element("#LowerPrice");
            toggleDisableNextButton();
        });

        $('#approveAgreementTerm').change(function () {
            if ($(this).is(':checked')) {
                $('#agreementSaveBtn').addClass('reverse-state').removeClass('disabled');
            } else {
                $('#agreementSaveBtn').addClass('disabled').removeClass('reverse-state');
            }
        });

        $("#agreementForm .showcase .dropdown a.agreement-item").each(function (idx, item) {
            $(item).click(function() {
                var $item = $(item);
                var agreementId = $item.attr("data-agreementid");
                var productItemId = $item.attr("data-productItemId");
                var priceAreaId = $item.attr("data-priceAreaId");
                var grainType = $item.attr("data-grainType");
                var productItemName = $item.attr("data-productItemName");
                getProtectAgreement(agreementId, productItemId, priceAreaId, grainType, productItemName, function () {
                    GroCommon.disableOrEnableElement('#agreementForm .agreement-element', $(item));
                    toggleDisableNextButton();
                });
                showOverview();
            });
        });
        function showOverview() {
            $("#area-sort-overview").removeClass("hidden");
        }

        $("#agreementForm .agreement-element input").each(function (idx, item) {
            $(item).focusout(function () {
                GroCommon.disableOrEnableElement('#agreementForm .agreement-element', $(item));
                toggleDisableNextButton();
            });
        });

        $('#agreementForm input[name="TargetAction"]:radio').change(function () {
            GroCommon.disableOrEnableElement('#agreementForm .agreement-element', $(this));
            toggleDisableNextButton();
        });

        $('#agreementForm input[name="DeliveryMode"]:radio').change(function () {
            GroCommon.disableOrEnableElement('#agreementForm .agreement-element', $(this));
            toggleDisableNextButton();
        });

        $('#move-to-step2').click(function () {
            if ($().validate) {
                var isValid = $(formSelector).valid();
                if (isValid) {
                    $('#span_Avtal').html(getDisplayValueInDropdown($('.avtalDropdown .dropdown'), $('#AgreementAvtal').val()));
                    $('#span_Groda').html($('#product-item-name').html());
                    $('#span_Sort').html($('#grain-type').html());
                    $('#span_Leveransperiod').html(getDisplayValueInDropdown($('.periodDropdown .dropdown'), $('#AgreementPeriod').val()) + " " + getHarvestYear($('#AgreementPeriod').val()));
                    $('#span_UpperPrice').html($('#UpperPrice').val());
                    $('#span_UnderPrice').html($('#LowerPrice').val());

                    $('#span_Quantity').html($('#CommitQuantity').val());
                    $('#span_TransportType').html($('input[name="DeliveryMode"]:checked').prop('id'));
                    $('#span_Prissakra').html($('input[name="TargetAction"]:checked').prop('id'));
                    $('#span_AgreementDate').html($('#PriceWatchEndDate').val());

                    $('#pStep1').addClass('hidden');
                    $('#pStep2').removeClass('hidden');
                }
            }
        });

        $('#move-to-step1').click(function () {
            $('#pStep2').addClass('hidden');
            $('#pStep1').removeClass('hidden');
        });

        $(formSelector).find(".showcase .dropdown li a").click(function () {
            validateForDropdown(this);
            //disableOrEnableElement($(this));
            GroCommon.disableOrEnableElement('#agreementForm .agreement-element', $(this));
        });

        $('#agreementForm .periodDropdown .dropdown a').click(function () {
            $('.agreement-detail').removeClass('hidden');
            $('#lable_Period').html(getDisplayValueInDropdown($('.periodDropdown .dropdown'), $(this).parent().attr('data-value')));
        });

        $("a#agrement-info").click(function() {
            $("#agreement-introduce").removeClass("hidden");
        });
    }

    var init = function () {
        // init total-favourite-items
        $('#CommitQuantity, #UpperPrice, #LowerPrice').val('');
        addValidation();
        validatePriceHedge();
        initEvent();
        initPriceWatchEndDate();
        agreementElements = $('.agreement-element');
    }

    return {
        init: init,
        getProtectAgreement: getProtectAgreement,
        getGrainTypesForSpotAgreement: getGrainTypesForSpotAgreement
    };
})();

var PriceAlertPage = PriceAlertPage || (function () {

    var deletePriceAlert = function (priceWatch, id) {
        var parameters = "{'id':'" + id + "'}";
        $.ajax({
            type: 'POST',
            url: '/api/price-alert/DeletePriceWatch',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: parameters,
            success: function (response) {
                if (response.isRemoved) {
                    $(priceWatch).remove();
                }
                else {
                    alert("Error");
                }
            },
            error: function (response) { }
        });
    }

    var deletePriceAlertEvent = function () {
        $(".priceAlertDelete").on("click", function (e) {
            var confirmDelete = window.confirm("Är du säker på att du vill ta bort detta prisuppdraget ?");
            if (confirmDelete) {
                var id = $(this).attr("data-value");
                var itemPriceWatch = $(this).parents("tr");
                deletePriceAlert(itemPriceWatch, id);
            }
            e.preventDefault();
        });
    }

    var initEvent = function () {
        deletePriceAlertEvent();
    }

    var init = function () {
        initEvent();
    }

    return {
        init: init
    };
})();

var GrainPricePage = GrainPricePage || (function () {

    var getPricePeriodbyArea = function (id) {
        var tableResult = $("#price-period-result");
        var parameters = "{'priceAreaId':'" + id + "'}";
        $("#loader").show();
        $.ajax({
            type: 'POST',
            url: '/api/price-grain/GetPricePeriodbyArea',
            contentType: "application/json; charset=utf-8",
            data: parameters,

            success: function (response) {
                if (response.resultPricePeriods == undefined) {
                    tableResult.html(response);
                }
                else {
                    tableResult.empty();
                    alert("Error");
                }
                $("#loader").hide();
            },
            error: function () {
                tableResult.empty();
                alert("Error");
                $("#loader").hide();
            }
        });
    }

    var getPricePeriodbyAreaEvent = function () {
        var dropdownPriceAreas = $('#price-area-list .dropdown li a');
        dropdownPriceAreas.on("click", function (e) {
            var priceAreaId = $(this).parent().attr('data-value');
            getPricePeriodbyArea(priceAreaId);
            e.preventDefault();
        });
    }

    var initEvent = function () {
        getPricePeriodbyAreaEvent();
    }

    var init = function () {
        initEvent();
    }

    return {
        init: init
    };
})();

var DryingAgreement = DryingAgreement || (function () {

    var initEvent = function () {
        $('#approveDryingAgreementTerm').change(function () {
            if ($(this).is(':checked')) {
                $('#approveDryingAgreementTermInStep2').prop('checked', true);
                $('#dryingBtnMoveToStep2, #dryingBtnMoveToStep3').addClass('reverse-state').removeClass('disabled');
            } else {
                $('#approveDryingAgreementTermInStep2').prop('checked', false);
                $('#dryingBtnMoveToStep2, #dryingBtnMoveToStep3').addClass('disabled').removeClass('reverse-state');
            }
        });

        $('#dryingBtnMoveToStep2').click(function () {
            $('#dryingStep1').addClass('hidden');
            $('#dryingStep2').removeClass('hidden');
        });

        $('#dryingBtnBackToStep1').click(function () {
            $('#dryingStep2').addClass('hidden');
            $('#dryingStep1').removeClass('hidden');
        });

        if ($('#generateDryingPdf').length > 0) {
            $('#generateDryingPdf')[0].click();
        }
    }

    var init = function () {
        initEvent();
    }

    return {
        init: init
    };
})();

var PoolAndDepaAgreement = PoolAndDepaAgreement || (function () {

    var validateForDropdown = function (self) {
        var inputElement = $(self).parents(".showcase").find("input.form-element");
        var inputSelector = inputElement.attr('id');
        $("#newPurchasingAgreementForm").validate().element('#' + inputSelector);
        toggleDisableNextButton();
    }

    var enableDeliveryMode = function () {
        $('.div-DeliveryMode').removeClass('disabled');
    }

    var getDisplayValueInDropdown = function ($dropdown, selectedValue) {
        var result;
        $dropdown.find('li').each(function (index, item) {
            var matchValue = $(item).attr('data-value');
            if (matchValue === selectedValue) {
                result = $(item).find('a').html();
                return false;
            }
        });

        return result;
    }

    var getHarvestYear = function (periodInfo) {
        var startIndex = periodInfo.indexOf(';');
        var datePeriod = periodInfo.substring(startIndex + 1, startIndex + 9);
        var year = parseInt(datePeriod.substring(0, 4));
        var date = parseInt(datePeriod.substring(4));
        if (isNaN(year) || isNaN(date)) return "";

        return date < 701 ? year - 1 : year;
    }

    var validateForm = function () {
        if ($().validate) $('#newPurchasingAgreementForm').validate({
            ignore: [],
            errorElementClass: 'error',
            errorClass: 'error-item',
            errorElement: 'span',
            onkeyup: false,
            rules: {
                CommitQuantity: {
                    required: true,
                    digits: true,
                    min: 12
                },
                DeliveryMode: {
                    required: true
                },
                ProductItemId: {
                    required: true
                },
                GrainType: {
                    required: true
                },
                AgreementPeriod: {
                    required: true
                }
            },
            messages: {
                CommitQuantity: {
                    required: "Kvantitet ton måste anges",
                    digits: "Kvantitet ton anges numeriskt",
                    min: "Du kan inte ange kvantitet som är mindre än 12"
                },
                DeliveryMode: {
                    required: "Transportsätt måste väljas"
                },
                ProductItemId: {
                    required: "Gröda måste väljas"
                },
                GrainType: {
                    required: "Sort måste väljas"
                },
                AgreementPeriod: {
                    required: "Leveranssätt måste väljas"
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
                if ($('#approvePoolAgreementTerm').is(':checked')) {
                    form.submit();
                }
            }
            
        });
    }

    var toggleDisableNextButton = function () {
        var commitQuantity = parseInt($('#CommitQuantity').val());
        var isValid = $('#ProductItemId').val() != '' &&
            $('#GrainType').val() != '' &&
            $('input[name="DeliveryMode"]:checked').length > 0 &&
            $('#AgreementPeriod').val() != '' &&
            !isNaN(commitQuantity) && commitQuantity >= 12;
        if (isValid)
            $('#btnMoveToStep2').addClass('reverse-state').removeClass('disabled');
        else
            $('#btnMoveToStep2').addClass('disabled').removeClass('reverse-state');
    }

    var getGrainTypes = function (priceAreaId, productItemId) {
        $('#agreement-loader').show();
        $.ajax({
            dataType: "json",
            type: 'get',
            url: '/api/agreement/get-grain-type',
            data: { priceAreaId: priceAreaId, productItemId: productItemId },
            cache: false,
            success: function (data) {
                $('.grainTypeDropdown .dropdown').html('');
                $('.grainTypeDropdown > .showcase > a').html('Välj sort');
                $('#GrainType').val('');
                if (data.length > 0) {
                    $.each(data, function (index, item) {
                        $('.grainTypeDropdown .dropdown').append('<li data-value="' + item.GrainName + '"><a href="javascript:void(0)" onclick="PoolAndDepaAgreement.enableDeliveryMode()">' + item.GrainName + '</a></li>');
                    });

                    setDropdown($('.grainTypeDropdown'), 'type-3');

                    $('.grainTypeDropdown').find(".showcase .dropdown li a").click(function () {
                        validateForDropdown(this);
                    });

                    $('.grainTypeDropdown').parent().parent().removeClass('disabled');
                }
                toggleDisableNextButton();
            },
            error: function (jqXHR, textStatus, errorThrown) {
            },
            complete: function (jqXHR) {
                $('#agreement-loader').hide();
            }
        });
    }

    var initEvent = function () {
        $('#newPurchasingAgreementForm input[name="DeliveryMode"]:radio').change(function () {
            GroCommon.disableOrEnableElement('#newPurchasingAgreementForm .agreement-element', $(this));
            toggleDisableNextButton();
        });

        $('#newPurchasingAgreementForm .periodDropdown .dropdown a').click(function () {
            GroCommon.disableOrEnableElement('#newPurchasingAgreementForm .agreement-element', $(this));
        });

        $('#approvePoolAgreementTerm').change(function () {
            if ($(this).is(':checked')) {
                $('#agreementSaveBtn').addClass('reverse-state').removeClass('disabled');
            } else {
                $('#agreementSaveBtn').addClass('disabled').removeClass('reverse-state');
            }
        });

        $('#btnMoveToStep2').click(function () {
            if ($().validate) {
                var isValid = $("#newPurchasingAgreementForm").valid();
                if (isValid) {
                    $('#span_Groda').html(getDisplayValueInDropdown($('.productDropdown .dropdown'), $('#ProductItemId').val()));
                    $('#span_Sort').html(getDisplayValueInDropdown($('.grainTypeDropdown .dropdown'), $('#GrainType').val()));
                    $('#span_Period').html(getDisplayValueInDropdown($('.periodDropdown .dropdown'), $('#AgreementPeriod').val()) + " " + getHarvestYear($('#AgreementPeriod').val()));

                    $('#span_Quantity').html($('#CommitQuantity').val());
                    $('#span_TransportType').html($('input[name="DeliveryMode"]:checked').prop('id'));

                    $('#pStep1').addClass('hidden');
                    $('#pStep2').removeClass('hidden');
                }
            }
        });

        $('#btnBackToStep1').click(function () {
            $('#pStep2').addClass('hidden');
            $('#pStep1').removeClass('hidden');
        });

        $('#newPurchasingAgreementForm').find(".showcase .dropdown li a").click(function () {
            validateForDropdown(this);
        });

        $('#newPurchasingAgreementForm #CommitQuantity').blur(function () {
            toggleDisableNextButton();
        });
    }

    var init = function () {
        initEvent();
        validateForm();
    }

    return {
        init: init,
        getGrainTypes: getGrainTypes,
        enableDeliveryMode: enableDeliveryMode
    };
})();
