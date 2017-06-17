var MaintenPage = function (editId, addId, hideId, isShow) {
    this.Base = new Winner.ClassBase();
    this.Edit = document.getElementById(editId);
    this.Add = document.getElementById(addId);
    this.Hide = document.getElementById(hideId);
    if (this.Edit!=null && this.Add!=null &&this.Hide!=null )
        this.Initialize(isShow);
};
MaintenPage.prototype = {
    Initialize: function (isShow) {
        this.Display(isShow);
        this.BindEvent();
        this.ResetPosition();
    },
    BindEvent: function () {
        var self = this;
        this.Base.BindEvent(this.Add, "click", function () {
            self.Display(true);
        });
        this.Base.BindEvent(this.Hide, "click", function () {
            self.Display(false);
        });
    },
    Display: function (value) {
        this.Edit.style.display = value ? "" : "none";
    },
    ResetPosition: function () {
        //var func = function () {
        //    $("#menu").css("position", "inherit");
        //    $("#menu").css("float", "left");
        //    $(".main").css("width", $(".main").width()+"px");
        //    $(".main").css("position", "inherit");
        //    $(".main").css("position", "inherit");
        //    $(".main").css("float", "left");
        //    $(".main").css("padding-left", "20px");
        //    $(".main").css("margin-top", "91px");
        //};
        //setTimeout(func, 1000);
    }
};