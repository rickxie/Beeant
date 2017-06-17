Winner = typeof (Winner) != "undefined" ? Winner : {};
Winner.Skuer = function (id,products,config) {
    this.Base = new Winner.ClassBase();
    this.Container = document.getElementById(id);
    this.IdPropertyName = "SkuId";
    this.ValuePropertyName = "SkuValue";
    this.StyleFile = "/scripts/Winner/Skuer/Styles/Style.css";
    this.Properties = [];//[{Id:'',SelectItem:null,Items:[{Value:'',Control:null,IsSelect:true}]}]选择属性
    this.Products = products;//{Id:'',Sku:[{Id:'',Value:''}]}产品
    if (config != undefined) {
        this.Base.LoadConfig(this, config);
    }
};
Winner.Skuer.prototype = {
    Initialize: function () {//加载css样式文件
        if (this.Container == null) {
            return;
        }
        this.Container.className = "skuer";
        this.Base.LoadCssFile(this.StyleFile);
        this.LoadControl(this.Container.childNodes);
    },
    LoadControl: function (ctrls) {//加载控件
        for (var i = 0; i < ctrls.length; i++) {
            this.LoadControl(ctrls[i].childNodes);
            if (this.Base.GetAttribute(ctrls[i], this.IdPropertyName) == null) {
                continue;
            }
            this.AddPropertyValue(ctrls[i]);
            this.BindEvent(ctrls[i]);
        }
    },
    AddPropertyValue: function (ctrl) {//添加属性值
        var id = this.Base.GetAttribute(ctrl, this.IdPropertyName);
        var value = this.Base.GetAttribute(ctrl, this.ValuePropertyName);
        var item = { Value: value, Control: ctrl,IsSelect: true };
        var property = null;
        for (var i = 0; i < this.Properties.length; i++) {
            if (this.Properties[i].Id == id) {
                property = this.Properties[i];
                break;
            }
        }
        if (property == null) {
            property = { Id: id,SelectItem:null, Items: [] };
            this.Properties.push(property);
        }
        property.Items.push(item);
    },
    BindEvent: function (ctrl) {
        var self = this;
        this.Base.BindEvent(ctrl, "click", function () {
            var id = self.Base.GetAttribute(this, self.IdPropertyName);
            var value = self.Base.GetAttribute(this, self.ValuePropertyName);
            var sku={Id:id,Value:value};
            self.SetProperty(sku);
        });
    },
    SetProperty: function (sku) { //设置SKU属性
        var property = this.GetSelectPropety(sku);
        if (property == null)
            return;
        var selectSku = this.GetSelectSku();
        var product = this.GetSelectProduct(selectSku);
        if (product != null) {
            this.SetItemSelectStatus(selectSku);
            this.PushHistory(product.Id);
            this.ResetProduct(product);
        }
        this.ResetItemClass();
    },
    GetSelectPropety:function(sku) {//得到选择信息
        var property = null;
        for (var i = 0; i < this.Properties.length; i++) {
            if (this.Properties[i].Id == sku.Id) {
                property = this.Properties[i];
                break;
            }
        }
        if (property == null)
            return null;
        for (var j = 0; j < property.Items.length; j++) {
            if (property.Items[j].Value == sku.Value) {
                if (property.Items[j].IsSelect) {
                    property.SelectItem = property.Items[j];
                    break;
                }
                return null;
            }
        }
        return property;
    },//得到当前选择item
    GetSelectSku: function () {//得到选择的SKU
        var selectSku = [];
        for (var i = 0; i < this.Properties.length; i++) {
            if (this.Properties[i].SelectItem != null) {
                selectSku.push({ Id: this.Properties[i].Id, Value: this.Properties[i].SelectItem.Value });
            }
        }
        return selectSku;
    },
    SetItemSelectStatus: function (selectSku) {//设置选择项状态
        for (var i = 0; i < selectSku.length; i++) {
            var temp = this.Base.Clone(selectSku);
            for (var j = 0; j < this.Properties[i].Items.length; j++) {
                temp[i].Value = this.Properties[i].Items[j].Value;
                this.Properties[i].Items[j].IsSelect = this.HasProduct(temp);
            }
        }
    },
    HasProduct: function (temp) {//检查是否存在产品
        for (var k = 0; k < this.Products.length; k++) {
            if (this.MatchSkus(this.Products[k].Sku, temp)) {
                return true;
            }
        }
        return false;
    },
    GetSelectProduct: function (selectSku) { //得到选中的产品
        if (selectSku.length != this.Properties.length) {
            return null;
        }
        for (var k = 0; k < this.Products.length; k++) {
            if (this.MatchSkus(this.Products[k].Sku, selectSku)) {
                return this.Products[k];
            }
        }
        return null;
    },
    MatchSkus: function (skus, dataSkus) {//匹配SKU
        if (skus == null || dataSkus == null)
            return false;
        for (var j = 0; j < dataSkus.length; j++) {
            var isMatch = false;
            for (var k = 0; k < skus.length; k++) {
                if (dataSkus[j].Id == skus[k].Id && dataSkus[j].Value == skus[k].Value) {
                    isMatch = true;
                    break;
                }
            }
            if (!isMatch) {
                return false;
            }
        }
        return true;
    },
    ResetItemClass: function () {//重置样式
        for (var i = 0; i < this.Properties.length; i++) {
            for (var j = 0; j < this.Properties[i].Items.length; j++) {
                this.Properties[i].Items[j].Control.className = this.Properties[i].Items[j].IsSelect ? "normal" : "unselect";
            }
            if (this.Properties[i].SelectItem != null) {
                this.Properties[i].SelectItem.Control.className = "select";
            }
        }
    },
    PushHistory: function (id) {//保存地址
        try {
            if (this.Url == undefined)
                return;
            var stateObject = {};
            var title = document.title;
            var newUrl = this.Url + id;
            history.pushState(stateObject, title, newUrl);
        } catch (e) {

        }

    },
    ResetProduct: function(product) {
    }
    
};
