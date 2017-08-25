jQuery(function() {

    var DeliveryNote = DeliveryNote || (function() {

        var $orderLink = jQuery('.deliveryNote a.showPdf');

        var getPdf = function(orderNumber, fabricId, rowNumber) {
            GroCommon.blockUI();
            var url = '/api/delivery-note/get-pdf' + '?orderNumber=' + orderNumber + '&fabricId=' + fabricId + '&rowNumber=' + rowNumber;

            jQuery.ajax({
                dataType: "json",
                url: url,
                success: function(data) {
                    GroCommon.unblockUI();

                    if (data != null && data.pdfData != null) {

                        var pdfData = data.pdfData;
                        var $dialog = $(".pdf-getting-error");
                        if (pdfData && pdfData.trim().endsWith(".pdf")) {
                            window.open('http://wcf.lantmannenlantbruk.se/pdf/' + pdfData);
                        } else {
                            var errorMessage = "Ett fel uppstod när pdf skulle laddas";
                            $('.success-header-title', $dialog).html(errorMessage);
                            $dialog.fadeIn();
                        }

                        //if (pdfData.length == 1) {
                        //    if (pdfData[0].indexOf('.pdf') > -1) {
                        //        window.open('http://wcf.lantmannenlantbruk.se/pdf/' + pdfData[0]);
                        //    } else {
                        //        // error
                        //        alert(pdfData[0]);
                        //    }
                        //} else {
                        //    //Todo
                        //}
                    }
                },
                error: function(e) {
                    GroCommon.unblockUI();
                    //console.log("Error occured:" + ':\nReadystate:' + XMLHttpRequest.readyState + '\nResponsetext:' + XMLHttpRequest.responseText + '\nResponseXML:' + XMLHttpRequest.responseXML + '\nStatus:' + XMLHttpRequest.status + '\nStatusText:' + XMLHttpRequest.statusText);
                }
            });
        }

        var init = function() {
            $orderLink.click(function() {

                var orderNumber = jQuery(this).attr('cdata-orderNumber');
                var activePdf = jQuery(this).attr('cdata-activePdf');
                var fabricId = jQuery(this).attr('cdata-fabricID');
                var rowNumber = jQuery(this).attr('cdata-rowNumber');
                var customerNumber = jQuery(this).attr('cdata-customerNumber');
                var system = jQuery(this).attr('cdata-system');

                if (activePdf == 'True') {
                    getPdf(orderNumber, fabricId, rowNumber);
                } else {
                    var url = null;
                    switch (system) {
                    case '3':
                    {
                        break;
                    }
                    case '4':
                    {
                        break;
                    }
                    default:
                    {
                        break;
                    }
                    }
                }
            });
        };
        return {
            init: init
        };
    })();

    $(document).ready(function() {
        DeliveryNote.init();
    });
});


