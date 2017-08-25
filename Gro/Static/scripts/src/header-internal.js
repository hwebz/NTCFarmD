
$(document).ready(function () {
    $("#header-internal-search .dropdown li a").click(function () {
        var searchOption = $(this).attr("data-value");
        $("#searchOption").val(searchOption);
    });
    $("button#internal-search").click(function () {
        var searchKey = $("#input-key-search").val();
        if (searchKey.trim() !== "") {
            $("#searchKey").val(searchKey);
            $("#internalSearchForm").submit();
        }
    });

    $("header.internal-page a.close-btn").click(function() {
        if ($(this).closest("#customer-popup").hasClass('open')) {
            $.ajax({
                dataType: "json",
                url: '/internal-portal/close-customer-session',
                type: 'get',
                cache: false,
                success: function (data) {
                    if (data) {
                        // reload the page
                        location.reload();
                    }
                }
            });
        }
    });

});

