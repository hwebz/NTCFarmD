$(function () {

    var UpcomingDelivery = function () {
        var carousel = $(".lm__carousel-block");
        //var carouselItem = carousel.find(".lm__carousel__content .lm__carousel__item .lm__blue-btn");
        var carouselItem = carousel.find(".lm__carousel__item a").not(".callendar");
        var $itemPopup = $("#upcoming-detail");
        var agreementSidebar = $(".lm__block.agreement");

        var showInformation = function () {
            carouselItem.on('click', function (e) {
                e.preventDefault();
                var $this = $(this);

                var deliveryObj = getDeliveryInfor($this);

                $.ajax({
                    type: "post",
                    data: deliveryObj,
                    dataType: "html",
                    url: "/api/upcomming-deliveries/get-detail",
                    success: function (data) {
                        $itemPopup.html(data);
                        $itemPopup = $("#upcoming-detail");
                        $itemPopup.fadeIn();
                    }
                });

                return false;
            });
        };

        function getDeliveryInfor($carouselItem) {
            //var $inforElement = $carouselItem.parent().parents();
            var $inforElement = $carouselItem.parent();
            var isDeliveryFromCustomer = $carouselItem.closest("section").attr("data-isFromCustomer");
            return {
                orderNo: $inforElement.attr("data-orderNo"),
                orderLine: $inforElement.attr("data-orderLine"),
                itemName: $inforElement.attr("data-itemName"),
                orderQuantity: $inforElement.attr("data-orderQuantity"),
                warehouse: $inforElement.attr("data-warehouse"),
                container: $inforElement.attr("data-container"),
                planedDeliveryDate: $inforElement.attr("data-planedDate"),
                isFromCustomer: isDeliveryFromCustomer
            }
        }

        // sidebars
        var toggleSidebar = function () {
            if (agreementSidebar.length > 0) agreementSidebar.each(function () {
                var items = $(this).find(".lm__news-item");
                items.each(function () {
                    var $that = $(this);
                    var toggleLink = $that.find(">a");

                    if ($that.hasClass('open')) {
                        $that.find('.detail-information').slideDown('fast');
                    }


                    toggleLink.on('click', function () {
                        var $this = $(this);
                        var parentItem = $this.parent();
                        var isOpen = parentItem.hasClass('open');
                        var detailInformation = $this.next();

                        items.removeClass('open').find('.detail-information').slideUp('fast');
                        if (isOpen) {
                            parentItem.removeClass('open');
                            detailInformation.slideUp('fast');
                        } else {
                            parentItem.addClass('open');
                            detailInformation.slideDown('fast');
                        }
                        return false;
                    });
                });
            });
        };

        return {
            showInformation: function () {
                showInformation();
            },
            toggleSidebar: function () {
                toggleSidebar();
            }
        }
    };

    $(document).ready(function () {
        // stick carousel
        if ($.fn.slick) $('.lm__carousel-block .lm__carousel__content').slick({
            dots: true,
            infinite: false,
            speed: 300,
            slidesToShow: 3,
            slidesToScroll: 3,
            responsive: [
            //{
            //    breakpoint: 1366,
            //    settings: {
            //        slidesToShow: 3,
            //        slidesToScroll: 3
            //    }
            //},
            {
                breakpoint: 1100,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1,
                    variableWidth: true,
                    centerMode: true,
                    centerPadding: '40px',
                    infinite: true
                }
            },
            {
                breakpoint: 640,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1,
                    dots: false,
                    arrows: false,
                    variableWidth: true,
                    centerMode: true,
                    centerPadding: '40px',
                    //infinite: true
                }
            }
            ]
        });

        // init event for item in upcoming delivery
        var upcomingDelivery = new UpcomingDelivery();
        upcomingDelivery.showInformation();
        upcomingDelivery.toggleSidebar();
    });
});