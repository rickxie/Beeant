Winner = typeof(Winner)!="undefined" ? Winner : {};
Winner.Publisher = Winner.Publisher ? Winner.Publisher : {};
Winner.Publisher = function (controls, config) {
    this.Base = new Winner.ClassBase();
    this.Titles = { CategoryMatchTitle: "", PropertyTitles: {} };
    if (config != undefined) {
        this.Base.LoadConfig(this, config);
    }
    this.Category = new Winner.Publisher.Category(controls.CategoryContainerId, controls.PublishButtonId, this.CategoryConfig);
    this.Sku = new Winner.Publisher.Sku(controls.SkuContainerId, controls.SkuProductContainerId, controls.SkuImageContainerId, controls.SelectSkuContainerId, this.ImageConfig, this.SkuConfig);
    this.Property = new Winner.Publisher.Property(controls.PropertyContainerId, this.PropertyConfig);
    this.Image = new Winner.Publisher.Image(controls.ImageContainerId, this.ImageConfig);
    this.ResetButton = document.getElementById(controls.ResetbButtonId);
    this.PublishButton = document.getElementById(controls.PublishButtonId);
    this.Branch = document.getElementById(controls.BranchId);
    this.GoodsContainer = document.getElementById(controls.GoodsContainerId);
    this.ImageValueControl = document.getElementById(controls.ImageValueControlId);
    this.PropertyValueControl = document.getElementById(controls.PropertyValueControlId);
    this.ProductValueControl = document.getElementById(controls.ProductValueControlId);
    this.StyleFile = "/scripts/Winner/Publisher/Styles/Style.css";
};
Winner.Publisher.prototype = {
    Initialize: function () { //初始化
        this.Base.LoadCssFile(this.StyleFile);
        this.Category.Initialize();
        this.BindResetEvent();
        this.BindPublishEvent();
    },
    BindResetEvent: function () {//重置类目
        if (this.ResetButton == null) return;
        var self = this;
        this.ResetButton.onclick = function () {
            self.Reset();
        };
    },
    BindPublishEvent: function () {//重置类目
        var self = this;
        if (this.PublishButton == null) return;
        this.PublishButton.onclick = function () {
            self.Publish();
        };
    },
    Reset: function () {//重置
        this.Category.Container.style.display = "";
        this.PublishButton.style.display = "";
        this.Property.Container.style.display = "none";
        this.Sku.Container.style.display = "none";
        this.Image.Container.style.display = "none";
        if (this.GoodsContainer != null)
            this.GoodsContainer.style.display = "none";
        if (this.ResetButton != null)
            this.ResetButton.style.display = "none";
        var array = this.Branch.value.split(',');
        this.Category.Reset(array);

    },
    Publish: function () {//发布
        this.Branch.value = this.Category.GetBranch(null, null);
        this.Category.Container.style.display = "none";
        this.Property.Container.style.display = "";
        this.Sku.Container.style.display = "";
        this.PublishButton.style.display = "none";
        if (this.ResetButton != null)
            this.ResetButton.style.display = "";
        if (this.GoodsContainer != null)
            this.GoodsContainer.style.display = "";
        if (this.Image.Container != null)
            this.Image.Container.style.display = "";
        var imageCount = 0;
        if (this.Category.CurrentNode != null && this.Category.CurrentNode.Info != null) {
            imageCount = parseInt(this.Base.GetAttribute(this.Category.CurrentNode.Info, "ImageCount"));
        }
        var values = this.Branch.value.split(',');
        this.Property.Load(values[values.length - 1]);
        this.Sku.Load(values[values.length - 1], imageCount, this.Property);
        this.Image.Load(imageCount);
    },
    SetValue: function () {
        this.SetImageValue();
        this.SetPropertyValue();
        this.SetProductValue();
    },
    //设置图片值
    SetImageValue: function () {
        if (this.ImageValueControl == null) return;
        this.ImageValueControl.value = "[";
        var controls = this.GetImageControls();
        for (var i = 0; i < controls.length; ) {
            this.ImageValueControl.value = this.ImageValueControl.value + this.GetImageJsonValue(controls[i]);
            i++;
        }
        if (this.ImageValueControl.value.length > 1)
            this.ImageValueControl.value = this.ImageValueControl.value.substr(0, this.ImageValueControl.value.length - 1);
        this.ImageValueControl.value = this.ImageValueControl.value + "]";
    },
    GetImageControls: function () {
        var files = [];
        var inputs = this.Image.Container.getElementsByTagName("input");
        for (var j = 0; j < inputs.length; j++) {
            if (inputs[j].type == "file" && this.Base.GetAttribute(inputs[j], "RealValue") != "") {
                files.push(inputs[j]);
            }
        }
        for (var k = 0; k < this.Sku.ImageContainer.Content.rows.length; k++) {
            var row = this.Sku.ImageContainer.Content.rows[k];
            if (row.style.display == "none") {
                var imgs = row.getElementsByTagName("input");
                for (var l = 0; l < imgs.length; l++) {
                    if (imgs[l].type == "file") {
                        this.Base.SetAttribute(imgs[l], "SaveType", "Remove");
                    }
                }
            }
        }
        var skuInputs = this.Sku.ImageContainer.Content.getElementsByTagName("input");
        for (var i = 0; i < skuInputs.length; i++) {
            if (skuInputs[i].type == "file" && this.Base.GetAttribute(skuInputs[i], "RealValue") != "") {
                files.push(skuInputs[i]);
            }
        }
        return files;
    },
    GetImageJsonValue: function (file) { //得到Json值
        var value = "{Id:'@Id',Value:'@Value',Sku:'@Sku',SaveType:'@SaveType'},";
        var id = this.Base.GetAttribute(file, "Id");
        var saveType = this.Base.GetAttribute(file, "SaveType");
        var realValue = this.Base.GetAttribute(file, "RealValue");
        value = value.replace("@Id", id).replace("@Value", realValue)
            .replace("@Sku", this.Base.GetAttribute(file, "Sku"))
            .replace("@SaveType", saveType);
        return value;
    },
    //设置属性值
    SetPropertyValue: function () {
        if (this.PropertyValueControl == null) return;
        this.PropertyValueControl.value = "[";
        for (var i = 0; i < this.Property.Properties.length; ) {
            var valueCtrls = this.GetPropertyControls(this.Property.Properties[i]);
            this.PropertyValueControl.value = this.PropertyValueControl.value + this.GetPropertyValue(this.Property.Properties[i], valueCtrls);
            i++;
        }
        for (var j = 0; j < this.Sku.Properties.length; ) {
            var skuValueCtrls = this.GetSkuPropertyControls(this.Sku.Properties[j]);
            this.PropertyValueControl.value = this.PropertyValueControl.value + this.GetPropertyValue(this.Sku.Properties[j], skuValueCtrls);
            j++;
        }
        if (this.PropertyValueControl.value.length > 1)
            this.PropertyValueControl.value = this.PropertyValueControl.value.substr(0, this.PropertyValueControl.value.length - 1);
        this.PropertyValueControl.value = this.PropertyValueControl.value + "]";
    },
    GetPropertyValue: function (property, valueCtrls) {
        var value = "";
        for (var i = 0; i < valueCtrls.length; i++) {
            if (valueCtrls[i].type == "checkbox" && valueCtrls[i].name == property.Info.Id && !valueCtrls[i].checked) continue;
            if (valueCtrls[i].type == "select-one" && valueCtrls[i].name == property.Info.Id && valueCtrls[i].options[0].selected) continue;
            if (valueCtrls[i].type == "input" && valueCtrls[i].value == "") continue;
            value = value + this.GetPropertyJsonValue(property, valueCtrls[i]);
        }
        return value;

    },
    GetPropertyControls: function (property) { //得到控件
        var ctrls = [];
        var inputs = property.Container.getElementsByTagName("input");
        for (var j = 0; j < inputs.length; j++) {
            if (inputs[j].name == property.Info.Id) {
                ctrls.push(inputs[j]);
            }
        }
        var selects = property.Container.getElementsByTagName("select");
        for (var i = 0; i < selects.length; i++) {
            if (selects[i].name == property.Info.Id) {
                ctrls.push(selects[i]);
            }
        }
        return ctrls;
    },
    GetSkuPropertyControls: function (property) { //得到控件
        var ctrls = [];
        var skuInputs = property.Container.getElementsByTagName("input");
        for (var i = 0; i < skuInputs.length; i = i + 2) {
            if (skuInputs[i].name == property.Info.Id && skuInputs[i].type == "checkbox" && skuInputs[i].checked) {
                ctrls.push(skuInputs[i + 1]);
            }
        }
        return ctrls;
    },
    GetPropertyJsonValue: function (property, valueControl) { //得到Json值
        var value = "{PropertyId:'@PropertyId',Value:'@Value',Id:'@Id'},";
        value = value.replace("@PropertyId", property.Info.Id)
            .replace("@Id", this.Base.GetAttribute(valueControl, "Id") == null ? "" : this.Base.GetAttribute(valueControl, "Id"))
            .replace("@Value", valueControl.value);
        return value;
    },
    //设置产品
    SetProductValue: function () {
        if (this.ProductValueControl == null) return;
        this.ProductValueControl.value = "[";
        var items = this.GetProductItems();
        for (var i = 0; i < items.length;) {
            this.ProductValueControl.value = this.ProductValueControl.value + this.GetProductJsonValue(items[i]);
            i++;
        }
        if (this.ProductValueControl.value.length > 1)
            this.ProductValueControl.value = this.ProductValueControl.value.substr(0, this.ProductValueControl.value.length - 1);
        this.ProductValueControl.value = this.ProductValueControl.value + "]";
    },
    GetProductItems: function () {
        var items = [];
        for (var k = 1; k < this.Sku.ProductContainer.Content.rows.length; k++) {
            var row = this.Sku.ProductContainer.Content.rows[k];
            var item = {};
            var value = this.Base.GetAttribute(row, "Id");
            if (row.style.display == "none" && (value == null || value == ""))
                continue;
            item.Controls = [];
            var inputs = row.getElementsByTagName("input");
            for (var l = 0; l < inputs.length; l++) {
                item.Controls.push(inputs[l]);
            }
            item.Sku = this.Base.GetAttribute(row, "Sku");
            item.Id = this.Base.GetAttribute(row, "Id");
            item.SaveType = row.style.display == "none" && this.Base.GetAttribute(row, "Id") != "" ? "Remove" : "";
            items.push(item);
        }
        return items;
    },
    GetProductJsonValue: function (item) { //得到Json值
        var value = "{Id:'@Id',Sku:'@Sku',SaveType:'@SaveType'";
        value = value.replace("@Sku", item.Sku).replace("@Id", item.Id);
        if (item.SaveType == "") {
            if (item.Id == "") {
                value = value.replace("@SaveType", "Add");
            } else {
                value = value.replace("@SaveType", "Modify");
            }
        } else {
            value = value.replace("@SaveType", item.SaveType);
        }
        for (var i = 0; i < item.Controls.length; i++) {
            var ctrlValue = item.Controls[i].value;
            if (ctrlValue == "on" && item.Controls[i].type == "checkbox")
                ctrlValue = item.Controls[i].checked ? "true" : "false";
            value += "," + item.Controls[i].name + ":'" + ctrlValue + "'";
        }
        value += "},";
        return value;
    }
};

