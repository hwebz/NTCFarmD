var $ = window["$"];

export function scrollToTop() {
    try {
        window.scrollTo(0, 0);
    } catch (e) {

    }

    try {
        document.body.scrollTop = 0;
    } catch (e) {

    }
}

export function calculateBreadcrumStep() {
    $(".lm__form-progress").each(function () {
        var step = $(this).find("li");
        var activeStep = $(this).find("li.active");
        var activeLine = $(this).find(".lm__form-progress__line");
        var endPoint = activeStep.offset().left - $(this).offset().left + step.width() / 2;
        console.log(endPoint);
        activeLine.css({ width: endPoint });
    });
};
