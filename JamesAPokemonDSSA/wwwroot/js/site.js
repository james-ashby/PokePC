$( document ).ready(function() {
    console.log( "ready!" );

    $('.menu-toggle').click(function() {

        $('.site-nav').toggleClass('site-nav-open', 300);
        $(this).toggleClass('menu-open');
    })
});