Winner.Publisher.Category = function (containerId,publishButtonId,config) {
    this.Base = new Winner.ClassBase();
    this.MatchTitle = "";
    this.ScrollStep = 215;
    if (config != undefined) {
        this.Base.LoadConfig(this, config);
    }
    this.Container = document.getElementById(containerId);
    this.PublishButton = document.getElementById(publishButtonId);
    this.Node = null; //类目连
    this.CurrentNode = null;
    this.Html = "<div class='button btnleft1' Instance='LeftMoveButton' ></div>" +
        "<div class='container' Instance='Panel'><div class='content' Instance='NodeContainer'></div></div>" +
        "<div class='button btnright1' Instance='RightMoveButton' ></div>" +
        "<div Instance='Loader'  class='loading' style='display:none;'></div>";
    this.NodeHtml = "<input type='text' class='fillter' Instance='Match' /><div Instance='Infos'></div>";
    this.InfoHtml = "<div class='info' CategoryId='@Id' CategoryName='@Name' Pinyin='@Pinyin' Initial='@Initial'" +
        " IsPublish='@IsPublish' HasChild='@HasChild' ImageCount='@ImageCount'><dt class='font'> @Text</dt><dt class='@HasChildClass'></dt></div>";

};
Winner.Publisher.Category.prototype =
{
    Initialize: function () { //初始化
        if (this.Container == null) return;
        this.Container.innerHTML = this.Html;
        this.CurrentNode = null;
        this.Node = null; //类目连
        this.Container.className = "category";
        this.Base.LoadInstances(this, this.Container);
        this.BindMoveEvent(this.LeftMoveButton, -1, "button btnleft1", "button btnleft2", "button btnleft3");
        this.BindMoveEvent(this.RightMoveButton, 1, "button btnright1", "button btnright2", "button btnright3");
    },
    BindMoveEvent: function (sender, movecount, className1, className2, className3) {//绑定按钮事件
        var self = this;
        sender.onmouseover = function () {
            if (sender.className != className1) sender.className = className3;
        };
        sender.onmouseout = function () {
            if (sender.className != className1) sender.className = className2;
        };
        sender.onclick = function () {
            self.Move(movecount);
        };
    },
    Move: function (movecount) {//移动
        var width = this.NodeContainer.clientWidth;
        var step = this.ScrollStep * movecount;
        if (this.Panel.scrollLeft + step >= 0 && this.Panel.scrollLeft + step <= width)
            this.Panel.scrollLeft = this.Panel.scrollLeft + step;
        this.SetMoveClassName(width);
    },
    SetMoveClassName: function (width) {//设置移动的样式
        if (this.Panel.scrollLeft == 0) this.LeftMoveButton.className = "button btnleft1";
        else this.LeftMoveButton.className = "button btnleft2";
        if (this.Panel.scrollLeft + this.Panel.clientWidth >= width) this.RightMoveButton.className = "button btnright1";
        else this.RightMoveButton.className = "button btnright2";
    },
    GetShowNodeCount: function (node) {//得到显示个数
        if (node == null) return 0;
        for (var i = 0; i < node.Children.length; i++) {
            if (node.Children[i].Container.style.display != "none")
                return 1 + this.GetShowNodeCount(node.Children[i]);
        }
        return 1;
    },
    SetNodeContainer: function () {//设置节点容器宽度
        var nodeCount = this.GetShowNodeCount(this.Node);
        var moveCount = nodeCount - this.Panel.offsetWidth / this.Node.Container.offsetWidth;
        this.NodeContainer.style.width = this.Node.Container.offsetWidth * nodeCount + "px";
        this.Move(moveCount);
    },
    Load: function (node) {//加载类目
        this.BeginLoad(node);
        if (node == null) node = this.Node;
        this.LoadNode(node);
        this.CurrentNode = node;
        this.EndLoad(node);
    },
    LoadNode: function (node) {//加载节点
        if (node == null) this.AddNode(node, "");
        else if (node.Info != null) {
            var parentId = this.Base.GetAttribute(node.Info, "CategoryId");
            var hasChild = this.Base.GetAttribute(node.Info, "HasChild");
            if (hasChild == "true" && this.GetNode(node.Children, parentId) == null) this.AddNode(node, parentId);
        }
    },
    BeginLoad: function (node) {//开始加载
        this.Loader.style.display = "";
        var hideNode = this.GetHideNode(node);
        this.HideChildren(hideNode);
        if (node != null && node.Info != null) node.Info.className = "info";
    },
    EndLoad: function (node) { //结束加载
        this.Loader.style.display = "none";
        if (node == null || node.Info == null) return;
        if (this.Base.GetAttribute(node.Info, "IsPublish") == "true") this.PublishButton.disabled = "";
        else this.PublishButton.disabled = "disabled";
        node.SelectId = this.Base.GetAttribute(node.Info, "CategoryId");
        this.ShowChildren(node, node.SelectId);
        node.Info.className = "info select";
        this.Base.SetAttribute(node.Info, "IsSelect", "true");
        this.SetNodeContainer();
    },
    ClearSelectInfo: function (node) {//清除选择项
        var func = this.Base.GetAttribute;
        var divs = node.Infos.getElementsByTagName('div');
        for (var i = 0; i < divs.length; i++) {
            if (func(divs[i], "CategoryId") == null) continue;
            divs[i].className = "info";
            this.Base.SetAttribute(divs[i], "IsSelect", "false");
        }
    },
    GetNode: function (nodes, parentId) {//得到类目信息
        for (var i = 0; i < nodes.length; i++) {
            if (nodes[i].ParentId == parentId)
                return nodes[i];
        }
        return null;
    },
    ShowChildren: function (node, parentId) {//显示子类目
        for (var i = node.Children.length - 1; i >= 0; i--) {
            if (node.Children[i].ParentId == parentId) {
                node.Children[i].Container.style.display = "";
                break;
            }
        }
    },
    GetHideNode: function (node) {//得到隐藏的开始节点
        if (this.CurrentNode == null) return null;
        var nd = this.CurrentNode;
        while (nd != null) {
            if (nd == node) return nd;
            nd = nd.Parent;
        }
        return null;
    },
    HideChildren: function (node) {//隐藏子类目
        if (node == null) return;
        this.ClearSelectInfo(node);
        for (var i = 0; i < node.Children.length; i++) {
            node.Children[i].Container.style.display = "none";
            this.HideChildren(node.Children[i]);
        }
    },
    AddNode: function (node, parentId) {//添加类目
        var infos = this.GetInfos(parentId);
        var child = this.CreateNode(infos, parentId);
        child.Parent = node;
        var divs = child.Infos.getElementsByTagName('div');
        this.BindMatchEvent(child.Match, divs);
        this.BindInfosEvent(child, divs);
        this.NodeContainer.appendChild(child.Container);
        if (node != null) node.Children.push(child);
        else this.Node = child;
        return child;
    },
    CreateNode: function (infos, parentId) {//创建类目节点
        var node = {};
        node.Container = document.createElement("div");
        node.Container.innerHTML = this.NodeHtml;
        node.Container.className = "node";
        this.Base.LoadInstances(node, node.Container);
        node.Infos.innerHTML = this.GetInfosHtml(infos);
        node.Children = [];
        node.ParentId = parentId;
        return node;
    },
    GetInfosHtml: function (infos) {//创建类目列表
        var rev = "";
        for (var i = 0; i < infos.length; i++) {
            rev += this.InfoHtml.replace("@Id", infos[i].Id)
                .replace("@Name", infos[i].Name).replace("@Text", infos[i].Name)
                .replace("@Pinyin", infos[i].Pinyin).replace("@Initial", infos[i].Initial).replace("@IsPublish", infos[i].IsPublish)
                .replace("@HasChild", infos[i].HasChild).replace("@ImageCount", infos[i].ImageCount)
                .replace("@HasChildClass", infos[i].HasChild ? "hasimg" : "");
        }
        return rev;
    },
    BindMatchEvent: function (match, infos) {//绑定匹配事件
        match.value = this.MatchTitle;
        var self = this;
        match.onfocus = function () {
            if (match.value == self.MatchTitle) match.value = "";
        };
        match.onblur = function () {
            if (match.value == "") match.value = self.MatchTitle;
        };
        match.onkeyup = function () {
            self.Match(infos, match.value.toLowerCase());
        };
    },
    Match: function (infos, value) {//匹配类目
        var func = this.Base.GetAttribute;
        for (var i = 0; i < infos.length; i++) {
            if (func(infos[i], "CategoryId") == null) continue;
            if (value != "" && (func(infos[i], "CategoryName").indexOf(value) > -1 || func(infos[i], "Pinyin").indexOf(value) > -1 || func(infos[i], "Initial").indexOf(value) > -1)) {
                if (infos[i].className.indexOf(" match") == -1)
                    infos[i].className = infos[i].className + " match";
            } else
                infos[i].className = infos[i].className.replace(" match", "");
        }
    },
    BindInfosEvent: function (node, infos) {
        var func = this.Base.GetAttribute;
        for (var i = 0; i < infos.length; i++) {
            if (func(infos[i], "CategoryId") == null) continue;
            this.BindInfoEvent(node, infos[i]);
        }
    },
    BindInfoEvent: function (node, info) {//绑定点击事件
        var self = this;
        info.onclick = function () {
            node.Info = info;
            self.Load(node, info);
        };
        info.onmouseover = function () {
            if (info.className == "info") info.className = "info over";
        };
        info.onmouseout = function () {
            if (info.className == "info over") info.className = "info";
        };
    },
    Reset: function (branch) {//重置类目
        this.Load(null);
        if (this.CurrentNode == null) this.CurrentNode = this.Node;
        var node = this.CurrentNode;
        for (var i = 0; i < branch.length; i++) {
            this.SetResetInfo(node, branch[i]);
            for (var j = 0; j < node.Children.length; j++) {
                if (node.Children[j].ParentId == branch[i]) {
                    node = node.Children[j];
                }
            }
        }
    },
    SetResetInfo: function (node, id) {//重置类目的选择
        var func = this.Base.GetAttribute;
        var infos = node.Infos.getElementsByTagName("div");
        for (var i = 0; i < infos.length; i++) {
            if (func(infos[i], "CategoryId") != id) continue;
            node.Info = infos[i];
            this.Load(node);
            break;
        }
    },
    GetBranch: function (branch, node) {//得到节点链
        if (branch == null) branch = [];
        if (node == null) node = this.Node;
        if (node == null) return branch;
        if (node.SelectId == null || this.Base.GetAttribute(node.Info, "IsSelect") != "true") return branch;
        branch.push(node.SelectId);
        for (var i = 0; i < node.Children.length; i++) {
            if (node.Children[i].Container.style.display == "none") continue;
            this.GetBranch(branch, node.Children[i]);
        }
        return branch;
    },
    GetInfos: function (parentId) {//得到类目信息接口
        return parentId; //[{Id:"",Name:"",Pinyin:"",Initial:"",IsPublish:"",HasChild:false,ImageCount:0}]
    }
};

