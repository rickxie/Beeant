$(document).ready(function () {

    //设置标签
    function getSelectTag(name) {
        var sender = null;
        $(".addtag").find("*[tag='true']").each(function () {
            if ($(this).attr("tagname") == name) {
                sender = this;
                return false;
            }
        });
        return sender;
    }
    function setSelectTag(name) {
        var sender = getSelectTag(name);
        if (sender != null) {
            $("*[name='showtag']").append("<span class='sel' tagid='" + $(sender).attr("tagid") + "' tagname='"+name+"'>" + name + "</span>");
            $(sender).attr("class", "sel tg");
            return true;
        }
        return false;
    }

    function showCanceAddtag() {
        $(".addtag").show();
        $(".addcontent").hide();
        $(".addtag").find("input[type='text']")[0].focus();
        $("*[name='existtags']").find("span").each(function () {
            setSelectTag($(this).attr("tagname"));

        });
    }
 
  
    function checkSelectTag(name) {
        var isExist = false;
        $("*[name='showtag']").find("span").each(function () {
            if ($(this).attr("tagname") == name) {
                isExist = true;
                return false;
            }
        });
        return isExist;
    }
    function cancelTag(sender) {
        var name = $(sender).attr("tagname");
        $("*[name='showtag']").find("span").each(function() {
            if ($(this).attr("tagname")== name) {
                $(this).remove();
                return false;
            }
        });
        $(sender).attr("class","tg");
    }

    function clickTag(sender) {
        var name = $(sender).attr("tagname");
        if (checkSelectTag(name))
            cancelTag(sender);
        else
            setSelectTag(name);
    }

    function hideCanceAddtag() {
        $(".addcontent").show();
        $(".addtag").hide();
        $("*[name='showtag']").html("");
        $(".addtag").find("*[tag='true']").attr("class", "tg");
    }



    $("#hfAddTag").click(function () {
        showCanceAddtag();
    });

    $("#btnCanceAddTag").click(function () {
        hideCanceAddtag();
    });


    $(".addtag").find("input[type='text']").bind("blur", function () {
        var name = $(this).val();
        if (name == "" || checkSelectTag(name) || setSelectTag(name)) {
            $(this).val("");
            return;
        }
        $("*[name='showtag']").append("<span class='sel' tagid='' tagname='" + name + "'>" + name + "</span>");
        var html = "<div class='sel tg' tag='true' tagid='' tagname='" + name + "'>" + name + "</div>";
        $("div[class='alltag']").append(html);
        var divs = $("div[class='alltag']").find("div[tag='true']");
        $(divs[divs.length-1]).click(function () {
            clickTag(this);
        });
        $(this).val("");
    });



    function saveTags() {
        var names = new Array();
        $("*[name='showtag']").find("span").each(function () {
            if ($(this).attr("tagid") == "") {
                var name = $(this).attr("tagname");
                names.push(name);
            }
        });
        if (names.length > 0) {
            if (!window.checkSaveTag("btnSaveTag")) {
                return false;
            }
            $.ajax({
                type: "Post",
                url: "/Tag/Adds",
                data: {
                    names: names
                },
                traditional: true,
                async: true,
                dataType: "json",
                success: function (data) {
                    if (data.Status) {
                        $("*[name='showtag']").find("span").each(function() {
                            for (var i = 0; i < names.length; i++) {
                                if (names[i] == $(this).attr("tagname")) {
                                    $(this).attr("tagid", data.Ids[i]);
                                    var sender = getSelectTag(name);
                                    if (sender != null)
                                        $(sender).attr("tagid", data.Id);
                                    break;
                                }
                            }
                        });
                        $("*[name='existtags']").html("");
                        $("*[name='showtag']").find("span").each(function () {
                            if ($(this).attr("tagid") != "") {
                                window.addTagExist($(this).attr("tagid"), $(this).attr("tagname"));
                            }
                        });
                    }
                    hideCanceAddtag();
                    window.removeSaveTag("btnSaveTag");
                },
                error: function () {
                    window.removeSaveTag("btnSaveTag");
                    alert("系统忙，请稍候再试");

                }
            });
        } else {
            $("*[name='existtags']").html("");
            $("*[name='showtag']").find("span").each(function () {
                if ($(this).attr("tagid") != "") {
                    window.addTagExist($(this).attr("tagid"), $(this).attr("tagname"));
                }
            });
            hideCanceAddtag();
        }
    }
    $("#btnSaveTag").bind("click", function () {
 
        saveTags();
       


    });
    window.addTagExist = function (id, name) {
        $("*[name='existtags']").append("<span tagid='" + id + "' tagname='" + name + "'>" + name + "</span>");
    }

    var config = [
   {
       Triggers: [
           {
               Sender: "",
               Event: ""
           }
       ],
       Loading: { Content: "Content", Type: "Append" },
       Url: "/Tag/All/" + window.SiteId,
       Paramters: { },
       Content: $(".alltag")[0],
       ShowType: "Replace",
       DataType: "json",
       RequestType: "OneTime",
       IsExecute: true,
       IsLoadHideContent: false,
       BeginLoadFunction: function () {

       },
       BeginShowFunction: function (sender, info, data) {
          
       },
       EndShowFunction: function (sender, info, data) {
           $("div[class='alltag']").find("div[tag='true']").click(function() {
               clickTag(this);
           });
       }
   }
    ];
    var dataloader = new Winner.DataLoader(config);
    dataloader.Initialize();
    ///设置保存按钮
    $('.addtag').find("input").bind('focus', function () {
        $("#btnSaveTag").hide();

    }).bind('blur', function () {
        $("#btnSaveTag").show();
    });
});