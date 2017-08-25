$(function () {

    var SokGrobarhet = function () {

        var container = $(".lm__sok-grobarhet");
        var infoBtn = container.find(".lm__info");
        var modal = container.find(".lm__modal-alert");
        var messageClosable = $(".lm__message-closable .close-btn, .lm__modal-alert .button-confirm .success-confirm-inform");
        var informModal = $(".lm__information-modal__wrapper");

        var modalPopup = function () {
            infoBtn.click(function () {
                modal.fadeIn();
            });
        };

        var closeMessage = function () {
            messageClosable.click(function () {
                $(this).parents(".lm__modal-alert, .lm__message-closable").fadeOut();
            });
        };

        var informModalEvent = function () {
            $(".lm__information-modal__close-btn").click(function () {
                $(this).parents(".lm__information-modal__wrapper").addClass("hidden");
                $(this).parents(".lm__modal-alert").fadeOut();
                return false;
            });
        };

        return {
            init: function () {
                modalPopup();
                closeMessage();
                informModalEvent();
                return false;
            }
        }
    }

    var CommonModal = function () {
        var modal = $(".lm__modal__wrapper");
        var closeXBtn = modal.find(".lm__wide-modal__close-btn");
        var closeBtn = modal.find(".lm__btn-close");

        var closeModalEvent = function () {
            closeXBtn.click(function() {
                $(this).parents(".lm__modal__wrapper").addClass("hidden");
                //console.log("lm-btn");
                return false;
            });

            closeBtn.click(function() {
                $(this).parents(".lm__modal__wrapper").addClass("hidden");
                return false;
            });
        }

        return {
            init: function() {
                closeModalEvent();
                return false;
            }
        }
    }

    $(document).ready(function () {
        var sok = new SokGrobarhet();
        sok.init();

        var modal = new CommonModal();
        modal.init();
    });
});
