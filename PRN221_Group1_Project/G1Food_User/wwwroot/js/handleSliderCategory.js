
$('#slider-category').owlCarousel({
    loop:true,
    dotsEach: 1,
    items: 6,
    margin:16,
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