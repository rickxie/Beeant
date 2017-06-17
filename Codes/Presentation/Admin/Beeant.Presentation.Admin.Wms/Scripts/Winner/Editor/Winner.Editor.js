Winner = typeof(Winner)!="undefined" ? Winner : {};
Winner.Editor = function (inputId, className) {
    this.Base = new Winner.ClassBase();
    Winner.Editor.Language = {};
    this.Input = document.getElementById(inputId);
    if (this.Input == null) {
        return;
    }
    this.ClassName = className != undefined ? className : inputId;
    eval("window." + this.ClassName + "=this;");
    this.Container = null; //容器ImageDialog
    this.Html = "<table cellspacing='0px' cellpadding='0px' width='100%' height='100%'>" +
        "<tr><td class='toolbar' Instance='ToolBar'></td></tr>" +
        "<tr><td><iframe scrolling='none' class='iframe' Instance='Iframe' frameborder='0' ></iframe></td></tr>" +
        "<tr><td class='bottom'  Instance='Bottom'><div Instance='Resizer'></div></td></tr>" +
        "</table>";
    this.Width = 650;
    this.Height = 600;
    this.EditWindow = [];
    this.EditDocument = [];
    this.Tools = {
        "Source": { Click: "ToolMethod.ShowSource(['Source','Preview','NewPage','SelectAll','FullScreen']);" },
        "Preview": { Click: "ToolMethod.Preview();", HasSelectClass: false },
        "NewPage": { Click: "ToolMethod.NewPage();", HasSelectClass: false },
        "Tempalte": { Click: "TemplateDialog.ShowDialog();", HasSelectClass: false },
        "Print": { Click: "ToolMethod.Print();", HasSelectClass: false },
        "Redo": { Click: "UndoRedo.Redo();", HasSelectClass: false, IsEnable: false },
        "Undo": { Click: "UndoRedo.Undo();", HasSelectClass: false, IsEnable: false },
        "Bold": { Click: "ExecuteCommand('Bold',false,'');" },
        "Find": { Click: "FindDialog.ShowDialog();", HasSelectClass: false },
        "Italic": { Click: "ExecuteCommand('Italic',false,'');" },
        "Underline": { Click: "ExecuteCommand('Underline',false,'');" },
        "StrikeThrough": { Click: "ExecuteCommand('StrikeThrough',false,'');" },
        "Indent": { Click: "ExecuteCommand('Indent',false,'');", HasSelectClass: false },
        "Outdent": { Click: "ExecuteCommand('Outdent',false,'');", HasSelectClass: false },
        "SelectAll": { Click: "ToolMethod.SelectAll();", HasSelectClass: false },
        "FullScreen": { Click: "ToolMethod.FullScreen();" },
        "RemoveFormat": { Click: "ExecuteCommand('RemoveFormat',false,'');" },
        "JustifyCenter": { Click: "ExecuteCommand('JustifyCenter',false,'');", MutexSelectClasses: ['JustifyLeft', 'JustifyRight', 'JustifyFull'] },
        "JustifyLeft": { Click: "ExecuteCommand('JustifyLeft',false,'');", MutexSelectClasses: ['JustifyCenter', 'JustifyRight', 'JustifyFull'] },
        "JustifyRight": { Click: "ExecuteCommand('JustifyRight',false,'');", MutexSelectClasses: ['JustifyCenter', 'JustifyLeft', 'JustifyFull'] },
        "JustifyFull": { Click: "ExecuteCommand('JustifyFull',false,'');", MutexSelectClasses: ['JustifyCenter', 'JustifyLeft', 'JustifyRight'] },
        "Subscript": { Click: "ExecuteCommand('Subscript',false,'');" },
        "Superscript": { Click: "ExecuteCommand('Superscript',false,'');" },
        "CreateLink": { Click: "LinkDialog.ShowDialog();", HasSelectClass: false },
        "Unlink": { Click: "ExecuteCommand('Unlink',false,'');", HasSelectClass: false },
        "FontName": { Click: "FontNamePanel.ShowPanel(" + this.ClassName + ".Tools['FontName'].Object);", HasSelectClass: false, Html: "<span class='fontname'>{$name}</span><span class='down'>&nbsp;</span>" },
        "FontSize": { Click: "FontSizePanel.ShowPanel(" + this.ClassName + ".Tools['FontSize'].Object);", HasSelectClass: false, Html: "<span class='fontsize'>{$name}</span><span class='down'>&nbsp;</span>" },
        "ForeColor": { Click: "ColorPanel.ShowPanel(" + this.ClassName + ".Tools['ForeColor'].Object,'ForeColor');", HasSelectClass: false, Html: "<span class='forecolor'></span><span class='down'>&nbsp;</span>", MutexSelectClasses: ['BackColor'] },
        "BackColor": { Click: "ColorPanel.ShowPanel(" + this.ClassName + ".Tools['BackColor'].Object,'BackColor');", HasSelectClass: false, Html: "<span class='backcolor'></span><span class='down'>&nbsp;</span>", MutexSelectClasses: ['ForeColor'] },
        "HorizontalRule": { Click: "ExecuteCommand('InsertHorizontalRule',false,'');", HasSelectClass: false },
        "Flag": { Click: "FlagPanel.ShowPanel(" + this.ClassName + ".Tools['Flag'].Object);", HasSelectClass: false },
        "Table": { Click: "TableDialog.ShowDialog();", HasSelectClass: false },
        "Face": { Click: "FacePanel.ShowPanel(" + this.ClassName + ".Tools['Face'].Object);", HasSelectClass: false },
        "Image": { Click: "ImageDialog.ShowDialog();", HasSelectClass: false },
        "Div": { Click: "DivDialog.ShowDialog();", HasSelectClass: false },
        "Flash": { Click: "FlashDialog.ShowDialog();", HasSelectClass: false },
        "OrderedList": { Click: "ExecuteCommand('InsertOrderedList',false,'');" },
        "UnorderedList": { Click: "ExecuteCommand('InsertUnorderedList',false,'');" }
    }; //工具箱
    this.StyleFiles = ["/Styles/Style.css"];
    this.ScriptFiles = ["/Languages/cn.js", "/Config.js"];
    this.PreviewFile = "/Htmls/Preview.html";
    this.Path = "/scripts/Winner/Editor";
};
Winner.Editor.prototype =
{
    Initialize: function (config) {//初始化
        this.LoadResource();
        var self = this;
        var func = function () {
            if (Winner.Editor.Config == undefined || Winner.Editor.Language == undefined)
                setTimeout(func, 100);
            else
                self.CreateEditor(config);
        };
        this.Base.BindEvent(window, "load", function () {
            func();
        });
    },
    LoadResource: function () {//加载资源
        for (var i = 0; i < this.StyleFiles.length; i++) {
            this.Base.LoadCssFile(this.Path + this.StyleFiles[i]);
        }
        for (i = 0; i < this.ScriptFiles.length; i++) {
            this.Base.LoadScriptFile(this.Path + this.ScriptFiles[i]);
        }
    },
    CreateEditor: function (config) {
        Winner.Editor.Config(this);
        this.Languages = Winner.Editor.Language.Editor();
        this.Base.LoadConfig(this, config);
        this.CreateContainer();
        this.LoadInstances(this, this.Container);
        this.ResetInput();
        this.CreatePlugins();
        this.InitlizeEditor();
        this.ToolCreator.LoadToolbar(this.ToolBar);
    },
    CreateContainer: function () {//创建容器
        this.Container = document.createElement('div');
        this.Container.className = "editor";
        this.Container.style.overflow = "hidden";
        this.Container.style.width = this.Width + "px";
        this.Container.style.height = this.Height + "px";
        this.Container.innerHTML = this.Html;
        this.ParentNode = this.Input.parentNode == null ? document.body : this.Input.parentNode;
        this.ParentNode.insertBefore(this.Container, this.Input);
    },
    CreatePlugins: function () {//创建插件
        this.CreateDialogPlugins();
        this.CreatePanelPlugins();
        this.CreateOtherPlugins();
    },
    CreateDialogPlugins: function () {//创建对话框插件
        this.Dialog = new Winner.Editor.Dialog(this, Winner.Editor.Language.Dialog());
        this.LinkDialog = new Winner.Editor.LinkDialog(this.Dialog, Winner.Editor.Language.LinkDialog());
        this.TableDialog = new Winner.Editor.TableDialog(this.Dialog, Winner.Editor.Language.TableDialog());
        this.DivDialog = new Winner.Editor.DivDialog(this.Dialog, Winner.Editor.Language.DivDialog());
        this.ImageDialog = new Winner.Editor.ImageDialog(this.Dialog, this.ImageUploadUrl, this.ImageBrowserUrl, Winner.Editor.Language.ImageDialog());
        this.FlashDialog = new Winner.Editor.FlashDialog(this.Dialog, this.FlashUploadUrl, this.FlashBrowserUrl, Winner.Editor.Language.FlashDialog());
        this.TemplateDialog = new Winner.Editor.TemplateDialog(this.Dialog, this.TemplateUploadUrl, this.TemplateBrowserUrl, Winner.Editor.Language.TemplateDialog());
        this.FindDialog = new Winner.Editor.FindDialog(this.Dialog, Winner.Editor.Language.FindDialog());
    },
    CreatePanelPlugins: function () {//插件panel插件
        this.Panel = new Winner.Editor.Panel(this);
        this.FontPanel = new Winner.Editor.FontPanel(this.Panel);
        this.ColorPanel = new Winner.Editor.ColorPanel(this.Panel, Winner.Editor.Language.ColorPanel());
        this.FontSizePanel = new Winner.Editor.FontSizePanel(this.FontPanel, Winner.Editor.Language.FontSizePanel());
        this.FontNamePanel = new Winner.Editor.FontNamePanel(this.FontPanel, Winner.Editor.Language.FontNamePanel());
        this.FlagPanel = new Winner.Editor.FlagPanel(this.Panel);
        this.FacePanel = new Winner.Editor.FacePanel(this.Panel, Winner.Editor.Language.FacePanel());
    },
    CreateOtherPlugins: function () {//创建其它插件
        this.TableMenu = new Winner.Editor.TableMenu(this.TableDialog, Winner.Editor.Language.TableMenu());
        this.ContentEvent = new Winner.Editor.ContentEvent(this);
        this.ToolMethod = new Winner.Editor.ToolMethod(this);
        this.HideTag = new Winner.Editor.HideTag(this);
        this.ToolCreator = new Winner.Editor.ToolCreator(this);
        this.SelectRange = new Winner.Editor.SelectRange(this);
        this.Sizer = new Winner.Editor.Sizer(this);
        this.UndoRedo = new Winner.Editor.UndoRedo(this);
    },
    LoadInstances: function (sender, ctrl, name) {//加载实例
        this.Base.LoadInstances(sender, ctrl, name);
    },
    ResetInput: function () {//设置Input样式
        this.Input.style.display = "none";
        this.Input.className = "iframe";
        this.Input.style.resize = "none";
        var parent = this.Input.parentNode == null ? document.body : this.Input.parentNode;
        parent.removeChild(this.Input);
        parent = this.Iframe.parentNode == null ? document.body : this.Iframe.parentNode;
        parent.insertBefore(this.Input, this.Iframe);
    },
    InitlizeEditor: function () {//初始化
        var self = this;
        var func = function () {
            if (self.Iframe.contentWindow != undefined && self.Iframe.contentWindow.document != undefined && self.Iframe.contentWindow.document.body != undefined) {
                self.Iframe.contentWindow.document.designMode = "on";
                self.Iframe.contentWindow.document.contenteditable = "true";
                if (self.Domain != undefined) {
                    self.Iframe.contentWindow.document.domain = self.Domain;
                }
                self.InitlizeDesignMode();
            } else
                setTimeout(func, 100);
        };
        setTimeout(func, 100);
    },
    InitlizeDesignMode: function () {//初始化编辑模式
        var self = this;
        var func = function () {
            if (self.Iframe.contentWindow != undefined && self.Iframe.contentWindow.document != undefined && self.Iframe.contentWindow.document.body != undefined) {
                self.InitlizeIframe();
                self.InitlizeEditorPlugins();
                self.InitlizeEditorContent();
            } else
                setTimeout(func, 100);
        };
        setTimeout(func, 100);
    },
    InitlizeIframe: function () {//设置Iframe
        this.EditWindow = this.Iframe.contentWindow.window;
        this.EditDocument = this.Iframe.contentWindow.document;
        this.Iframe.style.display = "";

        this.Iframe.style.height = this.Height + "px";
    },
    InitlizeEditorContent: function () {//初始化值
        this.EditDocument.body.innerHTML = this.Input.value;
        if (this.EditDocument.body.innerHTML == "")
            this.EditDocument.body.innerHTML = "";
        this.Sizer.ResizeIframe(this.Width, this.Height);
    },
    InitlizeEditorPlugins: function () {//初始化插件
        this.Panel.Initialize();
        this.SelectRange.Initialize();
        this.HideTag.Initialize();
        this.Sizer.Initialize();
        this.UndoRedo.Initialize();
        this.TableMenu.Initialize();
    },
    InsertHtml: function (html) {//插入HTML代码
        this.BeforeExecute();
        if (document.all || document.selection) {
            this.SelectRange.Range.pasteHTML(html);
        } else if (document.getSelection) {
            var node = this.EditDocument.createElement("b");
            this.SelectRange.Range.surroundContents(node);
            node.innerHTML = html;

        } else {
            this.EditDocument.execCommand('InsertHtml', '', html);
        }

        this.HideTag.ReplaceTextToImage();
        this.AfaterExecute();
    },
    ExecuteCommand: function (commandName, isShow, paramters) {//执行命令
        this.BeforeExecute();
        var rev = this.EditDocument.execCommand(commandName, isShow, paramters);
        this.AfaterExecute();
        return rev;
    },
    BeforeExecute: function () {
        this.SelectRange.Save();
    },
    AfaterExecute: function () {
        this.SelectRange.Remove();
        this.ContentEvent.BindEvent();
        this.UndoRedo.AddDo();
    },
    ReplaceAll: function (source, oldString, newString) {//替换所有
        var reg = new RegExp("\\" + oldString, "g");
        return source.replace(reg, newString);
    },
    ReplaceLanguage: function (source, names) {//替换名称
        for (var i = 0; i < names.length; i++) {
            source = this.ReplaceAll(source, "@" + names[i].Value, names[i].Text);
        }
        return source;
    },
    Contain: function (names, name) {
        for (var i = 0; i < names.length; i++) {
            if (names[i] == name)
                return true;
        }
        return false;
    }

};

