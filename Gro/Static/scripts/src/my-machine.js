$(function () {
    MachineDetailPage.init();
    MachineAddPage.Init();
    MachineBookingService.Init();
});

(function ($) {
    $.fn.imgLoad = function (callback) {
        return this.each(function () {
            if (callback) {
                if (this.complete || /*for IE 10-*/ $(this).height() > 0) {
                    callback.apply(this);
                }
                else {
                    $(this).on('load', function () {
                        callback.apply(this);
                    });
                }
            }
        });
    };
})(jQuery);

var MachineDetailPage = MachineDetailPage || (function () {
    var updated = false;

    var addUploadPopupEvent = function () {
        $('.avatar-icon').on('click', function (e) {
            e.preventDefault();
            $(this).parents('.lm__block-metadata').find('.list-action-item').toggle();
        });
    };

    var fetchTotalHoursForMachine = function () {
        var inno = $('#inno').val();
        if (inno) {
            var url = 'https://api.trackunit.com/public/GetUnit';
            $.ajax({
                url: url,
                type: "GET",
                cache: false,
                data: { token: '11df2ae9bba646ceb8be959f23445b23', format: 'json', referenceNumber: inno },
                dataType: 'jsonp',
                success: function (result) {
                    if (result && result.list && result.list.length > 0) {
                        var trackUnitInfo = result.list[0];
                        var totalHours = ((parseFloat(trackUnitInfo.preRun1) + parseFloat(trackUnitInfo.run1)) / 3600).toFixed(1);
                        $('#total_hours').show().find('.total-text').html(totalHours + ' t');
                    } else {
                        $('#generic_banner').show();
                    }
                }
            });
        } else {
            $('#generic_banner').show();
        }
    };

    var autoFitImages = function () {
        var images = $(".machine-item__thumbnail-wrapper img");

        images.each(function () {
            $(this).imgLoad(function () {
                var $this = $(this);
                var size = {
                    width: $(this).width(),
                    height: $(this).height()
                };
                if (size.height < size.width) {
                    //console.log(size.width / size.height + ', ' + $this.parent().width() / $this.parent().height());
                    if (size.width / size.height > $this.parent().width() / $this.parent().height()) {
                        $(this).removeClass('horizontal-img').addClass('vertical-img');
                    } else {
                        $(this).removeClass('vertical-img').addClass('horizontal-img');
                    }
                } else {
                    //console.log(size.height / size.width + '; ' + $this.parent().height() / $this.parent().width());
                    if (size.height / size.width > $this.parent().height() / $this.parent().width()) {
                        $(this).removeClass('vertical-img').addClass('horizontal-img');
                    } else {
                        $(this).removeClass('horizontal-img').addClass('vertical-img');
                    }
                }
            });
        });
    };

    var attachDeleteMachineEvent = function () {
        $('.remove-machine-button').click(function () {
            $('.delete-machine-alert').show();
        });
        $('#agree-remove-machine').click(function () {
            $('#form-remove-machine').submit();
        });
        $('#disagree-remove-machine').click(function () {
            $('.lm__modal-alert').hide();
        });

        var isFailed = $('#deleteFailed').val();
        if (isFailed === "True") {
            alert("Radera maskin misslyckades. Kontrollera igen!");
        }
    }

    var init = function () {
        addUploadPopupEvent();
        fetchTotalHoursForMachine();
        attachDeleteMachineEvent();
        autoFitImages();

        $(window).on('resize orientationchange', function () {
            if (!updated) {
                updated = true;
                setTimeout(function () {
                    autoFitImages();
                    updated = false;
                }, 100);
            }
        });

        $(window).trigger('resize');
        
        var machineUploadSettings = {
            fileSelector: "#machine-file",
            previewImgSelector: "#machine-picture",
            uploadBtnSelector: "#machine-uploadBtn",
            cancelUploadBtnSelector: "#machine-cancelUpload",
            linkSelector: "#machine-link",
            handleUrl: "/api/machine/machine-upload-avatar",
            isNeedToUpdateHeader: false,
            errorMessage: "Det uppstod ett fel när bilden skulle laddas upp. Var gör försök igen!",
            deleteLinkSelector: "#machine-deleteBtn",
            handleDeleteUrl: "/api/machine/machine-delete-avatar"
        };
        var userUploadImageModule = new UploadImageModule(machineUploadSettings);
        userUploadImageModule.initDataForRequest([{ 'key': 'machineId', 'value': $('#machineId').val() }]);
        userUploadImageModule.initUploadFile();
        userUploadImageModule.initDeleteFile();
    };

    return {
        init: init,
        autoFitImages: autoFitImages
    };

})();

