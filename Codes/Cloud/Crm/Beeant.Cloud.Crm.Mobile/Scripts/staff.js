$(document).ready(function () {
    $("#btnAdd").css("left", ($(window).width() - $("#btnAdd").width()) / 2);

    function showSaveContent() {
        $(".addcontent").css("height", $(window).height() + "px");
        $(".addcontent").show();
        $(this).hide();
        $(".staff").hide();
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
        $("#txtLoginPassword").val("");
        $(".staff").show();
    }
    $("#btnAdd").click(function () {
        showSaveContent();
    });
    $("#btnCancel").click(function () {
        hideSaveContent();
    });
    function removeStaff(sender) {//删除
        if (!confirm("您确定要删除吗"))
            return false;
        if (!window.checkSaveTag(sender)) {
            return false;
        }
        var id = $(sender).attr("DataId");
        $.ajax({
            type: "Post",
            url: "/Staff/Remove",
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
    function unbindStaff(sender) {//删除
        if (!confirm("您确定要取消绑定吗"))
            return false;
        if (!window.checkSaveTag(sender)) {
            return false;
        }
        var id = $(sender).attr("DataId");
        $.ajax({
            type: "Post",
            url: "/Staff/UnBind",
            data: {
                id: id
            },
            async: false,
            dataType: "json",
            success: function (data) {
                if (data.Status) {
                    $(sender).find("*[name='unbind']").hide();
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
            url: "/Staff/Get",
            data: { id: $("#hfId").val() },
            async: false,
            dataType: "json",
            success: function (data) {
                if (data != null) {
                    if (data.Name == "") {
                        $("#txtName").attr("class", "input ctrlshow");
                        $("#txtName").val($("#txtName").attr("ShowValue"));
                    } else {
                        $("#txtName").attr("class", "input");
                        $("#txtName").val(data.Name);
                    }
                    $("#selDepartment").val(data.DepartmentId );
                    $('input:radio[name=ReadCustomerType]').each(function() {
                        this.checked = this.value == data.ReadCustomerType;
                    });
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

        $(sender).find(".remove").bind("click", function () {
            removeStaff(sender);
        });
        $(sender).find("*[name='unbind']").bind("click", function () {
            unbindStaff(sender);
        });
        $(sender).bind("swipeleft", function () {
            setOpButton(this, true);
        }).bind("swiperight", function () {
            setOpButton(this, false);
        }).find(".name").bind("click", function () {
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
           Url: "/Staff/List/",
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
               var length = $("<div>" + data + "</div>").find(".element").length;
               if (length < 24)
                   info.IsFullLoadComplate = true;
               var elemnts = $(info.Content).find(".element");
               for (var i = elemnts.length - length; i < elemnts.length; i++) {
                   bindOpEvent(elemnts[i]);
               }
             
              
           }
       },
        {
            Triggers: [
                {
                    Sender: "",
                    Event: ""
                }
            ],
            Loading: { Content: "Content", Type: "Append" },
            Url: "/Staff/Department/",
            Paramters: { page: 0 },
            Content: $("#divDepartment")[0],
            ShowType: "Append",
            DataType: "text",
            RequestType: "Repeat",
            IsExecute: true,
            IsLoadHideContent: false,
            BeginLoadFunction: function () {

            },
            BeginShowFunction: function (sender, info, data) {
     

            },
            EndShowFunction: function (sender, info, data) {
              


            }
        }
    ];
    var dataloader = new Winner.DataLoader(config);
    dataloader.Initialize();

    $("#btnSave").click(function () {
        if (!window.Validator.ValidateSubmit()) {
            alert("您输入的信息有误，请检测后重新输入");
            return false;
        }
        if (!window.checkSaveTag("btnSave")) {
            return false;
        }
        function getSaveData() {
            return {
                Name: $("#txtName").val() == $("#txtName").attr("ShowValue") ? " " : $("#txtName").val(),
                DepartmentId: $("#selDepartment").val(),
                ReadCustomerType: $('input:radio[name=ReadCustomerType]:checked').val(),
                LoginName: $("#txtLoginName").val() == $("#txtLoginName").attr("ShowValue") ? " " : $("#txtLoginName").val(),
                LoginPassword: $("#txtLoginPassword").val() == $("#txtLoginPassword").attr("ShowValue") ? " " : $("#txtLoginPassword").val()
            };
        }
        var saveData = getSaveData();
        var url = "/Staff/Add";
        if ($("#hfId").val() != "") {
            url = "/Staff/Modify";
            saveData.Id = $("#hfId").val();
        }
        $.ajax({
            type: "Post",
            url: url,
            data: saveData ,
            async: false,
            dataType: "json",
            success: function (data) {
                if (data.Status) {
                    if ($("#hfId").val() == "") {
                        var html = ' <div class="element"' + '" DataId="' + data.Id + '">' +
                            '<div class="name">' + saveData.Name + '</div>' +
                            '<input class="edit" name="unbind" type="button" value="取消绑定" style="' + (data.AccountId == 0 ? "" : "display: none;") + '" />' +
                             '<input class="remove" type="button" style="display: none;" value="删除"/>' +
                             '</div>';
                        $(".list").append(html);
                        bindOpEvent($(".list").find(".element")[$(".list").find(".element").length - 1]);
                    } else {
                        $(".list").find(".element[DataId='" + saveData.Id + "']").find(".name").html(saveData.Name);
                        if (data.AccountId > 0) {
                            $(".list").find(".element[DataId='" + saveData.Id + "']").find("*[name='unbind']").show();
                        }
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

});
