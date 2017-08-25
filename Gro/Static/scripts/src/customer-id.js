$(function () {

    var CustomerIDPage = function () {
        var parentCheckbox = $(".customer-id-block .lm__block-box .parent-checkbox .lm__checkbox input");
        var childCheckboxes = parentCheckbox.parents(".lm__block-box").find(".sub-checkbox .lm__checkbox input");
        var tooltipLink = $(".link-to-open-popup");
        var tooltipMask = $(".lm__information-modal__wrapper .mask");

        var addCheckboxEvent = function () {
            if (parentCheckbox) {
                parentCheckbox.change(function () {
                    var _that = $(this);
                    var childCheckboxesWrapper = _that.parents(".lm__block-box").find(".sub-checkbox");
                    var childCheckboxes = childCheckboxesWrapper.find(".lm__radio input");

                    if (childCheckboxesWrapper && childCheckboxes) {
                        if (_that.is(":checked")) {
                            childCheckboxes.parent().removeClass('sub-hidden');
                            $(childCheckboxes[0]).prop('checked', true);
                        } else {
                            childCheckboxes.parent().addClass('sub-hidden');
                           childCheckboxes.prop('checked', false);
                        }
                    }

                    return false;
                });
            }
        };

        var tooltipEvent = function () {
            if (tooltipLink && tooltipMask) {
                tooltipLink.click(function (e) {
                    e.preventDefault();
                    var tooltipPanel = $(this).parent().next();
                    if (tooltipPanel) tooltipPanel.removeClass("hidden");
                    return false;
                });

                tooltipMask.click(function () {
                    $(this).parent().addClass("hidden");
                });
            }
        };

        return {
            init: function () {
                addCheckboxEvent();
                tooltipEvent();
                return false;
            }
        }
    };

    $(document).ready(function () {
        var customerID = new CustomerIDPage();
        customerID.init();
    });

});