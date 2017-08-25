﻿$(function () {

    $(document).ready(function () {
        $("form").on("change", ".file-upload-field", function(){
            $(this).parent(".file-upload-wrapper").attr("data-text", $(this).val().replace(/.*(\/|\\)/, '') );
        });

        if ($('#attachments').length > 0) $('#attachments').filer({
		    showThumbs: true,
		    addMore: true,
		    allowDuplicates: false
        });
    });

});