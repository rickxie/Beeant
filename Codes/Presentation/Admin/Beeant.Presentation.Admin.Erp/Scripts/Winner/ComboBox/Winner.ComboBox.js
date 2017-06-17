Winner = typeof(Winner)!="undefined" ? Winner : {};
Winner.ComboBox = function (inputId, valueInput, config) {
    this.Base = new Winner.ClassBase();
    this.Input = document.getElementById(inputId);
    this.ValueInput = (valueInput == null || valueInput == "") ? null : document.getElementById(valueInput);
    this.SaveValuePropertyName = "Value";
    this.Container = null;
    this.List = [];
    this.IsAutoSearch = true;
    this.IsSelectedItem = false; //是否移动对象
    this.IsMoveItem = false;//是否移动对象
    this.IsCreateDown = true;
    this.IsCreateContainer = true;
    this.StyleFile = "/scripts/Winner/ComboBox/Styles/Style.css";
    if (config != undefined) {
        this.Base.LoadConfig(this, config);
    }
};
Winner.ComboBox.prototype =
 {
     Initialize: function () { //加载css样式文件
         if (!this.CheckDisabled())
             return;
         this.Base.LoadCssFile(this.StyleFile);
         this.CreateContainer();
         this.SetInput();
         this.CreateDown();
         this.CreateList();
         this.BindEvent();
     },
     CheckDisabled:function() {
         if (this.Input == null || this.Input.disabled == true || this.ValueInput != null && this.ValueInput.disabled == true) {
             this.Input.disabled = "true";
             this.ValueInput.disabled = "true";
             return false;
         }
         return true;
     },
     CreateContainer: function () {//创建容器
         if (this.IsCreateContainer) {
             this.Container = document.createElement('div');
             this.Container.className = "combo";
         }
         this.Container.style.position = "relative";
     },
     CreateList: function () {//创建List
         this.List = document.createElement('div');
         this.List.style.position = "absolute";
         this.List.style.display = "none";
         this.List.className = "listbody";
         this.Container.appendChild(this.List);
     },
     CreateDown: function () {//创建下拉框
         if (!this.IsCreateDown) return;
         var down = document.createElement('div');
         down.className = "down";
         this.BindDownEvent(down);
         this.Container.appendChild(down);
     },
     BindDownEvent: function (down) {//绑定下拉框事件
         var self = this;
         this.Base.BindEvent(down, "click", function (event) {
             self.ShowList("", event);
             return self.Base.CancelEventUp(event);
         });
     },
     SetInput: function () {//设置输入框
         this.Input.autocomplete = "off";
         if (this.IsCreateContainer) {
             this.Input.className = "input";
             var parent = this.Input.parentNode == null ? document.body : this.Input.parentNode;
             parent.insertBefore(this.Container, this.Input);
             parent.removeChild(this.Input);
             this.Container.appendChild(this.Input);
         }
     },
     BindEvent: function () {//绑定输入事件
         this.BindInputAutoEvent();
         this.BindInputClickEvent();
         this.BindBodyEvent();
     },
     BindInputAutoEvent: function () {//绑定输入事件
         if (!this.IsAutoSearch)
             return;
         var self = this;
         this.Base.BindEvent(this.Input, "keydown", function (event) {
             self.Show(self.GetInputValue(), false, event);
         });
         this.Base.BindEvent(this.Input, "keyup", function (event) {
             self.Show(self.GetInputValue(), true, event);
         });
         this.Base.BindEvent(this.Input, "blur", function () {
             self.CheckValue();
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
     CheckValue: function () {//检查是否有选择
         if (this.IsSelectedItem || this.IsMoveItem) return;
         var item = null;
         if (this.List.childNodes != null && this.List.childNodes.length > 0) {
             for (var i = 0; i < this.List.childNodes.length; i++) {
                 if (this.List.childNodes[i].innerHTML == this.Input.value) {
                     item = this.List.childNodes[i];
                     break;
                 }
             }
         }
         this.SelectValue(item == null ? "" : this.Base.GetAttribute(item, this.SaveValuePropertyName));
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
         if (!this.CheckDisabled())
             return;
         this.List.innerHTML = "";
         this.IsSelectedItem = false;
         var infos = this.GetInfos(value);
         this.AddItems(infos);
     },
     HideList: function () {
         this.List.style.display = "none";
     },
     AddItems: function (infos) {//添加选择项
         if (infos != null && infos.length > 0)
             this.List.style.display = "";
         else {
             this.HideList();
             return;
         }
         for (var i = 0; i < infos.length; i++) {
             this.List.appendChild(this.CreateItem(infos[i]));
         }
     },
     CreateItem: function (info) {//创建选择项
         var item = document.createElement('div');
         item.innerHTML = info.Text;
         item.className = "out";
         this.SetItem(item, info);
         this.BindItemEvent(item);
         this.Base.SetAttribute(item, this.SaveValuePropertyName, info.Value);
         return item;
     },
     SetItem: function (item, info) {//设置Item
     },
     GetInfos: function (value) { //默认返回结果
         return value; //[{ Text: "ddd", Value: "sss" }, { Text: "111", Value: "22"}];
     },
     BindItemEvent: function (item) {//绑定Item事件
         this.BindItemOverEvent(item);
         this.BindItemClickEvent(item);
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
             self.IsMoveItem = true;
             return self.Base.CancelEventUp(event);
         });
         this.Base.BindEvent(item, "mouseout", function (event) {
             self.IsMoveItem = false;
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
     SelectItem: function (item) {//选择
         this.Input.focus();
         this.SetSelectItem(item);
         this.HideList();
         this.Select(item);
     },
     Select: function (item) {
     },
     SetSelectItem: function (item) {
         if (item != null) this.IsSelectedItem = true;
         this.SelectText(item);
         this.SelectValue(item == null ? "" : this.Base.GetAttribute(item, this.SaveValuePropertyName));
     },
     SelectText: function (item) { //设置值
         if (this.Input != null) {
             this.Input.value = item == null ? "" : item.innerHTML;
         }
     },
     SelectValue: function (value) { //设置值
         if (this.ValueInput != null) {
             this.ValueInput.value = value;
         }
     },
     GetInputValue: function () {//得到输入值
         return encodeURI(this.Input.value);
     }
 };

 
 
  