(function() {
    var Guid = function() {
        return 'xxxxxxxx_xxxx_4xxx_yxxx_xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
            var r = Math.random() * 16 | 0,
                v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }

    var Constants = {
        invalidcontent: '请输入一个有效内容',
        operation: '操作',
        unallowblank: '该字段必填',
        charlencannotbigthan: '字符数不能大于[{#}]',
        pageindex: '第&nbsp;{#}&nbsp;页',
        allpage: '共&nbsp;{#}&nbsp;页',
        firstpage: '首页',
        prevpage: '上一页',
        nextpage: '下一页',
        lastpage: '尾页',
        refreshpage: '刷新',
        exportpage: '导出本页',
        exportallpage: '导出所有',
        pageshowrecord: '每页显示&nbsp{#}&nbsp条记录',
        allrecord: '共&nbsp{#}&nbsp条记录',
        unopenfunction: '该功能尚未完善...',
        nostr: '序号',
    }

    AExtButton = function(ImgUrl, Name, Action) {
        this.ImgUrl = ImgUrl;
        this.Name = Name;
        this.Action = Action;
    }

    AExtButton.prototype = {
        ImgUrl: '',
        Name: '',
        Action: function() {},
    }

    ALblColumn = function(MapID, Name, ID, Type) {
        this.init(MapID, Name, ID, Type, 'label');
    };

    ALblColumn.prototype = {
        Ctrl: '', //控件类型
        Type: 'string', //字段类型
        ColumnID: '', //字段ID
        ColumnName: '', //字段名称
        ColumnMapping: '', //映射字段ID
        verfiy: {
            Required: false,
            MaxLength: -1,
            Decimals: -1,
            RegExps: '',
            RegExpMsg: Constants.invalidcontent,
        },
        IsExport: function() {
            if (this.Type === "hidden")
                return false;
            else
                return true;
        },
        GetControl: function(rowData) {
            return this.GetColData(rowData);
        },
        UionId: '',
        init: function(MapID, Name, ID, Type, Ctrl, Verifty) {
            this.ColumnMapping = MapID;
            this.ColumnName = Name;
            if (ID)
                this.ColumnID = ID;
            else
                this.ColumnID = MapID;
            if (Ctrl)
                this.Ctrl = Ctrl;
            this.verfiy = $.extend({}, this.verfiy, Verifty);
            if (Type)
                this.Type = Type
        },
        GetColData: function(rowData) {
            if (typeof(this.ColumnMapping) == 'function') {
                return this.ColumnMapping.call(this, rowData);
            } else
                return rowData[this.ColumnMapping];
        },
        GetMappingID: function() {
            if (typeof(this.ColumnMapping) == 'function') {
                if (this.UionId == '')
                    this.UionId = Guid();
                return this.UionId;
            } else
                return this.ColumnMapping;
        },
        AddVerifty: function(ctrl, td) {
            this.AddVeriftyCtrl(ctrl, td);
            this.AddVeriftyAttr(ctrl, td);
            this.AddCtrlEvent(ctrl, td);
        },
        AddVeriftyAttr: function(ctrl, td) {

        },
        AddVeriftyCtrl: function(ctrl, td) {
            var verityCtrl = $("<span style='color:red;padding-left:5px' data-type='verifty'></span>");
            td.append(verityCtrl);
        },
        AddCtrlEvent: function(ctrl, td) {

        },
        CheckVerify: function(input) {
            return this.CheckVerifyEmpty(input) && this.CheckVerifyLength(input) && this.CheckVerifyRegExp(input)
        },
        CheckVerifyEmpty: function(input) {
            return true;
        },
        CheckVerifyLength: function(input) {
            return true;
        },
        CheckVerifyRegExp: function(input) {
            return true;
        },
        UpdateVerifyErrorMsg: function(row, message) {
            var span = $(row).find("span[data-type='verifty']");
            if (span.length > 0) {
                $(span[0]).html(message);
                return true;
            }
            return false;
        },
    }

    AHidColumn = function(MapID, Name, ID, Type) {
        this.init(MapID, Name, ID, Type, 'hidden')
    }

    AHidColumn.prototype = $.extend({}, new ALblColumn, {
        AddVeriftyAttr: function(ctrl, td) {
            $(td).hide();
        }
    });

    ATxtColumn = function(MapID, Name, ID, Type, Verifty) {
        this.init(MapID, Name, ID, Type, 'text', Verifty);
    }

    ATxtColumn.prototype = $.extend({}, new ALblColumn, {
        GetControl: function(rowData) {
            var input = $("<input type='text' data-id='itemctrl-" + this.ColumnID + "' value='" + this.GetColData(rowData) + "' data-type='itemctrl' data-dtayp='" + this.Type + "'/>");
            return input;
        },
        AddCtrlEvent: function(ctrl, td) {
            ctrl.on("change", function() {
                td.data("val", $(this).val());
            });
        },
        CheckVerifyEmpty: function(input) {
            if (this.verfiy.Required && $(input).val().length == 0) {
                $(input).parent().find("span[data-type='verifty']").html(Constants.unallowblank);
                return false;
            }
            return true;
        },
        CheckVerifyLength: function(input) {
            if (this.verfiy.MaxLength && this.verfiy.MaxLength > 0 && this.verfiy.MaxLength < $(input).val().length) {
                $(input).parent().find("span[data-type='verifty']").html(Constants.charlencannotbigthan.replace("{#}", this.verfiy.MaxLength));
                return false;
            }
            return true;
        },
    });

    ANumColumn = function(MapID, Name, ID, Type, AlanTableVerifty) {
        this.init(MapID, Name, ID, Type, 'num', AlanTableVerifty);
        if (!this.verfiy.RegExps || this.verfiy.RegExps == '') {
            if (this.verfiy.MaxLength && this.verfiy.MaxLength > 0) {
                if (this.verfiy.Decimals && this.verfiy.Decimals > 0) {
                    this.verfiy.RegExps = "/^[+-]?\\d{1," + (this.verfiy.MaxLength - this.verfiy.Decimals) + "}(.\\d{1," + this.verfiy.Decimals + "})?$/ig";
                } else {
                    this.verfiy.RegExps = "/^[+-]?\\d{1," + this.verfiy.MaxLength + "}$/ig";
                }
            }
        }
    }

    ANumColumn.prototype = $.extend({}, new ATxtColumn, {
        CheckVerifyRegExp: function(input) {
            if (this.verfiy.RegExps && this.verfiy.RegExps.length > 0) {
                if (!eval(this.verfiy.RegExps).test($(input).val())) {
                    $(input).parent().find("span[data-type='verifty']").html(this.verfiy.RegExpMsg);
                    return false;
                }
            }
            return true;
        },
        CheckVerifyLength: function(input) {
            //数字验证靠正则表达式
            return true;
        },
    });

    var AlanTable = function() {

    };

    AlanTable.prototype = {
        panelControl: {},
        tableControl: {},
        serviceURL: '',
        columns: {},
        params: '',
        sortDesc: false,
        sort: '',
        toolbars: [],
        operations: [],
        filter: '',
        data: [],
        dataCount: 0,
        option: {
            title: "",
            page: 0,
            limit: 20,
            postType: "post",
            dataType: "json",
            async: true,
            allowsort: true,
            showcheck: true,
            shownum: true,
            operationtitle: Constants.operation,

            tablewidth: '100%',
            selectedrowcolor: "#dfe8f6",
            gridstyle: "GridView",
            footerstyle: "GridViewFooterStyle",
            rowstyle: "GridViewRowStyle",
            selectedrowstyle: "GridViewSelectedRowStyle",
            pagerstyle: "GridViewPagerStyle",
            alternatingrowstyle: "GridViewAlternatingRowStyle",
            headerstyle: "GridViewHeaderStyle",
            panelboxstyle: "ATablePanelBox",
            toolbarstyle: "ToolBar",

            fistpageimg: "/Scripts/Alan/images/icons/arrow_start.png",
            lastpageimg: "/Scripts/Alan/images/icons/arrow_end.png",
            prevpageimg: "/Scripts/Alan/images/icons/arrow__180.png",
            nextpageimg: "/Scripts/Alan/images/icons/arrow.png",

            fistpageimg_unable: "/Scripts/Alan/images/icons/arrow_starts.png",
            lastpageimg_unable: "/Scripts/Alan/images/icons/arrow_ends.png",
            prevpageimg_unable: "/Scripts/Alan/images/icons/arrow__180s.png",
            nextpageimg_unable: "/Scripts/Alan/images/icons/arrows.png",

            refushimg: "/Scripts/Alan/images/icons/arrow_circle_double.png",
            exportpageimg: "/Scripts/Alan/images/icons/Export.png",
            exportallimg: "/Scripts/Alan/images/icons/document-excel-csv.png",
            showpagefoot: true,
            showexport: false,
        },
    };

    AlanTable.prototype.GetRequest = function(callback) {
        $.ajax({
            url: this.serviceURL,
            type: this.option.postType,
            dataType: this.option.dataType,
            async: this.option.async,
            data: this.CreateURLParams(),
            success: function(json) {
                if (json.result == 1) {
                    callback(json.returnData, json.recordCout);
                } else {
                    console.log('error');
                }
            },
            error: function(request, status, errorTxt) {
                console.log(errorTxt);
            }
        });
    };

    AlanTable.prototype.GetData = function() {
        var result = [];
        var fun_getdata_main = this;
        $.each($(this.tableControl).find("tbody").children(), function(i, row) {
            result.push(fun_getdata_main.GetRowData(row));
        });
        return result;
    }

    AlanTable.prototype.GetRowData = function(row) {
        var entity = {};
        $.each(this.columns, function(j, col) {
            entity[col.ColumnID] = $(row).find("td[data-id='" + col.ColumnID + "']").data("val");
        });
        return entity;
    }

    AlanTable.prototype.GetSelectedData = function() {
        var result = [];
        var fun_getseldata_main = this;
        $(fun_getseldata_main.tableControl.find("tbody tr [data-type='itemchk']:checked")).each(function() {
            result.push(fun_getseldata_main.GetRowData($(this).parent().parent()));
        });
        return result;
    }

    AlanTable.prototype.GetChangedData = function() {
        var result = [];
        var fun_getchangeddata_main = this;
        $.each(fun_getchangeddata_main.tableControl.find("tbody tr"), function(i, row) {
            $(row).find("td").each(function() {
                if ($(this).data("type") === 'itemdata' && $(this).data("val") != $(this).data("oldval")) {
                    result.push(fun_getchangeddata_main.GetRowData($(row)));
                    return false;
                }
            });
        });
        return result;
    }

    AlanTable.prototype.InitCheckBox = function() {
        var mainChk = this.tableControl.find("thead tr [data-type='mainChk']");
        if (mainChk != null) {
            var fun_initchk_main = this;
            $(mainChk[0]).on("click", fun_initchk_main.panelControl, function() {
                $(fun_initchk_main.tableControl.find("tbody tr [data-type='itemchk']")).each(function() {
                    $(this).prop("checked", $(mainChk).prop("checked"));
                });
            });
        }
    }

    AlanTable.prototype.InitSortFunc = function() {
        var fun_sortfun_main = this;
        var mainChk = this.tableControl.find("thead tr [data-type='header']").each(function() {
            $(this).on("click", fun_sortfun_main.panelControl, function() {
                if (fun_sortfun_main.sort == $(this).data("id")) {
                    fun_sortfun_main.sortDesc = !fun_sortfun_main.sortDesc;
                } else {
                    fun_sortfun_main.sortDesc = false;
                    fun_sortfun_main.sort = $(this).data("id");
                }
                fun_sortfun_main.ReLoad()
            });
        });
    }

    AlanTable.prototype.CreateURLParams = function() {
        var result = this.params + "&start=" + this.option.page + "&limit=" + this.option.limit + "&filter=" + this.filter;
        if (this.option.allowsort) {
            result += "&sort=" + this.sort + "&direction=" + this.sortDesc;
        }
        return result;
    }

    AlanTable.prototype.Init = function() {
        this.InitCheckBox();
        if (this.option.allowsort) //如果允许排序，加入排序事件
            this.InitSortFunc();
    }

    AlanTable.prototype.VerifyData = function() {
        var result = true;
        var fun_verifydata_main = this;
        this.tableControl.find("span[data-type='verifty']").html("");
        $.each(this.columns, function(i, col) {
            $.each($(fun_verifydata_main.tableControl).find("[data-id='itemctrl-" + this.ColumnID + "']"), function(j, input) {
                if (!col.CheckVerify(input))
                    result = false;
            });
        });
        return result;
    }

    AlanTable.prototype.CreateColumn = function(col, rowData) {
        var td = $("<td data-mapid='" + col.GetMappingID() + "' data-type='itemdata' data-dtype='" + col.Type + "' data-id='" + col.ColumnID + "' data-val='" + col.GetColData(rowData) + "' data-oldval='" + col.GetColData(rowData) + "' >" + "</td>");
        var input = col.GetControl(rowData);
        td.append(input);
        col.AddVerifty(input, td);
        return td;
    }

    //为输入控件添加自定义错误信息
    AlanTable.prototype.UpdateItemErrorMsg = function(columnId, rowIndex, message) {
        var result = false;
        var fun_updateitemerrormsg_main = this;
        $.each(this.columns, function(i, col) {
            if (col.ColumnID == columnId) {
                var row = $(fun_updateitemerrormsg_main.tableControl).find("td[data-id='" + this.ColumnID + "']").eq(rowIndex);
                result = col.UpdateVerifyErrorMsg(row, message);
                return result;
            }
        });
        return result;
    }

    AlanTable.prototype.LoadPageFoot = function(recordCount) {
        if (this.option.showpagefoot) {
            var fun_loadpagefoot_main = this;
            var maxPage = Math.ceil(recordCount / this.option.limit);
            var footPanel = $("<div data-type='pagePanel' class='" + this.option.panelboxstyle + "' style='text-align:left'></div>");
            footPanel.append($("<label style='padding-left:20px'>" + Constants.pageindex.replace("{#}", (this.option.page + 1)) + "&nbsp;/&nbsp;" + Constants.allpage.replace("{#}", maxPage) + "</label>"));

            if (this.option.page <= 0) {
                footPanel.append($("<img style='padding-left:20px;vertical-align:middle;cursor:pointer' data-type='firstpage' title=\"" + Constants.firstpage + "\" alt=\"" + Constants.firstpage + "\" src='" + this.option.fistpageimg_unable + "' data-disabled='true'/>"));
                footPanel.append($("<img style='padding-left:10px;vertical-align:middle;cursor:pointer' data-type='prevpage' title=\"" + Constants.prevpage + "\" alt=\"" + Constants.prevpage + "\" src='" + this.option.prevpageimg_unable + "' data-disabled='true'/>"));
            } else {
                footPanel.append($("<img style='padding-left:20px;vertical-align:middle;cursor:pointer' data-type='firstpage' title=\"" + Constants.firstpage + "\" alt=\"" + Constants.firstpage + "\" src='" + this.option.fistpageimg + "' data-disabled='false'/>"));
                footPanel.append($("<img style='padding-left:10px;vertical-align:middle;cursor:pointer' data-type='prevpage' title=\"" + Constants.prevpage + "\" alt=\"" + Constants.prevpage + "\" src='" + this.option.prevpageimg + "' data-disabled='false'/>"));
            }

            if (this.option.page >= (maxPage - 1)) {
                footPanel.append($("<img style='padding-left:10px;vertical-align:middle;cursor:pointer' data-type='nextpage' title=\"" + Constants.nextpage + "\" alt=\"" + Constants.nextpage + "\" src='" + this.option.nextpageimg_unable + "' data-disabled='true'/>"));
                footPanel.append($("<img style='padding-left:10px;vertical-align:middle;cursor:pointer' data-type='lastpage' title=\"" + Constants.lastpage + "\" alt=\"" + Constants.lastpage + "\" src='" + this.option.lastpageimg_unable + "' data-disabled='true'/>"));
            } else {
                footPanel.append($("<img style='padding-left:10px;vertical-align:middle;cursor:pointer' data-type='nextpage' title=\"" + Constants.nextpage + "\" alt=\"" + Constants.nextpage + "\" src='" + this.option.nextpageimg + "' data-disabled='false'/>"));
                footPanel.append($("<img style='padding-left:10px;vertical-align:middle;cursor:pointer' data-type='lastpage' title=\"" + Constants.lastpage + "\" alt=\"" + Constants.lastpage + "\" src='" + this.option.lastpageimg + "' data-disabled='false'/>"));
            }

            footPanel.append($("<img style='padding-left:15px;vertical-align:middle;cursor:pointer' data-type='refreshpage' title=\"" + Constants.refreshpage + "\" alt=\"" + Constants.refreshpage + "\" src='" + this.option.refushimg + "'>"));
            if (this.option.showexport) {
                footPanel.append($("<img style='padding-left:15px;vertical-align:middle;cursor:pointer' data-type='exportpage' title=\"" + Constants.exportpage + "\" alt=\"" + Constants.exportpage + "\" src='" + this.option.exportpageimg + "'>"));
                footPanel.append($("<img style='padding-left:10px;vertical-align:middle;cursor:pointer' data-type='exportallpage' title=\"" + Constants.exportallpage + "\" alt=\"" + Constants.exportallpage + "\" src='" + this.option.exportallimg + "'>"));
            }
            footPanel.append($("<label style='padding-left:20px'>" + Constants.pageshowrecord.replace("{#}", this.option.limit) + "&nbsp/&nbsp" + Constants.allrecord.replace("{#}", recordCount) + "</label>"))

            var pangePanel = this.panelControl.find("div[data-type='pagePanel']");
            if (pangePanel.length > 0)
                $(pangePanel[0]).remove();
            this.panelControl.append(footPanel);

            var firstPage = footPanel.find("img[data-type='firstpage']")[0];
            $(firstPage).on("click", function() {
                if (!$(this).data("disabled")) {
                    fun_loadpagefoot_main.option.page = 0;
                    fun_loadpagefoot_main.ReLoad();
                }
            });

            var lastPage = footPanel.find("img[data-type='lastpage']")[0];
            $(lastPage).on("click", function() {
                if (!$(this).data("disabled")) {
                    fun_loadpagefoot_main.option.page = maxPage - 1;
                    fun_loadpagefoot_main.ReLoad();
                }
            });

            var nextPage = footPanel.find("img[data-type='nextpage']")[0];
            $(nextPage).on("click", function() {
                if (!$(this).data("disabled")) {
                    fun_loadpagefoot_main.option.page++;
                    fun_loadpagefoot_main.ReLoad();
                }
            });

            var prevPage = footPanel.find("img[data-type='prevpage']")[0];
            $(prevPage).on("click", function() {
                if (!$(this).data("disabled")) {
                    fun_loadpagefoot_main.option.page--;
                    fun_loadpagefoot_main.ReLoad();
                }
            });

            var refushPage = footPanel.find("img[data-type='refreshpage']")[0];
            $(refushPage).on("click", function() {
                if (!$(this).prop("disabled")) {
                    fun_loadpagefoot_main.ReLoad();
                }
            });

            var exportPage = footPanel.find("img[data-type='exportpage']")[0];
            $(exportPage).on("click", function() {
                alert(Constants.unopenfunction);
            });

            var exportAllPage = footPanel.find("img[data-type='exportallpage']")[0];
            $(exportAllPage).on("click", function() {
                alert(Constants.unopenfunction);
            });
        }
    }

    //局部数据更新 数据集合，匹配关键字，更新关键字， Adata关键字， aData更新关键字
    AlanTable.prototype.UpdateData = function(newData, newDataKeyMap, newDataValueMap, inlineKeyMap, inlineValueMap) {
        var fun_updatedata_main = this;
        var MKeyCol = newDataKeyMap,
            MValCol = newDataValueMap;
        if (inlineKeyMap)
            MKeyCol = inlineKeyMap;
        if (inlineValueMap)
            MValCol = inlineValueMap;

        $.each(this.data, function(i, dataitem) {
            $.each(newData, function(j, newdataitem) {
                if (dataitem[MKeyCol] === newdataitem[newDataKeyMap]) {
                    fun_updatedata_main.data[i][MValCol] = newdataitem[newDataValueMap];
                }
            })
        });
        this.ReLoadData();
    }

    //根据数据重新加载表单
    AlanTable.prototype.ReLoadData = function(data, dataCount) {
        if (data)
            this.data = data;
        if (dataCount)
            this.dataCount = dataCount;
        var fun_reloaddata_main = this;

        var tbody = fun_reloaddata_main.tableControl.find('tbody');
        tbody.empty();
        $.each(fun_reloaddata_main.data, function(i, data) {
            var brow;
            if (i % 2 == 0)
                brow = $("<tr class='" + fun_reloaddata_main.option.rowstyle + "'></tr>");
            else
                brow = $("<tr class='" + fun_reloaddata_main.option.alternatingrowstyle + "'></tr>");
            //鼠标事件
            brow.on("mouseover", function() {
                this.currentcolor = this.style.backgroundColor;
                this.style.backgroundColor = fun_reloaddata_main.option.selectedrowcolor;
                this.style.fontWeight = ''
            });
            brow.on("mouseout", function() {
                this.style.backgroundColor = this.currentcolor;
                this.style.fontWeight = ''
            });
            var btd = '';
            if (fun_reloaddata_main.option.showcheck) {
                brow.append($("<td><input type='checkbox' data-type='itemchk' data-row='" + i + "' /></td>"));
            }
            if (fun_reloaddata_main.option.shownum) {
                brow.append($("<td data-type='itemnum'>" + (fun_reloaddata_main.option.page * fun_reloaddata_main.option.limit + i + 1) + "</td>"));
            }
            $.each(fun_reloaddata_main.columns, function(j, col) {
                brow.append(fun_reloaddata_main.CreateColumn(col, data));
            });

            if (fun_reloaddata_main.operations && fun_reloaddata_main.operations.length > 0) {
                var optd = $("<td data-type='itemoperation'></td>");
                $(fun_reloaddata_main.operations).each(function() {
                    var operationitem = this;
                    var item = $("<a style='padding-left:5px' href='#'><img src='" + this.ImgUrl + "' title='" + this.Name + "' alt='" + this.Name + "'/></a>");
                    item.on("click", function() {
                        operationitem.Action.call(this, fun_reloaddata_main.GetRowData(brow), i);
                    });
                    optd.append(item);
                });
                brow.append(optd);
            }
            tbody.append(brow);
        });
        fun_reloaddata_main.tableControl.append(tbody);
        fun_reloaddata_main.LoadPageFoot(fun_reloaddata_main.dataCount);
    }

    //从远程获取数据
    AlanTable.prototype.ReLoad = function(serviceURL, params) {
        var fun_reload_main = this;
        if (serviceURL)
            fun_reload_main.serviceURL = serviceURL;
        if (params)
            fun_reload_main.params = params;
        fun_reload_main.GetRequest(function(returnData, recordCount) {
            fun_reload_main.ReLoadData(returnData, recordCount);
        });
    }

    $.fn.TransAlanTable = function(serviceURL, params, filter, columns, options, toolbars, operations) {
        var atable = new AlanTable();
        $(this).empty();
        //属性赋值
        atable.params = params;
        atable.columns = columns;
        atable.filter = filter;
        atable.serviceURL = serviceURL;
        if (toolbars) {
            atable.toolbars = toolbars;
        }
        if (operations) {
            atable.operations = operations;
        }
        atable.panelControl = this;
        atable.option = $.extend({}, atable.option, {
            title: this.data("title"),
            page: this.data("page"),
            limit: this.data("limit"),
            postType: this.data("posttype"),
            dataType: this.data("datatype"),
            async: this.data("async"),
            allowsort: this.data("allowsort"),
            showcheck: this.data("showcheck"),
            shownum: this.data("shownum"),
            tablewidth: this.data("tablewidth"),
            selectedrowcolor: this.data("selectedrowcolor"),
            gridstyle: this.data("gridstyle"),
            footerstyle: this.data("footerstyle"),
            rowstyle: this.data("rowstyle"),
            selectedrowstyle: this.data("selectedrowstyle"),
            pagerstyle: this.data("pagerstyle"),
            alternatingrowstyle: this.data("alternatingrowstyle"),
            headerstyle: this.data("headerstyle"),
            panelboxstyle: this.data("titlestyle"),
            toolbarstyle: this.data("toolbarstyle"),
            operationtitle: this.data("operationtitle"),
        }, options);

        if (atable.toolbars && atable.toolbars.length > 0) {
            var panelbox = $("<div data-type='toolbarPanel' class='" + atable.option.panelboxstyle + "'></div>");
            var toolbar = $("<div class='" + atable.option.toolbarstyle + "'></div>");
            var ul = $("<ul></ul>");
            $(atable.toolbars).each(function() {
                var entity = this;
                var li = $("<li></li>");
                var item = $("<a href='#'><img src='" + this.ImgUrl + "'/>&nbsp;" + this.Name + "</a>");
                item.on("click", function() {
                    entity.Action.call();
                });
                li.append(item);
                ul.append(li);
            });
            toolbar.append(ul);
            panelbox.append(toolbar);
            this.append(panelbox);
        }

        if (atable.option.title.length > 0) {
            var titlePanel = $("<div data-type='titlePanel' class='" + atable.option.panelboxstyle + "'>" + atable.option.title + "</div>");
            this.append(titlePanel);
        }

        var alanTableHtml = $("<table class='" + atable.option.gridstyle + "' width='" + atable.option.tablewidth + "'><thead></thead><tbody></tbody><tfoot></tfoot></table>");
        var thead = alanTableHtml.find("thead");
        var hrow = $("<tr class='" + atable.option.headerstyle + "'></tr>");
        var htd = '';
        if (atable.option.showcheck) {
            hrow.append($("<th><input type='checkbox' data-type='mainChk' /></th>"));
        }
        if (atable.option.shownum) {
            hrow.append($("<th data-type='sortnum'>" + Constants.nostr + "</th>"));
        }
        $(atable.columns).each(function() {
            //初始化表头，排序是判断data-type='header'参与排序
            if (this.Ctrl == 'hidden') //隐藏字段表头不显示
                hrow.append($("<th style='display:none' data-type='hidheader' data-id='" + this.GetMappingID() + "' >" + this.ColumnName + "</th>"));
            else if (typeof(this.ColumnMapping) == 'function') //自定义字段不参与排序
                hrow.append($("<th data-type='cusheader' data-id='" + this.GetMappingID() + "' >" + this.ColumnName + "</th>"));
            else
                hrow.append($("<th data-type='header' data-id='" + this.GetMappingID() + "' ><a href='#'>" + this.ColumnName + "</a></th>"));
        });
        if (atable.operations && atable.operations.length > 0) {
            hrow.append($("<th data-type='operationheader' >" + atable.option.operationtitle + "</th>"));
        }
        thead.append(hrow);
        alanTableHtml.append(thead);

        atable.ReLoad();

        atable.tableControl = alanTableHtml;
        atable.panelControl.append(alanTableHtml);
        atable.Init();
        return atable;
    };

})();