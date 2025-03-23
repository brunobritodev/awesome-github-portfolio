$(document).ready(function () {


    document.querySelectorAll('#about a[href]').forEach(link => {
        link.setAttribute('target', '_blank');
    });
    /*Sidebar Menu*/
    "use strict";

    /*Preloader*/
    $(".preloader-wrap").delay(1500).fadeOut('slow');

    /*Navigation*/

    $(window).scroll(function () {
        var nav = $('.navbar');
        var top = 200;
        if ($(window).scrollTop() >= top) {

            nav.addClass('inbody');

        } else {
            nav.removeClass('inbody');
        }
    });


    $('a[href*="#"]:not([href="#"])').on('click', function () {
        if (location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '') && location.hostname == this.hostname) {
            var target = $(this.hash);
            target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
            if (target.length) {
                $('html, body').animate({
                    scrollTop: target.offset().top
                }, 1000);
                return false;
            }
        }
    });

    //var windowBottom = $(window).height();
    var index = 0;
    $(document).scroll(function () {
        var top = $('#skills').height() - $(window).scrollTop();

        if (top < -600) {
            if (index == 0) {

                $('.chart').easyPieChart({
                    easing: 'easeOutBounce',
                    onStep: function (from, to, percent) {
                        $(this.el).find('.percent').text(Math.round(percent));
                    }
                });

            }
            index++;
        }
    })


    $(function () {
        $('.selector').animatedHeadline({
            animationType: 'rotate-2'
        });
    })


    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl)
    })
    
    var particeExist = document.getElementById('particles-js');
    if (particeExist) {
        particlesJS.load('particles-js', '/assets/js/resume/particles.json');
    }

});