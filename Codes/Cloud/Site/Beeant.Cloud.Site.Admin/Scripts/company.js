$(document).ready(function () {
    $("#btnSave").click(function () {
        if (!window.checkSaveTag("btnSave")) {
            return false;
        }
        if (!window.Validator.ValidateSubmit()) {
            alert("您输入的信息有误，请检测后重新输入");
            window.removeSaveTag("btnSave");
            return false;
        }
        function getSaveData() {
            return {
                Name: $("#txtName").val() == $("#txtName").attr("ShowValue") ? " " : $("#txtName").val(),
                Mobile: $("#txtMobile").val() == $("#txtMobile").attr("ShowValue") ? " " : $("#txtMobile").val(),
                Email: $("#txtEmail").val() == $("#txtEmail").attr("ShowValue") ? " " : $("#txtEmail").val(),
                Linkman: $("#txtLinkman").val() == $("#txtLinkman").attr("ShowValue") ? " " : $("#txtLinkman").val(),
                Address: $("#txtAddress").val() == $("#txtAddress").attr("ShowValue") ? " " : $("#txtAddress").val(),
                Qq: $("#txtQq").val() == $("#txtQq").attr("ShowValue") ? " " : $("#txtQq").val(),
                Fax: $("#txtFax").val() == $("#txtFax").attr("ShowValue") ? " " : $("#txtFax").val(),
                Detail: $("#txtDetail").val() == $("#txtDetail").attr("ShowValue") ? " " : $("#txtDetail").val()
            };
        }
        var saveData = getSaveData();       
        $.ajax({
            type: "Post",
            url: "/Company/Save",
            data: saveData,
            async: true,
            dataType: "json",
            success: function (data) {
                if (data.Status) {
                    alert("保存成功");
                } else {
                    alert(data.Message);
                }
                window.removeSaveTag("btnSave");
            },
            error: function () {
                alert("系统忙，请稍候再试");
                window.removeSaveTag("btnSave");
            }
        });
        return false;
    });

});