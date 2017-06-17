$(document).ready(function () {
    $("#btnBind").css("left", ($(window).width() - $("#btnBind").width()) / 2);
    function remove(sender) {
        var id = $(sender).attr("DataId");
        $.ajax({
            type: "Post",
            url: "/Wechat/Remove",
            data: {
                id: id
            },
            async: false,
            dataType: "json",
            success: function (data) {
                if (data.Status) {
                    $(sender).remove();
                } else {
                    alert(data.Message);
                }
            },
            error: function () {
                alert("系统忙，请稍候再试");
            }
        });
    }
    function bindOpEvent(sender) {
        function setOpButton(sender, rev) {
            if (rev) {
               
                $(sender).find("input[class='remove']").show(100);
            } else {
       
                $(sender).find("input[class='remove']").hide();
            }
        }

        $(sender).find(".remove").click(function() {
            remove(sender);
        });
        $(sender).bind("swipeleft", function () {
            setOpButton(this, true);
            return false;
        }).bind("swiperight", function () {
            setOpButton(this, false);
            return false;
        });
    }
    $(".list").find(".element").each(function (index, sender) {
        bindOpEvent(this);
    });

    $("input[class='input']").bind("focus", function() {
        $(this).attr("DefaultValue", $(this).val());
        $(this).val("");
    }).bind("blur", function () {
        if (this.value == "" || this.value == $(this).attr("DefaultValue")) {
            this.value = $(this).attr("DefaultValue");
            return;
        }
        var input = this;
        $.ajax({
            type: "Post",
            url: "/Wechat/Modify",
            data: {
                Id: $(input).attr("DataId"),
                Name: $(input).val()
            },
            async: false,
            dataType: "json",
            success: function (data) {
                if (data.Status) {
                    $(input).attr("DefaultValue", $(input).val());
                } else {
                    alert(data.Message);
                    input.value = $(input).attr("DefaultValue");
                }
            },
            error: function () {
                alert("系统忙，请稍候再试");
            }
        });
    });



});
 