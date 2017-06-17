

function SetMessage(messageUrl) {
    var minites = 5;
    //var setWorkflowMessage = function (func) {
    //    try {
    //        function buildInfos(infos) {
    //            var result = [];
    //            if (infos == null || infos.length == 0) return result;
    //            for (var i = 0; i < infos.length; i++) {
    //                var title = infos[i].FlowName + "-" + infos[i].StatusName + "-" + infos[i].LevelName +
    //                    "-" + infos[i].HandleUserRealName;
    //                var detail = "<div style='width:100%;text-align:center; height:25px;'>" + infos[i].Title + "</div>" + infos[i].Detail; //
    //                detail = detail + "<div style='width:100%; height:25px;'>";
    //                detail = detail
    //                    + getWorkflowUrl(infos[i].AddUrl, 0, "新增")
    //                    + getWorkflowUrl(infos[i].EditUrl, infos[i].DataId, "编辑")
    //                    + getWorkflowUrl(infos[i].HandleUrl, infos[i].DataId, "处理")
    //                    + getWorkflowUrl(infos[i].DetailUrl, infos[i].DataId, "详情");
    //                detail = detail + "</div>";
    //                result.push({ Id: infos[i].Id, Title: title, Detail: detail });
    //            }
    //            return result;
    //        }
    //        var url = messageUrl+"/Ajax/Workflow/Message.aspx?times=" + minites + "&rand=" + (new Date()).valueOf() + "&jsoncallback=theWorkflowFunctionName";
    //        window.theWorkflowFunctionName = function(data) {
    //            var infos = buildInfos(data);
    //            func(infos);
    //        };
    //        $.ajax(url, {
    //            data: {
    //            },
    //            dataType: 'jsonp',
    //            crossDomain: true,
    //            success: function (data) {
    //                if (data && data.resultcode == '200') {
    //                    console.log(data.result.today);
    //                }
    //            },
    //            error: function (xhr, textStatus, errorThrown) {

    //                }
    //        });

    //    } catch(e) {
    //    }
    //};


    //function getWorkflowUrl(url, dataId, name) {
    //    if (name == "新增") {
    //        return "<a href='" + url + "' target='_blank'>新增</a>&nbsp;&nbsp;";
    //    } else {
    //        return "<a href='" + url + "?id=" + dataId + "' target='_blank'>" + name + "</a>&nbsp;&nbsp;";
    //    }
    //}

    //var setNoticeMessage = function (func) {
    //    try {
    //        function buildInfos(infos) {
    //            var result = [];
    //            if (infos == null || infos.length == 0) return result;
    //            for (var i = 0; i < infos.length; i++) {
    //                var title = infos[i].Title;
    //                var detail = infos[i].Detail + "&nbsp;&nbsp;<a href='/Desktop/Notice/Detail.aspx?id=" + infos[i].Id + "'target='_blank'>详情</a>";
    //                result.push({ Title: title, Detail: detail });
    //            }
    //            return result;
    //        }
    //        var url = messageUrl + "/Ajax/Information/content.aspx?tag=ERP_Message_Notice&times=" + minites+"&jsoncallback=theNoticeFunctionName";
    //        window.theNoticeFunctionName = function (data) {
    //            var infos = buildInfos(data);
    //            func(infos);
    //        };
    //        $.ajax(url, {
    //            data: {
    //            },
    //            dataType: 'jsonp',
    //            crossDomain: true,
    //            success: function (data) {
    //                if (data && data.resultcode == '200') {
    //                    console.log(data.result.today);
    //                }
    //            },
    //            error: function (xhr, textStatus, errorThrown) {

    //            }
    //        });
       
           
    //    } catch(e) {

    //    } 
    //};

    var mess = new Winner.Message();
    var colseHanlde = function (infoHandle, info) {
        $.ajax({
            type: 'GET',
            url: messageUrl+"/Ajax/Workflow/Message.aspx?Id=" + info.Id,
            async: false,
            data: "",
            dataType: "jsonp",
            success: function () {
            }
        });
    };
    var showHanlde = function (infoHandle, info) {
        $(mess.Container).find("a").click(function () {
            mess.Close(infoHandle, info);
        });
    };
    mess.Initialize();
    minites = minites * 1000;
    //mess.InfosHandles.push({ Name: "WorkflowMessage", Func: setWorkflowMessage, CloseFunc: colseHanlde, ShowFunc: showHanlde, Message: "【您有工作流消息】", InvokeTime: 1000 });
    //mess.InfosHandles.push({ Name: "NoticeMessage", Func: setNoticeMessage, CloseFunc: null, Message: "【您有系统通知】", InvokeTime: minites });
    //mess.Start();
    mess.IsRun=false;
}

