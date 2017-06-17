$(document).ready(function () {
    $("#aCode").click(function () {
        var url = window.LoginUrl == undefined ? "/Home/Code/" + window.SiteId + "?vesion=" : window.LoginUrl + "/Home/Code?vesion=";
        var date = new Date();
        $("#imgCode").attr("src", url + date);
    });
    $("#btnSave").click(function () {
        if ($("#divCode")[0].style.display != "none" && $("#txtCode").val()=="") {
            alert(window.Language.InputCodeTip);
            $("#divCode")[0].focus();
            return false;
        }
        if (!window.Validator.ValidateSubmit()) {
            alert(window.Language.SubmitError);
            return false;
        }
        function getSaveData() {
            return {
                Mobile: $("#txtMobile").val(),
                Linkman: $("#txtLinkman").val(),
                Content: $("#txtContent").val(),
                Code: $("#txtCode").val()
        };
        }
        var saveData = getSaveData();
        $.ajax({
            type: "Post",
            url: "/Inquery/Add/"+window.SiteId,
            data: saveData,
            async: false,
            dataType: "json",
            success: function (data) {
                if (data.Status) {
                    alert(window.Language.SubmitSucccessTip);
                    $("#btnSave").remove();
                } else {
                    if (data.Message == "codererror") {
                        alert(window.Language.CodeError); 
                       
                    } else {
                        alert(window.Language.SubmitFailError);
                    }
                  
                }
            },
            error: function () {
                alert(window.Language.LoadErrorMessage);
            }
        });
        return false;
    });

});