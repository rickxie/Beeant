var Goods = function (config) {
    this.Base = new Winner.ClassBase();
    if (config != undefined) {
        this.Base.LoadConfig(this, config);
    }
    this.PropertyInfos = null;
    this.ImageValueControl = $("#" + this.ImageValueId);
    this.CategoryPropertyControl = $("#" + this.CategoryPropertyId);
    this.PropertyControl = $("#" + this.PropertyValueId);
    this.ProductValueControl = $("#" + this.ProductValueId);
    this.PricesValueControl = $("#" + this.PricesValueId);
    if (this.ImageValueControl.val() == "" && this.GoodsImagesValue != "") {
        this.ImageValueControl.val(this.GoodsImagesValue);
    }
    if ($("#" + this.ProductValueId).val() == "" && this.ProductsValue != "") {
        $("#" + this.ProductValueId).val(this.ProductsValue);
    }
    if (this.PricesValueControl.val() == "" && this.PricesValue != "") {
        this.PricesValueControl.val(this.PricesValue);
    }
};
Goods.prototype = {
    Initialize: function () {
        this.CreatePublisher();
        this.SetPublisherCategory();
        this.SetPublisherProperty();
        this.SetPublisherImage();
        this.SetPublisherSku();
        this.SetPublisherSkuProduct();
        this.SetPublisherSkuImage();
        this.SetSaveButton();
        this.Execute();
    },
    CreatePublisher: function () {
        this.Publisher = new Winner.Publisher({ ResetbButtonId: "btnReset", PublishButtonId: "btnPushlish",
            BranchId: this.BranchId, CategoryContainerId: "divCategoryContainer",
            PropertyContainerId: "divPropertyContainer", PropertyValueControlId: this.PropertyValueId,
            SkuContainerId: "divSkuProperty", SkuProductContainerId: "divSkuProduct", SkuImageContainerId: "divSkuImage",
            SelectSkuContainerId: "divSelectSku",
            GoodsContainerId: "divGoodsContainer", ImageContainerId: "divImageContainer", ImageValueControlId: this.ImageValueId,
            ProductValueControlId: this.ProductValueId
        },
            { CategoryConfig: { MatchTitle: "请输入关键字" },
                PropertyConfig: { Titles: { Select: "请选择"} },
                SkuConfig: { Titles: { PropertyTitle: "销售", RemoveTitle: "删除", Image: { Title: "请上传图片（可以不选择）" },
                    Product: { Price: "面价", Cost: "底价", Count: "数量", OrderMinCount: "最小起订数量", OrderStepCount: "订购步长数量", OrderLimitCount: "订购限订数量", DataId: "商家编码", DepositRate: "定金比例", IsReturn: "是否支持退换", IsCustom: "是否支持定制", IsSales: "是否上架" },
                    Errors: { ImageCountOver: "图片数量已经超出SKU数量的范围", ProductCountOver: "产品数量已经超出SKU数量的范围" }
                }
                },
                ImageConfig: { ExtensionErrorMessage: "请选择图片文件", MaxSize: 512000, SizeErrorMessage: "图片大小不能超过500KB", AnsyUploadUrl: "/Product/Goods/Upload.aspx" }
            });

        this.Publisher.Sku.ProductHtml = "<table class='tb' Instance='Content'><tr><th class='per'>销售</th><th>" +
              "面价</th><th>底价</th><th>数量</th><th>最小起订数量</th><th>订购步长数量</th><th>商家编码</th><th>定金比例</th><th>是否支持退换</th><th>是否支持定制</th><th>是否上架</th></tr></table>";
        this.Publisher.Sku.ProductModelHtml = "<td><span><input type='text' name='Name'  value='@Name'/></span></td><td><input type='hidden' name='id'  value='@Id'/>" +
            "<input type='text' name='Price' maxlength='12' value='@Price' ValidateName='Price'/></td>" +
        "<td><input type='text'maxlength='9' name='Cost' value='@Cost' ValidateName='Cost'/></td>" +
        "<td><input type='text' name='Count' ValidateName='Count' maxlength='9' value='@Count'/></td>" +
        "<td><input type='text' name='OrderMinCount' ValidateName='OrderMinCount' maxlength='9' value='@OrderMinCount'/></td>" +
        "<td ><input type='text' name='OrderStepCount' ValidateName='OrderStepCount' maxlength='9' value='@OrderStepCount'/></td>" +
        "<td ><input type='text' name='DataId' maxlength='100' ValidateName='DataId' value='@DataId'/></td>" +
        "<td ><input type='text' name='DepositRate' maxlength='100' ValidateName='DepositRate' value='@DepositRate'/></td>" +
            "<td><input type='checkbox' name='IsReturn' @IsReturn/></td><td><input type='checkbox' name='IsCustom' @IsCustom/></td>" +
            "<td><input type='checkbox' name='IsSales' @IsSales/></td>";
        this.Publisher.Sku.ReplaceProductModelHtml= function (html, skus, skusIndex, info) {
            return html.replace("@Name", info == null ? "" : this.GetSkusValue(skus))
                .replace("@Id", info == null ? "" : info.Id).replace("@Count", info == null ? "99999" : info.Count)
                .replace("@Cost", info == null ? "80" : info.Cost).replace("@OrderMinCount", info == null ? "1" : info.OrderMinCount)
                .replace("@OrderStepCount", info == null ? "1" : info.OrderStepCount)
                .replace("@DataId", info == null ? "" : info.DataId)
                .replace("@Price", info == null ? "100" : info.Price).replace("@DepositRate", info == null ? "0" : info.DepositRate)
                .replace("@IsCustom", info != null && info.IsCustom=="true" ? "checked='checked'" : "")
                .replace("@IsReturn", info != null && info.IsReturn == "true" ? "checked='checked'" : "")
                .replace("@IsSales", info != null && info.IsSales == "true" ? "checked='checked'" : "");
        }
        this.Publisher.Initialize();
    },
    SetPublisherCategory: function () {
        this.Publisher.Category.GetInfos = function (parentId) {
            var rev = [];
            $.ajax({
                type: 'GET',
                url: '/Ajax/Product/Category.aspx?parentId=' + parentId+"&vesion="+new Date(),
                async: false,
                data: "",
                success: function(msg) {
                     rev = eval(msg);
                }
            });
            return rev;
        };
    },
    SetPublisherProperty: function () {
        var self = this;
        this.Publisher.Property.GetInfos = function (categoryId) {
            return self.GetPropertyInfos(categoryId, false);
        };
    },
    SetPublisherSku: function () {
        var self = this;
        this.Publisher.Sku.GetInfos = function (categoryId) {
            return self.GetPropertyInfos(categoryId, true);
        };
    },
    SetPublisherImage: function () {
        var self = this;
        this.Publisher.Image.GetInfos = function () {//设置产品接口
            return self.GetImageInfos(false);
        };
    },
    SetPublisherSkuProduct: function () {//设置产品接口
        var self = this;
        this.Publisher.Sku.GetProductInfos = function () {
            return self.GetProductInfos();
        };
    },
    SetPublisherSkuImage: function () {
        var self = this;
        this.Publisher.Sku.GetImageInfos = function () {
            return self.GetImageInfos(true);
        };
    },
    GetProductInfos: function () {//得到产品
        if (this.Products == null && this.ProductValueControl.val() != "") {
            this.Products = eval(this.ProductValueControl.val());
        }
        if (this.Products == null)
            return null;
        var infos = [];
        for (var i = 0; i < this.Products.length; i++) {
            if (this.Products[i].Sku != "") {
                infos.push(this.Products[i]);
            }
        }
        return infos;
    },
    GetImageInfos: function (isSku) {//得到图片
        if (this.GoodsImages == null && this.ImageValueControl.val() != "") {
            this.GoodsImages = eval(this.ImageValueControl.val());
        }
        if (this.GoodsImages == null)
            return null;
        var infos = [];
        for (var i = 0; i < this.GoodsImages.length; i++) {
            if (isSku && this.GoodsImages[i].Sku != "" || !isSku && this.GoodsImages[i].Sku == "") {
                infos.push(this.GoodsImages[i]);
            }
        }
        return infos;
    },
    GetPropertyInfos: function (categoryId, isSku) {//得到属性
        var self = this;
        if (this.PropertyInfos == null) {
            $.ajax({
                type: 'GET',
                url: '/Ajax/Product/Property.aspx?categoryId=' + categoryId + '&goodsId=' + self.GoodsId+"&vesion="+new Date(),
                async: false,
                data: null,
                success: function(msg) {
                     self.CategoryPropertyControl.val(msg);
                }
            });
            if (self.CategoryPropertyControl.val() != "") {
                this.PropertyInfos = eval(self.CategoryPropertyControl.val());
            }
        }
        if (this.PropertyInfos == null)
            return null;
        this.SetPropertyDefaultInfos();
        var infos = [];
        for (var i = 0; i < this.PropertyInfos.length; i++) {
            if (isSku == this.PropertyInfos[i].IsSku)
                infos.push(this.PropertyInfos[i]);
        }
        return infos;
    },
    SetPropertyDefaultInfos: function () {//设置默认
        if (this.PropertyControl.val() == "" || this.IsSetPropertyDefaultInfos==true)
            return;
        var defaultInfos = eval(this.PropertyControl.val());
        if (defaultInfos == null)
            return;
        for (var i = 0; i < this.PropertyInfos.length; i++) {
            for (var j = 0; j < defaultInfos.length; j++) {
                if (this.PropertyInfos[i].Id == defaultInfos[j].PropertyId) {
                    this.PropertyInfos[i].DefaultValues.push({ Id: defaultInfos[j].Id, Value: defaultInfos[j].Value, IsCustom: defaultInfos[j].IsCustom });
                }
            }
        }
        this.IsSetPropertyDefaultInfos = true;
    },
    ClearValidations: function () {
        for (var i = 0; i < window.validator.Validations.length; i++) {
            $(window.validator.Validations[i].Content).remove();
        }
    },
    SetSaveButton: function () {
        var self = this;
        document.getElementById(this.SaveButtonId).onclick = function () {
            self.Publisher.SetValue();
            var rev = window.validator.ValidateSubmit();
            rev = self.Publisher.Sku.Validate() && rev;
            rev = self.ProductValidator.ValidateSubmit() && rev;
            rev = self.GoodsDetailValidator.ValidateSubmit() && rev;
            rev = rev && self.Publisher.Property.ValidateSubmit();
            rev = rev && self.Publisher.Image.ValidateSubmit();
            return rev;
        };
    },
    Execute: function () {
        var self = this;
        if (this.IsPublish) {
            this.Publisher.Category.Reset($("#" + this.BranchId).val().split(','));
            this.Publisher.Publish();
            this.SetProductValidation();
            this.SetGoodsDetailValidation();
        } else {
            this.Publisher.Category.Load(null);
        }
        var func= function() {
            $("#divSelectSku").find("input:checkbox").click(function () {
                $("#divSkuProperty").find("input:checkbox").click(function () {
                    self.SetProductValidation();
                });
                self.SetProductValidation();
            });
        }
        $("#btnPushlish").click(function () {
            self.SetProductValidation();
            self.SetGoodsDetailValidation();
            func();
        });
        $("#btnReset").click(function () {
            self.PropertyInfos = null;
        });
        func();
    },
    SetProductValidation: function () {//设置产品属性
        this.ProductValidator = null;
        this.ProductValidator = new Winner.Validator({ PropertyName: "ValidateName" });
        this.ProductValidator.Initialize();
        var self = this;
        $("#divSkuProduct").find("span[class='validshowmess']").remove();
        $("#divSkuProduct").find("span[class='validsucessmess']").remove();
        $("#divSkuProduct").find("span[class='validerrormess']").remove();
        self.ProductValidator.InitializeControl(window.validateInfos, $("#divSkuProduct")[0]);
        $("#divSkuProduct").find("span[class='validshowmess']").html("");
    
    },
    SetGoodsDetailValidation: function () {
        if (this.GoodsDetailValidator == undefined) {
            this.GoodsDetailValidator = new Winner.Validator({ PropertyName: "ValidateName" });
            this.GoodsDetailValidator.Initialize();
            this.GoodsDetailValidator.InitializeControl(window.detailValidateInfos, $("#divGoodsContainer")[0]);
        }
    }

};
   
