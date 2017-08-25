$(function () {

    var MyAccount = function () {

        var userTypeRadios = $(".choose-user-type .lm__radio input[name='user-type']");
        var subUserTypeMenu = $(".user-type__sub-types");
        var subType = $(".sub-type .lm__checkbox input[type='checkbox']");

        var showSubOptions = function (ele) {
            if (ele.is(":checked")) {
                var subUserType = ele.attr("id");

                subUserTypeMenu.hide();
                subUserTypeMenu.each(function () {
                    if ($(this).attr("data-parent") == subUserType) {
                        $(this).show();
                    }
                });
            }
        };

        var membershipChoosen = function () {

        }

                //console.log(endPoint);
        return {
            init: function () {
                return false;
            }
        }
    };

    $(document).ready(function () {
        var myAcc = new MyAccount();
        myAcc.init();
    });

});