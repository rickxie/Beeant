Winner = typeof(Winner)!="undefined" ? Winner : {};
Winner.TreeView = function (id, config) {
    this.Base = new Winner.ClassBase();
    this.Container = document.getElementById(id);
    if (this.Container == null)
        return;
    this.Node = { Box: [], Parent: [], Childs: [] };
    this.StyleFile = "/scripts/Winner/TreeView/Styles/Style.css";
    if (config != undefined) {
        this.Base.LoadConfig(this, config);
    }
};
Winner.TreeView.prototype = {
    Initialize: function () { //初始化
        this.Base.LoadCssFile(this.StyleFile);
        this.Container.className = "treeview";
        this.LoadControl();
    },
    LoadControl: function () {//绑定node事件
        this.FillNode(this.Container, this.Node);
    },
    FillNode: function (content, parent) {//填充节点
        if (content != null && content.childNodes != null && content.childNodes.length > 0) {
            var node = [];
            for (var i = 0; i < content.childNodes.length; i++) {
                node = this.AddNode(content.childNodes[i], parent, node);
            }
        }
    },
    AddNode: function (content, parent, node) {//添加节点
        if (content.nodeName == "TABLE") {
            node = { Box: [], Parent: parent, Childs: [] };
            this.FillBox(content, node);
            parent.Childs.push(node);
        }
        else if (content.nodeName == "DIV") {
            this.FillNode(content, node);
        }
        return node;
    },
    FillBox: function (content, node) {//填充CheckBox
        var cks = content.getElementsByTagName('input');
        if (cks != null && cks.length > 0) {
            node.Box = cks[0];
            this.BindNodeEvent(node);
        }
    },
    BindNodeEvent: function (node) {//绑定点击事件
        var self = this;
        this.Base.BindEvent(node.Box, "click", function () {
            self.SetChildNodes(node);
            self.SetParentNode(node);
        });
    },
    SetChildNodes: function (node) {//设置子节点
        if (node.Childs == null || node.Childs.length == 0)
            return;
        for (var i = 0; i < node.Childs.length; i++) {
            node.Childs[i].Box.checked = node.Box.checked;
        }
    },
    SetParentNode: function (node) {//设置父节点
        if (node.Parent == null)
            return;
        var count = this.GetChildCheckCount(node.Parent);
        node.Parent.Box.checked = count > 0 ? true : false;
        node.Parent.Box.className = node.Parent.Box.checked == false || count == node.Parent.Childs.length ? "" : "part";
        this.SetParentNode(node.Parent);
    },
    GetChildCheckCount: function (node) {//得到子节点选择个数
        if (node.Childs == null || node.Childs.length == 0)
            return 0;
        var count = 0;
        for (var i = 0; i < node.Childs.length; i++) {
            if (node.Childs[i].Box.checked) {
                count++;
            }
        }
        return count;
    }
};