//Panel
Winner.Editor.Panel = function (editor) {
    this.Editor = editor; //Winner.Editor对象
    this.Container = []; //容器
};
Winner.Editor.Panel.prototype = {
    Initialize: function () { //初始化函数
        this.CreateContainer();
        this.BindEvent();
    },
    CreateContainer: function () {//创建对话框
        this.Container = document.createElement('div');
        this.Container.style.display = "none";
        this.Container.style.position = "absolute";
        this.Editor.Container.appendChild(this.Container);
    },
    BindEvent: function () {
        this.BindHideEvent();
        this.BindContainerClickEvent();
    },
    BindContainerClickEvent: function () {//绑定容器事件
        var self = this;
        this.Container.onclick = function (event) {
            return self.Editor.Base.CancelEventUp(event);
        };
    },
    BindHideEvent: function () {//绑定隐藏事件
        var self = this;
        this.Editor.Base.BindEvent(this.Editor.Container, "click", function () {
            self.ClosePanel(true);
        });
        this.Editor.Base.BindEvent(this.Editor.EditDocument, "click", function () {
            self.ClosePanel(true);
        });
    },
    ShowPanel: function (obj) {//显示
        if (this.Sender == obj && this.Container.style.display != "none") {
            this.Container.style.display = "none";
            return;
        }
        this.Container.style.top = this.Editor.Base.GetElementTop(obj) + obj.offsetHeight + "px";
        this.Container.style.left = this.Editor.Base.GetElementLeft(obj) + "px";
        this.Container.style.display = "";
        this.Sender = obj;
        this.IsInvokeShowPanel = true;
    },
    ClosePanel: function () {//隐藏
        if (arguments[0] == undefined) {
            this.Container.style.display = "none";
            this.Container.innerHTML = "";
        }
        else if (arguments[0] == true && !this.IsInvokeShowPanel) {
            this.Container.style.display = "none";
            this.Container.innerHTML = "";
        }
        this.IsInvokeShowPanel = undefined;
    }

};
//ColorPanel
Winner.Editor.ColorPanel = function (panel, languages) {
    this.Panel = panel; //panel对象
    this.Languages = languages;
    this.ColorContent = [];
    this.ViewContent = [];
    this.CurrentView = [];
    this.SelectView = [];
    this.ColorInput = [];
    this.ColorSure = [];
    this.ColorSpan = null;
    this.Html = "<div class='select' Instance='ColorContent'></div>" +
    "<div class='view' Instance='ViewContent'>" +
    "<div class='cur' Instance='CurrentView'></div>" +
    "<div class='select' Instance='SelectView'></div>" +
    "<input class='val' type='text' maxLength=7  Instance='ColorInput'/>" +
    "<input type='button' class='button' value='@Sure'  Instance='ColorSure'/>" +
    "</div>";
    this.Html = this.Panel.Editor.ReplaceLanguage(this.Html, this.Languages.Names);
};
Winner.Editor.ColorPanel.prototype = {
    Initialize: function () { //初始化函数
        this.Panel.Container.className = "colorpanel";
        this.Panel.Container.innerHTML = this.Html;
        this.Panel.Editor.LoadInstances(this, this.Panel.Container);
        this.CreateColors();
        this.BindSureEvent();
    },
    BindSureEvent: function () {//绑定确定时间
        var self = this;
        this.ColorSure.onclick = function () {
            self.SelectColor();
        };
    },
    CreateColors: function () {//创建颜色
        var color = ["00", "33", "66", "99", "CC", "FF"];
        this.CreateTopColors(color);
        this.CreateButtomColors(color);
    },
    CreateTopColors: function (color) {//创建上部颜色
        for (var j = 0; j < color.length; j++) {
            for (var i = 0; i < color.length / 2; i++) {
                for (var k = 0; k < color.length; k++) {
                    this.CreateColorCell(color[i] + color[k] + color[j]);
                }
            }
        }
    },
    CreateButtomColors: function (color) {//创建下部颜色
        for (var j = 0; j < color.length; j++) {
            for (var i = color.length / 2; i < color.length; i++) {
                for (var k = 0; k < color.length; k++) {
                    this.CreateColorCell(color[i] + color[k] + color[j]);
                }
            }
        }
    },
    CreateColorCell: function (color) {//创建颜色
        var cell = document.createElement('span');
        cell.style.background = "#" + color;
        this.Panel.Editor.Base.SetAttribute(cell, "color", "#" + color);
        this.ColorContent.appendChild(cell);
        this.BindCellOverEvent(cell);
        this.BindCellClickEvent(cell);
        this.BindCellDoubleClickEvent(cell);
    },
    BindCellOverEvent: function (cell) {//绑定移动事件
        var self = this;
        cell.onmouseover = function () {
            self.CurrentView.style.backgroundColor = cell.style.backgroundColor;
        };
    },
    BindCellClickEvent: function (cell) {//绑定点击事件
        var self = this;
        cell.onclick = function () {
            self.ColorInput.value = self.Panel.Editor.Base.GetAttribute(cell, "color");
            self.SelectView.style.backgroundColor = cell.style.backgroundColor;
        };
    },
    BindCellDoubleClickEvent: function (cell) {//绑定点击事件
        var self = this;
        cell.ondblclick = function () {
            self.ColorInput.value = self.Panel.Editor.Base.GetAttribute(cell, "color");
            self.SelectColor();
        };
    },
    SelectColor: function () {//选择颜色
        this.Panel.ClosePanel();
        this.ExecuteCommand();
        if (this.ColorSpan != null) {
            this.ColorSpan.style.background = this.ColorInput.value;
        }
    },
    ExecuteCommand: function () {//执行命令
        if (this.CommandName.toLowerCase() == "backcolor" && !document.all) {
            var value = this.Panel.Editor.SelectRange.Text == undefined ? "" : this.Panel.Editor.SelectRange.Text;
            this.Panel.Editor.InsertHtml("<span style=' background-color:" + this.ColorInput.value + "'>" + value + "</span>");
        }
        else {
            this.Panel.Editor.ExecuteCommand(this.CommandName, false, this.ColorInput.value);
        }
    },
    ShowPanel: function (obj, commandName) {//显示对话框
        this.Panel.ShowPanel(obj);
        this.Initialize();
        this.CommandName = commandName;
        this.ColorInput.value = "";
        this.CurrentView.style.backgroundColor = "#FFFFFF";
        this.SelectView.style.backgroundColor = "#FFFFFF";
        var spans = obj.getElementsByTagName('span');
        if (spans != null && spans.length > 0) {
            this.ColorSpan = spans[0];
        }
    }
};
//FacePanel
Winner.Editor.FacePanel = function (panel, languages) {
    this.Panel = panel; //panel对象
    this.Path = "/Scripts/Winner/Editor/Images/Face/";
    this.Languages = languages;
};
Winner.Editor.FacePanel.prototype = {
    Initialize: function () { //初始化函数
        this.Panel.Container.innerHTML = "";
        this.Panel.Container.className = "facepanel";
        this.LoadImages();
    },
    LoadImages: function () {//加载特殊符
        for (var i = 0; i < this.Languages.length; i++) {
            this.CreateImage(this.Languages[i]);
        }
    },
    CreateImage: function (language) {//创建特殊符
        var img = document.createElement("img");
        img.src = this.Path + language.Value;
        img.alt = language.Text;
        img.title = language.Text;
        this.Panel.Container.appendChild(img);
        this.BindImageClickEvent(img);
    },
    BindImageClickEvent: function (img) {
        var self = this;
        img.onclick = function () {
            self.Select(img);
        };
    },
    Select: function (img) {
        this.Panel.ClosePanel();
        this.Panel.Editor.InsertHtml("<img src='" + img.src + "' Alt='" + img.alt + "' title='" + img.title + "'/>");
    },
    ShowPanel: function (obj) {
        this.Panel.ShowPanel(obj);
        this.Initialize();
    }
};
//FlagPanel
Winner.Editor.FlagPanel = function (panel) {
    this.Panel = panel; //panel对象
};
Winner.Editor.FlagPanel.prototype = {
    Initialize: function () { //初始化函数
        this.Panel.Container.innerHTML = "";
        this.Panel.Container.className = "flagpanel";
        this.LoadFlags();
    },
    LoadFlags: function () {//加载特殊符
        for (var i = 33; i < 255; i++) {
            this.CreateFlag(i);
            if (i == 123)
                i += 37;
        }
    },
    CreateFlag: function (code) {//创建特殊符
        var span = document.createElement("span");
        span.innerHTML = String.fromCharCode(code);
        this.Panel.Container.appendChild(span);
        this.BindSpanEvent(span);
    },
    BindSpanEvent: function (span) {
        this.BindSpanOverEvent(span);
        this.BindSpanOutEvent(span);
        this.BindSpanClickEvent(span);
    },
    BindSpanOverEvent: function (span) {
        span.onmouseover = function () {
            span.className = "over";
        };
    },
    BindSpanOutEvent: function (span) {
        span.onmouseout = function () {
            span.className = "out";
        };
    },
    BindSpanClickEvent: function (span) {
        var self = this;
        span.onclick = function () {
            self.Select(span);
        };
    },
    Select: function (span) {
        this.Panel.ClosePanel();
        this.Panel.Editor.InsertHtml(span.innerHTML);

    },
    ShowPanel: function (obj) {
        this.Panel.ShowPanel(obj);
        this.Initialize();
    }
};

//FontPanel
Winner.Editor.FontPanel = function (panel) {
    this.Panel = panel; //panel对象
    this.FontSpan = null;
    this.Html = "<span class='top' Instance='TopName'></span>";
};
Winner.Editor.FontPanel.prototype = {
    Initialize: function () { //初始化函数
        this.Panel.Container.innerHTML = this.Html;
        this.Panel.Editor.LoadInstances(this, this.Panel.Container);
        if (this.FontSpan != null) {
            this.TopName.innerHTML = this.FontSpan.innerHTML;
        }
    },
    ShowPanel: function (obj) {//显示对话框
        var spans = obj.getElementsByTagName('span');
        if (spans != null && spans.length > 0) {
            this.FontSpan = spans[0];
        }
        this.Initialize();
        this.Panel.ShowPanel(obj);
    },
    BindSpanEvent: function (span) {
        this.BindSpanOverEvent(span);
        this.BindSpanOutEvent(span);
        this.BindSpanClickEvent(span);
    },
    BindSpanOverEvent: function (span) {
        span.onmouseover = function () {
            span.className = "over";
        };
    },
    BindSpanOutEvent: function (span) {
        span.onmouseout = function () {
            span.className = "out";
        };
    },
    BindSpanClickEvent: function (span) {
        var self = this;
        span.onclick = function () {
            self.Select(span);
        };
    },
    Select: function (span) {
        this.Panel.ClosePanel();
        this.ExecuteCommand(span);
        if (this.FontSpan != null) {
            this.FontSpan.innerHTML = span.innerHTML;
        }
    }
};
//FontNamePanel
Winner.Editor.FontNamePanel = function (fontPanel, languages) {
    this.FontPanel = fontPanel; //FontPanel对象
    this.Languages = languages;
};
Winner.Editor.FontNamePanel.prototype = {
    Initialize: function () { //初始化函数
        this.FontPanel.Panel.Container.className = "fontnamepanel";
        this.FontPanel.ExecuteCommand = this.ExecuteCommand;
        this.LoadSpans();
    },
    LoadSpans: function () {
        for (var i = 0; i < this.Languages.length; i++) {
            this.CreateSpan(this.Languages[i]);
        }
    },
    ExecuteCommand: function (span) {//执行命令
        var value = this.Panel.Editor.SelectRange.Text == undefined ? "" : this.Panel.Editor.SelectRange.Text;
        this.Panel.Editor.InsertHtml("<span style='font-family:" + span.innerHTML.toLowerCase() + "'>" + value + "</span>");
    },
    CreateSpan: function (language) {
        var span = document.createElement("span");
        span.innerHTML = language;
        span.style.fontFamily = span.innerHTML.toLowerCase();
        this.FontPanel.BindSpanEvent(span);
        this.FontPanel.Panel.Container.appendChild(span);
    },
    ShowPanel: function (obj) {
        this.FontPanel.ShowPanel(obj);
        this.Initialize();
    }
};
//FontSizePanel
Winner.Editor.FontSizePanel = function (fontPanel, languages) {
    this.FontPanel = fontPanel; //FontPanel对象
    this.Languages = languages;
};
Winner.Editor.FontSizePanel.prototype = {
    Initialize: function () { //初始化函数
        this.FontPanel.Panel.Container.className = "fontsizepanel";
        this.FontPanel.ExecuteCommand = this.ExecuteCommand;
        this.LoadSpans();
    },
    LoadSpans: function () {
        for (var i = 0; i < this.Languages.length; i++) {
            this.CreateSpan(this.Languages[i]);
        }
    },
    ExecuteCommand: function (span) {//执行命令
        var value = this.Panel.Editor.SelectRange.Text == undefined ? "" : this.Panel.Editor.SelectRange.Text;
        this.Panel.Editor.InsertHtml("<span style='font-size:" + span.innerHTML + "px'>" + value + "</span>");
    },
    CreateSpan: function (language) {
        var span = document.createElement("span");
        span.innerHTML = language;
        span.style.fontSize = language + "px";
        this.FontPanel.BindSpanEvent(span);
        this.FontPanel.Panel.Container.appendChild(span);
    },
    ShowPanel: function (obj) {
        this.FontPanel.ShowPanel(obj);
        this.Initialize();
    }
};
//Dialog
Winner.Editor.Dialog = function (editor, languages) {
    this.Editor = editor; //Winner.Editor对象
    this.Container = []; //容器
    this.Shelter = []; //遮挡层
    this.Languages = languages;
    this.Html = "<div Instance='Top' class='top'>" +
    "<div class='tip' Instance='Title'></div><div Instance='Close' class='close'></div></div>" +
    "<div Instance='Content' class='content'></div>" +
    "<div class='bottom' Instance='Buttom'><span class='sure' Instance='Sure'>@Sure</span><span class='cancel' Instance='Cancel'>@Cancel</span></div>";
    this.MoveInfo = { IsMove: false, Left: 0, Top: 0, ClickLeft: 0, ClickTop: 0 }; //移动信息
    this.Html = this.Editor.ReplaceLanguage(this.Html, this.Languages.Names);
    if (arguments[1] != false) {
        this.Initialize();
    }
};
Winner.Editor.Dialog.prototype = {
    Initialize: function () { //初始化函数
        this.CreateShelter();
        this.CreateContainer();
        this.Editor.LoadInstances(this, this.Container);
        this.BindEvent();
    },
    CreateContainer: function () { //创建对话框
        this.Container = document.createElement('div');
        this.Container.className = "editordialog";
        this.Container.style.display = "none";
        this.Container.style.position = "absolute";
        this.Container.innerHTML = this.Html;
        document.body.appendChild(this.Container);
    },
    CreateShelter: function () { //创建遮挡
        this.Shelter = document.createElement('div');
        this.Shelter.className = "editorshelter";
        this.Shelter.style.display = "none";
        this.Shelter.style.position = "absolute";
        document.body.appendChild(this.Shelter);
    },
    BindEvent: function () {
        this.BindMoveEvent(this.Top);
        this.BindCloseEvent();
        this.BindCancelEvent();
    },
    BindCloseEvent: function () { //绑定关闭按钮
        var self = this;
        this.Close.onclick = function () {
            self.CloseDialog();
        };
    },
    BindCancelEvent: function () { //绑定取消按钮
        var self = this;
        this.Cancel.onclick = function () {
            self.CloseDialog();
        };
    },
    BindMoveEvent: function (box) { //绑定移动对话框事件
        this.BindMouseDownEvent(box);
        this.BindMouseUpEvent(box);
        this.BindMouseMoveEvent(box);
    },
    BindMouseDownEvent: function (box) { //绑定鼠标按下事件
        var self = this;
        box.onmousedown = function (event) {
            event = window.event ? window.event : event;
            self.FillMoveInfo(event.clientX, event.clientY);
        };
    },
    BindMouseUpEvent: function () { //绑定鼠标离开事件
        var self = this;
        this.Editor.Base.BindEvent(document, "mouseup", function () {
            self.MoveInfo.IsMove = false;
        });
    },
    BindMouseMoveEvent: function () { //绑定鼠标移动事件
        var self = this;
        this.Editor.Base.BindEvent(document, "mousemove", function (event) {
            event = window.event ? window.event : event;
            self.MoveContent(event.clientX, event.clientY);
        });
    },
    FillMoveInfo: function (x, y) { //填充移动信息
        this.MoveInfo.IsMove = true;
        this.MoveInfo.Left = parseInt(this.Container.style.left.replace("px", ""));
        this.MoveInfo.Top = parseInt(this.Container.style.top.replace("px", ""));
        this.MoveInfo.ClickLeft = x;
        this.MoveInfo.ClickTop = y;
    },
    MoveContent: function (x, y) { //移动对话框
        if (this.MoveInfo.IsMove) {
            var position = this.SetPosition(x, y);
            this.Container.style.left = position.X + "px";
            this.Container.style.top = position.Y + "px";
        }
    },
    SetPosition: function (x, y) { //检查是否移动
        var moveX = this.MoveInfo.Left + x - this.MoveInfo.ClickLeft;
        var moveY = this.MoveInfo.Top + y - this.MoveInfo.ClickTop;
        return { X: moveX, Y: moveY };
    },
    ShowDialog: function () { //显示对话框
        this.ShowContainer();
        this.ShowShelter();
        this.CreateHideInput();
    },
    ShowContainer: function () { //展示容器
        this.Container.style.display = "";
        this.Container.style.width = this.Content.clientWidth + "px";
        this.Container.style.left = this.Editor.Base.GetElementLeft(this.Editor.Container) + (this.Editor.Container.clientWidth - this.Container.clientWidth) / 2 + "px";
        this.Container.style.top = this.Editor.Base.GetElementTop(this.Editor.Container) + 40 + "px";
    },
    ShowShelter: function () { //显示遮挡层
        this.Shelter.style.left = "0px";
        this.Shelter.style.top = "0px";
        var info = this.GetShelterInfo();
        this.Shelter.style.width = info.Width + "px";
        this.Shelter.style.height = info.Height + "px";
        this.Shelter.style.display = "";
    },
    GetShelterInfo: function () {//得到显示宽度和高度
        var width, height;
        if (document.documentElement && document.documentElement.scrollWidth > document.body.scrollWidth)
            width = document.documentElement.scrollWidth;
        else width = document.body.scrollWidth;
        if (document.documentElement && document.documentElement.scrollHeight > document.body.scrollHeight)
            height = document.documentElement.scrollHeight;
        else height = document.body.scrollHeight;
        return { Width: width, Height: height };
    },
    CreateHideInput: function () {//创建隐藏控件
        var inputs = this.Container.getElementsByTagName('input');
        if (inputs.length == 1) {
            var input = document.createElement('input');
            input.style.display = 'none';
            input.type = "text";
            this.Container.appendChild(input);
        }
    },
    CloseDialog: function () {//关闭对话框
        this.Container.style.display = "none";
        this.Content.innerHTML = "";
        this.Shelter.style.display = "none";
    },
    SetInputEvent: function () {//绑定enter事件
        this.BindTextAreaEvent();
        this.BindInputEvent();
    },
    BindTextAreaEvent: function () {
        var inputs = this.Content.getElementsByTagName('textarea');
        if (inputs == null)
            return;
        for (var i = 0; i < inputs.length; i++) {
            this.BindKeyUpEvent(inputs[i]);
        }
    },
    BindInputEvent: function () {
        var inputs = this.Content.getElementsByTagName('input');
        if (inputs == null)
            return;
        for (var i = 0; i < inputs.length; i++) {
            if (inputs[i].type == "text") {
                this.BindKeyUpEvent(inputs[i]);
            }
        }
    },
    BindKeyUpEvent: function (input) {//绑定enter事件
        var self = this;
        input.onkeydown = function (event) {
            return self.OnKeyEvent(event);
        };
    },
    OnKeyEvent: function (event) {//相应键盘事件
        event = window.event ? window.event : event;
        if (event.keyCode == 13) {
            this.Sure.click();
            return false;
        }
        else if (event.keyCode == 27) {
            this.Close.click();
        }
        return true;
    }
};

