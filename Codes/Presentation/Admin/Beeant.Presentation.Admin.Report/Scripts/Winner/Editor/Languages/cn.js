Winner.Editor.Language.Editor = function () {
    var languages = {
        "Source": "源码", "Tempalte": "模板", "Preview": "预览", "NewPage": "新建", "Print": "打印", "Redo": "重做", "Undo": "撤销",
        "Bold": "加粗","Find":"查找替换", "Italic": "斜体", "Underline": "下划线", "StrikeThrough": "删除线",
        "Indent": "增加缩进", "Outdent": "减少缩进", "RemoveFormat": "去除格式", "SelectAll": "全选", "FullScreen": "全屏",
        "JustifyCenter": "居中", "JustifyLeft": "居左", "JustifyRight": "居右", "JustifyFull": "两端对齐",
        "Subscript": "下标", "Superscript": "上标", "CreateLink": "超链接", "Unlink": "删除链接",
        "BackColor": "背景颜色", "FontName": "字体", "FontSize": "大小", "ForeColor": "字体颜色",
        "HorizontalRule": "水平线", "Image": "插入图片", "Flash": "插入Flash", "InsertFlag": "插入特殊符", "Div": "div容器",
        "Table": "插入表格", "Face": "插入表情", "Flag": "插入特殊符", "OrderedList": "编号列表", "UnorderedList": "项目符号列表", "Paragraph": "换行覆盖"
    };
    return languages;
};
Winner.Editor.Language.Dialog = function () {
    var languages = { Names: [{ Value: "Sure", Text: "确定" }, { Value: "Cancel", Text: "取消"}] };
    return languages;
};
Winner.Editor.Language.LinkDialog = function () {
    var languages = { Title: "超链接", Names:
    [
        { Value: "UrlSelect", Text: "链接" }, { Value: "AnchorpointSelect", Text: "锚点" },
        { Value: "EmailSelect", Text: "邮件" }, { Value: "UrlName", Text: "地址：" },
        { Value: "AnchorpointName", Text: "名称：" }, { Value: "AddressName", Text: "地址：" },
        { Value: "SubjectName", Text: "标题：" }, { Value: "BodyName", Text: "内容：" }
    ],
        UrlError: "请输入URL地址", AnchorpointError: "请输入锚点名称", EmailError: "请输入邮箱地址"
    };
    return languages;
};
Winner.Editor.Language.TableDialog = function () {
    var languages = { Title: "插入表格",
        Names:
     [
        { Value: "CellCount", Text: "列数" }, { Value: "RowCount", Text: "行数" }, { Value: "TableBorder", Text: "边框" },
        { Value: "TableClass", Text: "表格样式名称" }, { Value: "TableStyle", Text: "表格样式" },
        { Value: "CaptionName", Text: "标题" }, { Value: "CaptionClass", Text: "标题样式名称" },
        { Value: "CaptionStyle", Text: "标题样式" }, { Value: "Summary", Text: "摘要" }
     ],
        RowError: "请输入行数", CellError: "请输入列数"
    };
    return languages;
};
Winner.Editor.Language.TableMenu = function () {
    var languages = {
        Names:
     [
        { Value: "Property", Text: "表格属性" }, { Value: "InsertDownRow", Text: "插入下方行" }, { Value: "InsertUpRow", Text: "插入上方行" },
        { Value: "InsertLeftColunm", Text: "插入左侧列" }, { Value: "InsertRightColunm", Text: "插入右侧列" },
        { Value: "DeleteRow", Text: "删除行" }, { Value: "DeleteColumn", Text: "删除列" },
        { Value: "DeleteTable", Text: "删除表格" }
     ]
    };
    return languages;
};
Winner.Editor.Language.DivDialog = function () {
       var languages =  { Title: "插入div",
        Names:
     [
        { Value: "DivClass", Text: "样式名称" }, { Value: "DivStyle", Text: "样式" }
     ]
    };
    return languages;
};
Winner.Editor.Language.ImageDialog = function () {
    var languages = {
        Title: "插入图片",
        Error: "请输入地址",
        Names:
        [
            { Value: "ImageSrc", Text: "地址" }, { Value: "ImageAlt", Text: "文本替换" },
            { Value: "ImageTitle", Text: "提示" }, { Value: "UploadContainer", Text: "上传文件" },
            { Value: "UploadButton", Text: "上传" }, { Value: "BrowserButton", Text: "浏览服务器" },
            { Value: "ImageClass", Text: "样式名称" }, { Value: "ImageStyle", Text: "样式" },
            { Value: "Preview", Text: "预览" }
        ],
        ExtensionErrorMessage: "请选择图片文件",
        SizeErrorMessage: "图片大小不能超过300KB"
    };
    return languages;
};
Winner.Editor.Language.FlashDialog = function () {
    var languages = {
        Title: "插入Flash",
        Error: "请输入地址",
        Names:
        [
            { Value: "FlashSrc", Text: "地址" }, { Value: "FlashWidth", Text: "宽度" },
            { Value: "FlashHeight", Text: "高度" }, { Value: "FlashHspace", Text: "水平间距" },
            { Value: "FlashVspace", Text: "垂直间距" }, { Value: "FlashAlign", Text: "对齐方式" },
            { Value: "FlashScale", Text: "缩放" }, { Value: "FlashSmode", Text: "窗体模式" },
            { Value: "FlashAllowScriptAccess", Text: "允许脚本访问" }, { Value: "FlashQuality", Text: "质量" },
            { Value: "FlashVariable", Text: "变量" }, { Value: "UploadContainer", Text: "上传文件" },
            { Value: "UploadButton", Text: "上传" }, { Value: "BrowserButton", Text: "浏览服务器" },
            { Value: "Preview", Text: "预览" }, { Value: "NoSelect", Text: "" },
            { Value: "Showall", Text: "全部显示" }, { Value: "Noborder", Text: "无边框" }, { Value: "Exactfit", Text: "严格匹配" },
            { Value: "Window", Text: "窗体" }, { Value: "Opaque", Text: "不透明" }, { Value: "Transparent", Text: "透明" },
            { Value: "Best", Text: "最好" }, { Value: "High", Text: "高" }, { Value: "Autohigh", Text: "高(自动)" },
            { Value: "Medium", Text: "中(自动)" }, { Value: "Autolow", Text: "低(自动)" }, { Value: "Low", Text: "低" },
            { Value: "Left", Text: "左对齐" }, { Value: "AbsBottom", Text: "绝对底部" }, { Value: "AbsMiddle", Text: "绝对居中" },
            { Value: "Baseline", Text: "基线" }, { Value: "Bottom", Text: "底部" }, { Value: "Middle", Text: "居中" },
            { Value: "Right", Text: "右对齐" }, { Value: "TextTop", Text: "文本上方" }, { Value: "Top", Text: "顶端" },
            { Value: "Always", Text: "总是" }, { Value: "Samedomain", Text: "同域" }, { Value: "Never", Text: "从不" },
            { Value: "FlashVariable", Text: "Flash变量" }, { Value: "AllowFullScreen", Text: "启用全屏" },
            { Value: "FlashLoop", Text: "循环" }, { Value: "FlashPlay", Text: "自动播放" }, { Value: "FlashMenu", Text: "启用 Flash 菜单" }
        ],
        ExtensionErrorMessage: "请选择flash文件",
        SizeErrorMessage: "flash大小不能超过1MB"
    };
    return languages;
};
Winner.Editor.Language.TemplateDialog = function () {
    var languages = {
        Title: "插入模板",
        Error: "对不起，无可用的模板",
        Names:
        [
            { Value: "TemplateSrc", Text: "地址" }, { Value: "TemplateID", Text: "ID标示" },
            { Value: "IsReplace", Text: "是否替换" }, { Value: "Preview", Text: "预览" },
            { Value: "UploadContainer", Text: "保存当前模板" }, { Value: "UploadButton", Text: "保存" },
            { Value: "BrowserButton", Text: "浏览服务器" }
        ],
        TemplateNameErrorMessage: "请输入模板名称"
    };
    return languages;
};
Winner.Editor.Language.FindDialog = function () {
    var languages = { Title: "查找替换", Error: "请输入查找内容",
        Names:
     [
        { Value: "FindInput", Text: "查找内容" }, { Value: "ReplaceInput", Text: "替换内容" },
        { Value: "CaseWord", Text: "大小写匹配" }, { Value: "FullWord", Text: "全字匹配" },
        { Value: "FindButton", Text: "查找下一个" }, { Value: "ReplaceButton", Text: "替换" },
        { Value: "ReplaceAllButton", Text: "替换全部" }
     ]
    };
    return languages;
};
Winner.Editor.Language.ColorPanel = function () {
    return { Names: [{ Value: "Sure", Text: "确定"}] };
};
Winner.Editor.Language.FontSizePanel = function () {
    var languages = ["8", "9", "10", "11", "12", "14", "16", "18", "20", "22", "24", "26", "28", "36", "48", "72"];
    return languages;
};
Winner.Editor.Language.FontNamePanel = function () {
    var languages = ["Arial", "Comic Sans MS", "Courier New", "Georgia", "Lucida Sans Unicode", "Tahoma", "Times New Roman",
     "Trebuchet MS", "Verdana", "宋体"];
    return languages;
};
Winner.Editor.Language.FacePanel = function () {
    var languages = [{ Value: "angel_smile.gif", Text: "天使"}];
    return languages;
};
