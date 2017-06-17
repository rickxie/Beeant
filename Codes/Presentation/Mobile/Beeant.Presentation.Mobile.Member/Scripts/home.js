$(document).ready(function () {
    var cutor = new Winner.ImageCutor("divCutContainer");
    cutor.Initialize();
    cutor.LoadFinishFunction = function (file) {
        $(".cutbottom").show();
    }
    cutor.SaveImage=function(data) {
        $.ajax({
            type: "Post",
            url: "/Member/UpdateHeadUrl",
            data: { headUrlValue: data.split(',')[1], headUrl: cutor.File[0].files[0].name },
            async: false,
            dataType: "json",
            success: function (data) {
                $("#divSaveCut").removeAttr("IsClick");
                if (data.Status) {
                    $("#imgHeadUrl").attr("src", data.Message + "?v=" + new Date());
                    $("#divCutContainer").hide();
                    $(".cutbottom").hide();
                } else {
                    alert(data.Message);
                }
            },
            error: function () {
                alert("系统忙，请稍候再试");
            }
        });

    }
    $("#imgHeadUrl").click(function () {
        $("#divHeadUrl").show();
        $(".mask").show();
    });
    $("#divCloseHeadUrl").click(function() {
        $(".mask").hide();
        $("#divHeadUrl").hide();
        $("#divCutContainer").hide();
        $(".cutbottom").hide();
    });
    $("#divChangeHeadUrl").click(function () {
        cutor.Select();
        $(".mask").hide();
        $("#divHeadUrl").hide();
    });
    $("#divColseCut").bind("click",function () {
        $("#divCutContainer").hide();
        $(".cutbottom").hide();
    });
    $("#divSaveCut").bind("click", function () {
        if ($("#divSaveCut").attr("IsClick") == "true")
            return;
        $("#divSaveCut").attr("IsClick", "true");
        cutor.Cut();
    });
    $("#divCutContainer").css("height", $(window).height());
    $("#divCutContainer").find(".cutimg").css("height", $(window).height());
    var height = $("#divCutContainer").height() - $("canvas").height() - $(".cutbottom").height();
    $("#divCutContainer").find(".topmask").css("height", height / 2);
    $("#divCutContainer").find(".bottommask").css("height", height / 2).css("top", $("#divCutContainer").find("canvas").height() + height / 2 + "px");
    $("#divCutContainer").find("canvas").css("top", height / 2);
});