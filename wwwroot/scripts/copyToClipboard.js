$("#copyLink").click(function () {
    var copyText = $(this).parent().find("input").val();


    var $temp = $("<input>");
    $("body").append($temp);
    $temp.val(copyText).select();
    document.execCommand("copy");
    $temp.remove();


    var $popup = $(this).parent().find("span");
    $popup.toggleClass("show");
    setTimeout(function () { $popup.toggleClass("show"); }, 3000);

});
