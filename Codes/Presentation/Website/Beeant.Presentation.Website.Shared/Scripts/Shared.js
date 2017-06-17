function InitSearcher(quickUrl) {
    $(document).ready(function () {
        $("#txtSearch").click(function () {
            if (this.value == "搜索你想要的商品") {
                this.value = "";
                this.className = "input";
            }
        });
        $("#txtSearch").blur(function () {
            if (this.value == "") {
                this.value = "搜索你想要的商品";
                this.className = "input col";
            }
        });
        var searcher = new Winner.ComboBox("txtSearch", "", { StyleFile: "", IsCreateDown: false, IsCreateContainer: false, Container: document.getElementById("searcher") });
        searcher.Initialize();
        searcher.GetInfos = function () {
            var func = function () {
                if (searcher.InvokeFunc || $("#txtSearch").val() == "") return;
                try {
                    searcher.InvokeFunc = true;
                    $.ajax({
                        url: quickUrl + "?key=" + $("#txtSearch").val(),
                        type: "GET",
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            searcher.List.innerHTML = "";
                            searcher.AddItems(data);
                            searcher.InvokeFunc = false;
                        }
                    });
                } catch (e) {
                    searcher.InvokeFunc = false;
                }

            };
            func();
            return null;
        };
        searcher.SetItem = function (item, info) {
            item.innerHTML = "<div class='name'>" + info.Name + "</div><div class='datacount'>约" + info.DataCount + "</div>";
            $(item).attr("value", info.Name);
        };
        searcher.SelectText = function (item) {
            if (item == null) return;
            searcher.Input.value = $(item).attr("value");
        };
        searcher.Select = function () {
            search($("#txtSearch").val());
        };

        $("#btnSearch").click(function () {
            return search($("#txtSearch").val());
        });
        $("#hoter").find("a").click(function () {
            search(this.innerHTML);
        });
        function search(key) {
            if (key == "" || key == "搜索你想要的商品") {
                alert("请输入查询的商品");
                return false;
            }
            $("#formSearch").submit();
            return true;
          
        }
    });
}

function InitCategory(isExpand) {
    $(document).ready(function () {
        var category = new Winner.Category("category");
        category.Initialize();
        if (!isExpand) {
            $("#divCategoryFont").bind("mouseover", function() {
                $("#category").show();
            });
            $("#divCategoryFont").bind("mouseout", function () {
                $("#category").hide();
            });
            $("#category").bind("mouseover", function () {
                $("#category").show();
            });
            $("#category").bind("mouseout", function () {
                $("#category").hide();
            });
        }
    });
}

$(document).ready(function () {
    $("#hfAddToFavorite").click(function () {
        try {
            window.external.addFavorite(window.location.href, "Gobuy");
        } catch (e) {
            try {
                window.sidebar.addPanel("Gobuy", window.location.href, "");
            } catch (e) {
                alert("加入收藏失败，请使用Ctrl+D进行添加");
            }
        }
    });
    var keifu = function () {
        var kf = $("#kefuer");
        var wkbox = $("#kefuer .keifu_box");
        var kfClose = $("#kefuer .keifu_close");
        var iconKeifu = $("#kefuer .icon_keifu");
        var kH = wkbox.height();
        var kW = wkbox.width();
        var wH = $(window).height();
        kf.css({ height: kH });
        iconKeifu.css("top", parseInt((kH - 100) / 2));
        var kfTop = (wH - kH) / 2;
        if (kfTop < 0) kfTop = 0;
        kf.css("top", kfTop);
        $(kfClose).click(function () {
            kf.animate({ width: "0" }, 200, function () {
                wkbox.hide();
                iconKeifu.show();
                kf.animate({ width: 26 }, 300);
            });
        });
        $(iconKeifu).click(function () {
            $(this).hide();
            wkbox.show();
            kf.animate({ width: kW }, 200);
        });
    };
    keifu();
});

 