//DivDialog
Winner.Editor.DivDialog = function (dialog, languages) {
    this.Dialog = dialog;
    this.Languages = languages;
    this.Html = "<table class='middle'>" +
    "<tr><th class='th'>@DivClass</th><td class='td'><input type='text' Instance='DivClass' /></td></tr>" +
    "<tr><th class='th'>@DivStyle</th><td class='td'><input type='text' class='len'  Instance='DivStyle' value='line-height:22px;padding:10px;margin:0px;width:100%;' /></td></tr>" +
    "</table>";
    this.Html = this.Dialog.Editor.ReplaceLanguage(this.Html, this.Languages.Names);
};
Winner.Editor.DivDialog.prototype =
{
    Initialize: function () { //初始化函数
        this.Dialog.Content.innerHTML = this.Html;
        this.Dialog.Editor.LoadInstances(this, this.Dialog.Container);
        this.Dialog.Title.innerHTML = this.Languages.Title;
        this.BindEvent();
    },
    BindEvent: function () {//绑定事件
        this.BindSureEvent();
        this.Dialog.SetInputEvent();
    },
    BindSureEvent: function () {//绑定输入事件
        var self = this;
        this.Dialog.Sure.onclick = function () {
            self.CreateDiv();
        };
    },
    CreateDiv: function () {//创建DIV
        var value = this.GetDivHtml();
        this.Dialog.CloseDialog();
        this.Dialog.Editor.InsertHtml(value);
    },
    GetDivHtml: function () {//得到DIV的html
        var rev = "<div";
        rev += this.GetDivStyleHtml();
        rev += "><div><br/>";
        return rev;
    },
    GetDivStyleHtml: function () {//得到table的样式
        var rev = "";
        if (this.DivClass.value != "")
            rev += " class='" + this.DivClass.value + "'";
        if (this.DivStyle.value != "")
            rev += " style='" + this.DivStyle.value + "'";
        return rev;
    },
    ShowDialog: function () {//显示
        this.Initialize();
        this.Dialog.ShowDialog();
        this.DivClass.focus();
    }
};

//FindDialog
Winner.Editor.FindDialog = function (dialog, languages) {
    this.Dialog = dialog;
    this.Languages = languages;
    this.Html = "<table class='middle'>" +
        "<tr><th class='th'>@FindInput</th><td class='td'><input type='text' Instance='FindInput' class='len' /></td></tr>" +
        "<tr><th class='th'>@ReplaceInput</th><td class='td'><input type='text'  Instance='ReplaceInput' class='len' /></td></tr>" +
        "<tr><th class='th'>@CaseWord</th><td class='td'><input type='checkbox'  Instance='CaseWord' /></td></tr>" +
        "<tr><th class='th'>@FullWord</th><td class='td'><input type='checkbox'  Instance='FullWord' /></td></tr>" +
        "<tr><td colspan='2' class='td center'><input type='button'  Instance='FindButton' value='@FindButton' class='button' />" +
        "<input type='button' class='button'   Instance='ReplaceButton' value='@ReplaceButton' />" +
        "<input type='button'  class='button'  Instance='ReplaceAllButton' value='@ReplaceAllButton' /></td></tr></table>";
    this.Html = this.Dialog.Editor.ReplaceLanguage(this.Html, this.Languages.Names);
    this.FindNodes = null;
    this.CurrentNodeIndex = 0;
    this.NodeOffset = 0;
};
Winner.Editor.FindDialog.prototype =
{
    Initialize: function () { //初始化函数
        this.Dialog.Content.innerHTML = this.Html;
        this.Dialog.Editor.LoadInstances(this, this.Dialog.Container);
        this.Dialog.Title.innerHTML = this.Languages.Title;
        this.Dialog.Sure.style.display = "none";
        this.BindEvent();
    },
    BindEvent: function () {//绑定事件
        this.BindCancelEvent();
        this.Dialog.SetInputEvent();
        this.BindFindButtonEvent();
        this.BindReplaceAllEvent();
        this.BindReplaceEvent();
        this.BindFindInputEvent();
    },
    BindCancelEvent: function () {//绑定输入事件
        var self = this;
        this.Dialog.Cancel.onclick = function () {
            self.Dialog.Sure.style.display = "";
            self.Dialog.CloseDialog();
            self.Dialog.Editor.SelectRange.Save();
        };
    },
    BindFindButtonEvent: function () {//查找
        var self = this;
        this.FindButton.onclick = function () {
            self.Find();
        };
    },
    BindReplaceAllEvent: function () {//替换事情
        var self = this;
        this.ReplaceAllButton.onclick = function () {
            self.ReplaceAll();
        };
    },
    BindReplaceEvent: function () {
        var self = this;
        this.ReplaceButton.onclick = function () {
            self.Replace();
        };
    },
    BindFindInputEvent: function () {
        var self = this;
        this.FindInput.onblur = function () {
            self.ResetFindNodes();
        };
    },
    ResetFindNodes: function () {//重置
        this.FindNodes = [];
        this.CurrentNodeIndex = 0;
        this.NodeOffset = 0;
        this.FillFindNodes(this.Dialog.Editor.EditDocument.body);
    },
    FillFindNodes: function (node) {//填充匹配的节点
        this.AddFindNode(node);
        for (var i = 0; i < node.childNodes.length; i++) {
            this.FillFindNodes(node.childNodes[i]);
        }
    },
    AddFindNode: function (node) {//添加匹配的节点
        var source = this.GetNodeValue(node);
        if (source == null)
            return -1;
        var index = this.GetRegularIndex(source);
        if (index > -1) {
            this.FindNodes.push(node);
        }
        return index;
    },
    Find: function () {//查找
        if (!this.CheckInput()) return;
        if (this.CurrentNodeIndex >= this.FindNodes.length)
            this.ResetFindNodes();
        this.SetCurrentFindNode();
    },
    Replace: function () { //替换
        if (this.CurrentNodeIndex >= this.FindNodes.length)
            return;
        this.Dialog.Editor.UndoRedo.AddDo();
        var source = this.GetNodeValue(this.FindNodes[this.CurrentNodeIndex]);
        var value = source.substring(0, this.NodeOffset - this.FindInput.value.length) + source.substring(this.NodeOffset, source.length);
        this.ReplaceNodeValue(this.FindNodes[this.CurrentNodeIndex], value);
        this.ResetFindNodes();
        this.Find();
    },
    SetCurrentFindNode: function () {
        for (; this.CurrentNodeIndex < this.FindNodes.length; ) {
            var rev = this.SetFindNodeRange(this.FindNodes[this.CurrentNodeIndex]);
            if (rev) {
                break;
            }
            this.NodeOffset = 0;
            this.CurrentNodeIndex++;
        }
    },
    SetFindNodeRange: function (node) {
        var source = this.GetNodeValue(node);
        source = source.substring(this.NodeOffset, source.length);
        var index = this.GetRegularIndex(source);
        if (index >= 0) {
            this.Dialog.Editor.SelectRange.SetRangeByNode(this.FindNodes[this.CurrentNodeIndex], this.NodeOffset, this.FindInput.value.length + this.NodeOffset);
            this.NodeOffset = this.NodeOffset + this.FindInput.value.length;
            return true;
        }
        return false;
    },
    ReplaceAll: function () {//替换所有
        if (!this.CheckInput())
            return;
        this.Dialog.Editor.UndoRedo.AddDo();
        for (var i = 0; i < this.FindNodes.length; i++) {
            this.ReplaceNodeAllValue(this.FindNodes[i]);
        }
    },
    ReplaceNodeAllValue: function (node) {//替换节点内容
        var source = this.GetNodeValue(node);
        if (source == null)
            return;
        var valueArray = [];
        do {
            var index = this.GetRegularIndex(source);
            source = this.PushValue(index, source, valueArray);
        } while (index >= 0);
        valueArray.push(source);
        this.ReplaceNodeValue(node, valueArray.join(''));
    },
    PushValue: function (index, value, valueArray) {//追加节点值
        if (index > -1) {
            valueArray.push(value.substring(0, index) + this.ReplaceInput.value);
            value = value.substring(index + this.FindInput.value.length, value.length);
        }
        return value;
    },
    GetNodeValue: function (node) {//得到节点值
        if (node.nodeName == "#text" || node.childNodes.length == 0) {
            return document.all ? (node.innerText ? node.innerText : node.data) : node.textContent;
        }
        return null;
    },
    ReplaceNodeValue: function (node, value) {//替换节点值
        if (document.all)
            if (node.innerText)
                node.innerText = value;
            else
                node.data = value;
        else
            node.textContent = value;
    },

    GetRegularIndex: function (source) {
        var value = this.CaseWord.checked ? this.FindInput.value : this.FindInput.value.toLowerCase();
        source = this.CaseWord.checked ? source : source.toLowerCase();
        var rg = new RegExp(/^\w*?$/g);
        var replaceReg = this.FullWord.checked && rg.test(value) ? new RegExp("\b" + value + "\b", "g") : new RegExp(value, "g");
        return source.search(replaceReg);
    },
    CheckInput: function () {//检查输入
        if (this.FindInput.value == "") {
            alert(this.Languages.Error);
            return false;
        }
        return true;
    },
    ShowDialog: function () {//显示
        this.Initialize();
        this.Dialog.ShowDialog();
        this.FindInput.focus();
    }
};

