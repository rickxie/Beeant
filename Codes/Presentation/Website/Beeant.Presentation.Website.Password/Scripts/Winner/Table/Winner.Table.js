Winner.Table = function (id, config) {
    this.Container = document.getElementById(id);
    this.Base = new Winner.ClassBase();
    if (this.Container == null || this.Container.rows.length == 0) {
        return;
    }
    this.RowStartIndex = 1;
    this.RowEndIndex = this.Container.rows.length;
    this.SelectIndex = null;
    this.IsMoveWidth = true;
    this.IsMoveHeight = false;
    this.MoveWidth = { MoveTd: null, ChangeTd: null, IsLastTd: false, ClientX: [], MoveTdClientWidth: [], ChangeTdClientWidth: [],Margin:5 };
    this.MoveHeight = { MoveTr: null, ClientY: [], MoveTrClientHeight: [] };
    this.StyleFile = "/scripts/Winner/Table/Styles/Style.css";
    if (config != undefined) {
        this.Base.LoadConfig(this, config);
    }
};
Winner.Table.prototype = {
    Initialize: function() { //加载css样式文件
        this.Base.LoadCssFile(this.StyleFile);
        if (this.Container && (this.Container.className == undefined || this.Container.className == ""))
            this.Container.className = "wintable";
        this.BindEvent();
        this.InitializeMove();
    },
    InitializeMove: function () {//初始化移动的Td
        if (!this.IsMoveWidth && !this.IsMoveHeight) return;
        var self = this;
        var func = function () {
            self.InitlizeMoveTd();
        };
        setTimeout(func, 1000);
    },
    BindEvent: function () {//绑定事件
        this.BindMoveWidthEvent();
        this.BindMoveHeight();
        this.BindRowEvent();
    },
    BindMoveHeight: function () {//绑定移动高度
        if (!this.IsMoveHeight || this.RowEndIndex <= 0) return;
        for (var i = this.RowStartIndex; i < this.RowEndIndex; i++) {
            this.BindTrMoveHeightEvent(this.Container.rows[i]);
        }
    },
    BindTrMoveHeightEvent: function (tr) {//移动事件
        var self = this;
        this.Base.BindEvent(document, "mousemove", function (event) { self.ChangeTrHeight(event); });
        this.Base.BindEvent(tr, "mousedown", function (event) { self.BeginMoveHeight(tr, event); });
        this.Base.BindEvent(document, "mouseup", function () { self.EndMoveHeight(); });
    },
    BindMoveWidthEvent: function () {//移动宽度
        if (!this.IsMoveWidth || this.Container.rows.length <= 0 || this.Container.rows[0].cells.length <= 1) return;
        for (var i = 0; i < this.Container.rows[0].cells.length; i++) {
            this.BindTdMoveWitdhEvent(this.Container.rows[0].cells[i], i);
        }
    },
    BindTdMoveWitdhEvent: function (td, index) {//移动事件
        var self = this;
        this.Base.BindEvent(document, "mousemove", function (event) { self.ChangeTdWidth(event); });
        this.Base.BindEvent(td, "mousedown", function (event) { self.BeginMoveWidth(td, index, event); });
        this.Base.BindEvent(document, "mouseup", function () { self.EndMoveWidth(); });
    },
    BindRowEvent: function () {//鼠标移入事件
        for (var i = this.RowStartIndex; i < this.RowEndIndex; i++) {
            this.Container.rows[i].className = "out";
            this.BindRowClassEvent(this.Container.rows[i], "mouseover", "over");
            this.BindRowClassEvent(this.Container.rows[i], "mouseout", "out");
            this.BindRowSelectEvent(this.Container.rows[i], "mouseout", "out");
        }
    },

    BindRowClassEvent: function (tr, eventName, className) {//绑定事件
        var self = this;
        this.Base.BindEvent(tr, eventName, function () {
            self.ChangeClassName(tr, className);
        });
    },
    BindRowSelectEvent: function (tr) { //绑定选择事件
        var self = this;
        this.Base.BindEvent(tr, "click", function () {
            self.Select(tr);
        });
    },
    ChangeClassName: function (tr, className) {//改变样式
        if (tr.className != "select") {
            tr.className = className;
        }
    },
    Select: function (row) {//选择事件
        if (this.SelectRow != null) {
            this.SelectRow.className = "out";
        }
        row.className = "select";
        this.SelectRow = row;
    },
    InitlizeMoveTd: function () {//初始化表格
        for (var i = 0; i < this.Container.rows[0].cells.length; i++) {
            var td = this.Container.rows[0].cells[i];
            td.style.width = td.clientWidth == 0 ? document.body.clientWidth / this.Container.rows[0].cells.length : td.clientWidth + "px";
        }
        this.Container.style.tableLayout = "fixed";
    },
    BeginMoveWidth: function (td, index, event) {//开始移动
        this.MoveWidth.IsLastTd = index == (this.Container.rows[0].cells.length - 1);
        var nextIndex = this.MoveWidth.IsLastTd ? index - 1 : index + 1;
        this.SetMoveWidthInfo(td, index, nextIndex, event);
    },
    EndMoveWidth: function () {//结束移动
        if (this.MoveWidth.MoveTd == null) return;
        this.MoveWidth.MoveTd.style.cursor = "auto";
        this.MoveWidth.MoveTd = null;
        this.MoveWidth.ChangeTd = null;
    },
    SetMoveWidthInfo: function (td, index, nextIndex, event) {//设置宽度属性
        this.MoveWidth.MoveTd = td;
        this.MoveWidth.ClientX = event.clientX;
        this.MoveWidth.ChangeTd = this.Container.rows[0].cells[nextIndex];
        this.MoveWidth.MoveTdClientWidth = this.MoveWidth.MoveTd.clientWidth;
        this.MoveWidth.ChangeTdClientWidth = this.MoveWidth.ChangeTd.clientWidth;
        this.MoveWidth.MoveTd.style.cursor = "move";
    },
    ChangeTdWidth: function (event) {//改变Td宽度
        if (this.MoveWidth.MoveTd == null || this.MoveWidth.ChangeTd == null) return;
        var step = event.clientX - this.MoveWidth.ClientX;
        step = this.MoveWidth.IsLastTd ? 0 - step : step;
        this.SetTdWidth(step);
        return;
    },
    SetTdWidth: function (step) {//设置Td宽度
        if (this.MoveWidth.ChangeTdClientWidth - step <= this.MoveWidth.Margin) {
            this.MoveWidth.MoveTd.style.width = this.MoveWidth.MoveTdClientWidth + this.MoveWidth.ChangeTdClientWidth - this.MoveWidth.Margin + "px";
            this.MoveWidth.ChangeTd.style.width = this.MoveWidth.Margin + "px";
        }
        else if (this.MoveWidth.MoveTdClientWidth + step <= this.MoveWidth.Margin) {
            this.MoveWidth.MoveTd.style.width = this.MoveWidth.Margin + "px";
            this.MoveWidth.ChangeTd.style.width = this.MoveWidth.MoveTdClientWidth + this.MoveWidth.ChangeTdClientWidth - this.MoveWidth.Margin + "px";
        } else {
            this.MoveWidth.MoveTd.style.width = this.MoveWidth.MoveTdClientWidth + step + "px";
            this.MoveWidth.ChangeTd.style.width = this.MoveWidth.ChangeTdClientWidth - step + "px";
        }
    },
    BeginMoveHeight: function (tr, event) {//开始移动
        this.MoveHeight.MoveTr = tr;
        this.MoveHeight.ClientY = event.clientY;
        this.MoveHeight.MoveTrClientHeight = this.MoveHeight.MoveTr.clientHeight;
        this.MoveHeight.MoveTr.style.cursor = "move";
    },
    EndMoveHeight: function () {//结束移动
        if (this.MoveHeight.MoveTr == null) return;
        this.MoveHeight.MoveTr.style.cursor = "auto";
        this.MoveHeight.MoveTr = null;
    },
    ChangeTrHeight: function (event) {//改变Td宽度
        if (this.MoveHeight.MoveTr == null) return;
        var step = event.clientY - this.MoveHeight.ClientY;
        this.SetTrHeight(step);
        return;
    },
    SetTrHeight: function (step) {//设置Td宽度
        this.MoveHeight.MoveTr.style.height = this.MoveHeight.MoveTrClientHeight + step + "px";
    }
};
