$(document).ready(function () {
    $.ajax({
        type: "Post",
        url: "/Home/Mobile/"+window.SiteId,
        data: { },
        async: false,
        dataType: "text",
        traditional: true,
        success: function (data) {
            if (data!="") {
                $("#hfLinkMobile").attr("href", "tel:" + data);
            }  
        },
        error: function () {
      
        }
    });

    $(".toper").find(".name").css("width", $(window).width() - $(".contact").width() - $(".searchico").width() - 10 + "px");
    $("#txtTopSearch").blur(function() {
        if (this.value == "")
            return;
        window.location.href = "/Commodity/Index/" + window.SiteId + "?key=" + this.value;
    });
    //定位
    var isjustop = false;
    function justtop() {
        if (!isjustop)
            return;
        $(".toper").css({ 'position': "absolute" })
       .css("toper", $(document).scrollTop() + "px");
        setTimeout(justtop, 10);
    }


    $('.searchinput').bind('focus', function () {
        isjustop = true;
        justtop();

    }).bind('blur', function () {
        isjustop = false;
        $('.toper').css({ 'position': 'fixed', 'top': '0' });

    });

    //搜索
    $(".searchico").click(function () {
        $("#topnav").hide();
        $(".search").show();
        $(".searchinput")[0].focus();
    });
    $("#hfCancelSearch").click(function () {
        $("#topnav").show();
        $(".search").hide();
        $(".searchinput").val("");
        $(".searchinput").attr("BeforValue", "");
        $(".catalog").show();
    });
});



