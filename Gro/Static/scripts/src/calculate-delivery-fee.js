$(function () {
    $(document).ready(function () {
        if ($("#calculate_delivery_fee")) {
            if (jQuery.validator) {
                $("#calculateForm").validate({
                    ignore: [],
                    errorElementClass: 'error',
                    errorClass: 'error-item',
                    errorElement: 'span',
                    rules: {
                        LorryType: "required",
                        DeliveryAdress: {
                            required: true
                        },
                        Quantity: {
                            required: true,
                            number: true,
                            min: 1
                        },
                        DeliveryDate: {
                            required: true,
                            swedishDate: true
                        },
                        Artikel_Frakt: {
                            required: true
                        }
                    },
                    messages: {
                        LorryType: {
                            required: "Du måste välja leveranssätt"
                        },
                        DeliveryAdress: {
                            required: "Du måste välja en leveransadress"
                        },
                        Quantity: {
                            required: "Du måste ange en kvantitet större än 0",
                            number: "Ange ett numeriskt värde",
                            min: "Du måste ange en kvantitet större än 1"
                        },
                        DeliveryDate: {
                            required: "Du måste ange ett giltigt datum",
                            swedishDate: "Du måste ange ett giltigt datum"
                        },
                        Artikel_Frakt: {
                            required: "Du måste välja en artikel"
                        }
                    },
                    highlight: function (element, errorClass, validClass) {
                        $(element).addClass(this.settings.errorElementClass).removeClass(errorClass);
                    },
                    unhighlight: function (element, errorClass, validClass) {
                        $(element).removeClass(this.settings.errorElementClass).removeClass(errorClass);
                    },
                });
            }

            $("#calculate_delivery_fee").bind("click", function (e) {
                if (!$("#calculateForm").valid()) {
                    return;
                };
                if ($('.calculate_result .calculate_fee')) {
                    $('.calculate_result .calculate_fee').html('...');
                }
                $("#loaderCalculation").show();
                var url = "/api/deliveryfee/caluculate";
                var calculateObj = getCalculateObject();

                $.ajax({
                    type: "post",
                    data: calculateObj,
                    dataType: "html",
                    url: url,
                    success: function (data) {
                        //console.log(url);
                        $(".calculate_result").html(data);
                    },
                    complete: function (jqXHR) {
                        $('#loaderCalculation').hide();
                    },
                    error: function () {
                        $(".calculate_result").html('');
                    }
                });

            });
        }

        function getCalculateObject() {
            return {
                supplier: $('.calculate_supplier').val(),
                lorryType: $('#LorryType').val(),
                deliveryAddressId: $('#DeliveryAdress').val(),
                quantity: $('.calculate_quantity').val(),
                deliveryDate: $('.calculate_deliveryDate').val(),
                itemId: $('#Artikel_Frakt').val()
            };
        }
    });

});