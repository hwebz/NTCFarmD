jQuery(function () {
    CustomerCard.init();
});

var CustomerCard = CustomerCard || (function () {
    var customerNumber = "";
    var errorMessages = {
        delError: "",
        updateError: ""
    }
    function addProfileHandler() {
        var $userProfile = $("a.user-profile");
        $userProfile.each(function (idx, item) {
            $(item).click(function () {
                var selectedProfileId = $(item).closest("li.showcase").attr("data-value");
                var userName = $(item).attr("data-userName");
                var loader = $(item).closest("td").find(".loader-wrapper");
                var confirmPanel = $(item).closest("td").find(".confirm-panel .confirm-box");
                if (confirmPanel.length > 0) {
                    var $confirmPanel = $(confirmPanel);
                    $confirmPanel.show();
                    $("input[type=reset]", $confirmPanel).click(function () {
                        $confirmPanel.hide();
                    });
                    $("input[type=button]", $confirmPanel).click(function () {
                        showLoader(loader);
                        changeProfileForUser(userName, selectedProfileId, customerNumber,
                            function () { $confirmPanel.hide(); },
                            function() {
                                showErrorDialog(errorMessages.updateError); 
                            },
                            function() {
                                hideLoader(loader);
                                $confirmPanel.hide();
                            });
                    });
                }
            });
        });

        $(".fa.fa-trash-2").click(function () {
            var userName = $(this).attr("data-userName");
            var userId = $(this).attr("data-userId");
            var userElement = $(this).closest("tr");
            var loader = $(userElement).find(".loader-wrapper");
            var $confirmDialog = $("#dg-del-user-confirmation");
            if ($confirmDialog.length > 0) {
                $confirmDialog.fadeIn();
                // event for confirmation dialog
                $("button.yes").click(function() {
                    showLoader(loader);
                    removeUserFromCustomer(userName, userId, customerNumber,
                        function() {
                             $(userElement).remove();
                        },
                        function () { showErrorDialog(errorMessages.delError); },
                        function() {
                            hideLoader(loader);
                        });
                });
            }
        });
    }
    

    function removeUserFromCustomer(userName, userId, customerNumber, successCallBack, failCallBack, completeCallBack) {
        $.ajax({
            url: '/api/customer-card/remove-user?userName=' + userName + "&userId=" + userId + "&customerNo=" + customerNumber,
            type: "GET",
            dataType: "json",
            success: function (data) {
                if (data) {
                    successCallBack();
                } else {
                    failCallBack();
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                failCallBack();
            },
            complete:function() {
                completeCallBack();
            }
        });
    }
    function showLoader(loader) {
        if (loader != undefined) {
            $(loader).show();
        }
    }

    function hideLoader(loader) {
        if (loader != undefined) {
            $(loader).hide();
        }
    }

    function changeProfileForUser(userName, profileId, customerNo, successCallBack, failCallBack, completeCallBack) {
        $.ajax({
            url: '/api/customer-card/change-profile?userName=' + userName + "&profileId=" + profileId + "&customerNo=" + customerNo,
            type: "GET",
            dataType: "json",
            success: function (data) {
                if (data) {
                    successCallBack();
                } else {
                    failCallBack();
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                failCallBack();
            },
            complete: function () {
                completeCallBack();
            }
        });
    }

    function showErrorDialog(errorMess) {

    }

    var init = function () {
        customerNumber = $("input#customer-no") ? $("input#customer-no").val() : "";
        addProfileHandler();
    }
    return {
        init: init
    }
})();


