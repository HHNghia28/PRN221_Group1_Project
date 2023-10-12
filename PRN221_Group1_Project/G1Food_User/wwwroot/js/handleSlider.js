$('#slider-header').owlCarousel({
    loop:true,
    items: 1,
    margin:10,
    dots:true,
    nav:true,
    autoplay:true,
    autoplayTimeout: 2000,
    autoplayHoverPause:true,
});

$('#slider-voucher').owlCarousel({
    loop:true,
    items: 1,
    dotsEach: 1,
    margin:10,
    dots:true,
    autoplay:true,
    autoplayTimeout: 2000,
    autoplayHoverPause:true,
    responsive:{
        0:{
            items:1
        },
        600:{
            items:2,
            nav:false
        },
        1000:{
            items:4,
            loop:true,
        }
    }
})