$(document).ready(function () {

    //tips
    $('.tips').poshytip();

    //tab切换
    $(".tab_ul li").each(function () {
        $(this).click(function() {
            var index = $(this).index();
            $(this).addClass("tabin").siblings().removeClass("tabin");
            $(".tab_ul li").eq(index - 1).addClass("noteline").siblings().removeClass("noteline");
        });
    });


    $(".tab_bar li").each(function () {
        $(this).click(function() {
            var dataId = $(this).attr("data-id");
            $(this).addClass("tabin").siblings().removeClass("tabin");
            $(".tab_ctn_item[data-id='" + dataId + "']").fadeIn().siblings().fadeOut(0);
        });
    });

    var showEdit = function(sender) {
        if (!window.checkSaveTag(sender)) {
            return false;
        }
        $("#hfId").val($(sender).attr("DataId"));
        $.ajax({
            type: "Post",
            url: "/Folder/Get",
            data: { id: $("#hfId").val() },
            async: true,
            cache: false,
            dataType: "json",
            success: function(data) {
                if (data != null) {
                    for (var con in data) {
                        var ctrl = $("#txt" + con);
                        if (ctrl.length == 0)
                            continue;
                        if (data[con] == null || data[con] == "") {
                            ctrl.val(ctrl.attr("ShowValue"));
                        } else {
                            ctrl.val(data[con]);
                        }
                    }
                    $("#ddlType").val(data.Type);
                    $(".win_popup").fadeIn();
                }
                window.removeSaveTag(sender);
            },
            error: function (xmlHttpRequest, textStatus, errorThrown) {
                alert("系统忙，请稍候再试");
                window.removeSaveTag(sender);
            }
        });
    };

   

    $("#btnSave").click(function () {
        if (!window.Validator.ValidateSubmit()) {
            alert("您输入的信息有误，请检测后重新输入");
            return false;
        }
        if (!window.checkSaveTag("btnSave")) {
            return false;
        }
        function getSaveData() {
            var result = {};
            $("#divContent").find("input[ValidateName]").each(function () {
                if ($(this).attr("ValidateName") != undefined) {
                    var value = $(this).val() == $(this).attr("ShowValue") ? " " : $(this).val();
                    eval("result." + $(this).attr("ValidateName").replace(".","") + "=value;");
                }
            });
            result.Type = $("#ddlType").val();
            return result;
        }
        var saveData = getSaveData();
        var url = "/Folder/Add";
        if ($("#hfId").val() != "") {
            url = "/Folder/Modify";
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
                    window.location.reload();
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
    ///得到编号
    function getIds() {
        var ids = [];
        $("#tbList").find("input[type=checkbox]").each(function (index, sender) {
            if (this.value == "on" || this.value == "" || !this.checked)
                return;
            ids.push(this.value);
        });
        return ids;
    }
    window.remove = function () {
        if (!window.checkSaveTag("tbList")) {
            return false;
        }
        var ids = getIds();
        $.ajax({
            type: "Post",
            url: "/Folder/Remove",
            data: { id: ids },
            async: true,
            dataType: "json",
            traditional: true,
            success: function (data) {
                if (data != null && data.Status == true) {
                    alert("删除成功");
                    window.location.reload();
                } else if (data != null) {
                    alert(data.Message);
                }
                window.removeSaveTag("tbList");
            },
            error: function () {
                alert("系统忙，请稍候再试");
                window.removeSaveTag("tbList");
            }
        });
    };

    $(".newAdd").click(function () {
        $("#hfId").val("");
    });
    $(document).find("a[name=edit]").click(function() {
        showEdit(this);
    });
  
    $(document).find("a[name=remove]").click(function () {
        remove(this);
    });
    $(".tab_ul").find("li").click(function() {
        $(".tabCentent").hide();
        $("#tabCentent" + $(this).attr("tabcontentid")).show();
    });
   
    var checkbox = new Winner.CheckBox('tbList', { StyleFile: null });
    checkbox.Initialize();
    var confirmBox = new Winner.ConfirmBox();
    confirmBox.Initialize();
});




 