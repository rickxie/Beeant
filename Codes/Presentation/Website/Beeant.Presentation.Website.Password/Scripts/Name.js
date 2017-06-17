$(document).ready(function() {
    this.Validator = new Winner.Validator({ StyleFile: "", IsShowMessage: false });
    this.Validator.Initialize();
    for (var i = 0; i < window.ValidateInfos.length; i++) {
        window.ValidateInfos[i].ShowMessageEvent = "focus";
        window.ValidateInfos[i].HideMessageEvent = "blur";
    }
    this.Validator.InitializeControl(window.ValidateInfos);
    var nameInfo = this.Validator.GetValidateInfo($("#txtName")[0]);
    if (nameInfo != null) {
        var func = function () {
            return self.CheckName();
        };
        nameInfo.Handles.push({ Function: func, Message: "该用户已经存在" });
    }
    var self = this;
    $("#btnSubmit").click(function () {
        if (self.Validator != null) {
            return self.Validator.ValidateSubmit();
        }
        return true;
    });
});
 

