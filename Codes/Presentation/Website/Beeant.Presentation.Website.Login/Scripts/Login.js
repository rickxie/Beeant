$(document).ready(function () {
    function focusValue(ctrl) {
        if ($(ctrl).attr("DefaultValue") == undefined) {
            $(ctrl).attr("DefaultValue",  $(ctrl).attr("PlaceHolder"));
        }
        if (ctrl.value == $(ctrl).attr("DefaultValue")) {
            ctrl.value = "";
        }
    }
    function blurValue(ctrl) {
        if (ctrl.value == "") {
            ctrl.value = $(ctrl).attr("DefaultValue");
        }
    }
    $("#loginName").blur(function () {
        blurValue(this);
        this.className = "text";
        $(this).parent().parent().attr("class", "in bor");
    });
    $("#loginName").focus(function () {
        focusValue(this);
        this.className = "text select";
        $(this).parent().parent().attr("class", "in pass");
    });
    $("#loginPass").blur(function () {
        blurValue(this);
        this.className = "text";
        $(this).parent().parent().attr("class", "in bor");
    });
    $("#loginPass").focus(function () {
        focusValue(this);
        this.className = "text select";
        $(this).parent().parent().attr("class", "in pass");
    });
    $("#loginCode").blur(function () {
        blurValue(this);
        this.className = "code";
    });
    $("#loginCode").focus(function () {
        focusValue(this);
        this.className = "code select";
    });
    $("#aCode").click(function () {
        var date = new Date();
        $("#imgCode").attr("src", "/Home/LoginCode?vesion" + date);
    });
    $("#loginSubimt").click(function () {
        var rev = true;
        if ($("#loginName").val() == "" || $("#loginName").attr("DefaultValue") == undefined && $("#loginName").val() == $("#loginName").attr("DefaultValue")) {
            $("#loginName").parent().parent().attr("class", "in error");
            rev = false;
        }
        if ($("#loginPass").val() == "") {
            $("#loginPass").parent().parent().attr("class", "in error");
            rev = false;
        }
        if ($("#loginCode").length > 0 && ($("#loginCode").val() == "" || $("#loginCode").attr("DefaultValue") == undefined && $("#loginCode").val() == $("#loginCode").attr("DefaultValue"))) {
            $("#loginCode").attr("class", "code error");
            rev = false;
        }
        return rev;
    });
});