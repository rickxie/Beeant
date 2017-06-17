(function () {
    function setShopcardBar() {
        if (Winner.Buyer == undefined || Winner.Shopcart == undefined) {
            setTimeout(setShopcardBar, 500);
            return;
        }
        //
        function showShopcartBar() {
            $("#divShopcart").css("bottom", "0");
            $("#divShopcart").show();
            $("#divShopcartContent").show();
            $("#divShopcartMask").show();
        }
        function hideShopcartBar() {
            $("#divShopcartContent").hide();
            $("#divShopcart").hide();
            $("#divShopcartMask").hide();
        }
        function invokeShopcartBar(loginFunc, unLoginFunc) {
            showShopcartBar();
            $.ajax({
                url: window.SharedUrl + '/Shared/GetToken?isSet=false&date=' + new Date(),
                dataType: "script",
                async: false,
                success: function () {
                    if (window.UserCookie != undefined) {
                        loginFunc();
                    } else {
                        unLoginFunc();
                    }
                }
            });
        }
        var buyer = new Winner.Buyer();
        var shopcart = new Winner.Shopcart("fmShopcart", { IsSaveCookie: false });

        //
        function addBuyer(product) {
            if (product == null || $(product).attr("IsBind") == "true")
                return;
            $(product).attr("IsBind", "true");
            buyer.BindEvent(product);
        }

   
        function loadShopcart(handle) {
            if ($("#divShopcartContent").attr("IsLoadShopcart") == "true")
                return;
            $("#divShopcartContent").attr("IsLoadShopcart", "true");
            $("#divShopcartContent").hide();
            $("#divShopcartLoading").show();
            var func = function () {
                var html = InitShopcart();
                $("#divShopcartContent").html(html);
                $("#divShopcartLoading").hide();
                $("#divShopcartContent").show();
                $("#hfSettle").click(function () {
                    $("#fmShopcart")[0].submit();
                });
                shopcart.Container = $("#fmShopcart")[0];
                shopcart.Initialize();
                buyer.Initialize();
                if (handle != null) {
                    handle();
                }
            }
            var base = new Winner.ClassBase();
            base.LoadScriptFile(window.ShopcartUrl + "/Home/Dialog", $("#divShopcartContent")[0], func);

        }
        function loadLogin(funcName) {
            if ($("#divShopcartContent").attr("IsLoadLogin") == "true")
                return;
            $("#divShopcartContent").attr("IsLoadLogin", "true");
            $("#divShopcartContent").hide();
            $("#divShopcartLoading").show();
            var func = function () {
                var html = InitLogin();
                $("#divShopcartContent").html(html);
                $("#divShopcartLoading").hide();
                $("#divShopcartContent").show();
            }
            var base = new Winner.ClassBase();
            base.LoadScriptFile(window.LoginUrl + "/Home/Dialog?url=" + funcName, $("#divShopcartContent")[0], func);
        }
      
 

        $("#closeShopcart").click(function () {
            hideShopcartBar();
        });
        //
        shopcart.Html = '<div class="info"><input type="hidden" name="Products[0].ProductId" value="@ProductId"/>' +
         '<div class="file"><img src="@ProductFileName" alt="@ProductName"/></div>' +
         '<div class="con">'+
         '<div class="name">@ProductName</div>' +
         '<div class="od"><span class="red">@ProductPrice</span>×<span Buyer="Count@ProductId">@Count</span>' +
         '<a name="btnSubCount" href="javascript:void(0)" class="normal" Buyer="Down" ProductId="@ProductId">-</a>' +
         '<input name="Products[0].Count" type="text" class="countinput" value="@Count" Buyer="Count" SaleId="@ProductId" Price="@ProductPrice" ProductId="@ProductId"  MaxCount="@ProductCount" OrderMinCount="@ProductOrderMinCount" OrderLimitCount="@ProductOrderLimitCount" OrderStepCount="@ProductOrderStepCount"/>' +
         '<a name="btnAddCount" href="javascript:void(0)" class="normal" Buyer="Up" ProductId="@ProductId">+</a></div>' +
         '</div>'+
         '<div class="remove" Instance="RemoveButton"></div></div>';
        shopcart.AddHandle = function (product) {
            if (product.Info.IsReturnAddHandle == true)
                return false;
            $.getJSON(window.ShopcartUrl + "/Home/Add?callback=?&productid=" + product.Info.ProductId + "&tag=", function (data) {
                product.Info.Id = data.Id;
                product.Info.Count = data.Count;
                product.Info.SaleId = data.ProductId;
                $(product).find("input[name='Products[0].Count']").val(data.Count).attr("shopcartid", data.Id);
                addBuyer(product);
                buyer.Recalculate();
            });
            return true;
        };
        shopcart.RemoveHandle = function (product) {
            buyer.Recalculate();
            $.getJSON(window.ShopcartUrl + "/Home/Remove?callback=?&id=" + product.Info.Id, function (data) {
            });
            return true;
        };
        buyer.Reset = function (info) {
            if (info.OriginCount == info.Count)
                return false;
            $.getJSON(window.ShopcartUrl + "/Home/UpdateCount?callback=?&id=" + info.CountInput.attr("shopcartid") + "&count=" + info.Count, function (data) {
            });
            return true;
        }
        window.Shopcart = shopcart;
        window.InvokeShopcartBar = invokeShopcartBar;
        window.LoadShopcart = loadShopcart;
        window.LoadLogin = loadLogin;
    }
    setShopcardBar();
})();