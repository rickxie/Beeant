$(document).ready(function () {
    $(".cata").css("width", $(window).width() - $(".gohome").width() - $(".searchico").width() - $(".selectitem").width() - 15 + "px");
    $('#selSearchCatalog').bind('focus', function () {
        $('.mainten').css({ 'position': "absolute",'top': "0" });
    }).bind('blur', function () {
        $('.mainten').css({ 'position': 'fixed', 'top': '0' });
    });

    function hideFeild() {
        if (!window.IsPassword) {
            $("#txtPassword").hide();
            $("#txtName").css("width","94%");
        }
    }
    hideFeild();
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
    //保存数据
    function showSaveContent() {
        $(".addcontent").css("height", $(".addcontent").height()+ "px");
        $(".addcontent").show();
        $(".addbtnc").hide();
        $(".commodity").hide();
        $(window).scrollTop(0);
        $(".mainten").hide();
    }

    function hideSaveContent() {
        $(".addcontent").hide();
        $(".addbtnc").show();
        $(".mainten").show();
        $(".addcontent").find(".content").find("input[type='text']").each(function () {
            $(this).val($(this).attr("ShowValue"));
            $(this).attr("class", "input ctrlshow");
        });
        $(".addcontent").find(".content").find("textarea").each(function () {
            $(this).val($(this).attr("ShowValue"));
            $(this).attr("class", "input ctrlshow");
        });
        $(".addcontent").find("input[name='status']")[0].checked = true;
        $("#hfId").val("");
        $(".commodity").show();
        $("#imgFileName").attr("src", $("#imgFileName").attr("OriginalSrc"));
        $(document).find("img[ImageCutor='ShowImager']").each(function () {
           $(this).attr("DataId", "");
           $(this).attr("src", $(this).attr("OriginalSrc") + "?v=" + new Date());
           $(this).removeAttr("realsrc");
           $(this).parent().find(".cl").hide();
       });
        $("*[name='existtags']").html("");
    }
    $("#btnAdd").css("left", ($(window).width() - $("#btnAdd").width()) / 2);
    $("#btnAdd").click(function () {

        showSaveContent();
    });
    $("#btnCancel").click(function () {
        hideSaveContent();
    });
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
            var status = "1";
            $("input[name='status']").each(function () {
                if (this.checked) {
                    status = this.value;
                    return false;
                }
            });
            var images = [];
            $("#divImages").find("img[ImageCutor='ShowImager']").each(function(index, sender) {
                images.push({
                    Id: $(this).attr("DataId") == undefined ? "" : $(this).attr("DataId"),
                    FileName: $(this).attr("realsrc") == undefined ? "" : $(this).attr("realsrc")
                });
            });
            var tags = [];
            $("*[name='existtags']").find("span").each(function () {
                if ($(this).attr("tagid") != undefined && $(this).attr("tagid") != "") {
                    tags.push({ Id: $(this).attr("tagid") });
                }
                

            });
            var base = new Winner.ClassBase();
            return {
                Name: $("#txtName").val() == $("#txtName").attr("ShowValue") ? " " : $("#txtName").val(),
                CatalogId: $("#selCatalog").val(),
                Cost: parseFloat($("#txtCost").val()),
                Price: parseFloat($("#txtPrice").val()),
                Status: parseInt(status),
                VenderName: $("#txtVenderName").val() == $("#txtVenderName").attr("ShowValue") ? " " : $("#txtVenderName").val(),
                VenderLinkman: $("#txtVenderLinkman").val() == $("#txtVenderLinkman").attr("ShowValue") ? " " : $("#txtVenderLinkman").val(),
                VenderMobile: $("#txtVenderMobile").val() == $("#txtVenderMobile").attr("ShowValue") ? " " : $("#txtVenderMobile").val(),
                VenderAddress: $("#txtVenderAddress").val() == $("#txtVenderAddress").attr("ShowValue") ? " " : $("#txtVenderAddress").val(),
                FileName: $("#imgFileName").attr("realsrc") == undefined ? "" : $("#imgFileName").attr("realsrc"),
                IsShowPrice: $("#ckIsShowPrice")[0].checked,
                Password: $("#txtPassword").val() == $("#txtPassword").attr("ShowValue") ? " " : $("#txtPassword").val(),
                Description: $("#txtDescription").val() == $("#txtDescription").attr("ShowValue") ? " " : $("#txtDescription").val(),
                ImagesValue: base.Serialize(images),
                TagsValue: base.Serialize(tags),
                AlbumFileName: $("#imgAlbumFileName").attr("realsrc") == undefined ? "" : $("#imgAlbumFileName").attr("realsrc"),
                IsCreateAlbum: $("#ckIsCreateAlbum").length > 0 ? $("#ckIsCreateAlbum")[0].checked : false
        };
        }
        var saveData = getSaveData();
        var url = "/Commodity/Add";
        if ($("#hfId").val() != "") {
            url = "/Commodity/Modify";
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
                    if ($("#hfId").val() != "") {
                        var element = null;
                        $(".list").find(".element").each(function(index, sender) {
                            if ($(this).attr("DataId") == $("#hfId").val()) {
                                element = this;
                                return;
                            }
                        });
                        if ($("#selSearchCatalog").val() != "" && $("#selCatalog").val() != $("#selSearchCatalog").val()) {
                            $(element).remove();
                        } else {
                            $(element).find("img").attr("src", $("#imgFileName").attr("src"));
                            $(element).find(".name").html(saveData.Name);
                        }
                        hideSaveContent();
                    } else {
                        var html = '<div class="element" DataId="' + data.Id + '" Sequence="' + data.Sequence + '">' +
                            '<div class="content">' +
                            '<img src="' + $("#imgFileName").attr("src") + '" class="img" alt="" />' +
                            '<div class="name">' + saveData.Name + '</div>' +
                            '</div>' +
                            '<div class="select" style="display: none;">' +
                            '</div>' +
                            ' <div class="selectok" style="display: none;"></div>' +
                            ' </div>';
                        $(".list").prepend(html);
                        hideSaveContent();
                        bindElementEvent($(".list").find(".element")[0]);
                    }
                 
                } else {
                    alert(data.Message);
                }
                window.removeSaveTag("btnSave");
            },
            error: function () {
                $(".cutbottom").hide();
                $("#divCutContainer").hide();
                alert("系统忙，请稍候再试");
                window.removeSaveTag("btnSave");
            }
        });

        return false;
    });

    //上次图片
    var imgs = $("#divImages").find("div[class='div']");
    imgs.css("margin-left", parseInt(($(window).width() - imgs.width() * imgs.length) / (imgs.length + 1)) + "px");
    $("img[ImageCutor='ShowImager']").click(function() {
        $("#divSaveCut").attr("ImageId", $(this).attr("Id"));
    });
    $("#divImages").find(".cl").click(function() {
        var img = $(this).parent().find("img[ImageCutor='ShowImager']");
        img.removeAttr("realsrc").removeAttr("DataId")
            .removeAttr("src", img.attr("OriginalSrc") + "?v=" + new Date());
        $(this).hide();
    });
    $("*[name='cuttypebtn']").click(function() {
        $("*[name='cuttypebtn']").attr("class", "btn");
        $(this).attr("class", "btn sel");
    });
    $("*[name='cuttypebtn']").css("margin-left", ($(".cutbottom").width() - $(".cutbottom").find(".btn").width() * $(".cutbottom").find(".btn").length) / ($("*[name='cuttypebtn']").length + 1));
    var cutor = new Winner.ImageCutor("divCutContainer", { IsSynchSaveImage: false });
    cutor.Initialize();
    cutor.SaveImage = function (data,func) {
        if (!window.checkSaveTag("divCutContainer")) {
            return false;
        }
        $.ajax({
            type: "Post",
            url: "/Commodity/UpdateImage",
            data: { fileValue: data.split(',')[1], fileName: cutor.File[0].files[0].name },
            async: true,
            dataType: "json",
            success: function (data) {
                $("#divSaveCut").removeAttr("IsClick");
                if (data.Status) {
                    var id = "#" + $("#divSaveCut").attr("ImageId");
                    $(id).attr("src", data.Message + "?v=" + new Date());
                    $(id).attr("realsrc", data.Message);
                    $("#divCutContainer").hide();
                    $(".cutbottom").hide();
                    if (id != "#imgFileName") {
                        $(id).parent().find(".cl").show();
                    }
                 
                } else {
                    alert(data.Message);
                }
                func();
                window.removeSaveTag("divCutContainer");
            },
            error: function () {
                $("#divSaveCut").removeAttr("IsClick");
                alert("系统忙，请稍候再试");
                func();
                window.removeSaveTag("divCutContainer");
            }
        });

    }
 
    $("#divCutContainer").find(".cutimg").css("height", $("#divCutContainer").height());

    //异步加载
    $("#selSearchCatalog").change(function() {
        $(".list").html("");
    });
    var config = [
       {
           Triggers: [
                {
                    Sender: $("#selSearchCatalog")[0],
                    Event: "change",
                    Function: function (info) {
                        $(".list").html("");
                        info.Paramters.page = 0;
                        info.IsFullLoadComplate = false;
                        info.Paramters.key = "";
                        info.Paramters.catalogId = $("#selSearchCatalog").val();
                        info.Paramters.tagId = $("#selSearchTag").val();
                        return true;
                    }
                },
                  {
                      Sender: $("#selSearchTag")[0],
                      Event: "change",
                      Function: function (info) {
                          $(".list").html("");
                          info.Paramters.page = 0;
                          info.IsFullLoadComplate = false;
                          info.Paramters.key = "";
                          info.Paramters.catalogId = $("#selSearchCatalog").val();
                          info.Paramters.tagId = $("#selSearchTag").val();
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
                           info.Paramters.catalogId = "";
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
                        info.Paramters.catalogId = "";
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
                   Function: function () {
                       return $(document).scrollTop() >= $(document).height() - $(window).height();
                   }
               }
           ],
           Loading: { Content: "Content", Type: "Append" },
           Url: "/Commodity/List",
           Paramters: { page: 0},
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
               if (length < 12)
                   info.IsFullLoadComplate = true;
               var elemnts = $(info.Content).find(".element");
               for (var i = elemnts.length - length; i < elemnts.length ; i++) {
                   $(elemnts[i]).attr("Index", i);
                   bindElementEvent(elemnts[i]);
               }
           }
       }
    ];
    var dataloader = new Winner.DataLoader(config);
    dataloader.Initialize();

    //选中删除移动
    function moveItem(sender) {
        if (sender.attr("IsDrag") == "true" || $(".list").find(".element").length<2)
            return;
        sender.attr("IsDrag", "true");
        var self = this;
        function drag() {
            var left = sender.position().left + self.MoveEndX - self.MoveStartX;
            var top = sender.position().top + self.MoveEndY - self.MoveStartY;
            if (top - $(document).scrollTop() < 20) {
                $(document).scrollTop($(document).scrollTop() - 5);
            } else if (top - $(document).scrollTop() > $(window).height() - 200) {
                $(document).scrollTop($(document).scrollTop() + 5);
            }
            sender.css("left", left + "px").css("top", top + "px");
        }

        function enddrag() {
            self.MoveStartX = undefined;
            self.MoveStartY = undefined;
            sender.unbind("touchmove").unbind("touchend");
            var afterInsertObj = null;
            var beforeInsertObj = null;
            $(".list").find(".element").each(function (index, obj) {
                if ($(obj).attr("dataid") == sender.attr("dataid"))
                    return true;
                var left = $(this).position().left;
                var top = $(this).position().top;
                var disTop = Math.abs(sender.position().top - top);
                var disLeft = Math.abs(sender.position().left - left);
                if (sender.position().left<=left && disLeft <= sender.width() && disTop <= sender.height() / 2) {
                    afterInsertObj = this;
                    if (index > 0) {
                        beforeInsertObj = $(".list").find(".element")[index - 1];
                        if (sender.attr("dataid") == $(beforeInsertObj).attr("dataid"))
                            beforeInsertObj = null;
                    }
                        
                    return false;
                }
                else if (sender.position().left >= left && disLeft <= sender.width() && disTop <= sender.height() / 2) {
                    beforeInsertObj = this;
                    if (index < $(".list").find(".element").length - 1) {
                        afterInsertObj = $(".list").find(".element")[index + 1];
                        if (sender.attr("dataid") == $(afterInsertObj).attr("dataid"))
                            afterInsertObj = null;
                    }
                       
                }
            });
            if (afterInsertObj == null && beforeInsertObj==null) {
                beforeInsertObj = $(".list").find(".element")[$(".list").find(".element").length - 1];
            }
            var sequence = 0;
            if (afterInsertObj != null && beforeInsertObj != null) {
                sequence = parseInt($(afterInsertObj).attr("Sequence"));
                sequence = sequence-(sequence - parseInt($(beforeInsertObj).attr("Sequence"))) / 2;
            }
            else if (afterInsertObj != null) {
                sequence = parseInt($(afterInsertObj).attr("Sequence"));
                sequence = sequence-2500;
            }
            else if (beforeInsertObj != null) {
                sequence = parseInt($(beforeInsertObj).attr("Sequence"));
                sequence = sequence + 2500;
            }
            var id = $(sender).attr("DataId");
            sequence = parseInt(sequence);
            if (!window.checkSaveTag(sender)) {
                return false;
            }
            $.ajax({
                type: "Post",
                url: "/Commodity/Modify",
                data: { Id: id, Sequence: sequence },
                async: true,
                dataType: "json",
                success: function (data) {
                    if (data.Status) {
                        if (afterInsertObj!=null)
                            sender.insertBefore(afterInsertObj);
                        else if (beforeInsertObj!=null)
                            sender.insertAfter(beforeInsertObj);
                        sender.css("position", "").css("z-index", "")
                            .css("left", "").css("top", "")
                            .removeAttr("IsDrag").attr("Sequence", sequence);
                        cancelItemFunc(sender[0]);
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
        var oleft = sender.position().left;
        var otop = sender.position().top ;
        sender.css("position", "absolute").css("z-index", "9999")
            .css("left", oleft).css("top", otop)
        .bind("touchmove", function(event) {
                self.MoveStartX = self.MoveStartX == undefined ? event.originalEvent.touches[0].pageX : self.MoveStartX;
                self.MoveStartY = self.MoveStartY == undefined ? event.originalEvent.touches[0].pageY : self.MoveStartY;
                self.MoveEndX = event.originalEvent.touches[0].pageX;
                self.MoveEndY = event.originalEvent.touches[0].pageY;
                drag();
                self.MoveStartX = self.MoveEndX;
                self.MoveStartY = self.MoveEndY;
                return false;
            }).bind("touchend", function (event) {
                enddrag();
                return false;
            });
        return;
    }

    var isAllowSelectItem = false;
    var selectItemFunc=function(sender) {
        $(sender).find(".select").show();
        $(sender).find(".selectok").show();
        $(sender).attr("IsSelect", "true");
    }
    var cancelItemFunc = function (sender) {
        $(sender).find(".select").hide();
        $(sender).find(".selectok").hide();
        $(sender).removeAttr("IsSelect");
    }
    var showEdit = function (sender) {
        if (!window.checkSaveTag(sender)) {
            return false;
        }
        $("#hfId").val($(sender).attr("DataId"));
        $.ajax({
            type: "Post",
            url: "/Commodity/Get",
            data: { id: $("#hfId").val() },
            async: true,
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
                    $("#txtCost").attr("class", "input");
                    $("#txtCost").val(data.Cost);
                    $("#txtPrice").attr("class", "input");
                    $("#txtPrice").val(data.Price);
                    if (data.VenderName == "") {
                        $("#txtVenderName").attr("class", "input ctrlshow");
                        $("#txtVenderName").val($("#txtVenderName").attr("ShowValue"));
                    } else {
                        $("#txtVenderName").attr("class", "input");
                        $("#txtVenderName").val(data.VenderName);
                    }
                    if (data.VenderLinkman == "") {
                        $("#txtVenderLinkman").attr("class", "input ctrlshow");
                        $("#txtVenderLinkman").val($("#txtVenderLinkman").attr("ShowValue"));
                    } else {
                        $("#txtVenderLinkman").attr("class", "input");
                        $("#txtVenderLinkman").val(data.VenderLinkman);
                    }
                    if (data.VenderMobile == "") {
                        $("#txtVenderMobile").attr("class", "input ctrlshow");
                        $("#txtVenderMobile").val($("#txtVenderMobile").attr("ShowValue"));
                    } else {
                        $("#txtVenderMobile").attr("class", "input");
                        $("#txtVenderMobile").val(data.VenderMobile);
                    }
                    if (data.VenderAddress == "") {
                        $("#txtVenderAddress").attr("class", "input ctrlshow");
                        $("#txtVenderAddress").val($("#txtVenderAddress").attr("ShowValue"));
                    } else {
                        $("#txtVenderAddress").attr("class", "input");
                        $("#txtVenderAddress").val(data.VenderAddress);
                    }
                    if (data.Description == "") {
                        $("#txtDescription").attr("class", "input ctrlshow");
                        $("#txtDescription").val($("#txtDescription").attr("ShowValue"));
                    } else {
                        $("#txtDescription").attr("class", "input");
                        $("#txtDescription").val(data.Description);
                    }
                    if (data.Password == "") {
                        $("#txtPassword").attr("class", "input ctrlshow");
                        $("#txtPassword").val($("#txtPassword").attr("ShowValue"));
                    } else {
                        $("#txtPassword").attr("class", "input");
                        $("#txtPassword").val(data.Password);
                    }
                    $("#ckIsShowPrice")[0].checked = data.IsShowPrice;
                    if ($("#ckIsCreateAlbum").length > 0) {
                        $("#ckIsCreateAlbum")[0].checked = data.IsCreateAlbum;
                    }
                    $("#selCatalog").val(data.CatalogId);
                    $("input[name='status']").each(function() {
                        this.checked = this.value == data.Status;
                    });
                    $("#imgFileName").attr("src", data.FileName);
                    $("#divImages").find("img[ImageCutor='ShowImager']").each(function(i, sender) {
                        if (data.Images != null && i < data.Images.length) {
                            $(this).attr("DataId", data.Images[i].Id);
                            $(this).attr("src", data.Images[i].FileName + "?v=" + new Date());
                            $(this).parent().find(".cl").show();
                        } else {
                            $(this).attr("DataId", "");
                            $(this).attr("src", $(this).attr("OriginalSrc") + "?v=" + new Date());
                            $(this).parent().find(".cl").hide();
                        }
                    });
                    $("*[name='existtags']").html("");
                    if (data.Tags != null) {
                        $(data.Tags).each(function(index, sender) {
                            window.addTagExist(this.Id, this.Name);
                        });
                    }
                    if (data.AlbumFileName != null && data.AlbumFileName != "") {
                        $("#imgAlbumFileName").attr("src", data.AlbumFileName + "?v=" + new Date());
                    } else {
                        $("#imgAlbumFileName").attr("src", $("#imgAlbumFileName").attr("OriginalSrc") + "?v=" + new Date());
                    }
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
    function bindElementEvent(sender) {
        $(sender).find("img").css("width", Math.ceil($(window).width()*0.31))
            .css("height", Math.ceil($(window).width() * 0.31));
        //$(sender).css("height", $(sender).find("img").height() + $(sender).find(".name").height());
        $(sender).bind("click", function () {
            if (!isAllowSelectItem)
                return;
            if ($(this).attr("IsSelect") == "true") {
                cancelItemFunc(this);
            } else {
                selectItemFunc(this);
            }
        }).bind("click", function() {
            if (!isAllowSelectItem)
                showEdit(this);
        });
        $(sender).find(".select").bind("taphold", function() {
            moveItem($(this).parent());
            return false;
        });
    }

  
    $("#btnSelectItemButton").click(function() {
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
            $(".list").find(".element").each(function(index, sender) {
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
    $("#divMoveCatalog").click(function () {
        $(".movecatalog").show();
        $(".commodity").hide();
        $(".addbtnc").hide();
    });
    $("#btnCancelMoveCatalog").click(function () {
        $(".movecatalog").hide();
        $(".commodity").show();
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

    function removeSelectItem() {
        $(".list").find(".element").each(function (index, sender) {
            if ($(this).attr("IsSelect") == "true") {
                $(this).remove();
            }
        });
    }
    $("#btnRemove").click(function () {//删除 
        if (!confirm("您确定要删除吗"))
            return ;
        if (!window.checkSaveTag(this)) {
            return;
        }
        var self = this;
        var idarr = getSelectItemId();
        $.ajax({
            type: "Post",
            url: "/Commodity/Remove",
            data: { id: idarr },
            async: true,
            dataType: "json",
            traditional: true,
            success: function (data) {
                if (data.Status) {
                    removeSelectItem();
                } else {
                    alert(data.Message);
                }
                window.removeSaveTag(self);
            },
            error: function () {
                alert("系统忙，请稍候再试");
                window.removeSaveTag(self);
            }
        });
    
    });
    $(".movecatalog").find("li").click(function () {//更改类目
        if (!window.checkSaveTag(this)) {
            return;
        }
        var self = this;
        var catalogId = $(this).attr("DataId");
        if (catalogId == $("#selSearchCatalog").val())
            return;
        var idarr = getSelectItemId();
        $.ajax({
            type: "Post",
            url: "/Commodity/UpdataCatalog",
            data: { id: idarr, catalogId: catalogId },
            async: true,
            dataType: "json",
            traditional: true,
            success: function (data) {
                if (data.Status) {
                    if ($("#selSearchCatalog").val() != "" && catalogId != $("#selSearchCatalog").val()) {
                        removeSelectItem();
                    }
                    $(".movecatalog").hide();
                    $(".commodity").show();
                } else {
                    alert(data.Message);
                }
                window.removeSaveTag(self);
            },
            error: function () {
                alert("系统忙，请稍候再试");
                window.removeSaveTag(self);
            }
        });
  
    });

    function updateStatus(value, mess) {
        if (!window.checkSaveTag(this)) {
            return;
        }
        var self = this;
        var idarr = getSelectItemId();
        $.ajax({
            type: "Post",
            url: "/Commodity/UpdataStatus",
            data: { id: idarr, status: value },
            async: true,
            dataType: "json",
            traditional: true,
            success: function (data) {
                if (data.Status) {
                    alert(mess);
                } else {
                    alert(data.Message);
                }
                window.removeSaveTag(self);
            },
            error: function () {
                alert("系统忙，请稍候再试");
                window.removeSaveTag(self);
            }
        });
       
    }
    $("#divNormal").click(function () {//更改类目
        if (!window.checkSaveTag(this)) {
            return;
        }
        var self = this;
        updateStatus(1, "上架成功");
        window.removeSaveTag(self);
    });
    $("#divUnSale").click(function () {//更改类目
        if (!window.checkSaveTag(this)) {
            return;
        }
        var self = this;
        updateStatus(2, "下架成功");
        window.removeSaveTag(self);
    });

    //新增类目
    $("#hfAddCatalog").click(function() {
        $(".addcatalog").show();
        $(".addcontent").hide();
        $(".addcatalog").find("input[type='text']")[0].focus();
    });
    $("#btnCanceAddcatalog").click(function () {
        $(".addcatalog").hide();
        $(".addcontent").show();
    });
    function hideCanceAddcatalog() {
        $(".mask").hide();
        $(".addcatalog").hide();
        $(".addcontent").show();
    }
    $("#btnSaveCatalog").bind("click", function () {
        if (!window.checkSaveTag(this)) {
            return;
        }
        var self = this;
        var name = $("#txtCatalog").val();
        $.ajax({
            type: "Post",
            url: "/Catalog/Add",
            data: {
                Name: name
            },
            async: true,
            dataType: "json",
            success: function (data) {
                if (data.Status) {
                    hideCanceAddcatalog();
                    $("#selCatalog").append('<option value="' + data.Id + '">' + name + '</option>');
                    $("#selCatalog").val(data.Id);
                } else {
                    alert(data.Message);
                }
                window.removeSaveTag(self);
            },
            error: function () {
                alert("系统忙，请稍候再试");
                window.removeSaveTag(self);
            }
        });
   
    });

    ///设置保存按钮
    $('.addcontent').find("input[type='text']").bind('focus', function () {
        //$("#btnSave").hide();

    }).bind('blur', function () {
        $("#btnSave").show();
    });
    $('.addcontent').find("textarea").bind('focus', function () {
        //$("#btnSave").hide();

    }).bind('blur', function () {
        $("#btnSave").show();
    });
    ///设置保存按钮
    $('.addcatalog').find("input[type='text']").bind('focus', function () {
        $("#btnSaveCatalog").hide();

    }).bind('blur', function () {
        $("#btnSaveCatalog").show();
    });
    

});