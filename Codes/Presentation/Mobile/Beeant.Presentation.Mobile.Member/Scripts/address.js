function initEdit() {
    var changefunc = function (sender) {
        if (sender.value == "") {
            $(sender).addClass("ctrlshow");
        } else {
            $(sender).removeClass("ctrlshow");
        }
    }
    $(document).find("select").each(function (index, sender) {
        changefunc(this);
    });
    $(document).find("select").bind("change", function () {
        changefunc(this);
    });
    $("#btnSave").click(function () {
        $("input").each(function (index, sender) {
            if ($(this).val() == $(this).attr("ShowValue"))
                $(this).val("");
        });
    });
}

function remove(sender,id) {
    $.ajax({
        type: "Post",
        url: "/Address/Remove",
        data: { id: id },
        async: true,
        dataType: "json",
        success: function (data) {
            if (data.Status) {
                $(sender).parent().parent().remove();
            } else {
                alert(data.Message);
            }
        },
        error: function () {
            alert("系统忙，请稍后再试");
        }
    });
}
function updateDefault(id) {
    $.ajax({
        type: "Post",
        url: "/Address/UpdateDefault",
        data: { id: id },
        async: true,
        dataType: "json",
        success: function (data) {
            if (data.Status) {
               
            } else {
                alert(data.Message);
            }
        },
        error: function () {
            alert("系统忙，请稍后再试");
        }
    });
}