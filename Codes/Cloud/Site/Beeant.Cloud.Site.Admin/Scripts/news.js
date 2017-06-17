$(document).ready(function () {
    $("#btnAdd").css("left", ($(window).width() - $("#btnAdd").width()) / 2);

    function showSaveContent() {
        $(".addcontent").css("height", $(window).height() + "px");
        $(".addcontent").show();
        $(this).hide();
        $(".news").hide();
        $(window).scrollTop(0);
    }
    function hideSaveContent() {
        $(".addcontent").hide();
        $("#btnAdd").show();
        $(".addcontent").find(".content").find("input[type='text']").each(function () {
            $(this).val($(this).attr("ShowValue"));
            $(this).attr("class", "input ctrlshow");
        });
        $(".addcontent").find(".content").find("textarea").each(function () {
            $(this).val($(this).attr("ShowValue"));
            $(this).attr("class", "input ctrlshow");
        });
        $(".news").show();
    }
    $("#btnAdd").click(function () {
        showSaveContent();
    });
    $("#btnCancel").click(function () {
        hideSaveContent();
    });
    function removeNews(sender) {//删除
        if (!confirm("您确定要删除吗"))
            return;
        if (!window.checkSaveTag(sender)) {
            return;
        }
        var id = $(sender).attr("DataId");
        $.ajax({
            type: "Post",
            url: "/News/Remove",
            data: {
                id: id
            },
            async: true,
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
    function upNews(sender) {
        if (!window.checkSaveTag(sender)) {
            return;
        }
        var id = $(sender).attr("DataId");
        if ($(sender).prev().length == 0)
        {
            window.removeSaveTag(sender);
            return ;
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
            url: "/News/Modify",
            data: {
                id: id,
                Sequence: sequence 
            },
            async: true,
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
    function downNews(sender) {
        if (!window.checkSaveTag(sender)) {
            return;
        }
        var id = $(sender).attr("DataId");
        if ($(sender).next().length == 0)
        {
            window.removeSaveTag(sender);
            return;
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
            url: "/News/Modify",
            data: {
                id: id,
                Sequence: sequence
            },
            async: true,
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
    var showEdit = function (sender) {
        if (!window.checkSaveTag(sender)) {
            return;
        }
        $("#hfId").val($(sender).attr("DataId"));
        $.ajax({
            type: "Post",
            url: "/News/Get",
            data: { id: $("#hfId").val() },
            async: true,
            dataType: "json",
            success: function (data) {
                if (data != null) {
                    if (data.Title == "") {
                        $("#txtTitle").attr("class", "input ctrlshow");
                        $("#txtTitle").val($("#txtTitle").attr("ShowValue"));
                    } else {
                        $("#txtTitle").attr("class", "input");
                        $("#txtTitle").val(data.Title);
                    }
                    if (data.Content == "") {
                        $("#txtContent").attr("class", "input ctrlshow");
                        $("#txtContent").val($("#txtContent").attr("ShowValue"));
                    } else {
                        $("#txtContent").attr("class", "input");
                        $("#txtContent").val(data.Content);
                    }
                    $("#ckIsShow")[0].checked = data.IsShow;
                    showSaveContent();
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

        $(sender).find(".up").bind("click", function () {
            upNews(sender);
         
        });
        $(sender).find(".down").bind("click", function () {
   
            downNews(sender);
        });
        $(sender).find(".remove").bind("click", function () {
            removeNews(sender);
        });
        $(sender).bind("swipeleft", function () {
            setOpButton(this, true);
        }).bind("swiperight", function () {
            setOpButton(this, false);
        }).find(".title").bind("click", function () {
            showEdit(sender);
        });
    }
    //异步加载
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
           Url: "/News/List",
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
               if (length < 24)
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

    $("#btnSave").click(function () {
        if (!window.checkSaveTag("btnSave")) {
            return false;
        }
        if (!window.Validator.ValidateSubmit()) {
            alert("您输入的信息有误，请检测后重新输入");
            window.removeSaveTag("btnSave");
            return false;
        }
        function getSaveData() {
            return {
                Title: $("#txtTitle").val() == $("#txtTitle").attr("ShowValue") ? " " : $("#txtTitle").val(),
                Content: $("#txtContent").val() == $("#txtContent").attr("ShowValue") ? " " : $("#txtContent").val(),
                IsShow: $("#ckIsShow")[0].checked
            };
        }
        var saveData = getSaveData();
        var url = "/News/Add";
        if ($("#hfId").val() != "") {
            url = "/News/Modify";
            saveData.Id = $("#hfId").val();
        }
        $.ajax({
            type: "Post",
            url: url,
            data: saveData ,
            async: true,
            dataType: "json",
            success: function (data) {
                if (data.Status) {
                    if ($("#hfId").val() == "") {
                        var html = ' <div class="element"' + '" Sequence="' + data.Sequence + '" DataId="' + data.Id + '"><div class="up" ></div>' +
                               '<div class="title">' + saveData.Title + '</div>' +
                               '<div class="down" ></div>' +
                               '<input class="remove" style="display: none;" style="display: none;" value="删除"/>' +
                               '</div>';
                        $(".list").append(html);
                        bindOpEvent($(".list").find(".element")[$(".list").find(".element").length - 1]);
                    } else {
                        $(".list").find(".element[DataId='" + saveData.Id + "']").find(".title").html(saveData.Title);
                    }
                    hideSaveContent();
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
    ///设置保存按钮
    $('.addcontent').find("input[type='text']").bind('focus', function () {
        $("#btnSave").hide();

    }).bind('blur', function () {
        $("#btnSave").show();
    });
});
