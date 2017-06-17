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
                Content: $("#txtContent").val() == $("#txtContent").attr("ShowValue") ? " " : $("#txtContent").val()
            };
        }
        var saveData = getSaveData();       
        $.ajax({
            type: "Post",
            url: "/Message/SaveSubscribe",
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