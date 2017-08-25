$(function () {

    var MittKonto = function () {
        var uploadBtn = $(".lm__user-inform__wrapper .lm__avatar__upload-btn");
        var removeAvatarBtn = $(".lm__user-inform__wrapper .popup-sub li .remove-avatar");
        var uploadBtns = $(".file-upload-input");

        var addUploadPopupEvent = function () {
             $(document).on("click", function () {
                 $('.popup-sub').fadeOut();
             });
             $(".popup-sub").click(function () { return false; });
            uploadBtn.click(function () {
                var popupSub = $(this).parent();
                if (popupSub.has('.popup-sub')) {
                    popupSub.find('.popup-sub').fadeToggle();
                }
                return false;
            });

            //removeAvatarBtn.click(function () {
            //    var avatar = $(this).parents(".lm__avatar").find(".avatar-img");
            //    avatar.attr("src", $(this).attr("default-avatar"));
            //    return false;
            //});
        };

        var addUploadImageEvent = function () {
            uploadBtns.each(function (event) {
                $(this).change(function (event) {
                    var imgEle = $(this).parent().find(".avatar-img");
                    imgEle.attr('src', URL.createObjectURL(event.target.files[0]));
                });
            });
        }

        return {
            init: function () {
                addUploadPopupEvent();
                //addUploadImageEvent();
                return false;
            }
        }
    };

    $(document).ready(function () {
        //var mit = new MittKonto();
        //mit.init();
    });

});