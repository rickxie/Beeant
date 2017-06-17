Winner = typeof(Winner)!="undefined" ? Winner : {};
Winner.Dialog = function (subject, message, config) {
    this.Base = new Winner.ClassBase();
    this.StyleFile = "/scripts/Winner/Dialog/Styles/Style.css";
    this.Container = document.createElement('div');
    this.Mask = document.createElement('div');
    this.Container.innerHTML = "<div class='top'><div Instance='Title' class='title'></div><div Instance='Close' class='close'></div></div>" +
        "<div Instance='Detail' class='content'></div><div class='buttom' Instance='Bottom'><input Instance='Sure' type='button' class='sure'/></div>";
    this.Sure = [];
    this.Message = message;
    this.Subject = subject;
    this.IsShowDialog = true;
    this.SureFunction = null;
    this.CancelFunction = null;
    this.MoveInfo = { IsMove: false, Left: 0, Top: 0, ClickLeft: 0, ClickTop: 0 }; //移动信息
    if (config != undefined) {
        this.Base.LoadConfig(this, config);
    }
    this.Parent = this.Parent==undefined?document.body:this.Parent;
};
Winner.Dialog.prototype =
 {
     Initialize: function () { //加载css样式文件
         this.Base.LoadCssFile(this.StyleFile);
         this.Container.className = "dialog";
         this.Mask.className = "dialogmask";
         this.Mask.style.display = "none";
         if (this.Width && this.Width > 0)
             this.Container.style.width = this.Width + "px";
         this.Base.LoadInstances(this, this.Container);
         this.BindEvent();
         this.SetMessage();
         if (this.IsShowDialog)
             this.ShowDialog();
     },
     BindEvent: function () {
         this.BindMoveEvent(this.Title);
         this.BindCloseEvent();
         this.BindSureEvent();
         var self = this;
         window.onresize = window.onscroll = function () {
             if (self.Container.style.display != "none") {
                 var h = self.GetHeight();
                 self.Mask.style.height = h + "px";
                 self.ResizePosition();
             }
         };
     },
     BindCloseEvent: function () {//创建关闭
         var self = this;
         this.Base.BindEvent(this.Close, "click", function () {
             self.CloseDialog();
             if (self.CancelFunction != null) {
                 self.CancelFunction();
             }
         });
     },
     SetMessage: function () { //创建消息
         this.Title.innerHTML = this.Subject;
         this.Detail.innerHTML = this.Message;
     },
     BindSureEvent: function () { //创建确定按钮
         var self = this;
         this.Base.BindEvent(this.Sure, "click", function () {
             var rev = true;
             if (self.SureFunction != null) {
                 rev = self.SureFunction();
             }
             if (rev == undefined || rev)
                 self.CloseDialog();
             return rev;
         });
     },
     GetHeight: function () {
         var h = Math.max(window.clientHeight == undefined ? 0 : window.clientHeight,
             document.clientHeight == undefined ? 0 : document.clientHeight,
             document.body.clientHeight == undefined ? 0 : document.body.clientHeight);
         return h;
     },
     ShowDialog: function () {//显示对话框
         document.body.appendChild(this.Container);
         document.body.appendChild(this.Mask);
         var self = this;
         this.Container.style.display = "none";
         setTimeout(function () { self.ResizePosition(); }, 100);
         var h = this.GetHeight();
         this.Mask.style.height = h + "px";
         this.Mask.style.display = "";
     },
     CloseDialog: function () {//关闭对话
         this.Container.style.display = "none";
         this.Mask.style.display = "none";
     },
     ResizePosition: function () { //设置容器位置
         this.Container.style.position = "absolute";
         this.Container.style.display = "";
         var scrollTop = document.documentElement.scrollTop || window.pageYOffset || document.body.scrollTop;
         var scrollLeft = document.documentElement.scrollLeft || window.pageXOffset || document.body.scrollLeft;
         var ele = document.documentElement;
         var clientWidth = ele.clientWidth == 0 ? document.body.clientWidth : ele.clientWidth;
         var clientHeight = ele.clientHeight == 0 ? document.body.clientHeight : ele.clientHeight;
         var width = scrollLeft + clientWidth / 2 - this.Container.clientWidth / 2;
         var height = scrollTop + clientHeight / 2 - this.Container.clientHeight / 2;
         this.Container.style.left = width + "px";
         this.Container.style.top = height + "px";
     },
     BindMoveEvent: function (box) { //绑定移动对话框事件
         this.BindMouseDownEvent(box);
         this.BindMouseUpEvent(box);
         this.BindMouseMoveEvent(box);
     },
     BindMouseDownEvent: function (box) {//绑定鼠标按下事件
         var self = this;
         this.Base.BindEvent(box, "mousedown", function (event) {
             event = window.event ? window.event : event;
             self.FillMoveInfo(event.clientX, event.clientY);
         });
     },
     BindMouseUpEvent: function () {//绑定鼠标离开事件
         var self = this;
         this.Base.BindEvent(document, "mouseup", function () {
             self.MoveInfo.IsMove = false;
         });
     },
     BindMouseMoveEvent: function () {//绑定鼠标移动事件
         var self = this;
         this.Base.BindEvent(document, "mousemove", function (event) {
             event = window.event ? window.event : event;
             self.MoveContent(event.clientX, event.clientY);
         });
     },
     FillMoveInfo: function (x, y) {//填充移动信息
         this.MoveInfo.IsMove = true;
         this.MoveInfo.Left = parseInt(this.Container.style.left.replace("px", ""));
         this.MoveInfo.Top = parseInt(this.Container.style.top.replace("px", ""));
         this.MoveInfo.ClickLeft = x;
         this.MoveInfo.ClickTop = y;
     },
     MoveContent: function (x, y) { //移动对话框
         if (this.MoveInfo.IsMove) {
             this.Container.style.left = this.MoveInfo.Left + x - this.MoveInfo.ClickLeft + "px";
             this.Container.style.top = this.MoveInfo.Top + y - this.MoveInfo.ClickTop + "px";
         }
     }
 };

 
 
  