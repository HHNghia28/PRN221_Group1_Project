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

$('#slider-category').owlCarousel({
    loop:true,
    dotsEach: 1,
    items: 1,
    margin:1,
    dots:true,
    autoplay:true,
    autoplayTimeout: 2000,
    autoplayHoverPause:true,
    responsive:{
        0:{
            items:2,
        },
        600:{
            items:4,
        },
        1000:{
            items:6,
        }
    }
})