//FlashDialog
Winner.Editor.FlashDialog = function (dialog, uploadUrl, browserUrl, languages) {
    this.Base = new Winner.ClassBase();
    this.Dialog = dialog;
    this.UploadUrl = this.SetRequestUrl(uploadUrl, "parent.parent");
    this.BrowserUrl = this.SetRequestUrl(browserUrl, "opener");
    this.Languages = languages;
    this.MaxSize = 1048576;
    this.Extension = ".swf";
    this.Html = "<table class='middle'>" +
    "<tr><th class='th'>@FlashSrc</th><td class='td' colspan='3'><input type='text' Instance='FlashSrc' class='len' /><input type='button' Instance='BrowserButton' value ='@BrowserButton' class='button'/></td></tr>" +
    "<tr Instance='UploadContainer'><th class='th'>@UploadContainer</th><td class='td' colspan='3'><iframe class='iframe' Instance='Iframe' frameborder='0' scrolling='no'></iframe></td></tr>" +
     "<tr><th class='th'>@FlashVariable</th>" +
    "<td class='td' colspan='5'><input Instance='FlashAllowFullScreen' type='checkbox' checked='checked'/><lable>@AllowFullScreen</lable>" +
    "<input Instance='FlashLoop' type='checkbox' checked='checked' /><lable>@FlashLoop</lable>" +
    "<input Instance='FlashPlay' type='checkbox' checked='checked'/><lable>@FlashPlay</lable>" +
    "<input Instance='FlashMenu' type='checkbox' checked='checked'/><lable>@FlashMenu</lable></td></tr>" +
    "<tr><th class='th'>@FlashWidth</th><td class='td'><input type='text' Instance='FlashWidth' /></td>" +
    "<th class='th'>@FlashHeight</th><td class='td'><input type='text' Instance='FlashHeight' /></td></tr>" +
    "<tr><th class='th'>@FlashHspace</th><td class='td'><input type='text' Instance='FlashHspace' /></td>" +
    "<th class='th'>@FlashVspace</th><td class='td' ><input type='text' Instance='FlashVspace' /></td></tr>" +
    "<tr><th class='th'>@FlashScale</th><td class='td'><select Instance='FlashScale'> " +
    "<option value='showall'>@Showall</option><option value=''>@NoSelect</option>" +
    "<option value='noborder'>@Noborder</option><option value='exactfit'>@Exactfit</option></select></td>" +
    "<th class='th'>@FlashSmode</th><td class='td'><select Instance='FlashSmode'><option value='transparent'>@Transparent</option> " +
    "<option value='window'>@Window</option><option value='opaque'>@Opaque</option>" +
    "<option value=''>@NoSelect</option></select></td></tr>" +
    "<tr><th class='th'>@FlashQuality</th><td class='td'><select Instance='FlashQuality'> <option value='best'>@Best</option>" +
    "<option value=''>@NoSelect</option><option value='high'>@High</option>" +
    "<option value='autohigh'>@Autohigh</option><option value='medium'>@Medium</option>" +
    "<option value='autolow'>@Autolow</option><option value='low'>@Low</option></select></td>" +
    "<th class='th'>@FlashAlign</th><td class='td'><select Instance='FlashAlign'><option value='left'>@Left</option> " +
    "<option value=''>@NoSelect</option><option value='absBottom'>@AbsBottom</option>" +
    "<option value='absMiddle'>@AbsMiddle</option><option value='baseline'>@Baseline</option>" +
    "<option value='bottom'>@Bottom</option><option value='middle'>@Middle</option>" +
     "<option value='right'>@Right</option><option value='textTop'>@TextTop</option>" +
     "<option value='top'>@Top</option></select></td></tr>" +
    "<tr><th class='th'>@FlashAllowScriptAccess</th><td class='td' colspan='3'><select Instance='FlashAllowScriptAccess'> " +
    "<option value='always'>@Always</option><option value=''>@NoSelect</option>" +
    "<option value='samedomain'>@Samedomain</option><option value='never'>@Never</option></select></td></tr>" +
    "<tr><th class='th'>@Preview</th><td colspan='3' class='td'><div  Instance='Preview' class='preview' ></div></td></tr></table>";
    this.FormHtml = "<form Instance='Form' target='submit' enctype='multipart/form-data' method='POST' action='" + this.UploadUrl + "'>" +
       "<input type='file' name='flashfile' style='width:150px;'  Instance='UploadFile' />" +
       "<input type='submit' Instance='UploadButton'  disabled = 'disabled'  value ='@UploadButton'/>" +
       "</form><iframe name='submit' style='display:none'></iframe>";
    this.Html = this.Dialog.Editor.ReplaceLanguage(this.Html, this.Languages.Names);
    this.FormHtml = this.Dialog.Editor.ReplaceLanguage(this.FormHtml, this.Languages.Names);
};
Winner.Editor.FlashDialog.prototype =
{
    Initialize: function () { //初始化函数
        this.Dialog.Content.innerHTML = this.Html;
        this.Dialog.Editor.LoadInstances(this, this.Dialog.Content);
        this.Dialog.Title.innerHTML = this.Languages.Title;
        var self = this;
        var func = function () {
            if (self.Iframe.contentWindow != undefined && self.Iframe.contentWindow.document != undefined && self.Iframe.contentWindow.document.body != undefined)
                self.ResetForm();
            else setTimeout(func, 100);
        };
        setTimeout(func, 100);
        this.BindEvent();
    },
    ResetForm: function () {
        var self = this;
        this.Iframe.contentWindow.document.body.style.margin = "0px";
        this.Iframe.contentWindow.document.body.style.padding = "0px";
        if (this.Dialog.Editor.Domain != undefined) {
            this.Iframe.contentWindow.document.domain = this.Dialog.Editor.Domain;
        }
        this.Iframe.contentWindow.document.body.innerHTML = this.FormHtml;
        this.Dialog.Editor.LoadInstances(this, this.Iframe.contentWindow.document.body);
        this.Base.BindEvent(this.UploadFile, "change", function () {
            self.Validate();
        });
    },
    Validate: function () {
        var rev = false;
        var extarr = this.UploadFile.value.split('.');
        var ext = extarr[extarr.length - 1].toLowerCase();
        if (this.Extension.indexOf(ext) > -1) {
            rev = true;
        } else {
            alert(this.Languages.ExtensionErrorMessage);
        }
        if (rev) {
            var sizes = this.Base.GetFileSize(this.UploadFile);
            var size = sizes == null || sizes.length == 0 ? 0 : sizes[0];
            if (size > this.MaxSize) {
                alert(this.Languages.SizeErrorMessage);
                rev = false;
            }

        }
        if (rev)
            this.UploadButton.disabled = "";
        else
            this.UploadButton.disabled = "disabled";
    },
    SetRequestUrl: function (url, objName) {//重写设置url
        if (url != undefined && url != "") {
            url += url.indexOf('?') > -1 ? '&' : '?';
            url += 'function=window.' + objName + '.' + this.Dialog.Editor.ClassName + '.FlashDialog.SetSrc(\"{0}\",\"{1}\")';
        }
        return url;
    },
    BindEvent: function () {//绑定事件
        this.BindSureEvent();
        this.Dialog.SetInputEvent();
        this.BindBrowerEvent();
        this.PreviewEvent();
    },
    PreviewEvent: function () {//绑定浏览
        var self = this;
        this.FlashSrc.onchange = function () {
            var func = function () {
                self.Preview.innerHTML = self.GetFlashHtml();
            };
            setTimeout(func, 0);
        };
    },
    BindBrowerEvent: function () {//绑定浏览服务器事件
        var self = this;
        this.BrowserButton.onclick = function () {
            self.Browser();
        };
    },
    BindSureEvent: function () {//绑定输入事件
        var self = this;
        this.Dialog.Sure.onclick = function () {
            self.CreateImage();
        };
    },
    CreateImage: function () {//创建flash
        if (!this.Check()) {
            return;
        }
        var value = this.GetFlashHtml();
        this.Dialog.CloseDialog();
        this.Dialog.Editor.InsertHtml(value);
    },
    GetFlashHtml: function () {//得到Flash的html
        var rev = "<object classid='clsid:d27cdb6e-ae6d-11cf-96b8-444553540000' codebase='http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,40,0'";
        rev += this.GetFlashPropertyHtml();
        rev += this.GetFlashVariableHtml();
        rev += this.GetFlashSelectHtml();
        rev += this.GetEmbedHtml();
        rev += "</object><br/>";
        return rev;
    },
    GetFlashPropertyHtml: function () {//得到Flash属性
        var rev = "";
        rev += this.FlashWidth.value == "" ? "" : " width='" + this.FlashWidth.value + "'";
        rev += this.FlashHeight.value == "" ? "" : " height='" + this.FlashHeight.value + "'";
        rev += this.FlashHspace.value == "" ? "" : " hspace='" + this.FlashHspace.value + "'";
        rev += this.FlashVspace.value == "" ? "" : " vspace='" + this.FlashVspace.value + "'";
        rev += this.FlashAlign.value == "" ? "" : " align='" + this.FlashAlign.value + "'";
        rev += " >";
        return rev;
    },
    GetFlashVariableHtml: function () {//返回变量设置
        var rev = "";
        var template = "<param name=\"{0}\" value=\"false\" />";
        rev += this.FlashAllowFullScreen.checked ? "" : template.replace("{0}", "allowFullScreen");
        rev += this.FlashLoop.checked ? "" : template.replace("{0}", "loop");
        rev += this.FlashPlay.checked ? "" : template.replace("{0}", "play");
        rev += this.FlashMenu.checked ? "" : template.replace("{0}", "menu");
        return rev;
    },
    GetFlashSelectHtml: function () {//返回变量设置
        var rev = "";
        var template = "<param name=\"{0}\" value=\"{1}\" />";
        rev += this.FlashScale.value == "" ? "" : template.replace("{0}", "scale").replace("{1}", this.FlashScale.value);
        rev += this.FlashSmode.value == "" ? "" : template.replace("{0}", "smode").replace("{1}", this.FlashSmode.value);
        rev += this.FlashQuality.value == "" ? "" : template.replace("{0}", "quality").replace("{1}", this.FlashQuality.value);
        rev += this.FlashAllowScriptAccess.value == "" ? "" : template.replace("{0}", "allowScriptAccess").replace("{1}", this.FlashAllowScriptAccess.value);
        return rev;
    },
    GetEmbedHtml: function () {//得到EmbedHtml
        var rev = "<param name=\"movie\" value=\"" + this.FlashSrc.value + "\" /> <embed ";
        rev += this.GetEmbedVariableHtml();
        rev += this.GetEmbedProperty();
        rev += this.GetEmbedSelectHtml();
        rev += "></embed>";
        return rev;
    },
    GetEmbedProperty: function () {
        var rev = "pluginspage='http://www.macromedia.com/go/getflashplayer'";
        rev += "type='application/x-shockwave-flash'";
        rev += "src='" + this.FlashSrc.value + "'";
        return rev;
    },
    GetEmbedVariableHtml: function () {//得到EmbedVariable
        var rev = "";
        rev += this.FlashAllowFullScreen.checked ? "" : "allowfullscreen='false'";
        rev += this.FlashLoop.checked ? "" : "loop='false'";
        rev += this.FlashPlay.checked ? "" : "play='false'";
        rev += this.FlashMenu.checked ? "" : "menu='false'";
        return rev;
    },
    GetEmbedSelectHtml: function () {//得到EmbedSelect
        var rev = "";
        rev += this.FlashScale.value == "" ? "" : "scale='" + this.FlashScale.value + "'";
        rev += this.FlashSmode.value == "" ? "" : "smode='" + this.FlashSmode.value + "'";
        rev += this.FlashQuality.value == "" ? "" : "quality='" + this.FlashQuality.value + "'";
        rev += this.FlashAllowScriptAccess.value == "" ? "" : "allowScriptAccess='" + this.FlashAllowScriptAccess.value + "'";
        return rev;
    },
    Browser: function () {//浏览服务器
        window.open(this.BrowserUrl);
    },
    ShowDialog: function () {//显示
        this.Initialize();
        this.Dialog.ShowDialog();
        this.HideUrl();
        this.FlashSrc.focus();
        if (arguments[0] != undefined) {
            this.SetShowDefaultInfo(arguments[0]);
        }
    },
    SetShowDefaultInfo: function (obj) {//设置默认
        this.SetShowPropertyInfo(obj);
        var embeds = obj.getElementsByTagName('embed');
        this.SetShowEmbedSelectInfo(embeds[0]);
        this.SetShowEmbedVariableInfo(embeds[0]);
        this.SetSrc(this.Dialog.Editor.Base.GetAttribute(embeds[0], "src"));
    },
    SetShowPropertyInfo: function (obj) {//设置属性
        var getFunc = this.Dialog.Editor.Base.GetAttribute;
        this.FlashWidth.value = getFunc(obj, "width") == null ? "" : getFunc(obj, "width");
        this.FlashHeight.value = getFunc(obj, "height") == null ? "" : getFunc(obj, "height");
        this.FlashHspace.value = getFunc(obj, "hspace") == null ? "" : getFunc(obj, "hspace");
        this.FlashVspace.value = getFunc(obj, "vspace") == null ? "" : getFunc(obj, "vspace");
        this.FlashAlign.value = getFunc(obj, "align") == null ? "" : getFunc(obj, "align");
    },
    SetShowEmbedSelectInfo: function (obj) {//设置选择
        var getFunc = this.Dialog.Editor.Base.GetAttribute;
        this.FlashScale.value = getFunc(obj, "scale") == null ? "" : getFunc(obj, "scale");
        this.FlashSmode.value = getFunc(obj, "smode") == null ? "" : getFunc(obj, "smode");
        this.FlashQuality.value = getFunc(obj, "quality") == null ? "" : getFunc(obj, "quality");
        this.FlashAllowScriptAccess.value = getFunc(obj, "allowScriptAccess") == null ? "" : getFunc(obj, "allowScriptAccess");
    },
    SetShowEmbedVariableInfo: function (obj) {//设置变量
        var getFunc = this.Dialog.Editor.Base.GetAttribute;
        this.FlashAllowFullScreen.checked = getFunc(obj, "allowFullScreen") == null ? true : false;
        this.FlashLoop.checked = getFunc(obj, "loop") == null ? true : false;
        this.FlashPlay.checked = getFunc(obj, "play") == null ? true : false;
        this.FlashMenu.checked = getFunc(obj, "menu") == null ? true : false;
    },
    Check: function () {//检查flash路径
        if (this.FlashSrc.value == "") {
            alert(this.Languages.Error);
            return false;
        }
        return true;
    },
    HideUrl: function () {//隐藏控件
        if (this.UploadUrl == undefined || this.UploadUrl=="") {
            this.UploadContainer.style.display = 'none';
        }
        if (this.BrowserUrl == undefined || this.BrowserUrl == "") {
            this.BrowserButton.style.display = "none";
        }
    },
    SetSrc: function (url, message) {//设置URL
        if (message == undefined || message == '') {
            this.FlashSrc.value = url;
            this.FlashSrc.onchange();
            this.ResetForm();
            return;
        }
        alert(message);
    }
};
//ImageDialog
Winner.Editor.ImageDialog = function (dialog, uploadUrl, browserUrl, languages) {
    this.Base = new Winner.ClassBase();
    this.MaxSize = 307200;
    this.Extension = ".jpg.gif.png.bmp.jpeg";
    this.Dialog = dialog;
    this.UploadUrl = this.SetRequestUrl(uploadUrl, "parent.parent");
    this.BrowserUrl = this.SetRequestUrl(browserUrl, "opener");
    this.Languages = languages;
    this.Html = "<table class='middle'>" +
    "<tr><th class='th'>@ImageSrc</th><td colspan='5' class='td'><input type='text' Instance='ImageSrc' /><input type='button' class='button' Instance='BrowserButton' value ='@BrowserButton'/></td></tr>" +
    "<tr Instance='UploadContainer'><th class='th'>@UploadContainer</th><td colspan='5' class='td'><iframe Instance='Iframe' class='iframe' frameborder='0' scrolling='no'></iframe></td></tr>" +
    "<tr><th class='th'>@ImageAlt</th><td class='td'><input type='text' Instance='ImageAlt' class='name' /></td>" +
    "<th class='th'>@ImageTitle</th><td class='td'><input type='text' Instance='ImageTitle' class='name'/></td>" +
    "<th class='th'>@ImageClass</th><td class='td'><input type='text' Instance='ImageClass' class='name'/></td></tr>" +
    "<tr><th class='th'>@ImageStyle</th><td class='td' colspan='5'><input type='text'  Instance='ImageStyle' class='len' /></td></tr>" +
    "<tr><th class='th'>@Preview</th><td class='td' colspan='5' ><div  Instance='Preview' class='preview' ></div></td></tr>" +
    "</table>";
    this.FormHtml = "<form Instance='Form' target='submit' enctype='multipart/form-data' method='POST' action='" + this.UploadUrl + "'>" +
       "<input type='file' name='imagefile' style='width:150px;'  Instance='UploadFile' />" +
       "<input type='submit' disabled='disabled' Instance='UploadButton' value ='@UploadButton'/>" +
       "</form><iframe name='submit' style='display:none'></iframe>";
    this.Html = this.Dialog.Editor.ReplaceLanguage(this.Html, this.Languages.Names);
    this.FormHtml = this.Dialog.Editor.ReplaceLanguage(this.FormHtml, this.Languages.Names);

};
Winner.Editor.ImageDialog.prototype =
{
    Initialize: function () { //初始化函数
        this.Dialog.Content.innerHTML = this.Html;
        this.Dialog.Editor.LoadInstances(this, this.Dialog.Content);
        this.Dialog.Title.innerHTML = this.Languages.Title;
        var self = this;
        var func = function () {
            if (self.Iframe.contentWindow != undefined && self.Iframe.contentWindow.document != undefined && self.Iframe.contentWindow.document.body != undefined)
                self.ResetForm();
            else setTimeout(func, 100);
        };
        setTimeout(func, 100);
        this.BindEvent();
    },
    ResetForm: function () {
        this.Iframe.contentWindow.document.body.style.margin = "0px";
        this.Iframe.contentWindow.document.body.style.padding = "0px";
        if (this.Dialog.Editor.Domain != undefined) {
            this.Iframe.contentWindow.document.domain = this.Dialog.Editor.Domain;
        }
        this.Iframe.contentWindow.document.body.innerHTML = this.FormHtml;
        this.Dialog.Editor.LoadInstances(this, this.Iframe.contentWindow.document.body);
        var self = this;
        this.Base.BindEvent(this.UploadFile, "change", function () {
            self.Validate();
        });
    },
    Validate: function () {
        var rev = false;
        var extarr = this.UploadFile.value.split('.');
        var ext = extarr[extarr.length - 1].toLowerCase();
        if (this.Extension.indexOf(ext) > -1) {
            rev = true;
        } else {
            alert(this.Languages.ExtensionErrorMessage);
        }
        if (rev) {
            var sizes = this.Base.GetFileSize(this.UploadFile);
            var size = sizes == null || sizes.length == 0 ? 0 : sizes[0];
            if (size > this.MaxSize) {
                alert(this.Languages.SizeErrorMessage);
                rev = false;
            }
        }
        if (rev)
            this.UploadButton.disabled = "";
        else
            this.UploadButton.disabled = "disabled";
    },
    SetRequestUrl: function (url, objName) {//重写设置url
        if (url != undefined && url!="") {
            url += url.indexOf('?') > -1 ? '&' : '?';
            url += 'function=window.' + objName + '.' + this.Dialog.Editor.ClassName + '.ImageDialog.SetSrc(\"{0}\",\"{1}\")';
        }
        return url;
    },
    BindEvent: function () {//绑定事件
        this.BindSureEvent();
        this.Dialog.SetInputEvent();
        this.BindBrowerEvent();
        this.PreviewEvent();
    },
    PreviewEvent: function () {//绑定浏览
        var self = this;
        this.ImageSrc.onchange = function () {
            var func = function () {
                self.Preview.innerHTML = self.GetImageHtml();
            };
            setTimeout(func, 0);
        };
    },
    BindBrowerEvent: function () {//绑定浏览服务器事件
        var self = this;
        this.BrowserButton.onclick = function () {
            self.Browser();
        };
    },
    BindSureEvent: function () {//绑定输入事件
        var self = this;
        this.Dialog.Sure.onclick = function () {
            self.CreateImage();
        };
    },
    CreateImage: function () {//创建图片
        if (!this.Check()) {
            return;
        }
        var value = this.GetImageHtml();
        this.Dialog.CloseDialog();
        this.Dialog.Editor.InsertHtml(value);
    },
    GetImageHtml: function () {//得到图片的html
        var rev = "<img";
        rev += this.GetImagePropertyHtml();
        rev += this.GetImageStyleHtml();
        rev += "/><br/>";
        return rev;
    },
    GetImagePropertyHtml: function () {//得到图片属性
        var rev = "";
        if (this.ImageSrc.value != "")
            rev += " src='" + this.ImageSrc.value + "'";
        if (this.ImageAlt.value != "")
            rev += " alt='" + this.ImageAlt.value + "'";
        if (this.ImageTitle.value != "")
            rev += " title='" + this.ImageTitle.value + "'";
        return rev;
    },
    GetImageStyleHtml: function () {//得到table的样式
        var rev = "";
        if (this.ImageClass.value != "")
            rev += " class='" + this.ImageClass.value + "'";
        if (this.ImageStyle.value != "")
            rev += " style='" + this.ImageStyle.value + "'";
        return rev;
    },
    Browser: function () {//浏览服务器
        window.open(this.BrowserUrl);
    },
    ShowDialog: function () {//显示
        this.Initialize();
        this.Dialog.ShowDialog();
        this.HideUrl();
        this.ImageSrc.focus();
        if (arguments[0] != undefined) {
            this.SetShowDefaultInfo(arguments[0]);
        }
    },
    SetShowDefaultInfo: function (obj) {//设置默认
        this.SetSrc(obj.src);
        this.ImageAlt.value = obj.alt;
        this.ImageTitle.value = obj.title;
        this.ImageClass.value = obj.className;
        this.ImageStyle.value = obj.attributes["style"].value;
    },
    Check: function () {
        if (this.ImageSrc.value == "") {
            alert(this.Languages.Error);
            return false;
        }
        return true;
    },
    HideUrl: function () {//隐藏控件
        if (this.UploadUrl == undefined || this.UploadUrl == "") {
            this.UploadContainer.style.display = 'none';
        }
        if (this.BrowserUrl == undefined || this.BrowserUrl == "") {
            this.BrowserButton.style.display = "none";
        }
    },
    SetSrc: function (url, message) {//设置URL
        if (message == undefined || message == '') {
            this.ImageSrc.value = url;
            this.ImageSrc.onchange();
            this.ResetForm();
            return;
        }
        alert(message);
    }
};


