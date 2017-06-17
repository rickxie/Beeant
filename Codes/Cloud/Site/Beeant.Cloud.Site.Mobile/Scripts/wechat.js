
function initWx(config) {
    wx.config({
        debug: false, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
        appId: config.appId, // 必填，公众号的唯一标识
        timestamp: config.timestamp, // 必填，生成签名的时间戳
        nonceStr: config.nonceStr, // 必填，生成签名的随机串
        signature: config.signature,// 必填，签名，见附录1
        jsApiList: [
            'onMenuShareTimeline',
'onMenuShareAppMessage',
'onMenuShareQQ',
'onMenuShareWeibo',
'onMenuShareQZone',
'startRecord',
'stopRecord',
'onVoiceRecordEnd',
'playVoice',
'pauseVoice',
'stopVoice ',
'onVoicePlayEnd',
'uploadVoice',
'downloadVoice',
'chooseImage',
'previewImage',
'uploadImage',
'downloadImage',
'translateVoice',
'getNetworkType',
'openLocation',
'getLocation',
'hideOptionMenu',
'showOptionMenu',
'hideMenuItems',
'showMenuItems',
'hideAllNonBaseMenuItem',
'showAllNonBaseMenuItem',
'closeWindow',
'scanQRCode',
'chooseWXPay',
'openProductSpecificView',
'addCard',
'chooseCard',
'openCard'] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
    });
    wx.ready(function () {
        function getUrl(sender, others) {
            if (sender == undefined || sender == null)
                return window.location.href;
            var url = config.detailUrl + "?ps[0].id=" + $(sender).attr("dataid");
            if ($(sender).attr("password") != undefined) {
                url += "&ps[0].password=" + $(sender).attr("password");
            }
            if (others != undefined) {
                for (var i = 0; i < others.length; i++) {
                    url += "&ps[" + (i + 1) + "].id=" + $(others[i]).attr("dataid");
                    if ($(others[i]).attr("password") != undefined) {
                        url += "&ps[" + (i + 1) + "].password=" + $(others[i]).attr("password");
                    }
                }
            } 
            return url;
        }
        function getTitle(sender) {
            if (sender==undefined || sender == null)
                return window.document.title;
            return $(sender).attr("DataName");
        }
        function getimgUrl(sender) {
            if ((sender == undefined || sender == null) && $(document).find("img").length > 0)
                return $(document).find("img")[0].src;
            return $(sender).find("img[data-original]").attr("data-original");
        }
        function getDesc(sender) {
            if ((sender == undefined || sender == null) )
                return window.document.title;;
            return $(sender).attr("DataDescription");
        }
        window.SetWxEvent = function (sender,others) {
            var url = getUrl(sender, others);
            var title = getTitle(sender);
            var imgUrl = getimgUrl(sender);
            var desc = getDesc(sender); 
            wx.onMenuShareTimeline({
                title: title, // 分享标题
                link: url, // 分享链接
                imgUrl: imgUrl, // 分享图标
                success: function () {
                    // 用户确认分享后执行的回调函数
                },
                cancel: function () {
                    // 用户取消分享后执行的回调函数
                }
            }); 
            wx.onMenuShareAppMessage({
                title: title, // 分享标题
                desc: desc, // 分享描述
                link: url, // 分享链接
                imgUrl: imgUrl, // 分享图标
                type: '', // 分享类型,music、video或link，不填默认为link
                dataUrl: '', // 如果type是music或video，则要提供数据链接，默认为空
                success: function () {
                    // 用户确认分享后执行的回调函数
                },
                cancel: function () {
                    // 用户取消分享后执行的回调函数
                }
            }); 
            wx.onMenuShareQQ({
                title: title, // 分享标题
                desc: desc, // 分享描述
                link: url, // 分享链接
                imgUrl: imgUrl, // 分享图标
                success: function () {
                    // 用户确认分享后执行的回调函数
                },
                cancel: function () {
                    // 用户取消分享后执行的回调函数
                }
            });
            wx.onMenuShareWeibo({
                title: title, // 分享标题
                desc: desc, // 分享描述
                link: url, // 分享链接
                imgUrl: imgUrl, // 分享图标
                success: function () {
                    // 用户确认分享后执行的回调函数
                },
                cancel: function () {
                    // 用户取消分享后执行的回调函数
                }
            });
            wx.onMenuShareQZone({
                title: title, // 分享标题
                desc: desc, // 分享描述
                link: url, // 分享链接
                imgUrl: imgUrl, // 分享图标
                success: function () {
                    // 用户确认分享后执行的回调函数
                },
                cancel: function () {
                    // 用户取消分享后执行的回调函数
                }
            });
        }
    });
}


 