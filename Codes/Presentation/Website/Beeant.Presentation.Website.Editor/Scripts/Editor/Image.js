$(document).ready(function () {
    function getIds() {
        var ids = [];
        $("#divRight").find("input[type=checkbox]").each(function (index, sender) {
            if (this.value == "on" || this.value == "" || !this.checked)
                return;
            ids.push(this.value);
        });
        return ids;
    }
    window.remove = function () {
        if (!window.checkSaveTag("divRight")) {
            return false;
        }
        var ids = getIds();
        $.ajax({
            type: "Post",
            url: "/Image/Remove",
            data: { id: ids },
            async: true,
            dataType: "json",
            traditional: true,
            success: function (data) {
                if (data != null && data.Status == true) {
                    alert("删除成功");
                    window.location.reload();
                } else if (data != null) {
                    alert(data.Message);
                }
                window.removeSaveTag("divRight");
            },
            error: function () {
                alert("系统忙，请稍候再试");
                window.removeSaveTag("divRight");
            }
        });
    };
    $("#btnSave").click(function () {
        if (!window.Validator.ValidateSubmit()) {
            alert("您输入的信息有误，请检测后重新输入");
            return false;
        }
        $("#fmFile")[0].submit();
        return true;
    });
    var checkbox = new Winner.CheckBox('finder');
    var confirmBox = new Winner.ConfirmBox();
    var finder = new Winner.Editor.Finder('finder');
    var folder = new Folder('hfMoveSwitcher', 'hfMoveContainer', '/Image/Move');
    checkbox.Initialize();
    confirmBox.Initialize();
    finder.Initialize();
    folder.Initialize();
    window.Finder = finder;
})