Winner.Publisher.Property = function (containerId, config) {
    this.Container = document.getElementById(containerId);
    this.Base = new Winner.ClassBase();
    this.IsValidation = true;
    this.Titles = { Select: "", Custom: "", AddCustom: "", RemoveCustom: "" };
    if (config != undefined) {
        this.Base.LoadConfig(this, config);
    }
    this.InputHtml = "<input type='text' Id='' name='@PropertyId' value='@Value'/>";
    this.SelectHtml = "<select Id='' name='@PropertyId'>@Values</select>";
    this.CheckHtml = "<div class='ckparent'><input type='checkbox' Id='' name='@PropertyId' value='@Value' /><label>@Value</label></div>";
    this.ComboHtml = "<div class='cbparent' Instance='Container'><input type='text' Instance='Input' name='@PropertyId' class='cbinput' autocomplete = 'off' /><div Instance='List' class='listbody' style='display:none;'>@Values</div><div Instance='Down' class='down'></div></div>&nbsp;";
    this.PropertyHtml = "<div class='nickname'>@Name</div>" +
        "<div class='info' Instance='Content'><span Instance='Control' class='control'></span>" +
        "<span Instance='Message' class='showmess'>@Message</span></div>";
    this.Properties = [];
    this.Initialize();
};
Winner.Publisher.Property.prototype =
{
    Initialize: function () { //初始化
        if (this.Container == null) return;
        this.Container.className = "property";
    },
    Load: function (categoryId, tempInfos) {//加载
        this.Container.innerHTML = "";
        this.Properties = [];
        var infos = this.GetInfos(categoryId);
        if (infos == null || infos.length == 0) return;
        if (tempInfos != undefined && tempInfos.length > 0) {
            for (var j = 0; j < tempInfos.length; j++) {
                infos.push(tempInfos[j]);
            }
        }
        for (var i = 0; i < infos.length; i++) {
            this.Create(infos[i]);
        }
    },
    Create: function (info) {//创建属性
        var html = this.GetHtml(info);
        if (html == "") return;
        var ctrl = document.createElement("div");
        ctrl.innerHTML = this.PropertyHtml.replace("@Message", info.Message).replace("@Name", info.Name);
        var property = { CurrentCustomCount: 0 };
        property.Container = ctrl;
        property.Container.className = "element";
        property.Info = info;
        this.Properties.push(property);
        this.Base.LoadInstances(property.Container, ctrl);
        property.Container.Control.innerHTML = html;
        this.BindControlEvent(property);
        this.Container.appendChild(ctrl);
        this.SetDefaultValue(property);
    },
    GetHtml: function (info) {
        var html = "";
        switch (info.Type) {
            case "Input":
                html = this.GetInputHtml(info); break;
            case "Select":
                html = this.GetSelectHtml(info); break;
            case "Check":
                html = this.GetCheckHtml(info); break;
            case "Combo":
                html = this.GetComboHtml(info); break;
        }
        return html;
    },
    GetInputHtml: function (info) {//得到输入框
        var value = info.Values == null || info.Values.length == 0 ? "" : info.Values.join("");
        return this.InputHtml.replace("@Value", value).replace("@PropertyId", info.Id);
    },
    GetSelectHtml: function (info) { //得到选择框
        var valueHtml = "<option value=''>" + this.Titles.Select + "</option>";
        if (info.Values != null && info.Values.length > 0) {
            for (var i = 0; i < info.Values.length; i++) {
                valueHtml += "<option value='" + info.Values[i] + "'>" + info.Values[i] + "</option>";
            }
        }
        var html = this.SelectHtml.replace("@Values", valueHtml);
        return html.replace("@PropertyId", info.Id);
    },
    GetCheckHtml: function (info) {//得到多选框
        var html = "";
        if (info.Values != null && info.Values.length > 0) {
            for (var i = 0; i < info.Values.length; i++) {
                html += this.CheckHtml.replace("@PropertyId", info.Id)
                    .replace("@Value", info.Values[i]).replace("@Value", info.Values[i]);
            }
        }
        return html;
    },
    GetComboHtml: function (info) {//得到多选框
        var valueHtml = "";
        if (info.Values != null && info.Values.length > 0) {
            for (var i = 0; i < info.Values.length; i++) {
                valueHtml += "<div class='ditem'>" + info.Values[i] + "</div>";
            }
        }
        var html = this.ComboHtml.replace("@Values", valueHtml);
        return html.replace("@PropertyId", info.Id);
    },
    BindControlEvent: function (property) {//绑定事件
        switch (property.Info.Type) {
            case "Input":
                this.BindInputEvent(property); break;
            case "Select":
                this.BindSelectEvent(property); break;
            case "Check":
                this.BindCheckEvent(property); break;
            case "Combo":
                this.BindComboEvent(property); break;
        }
    },
    BindInputEvent: function (property) {//输入框事件
        var self = this;
        var input = property.Container.Control.getElementsByTagName("input")[0];
        input.onblur = function () {
            self.ValidateHandler(property);
        };
    },
    BindSelectEvent: function (property) {//选择事件
        var self = this;
        var select = property.Container.Control.getElementsByTagName("select")[0];
        select.onchange = function () {
            self.ValidateHandler(property);
        };
    },
    BindCheckEvent: function (property) {//复选框事件
        var self = this;
        var checks = property.Container.Control.getElementsByTagName("input");
        var func = function (ctrl) {
            ctrl.onclick = function () {
                self.ValidateHandler(property);
            };
        };
        for (var i = 0; i < checks.length; i++) {
            if (checks[i].type != "checkbox" || checks[i].name != property.Info.Id) continue;
            func(checks[i]);
        }
    },
    BindComboEvent: function (property) {//绑定组合框
        var ctrl = [];
        this.Base.LoadInstances(ctrl, property.Container.Control);
        var cb = new Winner.Publisher.ComboBox(ctrl.Container, ctrl.Input, ctrl.List, ctrl.Down);
        cb.Initialize();
        var self = this;
        var input = property.Container.Control.getElementsByTagName("input")[0];
        input.onfocus = function () {
            input.IsFocus = true;
        };
        this.Base.BindEvent(document, "click", function () {
            if (input.IsFocus) {
                self.ValidateHandler(property);
            }
            input.IsFocus = false;
        });
        this.Base.BindEvent(ctrl.Down, "click", function (event) {
            return self.Base.CancelEventUp(event);
        });
    },
    ValidateHandler: function (property) {
        return this.InvokeValidate(property);
    },
    GetControlValue: function (property) {//得到控件值
        switch (property.Info.Type) {
            case "Input":
                return this.GetInputControlValue(property);
            case "Select":
                return this.GetSelectControlValue(property);
            case "Check":
                return this.GetCheckControlValue(property);
            case "Combo":
                return this.GetComboControlValue(property);
        }
        return "";
    },
    GetInputControlValue: function (property) {//得到输入框的值
        return property.Container.Control.getElementsByTagName("input")[0].value;
    },
    GetSelectControlValue: function (property) {//得到选择框的值
        return property.Container.Control.getElementsByTagName("select")[0].value;
    },
    GetCheckControlValue: function (property) {//得到复选框的值
        var checks = property.Container.Control.getElementsByTagName("input");
        var value = [];
        for (var i = 0; i < checks.length; i++) {
            if (checks[i].type == "checkbox" && checks[i].name == property.Info.Id && checks[i].checked)
                value.push(checks[i].value);
        }
        return value.join(",");
    },
    GetComboControlValue: function (property) {//得到组合框的值
        return property.Container.Control.getElementsByTagName("input")[0].value;
    },
    InvokeValidate: function (property) {//调用验证
        var value = this.GetControlValue(property);
        var rev = this.Validate(property, value, property.Info.Rules);
        return rev;
    },
    Validate: function (property, value, rules) {//根据正则表达式验证函数
        if (rules == null || rules.length == 0)
            return true;
        for (var i = 0; i < rules.length; i++) {
            if (!this.ValidateRule(property, rules[i], value))
                return false;
        }
        return true;
    },
    ValidateRule: function (property, rule, value) {//验证
        var rev = true;
        if (rule.IsRange) {
            if (typeof value == 'string') {
                var reg = new RegExp("[^\\d]", "g");
                value = value.replace(reg, "");
                value = parseFloat(value);
            }
            var values = rule.Pattern.split('-');
            if (values.length > 0)
                rev = value >= parseFloat(values[0]);
            if (rev && values.length > 1)
                rev = value <= parseFloat(values[1]);
        } else {
            var rg = new RegExp(rule.Pattern, rule.Options);
            rev = rg.test(value);
        }
        var message = rule.Message;
        if (message == null || message == "")
            message = property.Info.Message;
        this.ValidateResult(property.Container.Control, property.Container.Message, message, rev);
        return rev;
    },
    ValidateResult: function (ctrl, messageCtrl, message, rev) {
        var errclass = " ctrlerror";
        var succlass = " ctrlsucess";
        ctrl.className = ctrl.className != null ? ctrl.className.replace(errclass, "").replace(succlass, "") : "";
        ctrl.className = ctrl.className + (rev ? succlass : errclass);
        messageCtrl.innerHTML = rev ? "&nbsp;&nbsp;&nbsp;&nbsp;" : message;
        messageCtrl.className = rev ? "sucessmess" : "errormess";
    },
    ValidateSubmit: function () {//提交验证
        var rev = true;
        for (var i = 0; i < this.Properties.length; ) {
            rev = this.InvokeValidate(this.Properties[i]) && rev;
            for (var j = 0; j < this.Properties[i].Container.Content.childNodes.length; j++) {
                var node = this.Properties[i].Container.Content.childNodes[j];
                if (node.CustomInput == null) continue;
                rev = this.Validate(node.CustomInput.value, this.Properties[i].Info.Rules) && rev;
                this.ValidateResult(node, this.Properties[i].Container.Message, this.Properties[i].Info.Message, rev);
            }
            i++;
        }
        return rev;
    },
    //设置默认值
    SetDefaultValue: function (property) {//设置默认值
        if (property.Info.DefaultValues == null || property.Info.DefaultValues.length == 0) return;
        switch (property.Info.Type) {
            case "Input":
                this.SetInputDefaultValue(property); break;
            case "Select":
                this.SetSelectDefaultValue(property); break;
            case "Check":
                this.SetCheckDefaultValue(property); break;
            case "Combo":
                this.SetComboDefaultValue(property); break;
        }
    },
    SetInputDefaultValue: function (property) { //设置输入框默认值
        var input = property.Container.Control.getElementsByTagName("input")[0];
        input.value = property.Info.DefaultValues[0].Value;
        this.Base.SetAttribute(input, "Id", property.Info.DefaultValues[0].Id);
    },
    SetSelectDefaultValue: function (property) {//设置选择框默认值
        var select = property.Container.Control.getElementsByTagName("select")[0];
        select.value = property.Info.DefaultValues[0].Value;
        this.Base.SetAttribute(select, "Id", property.Info.DefaultValues[0].Id);
    },
    SetCheckDefaultValue: function (property) {//设置多选框默认值
        var checks = property.Container.Control.getElementsByTagName("input");
        for (var i = 0; i < checks.length; i++) {
            if (checks[i].type != "checkbox" || checks[i].name != property.Info.Id) continue;
            for (var j = 0; j < property.Info.DefaultValues.length; j++) {
                if (checks[i].value == property.Info.DefaultValues[j].Value) {
                    checks[i].checked = true;
                    this.Base.SetAttribute(checks[i], "Id", property.Info.DefaultValues[j].Id);
                    break;
                }
            }
        }
    },
    SetComboDefaultValue: function (property) {//设置自定义属性默认值
        var input = property.Container.Control.getElementsByTagName("input")[0];
        input.value = property.Info.DefaultValues[0].Value;
        this.Base.SetAttribute(input, "Id", property.Info.DefaultValues[0].Id);
    },
    ReplaceAll: function (source, oldString, newString) {//替换所有
        var reg = new RegExp("\\" + oldString, "g");
        return source.replace(reg, newString);
    },
    GetInfos: function (categoryId) {//得到数据接口
        return categoryId; //[{Id:"",Name:"",Type:"",Values:[],Message:"",Rules:[Pattern:"",Options:""],CustomCount:0,DefaultValues:[{Id:'',Value:'',IsCustomer:false,Count:0,Price:0}]}]
    }
};
Winner.Publisher.Sku = function (containerId, productContainerId, imageContainerId, selectSkuContainerId, imageConfig, config) {
    this.ImageConfig = imageConfig;
    this.Base = new Winner.ClassBase();
    this.Titles = { PropertyTitle: "", RemoveTitle: "", Image: { Title: "" }, Product: { Price: "", Cost: "", Count: "", OrderMinCount: "" }, Errors: { ImageCountOver: "", ProductCountOver: ""} };
    this.Container = document.getElementById(containerId);
    this.ImageContainer = document.getElementById(imageContainerId);
    this.ProductContainer = document.getElementById(productContainerId);
    this.SelectSkuContainer = document.getElementById(selectSkuContainerId);
    if (config != undefined) {
        this.Base.LoadConfig(this, config);
    }
    this.SelectSkuHtml = "<input  type='checkbox' value='@PropertyId' @IsSelect /><label class='lable' >@PropertyName</label>";
    this.CheckHtml = "<div class='ckparent'><input Id='' name='@PropertyId' type='checkbox' value='@Value' /><label class='lable' @LabelDisabled >@Value</label>" +
        "<input PropertyId='@PropertyId' PropertyName='@PropertyName' type='input' @PropertyDisabled class='input' maxlength='10'  Value='@Value' /></div>";
    this.PropertyHtml = "<div class='nickname'>@Name</div>" +
        "<div class='info' Instance='Content'><span Instance='Control' class='control'>@Control</span></div>";
    this.CustomHtml = "<input Instance='CustomCheckbox' name='@PropertyId' IsCustom='true' type='checkbox' /><input name='@PropertyId'  PropertyId='@PropertyId' PropertyName='@PropertyName'  IsCustom='true' type='input'  maxlength='50'  class='input' value=''/>";
    this.ImageHtml = "<table class='tb' Instance='Content'><tr><th class='per'>" + this.Titles.PropertyTitle + "</th><th class='img'>" + this.Titles.Image.Title + "</th></tr></table>";
    this.ProductHtml = "<table class='tb' Instance='Content'><tr><th class='per'>" + this.Titles.PropertyTitle + "</th><th>" +
        this.Titles.Product.Price + "</th><th>" + this.Titles.Product.Cost + "</th><th>" + this.Titles.Product.Count + "</th><th>" + this.Titles.Product.OrderMinCount + "</th><th>" + this.Titles.Product.OrderStepCount + "</th><th>"
        + this.Titles.Product.DataId + "</th><th>" + this.Titles.Product.DepositRate + "</th><th>" + this.Titles.Product.IsReturn + "</th><th>" + this.Titles.Product.IsCustom
        + "</th><th>" + this.Titles.Product.IsSales + "</th></tr></table>";
    this.ProductModelHtml = "<td><span><input type='text' name='Name'  value='@Name'/></span></td><td><input type='hidden' name='id'  value='@Id'/><input type='text' name='Price' maxlength='12' value='@Price'/></td>" +
        "<td><input type='text'maxlength='9' name='Cost' value='@Cost'/></td><td><input type='text' name='Count' maxlength='9' value='@Count'/></td>" +
        "<td><input type='text' name='OrderMinCount' maxlength='9' value='@OrderMinCount'/></td><td><input type='text' name='OrderStepCount' maxlength='9' value='@OrderStepCount'/></td>" +
        "<td><input type='text' name='DataId' maxlength='100' value='@DataId'/></td><td><input type='text' name='DepositRate' maxlength='100' value='@DepositRate'/></td>" +
        "<td><input type='checkbox' name='IsReturn' @IsReturn/></td><td><input type='checkbox' name='IsCustom' @IsCustom/></td><td><input type='checkbox' name='IsSales' @IsSales/></td>";
    this.Properties = [];
    this.Initialize();
};
Winner.Publisher.Sku.prototype = {
    Initialize: function () { //初始化
        if (this.Container == null) return;
        this.Container.className = "sku";
    },
    Load: function (categoryId, imageCount, propertyInstance) {//加载
        this.ImageCount = imageCount;
        this.CategoryId = categoryId;
        this.SelectSkuInfos = this.GetInfos(categoryId);
        this.CreateSelectSku(propertyInstance);
        this.ReLoad();
    },
    CreateSelectSku: function (propertyInstance) {//创建选择的SKU
        var html = this.GetSelectSkuHtml();
        this.SelectSkuContainer.innerHTML = html;
        var checks = this.SelectSkuContainer.getElementsByTagName("input");
        var self = this;
        for (var i = 0; i < checks.length; i++) {
            if (checks[i].type == "checkbox") {
                checks[i].onclick = function () {
                    self.ReLoad();
                    if (propertyInstance != null) {
                        propertyInstance.Load(self.CategoryId, self.GetUnSelectSkuInfos());
                    }
                };
            }
        }

    },
    GetSelectSkuHtml: function () {//得到HTML代码
        var html = "";
        for (var i = 0; i < this.SelectSkuInfos.length; i++) {
            var value = this.ReplaceAll(this.SelectSkuHtml, "@PropertyId", this.SelectSkuInfos[i].Id);
            value = this.ReplaceAll(value, "@PropertyName", this.SelectSkuInfos[i].Name);
            if (this.SelectSkuInfos[i].IsSelect) {
                value = this.ReplaceAll(value, "@IsSelect", "checked='checked'");
            } else {
                value = this.ReplaceAll(value, "@IsSelect", "");
            }
            html += value;
        }
        return html;
    },
    ReLoad: function () {//重置SKU
        var infos = this.GetSelectSkuInfos();
        this.Container.innerHTML = "";
        this.ImageContainer.innerHTML = "";
        this.ProductContainer.innerHTML = "";
        this.Properties = [];
        if (infos != null && infos.length > 0) {
            for (var i = 0; i < infos.length; i++) {
                this.Create(infos[i]);
            }
        }
        this.CreateImageContent();
        this.CreateProductContent();
    },
    GetSelectSkuInfos: function () {//得到选择的SKU
        var infos = [];
        var checks = this.SelectSkuContainer.getElementsByTagName("input");
        for (var i = 0; i < checks.length; i++) {
            if (checks[i].type == "checkbox" && checks[i].checked) {
                for (var j = 0; j < this.SelectSkuInfos.length; j++) {
                    if (parseInt(checks[i].value) == this.SelectSkuInfos[j].Id) {
                        infos.push(this.SelectSkuInfos[j]);
                        break;
                    }
                }
            }
        }
        return infos;
    },
    GetUnSelectSkuInfos: function () {//得到选择的SKU
        var infos = [];
        var checks = this.SelectSkuContainer.getElementsByTagName("input");
        for (var i = 0; i < checks.length; i++) {
            if (checks[i].type == "checkbox" && !checks[i].checked) {
                for (var j = 0; j < this.SelectSkuInfos.length; j++) {
                    if (parseInt(checks[i].value) == this.SelectSkuInfos[j].Id) {
                        infos.push(this.SelectSkuInfos[j]);
                        break;
                    }
                }
            }
        }
        return infos;
    },
    Create: function (info) {//创建属性
        var property = { CurrentCustomCount: 0 };
        var html = this.GetHtml(info);
        var ctrl = document.createElement("div");
        ctrl.innerHTML = this.PropertyHtml.replace("@Name", info.Name);
        ctrl.className = "element";
        property.Container = ctrl;
        property.Info = info;
        this.Properties.push(property);
        this.Base.LoadInstances(property.Container, ctrl);
        property.Container.Control.innerHTML = html;
        this.BindControlEvent(property);
        this.Container.appendChild(ctrl);
        do {
            var t = this.CreateCustom(property);
        } while (t != null);
        this.SetDefaultValue(property);

    },
    //创建图片容器
    CreateImageContent: function () {
        this.ImageContainer.innerHTML = this.ImageHtml;
        this.ImageContainer.className = "skuimage";
        this.Base.LoadInstances(this.ImageContainer, this.ImageContainer);
        var infos = this.GetImageInfos();
        this.ImageInfos = infos;
        var skusArry = [];
        var checkArr = this.GetCheckboxs();
        for (var i = 0; i < checkArr.length; i++) {
            var skus = this.GetSkus(checkArr[i]);
            var skusIndex = this.GetSkusIndex(checkArr[i]);
            var value = this.Base.Serialize(skus);
            for (var j = 0; j < infos.length; j++) {
                if (value == infos[j].Sku) {
                    skusArry.push(infos[j].Sku);
                    break;
                }
            }
            this.CreateImage(skus, skusIndex, infos);
        }
        this.LoadUnsetImages(infos, checkArr, skusArry);
        this.SetImage();
    },
    LoadUnsetImages: function (infos, checkArr, skusArry) {
        if (infos != null) {
            for (var j = 0; j < infos.length; j++) {
                var dataSkus = this.Base.Deserialize(infos[j].Sku);
                var skusIndex = "";
                for (var i = 0; i < checkArr.length; i++) {
                    var skus = this.GetSkus(checkArr[i]);
                    var value = this.Base.Serialize(skus);
                    if (value == infos[j].Sku) {
                        skusIndex = this.GetSkusIndex(checkArr[i]);
                        break;
                    }
                }
                if (skusArry.indexOf(infos[j].Sku) == -1 || skusIndex == "") {
                    skusArry.push(infos[j].Sku);
                    this.CreateImage(dataSkus, skusIndex, infos);
                }

            }
        }
    },
    CreateImage: function (skus, skusIndex, infos) { //创建图片
        var tr = this.ImageContainer.Content.insertRow(-1);
        var value = this.Base.Serialize(skus);
        this.Base.SetAttribute(tr, "Sku", value);
        this.Base.SetAttribute(tr, "SkuIndex", skusIndex);
        var td = tr.insertCell(-1);
        td.innerHTML = "<span>" + this.GetSkusValue(skus) + "</span>";
        this.CreateRemoveButton(td);
        var td1 = tr.insertCell(-1);
        var image = new Winner.Publisher.Image("", this.ImageConfig);
        image.Container = td1;
        var self = this;
        image.GetInfos = function () {
            if (infos == null)
                return null;
            var temps = [];
            for (var i = 0; i < infos.length; i++) {
                var dataSkus = self.Base.Deserialize(infos[i].Sku);
                var isAdd = self.MatchSkus(skus, dataSkus);
                if (isAdd) {
                    temps.push(infos[i]);
                }
            }
            return temps;
        };
        image.Load(this.ImageCount);
        var files = tr.getElementsByTagName("input");
        for (var k = 0; k < files.length; k++) {
            this.Base.SetAttribute(files[k], "Sku", value);
        }
        return tr;
    },
    CreateRemoveButton: function (td) {//创建删除按钮
        var a = document.createElement("a");
        a.href = "javascript:void(0);";
        a.innerHTML = this.Titles.RemoveTitle;
        this.Base.BindEvent(a, "click", function () {
            td.parentNode.style.display = "none";
        });
        td.insertBefore(a, td.childNodes[0]);
    },

    //创建产品容器
    CreateProductContent: function () {
        this.ProductContainer.innerHTML = this.ProductHtml;
        this.ProductContainer.className = "skuproduct";
        this.Base.LoadInstances(this.ProductContainer, this.ProductContainer);
        var infos = this.GetProductInfos();
        var skusArry = [];
        var checkArr = this.GetCheckboxs();
        for (var i = 0; i < checkArr.length; i++) {
            var skus = this.GetSkus(checkArr[i]);
            var skusIndex = this.GetSkusIndex(checkArr[i]);
            var info = null;
            var value = this.Base.Serialize(skus);
            for (var j = 0; j < infos.length; j++) {
                if (value == infos[j].Sku) {
                    skusArry.push(infos[j].Id);
                    info = infos[j];
                    break;
                }
            }
            this.CreateProduct(skus, skusIndex, info);
        }
        this.LoadUnsetProducts(infos, checkArr, skusArry);
        this.SetProduct();
    },
    LoadUnsetProducts: function (infos, checkArr, skusArry) {
        if (infos != null) {
            for (var j = 0; j < infos.length; j++) {
                if (skusArry.indexOf(infos[j].Id) > -1) {
                    continue;
                }
                skusArry.push(infos[j].Id);
                this.CreateProduct(this.Base.Deserialize(infos[j].Sku), "", infos[j]);
            }
        }
    },
    CreateProduct: function (skus, skusIndex, info) { //创建产品
        var tr = this.ProductContainer.Content.insertRow(-1);
        var value = this.Base.Serialize(skus);
        this.Base.SetAttribute(tr, "Sku", value);
        this.Base.SetAttribute(tr, "SkuIndex", skusIndex);
        this.Base.SetAttribute(tr, "Id", info == null ? "" : info.Id);
        tr.innerHTML = this.ReplaceProductModelHtml(this.ProductModelHtml, skus, skusIndex, info);
        this.CreateRemoveButton(tr.cells[0]);
        return tr;
    },
    ReplaceProductModelHtml: function (html, skus, skusIndex, info) {
       return  html.replace("@Name", info == null ? "" : this.GetSkusValue(skus))
            .replace("@Id", info == null ? "" : info.Id).replace("@Count", info == null ? "" : info.Count)
            .replace("@Cost", info == null ? "" : info.Cost).replace("@OrderMinCount", info == null ? "1" : info.OrderMinCount)
            .replace("@OrderStepCount", info == null ? "1" : info.OrderStepCount).replace("@DataId", info == null ? "" : info.DataId)
            .replace("@Price", info == null ? "" : info.Price).replace("@DepositRate", info == null ? "" : info.DepositRate)
            .replace("@IsCustom", info != null && info.IsCustom ? "checked='checked'" : "")
            .replace("@IsReturn", info != null && info.IsReturn ? "checked='checked'" : "")
            .replace("@IsSales", info != null && info.IsSales ? "checked='checked'" : "");
    },
    //得到HTML代码
    GetHtml: function (info) {//得到HTML代码
        var html = "";
        if (info.Values != null && info.Values.length > 0) {
            for (var i = 0; i < info.Values.length; i++) {
                var value = this.ReplaceAll(this.CheckHtml, "@PropertyId", info.Id);
                value = this.ReplaceAll(value, "@Value", info.Values[i]);
                value = this.ReplaceAll(value, "@PropertyName", info.Name);
                if (info.IsAllowEdit) {
                    value = this.ReplaceAll(value, "@PropertyDisabled", "");
                    value = this.ReplaceAll(value, "@LabelDisabled", "style='display:none;'");
                } else {
                    value = this.ReplaceAll(value, "@PropertyDisabled", "disabled='Disabled' style='display:none;'");
                    value = this.ReplaceAll(value, "@LabelDisabled", "");
                }
                html += value;
            }
        }
        return html;
    },
    //自定义控件
    CreateCustom: function (property) {//创建自定义属性
        if (property.Info.CustomCount != undefined && property.CurrentCustomCount >= property.Info.CustomCount)
            return null;
        var html = this.ReplaceAll(this.CustomHtml, "@PropertyId", property.Info.Id);
        html = this.ReplaceAll(html, "@PropertyName", property.Info.Name);
        var ctrl = document.createElement("div");
        ctrl.innerHTML = html;
        ctrl.className = "ckparent";
        property.Container.Control.appendChild(ctrl);
        this.Base.LoadInstances(ctrl, ctrl);
        var checks = ctrl.getElementsByTagName("input");
        for (var i = 0; i < checks.length; i++) {
            this.BindCheckboxEvent(checks[i], property, "c" + property.CurrentCustomCount);
        }
        property.CurrentCustomCount = property.CurrentCustomCount + 1;
        return ctrl;
    },
    //绑定控件事件
    BindControlEvent: function (property) {//绑定事件
        var checks = property.Container.Control.getElementsByTagName("input");
        for (var i = 0; i < checks.length; i++) {
            this.BindCheckboxEvent(checks[i], property, i);
        }
    },
    BindCheckboxEvent: function (ctrl, property, index) {//绑定控件事件
        var self = this;
        if (ctrl.type == "checkbox") {
            this.Base.SetAttribute(ctrl, "Index", index);
            ctrl.onclick = function () {
                self.SetImage();
                self.SetProduct();
            };
        }
        else if (ctrl.type == "text") {
            this.Base.BindEvent(ctrl, "keyup", function () {
                self.SetImage();
                self.SetProduct();
            });
        }
    },

    //设置默认值
    SetDefaultValue: function (property) {//设置默认值
        if (property.Info.DefaultValues == null || property.Info.DefaultValues.length == 0) return;
        var hasValues = this.SetCheckDefaultValue(property);
        this.SetCustomDefaultValue(property, hasValues);
    },
    SetCheckDefaultValue: function (property) {//设置多选框默认值
        var hasValues = [];
        var inputs = property.Container.Control.getElementsByTagName("input");
        for (var i = 0; i < inputs.length; i += 2) {
            if (inputs[i].name != property.Info.Id) continue;
            for (var j = 0; j < property.Info.DefaultValues.length; j++) {
                if (inputs[i].value == property.Info.DefaultValues[j].Value) {
                    if (inputs[i].type == "checkbox") {
                        inputs[i].checked = true;
                        hasValues.push(property.Info.DefaultValues[j].Value);
                    }
                    if (inputs[i + 1].type == "text") {
                        inputs[i + 1].value = property.Info.DefaultValues[j].Value;
                        this.Base.SetAttribute(inputs[i + 1], "Id", property.Info.DefaultValues[j].Id);
                    }
                    break;
                }
            }
        }
        return hasValues;
    },
    SetCustomDefaultValue: function (property, hasValues) { //设置自定义属性默认值
        var inputs = property.Container.Control.getElementsByTagName("input");
        for (var i = 0; i < inputs.length; i += 2) {
            if (inputs[i].name != property.Info.Id || this.Base.GetAttribute(inputs[i], "iscustom") != "true") continue;
            for (var j = 0; j < property.Info.DefaultValues.length; j++) {
                if (hasValues.indexOf(property.Info.DefaultValues[j].Value) > -1) continue;
                hasValues.push(property.Info.DefaultValues[j].Value);
                if (inputs[i].type == "checkbox") {
                    inputs[i].checked = true;
                }
                if (inputs[i + 1].type == "text") {
                    inputs[i + 1].value = property.Info.DefaultValues[j].Value;
                    this.Base.SetAttribute(inputs[i + 1], "Id", property.Info.DefaultValues[j].Id);
                }
                break;
            }
        }
    },
    //设置图片
    SetImage: function () {
        var checkArr = this.GetCheckboxs();
        var tb = this.ImageContainer.Content;
        for (var i = 0; i < checkArr.length; i++) {
            var skus = this.GetSkus(checkArr[i]);
            var value = this.Base.Serialize(skus);
            var valueIndex = this.GetSkusIndex(checkArr[i]);
            var isHas = false;
            for (var l = 1; l < tb.rows.length; ) {
                var rowValueIndex = this.Base.GetAttribute(tb.rows[l], "SkuIndex");
                if (rowValueIndex == valueIndex) {
                    tb.rows[l].style.display = "";
                    var spans = tb.rows[l].cells[0].getElementsByTagName("span");
                    spans[0].innerHTML = this.GetSkusValue(skus);
                    this.Base.SetAttribute(tb.rows[l], "Sku", value);
                    var files = tb.rows[l].getElementsByTagName("input");
                    for (var k = 0; k < files.length; k++) {
                        this.Base.SetAttribute(files[k], "Sku", value);
                    }
                    isHas = true;
                    break;
                }
                l++;
            }
            if (!isHas) {
                this.CreateImage(skus, valueIndex, this.ImageInfos);
            }
        }
        this.HideImage(tb, checkArr);
    },
    HideImage: function (tb, checkArr) {//隐藏
        var isShow = false;
        for (var j = 1; j < tb.rows.length; j++) {
            var rowValueIndex = this.Base.GetAttribute(tb.rows[j], "SkuIndex");
            var isHas = false;
            for (var n = 0; n < checkArr.length; n++) {
                var skusIndex = this.GetSkusIndex(checkArr[n]);
                if (rowValueIndex == skusIndex) {
                    isHas = true;
                }
            }
            if (isHas || rowValueIndex == "") {
                tb.rows[j].style.display = "";
                isShow = true;
            } else {
                tb.rows[j].style.display = "none";
            }
        }
        if (isShow) {
            this.ImageContainer.style.display = "";
        } else {
            this.ImageContainer.style.display = "none";
        }
    },
    //设置产品
    SetProduct: function () {
        var checkArr = this.GetCheckboxs();
        var tb = this.ProductContainer.Content;
        for (var i = 0; i < checkArr.length; i++) {
            var skus = this.GetSkus(checkArr[i]);
            var value = this.Base.Serialize(skus);
            var valueIndex = this.GetSkusIndex(checkArr[i]);
            var isHas = false;
            for (var l = 1; l < tb.rows.length; ) {
                var rowValueIndex = this.Base.GetAttribute(tb.rows[l], "SkuIndex");
                if (rowValueIndex == valueIndex) {
                    tb.rows[l].style.display = "";
                    var inputs = tb.rows[l].cells[0].getElementsByTagName("input");
                    inputs[0].value = this.GetSkusValue(skus);
                    this.Base.SetAttribute(tb.rows[l], "Sku", value);
                    isHas = true;
                    break;
                }
                l++;
            }
            if (!isHas) {
                var tr = this.CreateProduct(skus, valueIndex, null);
                var tinputs = tr.cells[0].getElementsByTagName("input");
                tinputs[0].value = this.GetSkusValue(skus);
            }
        }
        this.HideProduct(tb, checkArr);

    },
    HideProduct: function (tb, checkArr) {//隐藏
        var isShow = false;
        for (var j = 1; j < tb.rows.length; j++) {
            var rowValueIndex = this.Base.GetAttribute(tb.rows[j], "SkuIndex");
            var isHas = false;
            for (var n = 0; n < checkArr.length; n++) {
                var skusInde = this.GetSkusIndex(checkArr[n]);
                if (rowValueIndex == skusInde) {
                    isHas = true;
                }
            }
            if (isHas || rowValueIndex == "") {
                tb.rows[j].style.display = "";
                isShow = true;
            } else {
                tb.rows[j].style.display = "none";
            }
        }
        if (isShow) {
            this.ProductContainer.style.display = "";
        } else {
            this.ProductContainer.style.display = "none";
        }
    },
    //得到属性值
    GetSkus: function (checks) {
        var infos = [];
        for (var i = 0; i < checks.length; i++) {
            var inputs = checks[i].parentNode.getElementsByTagName("input");
            for (var j = 0; j < inputs.length; j++) {
                if (inputs[j].type == "text") {
                    var info = {
                        Id: this.Base.GetAttribute(inputs[j], "PropertyId"),
                        Name: this.Base.GetAttribute(inputs[j], "PropertyName"),
                        Value: inputs[j].value
                    };
                    infos.push(info);
                    break;
                }
            }
        }
        return infos;
    },
    GetSkusIndex: function (checks) {//得到SKU索引
        var infos = [];
        for (var i = 0; i < checks.length; i++) {
            infos.push(this.Base.GetAttribute(checks[i], "Index"));
        }
        return infos.join(",");
    },
    MatchSkus: function (skus, dataSkus) {//匹配SKU
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
    GetSkusValue: function (skus) {//得到SKU值
        var value = [];
        for (var i = 0; i < skus.length; i++) {
            value.push(skus[i].Value);
        }
        return value.join(",");
    },
    //得到CheckBoxs
    GetCheckboxs: function () {
        var checks = [];
        for (var i = 0; i < this.Properties.length; i++) {
            var cks = this.Properties[i].Container.Control.getElementsByTagName("input");
            var temp = [];
            for (var j = 0; j < cks.length; j++) {
                if (cks[j].type != "checkbox" || cks[j].checked == false) continue;
                temp.push(cks[j]);
            }
            checks[i] = temp.splice(0);
        }
        var outChecks = [];
        var tempChecks = [];
        this.Descartes(checks, 0, outChecks, tempChecks);
        return outChecks;
    },
    Descartes: function (checks, index, outChecks, tempChecks) {//填充CheckBoxs
        if (checks == null || checks.length == 0)
            return;
        for (var i = 0; i < checks[index].length; i++) {
            tempChecks.push(checks[index][i]);
            if (index >= checks.length - 1) {
                var t = outChecks.length;
                outChecks[t] = [];
                for (var j = 0; j < tempChecks.length; j++) {
                    outChecks[t].push(tempChecks[j]);
                }
                tempChecks.pop();
            } else {
                this.Descartes(checks, index + 1, outChecks, tempChecks);
            }
        }
        tempChecks.pop();
    },
    ReplaceAll: function (source, oldString, newString) {//替换所有
        var reg = new RegExp("\\" + oldString, "g");
        return source.replace(reg, newString);
    },
    GetInfos: function (categoryId) {//得到数据接口
        return categoryId; //[{Id:"",Name:"",Type:"",Values:[],Message:"",Rules:[Pattern:"",Options:""],CustomCount:0,DefaultValues:[{Id:'',Value:'',IsSelect:true,Count:0,Price:0}]}]
    },
    GetImageInfos: function () {//得到数据接口
        return null; //[{Sku:"",Infos:[Id:"",Value:"",ProductId:""]}]
    },
    GetProductInfos: function () {//得到数据接口
        return null; //[Infos:{Id:"",Price:0,Cost:0,Count:0,OrderMinCount:0,Sku:""}]
    },
    Validate: function () {//验证
        var imageCount = 0;
        var productCount = 0;
        for (var i = 1; i < this.ProductContainer.Content.rows.length; i++) {
            if (this.ProductContainer.Content.rows[i].style.display != "none") {
                productCount += productCount;
            }
        }
        for (var j = 1; i < this.ImageContainer.Content.rows.length; j++) {
            if (this.ImageContainer.Content.rows[j].style.display != "none") {
                imageCount += imageCount;
            }
        }
        var count = this.GetCheckboxs();
        if (imageCount > count) {
            alert(this.ImageCountOverError);
            return false;
        }
        if (productCount > count) {
            alert(this.ProductCountOverError);
            return false;
        }
        return true;
    }
};
Winner.Publisher.Image = function (containerId, config) {
    this.Base = new Winner.ClassBase();
    this.MaxSize = 307200;
    this.Extension = ".jpg.gif.png.bmp.jpeg";
    this.ExtensionErrorMessage = "";
    this.SizeErrorMessage = "";
    this.IsAnsyUpload = true;
    this.AnsyUploadUrl = "";
    if (config != undefined) {
        this.Base.LoadConfig(this, config);
    }
    this.Container = document.getElementById(containerId);
    this.ImagePath = "/scripts/Winner/Publisher/Images/Nopic.jpg";
    this.Html = "<div Instance='Image'><img  src='@Src' alt='' /></div>" +
        "<div class='remove' Instance='RemoveButton' style='display:none'></div>" +
        "<a Instance='MovePreviewButton' href='javascript:void(0);' style='display:none' class='prevbtn movebtn'  ><span></span></a>" +
         "<a Instance='MoveNextButton' href='javascript:void(0);' style='display:none' class='nextbtn movebtn'  ><span></span></a>" +
        "<input Id='@Id' Instance='File' type='file' name='@Name' Sku='@Sku' RealValue='@RealValue' Index='@Index' SaveType='None'/>";
    this.UploadHtml = "<form Instance='Form' action='@Url'" + 
        " method='POST' enctype='multipart/form-data' target='ImageUploader'></form>" +
        "<iframe  Instance='Iframe' name='ImageUploader' style='display:none'></iframe>";
};
Winner.Publisher.Image.prototype = {
    Load: function (count) { //加载
        this.Count = count;
        this.Container.innerHTML = "";
        this.Container.className = "images";
        var infos = this.GetInfos();
        this.Create(infos);
    },
    Create: function (infos) { //创建图片
        Winner.Publisher.ImageIndex = Winner.Publisher.ImageIndex == undefined ? 0 : Winner.Publisher.ImageIndex;
        var count = infos != null && infos.length > this.Count ? infos.length : this.Count;
        for (var i = 0; i < count; i++) {
            var index = (new Date()).valueOf() + Winner.Publisher.ImageIndex;
            Winner.Publisher.ImageIndex++;
            var div = document.createElement('div');
            div.id = 'Image' + index;
            div.className = "image";
            var src = infos != null && i < infos.length ? infos[i].Value : this.ImagePath;
            div.innerHTML = this.Html.replace("@Name", i).replace("@Id", infos != null && i < infos.length ? infos[i].Id : "")
                .replace("@Sku", infos != null && i < infos.length ? infos[i].Sku : "").replace("@Src", src)
                .replace("@Index", index).replace("@Index", index).replace("@RealValue", src == this.ImagePath ? "" : src);
            this.Container.appendChild(div);
            this.Base.LoadInstances(div, div);
            this.BindView(div.File, div.Image);
            this.BindImageEvent(div);
        }
    },
    BindImageEvent: function (div) {//绑定事件
        var self = this;
        this.Base.BindEvent(div, "mouseover", function () {
            if (self.Base.GetAttribute(div.File, "RealValue") != "") {
                div.RemoveButton.style.display = "";
                div.MovePreviewButton.style.display = "";
                div.MoveNextButton.style.display = "";
            } else {
                div.RemoveButton.style.display = "none";
                div.MovePreviewButton.style.display = "none";
                div.MoveNextButton.style.display = "none";
            }
        });
        this.Base.BindEvent(div, "mouseout", function () {
            div.RemoveButton.style.display = "none";
            div.MovePreviewButton.style.display = "none";
            div.MoveNextButton.style.display = "none";
        });
        this.Base.BindEvent(div.RemoveButton, "click", function () {
            self.Remove(div);
        });
        this.Base.BindEvent(div.MovePreviewButton, "click", function () {
            self.Move(div, "Preview");
        });
        this.Base.BindEvent(div.MoveNextButton, "click", function () {
            self.Move(div, "Next");
        });
    },
    Remove: function (div) {//显示删除按钮
        var self = this;
        if (self.Base.GetAttribute(div.File, "Id") == "") {
            self.Base.SetAttribute(div.File, "SaveType", "None");
            this.Base.SetAttribute(div.File, "RealValue", "");
        } else {
            self.Base.SetAttribute(div.File, "SaveType", "Remove");
        }
        this.AddDefaultImage(div.Image);
    },
    Move: function (div, direction) {//移动
        if (direction == "Preview" && div.previousSibling != null) {
            div.parentNode.insertBefore(div, div.previousSibling);
        } else if (direction == "Next" && div.nextSibling != null) {
            if (div.parentNode.lastChild == div.nextSibling) {
                div.parentNode.appendChild(div);
            } else {
                div.parentNode.insertBefore(div, div.nextSibling.nextSibling);
            }
        }
    },
    BindView: function (file, view) { //绑定视图
        this.AddImage(view, this.GetInitializeSrc(view));
        this.BindFileEvent(file, view);
    },
    GetInitializeSrc: function (view) { //得到初始化路径
        var imgs = view.getElementsByTagName('img');
        if (imgs != null && imgs.length > 0 && imgs[0].src != "")
            return imgs[0].src;
        return this.ImagePath;
    },
    AddDefaultImage: function (view) { //设置默认图片
        this.AddImage(view, this.ImagePath);
    },
    AddImage: function (view, src) { //添加img
        view.innerHTML = "";
        var img = document.createElement('img');
        img.alt = "";
        img.src = src;
        view.appendChild(img);
    },
    GetFile: function () { //得到上传控件
        var file = document.createElement('input');
        file.type = "file";
        file.className = "file";
        file.Accept = "image/*";
        file.name = this.Container.id + "file";
        return file;
    },
    BindFileEvent: function (file, view) { //绑定文件选择事件
        var self = this;
        this.Base.BindEvent(file, "change", function () {
            if (file.value == "")
                return;
            if (self.IsAnsyUpload) {
                self.Upload(file);
            } else {
                self.ShowImage(file, view);
                self.Base.SetAttribute(file, "RealValue", file.value);
            }
            if (self.Base.GetAttribute(file, "Id") == "") {
                self.Base.SetAttribute(file, "SaveType", "Add");

            } else {
                self.Base.SetAttribute(file, "SaveType", "Modify");
            }
        });
    },
    Upload: function (file) {
        if (!this.IsAnsyUpload)
            return false;
        if (!this.Validate(file, true)) {
            return false;
        }
        var div = document.createElement("div");
        var index = this.Base.GetAttribute(file, "Index");
        div.id = "imagediv" + index;
        div.style.display = "none";
        var flag = this.AnsyUploadUrl.indexOf("?") > -1 ? "&" : "?";
        var url = this.AnsyUploadUrl + flag + "ctrlid=" + index;
        div.innerHTML = this.UploadHtml.replace("@Url", url);
        document.body.appendChild(div);
        this.Base.LoadInstances(div, div);
        div.Form.appendChild(file);
        div.Form.submit();
        var self = this;
        var func = function () {
            self.ResetUploadFile(index);
        };
        setTimeout(func, 30000);
    },
    UploadSucess: function (index, src) {//上传成功
        this.ResetUploadFile(index);
        var image = document.getElementById("Image" + index);
        var inputs = image.getElementsByTagName("input");
        var imgs = image.getElementsByTagName("img");
        imgs[0].src = src;
        this.Base.SetAttribute(inputs[0], "RealValue", src);
    },
    UploadFailure: function (index, mes) {//上传失败
        alert(mes);
        this.ResetUploadFile(index);
    },
    ResetUploadFile: function (index) { //重置上传
        var div = document.getElementById("imagediv" + index);
        if (div == null)
            return;
        var input = div.childNodes[0].childNodes[0];
        if (input == undefined) {
            document.body.removeChild(div);
            return;
        }
        var image = document.getElementById("Image" + index);
        input.value = "";
        image.appendChild(input);
        document.body.removeChild(div);
    },
    ValidateSubmit: function () { //提交验证
        var files = this.Container.getElementsByTagName("input");
        var rev = true;
        for (var i = 0; i < files.length; i++) {
            if (files[i].type == "file") {
                rev = this.Validate(files[i]) && rev;
            }
        }
        return rev;
    },
    ValidateResult: function (ctrl, rev) {//设置结果
        var errclass = " ctrlerror";
        var succlass = " ctrlsucess";
        ctrl.className = ctrl.className != null ? ctrl.className.replace(errclass, "").replace(succlass, "") : "";
        ctrl.className = ctrl.className + (rev ? succlass : errclass);
    },
    Validate: function (file, isShowMessage) { //验证控件
        var rev = false;
        var extarr = file.value.split('.');
        var ext = extarr[extarr.length - 1].toLowerCase();
        if (this.Extension.indexOf(ext) > -1) {
            rev = true;
        } else {
            if (isShowMessage)
                alert(this.ExtensionErrorMessage);
        }
        if (rev) {
            var sizes = this.Base.GetFileSize(file);
            var size = sizes == null || sizes.length == 0 ? 0 : sizes[0];
            if (size > this.MaxSize) {
                rev = false;
                if (isShowMessage)
                    alert(this.SizeErrorMessage);
            }
        }
        this.ValidateResult(file.parentNode, rev);
        return rev;
    },
    ShowImage: function (file, view) { //展示图形
        if (this.Validate(file, true)) {
            this.SelectViewType(file, view);
            return;
        }
        this.AddDefaultImage(view);
    },
    SelectViewType: function (file, view) { //判断浏览器选择设置显示图片方式
        switch (this.GetBrowserVision()) {
            case "Other":
                this.SetOtherView(file, view);
                break;
            case "IE6":
                this.SetSimpleView(file, view);
                break;
            case "IE":
                this.SetFilterView(file, view);
                break;
        }
    },
    SetOtherView: function (file, view) { //火狐下路径
        try {
            this.AddImage(view, window.URL.createObjectURL(file.files[0]));
        } catch (ex) {
        }
    },
    SetSimpleView: function (file, view) { //设置IE6下的路径
        try {
            this.AddImage(view, file.value);
        } catch (ex) {
        }
    },
    SetFilterView: function (file, view) { //滤镜路径
        try {
            view.innerHTML = "";
            var path = this.GetFilterPath(file);
            view.style.display = "block";
            view.style.filter = "progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod='scale',src=\"" + path + "\");";
        } catch (ex) {
        }
    },
    GetFilterPath: function (file) { //得到滤镜路径
        file.select();
        file.blur();
        try {
            return document.selection.createRange().text;
        } finally {
            document.selection.empty();
        }
    },
    GetBrowserVision: function () { //判断浏览器类型
        if (document.all) {
            if (navigator.userAgent.indexOf("MSIE 6.0") > 0) return "IE6";
            if (navigator.userAgent.indexOf("MSIE 7.0") > 0 || navigator.userAgent.indexOf("MSIE 8.0") > 0
                || navigator.userAgent.indexOf("MSIE 9.0") > 0) return "IE";
            return "Other";
        }
        return "Other";
    },
    GetInfos: function () { //得到数据接口
        return null; //[Id:"",Value:"",Sku:""]
    }
};
Winner.Publisher.ComboBox = function (contaner, input, list, down) {
    this.Base = new Winner.ClassBase();
    this.Input = input;
    this.Container = contaner;
    this.List = list;
    this.Down = down;
};
Winner.Publisher.ComboBox.prototype =
 {
     Initialize: function () { //加载css样式文件
         this.Container.style.position = "relative";
         this.List.style.position = "absolute";
         this.Input.style.position = "absolute";
         this.BindDownEvent(this.Down);
         this.BindInputAutoEvent();
         this.BindInputClickEvent();
         this.BindBodyEvent();
         this.BindItemEvent();
     },

     BindDownEvent: function (down) {//绑定下拉框事件
         var self = this;
         this.Base.BindEvent(down, "click", function (event) {
             self.ShowList("", event);
             return self.Base.CancelEventUp(event);
         });
     },
     BindInputAutoEvent: function () {//绑定输入事件
         var self = this;
         this.Base.BindEvent(this.Input, "keydown", function (event) {
             self.Show(self.Input.value, false, event);
         });
         this.Base.BindEvent(this.Input, "keyup", function (event) {
             self.Show(self.Input.value, true, event);
         });
     },
     BindInputClickEvent: function () {//绑定输入框点击焦点事件
         var self = this;
         this.Base.BindEvent(this.Input, "click", function (event) {
             return self.Base.CancelEventUp(event);
         });
     },
     BindBodyEvent: function () {//绑定页面事件
         var self = this;
         this.Base.BindEvent(document, "click", function () {
             self.HideList();
         });
     },
     BindItemEvent: function () {//绑定Item事件
         for (var i = 0; i < this.List.childNodes.length; i++) {
             this.BindItemOverEvent(this.List.childNodes[i]);
             this.BindItemClickEvent(this.List.childNodes[i]);
         }
     },
     BindItemOverEvent: function (item) {//绑定Item的Over事件
         var self = this;
         this.Base.BindEvent(item, "mouseover", function (event) {
             if (self.List.childNodes != null && self.List.childNodes.length > 0) {
                 for (var i = 0; i < self.List.childNodes.length; i++) {
                     self.List.childNodes[i].className = "out";
                 }
             }
             self.OverItem(item);
             return self.Base.CancelEventUp(event);
         });
         this.Base.BindEvent(item, "mouseout", function (event) {
             return self.Base.CancelEventUp(event);
         });
     },
     BindItemClickEvent: function (item) {//绑定Item的Click事件
         var self = this;
         this.Base.BindEvent(item, "click", function (event) {
             self.SelectItem(item);
             return self.Base.CancelEventUp(event);
         });
     },
     Show: function (value, isShow, event) {
         event = window.event ? window.event : event;
         if (isShow == true) {
             if (event.keyCode == 8 || event.keyCode == 13
                 || event.keyCode == 32 || event.keyCode == 46
                 || event.keyCode >= 48 && event.keyCode <= 111
                 || event.keyCode >= 186 && event.keyCode <= 222)
                 this.ShowList(value);
             return;
         }
         switch (event.keyCode) {
             case 13: this.EnterItem(); break;
             case 38: this.MoveItem(-1); break;
             case 40: this.MoveItem(1); break;
         }
     },
     EnterItem: function () {//键盘选择
         var index = this.GetSelectItemIndex();
         if (index != -1) {
             this.SelectItem(this.List.childNodes[index]);
         }
     },
     MoveItem: function (value) {//移动项
         if (this.List.childNodes == null || this.List.childNodes.length == 0)
             return;
         this.List.style.display = "";
         var index = this.GetSelectItemIndex();
         this.MovePreviousItem(index);
         index = index + value;
         this.MoveNextItem(index, true);
     },
     MovePreviousItem: function (index) {//移除前一个
         if (index != -1) {
             this.List.childNodes[index].className = "out";
         }
     },
     MoveNextItem: function (index, isSelectText) {//选择下一个
         if (index < 0)
             index = this.List.childNodes.length - 1;
         if (index >= this.List.childNodes.length)
             index = 0;
         this.OverItem(this.List.childNodes[index], isSelectText);
     },
     OverItem: function (item, isSelectText) { //移动到当前项
         item.className = "over";
         if (isSelectText) this.SetSelectItem(item);
     },
     GetSelectItemIndex: function () {//得到选择项
         if (this.List.childNodes != null && this.List.childNodes.length > 0) {
             for (var i = 0; i < this.List.childNodes.length; i++) {
                 if (this.List.childNodes[i].className == "over") {
                     return i;
                 }
             }
         }
         return -1;
     },
     ShowList: function (value) {//展示列表
         this.List.style.display = "";
         for (var i = 0; i < this.List.childNodes.length; i++) {
             if (value == "" || this.List.childNodes[i].innerHTML.indexOf(value, 0) > -1) {
                 this.List.childNodes[i].style.display = "";
             } else {
                 this.List.childNodes[i].style.display = "none";
             }
         }
     },
     HideList: function () {
         this.List.style.display = "none";
     },
     SelectItem: function (item) {//选择
         this.Input.focus();
         this.SetSelectItem(item);
         this.HideList();
     },
     SetSelectItem: function (item) {
         this.Input.value = item == null ? "" : item.innerHTML;
     }
 };