//LinkDialog
Winner.Editor.LinkDialog = function (dialog, languages) {
    this.Dialog = dialog;
    this.Languages = languages;
    this.Html = "<select Instance='Select'> <option selected='true' value='Url'>@UrlSelect</option><option value='Anchorpoint'>@AnchorpointSelect</option><option value='Email'>@EmailSelect</option></select>" +
    "<table class='middle' Instance='UrlContent' class='url'><tr><th class='th'>@UrlName</th><td class='td'><input type='text' Instance='UrlInput' /></td></tr></table>" +
    "<table class='middle' Instance='AnchorpointContent' class='anchorpoint'><tr><th class='th'>@AnchorpointName</th><td class='td'><input type='text' Instance='AnchorpointInput' /></td></tr></table>" +
    "<table class='middle' Instance='EmailContent' class='email'>" +
    "<tr><th class='th'>@AddressName</th><td class='td'><input type='text' Instance='EmailAddress' /></td></tr>" +
    "<tr><th class='th'>@SubjectName</th><td class='td'><input type='text' Instance='EmailSubject' /></td></tr>" +
    "<tr><th class='th'>@BodyName</th><td class='td'><textarea Instance='EmailBody'></textarea></td></tr>" +
    " </table>";
    this.Html = this.Dialog.Editor.ReplaceLanguage(this.Html, this.Languages.Names);
};
Winner.Editor.LinkDialog.prototype =
{
    Initialize: function () { //初始化函数

        this.Dialog.Content.innerHTML = this.Html;
        this.Dialog.Title.innerHTML = this.Languages.Title;
        this.Dialog.Editor.LoadInstances(this, this.Dialog.Content);
        this.BindEvent();
    },
    BindEvent: function () {//绑定事件
        this.BindSureEvent();
        this.Dialog.SetInputEvent();
        this.BindSelectChange();
    },
    BindSelectChange: function () {//绑定选择事件
        var self = this;
        this.Select.onchange = function () {
            self.ChangeSelect();
        };
    },
    BindSureEvent: function () {//绑定输入事件
        var self = this;
        this.Dialog.Sure.onclick = function () {
            self.CreateLink();
        };
    },
    ChangeSelect: function () {//改变选择
        this.UrlContent.style.display = 'none';
        this.AnchorpointContent.style.display = 'none';
        this.EmailContent.style.display = 'none';
        switch (this.Select.value) {
            case "Url": this.UrlContent.style.display = ''; this.UrlInput.focus(); break;
            case "Anchorpoint": this.AnchorpointContent.style.display = ''; this.AnchorpointInput.focus(); break;
            case "Email": this.EmailContent.style.display = ''; this.EmailAddress.focus(); break;
        }
    },
    SetShowDefaultInfo: function (obj) {//设置选择
        if (obj != undefined) {
            if (obj.name != "")
                this.SetShowDefaultAnchorpointInfo(obj);
            else if (obj.href.indexOf("mailto:") > -1)
                this.SetShowDefaultEmailInfo(obj);
            else
                this.SetShowDefaultUrlInfo(obj);
        }
        this.ChangeSelect();
    },
    SetShowDefaultUrlInfo: function (obj) {//设置连接信息
        this.UrlInput.value = obj.href;
        this.Select.value = "Url";
    },
    SetShowDefaultAnchorpointInfo: function (obj) {//设置锚点信息
        this.AnchorpointInput.value = obj.name;
        this.Select.value = "Anchorpoint";
    },
    SetShowDefaultEmailInfo: function (obj) {//设置Email信息
        this.Select.value = "Email";
        var href = obj.href.replace("mailto:", "");
        var arr = href.split('?');
        this.EmailAddress.value = arr[0];
        if (arr.length > 1) {
            var args = this.GetUrlParms(arr[1]);
            this.EmailSubject.value = args["subject"];
            this.EmailBody.value = args["subject"];
        }
        this.Select.value = "Email";
    },
    GetUrlParms: function (search) {//得到URL参数
        var args = new Object();
        var pairs = search.split("&"); //在逗号处断开
        for (var i = 0; i < pairs.length; i++) {
            var pos = pairs[i].indexOf('='); //查找name=value
            if (pos == -1) continue; //如果没有找到就跳过
            var argname = pairs[i].substring(0, pos); //提取name
            var value = pairs[i].substring(pos + 1); //提取value
            args[argname] = unescape(value); //存为属性
        }
        return args;
    },
    ShowDialog: function () {//显示
        this.Initialize();
        this.Dialog.ShowDialog();
        this.SetShowDefaultInfo(arguments[0]);
    },
    CreateLink: function () {//创建连接
        if (!this.Check())
            return;
        var html = this.GetLinkHtml();
        this.Dialog.CloseDialog();
        this.Dialog.Editor.InsertHtml(html);
    },
    GetLinkHtml: function () {//得到连接代码
        var html = "";
        switch (this.Select.value) {
            case "Url": html = this.GetUrlLinkHtml(); break;
            case "Anchorpoint": html = this.GetAnchorpointHtml(); break;
            case "Email": html = this.GetEmailLinkHtml(); break;
        }
        return html;
    },
    GetAnchorpointHtml: function () {
        return "<a name='" + this.AnchorpointInput.value + "'></a>";
    },
    GetUrlLinkHtml: function () {//得到Url连接代码
        var rev = "<a href='" + this.UrlInput.value + "'>";
        rev += this.CheckSelectText() ? this.Dialog.Editor.SelectRange.Text : this.UrlInput.value;
        rev += "</a>";
        return rev;
    },
    GetEmailLinkHtml: function () {//得到邮件HTML代码
        var rev = "<a href='mailto:" + this.EmailAddress.value;
        rev = this.GetEmailSubjectLinkHtml(rev);
        rev = this.GetEmailBodyLinkHtml(rev);
        rev = rev + "'>" + (this.CheckSelectText() ? this.Dialog.Editor.SelectRange.Text : this.EmailAddress.value);
        rev += "</a>";
        return rev;
    },
    GetEmailSubjectLinkHtml: function (source) {//得到邮件标题HTML代码
        if (this.EmailSubject.value != '') {
            source += source.indexOf('?') == -1 ? "?" : "&";
            source += "subject=" + this.EmailSubject.value;
        }
        return source;
    },
    GetEmailBodyLinkHtml: function (source) {//得到内容标题HTML代码
        if (this.EmailBody.value != '') {
            source += source.indexOf('?') == -1 ? "?" : "&";
            source += "body=" + this.EmailBody.value;
        }
        return source;
    },
    CheckSelectText: function () {//检查是否有文字
        return this.Dialog.Editor.SelectRange.Text != undefined && this.Dialog.Editor.SelectRange.Text != "";
    },
    Check: function () {//检查输入
        var mess = "";
        switch (this.Select.value) {
            case "Url": mess = this.UrlInput.value == '' ? this.Languages.UrlError : ""; break;
            case "Anchorpoint": mess = this.AnchorpointInput.value == '' ? this.Languages.AnchorpointError : ""; break;
            case "Email": mess = this.EmailAddress.value == '' ? this.Languages.EmailError : ""; break;
        }
        if (mess == "")
            return true;
        alert(mess);
        return false;
    }
};
//TableDialog
Winner.Editor.TableDialog = function (dialog, languages) {
    this.Dialog = dialog;
    this.Languages = languages;
    this.Html = "<table class='middle'>" +
   "<tr>" +
   "<th class='th'>@CellCount</th><td class='td'><input type='text' class='number' Instance='CellCount' value='3'/></td>" +
    "<th class='th'>@RowCount</th><td class='td'><input type='text'  class='number' Instance='RowCount'  value='3'/></td>" +
    "<th class='th'>@TableClass</th><td class='td'><input type='text'  class='number' Instance='TableClass' /></td>" +
    "<th class='th'>@TableBorder</th><td class='td'><input type='text'  class='number' Instance='TableBorder' value='1' /></td>" +
   "</tr>" +
   "<tr>" +
   "<th class='th'>@TableStyle</th><td colspan='7' class='td'><input type='text' class='len' value='padding:0px;margin:0px;width:400px;' Instance='TableStyle' /></td>" +
   "</tr>" +
   "<tr>" +
   "<th class='th'>@CaptionName</th><td colspan='5' class='td'><input type='text' class='len' Instance='CaptionName'/></td>" +
    "<th class='th'>@CaptionClass</th><td class='td'><input type='text' Instance='CaptionClass'/></td>" +
   "</tr>" +
    "<tr>" +
    "<th class='th'>@CaptionStyle</th><td colspan='7' class='td'><input type='text'  class='len' Instance='CaptionStyle' /></td>" +
   "</tr>" +
   "<tr>" +
   "<th class='th'>@Summary</th><td colspan='7' class='td'><input type='text' class='len' Instance='Summary'/></td>" +
   "</tr>" +
   "</table>";
    this.Html = this.Dialog.Editor.ReplaceLanguage(this.Html, this.Languages.Names);
};
Winner.Editor.TableDialog.prototype =
{
    Initialize: function () { //初始化函数
        this.Dialog.Content.innerHTML = this.Html;
        this.Dialog.Editor.LoadInstances(this, this.Dialog.Content);
        this.Dialog.Title.innerHTML = this.Languages.Title;
        this.BindEvent();
    },
    BindEvent: function () {//绑定事件
        this.BindSureEvent();
        this.Dialog.SetInputEvent();
    },
    BindSureEvent: function () {//绑定输入事件
        var self = this;
        this.Dialog.Sure.onclick = function () {
            self.CreateTable();
        };
    },
    CreateTable: function () {//创建表格
        if (!this.Check())
            return;
        var value = this.GetTableHtml();
        this.Dialog.CloseDialog();
        if (this.Table == undefined) {
            this.Dialog.Editor.InsertHtml(value + "<br/>");
        }
        else {
            this.Table.outerHTML = value;
            this.Dialog.Editor.ContentEvent.BindTableEvent();
        }
    },
    Check: function () {//检查
        var rev = this.CheckRowCount();
        if (rev) {
            rev = this.CheckCellCount();
        }
        return rev;
    },
    CheckRowCount: function () {//检查行数
        try {
            parseInt(this.RowCount.value);
        }
        catch (ex) {
            alert(this.Languages.RowError);
            return false;
        }
        return true;
    },
    CheckCellCount: function () {//检查列数
        try {
            parseInt(this.CellCount.value);
        }
        catch (ex) {
            alert(this.Languages.CellError);
            return false;
        }
        return true;
    },
    GetTableHtml: function () {//得到table的html
        var rev = "<table";
        rev += this.GetTableStyleHtml();
        rev += ">" + this.GetTableCaptionHtml() + "<tbody>";
        rev += this.GetTableRowHtml();
        rev += "<tbody></table>";
        return rev;
    },
    GetTableStyleHtml: function () {//得到table的样式
        var rev = "";
        if (this.TableBorder.value != "")
            rev += " border='" + this.TableBorder.value + "px'";
        if (this.TableClass.value != "")
            rev += " class='" + this.TableClass.value + "'";
        if (this.TableStyle.value != "")
            rev += " style='" + this.TableStyle.value + "'";
        return rev;
    },
    GetTableCaptionHtml: function () {//得到标题
        var rev = "";
        if (this.CaptionName.value != "") {
            rev = "<caption";
            if (this.CaptionClass.value != "")
                rev += " class='" + this.CaptionClass.value + "'";
            if (this.CaptionStyle.value != "")
                rev += " style='" + this.CaptionStyle.value + "'";
            rev += ">" + this.CaptionName.value + "</caption>";
        }
        return rev;
    },
    GetTableRowHtml: function () {//得到表格行
        var rev = "";
        for (var i = 0; i < parseInt(this.RowCount.value); i++) {
            rev += "<tr>" + this.GetTableCellHtml() + "</tr>";
        }
        return rev;
    },
    GetTableCellHtml: function () {//得到表格列
        var rev = "";
        for (var i = 0; i < parseInt(this.CellCount.value); i++) {
            rev += "<td>&nbsp;</td>";
        }
        return rev;
    },
    ShowDialog: function () {//显示
        this.Initialize();
        this.Dialog.ShowDialog();
        this.CellCount.focus();
        if (arguments[0] != undefined) {
            this.SetShowDefaultInfo(arguments[0]);
        }
    },
    SetShowDefaultInfo: function (table) {//设置默认值
        this.Table = table;
        this.RowCount.value = table.rows.length;
        this.CellCount.value = table.rows[0].cells.length;
        this.SetShowStyleInfo(table);
        this.SetShowCaptionInfo(table);
    },
    SetShowStyleInfo: function (table) {//显示表格样式
        var getAttribute = this.Dialog.Editor.Base.GetAttribute;
        this.TableBorder.value = getAttribute(table, "border") == null ? "" : getAttribute(table, "border");
        this.TableClass.value = getAttribute(table, "class") == null ? "" : getAttribute(table, "class");
        this.TableStyle.value = getAttribute(table, "style") == null ? "" : getAttribute(table, "style");
    },
    SetShowCaptionInfo: function (table) {//显示表格标题
        var captions = table.getElementsByTagName('caption');
        if (captions != null && captions.length > 0) {
            var getAttribute = this.Dialog.Editor.Base.GetAttribute;
            this.CaptionName.value = captions[0].innerHTML;
            this.CaptionClass.value = getAttribute(captions[0], "class") == null ? "" : getAttribute(captions[0], "class");
            this.CaptionStyle.value = getAttribute(captions[0], "style") == null ? "" : getAttribute(captions[0], "style");
        }
    }
};


