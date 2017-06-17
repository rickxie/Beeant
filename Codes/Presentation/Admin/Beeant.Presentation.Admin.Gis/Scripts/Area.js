var Area = function (ddlCity, txtTag, txtColor, datas, isEdit, isPublish) {
    $(document).ready(function() {
        var baidu = new baiduArea(ddlCity, txtColor, isEdit);
        baidu.Initialize();
        Area.baidu = baidu;
        Area.IsPublish = isPublish;
        var cancel = function () {
            Area.baidu.Cancel();
            Area.Dialog.CloseDialog();
        };
        $("#btnSave").click(function () {
            Area.Save();
        });
        $("#btnDistrict").click(function () {
            Area.baidu.SetDistrict($("#"+ddlCity).val()+ $("#txtDistrict").val());
        });
        $("#btnCancel").click(function () {
            cancel();
        });
        $("#" + ddlCity).change(function () {
            Area.ShowList();
        });
        $("#btnPantoAddress").click(function () {
            Area.PanTo();
        });
        Area.CityControl = $("#" + ddlCity);
        Area.ShowList();
        Area.Dialog = new Winner.Dialog("区域保存", "", { IsShowDialog: false, CancelFunction:cancel });
        Area.Dialog.Initialize();
        $(Area.Dialog.Bottom).hide();
        $(Area.Dialog.Detail).append($("#divSaveContent"));
        if ($("#txtAddress").val()!="") {
            Area.PanTo();
        }
        function InitColorPan() {
            $('#' + txtColor).iColor(function (hx) {
                this.val('#' + hx).css('background', '#' + hx);
                Area.baidu.InitDraw();
            });
            $("#iColorPicker").css("z-index", 99999);
            $(Area.Dialog.Container).css("z-index", 99998);
        };
        InitColorPan();
    });
};
Area.Save = function () {
    $.ajax({
        type: "Post",
        url: "/Gis/Area/SaveAjax.aspx?id=" + Area.ShowSave.Id,
        data: {
            name: $("#txtName").val(),
            path: Area.ShowSave.Path,
            type: Area.ShowSave.Type,
            color: $("#txtsaveColor").val(), city: Area.ShowSave.City, tag: $("input[name='tag']:checked").val(),
            value: $("#txtValue").val(),
            isUsed: $("#ckIsUsed")[0].checked
        },
        async: false,
        dataType: "text",
        success: function (data) {
            var json = eval("[" + data + "]");
            if (json[0].Code == "true") {
                if (Area.ShowSave.Id == undefined || Area.ShowSave.Id == "") {
                    Area.Id = json[0].Message;
                    alert("保存成功");
                    Area.Dialog.CloseDialog();
                } else {
                    alert("保存成功"); 
                    Area.baidu.Cancel();
                    Area.Dialog.CloseDialog();
                }
                Area.ShowList();
            } else {
                alert(json[0].Message);
            }
        },
        error: function () {
            alert("保存失败");
        }
    });
};
Area.Remove = function (id) {
    var rev = false;
    $.ajax({
        type: "Post",
        url: "/Gis/Area/SaveAjax.aspx?id=" + id + "&savetype=remove",
        data: {},
        async: false,
        dataType: "text",
        success: function (data) {
            var json = eval("[" + data + "]");
            if (json[0].Code == "true") {
                rev = true;
            }
        },
        error: function () {
            alert("删除失败");
        }
    });
    Area.ShowList();
    return rev;
};
Area.ShowList = function() {
    $.ajax({
        type: "get",
        url: "/Gis/Area/ListAjax.aspx?city=" + Area.CityControl.val() + "&IsPublish=" + Area.IsPublish,
        data: { Id: Area.ShowSave.Id },
        async: false,
        dataType: "text",
        success: function(data) {
            var infos = eval(data);
            var html = "";
            Area.baidu.Clear();
            Area.baidu.Polygons = [];
            if (infos != null) {
                for (var i = 0; i < infos.length; i++) {
                    html += "<input id='" + infos[i].Id + "' type='checkbox' checked='checked' value='" + infos[i].Path + "' /><label for='" + infos[i].Id + "'>" + infos[i].Name + "</label>";
                    if (infos[i].Type == "Baidu") {
                        Area.baidu.PushPolygon(infos[i].Id, infos[i].Color, eval(infos[i].Path));
                    }
                }
            }
            $("#dviArea").html(html);

            Area.baidu.BindCheckBox($("#dviArea").find("input"));

        }
    });
};
Area.ShowSave = function() {
    if (Area.ShowSave.Id != undefined && Area.ShowSave.Id != 0) {
        $.ajax({
            type: "get",
            url: "/Gis/Area/Detail.aspx?id=" + Area.ShowSave.Id,
            data: {},
            async: false,
            dataType: "text",
            success: function(data) {
                Area.Dialog.ShowDialog();
                var json = eval("[" + data + "]");
                $("#txtName").val(json[0].name);
                $("#txtValue").val(json[0].value),
                $("#ckIsUsed")[0].checked = json[0].isused;
                $("#txtsaveColor").val(json[0].color).css("background", json[0].color);
                $("input[name='tag']").each(function (index, sender) {
                    if (sender.value == json[0].tag)
                        sender.checked = true;
                });

            }
        });
    } else {
        Area.Dialog.ShowDialog();
    }
    $("#txtsaveColor").val(Area.ShowSave.Color);
    $("#txtsaveColor").css('background', Area.ShowSave.Color);
    $('#txtsaveColor').iColor(function (hx) {
        this.val('#' + hx).css('background', '#' + hx);
    });
};
Area.PanTo = function () {
    var setReuslt = function (infos) {
        var html = "";
        for (var j = 0; j < infos.length; j++) {
            html += infos[j];
            if (infos.length - 1 != j) {
                html += ",";
            }
        }
        $("#spAreas").html(html);
    };
    var func= function(type) {
        $.ajax({
            type: "get",
            url: "/Gis/Address/GetAjax.aspx?type=" + type + "&city=" + Area.CityControl.val() + "&address=" + $("#txtAddress").val(),
            data: { Id: Area.ShowSave.Id },
            async: false,
            dataType: "json",
            success: function (json) {
                if (type == "Baidu") {
                    Area.baidu.PanTo(json.Result.lng, json.Result.lat);
                    setReuslt(json.Areas);
                }
            }
        });
    }
    var types = ["Baidu"];
    for (var i = 0; i < types.length; i++) {
        func(types[i]);
    }
};

