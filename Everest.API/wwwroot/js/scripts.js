$(document).ready(function() {
    $('#carousel-prev').on('click', function(ev) {
        if ($(this).attr('disabled') == undefined)
            carouselScroll($('#carousel-container'), $('#carousel-prev, #carousel-next'))
    })

    $('#carousel-next').on('click', function(ev) {
        if ($(this).attr('disabled') == undefined)
            carouselScroll($('#carousel-container'), $('#carousel-prev, #carousel-next'), true)
    })

    $('#products-left').on('click', function(ev) {
        if ($(this).attr('disabled') == undefined)
            carouselScroll($('#products'), $('#products-left, #products-right'))
    })

    $('#products-right').on('click', function(ev) {
        if ($(this).attr('disabled') == undefined)
            carouselScroll($('#products'), $('#products-left, #products-right'), true)
    })
})

function carouselScroll(container, buttons, isForward = false) {
    var itemWidth = container.children().first().outerWidth()
    
    if (isForward == false)
        itemWidth *= -1

    var value = container.scrollLeft() + itemWidth

    buttons.attr('disabled', true)
    setTimeout(() => {
        buttons.removeAttr('disabled', '')
    }, 500);

    container.animate({ scrollLeft: value }, 500)
}