//TemplateDialog
Winner.Editor.TemplateDialog = function (dialog, uploadUrl, browserUrl, languages) {
    this.Dialog = dialog;
    this.Base = new Winner.ClassBase();
    this.UploadUrl = this.SetRequestUrl(uploadUrl, "parent.parent");
    this.BrowserUrl = this.SetRequestUrl(browserUrl, "opener");
    this.Languages = languages;
    this.Html = "<table class='middle'>" +
    "<tr><th class='th'>@TemplateSrc</th><td colspan='3' class='td'><input type='text' Instance='TemplateSrc' class='len' /><input type='button' Instance='BrowserButton' value ='@BrowserButton' class='button'/></td></tr>" +
    "<tr Instance='UploadContainer'> <th class='th'> @UploadContainer</th><td colspan='3' class='td'><iframe Instance='Iframe' frameborder='0' class='iframe' scrolling='no' ></iframe></td></tr>" +
    "<tr><th class='th'>@TemplateID</th><td class='td'><input type='text' Instance='TemplateID' /></td>" +
    "<th class='th'>@IsReplace</th><td class='td'><input type='checkbox' Instance='IsReplace' /></td></tr>" +
    "<tr><th class='th'>@Preview</th><td colspan='3' class='td' ><div  Instance='Preview' class='preview' ></div></td></tr>" +
    "</table>";
    this.FormHtml = "<form Instance='Form' target='submit' enctype='multipart/form-data' method='POST' action='" + this.UploadUrl + "'>" +
       "<input type='text' name='editorvalue' style='display:none'  Instance='EditorValue' />" +
        "<input type='text' name='templatename'   Instance='TemplateName' />" +
       "<input type='submit' Instance='UploadButton' value ='@UploadButton' class='button'/>" +
       "</form><iframe name='submit' style='display:none'></iframe>";
    this.Html = this.Dialog.Editor.ReplaceLanguage(this.Html, this.Languages.Names);
    this.FormHtml = this.Dialog.Editor.ReplaceLanguage(this.FormHtml, this.Languages.Names);
};
Winner.Editor.TemplateDialog.prototype =
{
    Initialize: function () { //初始化函数
        this.Dialog.Content.innerHTML = this.Html;
        this.Dialog.Editor.LoadInstances(this, this.Dialog.Content);
        this.Dialog.Title.innerHTML = this.Languages.Title;
        var self = this;
        var func = function () {
            if (self.Iframe.contentWindow != undefined && self.Iframe.contentWindow.document != undefined && self.Iframe.contentWindow.document.body != undefined)
                self.ResetForm();
            else setTimeout(func, 100);
        };
        setTimeout(func, 100);
        this.BindEvent();
    },
    ResetForm: function () { //设置form
        var self = this;
        this.Iframe.contentWindow.document.body.style.margin = "0px";
        this.Iframe.contentWindow.document.body.style.padding = "0px";
        if (this.Dialog.Editor.Domain != undefined) {
            this.Iframe.contentWindow.document.domain = this.Dialog.Editor.Domain;
        }
        this.Iframe.contentWindow.document.body.innerHTML = this.FormHtml;
        this.Dialog.Editor.LoadInstances(this, this.Iframe.contentWindow.document.body);
        this.EditorValue.value = this.Dialog.Editor.EditDocument.body.innerHTML;
        this.Base.BindEvent(this.UploadButton, "click", function (event) {
            var rev = self.Validate();
            if (!rev)
                return self.Base.CancelEventUp(event);
            return true;
        });
    },
    Validate: function () {
        if (this.TemplateName.value == "") {
            alert(this.Languages.TemplateNameErrorMessage);
            return false;
        }
        return true;
    },
    SetRequestUrl: function (url, objName) {//重写设置url
        if (url != undefined && url != "") {
            url += url.indexOf('?') > -1 ? '&' : '?';
            url += 'function=window.' + objName + '.' + this.Dialog.Editor.ClassName + '.TemplateDialog.SetSrc(\"{0}\",\"{1}\")';
        }
        return url;
    },
    BindEvent: function () {//绑定事件
        this.BindSureEvent();
        this.Dialog.SetInputEvent();
        this.BindBrowerEvent();
        this.PreviewEvent();
    },
    PreviewEvent: function () {//绑定浏览
        var self = this;
        var func = function () {//异步加载
            self.SetPreviewHtml();
        };
        this.TemplateID.onchange = function () {
            setTimeout(func, 0);
        };
        this.TemplateSrc.onchange = function () {
            setTimeout(func, 0);
        };
    },
    BindBrowerEvent: function () {//绑定浏览服务器事件
        var self = this;
        this.BrowserButton.onclick = function () {
            self.Browser();
        };
    },
    BindSureEvent: function () {//绑定输入事件
        var self = this;
        this.Dialog.Sure.onclick = function () {
            self.CreateTemplate();
        };
    },
    CreateTemplate: function () {//创建模板
        if (!this.Check()) {
            return;
        }
        var value = this.Preview.innerHTML;
        this.Dialog.CloseDialog();
        if (this.IsReplace.checked) {
            this.Dialog.Editor.EditDocument.body.innerHTML = value;
        }
        else {
            this.Dialog.Editor.InsertHtml(value);
        }
    },
    SetPreviewHtml: function () {//设置预览
        var self = this;
        var iframe = document.createElement('iframe');
        iframe.src = this.TemplateSrc.value;
        this.Dialog.Editor.Base.BindEvent(iframe, "load", function () {
            if (iframe.contentWindow != undefined && iframe.contentWindow.document != undefined && iframe.contentWindow.document.body != undefined) {
                if (self.Dialog.Editor.Domain != undefined) {
                    iframe.contentWindow.document.domain = self.Dialog.Editor.Domain;
                }
            }
            self.Preview.innerHTML = self.GetTemplateHtml(iframe);
        });
        this.Preview.appendChild(iframe);
    },
    GetTemplateHtml: function (iframe) {//得到HTML代码
        try {
            return this.GetPreviewIframeHtml(iframe);
        }
        catch (e) {
            return "";
        }
    },
    GetPreviewIframeHtml: function (iframe) {
        if (this.TemplateID.value == "") {
            return iframe.contentWindow.document.body.innerHTML;
        }
        else {
            var obj = iframe.contentWindow.document.getElementById(this.TemplateID.value);
            if (obj != null) {
                return obj.innerHTML;
            }
        }
        return "";
    },
    Browser: function () {//浏览服务器
        window.open(this.BrowserUrl);
    },
    ShowDialog: function () {//显示
        this.Initialize();
        this.Dialog.ShowDialog();
        this.HideUrl();
        this.TemplateSrc.focus();
    },
    Check: function () {
        if (this.Preview.innerHTML == "") {
            alert(this.Languages.Error);
            return false;
        }
        return true;
    },
    HideUrl: function () {//隐藏控件
        if (this.UploadUrl == undefined || this.UploadUrl == "") {
            this.UploadContainer.style.display = 'none';
        }
        if (this.BrowserUrl == undefined || this.BrowserUrl == "") {
            this.BrowserButton.style.display = "none";
        }
    },
    SetSrc: function (url, message) {//设置URL
        if (message == undefined || message == '') {
            this.TemplateSrc.value = url;
            this.TemplateSrc.onchange();
            this.ResetForm();
            return;
        }
        alert(message);
    }
};

