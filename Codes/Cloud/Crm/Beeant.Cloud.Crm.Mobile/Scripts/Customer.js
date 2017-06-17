$(document).ready(function () {

    //操作
    var isAllowSelectItem = false;
    var selectItemFunc = function (sender) {
        $(sender).find(".select").show();
        $(sender).find(".selectok").show();
        $(sender).attr("IsSelect", "true");
    }
    var cancelItemFunc = function (sender) {
        $(sender).find(".select").hide();
        $(sender).find(".selectok").hide();
        $(sender).removeAttr("IsSelect");
    }
    $("#btnSelectItemButton").click(function () {
        isAllowSelectItem = !isAllowSelectItem;
        if (isAllowSelectItem) {
            $(this).html("取消");
            $("#btnAdd").hide();
            $(".edit").show();
        } else {
            $(this).html("编辑");
            $("#divSelectAll").removeAttr("IsSelect");
            $("#btnAdd").show();
            $(".edit").hide();
            $(".list").find(".element").each(function (index, sender) {
                cancelItemFunc(this);
            });
        }
    });
    $("#divSelectAll").click(function () {

        $(".list").find(".element").each(function (index, sender) {
            if ($("#divSelectAll").attr("IsSelect") == "true") {
                cancelItemFunc(this);
            } else {
                selectItemFunc(this);
            }

        });
        if ($("#divSelectAll").attr("IsSelect") == "true") {
            $("#divSelectAll").removeAttr("IsSelect");
        } else {
            $("#divSelectAll").attr("IsSelect", "true");
        }
    });
    $("#divMoveStaff").click(function () {
        $(".movestaff").show();
        $(".customer").hide();
        $(".addbtnc").hide();
    });
    $("#btnCancelMoveStaff").click(function () {
        $(".movestaff").hide();
        $(".customer").show();
        $(".addbtnc").show();
    });

    function getSelectItemId() {
        var id = [];
        $(".list").find(".element").each(function (index, sender) {
            if ($(this).attr("IsSelect") == "true") {
                id.push($(this).attr("DataId"));
            }
        });
        return id;
    }

    function bindStaffEvent(sender) {
        $(sender).click(function () {//更改类目
            if (!window.checkSaveTag(this)) {
                return;
            }
            var self = this;
            var staffId = $(this).attr("DataId");
            if (staffId == $("#selSearchCatalog").val())
                return;
            var idarr = getSelectItemId();
            $.ajax({
                type: "Post",
                url: "/Customer/UpdataStaff",
                data: { id: idarr, staffId: staffId },
                async: true,
                dataType: "json",
                traditional: true,
                success: function (data) {
                    if (data.Status) {
                        $(".movestaff").hide();
                        $(".customer").show();
                    } else {
                        alert(data.Message);
                    }
                    $(idarr).each(function() {
                        $(".list").find(".element[DataId=" + this + "]").attr("StaffId", staffId);
                    });
                    window.removeSaveTag(self);
                },
                error: function () {
                    alert("系统忙，请稍候再试");
                    window.removeSaveTag(self);
                }
            });

        });
    }
   
 
    //定位


    $(".cata").css("width", $(window).width() - $(".gohome").width() - $(".searchico").width() - $(".selectitem").width() - 15 + "px");
    $('#selSearchCustomerType,#selSearchCustomerChannel').bind('focus', function () {
        $('.mainten').css({ 'position': "absolute", 'top': "0" });
    }).bind('blur', function () {
        $('.mainten').css({ 'position': 'fixed', 'top': '0' });
    });


    //定位搜索
    var isjustmainten = false;
    function justmainten() {
        if (!isjustmainten)
            return;
        $(".mainten").css({ 'position': "absolute" })
       .css("top", $(document).scrollTop() + "px");
        setTimeout(justmainten, 10);
    }
    $('.searchinput').bind('focus', function () {
        isjustmainten = true;
        justmainten();

    }).bind('blur', function () {
        isjustmainten = false;
        $('.mainten').css({ 'position': 'fixed', 'top': '0' });
    });
    //定位保存
    var isjustsavecontent = false;
    function justsavecontent() {
        if (!isjustsavecontent)
            return;
        $(".ope").css({ 'position': "absolute" })
       .css("top", $(document).scrollTop() + "px");
        setTimeout(justsavecontent, 10);
    }
    $('.addcontent').find("input,select").bind('focus', function () {
        isjustsavecontent = true;
        justsavecontent();

    }).bind('blur', function () {
        isjustsavecontent = false;
        $('.ope').css({ 'position': 'fixed', 'top': '0' });
    });
    //搜索
    $(".searchico").click(function () {
        $(".mcp").hide();
        $(".search").show();
        $(".searchinput")[0].focus();
    });
    function cancelSearch() {
        $(".mcp").show();
        $(".search").hide();
        $(".searchinput").val("");
        $(".searchinput").attr("BeforValue", "");
    }
    $("#hfCancelSearch").click(function () {
        cancelSearch();
    });



    $("#btnAdd").css("left", ($(window).width() - $("#btnAdd").width()) / 2);

    function showSaveContent() {
        $(".addcontent").css("height", $(window).height() + "px");
        $(".addcontent").show();
        $(this).hide();
        $(".customer").hide();
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
        $(".customer").show();
    }
    $("#btnAdd").click(function () {
        showSaveContent();
    });
    $("#btnCancel").click(function () {
        hideSaveContent();
    });
    function removeCustomer(sender) {//删除
        if (!confirm("您确定要删除吗"))
            return ;
        if (!window.checkSaveTag(sender)) {
            return;
        }
        var id = $(sender).attr("DataId");
        $.ajax({
            type: "Post",
            url: "/Customer/Remove",
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

    function showDetail(sender) {
        if (!window.checkSaveTag(sender)) {
            return;
        }
        $("#hfId").val($(sender).attr("DataId"));
        $.ajax({
            type: "Post",
            url: "/Customer/Get",
            data: { id: $("#hfId").val() },
            async: true,
            dataType: "json",
            success: function (data) {
                if (data != null) {
                    var ctrl = $(sender).find(".more");
                    for (var con in data) {
                        if (con == "Name" || con == "Mobile" || con == "Id")
                            continue;
                        var value = "";
                        var name = "";
                        switch (con) {
                        case "TypeId":
                            name = "客户类型";
                            $("#selCustomerType").find("option").each(function() {
                                if ($(this).val() == data[con]) {
                                    value = $(this).text();
                                    return false;
                                }
                            });
                            break;
                        case "ChannelId":
                            name = "渠道来源";
                            $("#selCustomerChannel").find("option").each(function() {
                                if ($(this).val() == data[con]) {
                                    value = $(this).text();
                                    return false;
                                }
                            });
                            break;
                        default:
                            switch (con) {
                            case "Gender":
                                name = "性别";
                                break;
                            case "RemindNoteDate":
                                name = "下次跟踪时间";
                                break;
                            default:
                                name = $("#txt" + con).attr("ShowValue");
                                break;
                            }
                            value = data[con];
                            break;
                        }
                        if (name == "" || name == undefined || value == "" || value == undefined)
                            continue;
                        var html = '<div class="el">' + name + ":" + value + "</div>";
                        ctrl.append(html);
                    }
                    $(sender).find("div[name=loadmore]").hide();
                    ctrl.show();
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
        if (window.StaffId == "" || $(sender).attr("StaffId") != window.StaffId)
            return;
        if (!window.checkSaveTag(sender)) {
            return;
        }
        $("#hfId").val($(sender).attr("DataId"));
        $.ajax({
            type: "Post",
            url: "/Customer/Get",
            data: { id: $("#hfId").val() },
            async: true,
            dataType: "json",
            success: function (data) {
                if (data != null) {
                    for (var con in data) {
                        var ctrl =  $("#txt" + con);
                        if (ctrl.length == 0)
                            continue;
                        if (data[con]==null || data[con] == "") {
                            ctrl.attr("class", "input ctrlshow");
                            ctrl.val($("#txt" + con).attr("ShowValue"));
                        } else {
                            ctrl.attr("class", "input");
                            ctrl.val(data[con]);
                        }
                    }
                    $("#selCustomerType").val(data.TypeId);
                    $("#selCustomerChannel").val(data.ChannelId);
                    $('input:radio[name=gender]').each(function () {
                        this.checked = this.value == data.Gender;
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
                $(sender).find("input[class='remove']").show(100);
            } else {
                $(sender).find("input[class='remove']").hide();
            }
        }
        $(sender).find(".remove").bind("click", function () {
            removeCustomer(sender);
        });
        $(sender).find("*[name=loadmore]").bind("click", function () {
            showDetail(sender);
        });
        $(sender).bind("swipeleft", function () {
            setOpButton(this, true);
        }).bind("swiperight", function () {
            setOpButton(this, false);
        }).bind("click",function() {
            if (!isAllowSelectItem)
                return;
            if ($(this).attr("IsSelect") == "true") {
                cancelItemFunc(this);
            } else {
                selectItemFunc(this);
            }
        }).find(".name").bind("click", function () {
            if (isAllowSelectItem) {
                
            } else {
                showEdit(sender);
            }
        });
    }


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
            var reuslt = {};
            $("#divContent").find("input[type=text]").each(function() {
                if ($(this).attr("ValidateName") != undefined) {
                    var value = $(this).val() == $(this).attr("ShowValue") ? " " : $(this).val();
                    eval("reuslt." + $(this).attr("ValidateName") + "=value;");
                }
            });
            reuslt.TypeId = $("#selCustomerType").val();
            reuslt.ChannelId = $("#selCustomerChannel").val();
            reuslt.Gender = $('input:radio[name=gender]:checked').val();
            return reuslt;
        }
        var saveData = getSaveData();
        var url = "/Customer/Add";
        if ($("#hfId").val() != "") {
            url = "/Customer/Modify";
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
                        var html = '<div class="element"  DataId="' + saveData.Id + '" StaffId="' + window.StaffId + '">' +
                            '<div class="name">' + saveData.Name + '</div>' +
                            //'<div class="mobile"><a href="tel:' + saveData.Mobile + '">' + saveData.Mobile + '</a> </div>' +
                            '<input class="remove" type="button" style="display: none;" value="删除" />' +
                            '</div>';
                        $(".list").append(html);
                        bindOpEvent($(".list").find(".element")[$(".list").find(".element").length - 1]);
                    } else {
                        $(".list").find(".element[DataId='" + saveData.Id + "']").find(".name").html(saveData.Name);
                        $(".list").find(".element[DataId='" + saveData.Id + "']").find(".mobile").html('<a href="' + saveData.Mobile + '">' + saveData.Mobile+"</a>");
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
    //异步加载
    var config = [
       {
           Triggers: [
              {
                  Sender: $("#selSearchCustomerType")[0],
                  Event: "change",
                  Function: function (info) {
                      $(".list").html("");
                      info.Paramters.page = 0;
                      info.IsFullLoadComplate = false;
                      info.Paramters.key = "";
                      info.Paramters.typeId = $("#selSearchCustomerType").val();
                      info.Paramters.channelId = $("#selSearchCustomerChannel").val();
                      return true;
                  }
              },
                  {
                      Sender: $("#selSearchCustomerChannel")[0],
                      Event: "change",
                      Function: function (info) {
                          $(".list").html("");
                          info.Paramters.page = 0;
                          info.IsFullLoadComplate = false;
                          info.Paramters.key = "";
                          info.Paramters.typeId = $("#selSearchCustomerType").val();
                          info.Paramters.channelId = $("#selSearchCustomerChannel").val();
                          return true;
                      }
                  },
               {
                   Sender: $(".searchinput")[0],
                   Event: "blur",
                   Function: function (info) {
                       var rev = $(".searchinput").attr("BeforValue") != $(".searchinput").val();
                       if (rev) {
                           $(".list").html("");
                           info.Paramters.typeId = "";
                           info.Paramters.channelId = "";
                           info.Paramters.page = 0;
                           info.IsFullLoadComplate = false;
                           info.Paramters.key = $(".searchinput").val();
                           $(".searchinput").attr("BeforValue", $(".searchinput").val());
                       } else if ($(".searchinput").val() == "") {
                           cancelSearch();
                       }
                       return rev;
                   }
               },
                {
                    Sender: $("#hfCancelSearch")[0],
                    Event: "click",
                    Function: function (info) {
                        $(".list").html("");
                        info.Paramters.typeId = "";
                        info.Paramters.channelId = "";
                        info.Paramters.page = 0;
                        info.IsFullLoadComplate = false;
                        info.Paramters.key = "";
                        $(".searchinput").attr("BeforValue", "");
                        return true;
                    }
                },
               {
                   Sender: window,
                   Event: "scroll",
                   Function: function (info) {
                     
                       return $(".customer")[0].style.display != "none" && $(document).scrollTop() >= $(document).height() - $(window).height();
                   }
               }
           ],
           Loading: { Content: "Content", Type: "Append" },
           Url: "/Customer/List",
           Paramters: { page: 0 },
           Content: $(".list")[0],
           ShowType: "Append",
           DataType: "text",
           RequestType: "Repeat",
           IsExecute: true,
           IsLoadHideContent: false,
           BeginLoadFunction: function (sender,info) {
               info.Paramters.isReadSelf = window.IsReadSelf;
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
       },
       {
           Triggers: [
              {
                  Sender: $("#divMoveStaff")[0],
                  Event: "click",
                  Function: function (info) {
                      var rev = $("#divMoveStaff").attr("IsLoad") != "true";
                      $("#divMoveStaff").attr("IsLoad", "true");
                     
                      return rev;
                  }
              },
               {
                   Sender: window,
                   Event: "scroll",
                   Function: function (info) {

                       return $(".movestaff")[0].style.display!="none" && $(document).scrollTop() >= $(document).height() - $(window).height();
                   }
               }
           ],
           Loading: { Content: "Content", Type: "Append" },
           Url: "/Customer/Staffs",
           Paramters: { page: 0 },
           Content: $(".movestaff").find("ul")[0],
           ShowType: "Append",
           DataType: "text",
           RequestType: "Repeat",
           IsExecute: false,
           IsLoadHideContent: false,
           BeginLoadFunction: function (sender, info) {
           },
           BeginShowFunction: function (sender, info, data) {
               info.Paramters.page = info.Paramters.page + 1;

           },
           EndShowFunction: function (sender, info, data) {
               window.lazyloadImage($(info.Content)[0]);
               var length = $("<div>" + data + "</div>").find(".li").length;
               if (length < 24)
                   info.IsFullLoadComplate = true;
               var elemnts = $(info.Content).find(".li");
               for (var i = elemnts.length - length; i < elemnts.length; i++) {
                   bindStaffEvent(elemnts[i]);
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
