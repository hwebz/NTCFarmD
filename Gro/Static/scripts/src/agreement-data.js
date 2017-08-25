$(function () {
    var modal = $(".lm__modal-alert");
    var containerData = modal.find(".lm__modal-dialog");
    var agreementData;

    var AgreementData = function (url) {
        var modal = $(".lm__modal-alert");
        var closeModal = modal.find(".modal-close");
        var loadingIcon = modal.find(".loader-wrapper");
        var confirm = modal.find(".success-confirm");

        var CloseModalAlert = function () {
            modal.fadeOut(function () {
                containerData.find(".modal-header-title, .modal-content").remove();
            });

            loadingIcon.fadeIn();

            return false;
        }

        var addEvents = function () {
            modal.bind("click", CloseModalAlert);
            containerData.bind("click", function (event) { event.preventDefault(); return false; });
            closeModal.bind("click", CloseModalAlert);
            confirm.bind("click", CloseModalAlert);
        }

        return {
            init: function () {
                addEvents();
                return true;
            }
        }
    };

    function bindData(data) {
        var loadingIcon = modal.find(".loader-wrapper");
        $(data).insertBefore(".button-confirm");

        loadingIcon.fadeOut();
    }

    $(document).ready(function () {
        
        // enhance table saw to make responsive table
        $(document).trigger("enhance.tablesaw");

        agreementData = new AgreementData();
        agreementData.init();

        if ($(".showAnalyze")) $(".showAnalyze").bind('click',
            function () {
                var url = '/api/agreement/farmsample/' + $(this).attr("data-id").toString();
                $.ajax({
                    dataType: "html",
                    url: url,
                    success: function (data) {
                        bindData(data);
                    }
                });

                modal.fadeIn();

                return false;
            });

        if ($(".showPriceHedging")) $(".showPriceHedging").bind('click',
            function () {
                var url = '/api/agreement/price-hedging/' + $(this).attr("data-id").toString();
                $.ajax({
                    dataType: "html",
                    url: url,
                    success: function (data) {
                        bindData(data);
                    }
                });

                modal.fadeIn();

                return false;
            });

        if ($(".showWeighInMoreInfo")) $(".showWeighInMoreInfo").bind('click',
            function () {

                var url = '/api/weigh-in/more-info/' + $(this).attr("data-id").toString();
                url += "?date=" + encodeURIComponent($(this).attr("data-date")) + "&sort=" + encodeURIComponent($(this).attr("data-sort"));
                $.ajax({
                    dataType: "html",
                    url: url,
                    success: function (data) {
                        bindData(data);
                    },
                    error: function () {
                        modal.fadeOut();
                    }
                });

                modal.fadeIn();

                return false;
            });

        if ($(".showWeighInAnalyze")) $(".showWeighInAnalyze").bind('click',
            function () {
                var url = '/api/weigh-in/analyze/' + $(this).attr("data-id").toString();
                url += "?date=" + encodeURIComponent($(this).attr("data-date")) + "&sort=" + encodeURIComponent($(this).attr("data-sort"));
                $.ajax({
                    dataType: "html",
                    url: url,
                    success: function (data) {
                        bindData(data);
                    },
                    error: function()
                    {
                        modal.fadeOut();
                    }
                });
                modal.fadeIn();
                return false;
            });
    });
});
