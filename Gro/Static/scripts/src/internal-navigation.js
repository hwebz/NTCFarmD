/*------------------------------------*\
    #INTERNAL-NAVIGATION
\*------------------------------------*/


window.gro = window.gro || {};
gro.modules = gro.modules || {};


gro.modules.internalNavigation = (function () {
    var priv = {};

    priv.toggleNavigation = function(parentNav, isToggleClicked) {
        var isToggleClicked = false || isToggleClicked;
        // check navigation expanded or not
        if (!parentNav.hasClass("expanded")) {
            parentNav.addClass("expanded");
        } else {
            if (!isToggleClicked) parentNav.removeClass("expanded");
        }
    };

    /**
     * Private initialization method
     */
    priv.init = function () {

        /**
         * Click on category link
         * Link should only expand child list, not be followed
         * Fallback (href on the <a> tag) should be the first page in the child list
         */
        $(".js-internal-navigation-category").on("click", function (e) {
            e.preventDefault();
            var control = this;
            var target = $(control).next(".internal-navigation__list--sub-level");
            var parentNav = $(control).parents(".internal-navigation");

            if ($(control).hasClass("is-expanded")) {
                $(target).slideUp(function () {
                    $(control).removeClass("is-expanded");
                    $(target)
                        .removeClass("is-expanded")
                        .removeAttr("style");
                });
            } else {
                $(control)
                    .addClass("is-expanded");
                $(target).slideDown(function () {
                    $(target)
                        .addClass("is-expanded")
                        .removeAttr("style");
                });
            }

            /*
             * Expand navigation if end users click any links which have sub-navigation inside
             */
            priv.toggleNavigation(parentNav, true);
        });

        /**
         * Click on toggle link
         * Link should toggle whole navigation
         */
        $(".internal-navigation__toggle-btn").on("click",
            function(e) {
                e.preventDefault();
                var control = this;
                var target = $(control).parent().find(".internal-navigation__list--sub-level");
                var parentNav = $(control).parents(".internal-navigation");

                /*
                 * Toggle navigation if end users click on toggle btn
                 */
                priv.toggleNavigation(parentNav);

                /*
                 * Collapse all opened sub-navigation when navigation collapsed
                 */
                $(target).slideUp(function () {
                    $(this).prev().removeClass("is-expanded");
                    $(this)
                        .removeClass("is-expanded")
                        .removeAttr("style");
                });
            });
    };

    // Initialize module
    $(function () {
        priv.init();
    });

})();
