function InitShared(url) {
    window.lazyloadImage = function () {
        $("img").lazyload({
            placeholder: url + "/images/logo.png",
            effect: "fadeIn",
            failurelimit: 10,
            threshold: 200
        });
    };
    $(document).ready(
        function ($) {
            window.lazyloadImage();
        });
    $("#footer").find("a[menu='true']").bind("touchstart", function () {
        var display = $(this).parent().find("ul").css("display");
        $("#footer").find("ul").hide();
        if (display == "none") {
            $(this).parent().find("ul").show();
        } else {
            $(this).parent().find("ul").hide();
        }
        return false;
    });
    $(document).click(function() {
        $("#footer").find("ul").hide();
    });
    $(document).bind("touchstart",function () {
        $("#footer").find("ul").hide();
    });
}