$(function () {
    var MachineListing = function () {

        var toggleViewBtns = $("#toggle-view a");
        var toggleView = $(".machine-list");
        var machineItems = $('.machine-item');

        var ToggleViewEvent = function () {
            toggleViewBtns.each(function () {
                var $this = $(this);
                if ($this.hasClass('active')) {
                    $this.css({backgroundColor: '#fff'});
                }
            });
            toggleViewBtns.on('click', function (e) {
                e.preventDefault();

                var $this = $(this);
                var tgr = $this.data('target');

                // Switch view
                toggleView.addClass((tgr == 'list') ? 'list-view' : '');
                toggleView.removeClass((tgr == 'grid') ? 'list-view' : '');

                // Change active class for selected btn
                toggleViewBtns.removeClass('active').removeAttr('style');
                $this.addClass('active').css({ backgroundColor: '#fff' });

                return false;
            });
        };

        return {
            init: function () {
                ToggleViewEvent();

                //$(window).on('resize orientationchange', function () {
                //    if (window.innerWidth < 768) {
                //        toggleView.removeClass('list-view');
                //        toggleViewBtns.removeClass('active').first().addClass('active');
                //    }
                //});
            }
        }
    };

    $(document).ready(function () {
        /** ======================= DEPREACATED ======================= */
        $('.lm__list-type a').on('click', function () {
            var dataLayout = $(this).attr("data-layout");

            // Change active layout button
            $('.lm__list-type a').removeClass('active');
            $(this).addClass('active');

            // Change layout
            $(".list-maskin").removeClass("list-type").removeClass("grid-type").addClass(dataLayout).show();

            //$(".list-maskin").hide();
            //$("."+dataLayout).show();
        });

        $('.lm__list-type a.active').click();

        $('#checkRegisterBtn').click(function () {
            $('.lm__modal-alert').show();
        });

        /** ======================= DEPREACATED ======================= */

        var machineListing = new MachineListing()
            machineListing.init();
    });

});
