(function() {
    var Constants = {
        updatetitle: '修改',
	    newtitle: '新增',
        operation: '操作',
        deletetitle: '删除',
        nostr: '序号',
    };

    NancyGrid = function() {

    };

    NewToolBar = function(Name, Url) {
	    if(Url)
		    this.ImgUrl = "";
	    if(Name)
		    this.Name = Name;
	    else
		    this.Name = Constants.newtitle;
        this.Action = function() {
            this.isNewData = true;
		    this.resetDialog();
		    this.ShowDialog();
        };
    };

    DelToolBar = function(Name,Url) {
        if(Url)
		    this.ImgUrl = "";
	    if(Name)
		    this.Name = Name;
	    else
		    this.Name = Constants.deletetitle;
        this.Action = function() {
		    var grid = this;
		    var delTr = $(this.panelControl).find("tbody").find("input:checkbox:checked").closest('tr');
		    $(delTr).remove();
            $(this.panelControl).find("thead tr [data-type='mainChk']").prop('checked','');
		    grid.SaveEntities();
            grid.ReLoad();
        };
    };
    UpdateOperation = function(Name, Url) {
	    if(Url)
		    this.ImgUrl = "";
	    if(Name)
		    this.Name = Name;
	    else
		    this.Name = Constants.updatetitle;
        this.Action = function(operation) {
		
            this.isNewData = false;
		    this.resetDialog();
		    this.LoadToDialog(operation.index);
		    this.ShowDialog(operation);
		
        };
    };


    DelOperation = function(Name, Url) {
	    if(Url)
		    this.ImgUrl = "";
	    if(Name)
		    this.Name = Name;
	    else
		    this.Name = Constants.deletetitle;
        this.Action = function(operation) {
		    this.DeleteRow(operation.index);
        };
    };  


    NancyCol = function(name, bandingName) {
        this.init(name, bandingName);
    };

    NancyCol.prototype = {
        Name: '',
        Type:'Text',
        BandingName: '', //映射字段ID
        init: function(name, bandingName) {
            this.Name = name;
            this.BandingName = bandingName;
        },
    };
    
    NancyHidCol = function(name, bandingName) {
        this.init(name, bandingName);
    };
    
      NancyHidCol.prototype = {
        Name: '',
        Type:'Hid',
        BandingName: '', //映射字段ID
        init: function(name, bandingName) {
            this.Name = name;
            this.BandingName = bandingName;
        },
    };
        
     NancySelCol = function(name, bandingName) {
        this.init(name, bandingName);
    };

    NancySelCol.prototype = {
        Name: '',
        Type:'Sel',
        BandingName: '', //映射字段ID
        init: function(name, bandingName) {
            this.Name = name;
            this.BandingName = bandingName;
        },
    };



    NancyGrid.prototype = {
        data: [],
	    dialogTarget: null,
        toolbars: [],
        operations: [],
        dataCount: 0,
        panelControl: null,
        columns: null,
        dialog: null,
        saveCtrl: null,
	    isNewData: false,
        option: {
            title: "",
            showcheck: true,
            shownum: true,
            tablewidth: '100%',
            selectedrowcolor: "#dfe8f6",
            gridstyle: "GridView",
            footerstyle: "GridViewFooterStyle",
            rowstyle: "GridViewRowStyle",
            selectedrowstyle: "GridViewSelectedRowStyle",
            pagerstyle: "GridViewPagerStyle",
            alternatingrowstyle: "GridViewAlternatingRowStyle",
            headerstyle: "GridViewHeaderStyle",
            toolbarPanelbox: "toolbarPanelbox",
            titlePanelbox: "titlePanelbox",
            toolbarstyle: "ToolBar",
            showpagefoot: true,
        },
	    Clone: function (obj) {  //克隆
            function func(){}
            func.prototype = obj;
            var o = new func();  
            for(var a in o){  
                if(typeof o[a] == "object") {
                    o[a] = this.Clone(o[a]);  
                }  
            }  
            return o;  
        },
        StyleFile:"/scripts/Nancy/NancyGrid/Styles/Style.css"
    };

    //把Entity添加到新的行
    NancyGrid.prototype.AppendRow = function(entity) {
        var grid = this;
        var $tbody = $(this.panelControl).find("tbody");
	    var num_Tr = $tbody.find('tr').length;
        var brow = $("<tr class='" + this.option.rowstyle + "'></tr>");
        if(this.option.showcheck)
            brow.append($("<td><input type='checkbox' data-type='itemchk' data-row='" + num_Tr + "' /></td>")); //添加checkbox
        if(this.option.shownum)
            brow.append($("<td data-type='itemnum'>" + (num_Tr+1) + "</td>")); //显示序号
	    $(grid.columns).each(function(){
		    if(this.Type == 'Sel'){
			    var optd = $("<td data-type='sel' data-bindname='" + this.BandingName + "' data-val='" + entity[this.BandingName] + "'>" +  entity[this.BandingName + "-text"] + "</td>");
		    }
	        else if (this.Type == 'Hid') {
		        var optd = $("<td data-type='hid' style='display:none' data-bindname='" + this.BandingName + "' data-val='" + entity[this.BandingName] + "'>" +  entity[this.BandingName + "-text"] + "</td>");
		    }
		    else{
			    var optd = $("<td data-type='text' data-bindname='" + this.BandingName + "' data-val='" + entity[this.BandingName] + "'>" +  entity[this.BandingName] + "</td>");
		    }
		    brow.append(optd);
	    });
	    if(this.operations.length > 0){
		    var optd = $("<td data-type='operation'></td>");
		    $(this.operations).each(function() {
			    var operation = grid.Clone(this);
			    operation.index = num_Tr;
			    //var item = $("<a href='#'><img src='" + this.ImgUrl + "'/>&nbsp;" + this.Name + "</a>");
			      var item = $("<a href='#'>" + this.Name + "</a>");
				    item.on("click", function() {
				    operation.Action.call(grid, operation);
			    });
			    optd.append(item);
		    });
		    brow.append(optd);
	    }
        $tbody.append(brow);
        this.SaveEntities();
    };

    NancyGrid.prototype.ShowDialog = function(operation){
	    var ngrid = this;
	    $(this.dialogTarget.Detail).append($(this.dialog));
	    $(this.dialog).show();
	    this.dialogTarget.ShowDialog();
	    this.dialogTarget.SureFunction = null;
	    this.dialogTarget.SureFunction = function () {
	        var rev = true;
	         if (ngrid.Validator != undefined) {
	            rev = ngrid.Validator.ValidateSubmit();
	        }
	        if (!rev) {
	            return false;
	        }
		    if(ngrid.isNewData){
			    ngrid.SaveFromDialog(0);
			    ngrid.isNewData = false;
		    }
		    else
		        ngrid.SaveFromDialog(operation.index);
	    };
    };
    
    NancyGrid.prototype.resetDialog = function() {
        $(this.dialog).find('*[data-bindname]').each(function(i, sender) {
            if (sender.nodeName == 'SELECT') {
                $(sender).val('selected', '');
                ;
            } else {
                $(sender).val('');
            }
        });
    };
    
    //修改对应行的数据
    NancyGrid.prototype.ModifyRow = function(entity, i) {
	    var tBody = this.panelControl.find("tbody");
	    var tds = tBody.find('tr').eq(i).find('td');
	    tds.each(function(j,item){
		    for(var att in entity){
			    if ($(item).data('bindname') == att){
				    if($(item).data('type') == 'sel')
				    {
					    $(item).html(entity[att + "-text"]);
				    }
				    else
				    {
					    $(item).html(entity[att]);
				    }
				    $(item).attr("data-val",entity[att]);  //每次保存 更改自定义属性data-val
			    }   
		    }
	    });
        this.SaveEntities();
    };
    
    NancyGrid.prototype.SaveFromDialog = function(i){ 
	    var entity = {}; 
	    var iTems= this.dialog.find('*[data-bindname]');//弹出框中包含bindName属性的元素
	    $.each(iTems,function(j,item){
		    if($(item).get(0).nodeName == 'SELECT'){
			    entity[$(item).attr('data-bindname')] = $(item).find("option:selected").val();
			    entity[$(item).attr('data-bindname') + '-text'] = $(item).find("option:selected").text();
		    }else{
			    entity[$(item).attr('data-bindname')] = $(item).val();
		    }
	    });	
	    if(this.isNewData)          //判断是否是新的数据
		    this.AppendRow(entity);
	    else
		    this.ModifyRow(entity, i);
    };

    NancyGrid.prototype.LoadToDialog = function(i) {
	    var divDialog = $(this.dialog);
	    $.each($(this.panelControl).find("tbody").find('tr').eq(i).find('td[data-bindname]'),function(i,item){
		    var col = item;
		    $.each(divDialog.find('*[data-bindname]'),function(j,ctrl){
			    if($(ctrl).data("bindname") == $(col).data("bindname")){	
				    $(ctrl).val($(col).attr("data-val"));   //每次加载，将自定义属性data-val赋值给控件（包括select属性）
			    }
	       });
         });
    };

    NancyGrid.prototype.InitCheckBox = function() {
	    var mainChk = this.panelControl.find("thead tr [data-type='mainChk']");
	    if (mainChk != null) {
		    var fun_initchk_main = this;
		    $(mainChk[0]).on("click", fun_initchk_main.panelControl, function() {
			    $(fun_initchk_main.panelControl.find("tbody tr [data-type='itemchk']")).each(function() {
				    $(this).prop("checked", $(mainChk).prop("checked"));
			    });
		    });
	    }
    };

    NancyGrid.prototype.DeleteRow = function(i) {
	    var TRS = $(this.panelControl).find('tbody').find('tr');
	    $(this.panelControl).find('tbody tr').eq(i).remove();
        this.SaveEntities();
	    this.ReLoad();
    };
    
    NancyGrid.prototype.Clear = function() {
	    $(this.panelControl).find('tbody tr').remove();
        this.SaveEntities();
	    this.ReLoad();
    };

    NancyGrid.prototype.SaveEntities = function() {
	    var This = this;
	    var trs = $(this.panelControl).find("tbody").find('tr');
	    this.data = [];
	    trs.each(function(i,item){
		    var entity={};
		    var tds = $(item).find('td');
		    $.each(This.columns, function(j,sender){   //保存数据时，与全局This.columns对应
			    $.each(tds, function(k, td){
				    if($(td).data("bindname") == sender.BandingName){
					    if(sender.Type == 'Sel')
					    entity[sender.BandingName + "-text"] = $(td).text();
					    entity[sender.BandingName] =  $(td).attr("data-val");
				    }
			    });
		    });
		    This.data.push(entity);
	    });
	    $(this.saveCtrl).val(JSON.stringify(this.data));
    };

    NancyGrid.prototype.ReLoad = function() {
        var entities = eval($(this.saveCtrl).val());
        $(this.panelControl).find("tbody").empty();
        var _parent = this;
        if (entities) {
            $.each(entities, function(i, json) {
                _parent.AppendRow(this);
            });
        }
    };

    NancyGrid.prototype.Show = function() {
        $(this).show();
    };

    $.fn.TransNancyGrid = function(dialog, saveCtrl, columns, options, toolbars, operations) {
       var ngrid = new NancyGrid();
        ngrid.option = $.extend({}, ngrid.option, {
            title: this.data("title"),
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
            toolbarPanelbox: this.data("toolbarPanelbox"),
            titlePanelbox: this.data("titlePanelbox"),
            toolbarstyle: this.data("toolbarstyle"),
            operationtitle: this.data("operationtitle"),
        }, options);
        
        var ThisBase = new Winner.ClassBase();
        ThisBase.LoadCssFile(ngrid.StyleFile);
        $(dialog).hide();    
        $(this).empty();
        ngrid.dialogTarget = new Winner.Dialog(ngrid.option.title || "操作", "", { Width: 500 });
        ngrid.dialog = dialog;
	    ngrid.dialogTarget.IsShowDialog = false;
	    ngrid.dialogTarget.Initialize();
        ngrid.saveCtrl = saveCtrl; 
        ngrid.columns = columns;
        if (toolbars) {
            ngrid.toolbars = toolbars;
        }
        if (ngrid) {
            ngrid.operations = operations;
        }
        ngrid.panelControl = this;  //所绑定的对象

        if (ngrid.toolbars && ngrid.toolbars.length > 0) {
            var panelbox = $("<div data-type='toolbarPanel' class='" + ngrid.option.toolbarPanelbox + "'></div>");
            var toolbar = $("<div class='" + ngrid.option.toolbarstyle + "'></div>");
            var ul = $("<ul></ul>");
       
            $(ngrid.toolbars).each(function() {
                var entity = this;
                var li = $("<li></li>");
			    var item = $("<a href='#'>" + this.Name + "</a>");
                item.on("click", function() {
                    entity.Action.call(ngrid);
                });
                li.append(item);
                ul.append(li);
            });
            toolbar.append(ul);
            panelbox.append(toolbar);
            this.append(panelbox);
        }
        if (ngrid.option.title.length > 0) {
            var titlePanel = $("<div data-type='titlePanel' class='" + ngrid.option.titlePanelbox + "'>" + ngrid.option.title + "</div>");
            this.append(titlePanel);
        }
        var tableHtml = $("<table class='" + ngrid.option.gridstyle + "' width='" + ngrid.option.tablewidth + "'><thead></thead><tbody></tbody><tfoot></tfoot></table>");
        var thead = tableHtml.find("thead");
        var hrow = $("<tr class='" + ngrid.option.headerstyle + "'></tr>");
        var htd = '';
        if (ngrid.option.showcheck) {
            hrow.append($("<th><input type='checkbox' data-type='mainChk' /></th>"));
        }
        if (ngrid.option.shownum) {
            hrow.append($("<th data-type='sortnum'>" + Constants.nostr + "</th>"));
        }
	    $(ngrid.columns).each(function() {
	        if(this.Type == 'Hid')
	            hrow.append($("<th data-type='header' style='display:none'><a href='#'>" + this.Name + "</a></th>"));
	        else
		        hrow.append($("<th data-type='header'><a href='#'>" + this.Name + "</a></th>"));
	    });
        if (ngrid.operations && ngrid.operations.length > 0) {
            hrow.append($("<th data-type='operationheader' >" + Constants.operation + "</th>"));
        }
        thead.append(hrow);
        tableHtml.append(thead);
        ngrid.panelControl.append(tableHtml);
        ngrid.ReLoad();
	    ngrid.InitCheckBox();
        return ngrid; 
    };
})()

 
  