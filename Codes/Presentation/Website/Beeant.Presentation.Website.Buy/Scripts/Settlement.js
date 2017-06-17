Settlement = function(addValidateInfos, modifyValidateInfos, districts) {
    this.AddAddressValidateInfos = addValidateInfos;
    this.ModifyAddressValidateInfos = modifyValidateInfos;
    this.Districts = districts;
    this.SubCountButton = $("a[name='btnSubCount']");
    this.AddCountButton = $("a[name='btnAddCount']");
    this.SelectAddressId = $("#hfSelectAddressId");
    this.AddAddressButton = $("#hfAddAddress");
    this.AddressContainer = $("#ulAddress");
    this.CouponContainer = $("#ulCoupon");
    this.InputCount = $("input[class='countinput']");
    this.RemoveButton = $("#btnRemove");
    this.GoodsContainer = $("#divGoods");
    for (var i = 0; i < this.InputCount.length; i++) {
        this.SetCountClass(this.InputCount[i]);
    }
    this.InitDialog();
    this.InitValidator();
    this.BindCountEvent();
    this.BindAddressEvent();
    this.BindCouponEvent();
    this.InitRemove();
    this.InitCode();
};
Settlement.prototype = {
    Initialize: function (index) {
        this.InitCheckBox(index);
    },
    //初始化验证
    InitCheckBox: function (index) {
        this.CheckBox = new Winner.CheckBox("tbSettlement" + index, { StyleFile: "" });
        this.CheckBox.Initialize();
    },
    InitRemove: function() {
        var self = this;
        this.RemoveButton.click(function() {
            var cks = self.GoodsContainer.find("input[type='checkbox']");
            var count = 0;
            cks.each(function(index, value) {
                if (this.checked) {
                  count=count+1;
                }  
            });
             if (count == 0) {
                alert("请选择要删除的商品");
                return;
            }
            var i = 0;
            cks.each(function(index, value) {
                if (this.checked) {
                    $(this).parent().parent().remove();
                } else {
                    this.name = "GoodsId[" + i + "]";
                    i++;
                }
            });
            self.Redirect();
        });
    },
    InitCode: function() {
        $("#hfCode").click(function() {
            var date = new Date();
            $("#imgCode").attr("src", "/Order/SettlementCode?vesion" + date);
        });
        $("#btnSubmit").click(function() {
            if ($("#txtCode").length > 0 && $("#txtCode").val() == "") {
                alert("请输入验证码");
                return false;
            }
            return true;
        });
    },
    //优惠券
    BindCouponEvent: function() {
        var self = this;
        this.CouponContainer.find("input[type='radio']").click(function() {
            self.Redirect();
        });

    },
    //调整数量
    BindCountEvent: function () {
        var self = this;
        this.SubCountButton.click(function () {
            var input = $(this).next();
            if (parseInt(input.val()) <= 1) return;
            var value = parseInt(input.val()) - 1;
            input.val(value);
            self.Redirect();
        });
        this.AddCountButton.click(function () {
            var input = $(this).prev();
            var maxValue = parseInt(input.attr("MaxCount"));
            if (parseInt(input.val()) >= maxValue) return;
            var value = parseInt(input.val()) + 1;
            input.val(value);
            self.Redirect();
        });
        var validateFunc = function(ctrl) {
            ctrl.value = ctrl.value.replace(/[^\d]/g, '');
            var value = parseInt($(ctrl).val());
            if (isNaN(value))
                value = 1;
            var maxValue = parseInt($(ctrl).attr("MaxCount"));
            if (value < 1 || value > maxValue)
                value = 1;
            ctrl.value = value;
        };
        this.InputCount.keyup(function () {
            validateFunc(this);
        });
        this.InputCount.bind("afterpaste", function () {
            validateFunc(this);
        });
        this.InputCount.bind("blur", function () {
            if ($(this).val() != $(this).attr("SelectCount")) {
                self.Redirect();
            }
        });
    },
    SetCountClass: function (input) {
        var subbtn = $(input).prev();
        var addbtn = $(input).next();
        var value = parseInt(input.value);
        var maxValue = parseInt($(input).attr("MaxCount"));
        if (value <= 1) {
            subbtn.attr("class", "countbtn unclick");
        } else {
            subbtn.attr("class", "countbtn");
        }
        if (value >= maxValue) {
            addbtn.attr("class", "countbtn unclick");
        } else {
            addbtn.attr("class", "countbtn");
        }
    },
    //地址操作
    InitDialog: function () {
        this.Dialog = new Winner.Dialog("收货地址", "", { IsShowDialog: false, StyleFile: "" });
        this.Dialog.Initialize();
        this.Dialog.Container.style.width = "625px";
        $(this.Dialog.Bottom).hide();
    },
    InitValidator: function () {
        this.Validator = new Winner.Validator({ PropertyName: "ValidateName", StyleFile: "", IsShowMessage: false });
        this.Validator.Initialize();
    },
      
    BindAddressEvent: function() {
        var self = this;
        this.AddAddressButton.click(function() {
            self.ShowAddress(0);
        });
        this.AddressContainer.find("a[name='edit']").click(function() {
            var id = parseInt($(this).parent().find("input[type='radio']").val());
            self.ShowAddress(id);
        });
        this.AddressContainer.find("a[name='remove']").click(function() {
            var id = parseInt($(this).parent().find("input[type='radio']").val());
            self.RemoveAddress(id);
        });
        this.AddressContainer.find("input[type='radio']").click(function() {
            self.SelectAddressId.val(this.value);
            self.Redirect();
        });
        
    },
    InitAddressDialog: function () {//初始区域
        var self = this;
        eval($("#scriptDistrict").html());
        $("#btnSaveAddress").click(function () {
            var rev = self.Validator.ValidateSubmit();
            if (rev) {
                self.SaveAddress();
            }
        });
    },
    ShowAddress: function (id) {//显示地址
        var self = this;
        $.ajax({
            type: "GET",
            url: "/Address/Dialog?id=" + id,
            async: false,
            dataType: "text",
            success: function (data) {
                self.Dialog.Detail.innerHTML = data;
                self.Dialog.ShowDialog();
                self.InitAddressDialog();
                if (id == 0)
                    self.Validator.InitializeControl(self.AddAddressValidateInfos,$("#fmAddress")[0]);
                else
                    self.Validator.InitializeControl(self.ModifyAddressValidateInfos,$("#fmAddress")[0]);
            }
        });
    },
    SaveAddress: function () {//保存地址
        var self = this;
        var url = "/Address/AddDialog";
        if($("#hfAddressId").val()!="" && $("#hfAddressId").val()!="0")
            url = "/Address/ModifyDialog";
        $.ajax({
            type: "POST",
            url: url,
            async: false,
            dataType: "json",
            data:
            {
                Id: $("#hfAddressId").val(),
                DistrictId: $("#ddlCity").val(),
                Recipient: $("#txtAddressRecipient").val(),
                Mobile: $("#txtAddressMobile").val(),
                Postcode: $("#txtAddressPostcode").val(),
                Address: $("#txtAddressAddress").val()
            },
            success: function(data) {
                if (data.Result == 'true') {
                    self.SelectAddressId.val($("#hfAddressId").val());
                    self.Redirect();
                    self.Dialog.CloseDialog();
                } else {
                    alert(data.Result);
                }
            }
        });
    },
    RemoveAddress: function(id) { //移除地址
        var self = this;
        var rev = confirm('您确定要删除吗？');
        if (!rev) return;
        $.ajax({
            type: "POST",
            url: "/Address/Remove",
            async: false,
            dataType: "text",
            data:
            {
                Id: id,
            },
            success: function(data) {
                if (data == "true") {
                    if ($("#address" + id).attr("checked") == "checked") {
                        self.SelectAddressId.val(0);
                        self.Redirect();
                    } else {
                        $("#address" + id).parent().remove();
                    }

                } else {
                    alert(data);
                }
            }
        });
    },
    ReplaceAll: function (source, oldString, newString) {//替换所有
        var reg = new RegExp("\\" + oldString, "g");
        return source.replace(reg, newString);
    },
    //重写调整
    Redirect: function() {
        $("#fmSubmit").attr("action", "/Order/Settlement");
        $("#fmSubmit")[0].submit();
    }
};

