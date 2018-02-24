$(function () {
    var menu_ul = $('.menu > li > ul'),
           menu_a = $('.menu > li > a'),
           arrow_img = $('.arrow-img');
    menu_ul.hide();
    arrow_img.click(function (e) {
        if (!menu_a.hasClass('active')) {
                    menu_a.removeClass('active');
                    menu_ul.filter(':visible').slideUp('normal');
                    menu_a.addClass('active').next().stop(true, true).slideDown('normal');
                } else {

                menu_a.removeClass('active');
                menu_a.next().stop(true, true).slideUp('normal');
        }
        e.preventDefault();
    })
});