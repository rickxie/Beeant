Winner.ImageRecover = function (failover) {
    this.Failover = failover; //{[[Url:"",IsNormal:true,GroupName:'']],DefaultUrl:''}
};
Winner.ImageRecover.prototype = {
    Initialize: function () { //加载css样式文件
        this.LoadImages(document);
    },
    LoadImages:function(content) {
        var self = this;
        var allImages = content.getElementsByTagName("img");
        for (var i = 0; i < allImages.length; i++) {
            var img = allImages[i];
            img.onerror = function () {
                self.Replace(this);
            };
            img.src = img.src;
        }
    },
    Replace: function (img) {//替换
        if (img.src!=undefined && this.Failover.DefaultUrl!=undefined && img.src.toLowerCase() == this.Failover.DefaultUrl.toLowerCase())
            return;
        var date = new Date();
        var node = this.GetNode(img.src);
        if (node == null) {
            img.src = this.Failover.DefaultUrl + "?v=" + date;
            return;
        }
        var isRecover = false;
        for (var i = 0; i < this.Failover.Nodes.length; i++) {
            if (this.Failover.Nodes[i].IsNormal == false || this.Failover.Nodes[i].GroupName != node.GroupName)
                continue;
            img.src = img.src.replace(node.Url, this.Failover.Nodes[i].Url) + "?v=" + date;;
            isRecover = true;
            break;
        }
        if (isRecover == false && this.Failover.DefaultUrl != undefined && this.Failover.DefaultUrl != "") {
            img.src = this.Failover.DefaultUrl + "?v=" + date;
        }
    },
    GetNode: function (url) {//得到负载数据
        for (var i = 0; i < this.Failover.Nodes.length; i++) {
            if (url.indexOf(this.Failover.Nodes[i].Url) > -1) {
                this.Failover.Nodes[i].IsNormal = false;
                return this.Failover.Nodes[i];
            }
        }
        return null;
    }

};
