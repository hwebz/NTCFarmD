$(function () {
    function is_touch_device() {
        return 'ontouchstart' in window // works on most browsers
            || navigator.maxTouchPoints; // works on IE10/11 and Surface
    };

    var HeaderMainNavigation = function () {

        var toggleButton = $(".lm_toggle-menu");
        var parentList = $(".lm__main-navigation > ul > li").has("ul");
        var childList = $(".lm__main-navigation > ul li ul li").has(".lm__child-sub-navigation");
        var mainNav = toggleButton.next();
        var settingBtn = $(".lm__settings > a");
        var subNavigation = $(".lm__sub-navigation");
        var settingWrapper = $(".lm__user-avatar-wrapper");

        var removeActiveState = function () {
            mainNav.removeAttr("style");
            parentList.find('ul').slideUp("slow", function () {
                $(this).prev().removeClass('minus-sign').addClass('plus-sign');
                $(this).removeAttr("style");
            });
            //childList.find('ul').slideUp("slow", function () {
            //    $(this).prev().find('.toggle-icon').removeClass('minus-sign').addClass('plus-sign');
            //    $(this).removeAttr("style");
            //});
            toggleButton.removeClass('active');
            $(".humb").removeClass('open');
            settingBtn.parent().removeClass('active').find(".lm_sub-settings").slideUp();
            $(".lm__navigation-wrapper.showed-menu").removeClass("showed-menu");
            settingWrapper.find(".lm__user-avatar").next().slideUp();
            $(".user-info-wrapper > a").next().slideUp();
        }

        var addSettingEvent = function () {
            var avatar = settingWrapper.find(".lm__user-avatar");
            avatar.click(function () {
                removeActiveState();
                var sub = $(this).next();

                $(this).toggleClass("active");
                if (sub.css('display') == 'none') {
                    sub.slideDown();
                } else {
                    sub.slideUp();
                }
                return false;
            });
        };

        var addUserInfoEvent = function () {
            var userInfoWrapper = $(".user-info-wrapper > a");
            userInfoWrapper.click(function () {
                removeActiveState();
                if ($(this).next().css("display") == "none") $(this).next().slideDown();
                else $(this).next().slideUp();
                return false;
            });
        }

        var addToggleMenu = function () {

            // Remove padding from settings dot button if anchor link has no content.
            if ($(".lm__settings > a").text() == "") {
                $(".lm__settings > a").css({ padding: "0 25px" });
            }

            $(document).on("click", function () {
                removeActiveState();
            }).on('touchstart', function (e) {
                var $this = $(e.target);
                if ($this.attr('class') == 'lm__user-info' || $($this).parents('.user-info-wrapper').length > 0) {
                    return;
                } else {
                    $(".user-info-wrapper > a").next().slideUp();
                }
            });

            // Toggle Settings
            settingBtn.off('click').on('click', function (e) {
                e.stopPropagation();
                removeActiveState();
                var subMenu = $(this).next();

                parentList.find('.lm_sub-settings').slideUp();
                if (subMenu.css('display') == 'none') {
                    subMenu.slideDown('fast', function () {
                        if ($(this).is(':visible'))
                            $(this).css('display', 'inline-block');
                    });
                    $(this).parent().addClass('active');
                } else {
                    subMenu.slideUp('fast');
                    $(this).parent().removeClass('active');
                }

                toggleButton.removeClass('active');
                $(".humb").removeClass('open');
                subNavigation.slideUp("slow", function () {
                    $(this).removeAttr("style");
                });

                return false;
            });

            // Toggle menu navigation
            toggleButton.off('click').on("click", function (e) {
                e.stopPropagation();

                var $this = $(this);
                var subMenu = $this.next();
                if (is_touch_device()) {
                    subMenu.css({
                        'max-height': 'calc(100vh - 100px)',
                        'overflow': 'hidden',
                        'overflow-y': 'visible'
                    });
                }
                if (subMenu.css('display') == 'none') {
                    subMenu.slideDown('fast');
                    $this.addClass('active').find('.humb').addClass('open');
                } else {
                    subMenu.slideUp('fast');
                    $this.removeClass('active').find('.humb').removeClass('open');
                }

                settingBtn.parent().removeClass('active').find('>a').next().slideUp();
                return false;
            });

            // Toggle sub-menu
            parentList.each(function () {
                $(this).find("> a").off('click').on("click", function (e) {
                    var $this = $(this);
                    parentList.not($this.parent()).find('ul').slideUp(function () {
                        $(this).prev().removeClass('minus-sign').addClass('plus-sign');
                    });
                    settingBtn.parent().removeClass('active').find(".lm_sub-settings").slideUp();
                    $this.next().slideToggle(function () {
                        if ($(this).css('display') == 'none') {
                            $this.removeClass('minus-sign').addClass('plus-sign');
                        } else {
                            $this.removeClass('plus-sign').addClass('minus-sign');
                        }
                    });
                    if ($(window).width() < 1083) {
                        $this.next().find(".lm__child-sub-navigation").slideToggle();
                    }
                    return false;
                });
            });

            //childList.each(function () {
            //    $(this).find(">a .toggle-icon").off('click').on("click", function (e) {
            //        var $this = $(this);
            //        childList.not($this.parent().parent()).find('ul').slideUp(function () {
            //            $(this).prev().find('.toggle-icon').removeClass('minus-sign').addClass('plus-sign');
            //        });
            //        $this.parent().next().slideToggle(function () {
            //            if ($(this).css('display') == 'none') {
            //                $this.removeClass('minus-sign').addClass('plus-sign');
            //            } else {
            //                $this.removeClass('plus-sign').addClass('minus-sign');
            //            }
            //        });
            //        return false;
            //    });
            //});
        }

        var resizeSubNav = function () {
            var wHeight = $(window).height() - 50;
            var subHeight = $(".lm_sub-settings").height();
            if (subHeight > wHeight) $(".lm_sub-settings").css({ height: $(window).height() - 50 + "px" });
            else $(".lm_sub-settings").css('height', 'auto');
            //$(".humb").removeClass('open');
        }

        var setIconForParentMenu = function () {
            // set icon for menu which have sub menu
            parentList.each(function () {
                var $this = $(this);
                if ($this.has('.lm__sub-navigation')) {
                    $this.find('>a').addClass('plus-sign');
                }
            });

            //childList.each(function () {
            //    var $this = $(this);
            //    if ($this.has('.lm__child-sub-navigation')) {
            //        $this.find('>a .toggle-icon').addClass('plus-sign');
            //    }
            //});
        }

        var responsiveIcon = function () {
            if ($(window).width() >= 1083) {
                parentList.find('>a').addClass('hide-icon');
                //childList.find('>ul').hide();
            }
            else {
                parentList.find('>a').removeClass('hide-icon');
                //childList.find('>ul').removeAttr('style');
            }
        }

        return {
            init: function () {
                setIconForParentMenu();
                responsiveIcon();

                removeActiveState();
                addToggleMenu();
                addSettingEvent();
                addUserInfoEvent();
                resizeSubNav();

                $(window).on('resize orientationchange', function () {
                    if ($(window).width() >= 1083) $(".lm__main-navigation > ul").removeAttr("style");

                    resizeSubNav();
                    responsiveIcon();
                });
            }
        }
    };

    var SidebarNavigation = function () {

        var sidebarNav = $(".lm__category-nav > ul > li").has("ul");

        var addAccordion = function () {
            sidebarNav.each(function () {
                if ($(this).find("ul li.active").length > 0 || $(this).hasClass('active')) $(this).addClass('open').find('ul').slideDown();
                else $(this).removeClass('open');
            });
        }

        return {
            init: function () {
                addAccordion();
            }
        }
    }

    var MessageHub = function () {
        var getTotalUnreadMessage = function () {
            $.ajax({
                dataType: "json",
                url: '/api/user/get-inbox-statistics',
                type: 'post',
                cache: false,
                success: function (data) {
                    if (data && data.totalUnRead > 0) {
                        if (!$('#totalUnread').hasClass('lm__messages-count')) {
                            $('#totalUnread').addClass('lm__messages-count');
                        }
                        $('#totalUnread').html(data.totalUnRead);
                    }
                    else {
                        if ($('#totalUnread').hasClass('lm__messages-count')) {
                            $('#totalUnread').removeClass('lm__messages-count');
                        }
                        $('#totalUnread').html('');
                    }

                    if (data && data.totalStarred > 0) {
                        if (!$('#totalStarred').hasClass('lm__messages-stat-count')) {
                            $('#totalStarred').addClass('lm__messages-stat-count');
                        }
                        $('#totalStarred').html(data.totalStarred);
                    }
                    else {
                        if ($('#totalStarred').hasClass('lm__messages-stat-count')) {
                            $('#totalStarred').removeClass('lm__messages-stat-count');
                        }
                        $('#totalStarred').html('');
                    }

                }
            });
        }

        return {
            init: function () {
                getTotalUnreadMessage();
            }
        }
    }


    var CustomerList = function () {
        var registerEventHandler = function () {
            var customerContainer = $('.user-id__wrapper.lm__radio');
            if (!!customerContainer) {
                customerContainer.on("click", function () {
                    var newCustomerNo = $(this).attr("data-customer-no");
                    var currentActiveNo = $('a.lm__user-info').attr("data-active-No");
                    if (newCustomerNo === currentActiveNo) {
                        return;
                    }
                    var newCustomerName = $(this).attr("data-customer-name");

                    // udpate newvalue for view;
                    $('.lm__user-info .lm__user-id').html(newCustomerNo);
                    $('a.lm__user-info').attr("data-active-No", newCustomerNo);
                    $('.lm__user-info .lm__user-summary').html(newCustomerName);

                    $("#newCustomerNumber").val(newCustomerNo);
                    //$("#referenceLink").val(window.location.href);
                    $("#customerNumberForm").submit();

                });
            }
        }

        return {
            init: function () {
                registerEventHandler();
            }
        }
    }

    // disable :hover on touch devices
    // based on https://gist.github.com/4404503
    // via https://twitter.com/javan/status/284873379062890496
    // + https://twitter.com/pennig/status/285790598642946048
    // re http://retrogamecrunch.com/tmp/hover
    // NOTE: we should use .no-touch class on CSS
    // instead of relying on this JS code
    function removeHoverCSSRule() {
        try {
            var ignore = /:hover/;
            for (var i = 0; i < document.styleSheets.length; i++) {
                var sheet = document.styleSheets[i];
                if (!sheet.cssRules) {
                    continue;
                }
                for (var j = sheet.cssRules.length - 1; j >= 0; j--) {
                    var rule = sheet.cssRules[j];
                    if (rule.type === CSSRule.STYLE_RULE && ignore.test(rule.selectorText)) {
                        sheet.deleteRule(j);
                    }
                }
            }
        }
        catch(e) {
        }
    }

    function adjustStickyHeaderWidth() {
        var contentWidth = $(".lm__contents").width();
        var stickyHeader = $(".sticky-header");
        var leftColumn = $(".internal-page > .wrapper >.layout > .layout__item:first-child").width();
        var internalNav = $(".internal-navigation");

        if (stickyHeader.hasClass("fixed")) {
            stickyHeader.css({ width: contentWidth + "px" });
        } else {
            stickyHeader.removeAttr("style");
        }
        internalNav.css({ "width": leftColumn });
    }

    $(document).ready(function () {
        if ($(".lm__navigation-wrapper") != undefined && $(".lm__navigation-wrapper") != null && $(".lm__navigation-wrapper").length > 0) {
            // Client Sites - Main Navigation
            if ($(".lm__navigation-wrapper").length > 0) {
                $(window).scroll(function () {
                    if ($(this).scrollTop() > $(".lm__navigation-wrapper").offset().top) {
                        $(".lm__navigation").addClass("fixed");
                    }
                    if ($(this).scrollTop() <= $(".lm__navigation-wrapper").offset().top) {
                        $(".lm__navigation").removeClass("fixed");
                    }
                });
            }

            var headerMainNav = new HeaderMainNavigation();
            headerMainNav.init();

            var sidebarNav = new SidebarNavigation();
            sidebarNav.init();
            var messageHub = new MessageHub();
            messageHub.init();

            // change customer number
            if (!!$("#customer_list_selection")) {
                $("#customer_list_selection").on("change", function (e) {
                    var form = $(e.target).parents("form");
                    form.submit();
                });
            }

            var customerList = new CustomerList();
            customerList.init();

            if (is_touch_device()) {
                removeHoverCSSRule();
            }
        }

        // Internal Sites - Search box
        if ($(".lm__header.internal-page").length > 0) {


            $(window).scroll(function () {
                if ($(this).scrollTop() > $(".lm__header.internal-page").offset().top) {
                    $(".sticky-header").addClass("fixed")
                        .css({ width: $(".lm__contents").width() + "px" });
                }
                if ($(this).scrollTop() <= $(".lm__header.internal-page").offset().top) {
                    $(".sticky-header").removeClass("fixed")
                        .removeAttr("style");
                }
            });

            $(window).resize(function() {
                adjustStickyHeaderWidth();
            })
            adjustStickyHeaderWidth();
        }
        if (GroCommon) {
            GroCommon.handleExternalLink();
        }
    });

});
