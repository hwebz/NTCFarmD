$(document).ready(function () {
    var myTimeBooking = new TimeBookingPage();
    var myListingBooking = new ListingBookingPage();
    myTimeBooking.init();
    myListingBooking.init();
    $(document).trigger("enhance.tablesaw");
});
var blockUI = function () {
    $("#loader").show();
}

var unBlockUI = function () {
    $("#loader").hide();
}
var fillContentToModal = function (dialogId, header, content, openDialog) {
    var dialogArea = $(dialogId);
    dialogArea.find(".lm__wide-modal__title").html(header);
    dialogArea.find("p").append(content);
    dialogArea.find(".btn-ok").on("click", function () {
        dialogArea.addClass("hidden");
    });
    if (openDialog === undefined) {
        dialogArea.removeClass("hidden");
    }
}
var clearContentToModal = function (dialogId) {
    var dialogArea = $(dialogId);
    dialogArea.find(".lm__wide-modal__title").html("");
    dialogArea.find("p").html("");
    dialogArea.find(".btn-ok").unbind("click");
}
var fillContentToDeleteDialog = function (header, content) {
    var dialogArea = $("#deleteDialog");
    dialogArea.find(".success-header-title").html(header);
    dialogArea.find("p").append(content);
    dialogArea.find(".btn-cancel").on("click", function () {
        dialogArea.hide();
    });
    dialogArea.find(".btn-ok").on("click", function () {
        dialogArea.hide();
    });
    dialogArea.show();
}
var clearContentToDeleteDialog = function () {
    var dialogArea = $("#deleteDialog");
    dialogArea.find(".lm__wide-modal__title").html("");
    dialogArea.find("p").html("");
    dialogArea.find(".btn-ok").unbind("click");
    dialogArea.find(".btn-cancel").unbind("click");
}

