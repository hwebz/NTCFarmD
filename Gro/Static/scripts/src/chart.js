(function () {
    $(document).ready(function () {
        var chartDiv = document.getElementById("lm__chart-dygraphs");
        var labelDiv = document.getElementById('lm__chart-status');
        if (window.Dygraph && chartDiv && labelDiv) {
            g2 = new Dygraph(
                document.getElementById("lm__chart-dygraphs"),
                chartDiv.dataset.rowdata,
                {
                    labels: [
                        "Date", "Höstvete Kvarn", "Höstvete Foder", "Maltkorn", "Oljeväxter", "Grynhavre"
                    ],
                    labelsDiv: document.getElementById('lm__chart-status'),
                    //labelsSeparateLines: false,
                    labelsKMB: false,
                    legend: 'always',
                    colors: ["#0e8013", "#808080", "#030389", "#fc0019", "#aed8e5"],
                    width: "100%",
                    height: 540,
                    title: chartDiv.dataset.title,
                    //xlabel: 'Date',
                    //ylabel: 'Count',
                    axisLineColor: 'white',
                    //showLabelsOnHighlight: false
                    // drawXGrid: false
                }
            );
        }

        var selectMonthElement = document.getElementById("selectMonthPeriod");
        if (selectMonthElement) {
            $("#selectMonthPeriod > li > ul > li > a").click(function () {
                var dataValueType = $(this).parent().attr("data-value");
                location.href = window.location.origin + location.pathname + "?period=" + dataValueType;
            });
        }
    });
})();
