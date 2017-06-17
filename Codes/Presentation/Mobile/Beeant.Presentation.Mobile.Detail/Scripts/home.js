function initItem(nopic, goodsId, products,changeUrl) {
    //图片
    function initImages(content) {
        var banner = new Winner.Banner("");
        banner.Container = content.find("div[class='focusBar']");
        banner.Initialize();
    }
    //异步
    var config = [
        {
            Triggers: [
                {
                    Sender: window, Event: "scroll",
                    Function: function () {
                        return $("#divComment")[0].style.display != "none" && $(document).scrollTop() >= $(document).height() - $(window).height();
                    }
                },
                {
                    Sender: $("#liComment")[0],
                    Event: "click",
                    Function: function () {
                        if ($("#liComment").attr("Isload") == "true")
                            return false;
                        $("#liComment").attr("Isload", "true");
                        return true;
                    }
                },
                {
                    Sender: $("#btnLoadComment")[0],
                    Event: "click"
                }
            ],
            Loading: { Container: $("#btnLoadComment"), Type: "Replace" },
            Url: "/Home/Comment?goodsId=" + goodsId,
            Paramters: {page:0},
            Content: $("#divCommentContent")[0],
            ShowType: "Append",
            DataType: "text",
            RequestType: "Repeat",
            IsExecute: false,
            IsLoadHideContent: false,
            Group: "",
            BeginLoadFunction: function (sender, info) {

            },
            BeginShowFunction: function (sender, info, data) {
                info.Paramters.page = info.Paramters.page + 1;
            },
            EndShowFunction: function (sender, info, data) {
                if (data == "") {
                    $("#btnLoadComment").hide();
   
                }
            }
        }
    ];
    for (var i = 0; i < products.length; i++) {
        products[i].Sku = eval(products[i].Sku);
        config.push({
            Triggers: [
            {
                Sender: $("#btnLoadDetail" + products[i].Id)[0], Event: "click"
            }
            ],
            Loading: { Content: "content", Type: "Replace" },
            Url: "/Home/GoodsDetail?goodsid=" + goodsId + "&productid=" + products[i].Id,
            Paramters: {},
            Content: $("#divDetail" + products[i].Id)[0],
            ShowType: "Replace",
            DataType: "text",
            RequestType: "OneTime",
            IsExecute: false,
            IsLoadHideContent: false,
            Group:"Detail",
            BeginLoadFunction: function () {

            },
            BeginShowFunction: function (sender, info, data) {

            },
            EndShowFunction: function (sender, info, data) {
                $(info.Content).find("img").each(function () {
                    $(this).attr("data-original", $(this).attr("src"));
                    $(this).attr("src", nopic);
                    $(this).css("width", "100%");
                });
                window.lazyloadImage(info.Content);
            }
        });
    }
    for (var i = 0; i < products.length; i++) {
        config.push({
            Triggers: [
            {
                Sender: $("#btnLoadImage" + products[i].Id)[0], Event: "click"
            }
            ],
            Loading: { Content: "content", Type: "Replace" },
            Url: "/Home/GoodsImages?goodsid=" + goodsId + "&productid=" + products[i].Id,
            Paramters: {},
            Content: $("#divImage" + products[i].Id)[0],
            ShowType: "Replace",
            DataType: "text",
            RequestType: "OneTime",
            IsExecute: i == 0,
            IsLoadHideContent: false,
            Group: "Image",
            BeginLoadFunction: function () {

            },
            BeginShowFunction: function (sender, info, data) {

            },
            EndShowFunction: function (sender, info, data) {
                $(info.Content).find("img").css("width", "100%");
                setTimeout(initImages($(info.Content)), 500);
                window.lazyloadImage(info.Content);
            }
        });
    }
    for (var i = 0; i < products.length; i++) {
        config.push({
            Triggers: [
            {
                Sender: $("#btnLoadPromotion" + products[i].Id)[0], Event: "click"
            }
            ],
            Loading: { Content: "content", Type: "Replace" },
            Url: "/Home/Promotion?productid=" + products[i].Id,
            Paramters: {},
            Content: $("#divPromotion" + products[i].Id)[0],
            ShowType: "Replace",
            DataType: "text",
            RequestType: "OneTime",
            IsExecute: i == 0,
            IsLoadHideContent: false,
            Group: "Promotion",
            BeginLoadFunction: function () {

            },
            BeginShowFunction: function (sender, info, data) {

            },
            EndShowFunction: function (sender, info, data) {
              
            }
        });
    }
    var dataloader = new Winner.DataLoader(config);
    dataloader.Initialize();
 
    //切换
    $(".sidebar").find("li").bind("click", function () {
        if ($(this).attr("class") == "active")
            return;
        $(".sidebar").find("li[class='active']").attr("class", "");
        $(this).attr("class", "active");
        if ($(this).html() == "商品信息") {
            $("#divDetail" + $("#hfCurrentProductId").val()).hide();
            $("#divComment").hide();
            $("#divBasic").show();
        }
        else if ($(this).html() == "商品详情") {
            $("#divDetail" + $("#hfCurrentProductId").val()).show();
            $("#divComment").hide();
            $("#divBasic").hide();
            $("#btnLoadDetail" + $("#hfCurrentProductId").val())[0].click();

        } else {
            $("#divDetail" + $("#hfCurrentProductId").val()).hide();
            $("#divComment").show();
            $("#divBasic").hide();
        }
    });

    //
    function pushBrower() {
        $.ajax({
            type: "Get",
            url: "/Browse/Push?productId=" + $("#hfCurrentProductId").val(),
            async: true,
            dataType: "post",
            success: function (data) {
            },
            error: function (ex) {
             
            }
        });
    }
    pushBrower();
    //
    var skuer = new Winner.Skuer("divSkuer", products, { StyleFile: null, Url: changeUrl });
    skuer.ResetProduct = function (product) {
        $("#hfCurrentProductId").val(product.Id);
        $("#hfCurrentProductId").attr("ProductName",product.Name);
        $("#hfCurrentProductId").attr("ProductCount",product.Count);
        $("#hfCurrentProductId").attr("ProductOrderMinCount",product.OrderMinCount);
        $("#hfCurrentProductId").attr("ProductOrderStepCount",product.OrderStepCount);
        $("#hfCurrentProductId").attr("ProductOrderLimitCount",product.OrderLimitCount);
        $("#hfCurrentProductId").attr("ProductFileName", product.FileName);
        $("#hfCurrentProductId").attr("ProductPrice", product.Price);
        $("#btnLoadImage" + product.Id)[0].click();
        $("#btnLoadPromotion" + product.Id)[0].click();
        $("#divName").html(product.Name);
        $("#divPrice").html(product.Price);
        $("#divSalesCount").html(product.SalesCount);
        pushBrower();
    }
    skuer.Initialize();
    $(products).each(function(index, sender) {
        if (sender.Id != $("#hfCurrentProductId").val())
            return;
        for (var i = 0; i < sender.Sku.length; i++) {
            skuer.SetProperty(sender.Sku[i]);
        }

    });
    //
    function setCart() {
        var func = function () {
            var html = InitShopcartBar();
            $(document.body).append(html);
        }
        var base = new Winner.ClassBase();
        base.LoadScriptFile(window.ShopcartUrl + "/Home/Bar", $(".cartconer")[0], func);
    }
    setCart();
    $("#btnShopcart").click(function () {
        var loginfunc = function() {
            var addcartfunc = function () {
                if ($("#btnShopcart").attr("IsAdd") == "true")
                    return;
                window.Shopcart.Add({
                    Id: "p" + $("#hfCurrentProductId").val(),
                    Count: 1,
                    ProductId: $("#hfCurrentProductId").val(),
                    ProductName: $("#hfCurrentProductId").attr("ProductName"),
                    ProductCount: $("#hfCurrentProductId").attr("ProductCount"),
                    ProductOrderMinCount: $("#hfCurrentProductId").attr("ProductOrderMinCount"),
                    ProductOrderStepCount: $("#hfCurrentProductId").attr("ProductOrderStepCount"),
                    ProductOrderLimitCount: $("#hfCurrentProductId").attr("ProductOrderLimitCount"),
                    ProductPrice: $("#hfCurrentProductId").attr("ProductPrice"),
                    ProductFileName: $("#hfCurrentProductId").attr("ProductFileName")
                });
                $("#btnShopcart").attr("IsAdd", "true");
            }
            if ($("#divShopcartContent").attr("IsLoadShopcart") == "true") {
                addcartfunc();
                return;
            }
            window.LoadShopcart(addcartfunc);
        };
        var unloginfunc = function () {
            window.AddLoginShopcart = loginfunc;
            window.LoadLogin("window.AddLoginShopcart();");
        };
        window.InvokeShopcartBar(loginfunc, unloginfunc);
    });
    $("#btnOrder").click(function() {
        $("#fmOrder")[0].submit();
    });
    
}