var TimeBookingPage = function () {
    var clearResultTable = function () {
        $("#resultTable").html('');
    }

    var extractForDropdown = function (elId, extractFrom) {
        var arr = new Array();
        for (var i = 0; i < extractFrom.length; i++) {

            arr[arr.length] = '<li data-value="' + extractFrom[i].Value + '"><a href="#">' + extractFrom[i].Display + '<\/a><\/li>';
            if (extractFrom[i].IsSelected == true) {
                $('#' + elId + '> a').html(extractFrom[i].Display);
                if (elId == 'resourceDrp') {
                    $('#' + elId).attr('regNoMandatory', extractFrom[i].RegNoMandatory);
                }
                $('#' + elId).attr('data-value', extractFrom[i].Value);
            }
        }
        return arr.join('');
    }
    //Search customer
    var searchCustomer = function () {
        clearResultTable();
        //setCustomerInfo(null);
        $('#agrementDriedUnDried').hide();
        var searchString = $('#seachFieldTxt').val();
        var resourceValue = $('#resourceDrp').attr('data-value');
        if (jQuery.trim(searchString) != '') {
            var searchType = $('#search-option').attr('data-value');
            var loadOrUnlodValue = $("input[name='loadOrUnlodRad']:checked").val();
            var customerSearcType = $("input[name='companySelected']:checked").val();
            blockUI();
            var parameters = "{'resourceGroupId':'" + resourceValue + "', 'searchString':'" + searchString + "', 'searchType':'" + searchType + "', 'loadOrUnlodValue':'" + loadOrUnlodValue + "', 'customerSearcType':'" + customerSearcType + "'}";
            $.ajax({
                type: 'POST',
                url: '/api/time-booking/CustomerSearch',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: parameters,

                success: function (response) {
                    unBlockUI();
                    searchCustomerSuccess(response.customerResult);
                    //showLinkToDeliveryAssurance(response.customerResult);
                },
                error: function (response) {
                    unBlockUI();
                    //searchCustomerError(response); 
                }
            });
        }

        var searchCustomerSuccess = function (response) {
            var dialogHtml;
            if (response.Status == 500) {
                clearContentToModal("#dialogArea");
                dialogHtml = response.ErrorMessage + '<br/><br/>';
                fillContentToModal("#dialogArea", "Fel uppstod", dialogHtml);
                return;
            }
            var customers = (typeof response.Customers) == 'string' ? eval('(' + response.Customers + ')') : response.Customers;
            setCustomerInfo(customers);
            if ($('#search-option').attr("data-value") === '9' && response.Ios.length > 0) {
                clearContentToModal("#dialogArea");
                dialogHtml = '<p>Inköpsordrar kopplade till kundnummer: ' + response.Customers[0].CustomerNo + '</p>';
                dialogHtml += '<p>Klicka på någon av nedanstående inköpsordrar för att gå direkt till den</p><br/>';
                dialogHtml += '<table style="width: 580px; display: block; overflow-y: scroll; height: 300px;">' +
                    '<thead><tr>' +
                    '<th>IONummer</th><th>Artikel</th><th>Kvantitet</th><th>Anläggning</th></tr></thead>' +
                    '<tbody>';
                $.each(response.Ios, function (i, d) {
                    dialogHtml += '<tr><td><a href="?ion=' +
                        d.IONumber +
                        '">' +
                        d.IONumber +
                        '</a></td><td>' +
                        d.ItemName +
                        '</td><td>' +
                        d.Quantity +
                        '</td><td>' +
                        d.WarehouseName +
                        '</td></tr>';
                });
                dialogHtml += '</tbody>' + '</table>';
                fillContentToModal("#dialogArea", "Välj inköpsorder nedan", dialogHtml);
            }

            if (response.BookingOrder !== null) {
                if (response.BookingOrder.Quantity != undefined && response.BookingOrder.Quantity !== null) {
                    $('#quantityTxt').val(response.BookingOrder.Quantity / 1000);

                }

                if (response.BookingOrder.DeliveryDate !== null) {
                    $('#selectedDate').val(new Date(parseInt(response.BookingOrder.DeliveryDate.substr(6))).toShortDateString());
                    if ($('#selectedDate').val().length < 10) {
                        $('#selectedDate').val(new Date().toShortDateString());
                    }
                }
                if ($('#search-option').attr("data-value") === '5') {
                    if (response.BookingOrder.Status > 40) {
                        unblockUI();
                    } else if (response.BookingOrder.Status == 40) {
                        unblockUI();
                    }
                    else if (response.BookingOrder.DeliveryAssuranceConfirmed == false) {
                        clearContentToModal("#dialogArea");
                        fillContentToModal("#dialogArea", "Redan bokad", 'Obs! Bokning ej tillåten på denna inköpsorder! Leveransförsäkran saknas');
                    }
                    getIOResource(response.BookingOrder.Warehouse);
                    searchItems('', $('#resourceDrp').attr('data-value'));
                }
                else if ($('#search-option').attr("data-value") === '7' || $('#search-option').attr("data-value") === '8' || $('#search-option').attr("data-value") === '6') {
                    getIOResource(response.BookingOrder.Warehouse);
                    searchItems('', $('#resourceDrp').attr('data-value'));
                }

                if (response.BookingOrder.ItemNumber !== null && response.BookingOrder.ItemNumber != '') {
                    var dried = 'torkad';
                    var $articlesDrp = $('#articlesDrp');
                    $("#agrementDriedUnDried").val(response.BookingOrder.Torkat ? 'Torkad' : 'Otorkad');
                    var driedUnDried = $("#agrementDriedUnDried").val();

                    if (!response.BookingOrder.Torkat) {
                        dried = 'otorkad';
                    }

                    var searchString = new String(response.BookingOrder.ItemNumber + '-' + dried + '-' + response.BookingOrder.Sort);

                    searchString = searchString.toLowerCase();
                    $('#articlesDrp > ul li').each(function () {
                        if ($(this).attr("data-value").toLowerCase().indexOf(searchString) != -1) {
                            $articlesDrp.attr("data-value", $(this).attr("data-value"));
                            $articlesDrp.find(">a").html($(this).find(">a").html());
                        }
                    });
                }

                $("div").data("Linenumber", response.BookingOrder.Linenumber);
            }
            if (response.RegNo !== null) {
                $('#IDReg').val(response.RegNo);
            }
            if (validateRequiredFieldsForSearch()) {
                $("#searchAvailbleSlotsBtn").parent().removeClass("disabled-btn");
                searchAvailableSlots();
                if ($('#search-option').attr('data-value') == 5) {
                    $('#IDLevnr_StartValue').val($('#seachFieldTxt').val());
                } else {
                    $('#IDLevnr_StartValue').val('');
                }
            }
            else {
                $('#searchAvailbleSlotsBtn').parent().addClass("disabled-btn");
                $('#IDLevnr_StartValue').val('');
            }
        };

        var getIOResource = function (warehouse) {
            var resourceListLoadSuccess = function (response, warehouseId) {
                var resource = $("#resourceDrp");
                clearDropDown('resourceDrp', 'Välj anläggning');
                clearDropDown('articlesDrp', 'Välj artikel');
                //clearDropDown('veichleTypeDrp', '- Välj fordon -');
                var resources = (typeof response) == 'string' ? eval('(' + response + ')') : response;
                $(resource).find('> ul > li').append(extractForDropdown('resourceDrp', resources));
                setDropdown($('#resourceDrp').parent(), 'type3');
                for (var i = 0; i < resources.length; i++) {
                    if ((warehouseId !== undefined && response[i].M3Id === warehouseId) ||
                        (warehouseId !== undefined && response[i].Value === warehouseId)) {
                        $(resource).attr("data-value", resources[i].Value);
                        $(resource).find(">a").html(resources[i].Display);
                        break;
                    }
                }
                validateRequiredFieldsEvent();
                searchItemEvent();
            };
            $.ajax({
                type: 'POST',
                url: '/api/time-booking/LoadResourceGroupsOnIO',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: "{'warehouseId':'" + warehouse + "'}",
                async: false,
                success: function (response) {
                    resourceListLoadSuccess(response.Resource, warehouse);
                },
                error: function (response) {
                    clearDropDown('resourceDrp', 'Välj anläggning');
                    clearDropDown('articlesDrp', 'Välj artikel');
                    clearDropDown('veichleTypeDrp', 'Välj fordon');
                    alert('Error');
                }
            });


        };

        var setCustomerInfo = function (customers) {
            if (customers != null) {

                var arr = new Array();
                var noOfCustomers = customers.length;


                if (noOfCustomers == 1) {
                    $('#customerNo').val(customers[0].CustomerNo);
                    $('#customerName').val(customers[0].Name);
                    $('#IDEpost_StartValue').val(customers[0].Email);
                    $('#IDMobil_StartValue').val(customers[0].MobileNo);

                    $('#IDEpost').val(customers[0].Email);
                    $('#IDMobil').val(customers[0].MobileNo);
                    displayWarningAboutDemoCustomer(customers[0].Name);

                } else {
                    clearContentToModal("#dialogArea");
                    fillContentToModal("#dialogArea", "Information saknas", 'Hittade ingen kund, avtal, transportorder eller sändningsorder med nr ' + $('#seachFieldTxt').val() + '.');
                }

            }
        };

        var displayWarningAboutDemoCustomer = function (customerName) {
            customerName = customerName.toLowerCase();
            if ((customerName.indexOf('testkund') >= 0) || (customerName.indexOf('demokund') >= 0)) {
                clearContentToModal("#dialogArea");
                fillContentToModal("#dialogArea", "Demokund", 'Obs! Du försöker boka på en test eller demokund vilket orsakar problem i vissa fall. Gör därför alltid bokningen i rätt kundnummer!');
            }
        }
    };

    //Update reservation
    var clearForm = function () {
        var reminder = $("#IDReminder");
        reminder.attr("data-value", "0");
        reminder.find(">a").html("Ingen påminnelse");

        $("#IDOvrigt").val('');
        $("#IDLevnr").val($("#IDLevnr_StartValue").val());
        $("#IDReg").val('');
        $('#IDEpost').val($('#IDEpost_StartValue').val());
        $('#IDMobil').val($('#IDMobil_StartValue').val());
        $('#saveAction').val('New');
        $('#updateBtn').hide();
        $('#saveBtn').show();

        $('#myDisplayMessage').html('');
        $('#dispalyMessageHeader').html('&nbsp;');


        //$('#newCustomerHeading').attr('disabled', 'disabled');
        $('#newCustomerNo').attr('disabled', 'disabled');
    }
    var resetForm = function () {
        $('#agrementDriedUnDried').val('Torkad');
        $('#agrementDriedUnDried').hide();

        //$('#newCustomerHeading').attr('disabled', 'disabled');
        $('#newCustomerNo').attr('disabled', 'disabled');

        $('#searchAvailbleSlotsBtn').parent().addClass("disabled-btn");
        $('#showReservationStopps').hide();

        /*  Clears form from editing values.*/
        $('.fieldToClearWithClearForm').val('');
        $('#IDEpost').val('');
        $('#IDEpost_StartValue').val('');

        $('#IDMobil').val('');
        $('#IDMobil_StartValue').val('');
        $('#saveAction').val('New');

        $('#updateBtn').hide();
        $('#saveBtn').show();


        $('#myDisplayMessage').html('');
        $('#dispalyMessageHeader').html('&nbsp;');



        $('#resourceDrp').attr('data-value', '');
        $('#resourceDrp > a').html('Välj anläggning');

        $('#articlesDrp').attr('data-value', '');
        $('#articlesDrp > a').html('Välj artikel');
        $('#loadOrUnlodRad').val('1');

        $('#veichleTypeDrp').attr('data-value', '');
        $('#veichleTypeDrp > a').html('Välj fordon');
        $('#quantityTxt').val('');
    }
    var copyValuesToEnableEdit = function (editObj) {

        $("div").data("dateRegistered", editObj.dateRegistered);
        $("#dispalyMessageHeader").html('Ändra bokning genom att ändra uppgifter nedan och klicka Uppdatera bokning.');
        $('#myDisplayMessage').html(editObj.displayMessage);

        $('#IDLevnr').val(editObj.idLevnr);
        $('#IDReg').val(editObj.idReg);
        $('#IDOvrigt').val(editObj.idOvrigt);
        var reminder = $("#IDReminder");
        if (editObj.idReminder === "0" || editObj.idReminder === "") {
            reminder.attr("data-value", "");
            reminder.find(">a").html("Ingen påminnelse");
        } else {
            reminder.attr("data-value", editObj.idReminder);
            var subReminder = reminder.find('ul > li[data-value="' + editObj.idReminder + '"]');
            var valueSubReminder = subReminder.find("a").html();
            subReminder.addClass("selected");
            reminder.find(">a").html(valueSubReminder);
        }
        $('#IDEpost').val(editObj.idEpost);
        $('#IDMobil').val(editObj.idMobil);
        $('#saveBtn').hide();
        $('#updateBtn').show();

        $('#saveAction').val('Update');
        $('#ReservationID').val(editObj.reservationId);

        $('#SpeditorNo_ForUpdate').val(editObj.speditorNo);

        $('#CustomerNo_ForUpdate').val(editObj.customerNo);
        $('#CustomerName_ForUpdate').val(editObj.customerName);

        $('#newCustomerNo').val(editObj.customerNo);
        $('#newCustomerName').val(editObj.customerName);

        $('#TransportOrderNo_ForUpdate').val(editObj.transportOrderNo);
        $('#ContractNo_ForUpdate').val(editObj.contractNo);
        $('#ReminderEmail_ForUpdate').val(editObj.reminderEmail);
        $('#ReminderSMS_ForUpdate').val(editObj.reminderSms);
        $('#Owner').val(editObj.owner);

        //$('#newCustomerHeading').removeAttr('disabled');
        $('#newCustomerNo').removeAttr('disabled');
        $('#newCustomerNo').val(editObj.customerNo);
        $('input:checkbox').attr('checked', false);
        $("#SaveTable").show();
    }
    var updateReservation = function () {
        var updateReservationSuccess = function (response) {
            var arr = new Array();

            arr[arr.length] = '<p>';
            for (var i = 0; i < response.length; i++) {

                if (response[i].Status == 200) {
                    arr[arr.length] = response[i].Message;
                } else {
                    arr[arr.length] = response[i].ErrorMessage;
                }
                arr[arr.length] = '<br \/>';
            }

            arr[arr.length] = '<\/p>';

            clearContentToModal("#dialogArea");
            clearForm();
            //unblockUI();
            fillContentToModal("#dialogArea", "Bokningen uppdaterad", arr.join(''));
            searchAvailableSlots();
        }

        var updateReservationError = function (response) {
            clearContentToModal("#dialogArea");
            fillContentToModal("#dialogArea", "Uppdateringsfel", "Fel uppstod när bokningen skulle uppdateras.");
            searchAvailableSlots();
        }

        var errors = validateFieldsForReservation(true);

        if (errors.length > 0) {
            clearContentToModal("#dialogArea");
            var arr = new Array();
            arr[arr.length] = '<p>';
            arr[arr.length] = '<ul>';
            arr[arr.length] = errors.join('');
            arr[arr.length] = '<\/ul>';
            arr[arr.length] = '<\/p>';
            fillContentToModal("#dialogArea", "Uppgifter saknas", arr.join(''));
        } else {

            var oldCustomerNo = $('#CustomerNo_ForUpdate').val();
            var oldCustomerName = $('#CustomerName_ForUpdate').val();

            var newCustomerNo = $('#newCustomerNo').val();
            var newCustomerName = $('#newCustomerName').val();
            var loadUnLoadVale = $('#loadOrUnlodRad').val();
            var mobileNo = $('#IDMobil').val();
            var leveransforsakransnummer = $('#IDLevnr').val();
            var note = $('#IDOvrigt').val();
            var reminderInMinutesBefore = $('#IDReminder').attr("data-value");
            var licensePlateNo = $('#IDReg').val();
            var email = $('#IDEpost').val();
            var owner = $('#Owner').val();
            var reservationId = $('#ReservationID').val();
            var speditor = $('#SpeditorNo_ForUpdate').val();

            if (validateSaveTable(email, mobileNo, licensePlateNo, leveransforsakransnummer)) {
                var innerDto = new Object();
                innerDto['Note'] = note;
                innerDto['ReminderInMinutesBefore'] = reminderInMinutesBefore;
                innerDto['EmailAddress'] = email;
                innerDto['NewCustomerName'] = newCustomerName;
                innerDto['NewCustomerNo'] = newCustomerNo;
                innerDto['OldCustomerName'] = oldCustomerName;
                innerDto['OldCustomerNo'] = oldCustomerNo;

                innerDto['Leveransforsakransnummer'] = leveransforsakransnummer;
                innerDto['LicensePlateNo'] = licensePlateNo;
                innerDto['MobilePhone'] = mobileNo;
                innerDto['Note'] = note;
                innerDto['ReminderInMinutesBefore'] = reminderInMinutesBefore;
                innerDto['EmailAddress'] = email;
                innerDto['SpeditorNo'] = speditor;

                innerDto['ReservationId'] = reservationId;
                innerDto['Owner'] = owner;
                innerDto['LineNumber'] = $("div").data("Linenumber");
                innerDto['DateRegistered'] = $("div").data("dateRegistered");

                var containerDto = new Object();
                containerDto.reservationToUpdate = innerDto;

                $.ajax({
                    type: 'POST',
                    url: '/api/time-booking/UpdateReservation',
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(containerDto),

                    success: function (response) { updateReservationSuccess(response.updateReservations); },
                    error: function (response) { updateReservationError(response.updateReservations); }
                });
            }
        }
    }

    //Delete reservation
    var deleteReservation = function (reservationId, time, owner, customerNo, dateRegistered) {
        var deletReservationSuccess = function (response) {
            var arr = new Array();
            arr[arr.length] = '<p>';
            for (var i = 0; i < response.length; i++) {
                if (response[i].Status == 500) {

                    arr[arr.length] = '<span class="ui-icon ui-icon-alert" style="float:left; margin:0 7px 20px 0;"></span>';
                }
                arr[arr.length] = response[i].Message + '<br \/>';
            }
            arr[arr.length] = '<\/p>';

            clearContentToModal("#dialogArea");
            fillContentToModal("#dialogArea", "Borttagning", arr.join(''));
            $("#dialogArea").find(".btn-ok").on("click", function () {
                searchAvailableSlots();
            });
        }

        var deletReservationError = function (response) {
            clearContentToModal("#dialogArea");
            fillContentToModal("#dialogArea", "Raderingsfel", "Fel uppstod när bokningen skulle raderas.");
            searchAvailableSlots();
        }

        var deletReservationProcess = function (reservationId, owner, customerNo, dateRegistered) {
            clearForm();
            var o = new Object();
            o.reservationId = reservationId;
            o.owner = owner;
            o.customerNo = customerNo;
            o.dateRegistered = dateRegistered;
            $.ajax({
                type: 'POST',
                url: '/api/time-booking/DeleteReservation',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(o),

                success: function (response) { deletReservationSuccess(response.deleteReservations); },
                error: function (response) { deletReservationError(response.deleteReservations); }
            });
        }

        var arr = new Array();
        arr[arr.length] = '<p>';
        arr[arr.length] = 'Vill du verkligen radera bokningen<br \/> kl: ' + time + '?';
        arr[arr.length] = '<\/p>';
        clearContentToDeleteDialog();
        fillContentToDeleteDialog("Ta bort bokning?", arr.join(''));
        $("#deleteDialog").find(".btn-ok").on("click", function () {
            deletReservationProcess(reservationId, owner, customerNo, dateRegistered);
        });
    }

    //Search items
    var searchItems = function (currentArticleItem, resource) {
        var searchItemComplete = function (response) {
            var vechicles = (typeof response.Vehicles) == 'string' ? eval('(' + response.Vehicles + ')') : response.Vehicles;
            var reservationStops = (typeof response.ReservationStops) == 'string' ? eval('(' + response.ReservationStops + ')') : response.ReservationStops;
            var articles = (typeof response.Items) == 'string' ? eval('(' + response.Items + ')') : response.Items;
            clearDropDown('articlesDrp', 'Välj artikel');
            $('#articlesDrp > ul').append(extractForDropdown('articlesDrp', articles));
            $('#veichleTypeDrp > ul').append(extractForDropdown('veichleTypeDrp', vechicles));
            setDropdown($('#articlesDrp').parent(), 'type3');
            setDropdown($('#veichleTypeDrp').parent(), 'type3');
            if (vechicles.length > 0) {
                $("#veichleTypeDrp > a").html(vechicles[0].Display);
                $("#veichleTypeDrp").attr("data-value", vechicles[0].Value);
            }
            validateRequiredFieldsEvent();
            buildReservationStops(reservationStops);
        }

        var buildReservationStops = function (reservationStops) {
            var i = 0;
            var arr = new Array();
            for (i = 0; i < reservationStops.length; i++) {
                arr[arr.length] = '<li>' + reservationStops[i].ResourceName + '<\/li>';
                arr[arr.length] = '<li>' + reservationStops[i].FromDate + ' ' + reservationStops[i].FromTime + ' - ';
                if (reservationStops[i].ToDate.length > 0) {
                    arr[arr.length] = reservationStops[i].ToDate + ' ' + reservationStops[i].ToTime + '<\/li>';
                } else {
                    arr[arr.length] = ' tillsvidare<\/li>';
                }
                arr[arr.length] = '<li style="font-style: italic">' + reservationStops[i].Message + '<br \/><\/li>';
            }
            $("#showReservationStopps").find("> ul").html(arr.join(''));
            if (i > 0) {
                $('#showReservationStopps').show();
            } else {
                $('#showReservationStopps').hide();
            }
        }

        var resourceValue = resource;
        var selectedDateValue = $('#selectedDate').val();

        if (!currentArticleItem) {
            currentArticleItem = $('#articlesDrp').attr("data-value");
        }

        var showOnlyUnloadingItems = ($("input:radio[name='loadOrUnlodRad']:checked").val() == '2');
        clearDropDown('articlesDrp', 'Välj artikel');
        clearDropDown('veichleTypeDrp', 'Välj fordon');
        $('#showReservationStopps').hide();
        if (resourceValue != '0' && resourceValue != '' && selectedDateValue != '') {
            blockUI();
            var parameters = "{'resourceGroupId':'" + resourceValue + "', 'selectedDate':'" + selectedDateValue + "', 'currentArticleItem':'" + currentArticleItem + "', 'showOnlyUnloadingItems':'" + showOnlyUnloadingItems + "'}";
            $.ajax({
                type: 'POST',
                url: '/api/time-booking/loadItemsOnresourceGroup',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: parameters,
                async: false,
                success: function (response) {
                    searchItemComplete(response);
                    unBlockUI();
                },
                error: function () {
                    unBlockUI();
                    alert('Error');
                }
            });
        }
    };
    var clearDropDown = function (elId, defaultText) {

        $('#' + elId + '> ul > li').remove();
        if ($.trim(defaultText) != '') {
            $('#' + elId).attr('data-value', '');
            $('#' + elId + '> a').html(defaultText);
        }

    };
    var validateRequiredFieldsForSearch = function () {
        var aRequiredFieldHasNoValue = false;
        $('.searchSlotRequired').each(function () {
            if ($(this).is('input')) {
                if ($.trim($(this).val()).length == 0) {
                    aRequiredFieldHasNoValue = true;
                    if ($(this).is('#quantityTxt')) {
                        $("#Quantity-error").hide();
                        $(this).removeClass("error");
                    }
                }
                else if ($(this).is('#quantityTxt')) {
                    if (!$.isNumeric($.trim($(this).val()))) {
                        aRequiredFieldHasNoValue = true;
                        $("#Quantity-error").show();
                        $(this).addClass("error");
                    } else {
                        $("#Quantity-error").hide();
                        $(this).removeClass("error");
                    }
                }
            }
            if ($(this).is('li')) {
                if ($(this).attr('data-value') == 0 || $(this).attr('data-value') == '') {
                    aRequiredFieldHasNoValue = true;
                }
            }
        });
        return !aRequiredFieldHasNoValue;
    }

    //Searchavailble slots
    var searchAvailableSlots = function () {
        var showHideTimebookingTable = function (isShow) {
            if (isShow) {
                $("#resultTable").show();
                $("#resultTable-box").addClass("has-separator");
            } else {
                $("#resultTable").hide();
                $("#resultTable-box").removeClass("has-separator");
            }
            $("#SaveTable").hide();
            $("#saveBtn").hide();
        }
        var searchAvailableSlotsSuccess = function (response) {
            var hookupEventsForTimeCheckboxes = function () {
                $(".bookingRows").change(function () {
                    var anyChecked = false;
                    $(".bookingRows").each(function () {
                        if ($(this).prop("checked")) {
                            anyChecked = true;
                        }

                    });
                    if (anyChecked) {
                        $("#SaveTable").show();
                        $("#saveBtn").removeClass("disabled-btn");
                        clearForm();
                        clearValidateSaveTable();

                    } else {
                        $("#SaveTable").hide();
                        $("#saveBtn").hide();
                        $("#saveBtn").addClass("disabled-btn");
                    }
                });
            }

            clearResultTable();
            $("#resultTable").append(response);
            if (response !== "") {
                $(".tb-deleteBtn").unbind("click").bind("click", function (e) {
                    var row = $(this).parents(".tb-row");
                    var reservationId = row.find(".tb-reservationId").val();
                    var fromDateTime = row.find(".tb-fromDateTime").val();
                    var owner = row.find(".tb-owner").val();
                    var customerNo = row.find(".tb-customerNo").val();
                    var dateRegistered = row.find(".tb-dateRegistered").val();
                    deleteReservation(reservationId, fromDateTime, owner, customerNo, dateRegistered);
                    e.preventDefault();
                });

                $(".tb-editBtn").unbind("click").bind("click", function (e) {
                    var row = $(this).parents(".tb-row");
                    var editObj = new Object();
                    editObj.idLevnr = row.find(".tb-idLevnr").val();
                    editObj.idReg = row.find(".tb-idReg").val();
                    editObj.idOvrigt = row.find(".tb-idOvrigt").val();
                    editObj.idReminder = row.find(".tb-idReminder").val();
                    editObj.idEpost = row.find(".tb-idEpost").val();
                    editObj.idMobil = row.find(".tb-idMobil").val();
                    editObj.reservationId = row.find(".tb-reservationId").val();
                    editObj.speditorNo = row.find(".tb-speditorNo").val();
                    editObj.customerNo = row.find(".tb-customerNo").val();
                    editObj.customerName = row.find(".tb-customerName").val();
                    editObj.transportOrderNo = row.find(".tb-transportOrderNo").val();
                    editObj.contractNo = row.find(".tb-contractNo").val();
                    editObj.reminderEmail = row.find(".tb-reminderEmail").val();
                    editObj.reminderSms = row.find(".tb-reminderSms").val();
                    editObj.fromDateTime = row.find(".tb-fromDateTime").val();
                    editObj.itemInfo = row.find(".tb-item").html();
                    editObj.quantity = row.find(".tb-quantity").html();
                    editObj.unit = row.find(".tb-unit").html();
                    editObj.owner = row.find(".tb-owner").val();
                    editObj.resourceName = row.find(".tb-resourceName").val();
                    editObj.dateRegistered = row.find(".tb-dateRegistered").val();
                    editObj.displayMessage = "Tid: " + editObj.fromDateTime + ", Kund: " + editObj.customerNo + ' ' + editObj.customerName + ', Bokningsnr: ' + editObj.reservationId +
                        ', Artikel: ' + editObj.itemInfo + ' ' + editObj.quantity + ' ' + editObj.unit + '<br \/>Resurs: ' + editObj.resourceName;
                    copyValuesToEnableEdit(editObj);
                    e.preventDefault();
                });
                $(document).trigger("enhance.tablesaw");
                showHideTimebookingTable(true);
            }
            else {
                $(".tb-deleteBtn").unbind("click");
                $(".tb-editBtn").unbind("click");
                showHideTimebookingTable(false);
            }
            hookupEventsForTimeCheckboxes();
        }
        clearResultTable();
        if (validateRequiredFieldsForSearch()) {
            blockUI();
            var resourceValue = $('#resourceDrp').attr('data-value');
            var selectedDateValue = $('#selectedDate').val();
            var agrementDriedUnDriedValue = $('#agrementDriedUnDried').val();

            var articlesValue = $('#articlesDrp').attr('data-value');
            var quantityTxtValue = $('#quantityTxt').val();
            var loadOrUnlodValue = $("input[name='loadOrUnlodRad']:checked").val();
            var veichleTypeValue = $('#veichleTypeDrp').attr('data-value');

            var searchTypeValue = $('#search-option').attr('data-value');
            var customerNo = $('#customerNo').val();

            if (resourceValue != '' && selectedDateValue != '' && quantityTxtValue != '' && (articlesValue != '' || argementValue != '')) {
                var parameters = "{'resourceGroupId':'" + resourceValue + "', 'selectedDate':'" + selectedDateValue + "', 'article':'" + articlesValue + "', 'qty':'" + quantityTxtValue + "', 'loadunload':'" + loadOrUnlodValue + "', 'veichleType':'" + veichleTypeValue + "', 'driedUnDried':'" + agrementDriedUnDriedValue + "', 'customerNo':'" + customerNo + "','searchType':'" + searchTypeValue + "'}";
                $.ajax({
                    type: 'POST',
                    url: '/api/time-booking/SearchAvailbleSlots',
                    contentType: "application/json; charset=utf-8",
                    data: parameters,

                    success: function (response) {
                        searchAvailableSlotsSuccess(response);
                        unBlockUI();
                    },
                    error: function () {
                        unBlockUI();
                        showHideTimebookingTable(false);
                        alert("Error");
                    }
                });
            }
        }
    };
    var validateFieldsForReservation = function (isUpdate) {
        var arr = new Array();

        if (jQuery.trim($('#quantityTxt').val()) == '') {
            arr[arr.length] = '<li>Kvantitet saknar värde.<\/li>';
        }

        var quantityValue = parseInt(jQuery.trim($('#quantityTxt').val()), 10);
        if (isNaN(quantityValue) || quantityValue <= 0) {
            arr[arr.length] = '<li>Kvantitet måste ha ett värde större än 0.<\/li>';
        }


        if (jQuery.trim($('#customerNo').val()) == '') {
            arr[arr.length] = '<li>Ingen gilltigt kund har valts.<\/li>';
        }

        /* Kontrollera om resursgruppen kräver regnr.*/
        if ($('#IDReg').hasClass('updateRequiered')) {

            if (jQuery.trim($('#IDReg').val()) == '') {
                arr[arr.length] = '<li>Registreringsnummer saknas.<\/li>';
            }
        }

        if (jQuery.trim($('#veichleTypeDrp').attr('data-value')) == '') {
            arr[arr.length] = '<li>Fordonstyp saknas.<\/li>';
        }


        if (jQuery.trim($('#selectedDate').val()) == '') {
            arr[arr.length] = '<li>Datum saknas.<\/li>';
        }


        if (jQuery.trim($('#articlesDrp').attr('data-value')) == '') {
            arr[arr.length] = '<li>Artikel saknas.<\/li>';
        }


        if (jQuery.trim($('#resourceDrp').attr('data-value')) == '') {
            arr[arr.length] = '<li>Resurs saknas.<\/li>';
        }

        if (!isUpdate) {
            var resourceValue = $('#resourceDrp').attr('data-value');
            var selectedDateValue = $('#selectedDate').val();
            var agrementDriedUnDriedValue = $('#agrementDriedUnDried').val();
            var articlesValue = $('#articlesDrp').attr('data-value');
            var quantityTxtValue = $('#quantityTxt').val();
            var loadOrUnlodValue = $("input[name='loadOrUnlodRad']:checked").val();
            var veichleTypeValue = $('#veichleTypeDrp').attr('data-value');
            var searchTypeValue = $('#search-option').attr('data-value');
            var customerNo = $('#customerNo').val();
            var ioNumber = $('#seachFieldTxt').val();

            if (resourceValue != '' && selectedDateValue != '' && quantityTxtValue != '' && (articlesValue != '' || argementValue != '')) {

                //blockUI();
                var parameters = "{'resourceGroupId':'" + resourceValue + "', 'selectedDate':'" + selectedDateValue + "', 'article':'" + articlesValue + "', 'qty':'" + quantityTxtValue + "', 'loadunload':'" + loadOrUnlodValue + "', 'veichleType':'" + veichleTypeValue + "', 'driedUnDried':'" + agrementDriedUnDriedValue + "', 'customerNo':'" + customerNo + "','searchType':'" + searchTypeValue + "', 'iONumber':'" + ioNumber + "'}";
                $.ajax({
                    type: 'POST',
                    url: '/api/time-booking/ExistBooking',
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    data: parameters,
                    async: false,
                    success: function (response) {
                        //unblockUI();
                        if (response.IsExistBooking == true) {
                            arr[arr.length] = "<li>Det finns redan en bokning på inköpsordern. Radera befintlig bokning innan du kan skapa en ny!<\/li>";
                        }
                    },
                    error: function () {
                        //unblockUI();
                        alert('ExistBooking');
                    }
                });
            }
        }
        return arr;
    }

    //Make reservation
    var makeReservation = function () {
        
        var makereservationContinue = function () {
            var reservations = new Array();
            var i = 0;
            var qty = $("#quantityTxt").val();
            var customerNo = $("#customerNo").val();
            var customerName = $("#customerName").val();


            var performUnload = ($("input:radio[name='loadOrUnlodRad']:checked").val() === "2");

            var mobileNo = $('#IDMobil').val();
            var leveransforsakransnummer = $('#IDLevnr').val();
            var note = $('#IDOvrigt').val();
            var reminderInMinutesBefore = $('#IDReminder').attr("data-value");
            var licensePlateNo = $('#IDReg').val() != null ? $('#IDReg').val() : "";
            var dried = $('#agrementDriedUnDried').val();
            var email = $('#IDEpost').val();
            var vehicleAssortmentId = $('#veichleTypeDrp').attr('data-value');
            var searchType = $('#search-option').attr('data-value');
            var searchValue = $('#seachFieldTxt').val();
            var selectedDate = $('#selectedDate').val();
            var item = $('#articlesDrp').attr('data-value');
            var resourceId = $('#resourceDrp').attr('data-value');
            var agrement = '';
            if (validateSaveTable(email, mobileNo, licensePlateNo, leveransforsakransnummer)) {
                $('.bookingRows').each(function () {
                    if ($(this).is(':checked')) {
                        i++;

                        var o = new Object();

                        o['FromTime'] = $(this).attr('fromtime');

                        o['ResourceId'] = $(this).attr('resursid');
                        if (performUnload == true) {
                            o['Loading'] = 'false';
                            o['Unloading'] = 'true';
                        } else {
                            o['Loading'] = 'true';
                            o['Unloading'] = 'false';
                        }
                        reservations.push(o);

                    }

                });

                if (reservations.length > 0) {
                    var innerDTO = new Object();
                    innerDTO['Reservations'] = reservations;
                    innerDTO['Agrement'] = agrement;
                    innerDTO['SearchValue'] = searchValue;
                    innerDTO['Qty'] = qty;
                    innerDTO['VehicleAssortmentID'] = vehicleAssortmentId;
                    innerDTO['ResourceId'] = resourceId;
                    innerDTO['Note'] = note;

                    innerDTO['ReminderInMinutesBefore'] = reminderInMinutesBefore;
                    innerDTO['EmailAddress'] = email;

                    innerDTO['CustomerName'] = customerName;
                    innerDTO['CustomerNo'] = customerNo;
                    innerDTO['Dried'] = dried;
                    innerDTO['SearchType'] = searchType;
                    innerDTO['SelectedDate'] = selectedDate;
                    innerDTO['Item'] = item;
                    innerDTO['Leveransforsakransnummer'] = leveransforsakransnummer;
                    innerDTO['LicensePlateNo'] = licensePlateNo;
                    innerDTO['MobilePhone'] = mobileNo;

                    var Linenumber = $("div").data("Linenumber");
                    if (Linenumber == null) {
                        Linenumber = 0;
                    }

                    innerDTO['LineNumber'] = Linenumber;

                    var containerDTO = new Object();
                    containerDTO.reservationToMake = innerDTO;

                    $.ajax({
                        type: 'POST',
                        url: '/api/time-booking/MakeReservation',
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(containerDTO),

                        success: function (response) {
                            makeReservationSuccess(response.makeReservations);

                        },
                        error: function (response) { makeResrvationError(response); }
                    });
                } else {
                    clearContentToModal("#dialogArea");
                    var arr = new Array();
                    arr[arr.length] = '<p>Inga tider valda<\/p>';
                    fillContentToModal("#dialogArea", "Tider saknas", arr.join(''));
                }
            }

        }
        var makeReservationSuccess = function (response) {
            var arr = new Array();
            var msgTitle = '';
            var successCnt = 0;
            var errorCnt = 0;
            arr[arr.length] = '<h4>Bokningar:<\/h4>';
            arr[arr.length] = '<p>';
            for (var i = 0; i < response.length; i++) {
                if (response[i].Status == 200) {
                    if (i > 0) {
                        arr[arr.length] = '<br \/><br \/>';
                    }
                    arr[arr.length] = response[i].StartDate;
                    arr[arr.length] = ' kl: ';
                    arr[arr.length] = response[i].StartTime;
                    arr[arr.length] = '<br \/>';
                    arr[arr.length] = response[i].CustomerNo;
                    arr[arr.length] = ' ';
                    arr[arr.length] = response[i].CustomerName;
                    successCnt++;
                } else {
                    errorCnt++;

                    arr[arr.length] = response[i].ErrorMessage;
                }
            }
            arr[arr.length] = "<br \/>";
            arr[arr.length] = "<\/p>";
            if (errorCnt > 0 && successCnt === 0) {
                msgTitle = "Fel uppstod vid bokningen";
            } else if (errorCnt > 0 && successCnt > 0) {
                msgTitle = "Alla bokningar gick inte igenom";
            } else if (errorCnt == 0 && successCnt > 0) {
                msgTitle = "Bokningar sparade";
            }

            clearContentToModal("#dialogArea");
            fillContentToModal("#dialogArea", msgTitle, arr.join(""));
            $("#saveBtn").addClass("disabled-btn");
            clearForm();
            searchAvailableSlots();
        }

        var makeResrvationError = function (response) {

            //clearForm();
            clearContentToModal("#dialogArea");
            fillContentToModal("#dialogArea", "Bokningsfel", "Fel uppstod när boking skulle registreras.");
        }

        var errors = validateFieldsForReservation(false);
        if (errors.length > 0) {
            clearContentToModal("#dialogArea");
            var arr = new Array();
            arr[arr.length] = '<p>';
            arr[arr.length] = '<ul>';
            arr[arr.length] = errors.join('');
            arr[arr.length] = '<\/ul>';
            arr[arr.length] = '<\/p>';
            fillContentToModal("#dialogArea", "Uppgifter saknas", arr.join(''));
            return false;
        } else {
            makereservationContinue();
        }
    }

    var searchAvalibleSlotAction = function () {
        clearForm();
        $("#resultTable").hide();
        $("#resultTable-box").removeClass("has-separator");
        $("#SaveTable").hide();
        $("#saveBtn").hide();
        searchAvailableSlots();
    }
    //Event
    var validateButtonSearchEvent = function () {
        $("#seachFieldTxt").unbind('keyup').keyup(function () {
            if ($(this).val() !== "") {
                $("#searcFieldhBtn").removeClass("disabled-btn");
            } else {
                $("#searcFieldhBtn").addClass("disabled-btn");
            }
        });
        $("#seachFieldTxt").unbind('change').change(function () {
            if ($(this).val() !== "") {
                $("#searcFieldhBtn").removeClass("disabled-btn");
            } else {
                $("#searcFieldhBtn").addClass("disabled-btn");
            }
        });
    }

    var makeReservationEvent = function () {
        $("#saveBtn").click(function () { makeReservation(); });
    }

    var updateReservationEvent = function () {
        $("#updateBtn").click(function () { updateReservation(); });
    }

    var validateRequiredFieldsEvent = function () {
        $("#form-time-booking").unbind('change').change(function () {
            if (validateRequiredFieldsForSearch()) {
                $("#searchAvailbleSlotsBtn").parent().removeClass("disabled-btn");
            }
            else {
                $('#searchAvailbleSlotsBtn').parent().addClass("disabled-btn");
            }
        });
        $("#quantityTxt").unbind('keyup').keyup(function () {
            if (validateRequiredFieldsForSearch()) {
                $("#searchAvailbleSlotsBtn").parent().removeClass("disabled-btn");
            }
            else {
                $('#searchAvailbleSlotsBtn').parent().addClass("disabled-btn");
            }
        });
        setDropdown($('#resourceDrp').parent(), 'type3');
        $(".searchSlotRequired .dropdown li a").on('click', function () {
            if (validateRequiredFieldsForSearch()) {
                $('#searchAvailbleSlotsBtn').parent().removeClass("disabled-btn");
            }
            else {
                $('#searchAvailbleSlotsBtn').parent().addClass("disabled-btn");
            }
        });
        if (validateRequiredFieldsForSearch()) {
            $('#searchAvailbleSlotsBtn').parent().removeClass("disabled-btn");
        }
        else {
            $('#searchAvailbleSlotsBtn').parent().addClass("disabled-btn");
        }
    }

    var searchItemEvent = function () {
        var dropdownResource = $('#resourceDrp .dropdown li a');
        dropdownResource.on('click', function () {
            var resourceValue = $(this).parent().attr('data-value');
            searchItems('', resourceValue);
            var regNoMandatory = $(this).parent().attr('regNoMandatory');
            if (regNoMandatory != null) {
                if (regNoMandatory.toLowerCase() == 'true') {
                    $('#IDReg').addClass('updateRequiered');
                } else {
                    $('#IDReg').removeClass('updateRequiered');
                }
            } else {
                $('#IDReg').removeClass('updateRequiered');
            }
        });

        var dropdownArticle = $('#articlesDrp .dropdown li a');
        dropdownArticle.on('click', function () {
            var articleValue = $(this).parent().attr('data-value');
            if (articleValue != null && articleValue != "") {
                var articleArr = articleValue.split('-');
                if (articleArr.length == 3) {
                    var artNo = articleArr[0];
                    var driedUndried = articleArr[1];
                    if (driedUndried.length >= 2) {
                        driedUndried = driedUndried.substring(0, 1).toUpperCase() + driedUndried.substring(1);
                    }

                    $('#agrementDriedUnDried').val(driedUndried);

                }
            }
        });
        $("#loadRadio").unbind("change").change(function () {
            searchItems("", $("#resourceDrp").attr("data-value"));
        });

        $("#unloadradio").unbind("change").change(function () {
            searchItems("", $("#resourceDrp").attr("data-value"));
        });

        $("#selectedDate").change(function () {
            searchItems("", $("#resourceDrp").attr("data-value"));
        });
    };

    var searchAvailableSlotsEvent = function () {
        $('#searchAvailbleSlotsBtn').on('click', function () {
            searchAvalibleSlotAction();
        });
    };

    var searchCustomerEvent = function () {
        $('#searcFieldhBtn').click(function () { searchCustomer(); });
    };

    var bokaSearchTypeChangeEvent = function () {
        $("#search-option > ul > li > a").click(function () {
            var placeholderData = $(this).html();
            $("#seachFieldTxt").attr('placeholder', "Ange " + placeholderData.toLowerCase());
        });
    }

    var searchCustomerWhenLoadPageEvent = function () {
        if ($('#seachFieldTxt').val() != '') {
            $("#searcFieldhBtn").removeClass("disabled-btn");
            searchCustomer();
        } else {
            $("#searcFieldhBtn").addClass("disabled-btn");
        }
    }

    var init = function () {
        searchItemEvent();
        validateRequiredFieldsEvent();
        searchAvailableSlotsEvent();
        makeReservationEvent();
        updateReservationEvent();
        searchCustomerEvent();
        bokaSearchTypeChangeEvent();
        searchCustomerWhenLoadPageEvent();
        validateButtonSearchEvent();
    }

    return {
        init: init,
        searchItems: searchItems,
        searchAvalibleSlotAction: searchAvalibleSlotAction
    }
};

