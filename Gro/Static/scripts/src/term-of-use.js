$(function () {
    var TermOfUse = function () {
        function registerHandler() {
            if ($("#term-of-use-no").length === 0 && $("#term-of-use-yes").length === 0) {
                return;
            }

            $("#term-of-use-no").click(function () {
                var firstRefectDialog = $(".lm__main-content").find("#dialog-first-reject");
                firstRefectDialog.fadeIn();

                $(".back-to-term", firstRefectDialog).click(function () {
                    firstRefectDialog.fadeOut();
                });

                $(".reject", firstRefectDialog).click(function () {
                    var secondRejectDialog = $(".lm__main-content").find("#dialog-reconfirm-reject");
                    secondRejectDialog.fadeIn();
                    firstRefectDialog.fadeOut();

                    $(".back-to-term", secondRejectDialog).click(function () {
                        secondRejectDialog.fadeOut();
                    });

                    $(".reject", secondRejectDialog).click(function () {
                        secondRejectDialog.fadeOut();
                        $("#rejectForm").submit();
                    });
                });
            });
        }

        return {
            init: function () {
                registerHandler();
            }
        }
    }

    $(document).ready(function () {
        var termOfUse = new TermOfUse();
        termOfUse.init();
    });
});