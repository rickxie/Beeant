
window.checkSaveTag = function (id) {
    var sender = (typeof id === 'string' ? $("#" + id) : $(id));
    if (sender.attr("issave") == true) {
        return false;
    }
    sender.attr("issave", "true");
    sender.attr("disabled", "disabled");
    $(".savemask").css("height", $(window).height() + "px").show();
    $(".savemask").find("img").css("left", $(window).width() / 2 - $(".savemask").find("img").width() / 2 + "px")
        .css("top", $(window).height() / 2 - $(".savemask").find("img").height() / 2 + "px"); 
    return true;
}

window.removeSaveTag = function (id) {
    var sender = (typeof id === 'string' ? $("#" + id) : $(id));
    sender.removeAttr("issave");
    sender.removeAttr("disabled");
    $(".savemask").hide();
}