var ListingBookingPage = function () {
    //search
    var doSearch = function () {
        blockUI();
        var fromDateValue = $('#fromDateTxt').val();
        var toDateValue = $('#toDateTxt').val();
        var customerNoValue = $('#customerNo').val();
        var regNoValue = $('#RegNo').val();
        var resourceGroupIdValue = $('#bl-resourceDrp').attr("data-value");
        var resourceType = '';
        var resourceNumber = '';
        var referenceType = '';
        var referenceNumber = '';
        $('#SaveTable').hide();
        $('#updateBtn').hide();

        var obj = new Object();
        obj.ResourceGroupId = resourceGroupIdValue;
        obj.FromDate = fromDateValue;
        obj.ToDate = toDateValue;
        obj.RegNo = regNoValue;
        obj.CustomerNo = customerNoValue;
        obj.ReferenceType = referenceType;
        obj.ReferenceNumber = resourceNumber;
        obj.PurchseOrderLine = referenceNumber;
        var containerDto = new Object();
        containerDto.request = obj;

        $.ajax({
            type: 'POST',
            url: '/api/listing-booking/SearchBookings',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(containerDto),

            success: function (response) {
                searchComplete(response);
                unBlockUI();
            },
            error: function (response) {
                alert('Error: ' + response);
                $("#search-result-book-listing").hide();
                unBlockUI();
            }
        });

        var searchComplete = function (response) {
            $("#search-result-book-listing").html(response);
            if (response.resultSearchBookings == undefined) {
                $("#search-result-book-listing").show();
            } else {
                $("#search-result-book-listing").hide();
            }
            deleteBookingEvents();
            editBookingEvent();
            printPageEvents();
            $(document).trigger("enhance.tablesaw");
        };
    };

    //delete
    var deleteBooking = function (deleteButton) {
        var dataValues = $(deleteButton).parent().parent();

        var reservationId = dataValues.find("> #reservationId > a").html();
        var reservationDate = dataValues.find("#from-date").val();
        var time = dataValues.find("#from-time").val();
        var owner = dataValues.find("> #owner").html();
        var customerNo = dataValues.find("> #customer-number").html();
        var arr = new Array();
        arr[arr.length] = '<p>';
        arr[arr.length] = '<span class="ui-icon ui-icon-alert" style="float:left; margin:0 7px 20px 0;"></span>';

        arr[arr.length] = 'Vill du verkligen radera bokningen<br \/> kl: ' + time + '?';
        arr[arr.length] = '<\/p>';

        clearContentToDeleteDialog();
        fillContentToDeleteDialog("Ta bort bokning?", arr.join(''));
        $("#deleteDialog").find(".btn-ok").on("click", function () {
            deletReservationProcessList(reservationId, owner, customerNo, reservationDate, time);
        });

        var deletReservationProcessList = function (reservationId, owner, customerNo, reservationDate, time) {
            clearFormList();
            var o = new Object();
            o.reservationId = reservationId;
            o.owner = owner;
            o.customerNo = customerNo;
            o.dateRegistered = reservationDate + " " + time;
            $.ajax({
                type: 'POST',
                url: '/api/time-booking/DeleteReservation',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(o),

                success: function (response) { deletReservationSuccessList(response.deleteReservations); },
                error: function (response) { deletReservationErrorList(); }
            });

            var deletReservationSuccessList = function (response) {
                var arr = new Array();
                arr[arr.length] = '<p>';
                for (var i = 0; i < response.length; i++) {
                    if (response[i].Status == 500) {

                        arr[arr.length] = '<span class="ui-icon ui-icon-alert" style="float:left; margin:0 7px 20px 0;"></span>';
                    }
                    arr[arr.length] = response[i].Message + '<br \/>';
                }
                arr[arr.length] = '<\/p>';

                clearContentToModal("#dialogArea");
                fillContentToModal("#dialogArea", "Borttagning", arr.join(''));
                $("#dialogArea").find(".btn-ok").on("click", function () {
                    doSearch();
                });

                $('#SaveTable').hide();
                $('#updateBtn').hide();
            }

            var deletReservationErrorList = function () {
                $('#SaveTable').hide();
                $('#updateBtn').hide();
                alert('Fel uppstod vid anropet till tjänsten.');
            }
        }

    }

    //edit
    var editBooking = function (editButton) {
        var copyValuesToEnableEditList = function (editOjb) {
            $('#myDisplayMessage').html(editOjb.displayMessage);
            $("div").data("dateRegisteredList", editOjb.dateRegistered);
            $('#SaveTable').show();
            $('#IDLevnr').val(editOjb.idLevnr);
            $('#IDReg').val(editOjb.idReg);
            $('#IDOvrigt').val(editOjb.idOvrigt);
            var reminder = $("#IDReminder");
            if (editOjb.idReminder === "0" || editOjb.idReminder === "") {
                reminder.attr("data-value", "");
                reminder.find(">a").html("Ingen påminnelse");
            } else {
                reminder.attr("data-value", editOjb.idReminder);
                var subReminder = reminder.find('ul > li[data-value="' + editOjb.idReminder + '"]');
                var valueSubReminder = subReminder.find("a").html();
                subReminder.addClass("selected");
                reminder.find(">a").html(valueSubReminder);
            }

            $('#IDEpost').val(editOjb.idEpost);
            $('#IDMobil').val(editOjb.idMobil);

            $('#saveAction').val('Update');
            $('#ReservationID').val(editOjb.reservationId);


            $('#SpeditorNo_ForUpdate').val(editOjb.speditorNo);

            $('#CustomerNo_ForUpdate').val(editOjb.customerNo);
            $('#CustomerName_ForUpdate').val(editOjb.customerName);

            $('#newCustomerNo').val(editOjb.customerNo);
            $('#newCustomerName').val(editOjb.customerName);

            $('#TransportOrderNo_ForUpdate').val(editOjb.transportOrderNo);
            $('#ContractNo_ForUpdate').val(editOjb.contractNo);
            $('#ReminderEmail_ForUpdate').val(editOjb.reminderEmail);
            $('#ReminderSMS_ForUpdate').val(editOjb.reminderSms);
            $('#Owner').val(editOjb.owner);


            $('#dispalyMessageHeader').html('Ändra bokning genom att ändra uppgifter nedan och klicka Uppdatera bokning.');
            if ($('#CustomerName_ForUpdate').val() != editOjb.customerName) {
                alert('fel');
            }

            $('#updateBtn').unbind('click');
            $('#updateBtn').click(function () { updateReservationList(); });
            $('#newCustomerNo').val(editOjb.customerNo);

            $('#updateBtn').show();
        }
        var updateReservationList = function () {
            var oldCustomerNo = $('#CustomerNo_ForUpdate').val();
            var oldCustomerName = $('#CustomerName_ForUpdate').val();

            var newCustomerNo = $('#newCustomerNo').val();
            var newCustomerName = $('#newCustomerName').val();
            var loadUnLoadVale = $('#loadOrUnlodRad').val();
            var mobileNo = $('#IDMobil').val();
            var leveransforsakransnummer = $('#IDLevnr').val();
            var note = $('#IDOvrigt').val();
            var reminderInMinutesBefore = $('#IDReminder').attr("data-value");
            var licensePlateNo = $('#IDReg').val();
            var email = $('#IDEpost').val();
            var owner = $('#Owner').val();
            var reservationId = $('#ReservationID').val();
            var speditor = $('#SpeditorNo_ForUpdate').val();

            if (validateSaveTable(email, mobileNo, licensePlateNo, leveransforsakransnummer)) {
                var innerDto = new Object();
                innerDto['Note'] = note;
                innerDto['ReminderInMinutesBefore'] = reminderInMinutesBefore;
                innerDto['EmailAddress'] = email;
                innerDto['NewCustomerName'] = newCustomerName;
                innerDto['NewCustomerNo'] = newCustomerNo;
                innerDto['OldCustomerName'] = oldCustomerName;
                innerDto['OldCustomerNo'] = oldCustomerNo;

                innerDto['Leveransforsakransnummer'] = leveransforsakransnummer;
                innerDto['LicensePlateNo'] = licensePlateNo;
                innerDto['MobilePhone'] = mobileNo;
                innerDto['ReminderInMinutesBefore'] = reminderInMinutesBefore;
                innerDto['EmailAddress'] = email;
                innerDto['SpeditorNo'] = speditor;

                innerDto['ReservationId'] = reservationId;
                innerDto['Owner'] = owner;
                innerDto['DateRegistered'] = $("div").data("dateRegisteredList");

                var containerDto = new Object();
                containerDto.reservationToUpdate = innerDto;
                $.ajax({
                    type: 'POST',
                    url: '/api/time-booking/UpdateReservation',
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(containerDto),

                    success: function (response) {
                        updateReservationListSuccess(response.updateReservations);
                    },
                    error: function (response) { updateReservationListError(response.updateReservations); }
                });
            }
        }
        var updateReservationListSuccess = function (response) {
            var arr = new Array();

            arr[arr.length] = '<p>';
            for (var i = 0; i < response.length; i++) {

                if (response[i].Status == 200) {
                    arr[arr.length] = response[i].Message;
                } else {
                    arr[arr.length] = response[i].ErrorMessage;
                }
                arr[arr.length] = '<br \/>';
            }

            arr[arr.length] = '<\/p>';

            clearContentToModal("#dialogArea");
            fillContentToModal("#dialogArea", "Bokningen uppdaterad", arr.join(''));
            clearFormList();

            $('#SaveTable').hide();
            $('#updateBtn').hide();
            doSearch();
        }
        var updateReservationListError = function (response) {
            $('#SaveTable').hide();
            alert('updateReservationError');
            doSearch();
        }

        var editObj = new Object();
        var dataValues = $(editButton).parent().parent();
        editObj.resourceName = dataValues.find("#resourceName").val();
        editObj.itemName = dataValues.find("#itemName").val();
        editObj.weight = dataValues.find(">#weight").html();
        editObj.fromTime = dataValues.find("#from-time").val();
        editObj.itemNo = dataValues.find(">#itemNo").html();
        editObj.idLevnr = dataValues.find(">#leveransforsakransnr").html();
        editObj.idReg = dataValues.find(">#licensePlateNo").html();
        editObj.idOvrigt = dataValues.find("#note").val();
        editObj.idReminder = dataValues.find("#reminderMinutesBefore").val();
        editObj.idEpost = dataValues.find("#emailAddress").val();
        editObj.idMobil = dataValues.find("#mobileNo").val();
        editObj.reservationId = dataValues.find(">#reservationId > a").html();
        editObj.speditorNo = dataValues.find("#speditorNo").val();
        editObj.customerNo = dataValues.find(">#customer-number").html();
        editObj.customerName = dataValues.find("#customer-name").val();
        editObj.transportOrderNo = dataValues.find("#transportOrderNo").val();
        editObj.contractNo = dataValues.find("#contractNo").val();
        editObj.reminderEmail = dataValues.find("#reminderEmail").val();
        editObj.reminderSms = dataValues.find("#reminderSms").val();
        editObj.owner = dataValues.find(">#owner").html();
        editObj.dateRegistered = dataValues.find("#dateRegistered").val();
        editObj.displayMessage = 'Tid: ' + editObj.fromTime + ', Kund: ' + editObj.customerNo + ' ' + editObj.customerName + ', Bokningsnr: ' + editObj.reservationId + ', Artikel: ' + editObj.itemNo + ' ' + editObj.itemName + editObj.weight + '<br \/>Resurs: ' + editObj.resourceName;
        copyValuesToEnableEditList(editObj);
    }

    var clearFormList = function () {
        $('.fieldToClearWithClearForm').val('');
        $('#SaveTable').hide();
        $('#updateBtn').hide();

        $('#IDEpost').val($('#IDEpost_StartValue').val());
        $('#IDMobil').val($('#IDMobil_StartValue').val());
        $('#saveAction').val('New');
        //$('#saveBtn').val('Spara bokningen');


        $('#saveAction').val('New');
        $('#myDisplayMessage').html('');
        $('#dispalyMessageHeader').html('&nbsp;');
        //$('#saveBtn').button('disable');
    }

    var printPageEvents = function () {
        var printBtn = $('#printLink');
        $(printBtn).unbind("click");
        $(printBtn).click(function () {
            var blockSearchResult = $("#search-result-book-listing");
            var printWin = window.open();
            blockSearchResult.find("#printLink").hide();
            printWin.document.write(blockSearchResult.html());
            blockSearchResult.find("#printLink").show();
            printWin.print();
            printWin.close();
        });
    }

    var searchBookingEvents = function () {
        $('#search-booking-button').click(function (e) {
            doSearch();
            e.preventDefault();
        });
    }
    var deleteBookingEvents = function () {
        var deleteBtn = $(".boka-listing-deleteBtn");
        $(deleteBtn).unbind("click");
        $(deleteBtn).click(function (e) {
            deleteBooking($(this));
            e.preventDefault();
        });
    }
    var editBookingEvent = function () {
        var editBtn = $(".boka-listing-editBtn");
        $(editBtn).unbind("click");
        $(editBtn).click(function (e) {
            editBooking($(this));
            clearValidateSaveTable();
            e.preventDefault();
        });
    }

    var init = function () {
        searchBookingEvents();
    }

    return {
        init: init
    }
}

