$(document).ready(function () {
    $("#btnAdd").css("left", ($(window).width() - $("#btnAdd").width()) / 2);
    $("#btnAdd").click(function () {
        $(".addcontent").css("height", $(window).height() + "px").css("top", "0");
        $(".addcontent").show();
        $(window).scrollTop(0);
        $(this).hide();
        $(".tag").hide();
        $(window).scrollTop(0);
        $("#txtAdd").focus();

    });
    $("#btnCancel").click(function () {
        $(".addcontent").hide();
        $("#btnAdd").show();
        $(".tag").show();
    });
    $("input[class='input']").bind("focus", function () {
        this.value = "";
    });
    function removeTag(sender) {//删除
        if (!confirm("您确定要删除吗"))
            return;
        if (!window.checkSaveTag(sender)) {
            return false;
        }
        var id = $(sender).attr("DataId");
        $.ajax({
            type: "Post",
            url: "/Tag/Remove",
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
                window.removeSaveTag(sender);
            },
            error: function () {
                alert("系统忙，请稍候再试");
                window.removeSaveTag(sender);
            }
        });
    }
    function upTag(sender) {
        if (!window.checkSaveTag(sender)) {
            return false;
        }
        var id = $(sender).attr("DataId");
        if ($(sender).prev().length == 0) {
            window.removeSaveTag(sender);
            return false;
        }
        var prevSender = $(sender).prev();
        var prevprevSender = $(sender).prev().prev();
        var sequence = 0;
        if (prevprevSender.length == 0) {
            sequence = parseInt(prevSender.attr("Sequence")) - 2500;
        } else {
            sequence = parseInt(prevSender.attr("Sequence"));
            sequence = sequence - (sequence - parseInt(prevprevSender.attr("Sequence"))) / 2;
        }
        sequence = parseInt(sequence);
        $.ajax({
            type: "Post",
            url: "/Tag/Modify",
            data: {
                id: id,
                Sequence: sequence
            },
            async: false,
            dataType: "json",
            success: function (data) {
                if (data.Status) {
                    $(sender).attr("Sequence", sequence);
                    $(sender).insertBefore($(sender).prev());
                } else {
                    alert(data.Message);
                }
                window.removeSaveTag(sender);
            },
            error: function () {
                alert("系统忙，请稍候再试");
                window.removeSaveTag(sender);
            }
        });
    }
    function downTag(sender) {
        if (!window.checkSaveTag(sender)) {
            return false;
        }
        var id = $(sender).attr("DataId");
        if ($(sender).next().length == 0) {
            window.removeSaveTag(sender);
            return false;
        }
        var nextSender = $(sender).next();
        var nextnextSender = $(sender).next().next();
        var sequence = 0;
        if (nextnextSender.length == 0) {
            sequence = parseInt(nextSender.attr("Sequence")) + 2500;
        } else {
            sequence = parseInt(nextSender.attr("Sequence"));
            sequence = sequence + (parseInt(nextnextSender.attr("Sequence")) - sequence) / 2;
        }
        sequence = parseInt(sequence);
        $.ajax({
            type: "Post",
            url: "/Tag/Modify",
            data: {
                id: id,
                Sequence: sequence
            },
            async: false,
            dataType: "json",
            success: function (data) {
                if (data.Status) {
                    $(sender).attr("Sequence", sequence);
                    $(sender).insertAfter($(sender).next());

                } else {
                    alert(data.Message);
                }
                window.removeSaveTag(sender);
            },
            error: function () {
                alert("系统忙，请稍候再试");
                window.removeSaveTag(sender);
            }
        });
    }
    function bindOpEvent(sender) {
        function setOpButton(sender, rev) {
            if (rev) {
                $(sender).find("div[class='up']").css("margin-left", 0 - $(sender).find("input[class='remove']").width() + "px");
                $(sender).find("input[class='remove']").show(100);
            } else {
                $(sender).find("div[class='up']").css("margin-left", "0");
                $(sender).find("input[class='remove']").hide();
            }
        }
        $(sender).bind("swipeleft", function () {
            setOpButton(this, true);
        }).bind("swiperight", function () {
            setOpButton(this, false);
        });
        $(sender).find("input[class='input']").bind("blur", function () {
            if (!window.checkSaveTag(sender)) {
                return;
            }
            if (this.value == "") {
                this.value = $(this).attr("DefaultValue");
                return;
            }
            var input = this;
            $.ajax({
                type: "Post",
                url: "/Tag/Modify",
                data: {
                    Id: $(sender).attr("DataId"),
                    Name: $(input).val()
                },
                async: true,
                dataType: "json",
                success: function (data) {
                    if (data.Status) {
                        $(input).attr("DefaultValue", $(input).val());
                    } else {
                        alert(data.Message);
                        input.value = $(input).attr("DefaultValue");
                    }
                    window.removeSaveTag(sender);
                },
                error: function () {
                    alert("系统忙，请稍候再试");
                    window.removeSaveTag(sender);
                }
            });
        });
        $(sender).find(".up").bind("click", function () {
            upTag(sender);
        });
        $(sender).find(".down").bind("click", function () {
            downTag(sender);
        });
        $(sender).find(".remove").bind("click", function () {

            removeTag(sender);
        });
        $(sender).find("input").css("margin-left", parseInt($(sender).width() - $(sender).find("input").width() - $(sender).find(".up").width() - $(sender).find(".down").width())/2);
    }


    $("#btnSave").click(function () {
        if (!window.checkSaveTag("btnSave")) {
            return false;
        }
        var inputs = $(".list").find("input[type='text']");
        var sequence = inputs.length == 0 ? 1 : parseInt($(inputs[inputs.length - 1]).attr("Sequence")) + 1;
        var name = $("#txtAdd").val();
        $.ajax({
            type: "Post",
            url: "/Tag/Add",
            data: {
                Name: name,
                Sequence: sequence
            },
            async: true,
            dataType: "json",
            success: function (data) {
                if (data.Status) {
                    $(".addcontent").hide();
                    $("#btnAdd").show();
                    $(".tag").show();
                    var html = ' <div class="element"><div class="up" "></div>' +
                        '<input name="Name" class="input" value="' + name + '" Sequence="' + sequence + '" DataId="' + data.Id + '" DefaultValue="' + name + '" />' +
                        '<div class="down" "></div>' +
                        '<input class="remove" style="display: none;" style="display: none;"  value="删除"/>' +
                        '</div>';
                    $(".list").append(html);
                    bindOpEvent($(".list").find(".element")[$(".list").find(".element").length - 1]);
                } else {
                    alert(data.Message);
                }
                window.removeSaveTag("btnSave");
            },
            error: function () {
                alert("系统忙，请稍候再试");
                window.removeSaveTag("btnSave");
            }
        });

        return false;
    });
    var config = [
   {
       Triggers: [
           {
               Sender: window,
               Event: "scroll",
               Function: function () {
                   return $(document).scrollTop() >= $(document).height() - $(window).height();
               }
           }
       ],
       Loading: { Content: "Content", Type: "Append" },
       Url: "/Tag/List",
       Paramters: { page: 0 },
       Content: $(".list")[0],
       ShowType: "Append",
       DataType: "text",
       RequestType: "Repeat",
       IsExecute: true,
       IsLoadHideContent: false,
       BeginLoadFunction: function () {

       },
       BeginShowFunction: function (sender, info, data) {
           info.Paramters.page = info.Paramters.page + 1;

       },
       EndShowFunction: function (sender, info, data) {
           window.lazyloadImage($(info.Content)[0]);
           var length = $("<div>" + data + "</div>").find(".element").length;
           if (length < 50)
               info.IsFullLoadComplate = true;
           var elemnts = $(info.Content).find(".element");
           for (var i = elemnts.length - length; i < elemnts.length; i++) {
               bindOpEvent(elemnts[i]);
           }


       }
   }
    ];
    var dataloader = new Winner.DataLoader(config);
    dataloader.Initialize();
    ///设置保存按钮
    $('.addcontent').find("input[type='text']").bind('focus', function () {
        $("#btnSave").hide();

    }).bind('blur', function () {
        $("#btnSave").show();
    });
});

