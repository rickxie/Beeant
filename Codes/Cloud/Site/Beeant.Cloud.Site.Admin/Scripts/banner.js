$(document).ready(function () {
    $("#btnAdd").css("left", ($(window).width() - $("#btnAdd").width()) / 2);

    function showSaveContent() {
        $(".addcontent").css("height", $(window).height() + "px");
        $(".addcontent").show();
        $(this).hide();
        $(".banner").hide();
        $(window).scrollTop(0);
        $("#imgFileName").attr("src", $("#imgFileName").attr("OriginalSrc") + "?v=" + new Date());
        $(".addcontent").find("input[type='text']").val("");
        $("#imgFileName").removeAttr("realsrc");
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
        $(".banner").show();
    }
    $("#btnAdd").click(function () {
        showSaveContent();
    });
    $("#btnCancel").click(function () {
        hideSaveContent();
    });
    function removeBanner(sender) {//删除
        if (!confirm("您确定要删除吗"))
            return false;
        if (!window.checkSaveTag(sender)) {
            return false;
        }
        var id = $(sender).attr("DataId");
        $.ajax({
            type: "Post",
            url: "/Banner/Remove",
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
    function upBanner(sender) {
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
            sequence =sequence- (sequence - parseInt(prevprevSender.attr("Sequence")))/2;
        }
        sequence = parseInt(sequence);
        $.ajax({
            type: "Post",
            url: "/Banner/Modify",
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
    function downBanner(sender) {
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
            sequence = sequence + (parseInt(nextnextSender.attr("Sequence"))-sequence ) / 2;
        }
        sequence = parseInt(sequence);
        $.ajax({
            type: "Post",
            url: "/Banner/Modify",
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
    var showEdit = function (sender) {
        if (!window.checkSaveTag(sender)) {
            return false;
        }
        $("#hfId").val($(sender).attr("DataId"));
        $.ajax({
            type: "Post",
            url: "/Banner/Get",
            data: { id: $("#hfId").val() },
            async: true,
            dataType: "json",
            success: function (data) {
                if (data != null) {
                    showSaveContent();
                    if (data.Url == "") {
                        $("#txtUrl").attr("class", "input ctrlshow");
                        $("#txtUrl").val($("#txtUrl").attr("ShowValue"));
                    } else {
                        $("#txtUrl").attr("class", "input");
                        $("#txtUrl").val(data.Url);
                    }
                    $("#ckIsShow")[0].checked = data.IsShow;
                    $("#imgFileName").attr("src", data.FileName + "?v=" + new Date());
              
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
            upBanner(sender);
        });
        $(sender).find(".down").bind("click", function () {
            downBanner(sender);
        });
        $(sender).find(".remove").bind("click", function () {
        
            removeBanner(sender);
        });
        $(sender).bind("swipeleft", function () {
            setOpButton(this, true);
        }).bind("swiperight", function () {
            setOpButton(this, false);
        }).find("img").bind("click", function () {
            showEdit(sender);
        });
        $(sender).find("img").css("width", $(sender).find("img").height() * 1920 / 1000);
        $(sender).find("img").css("margin-left", ($(sender).width() - $(sender).find("img").width() - $(sender).find(".up").width() - $(sender).find(".down").width()) / 2);
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
           Url: "/Banner/List",
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
                Url: $("#txtUrl").val() == $("#txtUrl").attr("ShowValue") ? " " : $("#txtUrl").val(),
                FileName: $("#imgFileName").attr("realsrc") == undefined ? "" : $("#imgFileName").attr("realsrc"),
                IsShow: $("#ckIsShow")[0].checked
            };
        }
        var saveData = getSaveData();
        var url = "/Banner/Add";
        if ($("#hfId").val() != "") {
            url = "/Banner/Modify";
            saveData.Id = $("#hfId").val();
        }
        $.ajax({
            type: "Post",
            url: url,
            data: saveData,
            async: true,
            dataType: "json",
            success: function (data) {
                if (data.Status) {
                    hideSaveContent();
                    if ($("#hfId").val() == "") {
                        var html = ' <div class="element"' + '" Sequence="' + data.Sequence + '" DataId="' + data.Id + '"><div class="up" ></div>' +
                                 ' <img src="' + saveData.FileName + '" class="imgp"  />' +
                               '<div class="down" ></div>' +
                               '<input class="remove" style="display: none;" style="display: none;" value="删除"/>' +
                               '</div>';
                        $(".list").append(html);
                        bindOpEvent($(".list").find(".element")[$(".list").find(".element").length - 1]);
                    } else if (saveData.FileName!="") {
                        $(".list").find(".element[DataId='" + saveData.Id + "']").find("img").attr("src",saveData.FileName+"?v="+new Date());
                    }
       
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


    //上次图片
    $("*[name='cuttypebtn']").click(function () {
        $("*[name='cuttypebtn']").attr("class", "btn");
        $(this).attr("class", "btn sel");
    });
    var cutor = new Winner.ImageCutor("divCutContainer", { IsSynchSaveImage: false });
    cutor.MaxHeight = 550;
    cutor.MaxWidth = 1200;
    cutor.Initialize();
    cutor.SaveImage = function (data, func) {
        if (!window.checkSaveTag("divCutContainer")) {
            return false;
        }
        $.ajax({
            type: "Post",
            url: "/Commodity/UpdateImage",
            data: { fileValue: data.split(',')[1], fileName: cutor.File[0].files[0].name },
            async: false,
            dataType: "json",
            success: function (data) {
                $("#divSaveCut").removeAttr("IsClick");
                if (data.Status) {
                    var id = "#imgFileName";
                    $(id).attr("src", data.Message + "?v=" + new Date());
                    $(id).attr("realsrc", data.Message);
                    $("#divCutContainer").hide();
                    $(".cutbottom").hide();
                } else {
                    alert(data.Message);
                }
                func();
                window.removeSaveTag("divCutContainer");
            },
            error: function () {
                $("#divSaveCut").removeAttr("IsClick");
                $(".cutbottom").hide();
                $("#divCutContainer").hide();
                alert("系统忙，请稍候再试");
                func();
                window.removeSaveTag("divCutContainer");
            }
        });

    }

    $("#divCutContainer").find(".cutimg").css("height", $("#divCutContainer").height());
    ///设置保存按钮
    $('.addcontent').find("input[type='text']").bind('focus', function () {
        $("#btnSave").hide();

    }).bind('blur', function () {
        $("#btnSave").show();
    });
});




 