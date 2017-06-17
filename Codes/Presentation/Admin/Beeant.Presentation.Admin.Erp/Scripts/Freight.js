var Freight = function (carryId, btnCarryAddId, unitId,freeRegionId,districtId, config) {
    this.CarryContainer = $("#" + carryId);
    this.CarryAddButton = $("#" + btnCarryAddId);
    this.FreeRegionContainer = $("#" + freeRegionId);
    this.UnitDropdown = $("#" + unitId);
    this.DistrictId = districtId;
    this.DistrictContainer = $("#" + districtId);
    this.Base = new Winner.ClassBase();
    if (config != undefined) {
        this.Base.LoadConfig(this, config);
    }
};
Freight.prototype = {
    Initialize: function () { //加载css样式文件
        this.Base.LoadInstances(this, this.CarryContainer[0]);
        this.BindEvent();
        this.InitCarryTemplate();
        this.SetUnitType();
        this.Checkbox = new Winner.CheckBox(this.DistrictId);
        this.Checkbox.Initialize();
        this.BindDistrict();
    },

    BindEvent: function () {//绑定事件
        var self = this;
        $(document).ready(function () {
            self.SetUnitType();
        });
        
        this.CarryAddButton.click(function () {
            self.AddCarry();
            self.SetUnitType();
        });
        this.UnitDropdown.change(function () {
            self.SetUnitType();
        });
    },

    //模板
    InitCarryTemplate: function() {
        $(this.CarryTemplate).find("select").attr("name", "CarryType");
        var tr = document.createElement("tr");
        tr.innerHTML = this.CarryTemplate.innerHTML;
        $(tr).find("select").attr("name", "CarryType");
        $(tr).find("td")[0].innerHTML = "<span Instance='RegionName'>未添加地区</span>" +
            "<input type='hidden' Instance='RegionValue' name='CarryRegion'/>" +
            "<input type='hidden' name='CarryId'/>" +
            "<a href='javascript:void(0);' Instance='EditButton'>编辑</a>";
        $(tr).find("td").last().html("<a href='javascript:void(0);' Instance='RemoveButton'>删除</a>");
        this.Base.LoadInstances(this.CarryTemplate, this.CarryTemplate, "Instance");
        this.BindFilter(this.CarryTemplate);
        this.BindRemove(this.CarryTemplate);
        this.BindEdit(this.CarryTemplate);
        this.CarryTemplateTr = tr;
        if (!this.IsSaveTemplate) {
            $(this.CarryTemplate).remove();
        }
    },
    //模板
    InitFreeRegionTemplate: function () {;
        var tr = document.createElement("tr");
        tr.innerHTML = this.FreeRegionTemplate.innerHTML;
        $(tr).find("td")[0].innerHTML = "<span Instance='RegionName'>未添加地区</span>" +
            "<input type='hidden' Instance='RegionValue' name='PostageRegion'/>" +
            "<input type='hidden' name='PostageId'/>" +
            "<a href='javascript:void(0);' Instance='EditButton'>编辑</a>";
        $(tr).find("td").last().html("<a href='javascript:void(0);' Instance='RemoveButton'>删除</a>");
        this.Base.LoadInstances(this.CarryTemplate, this.CarryTemplate, "Instance");
        this.BindFilter(this.CarryTemplate);
        this.BindRemove(this.CarryTemplate);
        this.BindEdit(this.CarryTemplate);
        this.PostageTemplateTr = tr;
    },
    //区域事件
    BindDistrict: function () {//绑定事件
        var self = this;
        this.DistrictContainer.find("a").click(function () {
            var rev = $(this).parent().find("div[class='city']").is(":hidden");
            self.DistrictContainer.find("div[class='city']").hide();
            if (rev)
                $(this).parent().find("div[class='city']").show();
            return false;
        });
        this.DistrictContainer.find("input[type='checkbox']").click(function () {
            self.SetDistrictCount(this);
        });
        this.DistrictContainer.find("input[type='button']").click(function () {
            self.DistrictContainer.hide();
            self.SetDistrictValue();
        });
    },
    SetDistrictCount: function (checkbox) {//设置区域数量
        var count = 0;
        var cityContainer = checkbox.name == "City" ? $(checkbox).parent().parent() : $(checkbox).parent().find("div[class='city']");
        cityContainer.find("input[type='checkbox']").each(function (index, a) {
            if (a.checked)
                count++;
        });
        cityContainer.parent().find("span[name='spanCount']").html(count);
    },
    GetDistrictInfo: function () {//得到选择区域
        var infos = [];
        this.DistrictContainer.find("div[class='city']").each(function (index, city) {
            if ($(city).find("input[type='checkbox']").length == $(city).find("input[type='checkbox']:checked").length) {
                infos.push({ Id: $(city).parent().find("input[name='Province']").val(),
                    Name: $(city).parent().find("label[name='Province']").html()
                });
                return;
            }
            $(city).find("input[type='checkbox']:checked").each(function (i, input) {
                infos.push({ Id: $(input).val(),
                    Name: $(input).parent().find("label[name='City']").html()
                });
            });
        });
        return infos;
    },
    SetDistrictValue: function () {//设置区域
        if (this.RegionValue != undefined) {
            var infos = this.GetDistrictInfo();
            var values = [];
            var names = [];
            for (var i = 0; i < infos.length; i++) {
                values.push(infos[i].Id);
                names.push(infos[i].Name);
            }
            $(this.RegionValue).val(values.join('-'));
            $(this.RegionName).html(names.join(","));
        }
        this.DistrictContainer.find("input[type='checkbox']").attr("checked", false);
        this.DistrictContainer.find("span[name='spanCount']").html("");
        this.DistrictContainer.find("div[class='city']").hide();
    },
 
    //过滤事件
    BindFilter: function (tr) {
        var self = this;
        $(tr).find("input").keyup(function () {
            self.FilterInput(this);
        });
        $(tr).find("input").live("afterpaste", function () {
            self.FilterInput(this);
        });
    },
    FilterInput: function (ctrl) {
        if ( $(ctrl).attr("filtername") == "int")
            ctrl.value = ctrl.value.replace(/\D/g, '');
        else if ($(ctrl).attr("filtername") == "decimal") {
            if ('' != ctrl.value.replace(/\d{1,}\.{0,1}\d{0,2}/, '')) {
                ctrl.value = ctrl.value.match(/\d{1,}\.{0,1}\d{0,2}/) == null ? '' : ctrl.value.match(/\d{1,}\.{0,1}\d{0,2}/);
            }
        }
    },
    //添加编辑删除
    AddCarry: function (info) {//添加
        var tr = document.createElement("tr");
        tr.innerHTML = this.CarryTemplateTr.innerHTML;
        this.Base.LoadInstances(tr, tr);
        this.SetCarryTrValue(tr, info);
        this.BindRemove(tr);
        this.BindEdit(tr);
        this.CarryContainer.append(tr);
        this.BindFilter(tr);
    },
    SetCarryTrValue: function (tr, info) {
        if (info == undefined)
            return;
        $(tr).find("input[name='CarryId']").val(info.Id);
        $(tr).find("input[name='CarryRegion']").val(info.Region);
        $(tr).find("input[name='CarryName']").val(info.Name);
        $(tr).find("input[name='CarryDefaultCount']").val(info.DefaultCount);
        $(tr).find("input[name='CarryDefaultPrice']").val(info.DefaultPrice);
        $(tr).find("input[name='CarryContinueCount']").val(info.ContinueCount);
        $(tr).find("input[name='CarryContinuePrice']").val(info.ContinuePrice);
        $(tr.RegionName).html(info.Region);
    },

    BindRemove: function (tr) {//移除
        $(tr.RemoveButton).click(function () {
            $(tr).remove();
        });
    },
    BindEdit: function (tr) {//绑定编辑事件
        var self = this;
        $(tr.EditButton).click(function () {
            self.ShowDistrictContainer(tr);
        });
    },
    ShowDistrictContainer: function (tr) {
        var self = this;
        var maskHeight = $(document).height(); //文档的总高度
        var maskWidth = $(window).width(); //窗口的宽度
        var dialogTop = (maskHeight / 2) - (self.DistrictContainer.height() / 2);
        var dialogLeft = (maskWidth / 2) - (self.DistrictContainer.width() / 2);
        self.DistrictContainer.css({ top: dialogTop, left: dialogLeft }).show();
        var values = $(tr.RegionValue).val().split('-');
        if (values.length > 0) {
            self.DistrictContainer.find("input[type='checkbox']").each(function (index, checkBox) {
                for (var i = 0; i < values.length; i++) {
                    if ($(checkBox).val() == values[i]) {
                        checkBox.click();
                        return;
                    }
                }
            });
        }
    }
};
 