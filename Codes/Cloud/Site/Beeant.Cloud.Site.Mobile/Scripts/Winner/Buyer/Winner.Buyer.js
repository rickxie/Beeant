Winner = typeof (Winner) != "undefined" ? Winner : {};
Winner.Buyer = function (config) {
    this.Base = new Winner.ClassBase();
    if (config != undefined) {
        this.Base.LoadConfig(this, config);
    }
};
Winner.Buyer.prototype = {
    Initialize: function () {//加载css样式文件
        this.BindEvent();
    },
    BindEvent: function (container) {//绑定
        var self = this;
        container = container == undefined ? document : container;
        $(container).find("*[buyer='Up']").click(function () {
            self.SetCalculator(this, 'Up');
        });
        $(container).find("*[buyer='Down']").click(function () {
            self.SetCalculator(this, 'Down');
        });
        $(container).find("input[buyer='Count']").bind("keyup", function () {
            self.SetCalculator(this, 'Count');
        });
        $(container).find("input[buyer='Count']").bind("paste", function () {
            self.SetCalculator(this, 'Count');
        });
    },
    Recalculate: function (container) {//重新计算
        var self = this;
        container = container == undefined ? document : container;
        $(container).find("input[buyer='Count']").each(function (index,sender) {
            self.SetCalculator(this, 'Reset',true);
        });
    },
    SetCalculator: function (ctrl, type) {
        var self = this;
        var info = self.GetInfo($(ctrl), type);
        if (!this.Check(info))
            return false;
        self.Calculate(info);
        info.CountInput.attr("OriginCount", info.Count).val(info.Count);
    },
    Calculate: function (info) {
        if (info.IsCalculate) {
            $(document).find("*[Buyer='Count" + info.SaleId + "']").html(info.Count);
            $(document).find("*[Buyer='Price']").each(function(index, sender) {
                $(this).html(parseFloat($(this).html()) + info.DifferencePrice);
            });
            $(document).find("*[Buyer='Count']").each(function(index, sender) {
                $(this).html(parseInt($(this).html()) + info.Count - info.OriginCount);
            });
            $(document).find("*[Buyer='DiscountPrice']").each(function(index, sender) {
                $(this).html(parseFloat($(this).html()) + info.DifferenceDiscountPrice);
            });
            $(document).find("*[Buyer='Price" + info.SaleId + "']").each(function(index, sender) {
                $(this).html(parseFloat($(this).html()) + info.DifferencePrice);
            });
            $(document).find("*[Buyer='DiscountPrice" + info.SaleId + "']").each(function(index, sender) {
                $(this).html(parseFloat($(this).html()) + info.DifferenceDiscountPrice);
            });
        }
        this.ResetClass(info);
        this.Reset(info);
        return true;
    },
    Check:function(info) {//检查
        if (info.Count > info.MaxCount)
            return false;
        if (info.Count < info.OrderMinCount)
            return false;
        if (info.OrderLimitCount>0 && info.Count > info.OrderLimitCount)
            return false;
        if (info.OrderStepCount > 0 && info.Count % info.OrderStepCount != 0)
            return false;
        return true;
    },
    ResetClass:function(info) {
        var normalClass = this.ButtonNormalClass == undefined ? "normal" : this.ButtonNormalClass;
        var unclickClass = this.ButtonUnclickClass == undefined ? "unclick" : this.ButtonUnclickClass;
        if (info.Count <= info.OrderMinCount) {
            info.DownButton.attr("class",unclickClass);
        } else {
            info.DownButton.attr("class", normalClass);
        }
        if (info.Count >= info.MaxCount || info.OrderLimitCount>0 &&　info.Count >= info.OrderLimitCount) {
            info.UpButton.attr("class", unclickClass);
        } else {
            info.UpButton.attr("class", normalClass);
        }
    },
    GetCountInput:function(id) {
        return $(document).find("input[buyer='Count']").filter("[ProductId=" + id + "]");
    },
    GetUpButton:function(id) {
        return $(document).find("*[buyer='Up']").filter("[ProductId=" + id + "]");
    },
    GetDownButton:function(id) {
        return $(document).find("*[buyer='Down']").filter("[ProductId=" + id + "]");
    },
    GetInfo: function (sender,type) {//得到对象
        var productId = $(sender).attr("ProductId");
        var ctrl = this.GetCountInput(productId);
        var info = {
            ProductId: productId,
            Type:type,
            SaleId:ctrl.attr("SaleId"),
            UpButton:this.GetUpButton(productId),
            DownButton:this.GetDownButton(productId),
            CountInput: ctrl,
            Count: parseInt(ctrl.val()),
            OriginCount: ctrl.attr("OriginCount")==undefined?0:parseInt(ctrl.attr("OriginCount")),
            MaxCount: parseInt(ctrl.attr("MaxCount")),
            OrderMinCount: parseInt(ctrl.attr("OrderMinCount")),
            OrderLimitCount: parseInt(ctrl.attr("OrderLimitCount")),
            OrderStepCount: parseInt(ctrl.attr("OrderStepCount")),
            Price: ctrl.attr("Price") == undefined ? 0 : parseFloat(ctrl.attr("Price")),
            IsCalculate: ctrl.attr("IsCalculate") == undefined || ctrl.attr("IsCalculate") =="true",
            DiscountPrice:ctrl.attr("DiscountPrice")==undefined?0: parseFloat(ctrl.attr("DiscountPrice"))
        }
        switch (type) {
            case "Up": {
                info.Count += info.OrderStepCount;
            }; break;
            case "Down": {
                info.Count -= info.OrderStepCount;
            }; break;
        }
        info.DifferencePrice = info.Price * (info.Count - info.OriginCount);
        info.DifferenceDiscountPrice = info.DiscountPrice * (info.Count - info.OriginCount);
        return info;
    },
    Reset:function(info) {//重置
        
    }

};
