;(function( $ ) {
	$(document).trigger("enhance.tablesaw");

	// hide toggle if have no option columns.
	if ($(".tablesaw").length > 0) {
	    $(".tablesaw-bar").hide();
	    $(".tablesaw").each(function () {
	        var _that = $(this);
	        var tableHeads = _that.find('thead tr th');
	        var tableBar = _that.prev();

	        if (tableHeads.length > 2) {
	            tableBar.removeAttr("style");
	        } else {
	            _that.css("min-width", "inherit");
	        }
	    });
	}

	function DeleteTableRowWithAnimation(cellButton) {
        var row = $(cellButton).closest("tr").addClass("deleted").children("td");
        setTimeout(function () {
                $(row)
                .animate({ paddingTop: 0, paddingBottom: 0 }, 500)
                .wrapInner("<div />")
                .children()
                .slideUp(500, function() { $(this).closest('tr').remove(); });
    	    }, 1000
        );
	}

	$(".lm__internal-table tbody tr .lm__form-btn").click(function () {
	    DeleteTableRowWithAnimation($(this));
	});
})( jQuery );