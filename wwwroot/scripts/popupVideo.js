$(document).ready(function() {
    $('.testimonial img').click(function() {
        $(this).parent().children('#overlay').fadeIn(300);
    });

    $('#close').click(function() {
        $(this).parent().parent().fadeOut(300);
        $("#overlay").fadeOut(300);
    });

    $('#overlay').click(function () {
        $(this).fadeOut(300);
    });
});