var MachineAddPage = MachineAddPage || (function () {
    var brandDdContainerSelector = "#drop-machine-brand";
    var modelDdContainerSelector = "#drop-machine-model";

    function checkRegisterNumber() {
        var id = $('#registration').val().toUpperCase();
        if (!id || id.trim() === "") return;

        id = id.trim();
        showLoader();

        $.ajax({
            url: '/api/add-machine/check-register-number?registerNumber=' + id,
            type: "GET",
            dataType: "json",
            success: function (data) {
                hideLoader();
                if (data.id != null && data.id != '') {
                    //update serialnumber
                    $('#serial-number').val(data.serialNumber);

                    //update category
                    $('#drop-machine-category > .dropdown > li').each(function () {
                        if ($(this).attr('data-value') == data.category.Key) {
                            $('#drop-machine-category > .dropdown > li').removeClass('selected');
                            $(this).addClass('selected');
                            $(this).parents('.showcase').attr('data-value', data.category.Key);
                            $(this).parents('.showcase').find('>a').html(data.category.Name);
                            $(this).parents('.showcase').find('>input').val(data.category.Key);
                            return false;
                        }

                    });

                    //update brand and model
                    $('#drop-machine-brand > .dropdown > li').each(function () {
                        if ($(this).attr('data-value') == data.brand.Key) {
                            $('#drop-machine-brand > .dropdown > li').removeClass('selected');
                            $(this).addClass('selected');
                            $(this).parents('.showcase').attr('data-value', data.brand.Key);
                            $(this).parents('.showcase').find('>a').html(data.brand.Name);
                            $(this).parents('.showcase').find('>input').val(data.brand.Key);
                            //changeModelByBrand(data);
                            changeMachineModel(data.brand.Key, data.model.Name);
                            return false;
                        }
                    });
                }
                else {
                    $('.lm__modal-alert').show();
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                hideLoader();
                alert(errorThrown.toString());
            }
        });
    }

    function changeMachineModel(selectedBrandId, modelName) {
        var brandId = selectedBrandId == undefined ? $(brandDdContainerSelector).attr('data-value') : selectedBrandId;
        var apiUrl = '/api/add-machine/get-machine-models?brandId=' + brandId;
        if (modelName != undefined) {
            apiUrl += "&modelName=" + modelName;
        }
        $.ajax({
            url: apiUrl,
            type: "GET",
            dataType: "html",
            success: function (data) {
                if (data) {
                    $(".model-dd-container").html(data);
                }
                setDropdown($(modelDdContainerSelector).parent(), 'type-3');
            }
        });
    }

    function validateFormAddMachine() {
        var category = $('#drop-machine-category');
        var brand = $(brandDdContainerSelector);
        var model = $(modelDdContainerSelector);

        var brandDesc = brand.parent().parent().find('>input');
        var modelDesc = model.parent().parent().find('>input');

        var errorList = new Array();
        if (category.attr('data-value') == "" || category.attr('data-value') == undefined) {
            errorList.push("Typ");
            category.parent().parent().find('.error-item').show();
        }
        else {
            category.parent().parent().find('.error-item').hide();
        }
        if (brand.attr('data-value') == "" || brand.attr('data-value') == undefined) {
            errorList.push("Märke");
            brand.parent().parent().find('.error-item').show();
        }
        else if (brand.attr('data-value') == "Annan" && brandDesc.val() == "") {
            errorList.push("Märke");
            brand.parent().parent().find('.error-item').show();
        }
        else {
            brand.parent().parent().find('.error-item').hide();
        }
        if (model.attr('data-value') == "" || model.attr('data-value') == undefined) {
            errorList.push("Modell");
            model.parent().parent().find('.error-item').show();
        }
        else if (model.attr('data-value') == "Annan" && modelDesc.val() == "") {
            errorList.push("Modell");
            model.parent().parent().find('.error-item').show();
        }
        else {
            model.parent().parent().find('.error-item').hide();
        }
        var errorListBox = $('#machine-add-error-list');
        if (errorList.length > 0) {
            errorListBox.show();
            errorListBox.html('');
            errorListBox.append(
                $('<li></li>').addClass('errors-list__header').append('Du måste ange')
                );
            $.each(errorList, function (i, error) {
                errorListBox.append(
                    $('<li></li>').append(error)
                    );
            });
            return false;
        }
        else {
            errorListBox.hide();
            return true;
        }
    }

    function hideLoader() {
        $('.loader-wrapper').hide();
        $('.loader-wrapper').parent().removeClass('disabled');
    }

    function showLoader() {
        $('.loader-wrapper').show();
        $('.loader-wrapper').parent().addClass('disabled');
    }

    var addBrandHandler = function () {
        $("#drop-machine-brand .dropdown li a").each(function (idx, item) {
            var $item = $(item);
            $item.click(function () {
                if ($item.parent().data('value') === 'Annan') {
                    $item.parents('.branch-block').find('input[type="text"]').show();
                    $item.parents('ul.lm__form-dropdown.branch-dropdown').hide();

                    var $modelDdContainer = $(modelDdContainerSelector);
                    $modelDdContainer.parents('.model-block').find('input[type="text"]').show();
                    $modelDdContainer.attr('data-value', 'Annan');
                    $modelDdContainer.find('>input[type="hidden"]').val('Annan');
                    $modelDdContainer.parents('ul.lm__form-dropdown.model-dropdown').hide();
                }
                else {
                    var brandId = $item.attr('data-value');
                    changeMachineModel(brandId);
                }
            });
        });
    }

    var addCategoryHandler = function () {
        $("#drop-machine-category .dropdown li a").each(function (idx, item) {
            var $item = $(item);
            $item.click(function () {
                var selectedKey = $item.parents(".showcase").data('value');
                if (selectedKey === "Other") {
                    // incase choosing other category, show textbox to input value
                    $item.parents('.category-block').find('input[type="text"]').show();
                    $item.parents('ul.lm__form-dropdown').hide();
                    $item.parents('.showcase').find("input[name=machineCategory").val("");
                }
            });
        });
    }

    var searchByRegNoHandler = function () {
        $('#machine-registration-button').click(function () {
            checkRegisterNumber();
        });
    }

    var submitAddingFormHandler = function () {
        $('#form-adding-machine').submit(function () {
            return validateFormAddMachine();
        });
    }

    var init = function () {
        addBrandHandler();
        addCategoryHandler();
        searchByRegNoHandler();
        submitAddingFormHandler();
    };
    return {
        Init: init
    }
})();

var MachineBookingService = MachineBookingService || (function () {

    var dataMachines = null;

    function setDataMachines(data) {
        dataMachines = data;
    }

    function setOnclickMachineList(obj, data) {
        $(obj).find('p > a').click(function (e) {
            if (data != null && data.length > 0) {
                var machineId = $(this).parent().find('input[type="hidden"]').val();
                fillDetailMachine(data, machineId);
            }
            e.preventDefault();
        });
    }

    function getMachinesByCategory() {
        var key = $('#book-service-category').attr('data-value');
        $.ajax({
            url: '/api/book-service/get-machine-by-category?categoryKey=' + key,
            type: "GET",
            dataType: "json",
            success: function (data) {
                $('#book-service-loading').hide();
                $.each(data, function (i, machines) {
                    if (machines.length > 6) {
                        $('#book-machine-list-2').show();
                        $('#book-machine-list-1').hide();
                        var dropdown = $('#book-machine-list-2 > .address-sections > select');
                        dropdown.html('');
                        $.each(machines, function (j, machine) {
                            dropdown.append(
                                $('<option></option>').attr('value', machine.id).append(machine.modelName)
                            );
                        });
                        setDataMachines(machines);
                    }
                    else if (machines.length > 0 && machines.length <= 6) {
                        $('#book-machine-list-2').hide();
                        $('#book-machine-list-1').show();
                        var listSelectMachine = $('#book-machine-list-1 > .address-sections');
                        listSelectMachine.html('');
                        listSelectMachine.append($('<h3></h3>').addClass('lm__bold-title').append('Mina maskiner'));
                        $.each(machines, function (j, machine) {
                            listSelectMachine.append($('<p></p>').append(
                                $('<a></a>').addClass('lm__link').attr('href', '#').append(machine.modelName),
                                $('<input></input>').attr('type', 'hidden').val(machine.id)
                                ));
                        });
                        setOnclickMachineList(listSelectMachine, machines);
                    }
                    else {
                        $('#book-machine-list-2').hide();
                        $('#book-machine-list-1').hide();
                    }
                });
            },
        });
    }

    function getMachineById(listMachine, machineId) {
        var result = null;
        $.each(listMachine, function (i, machine) {
            if (machine.id.toString() == machineId.toString()) {
                result = machine;
            }
        });
        return result;
    }

    function fillDetailMachine(listMachine, machineId) {
        var machine = getMachineById(listMachine, machineId);
        var blockDetailMachine = $('#book-service-detail-machine').parent();
        if (machine != null) {
            blockDetailMachine.show();
            $('#book-service-detail-machine').html('');
            $('#book-service-detail-machine').append(
                $('<h2></h2>').addClass('heading-title-2').append('Vald maskin')
                );
            $('#book-service-detail-machine').append(
                $('<h3></h3>').addClass('lm__bold-title').append(machine.modelName)
                );

            $('#book-service-detail-machine').append(
                $('<p></p>').append(
                    $('<span></span>').addClass('lm__bold-title').append('Serienr'),
                    machine.serialNumber
                    )
                );
            $('#book-service-detail-machine').append(
                $('<p></p>').append(
                    $('<span></span>').addClass('lm__bold-title').append('Regnr'),
                    machine.registerNumber
                    )
                );
            blockDetailMachine.find("p > input[name=BrandAndModel]").val(machine.modelName);
            blockDetailMachine.find("p > input[name=SerialNumber]").val(machine.serialNumber);
            blockDetailMachine.find("p > input[name=RegisterNumber]").val(machine.registerNumber);
        }
        else {
            blockDetailMachine.hide();
            blockDetailMachine.find("p > input[name=BrandAndModel]").val('');
            blockDetailMachine.find("p > input[name=SerialNumber]").val('');
            blockDetailMachine.find("p > input[name=RegisterNumber]").val('');
        }
    }

    function bookingServiceSendFaild() {
        var result = $('#booking-service-send-faild').val();
        if (result != "" && result != undefined) {
            alert(result);
        }
    }

    var addMaskinTypeHandler = function () {
        // for choose Maskin Type in Book Service
        $(".maskin-type .showcase .dropdown li a").click(function () {
            $('#book-service-detail-machine').parent().hide();
            $('#book-machine-list-1').hide();
            $('#book-machine-list-2').hide();
            $('#book-service-loading').show();
            getMachinesByCategory();
        });
    }

    var addAddressHandler = function () {
        $("#book-machine-list-2 > .address-sections > select").change(function () {
            if (dataMachines != null && dataMachines.length > 0) {
                fillDetailMachine(dataMachines, $(this).val());
            }
        });
    }

    var addBookServiceFailedHandler = function () {
        //alert booking service send faild
        return bookingServiceSendFaild();
    }

    var init = function () {
        addMaskinTypeHandler();
        addAddressHandler();
        addBookServiceFailedHandler();
    }

    return {
        Init: init
    }
})();