var validateSaveTable = function (email, phone, regnr, levnr) {
    var reEmail = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    var rePhone = /^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$/;
    var reRegnr = /^([a-zA-Z]{3}\d{3})+$/;
    var reLevnr = /^\d+$/;
    var result = true;
    clearValidateSaveTable();

    if (email !== "" && !reEmail.test(email)) {
        result = false;
        $("#IDEpost").addClass("error");
        $("#IDEpost-error").show();
    }
    
    if (phone !== "" && !rePhone.test(phone)) {
        result = false;
        $("#IDMobil").addClass("error");
        $("#IDMobil-error").show();
    }
    if ($('#IDReg').hasClass('updateRequiered')) {
        if (!reRegnr.test(regnr)) {
            result = false;
            $("#IDReg").addClass("error");
            $("#IDReg-error").show();
        }
    }
    if (!reLevnr.test(levnr)) {
        result = false;
        $("#IDLevnr").addClass("error");
        $("#IDLevnr-error").show();
    }
    return result;
}

var clearValidateSaveTable = function() {
    $("#IDEpost-error").hide();
    $("#IDMobil-error").hide();
    $("#IDLevnr-error").hide();
    $("#IDReg-error").hide();
    $("#IDEpost").removeClass("error");
    $("#IDMobil").removeClass("error");
    $("#IDReg").removeClass("error");
    $("#IDLevnr").removeClass("error");
}