Area.SaveAddress = function (type, point, savetype) {
    var value = "{lng:" + point.lng + ",lat:" + point.lat + "}";
    $.ajax({
        type: "get",
        url: "/Gis/Address/SaveAjax.aspx?type=" + type + "&city=" + Area.CityControl.val()
            + "&name=" + $("#txtAddress").val() + "&point=" + value + "&IsStartWith=" + $("#ckIsStartWith")[0].checked + "&savetype=" + savetype,
        data: {},
        async: false,
        dataType: "text",
        success: function (data) {
            if (data != "") {
                alert(data);
            }
        }
    });
}

var baiduArea = function (cityId, colorId, isEdit) {
    this.Map = new BMap.Map("allmap", {enableMapClick:false});
    this.Map.enableScrollWheelZoom();
    //this.Map.disableDoubleClickZoom();
    this.IsEdit = isEdit;
    this.CityId = cityId;
    this.ColorId = colorId;
    this.Type = "Baidu";
    this.Polygons = [];
};
baiduArea.prototype = {
    Initialize: function () {
        this.BindEvent();
        this.InitDraw();
    },
    BindCheckBox: function (cks) {
        var self = this;
        cks.click(function() {
            for (var i = 0; i < self.Polygons.length; i++) {
                if (self.Polygons[i].FlagId == $(this).attr("Id")) {
                    if (!this.checked) {
                        self.Map.removeOverlay(self.Polygons[i]);
                    } else {
                        self.Map.addOverlay(self.Polygons[i]);
                    }
                }
            }
        });
    },
    BindEvent: function () {
        var self = this;
        self.Map.centerAndZoom($("#" + self.CityId).val(), 13);
        $("#" + self.CityId).change(function () {
            self.Map.centerAndZoom($("#" + self.CityId).val(), 13);
        });
        $("#" + self.ColorId).change(function () {
            self.InitDraw();
        });
    },
    SetDistrict: function (name) {//设置区域
        var self = this;
        if (name == "")
            return;
        var bdary = new BMap.Boundary();
        bdary.get(name, function (rs) {       //获取行政区域
            var count = rs.boundaries.length; //行政区域的点有多少个
            if (count === 0) {
                return;
            }
           
            var pointArray = [];
            for (var i = 0; i < count; i++) {
                var ply = new BMap.Polygon(rs.boundaries[i], { strokeWeight: 2, strokeColor: "#ff0000" }); //建立多边形覆盖物
                self.PushPolygon(0, $("#" + self.ColorId).val(), ply.getPath());
                pointArray = pointArray.concat(ply.getPath());
            }
            self.Map.setViewport(pointArray);    //调整视野                 
        });
    },
    PanTo: function (lng, lat) {//定位点
        var self = this;
        if (this.AddressMark!= undefined) {
            this.Map.removeOverlay(this.AddressMark);
        }
        var saveMarker = function (e, ee, marker) {
            Area.SaveAddress(self.Type, marker.point, "");
        }
        var removeMarker = function (e, ee, marker) {
            Area.SaveAddress(self.Type, marker.point, "remove");
            self.Map.removeOverlay(marker);
        }
        var point = new BMap.Point(lng, lat);
        this.AddressMark = new BMap.Marker(point);  // 创建标注
        this.Map.addOverlay(this.AddressMark);              // 将标注添加到地图中
        this.Map.panTo(point);
        var markerMenu = new BMap.ContextMenu();
        markerMenu.addItem(new BMap.MenuItem('保存', saveMarker.bind(this.AddressMark)));
        markerMenu.addItem(new BMap.MenuItem('删除', removeMarker.bind(this.AddressMark)));
        this.AddressMark.addContextMenu(markerMenu);
        this.AddressMark.enableDragging();

    },
    InitDraw: function() {
        var self = this;
        var color = $("#" + this.ColorId).val();
        var styleOptions = {
            strokeColor: color, //边线颜色。
            fillColor: color, //填充颜色。当参数为空时，圆形将没有填充效果。
            strokeWeight: 1, //边线的宽度，以像素为单位。
            strokeOpacity: 0.8, //边线透明度，取值范围0 - 1。
            fillOpacity: 0.6, //填充的透明度，取值范围0 - 1。
            strokeStyle: 'solid' //边线的样式，solid或dashed。
        }
        //实例化鼠标绘制工具
        var drawingManager = new BMapLib.DrawingManager(self.Map, {
            isOpen: false, //是否开启绘制模式
            enableDrawingTool: true, //是否显示工具栏
            drawingToolOptions: {
                anchor: BMAP_ANCHOR_TOP_RIGHT, //位置
                offset: new BMap.Size(5, 5), //偏离值
                scale: 0.8 //工具栏缩放比例
            },
            polygonOptions: styleOptions //多边形的样式
        });
        $('.BMapLib_marker').hide();
        $('.BMapLib_circle').hide();
        $('.BMapLib_polyline').hide();
        $('.BMapLib_rectangle').hide();
        if (this.IsEdit == false) {
            $('.BMapLib_hander').hide();
            $('.BMapLib_polygon').hide();
        }
        drawingManager.addEventListener('polygoncomplete', function(e) {
            Area.ShowSave.Id = undefined;
            self.ShowSave(e);
        });
    },
    Clear: function () {
        this.Map.clearOverlays();
    },
    ShowSave: function (e) {//保存
        if (this.IsEdit == false)
            return;
        var self = this;
        self.SetAreaSave(e);
        var func = function () {
            var color = $("#" + self.ColorId).val();
            this.PushPolygon(color, e.ia);
            self.Map.removeOverlay(e);
        };
        Area.ShowSave(func);
        this.CurrentArea = e;
    },
    SetAreaSave: function (e) {//设置
        var self = this;
        Area.ShowSave.City = $("#" + self.CityId).val();
        Area.ShowSave.Type = self.Type;
        Area.ShowSave.Color = $("#" + this.ColorId).val();
        var path = "[";
        for (var i = 0; i < e.ia.length; i++) {
            path += "{lat:" + e.ia[i].lat + ",lng:" + e.ia[i].lng + "}";
            if (i != e.ia.length - 1) {
                path += ",";
            }
        }
        path += "]";
        Area.ShowSave.Path = path;
    },
    Cancel: function () {
        this.Map.removeOverlay(this.CurrentArea);
    },
    Save: function() {
        var color = $("#" + this.ColorId).val();
        this.PushPolygon(Area.Id,color, this.CurrentArea.ia);
        this.Cancel();
    },
    PushPolygon: function (flagId, color, path) {
        var self = this;
        var map = this.Map;
        var polygon = new BMap.Polygon();
        polygon.setPath( path );
        polygon.setStrokeColor(color);
        polygon.setFillColor(color);
        polygon.setStrokeOpacity(0.8);
        polygon.setFillOpacity(0.6);
        polygon.setStrokeWeight(1);
        polygon.setStrokeStyle('solid');
        map.addOverlay(polygon);
        polygon.FlagId = flagId;
        var removeMarker = function (e, ee, marker) {
            if (Area.Remove(marker.FlagId)) {
                map.removeOverlay(marker);
            }
        }
        var saveMarker = function (e, ee, marker) {
            Area.ShowSave.Id = marker.FlagId;
            self.SetAreaSave(marker);
            Area.ShowSave.Color = color;
            Area.ShowSave();
        }
        if (self.IsEdit == true) {
            var markerMenu = new BMap.ContextMenu();
            markerMenu.addItem(new BMap.MenuItem('保存', saveMarker.bind(polygon)));
            markerMenu.addItem(new BMap.MenuItem('删除', removeMarker.bind(polygon)));
            polygon.addContextMenu(markerMenu);
        }
        polygon.addEventListener('click', function () {
            if (!polygon.edit_flg || polygon.edit_flg == 0) {
                polygon.enableEditing();
                polygon.edit_flg = 1;
            } else {
                polygon.disableEditing();
                polygon.edit_flg = 0;
            }
        });
        this.Polygons.push(polygon);
        return polygon;
    }
   
}