//HideTag
Winner.Editor.HideTag = function (editor) {
    this.Editor = editor;
};
Winner.Editor.HideTag.prototype =
{
    Initialize: function () {
        this.BindEvent();
    },
    BindEvent: function () {//绑定事件
        var self = this;
        var obj = document.all ? this.Editor.EditDocument.body : this.Editor.Iframe.contentWindow;
        this.Editor.Base.BindEvent(obj, "blur", function () {
            self.SetTextChange(true);
        });
        this.Editor.Base.BindEvent(this.Editor.Input, "blur", function () {
            self.SetTextChange(false);
        });
    },
    SetTextChange: function (isIframe) {//设置TextChange
        if (isIframe) {
            this.Editor.Input.value = this.GetReplaceImageToText();
        }
        else {
            this.Editor.EditDocument.body.innerHTML = this.Editor.Input.value;
            this.ReplaceTextToImage();
        }
    },
    ReplaceTextToImage: function () {//替换隐藏
        this.ReplaceLinkTextToImage();
        this.ReplaceFlashTextToImage();
    },
    ReplaceLinkTextToImage: function () {//替换锚点连接
        var objs = this.Editor.EditDocument.getElementsByTagName('a');
        for (var i = 0; i < objs.length; ) {
            if (objs[i].name != "") {
                this.ReplaceObjectValueToImage(objs[i], this.Editor.Path + "/Images/anchor.png");
            }
            else {
                i++;
            }
        }
    },
    ReplaceFlashTextToImage: function () {//替换锚点连接
        var objs = this.Editor.EditDocument.getElementsByTagName('object');
        for (var i = 0; i < objs.length; ) {
            this.ReplaceObjectValueToImage(objs[i], this.Editor.Path + "/Images/flash.png");
        }
    },
    ReplaceObjectValueToImage: function (obj, src) {//替换对象
        var img = document.createElement('img');
        img.src = src;
        this.Editor.Base.SetAttribute(img, "ObjectValue", encodeURI(obj.outerHTML));
        obj.parentNode.insertBefore(img, obj);
        obj.parentNode.removeChild(obj);
    },
    GetReplaceImageToText: function () {//替换图片到文本
        var tBody = document.createElement('div');
        tBody.innerHTML = this.Editor.EditDocument.body.innerHTML;
        var imgs = tBody.getElementsByTagName('img');
        for (var i = 0; i < imgs.length; ) {
            if (this.Editor.Base.GetAttribute(imgs[i], "ObjectValue") != null) {
                this.ReplaceImageToObjectValue(imgs[i]);
            }
            else {
                i++;
            }
        }
        return tBody.innerHTML;
    },
    ReplaceImageToObjectValue: function (img) {//图像用值替换
        var div = document.createElement('div');
        div.innerHTML = decodeURI(this.Editor.Base.GetAttribute(img, "ObjectValue"));
        img.parentNode.insertBefore(div.childNodes[0], img);
        img.parentNode.removeChild(img);
    }
};
//SelectRange
Winner.Editor.SelectRange = function (editor) {
    this.Editor = editor;
};
Winner.Editor.SelectRange.prototype =
{
    Initialize: function () {
        this.BindEvent();
    },
    BindEvent: function () {//保存range事件
        var self = this;
        this.Editor.Base.BindEvent(this.Editor.Iframe, "mouseout", function (event) {
            if (self.Editor.Iframe.style.diplay != "none") self.Save(event);
        });
        this.Editor.Base.BindEvent(this.Editor.Iframe, "mouseover", function () {
            if (self.Editor.Iframe.style.diplay != "none") self.Remove();
        });

    },
    Save: function () { //得到选中的文本
        this.Editor.Iframe.contentWindow.focus();
        if (this.Range == undefined) {
            this.CreateRange();
            this.SavePosiontion();
            this.SetText();
        }
        if (document.selection)
            this.Range.select();
    },
    ReSelectRange: function () {//重新选择
        if (this.Range == undefined)
            return;
        this.Editor.Iframe.contentWindow.focus();
        this.CreateRange();
        this.SetRangeIndex(this.Editor.EditDocument.body, this.StartIndex, this.EndIndex);
    },
    SetRangeByNode: function (node, startIndex, endIndex) {
        this.Editor.Iframe.contentWindow.focus();
        this.CreateRange();
        this.SetRangeIndex(node, startIndex, endIndex);
        this.SavePosiontion();
        this.SetText();
    },
    SetRangeIndex: function (node, startIndex, endIndex) {
        if (document.selection)
            this.SetRangeIndexBySelection(node, startIndex, endIndex);
        else
            this.SetRangeIndexByGetSelection(node, startIndex, endIndex);
    },
    SetRangeIndexBySelection: function (node, startIndex, endIndex) {
        if (node.nodeName != "#text")
            this.Range.moveToElementText(node);
        else
            this.Range.moveToElementText(node.parentNode);
        this.MoveRangeBySelection(this.Range, startIndex, endIndex);
    },
    SetRangeIndexByGetSelection: function (node, startIndex, endIndex) {
        try {
            this.Range.setStart(node, startIndex);
            this.Range.setEnd(node, endIndex);
        } catch (e) {

        }
    },
    CreateRange: function () {//保存Range
        if (document.selection)
            this.CreateRangeBySelection();
        else
            this.CreateRangeByGetSelection();
    },
    SetText: function () {
        if (this.Range == undefined)
            return;
        this.Text = document.selection ? this.Range.text : this.Range.toString();
    },
    CreateRangeBySelection: function () {
        this.Range = this.Editor.EditDocument.selection.createRange();
    },
    CreateRangeByGetSelection: function () {//保存FF下的range
        try {
            if (this.Editor.EditWindow.getSelection != undefined && this.Editor.EditWindow.getSelection().getRangeAt != undefined)
                this.Range = this.Editor.EditWindow.getSelection().getRangeAt(0);
            else { // Safari!
                var range = this.Editor.EditDocument.createRange();
                if (this.Editor.EditWindow.getSelection() != undefined) {
                    range.setStart(this.Editor.EditWindow.getSelection().anchorNode, this.Editor.EditWindow.getSelection().anchorOffset);
                    range.setEnd(this.Editor.EditWindow.getSelection().focusNode, this.Editor.EditWindow.getSelection().focusOffset);
                }
                this.Range = range;
            }
        } catch (e) {

        }
    },
    SavePosiontion: function () {//保存坐标
        this.EndIndex = this.GetAllHtmlLength();
        this.StartIndex = this.EndIndex - this.GetSelectHtmlLength();
    },
    GetAllHtmlLength: function () {//得到从文档开始到结束去除结尾标签的HTML长度
        var html = document.selection ? this.GetStartRangeHtmlBySelection() : this.GetStartRangeHtmlByGetSelection();
        return this.GetFilterHtml(html).length;
    },
    GetSelectHtmlLength: function () {//得到选中的HTML去除开始标签的HTML长度
        var html = document.selection ? this.GetSelectRangeHtmlBySelection() : this.GetSelectRangeHtmlByGetSelection();
        html = this.GetFilterHtml(html);
        var reg = /^(<.*?>\s{0,}){0,}/gi;
        var matchString = html.replace(reg, "");
        return matchString.length;
    },
    GetFilterHtml: function (html) {
        var reg = /(<\/{0,1}\w*?\/{0,1}>\s{0,}){0,}$/gi;
        var matchString = html.replace(reg, "");
        matchString = this.FilterCharacter(matchString);
        return matchString;
    },
    GetStartRangeHtmlBySelection: function () {//IE下保存坐标
        var range = this.Range.duplicate();
        range.moveToElementText(this.Editor.EditDocument.body);
        range.setEndPoint('EndToEnd', this.Range);
        return range.htmlText;
    },
    GetStartRangeHtmlByGetSelection: function () {//保存非IE下坐标
        if (this.Range == undefined || this.Range == null)
            return "";
        var range = this.Range.cloneRange();
        range.selectNodeContents(this.Editor.EditDocument.body);
        range.setEnd(this.Range.endContainer, this.Range.endOffset);
        var container = document.createElement('div');
        container.appendChild(range.cloneContents());
        return container.innerHTML;
    },
    GetSelectRangeHtmlBySelection: function () {//IE下保存坐标
        return this.Range.htmlText;
    },
    GetSelectRangeHtmlByGetSelection: function () {//保存非IE下坐标
        if (this.Range == undefined || this.Range == null)
            return "";
        var container = document.createElement('div');
        container.appendChild(this.Range.cloneContents());
        return container.innerHTML;
    },
    SetInputSelectRange: function (obj, startIndex, endIndex) {//设置自定义选中
        obj.value = this.FilterCharacter(obj.value);
        if (document.selection)
            this.SetInputRangeRegionBySelection(obj, startIndex, endIndex);
        else
            this.SetInputRangeRegionByGetSelection(obj, startIndex, endIndex);
    },
    FilterCharacter: function (value) {
        value = this.Editor.ReplaceAll(value, "\r", "");
        value = this.Editor.ReplaceAll(value, "\n", "");
        return value;
    },
    SetInputRangeRegionBySelection: function (obj, startIndex, endIndex) {
        var range = obj.createTextRange();
        this.MoveRangeBySelection(range, startIndex, endIndex);
    },
    SetInputRangeRegionByGetSelection: function (obj, startIndex, endIndex) {
        obj.setSelectionRange(startIndex, endIndex);
    },
    MoveRangeBySelection: function (range, startIndex, endIndex) {
        range.collapse(true);
        range.moveStart("character", startIndex);
        range.moveEnd("character", endIndex - startIndex);
        range.select();
    },
    Remove: function () {//移除SelectRange
        this.Range = undefined;
        this.Text = undefined;
        this.StartIndex = undefined;
        this.EndIndex = undefined;
    }
};
//Sizer
Winner.Editor.Sizer = function (editor) {
    this.Editor = editor;
};
Winner.Editor.Sizer.prototype =
{
    Initialize: function () {
        this.Left = this.Editor.Base.GetElementLeft(this.Editor.Container);
        this.Top = this.Editor.Base.GetElementTop(this.Editor.Container);
        this.BindEvent();
    },
    BindEvent: function () {//保存range事件
        var self = this;
        this.Editor.Resizer.onmousedown = function () { self.IsResizer = []; };
        this.Editor.Base.BindEvent(document, "mousemove", function (event) { self.ResizeHandler(event); });
        this.Editor.Base.BindEvent(this.Editor.EditDocument, "mousemove", function (event) { self.ResizeHandler(event, false); });
        this.Editor.Base.BindEvent(document, "mouseup", function () { self.IsResizer = undefined; });
        this.Editor.Base.BindEvent(this.Editor.EditDocument, "mouseup", function () { self.IsResizer = undefined; });
    },
    ResizeHandler: function (event) {//拖动编辑器
        if (this.IsResizer == undefined)
            return;
        event = window.event ? window.event : event;
        var x = arguments[1] == false ? event.clientX : event.clientX - this.Left;
        var y = arguments[1] == false ? event.clientY : event.clientY - this.Top;
        this.ResizeContainer(x, y);
    },
    ResizeIframe: function (width, height) {//调整大小
        this.Editor.Iframe.style.width = width + "px";
        this.Editor.Iframe.style.height = height - this.Editor.ToolBar.clientHeight - this.Editor.Bottom.clientHeight + "px";
        this.Editor.Input.style.width = width + "px";
        this.Editor.Input.style.height = height - this.Editor.ToolBar.clientHeight - this.Editor.Bottom.clientHeight + "px";
    },
    ResizeContainer: function (width, height) {//调整编辑
        width = this.Editor.Width > width ? this.Editor.Width : width;
        height = this.Editor.Height > height ? this.Editor.Height : height;
        this.Editor.Container.style.width = width + "px";
        this.Editor.Container.style.height = height + "px";
        this.ResizeIframe(width, height);
    }
};
//TableMenu
Winner.Editor.TableMenu = function (tableDialog, languages) {
    this.TableDialog = tableDialog;
    this.Container = document.createElement('div');
    this.Languages = languages;
    this.Html = "<table >" +
    "<tr><td Instance='Property'>@Property</td></td></tr>" +
    "<tr><td Instance='InsertDownRow'> @InsertDownRow</td></tr>" +
    "<tr><td Instance='InsertUpRow'> @InsertUpRow</td></tr>" +
    "<tr><td Instance='InsertLeftColunm'> @InsertLeftColunm</td></tr>" +
    "<tr><td Instance='InsertRightColunm'> @InsertRightColunm</td></tr>" +
    "<tr><td Instance='DeleteRow'> @DeleteRow</td></tr>" +
    "<tr><td Instance='DeleteColumn'> @DeleteColumn</td></tr>" +
    "<tr><td Instance='DeleteTable'> @DeleteTable</td></tr>" +
   "</table>";
    this.Html = this.TableDialog.Dialog.Editor.ReplaceLanguage(this.Html, this.Languages.Names);

};
Winner.Editor.TableMenu.prototype =
{
    Initialize: function () { //初始化函数
        this.CreateContainer();
        this.TableDialog.Dialog.Editor.LoadInstances(this, this.Container);
        this.BindEvent();
    },
    CreateContainer: function () {//设置容器样式
        this.Container.innerHTML = this.Html;
        this.Container.className = "tablemenu";
        this.Container.style.position = "absolute";
        this.Container.style.display = "none";
        this.TableDialog.Dialog.Editor.Container.appendChild(this.Container);
    },
    BindEvent: function () {//绑定事件
        this.BindTrEvent();
        this.BindHandlerEvent();
        this.BindCloseMenuEvent();
    },
    BindCloseMenuEvent: function () {// 绑定点击事件
        this.CloseMenuHanlder(this.TableDialog.Dialog.Editor.EditDocument, "click");
        this.CloseMenuHanlder(this.TableDialog.Dialog.Editor.EditDocument, "contextmenu");
        this.CloseMenuHanlder(this.TableDialog.Dialog.Editor.EditDocument, "keyup");
        this.CloseMenuHanlder(this.TableDialog.Dialog.Editor.Container, "click");
        this.CloseMenuHanlder(this.TableDialog.Dialog.Editor.Container, "contextmenu");
    },
    CloseMenuHanlder: function (obj, eventName) {//关闭事件
        var self = this;
        this.TableDialog.Dialog.Editor.Base.BindEvent(obj, eventName, function () {
            self.CloseMenu();
        });
    },
    BindTrEvent: function () {//绑定输入事件
        var trs = this.Container.getElementsByTagName('tr');
        for (var i = 0; i < trs.length; i++) {
            this.TrMouseHanlder(trs[i]);
        }
    },
    BindHandlerEvent: function () {//绑定事件
        this.BindPropertyEvent();
        this.BindInsertDownRowEvent();
        this.BindInsertUpRowEvent();
        this.BindInsertLeftColunmEvent();
        this.BindInsertRightColunmEvent();
        this.BindDeleteRowEvent();
        this.BindDeleteColumnEvent();
        this.BindDeleteTableEvent();
    },
    BindPropertyEvent: function () {//绑定表格属性
        var self = this;
        this.Property.onclick = function () {
            self.PropertyHandler();
        };
    },
    BindInsertDownRowEvent: function () {//绑定插入下方行
        var self = this;
        this.InsertDownRow.onclick = function () {
            self.InsertDownRowHandler();
        };
    },
    BindInsertUpRowEvent: function () {//绑定插入上方行
        var self = this;
        this.InsertUpRow.onclick = function () {
            self.InsertUpRowHandler();
        };
    },
    BindInsertLeftColunmEvent: function () {//绑定左侧列
        var self = this;
        this.InsertLeftColunm.onclick = function () {
            self.InsertLeftColunmHandler();
        };
    },
    BindInsertRightColunmEvent: function () {//绑定右侧列
        var self = this;
        this.InsertRightColunm.onclick = function () {
            self.InsertRightColunmHandler();
        };
    },
    BindDeleteRowEvent: function () {//绑定删除行
        var self = this;
        this.DeleteRow.onclick = function () {
            self.DeleteRowHandler();
        };
    },
    BindDeleteColumnEvent: function () {//绑定删除列
        var self = this;
        this.DeleteColumn.onclick = function () {
            self.DeleteColumnHandler();
        };
    },
    BindDeleteTableEvent: function () {//绑定删除表格
        var self = this;
        this.DeleteTable.onclick = function () {
            self.DeleteTableHandler();
        };
    },
    TrMouseHanlder: function (tr) {//绑定鼠标移动
        tr.onmouseover = function () {
            tr.className = 'over';
        };
        tr.onmouseout = function () {
            tr.className = '';
        };
    },
    PropertyHandler: function () {//属性事件
        this.TableDialog.ShowDialog(this.Table);
        this.CloseMenu();
    },
    InsertDownRowHandler: function () {//插入下方行
        var rowIndex = this.RowIndex > this.TableBody.rows.length ? this.TableBody.rows.length : this.RowIndex + 1;
        this.CreateRow(rowIndex);
    },
    InsertUpRowHandler: function () {//插入上方行
        this.CreateRow(this.RowIndex);
    },
    InsertLeftColunmHandler: function () {//插入左侧列
        this.CreateColumn(this.ColumnIndex);
    },
    InsertRightColunmHandler: function () {//插入右侧列
        var columnIndex = this.ColumnIndex > this.TR.cells.length ? this.TR.cells.length : this.ColumnIndex + 1;
        this.CreateColumn(columnIndex);
    },
    DeleteRowHandler: function () {//删除行
        this.TableBody.removeChild(this.TR);
        this.CloseMenu();
    },
    DeleteColumnHandler: function () {//删除列
        for (var i = 0; i < this.TableBody.rows.length; i++) {
            var tr = this.TableBody.rows[i];
            tr.removeChild(tr.cells[this.ColumnIndex]);
        }
        this.CloseMenu();
    },
    DeleteTableHandler: function () {//删除表格
        this.Table.parentNode.removeChild(this.Table);
    },
    CreateRow: function (index) {//创建行
        var tr = this.TableBody.insertRow(index);
        for (var i = 0; i < this.TR.cells.length; i++) {
            var td = tr.insertCell(i);
            this.TableDialog.Dialog.Editor.ContentEvent.TableContextmenuHandler(td);
        }
        this.CloseMenu();
    },
    CreateColumn: function (index) {
        for (var i = 0; i < this.TableBody.rows.length; i++) {
            var td = this.TableBody.rows[i].insertCell(index);
            this.TableDialog.Dialog.Editor.ContentEvent.TableContextmenuHandler(td);
        }
        this.CloseMenu();
    },
    ShowMenu: function (td, event) {//显示
        this.SetCurrentTableInfo(td);
        this.Initialize();
        this.SetContainerPosition(event);
        this.Container.style.display = "";
    },
    CloseMenu: function () {
        this.Container.style.display = "none";
    },
    SetCurrentTableInfo: function (td) {//设置当前表格
        this.TD = td;
        this.TR = td.parentNode;
        this.TableBody = td.parentNode.parentNode;
        this.Table = this.TableBody.parentNode;
        this.SetCurrentRowIndex();
        this.SetCurrentColumnIndex();
    },
    SetCurrentRowIndex: function () {//设置当前行
        for (var i = 0; i < this.TableBody.rows.length; i++) {
            if (this.TableBody.rows[i] == this.TR) {
                this.RowIndex = i;
                return;
            }
        }
        this.RowIndex = this.TableBody.rows.length;
    },
    SetCurrentColumnIndex: function () {//设置当前列
        for (var i = 0; i < this.TR.cells.length; i++) {
            if (this.TR.cells[i] == this.TD) {
                this.ColumnIndex = i;
                return;
            }
        }
        this.ColumnIndex = this.TR.cells.length;
    },
    SetContainerPosition: function (event) {//设置显示位置
        event = window.event ? window.event : event;
        this.Container.style.left = event.clientX + this.TableDialog.Dialog.Editor.Base.GetElementLeft(this.TableDialog.Dialog.Editor.Container) + "px";
        this.Container.style.top = event.clientY + this.TableDialog.Dialog.Editor.Base.GetElementTop(this.TableDialog.Dialog.Editor.Container) + this.TableDialog.Dialog.Editor.ToolBar.clientHeight + "px";
    }
};
//ToolCreator
Winner.Editor.ToolCreator = function (editor) {
    this.Editor = editor;
};
Winner.Editor.ToolCreator.prototype =
{
    LoadToolbar: function (div) {//加载工具栏
        if (this.Editor.Tools == null || this.Editor.Tools.length == 0)
            return;
        div.innerHTML = "";
        for (var i = 0; i < this.Editor.ToolMenus.length; i++) {
            this.LoadTools(div, this.Editor.ToolMenus[i]);
        }
    },
    LoadTools: function (div, names) {//加载工具箱
        var container = this.GetToolsContainer();
        for (var i = 0; i < names.length; i++) {
            this.CreateTool(container, names[i]);
        }
        div.appendChild(container);
    },
    GetToolsContainer: function () {//得到工具箱容器
        var container = document.createElement("span");
        container.className = "tools";
        return container;
    },
    CreateTool: function (container, name) {//创建工具箱
        if (name == "-") {
            this.AddToolSeparate(container);
        }
        else {
            this.AddTool(container, name);
        }
    },
    AddToolSeparate: function (container) {//添加分隔
        var br = document.createElement("span");
        br.className = "br";
        br.innerHTML = "|";
        container.appendChild(br);
    },
    AddTool: function (container, name) {//添加工具
        var tool = this.Editor.Tools[name];
        if (tool != null) {
            container.appendChild(this.GetTool(name, tool));
        }
    },
    GetTool: function (name, info) {//得到工具按钮
        if (info == null)
            return null;
        info.Object = document.createElement("a");
        this.SetTool(name, info);
        this.BindToolClickEvent(info);
        this.SetToolDefaultAttibute(info);
        return info.Object;
    },
    SetToolDefaultAttibute: function (info) {
        if (info.IsEnable != undefined && info.IsEnable != null) {
            this.Editor.ToolMethod.SetToolEnableAttibute(info.Object, info.IsEnable);
        }
    },
    SetToolSelectClass: function (info) {//选择tool的样式
        if (info.HasSelectClass == null || info.HasSelectClass) {
            info.Object.className = info.Object.className == "aselect" ? "" : "aselect";
        }
        if (info.MutexSelectClasses != null && info.MutexSelectClasses.length > 0) {
            for (var i = 0; i < info.MutexSelectClasses.length; i++) {
                this.Editor.Tools[info.MutexSelectClasses[i]].Object.className = "";
            }
        }
    },
    GetToolEnableAttibute: function (tool) {//得到控件的Enable
        var value = this.Editor.Base.GetAttribute(tool, "IsEnable");
        if (value != "false")
            return true;
        return false;
    },
    SetTool: function (name, info) {//设置工具
        var tip = this.Editor.Languages[name];
        info.Object.href = "javascript:void('" + tip + "')";
        info.Object.title = tip;
        info.Object.innerHTML = info.Html == null ? "<span class='" + name.toLowerCase() + "'>&nbsp;</span>" : info.Html.replace("{$name}", tip);
    },
    BindToolClickEvent: function (info) {//绑定点击事件
        if (info.Click == null)
            return;
        var self = this;
        this.Editor.Base.BindEvent(info.Object, "click", function () {
            if (self.GetToolEnableAttibute(info.Object)) {
                self.SetToolSelectClass(info);
                eval(self.Editor.ClassName + "." + info.Click);
            }
        });
    }
};
//ToolMethod
Winner.Editor.ToolMethod = function (editor) {
    this.Editor = editor;
};
Winner.Editor.ToolMethod.prototype =
{
    SetToolsEnableAttibute: function (names, value) {//设置工具栏显示
        for (var i = 0; i < this.Editor.ToolMenus.length; i++) {
            for (var k = 0; k < this.Editor.ToolMenus[i].length; k++) {
                if (!this.Editor.Contain(names, this.Editor.ToolMenus[i][k])) {
                    var info = this.Editor.Tools[this.Editor.ToolMenus[i][k]];
                    var val = value && info.IsEnable != undefined && info.IsEnable != null ? info.IsEnable : value;
                    this.SetToolEnableAttibute(info.Object, val);
                }
            }
        }
    },
    SetToolEnableAttibute: function (tool, value) {//设置控件的Enable
        this.Editor.Base.SetAttribute(tool, "IsEnable", value);
        tool.className = value ? "" : "aenable";
    },
    ShowInput: function (names) {//显示Input
        this.Editor.Input.style.display = "";
        this.Editor.Iframe.style.display = "none";
        this.Editor.SelectRange.SetInputSelectRange(this.Editor.Input, this.Editor.SelectRange.StartIndex, this.Editor.SelectRange.EndIndex);
        this.Editor.Input.focus();
        this.SetToolsEnableAttibute(names, false);
    },
    ShowIframe: function (names) {//显示Iframe
        this.Editor.Input.style.display = "none";
        this.Editor.Iframe.style.display = "";
        this.Editor.SelectRange.ReSelectRange();
        this.Editor.Iframe.focus();
        this.SetToolsEnableAttibute(names, true);
    },
    //共有方法
    ShowSource: function (names) {//显示源码
        if (this.Editor.Input.style.display == "none") {
            this.ShowInput(names);
        }
        else {
            this.ShowIframe(names);
            this.Editor.ContentEvent.BindEvent();
        }
    },
    NewPage: function () {//新建
        this.Editor.EditDocument.body.innerHTML = "";
        this.Editor.Input.value = "";
    },
    Preview: function () { //预览
        if (this.Editor.Input.style.display == "") {
            this.Editor.EditDocument.body.innerHTML = this.Editor.Input.value;
        }
        window.open(this.Editor.Path + this.Editor.PreviewFile + "?editor=" + this.Editor.ClassName);
    },
    FullScreen: function () {//全屏
        if (this.IsFullScreen == undefined) {
            this.SetFullScreen();
        }
        else {
            this.SetUnFullScreen();
        }
    },
    SetFullScreen: function () {//设置全屏
        this.IsFullScreen = [];
        this.Editor.Container.style.position = "absolute";
        this.Editor.Container.style.left = "0px";
        this.Editor.Container.style.top = "0px";
        this.Editor.Sizer.ResizeContainer(document.body.clientWidth, document.body.clientHeight);
    },
    SetUnFullScreen: function () {//收回全屏
        this.IsFullScreen = undefined;
        this.Editor.Container.style.position = "";
        this.Editor.Sizer.ResizeContainer(this.Editor.Width, this.Editor.Height);
    },
    Print: function () { //打印
        this.Editor.EditWindow.print();
    },
    SelectAll: function () {//选中
        if (this.Editor.Input.style.display == "") {
            this.Editor.Input.select();
        }
        else {
            this.Editor.ExecuteCommand('SelectAll', false, '');
        }
    }
};
//UndoRedo
Winner.Editor.UndoRedo = function (editor) {
    this.Editor = editor;
    this.Dos = [];
    this.Index = 0;
    this.Times = 0;
    this.AutoSave = false;
};
Winner.Editor.UndoRedo.prototype =
{
    Initialize: function () {
        this.Dos.push("");
        this.AutoSaveHandler();
        this.BindEvent();
    },
    BindEvent: function () {
        var self = this;
        this.Editor.Base.BindEvent(this.Editor.EditDocument.body, "keyup", function (event) {
            self.KeyupHandler(event);
        });
        this.Editor.Base.BindEvent(this.Editor.EditDocument.body, "paste", function () {
            var func = function () { self.AddDo(); };
            setTimeout(func, 100);
        });
    },
    KeyupHandler: function (event) {//添加
        event = window.event ? window.event : event;
        if (event.ctrlKey && event.keyCode == 90)
            this.Undo();
        else if (event.ctrlKey && event.keyCode == 89)
            this.Redo();
        else if (this.CheckKeyup(event))
            this.AddDo();
    },
    AutoSaveHandler: function () {//15秒自动保存
        var self = this;
        var func = function () {
            if (self.AutoSave == true && self.GetNowTimes() >= self.Times + 15)
                self.AddDo();
            setTimeout(func, 15000);
        };
        setTimeout(func, 15000);
    },
    CheckKeyup: function (event) {//检查是否要保存

        if (event.keyCode == 13 || event.keyCode == 32) {
            return true;
        }
        return false;
    },
    AddDo: function () {
        if (this.Index == 0 || this.Dos[this.Index] != this.Editor.EditDocument.body.innerHTML) {
            this.ResetDos();
            this.Dos.push(this.Editor.EditDocument.body.innerHTML);
            if (this.Dos.length > 10)
                this.Dos.splice(0, 1);
            this.Index = this.Dos.length - 1;
            this.ResetToolAndTimes();
            this.AutoSave = true;
        }
    },
    ResetDos: function () {//重置数组
        if (this.Index != this.Dos.length - 1) {
            this.Dos.splice(this.Index + 1);
        }
    },
    GetNowTimes: function () {
        var now = new Date();
        var minutes = now.getMinutes();
        var seconds = now.getSeconds();
        return minutes * 60 + seconds;
    },
    Undo: function () {//撤销
        if (this.Index <= 0)
            return;
        this.AutoSave = false;
        if (this.Index == this.Dos.length - 1) {
            this.AddDo();
        }
        this.Index--;
        this.Editor.EditDocument.body.innerHTML = this.Dos[this.Index];
        this.ResetToolAndTimes();
    },
    Redo: function () { //重做
        if (this.Index >= this.Dos.length - 1)
            return;
        this.AutoSave = false;
        this.Index++;
        this.Editor.EditDocument.body.innerHTML = this.Dos[this.Index];
        this.ResetToolAndTimes();
    },
    ResetToolAndTimes: function () {
        var redoValue = this.Index == this.Dos.length - 1 ? false : true;
        var undoValue = this.Index == 0 ? false : true;
        this.Editor.ToolMethod.SetToolEnableAttibute(this.Editor.Tools["Redo"].Object, redoValue);
        this.Editor.ToolMethod.SetToolEnableAttibute(this.Editor.Tools["Undo"].Object, undoValue);
        this.Times = this.GetNowTimes();
    }
};
//ContentEvent
Winner.Editor.ContentEvent = function (editor) {
    this.Editor = editor;
};
Winner.Editor.ContentEvent.prototype =
{
    Initialize: function () {
        this.BindIframePaste();
    },
    BindIframePaste: function () {//绑定黏贴事件
        var self = this;
        this.Editor.Base.BindEvent(this.Editor.EditDocument.body, "paste", function () {
            var func = function () {
                self.BindEvent();
            };
            setTimeout(func, 100);
        });
    },
    BindEvent: function () {//绑定Iframe的改变的的事件
        this.Document = this.Editor.EditDocument;
        this.BindLinkEvent();
        this.BindImageEvent();
        this.BindTableEvent();
    },
    BindTableEvent: function () {//绑定表格右键事件
        var objs = this.Document.getElementsByTagName('td');
        var i;
        for (i = 0; i < objs.length; i++) {
            this.TableContextmenuHandler(objs[i]);
        }
        objs = this.Document.getElementsByTagName('th');
        for (i = 0; i < objs.length; i++) {
            this.TableContextmenuHandler(objs[i]);
        }
    },
    TableContextmenuHandler: function (td) {//绑定表格右键事件
        var self = this;
        this.Editor.Base.BindEvent(td, "contextmenu", function (event) {
            self.Editor.TableMenu.ShowMenu(td, event);
            return self.Editor.Base.CancelEventUp(event);
        });
    },
    BindImageEvent: function () {//绑定图片事件
        var objs = this.Document.getElementsByTagName('img');
        for (var i = 0; i < objs.length; i++) {
            this.ImageDoubleClickHandler(objs[i]);
        }
    },
    ImageDoubleClickHandler: function (obj) {//绑定Iframe的图片的Click事件
        var self = this;
        this.Editor.Base.BindEvent(obj, "dblclick", function () {
            self.SelectImageShowWay(obj);
        });
    },
    SelectImageShowWay: function (obj) {//是否是真实显示图片
        if (this.Editor.Base.GetAttribute(obj, "ObjectValue") != null) {
            var div = document.createElement("div");
            div.innerHTML = decodeURI(this.Editor.Base.GetAttribute(obj, "ObjectValue"));
            this.ShowImageReplaceValueObject(div.childNodes[0]);
        }
        else {
            this.Editor.ImageDialog.ShowDialog(obj);
        }
    },
    ShowImageReplaceValueObject: function (obj) {//显示对话框
        switch (obj.nodeName) {
            case "A": this.Editor.LinkDialog.ShowDialog(obj); break;
            case "OBJECT": this.Editor.FlashDialog.ShowDialog(obj); break;
        }
    },
    BindLinkEvent: function () {//绑定Iframe的a的事件
        var objs = this.Document.getElementsByTagName('a');
        for (var i = 0; i < objs.length; i++) {
            this.LinkDoubleClickHandler(objs[i]);
        }
    },
    LinkDoubleClickHandler: function (obj) {//绑定Iframe的a的Click事件
        var self = this;
        this.Editor.Base.BindEvent(obj, "dblclick", function () {
            self.Editor.LinkDialog.ShowDialog(obj);
        });
    }

};
