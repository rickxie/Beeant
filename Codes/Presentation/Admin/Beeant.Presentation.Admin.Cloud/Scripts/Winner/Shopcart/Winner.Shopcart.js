Winner = typeof(Winner)!="undefined" ? Winner : {};
Winner.Shopcart = function (containerId, config) {
    this.Base = new Winner.ClassBase();
    this.Container = document.getElementById(containerId);
    this.Html = "";
    this.CookieName = "Shopcart";
    this.CookieTime = 1000 * 60 * 60 * 24;
    this.MaxCount = 10;
    this.AddHandle = null;
    this.RemoveHandle = null;
    this.UpdateHandle = null;
    this.Products = [];
    this.IsSaveCookie = true;
    if (config != undefined) {
        this.Base.LoadConfig(this, config);
    }

};
Winner.Shopcart.prototype =
 {
     Initialize: function () { //初始化
         if (this.Container != null)
             this.Container.innerHTML = "";
         this.Products = this.GetProducts();
         var infos = this.Gets();
         if (infos != null && infos.length > 0) {
             for (var i = 0; i < infos.length; i++) {
                 this.Append(infos[i], false);
             }
         }
     },
     Add: function (info) {//添加
         if (info == null) return false;
         for (var j = 0; j < this.Products.length; j++) {
             if (this.Products[j].Info.Id == info.Id) {
                 if (this.Products[j].CountInput != null) {
                     if (this.Products[j].CountInput.innerHTML == undefined) {
                         this.Products[j].CountInput.value = parseInt(this.Products[j].CountInput.value) + 1;
                     } else {
                         this.Products[j].CountInput.innerHTML = parseInt(this.Products[j].CountInput.innerHTML) + 1;
                     }
                 }
                 var infos = this.Gets();
                 if (infos != null) {
                     for (var i = 0; i < infos.length; i++) {
                         if (info.Id == infos[i].Id) {
                             infos[i].Count = infos[i].Count + 1;
                             this.SetCookie(this.CookieName, this.Base.Serialize(infos), this.CookieTime);
                         }
                     }
                 }
                 if (this.AddHandle != null)
                     this.AddHandle(this.Products[j]);
                 return this.Products[j];
             }
         }
         if (this.Products.length >= this.MaxCount)
             return false;
         return this.Append(info, true);
     },
     Append: function (info, isAddCookie) {
         var product = document.createElement("div");
         product.innerHTML = this.Html;
         for (var name in info) {
             if (name != "undefined") {
                 product.innerHTML = this.ReplaceAll(product.innerHTML, "@" + name, info[name]);
             }
         }
         product.Info = info;
         this.Base.LoadInstances(product, product);
         if (this.AddHandle != null && !this.AddHandle(product)) {
             return null;
         }
         this.Products.push(product);
         if (this.Container != null) {
             this.BindRemoveEvent(product);
             this.Container.appendChild(product);
         }
         if (isAddCookie)
             this.AddCookie(product);
         return product;
     },
     BindRemoveEvent: function (product) {
         if (product.RemoveButton == null)
             return;
         var self = this;
         this.Base.BindEvent(product.RemoveButton, "click", function () {
             self.Remove(product.Info.Id);
         });
     },
     Remove: function (id) { //移除
         var product = null;
         for (var i = 0; i < this.Products.length; i++) {
             if (this.Products[i].Info.Id == id) {
                 product = this.Products[i];
                 this.Products.splice(i, 1);
                 break;
             }
         }
         if (this.RemoveHandle != null && !this.RemoveHandle(product)) {
             return false;
         }
         if (this.Container != null)
             this.Container.removeChild(product);
         this.RemoveCookie(product);
         return true;
     },
     Clear: function () { //清空
         if (this.Container != null)
             this.Container.innerHTML = "";
         this.SetCookie(this.CookieName, "", 0 - this.CookieTime);
     },
     ReplaceAll: function (source, oldString, newString) { //替换所有
         var reg = new RegExp("\\" + oldString, "g");
         return source.replace(reg, newString);
     },
     GetProducts: function() {
         return [];//[{Id:'',Count:0}]
     },
     Gets: function () {
         if (!this.IsSaveCookie)
             return null;
         return this.Base.Deserialize(this.GetCookie(this.CookieName));
     },
     AddCookie: function (product) {//添加cookie
         if (!this.IsSaveCookie)
             return;
         var infos = this.Gets();
         infos.push(product.Info);
         this.SetCookie(this.CookieName, this.Base.Serialize(infos), this.CookieTime);
     },
     RemoveCookie: function (product) {//移除cookie
         if (!this.IsSaveCookie)
             return;
         var infos = this.Gets();
         for (var i = 0; i < infos.length; i++) {
             if (infos[i].Id == product.Info.Id) {
                 infos.splice(i, 1);
                 break;
             }
         }
         this.SetCookie(this.CookieName, this.Base.Serialize(infos), this.CookieTime);
     },
     SetCookie: function (name, value, time) {
         var exp = new Date();
         exp.setTime(exp.getTime() + time);
         document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString() + ";path=/";
     },
     GetCookie: function (name) {
         var arr = document.cookie.match(new RegExp("(^|)" + name + "=([^;]*)(;|$)"));
         if (arr != null)
             return unescape(arr[2]);
         return null;
     }
 };

 
 
  