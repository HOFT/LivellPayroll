//Common Part Function
function ErrorBox(message) {
    $.smallBox({
        title: "Error!",
        content: "<i class='fa fa-warning'></i> <i>" + message + "</i>",
        color: "#FF0000",
        iconSmall: "fa fa-check bounce animated",
        timeout: 4000
    });
};
function SuccessBox(message) {
    $.smallBox({
        title: "Success！",
        content: "<i class='fa fa-clock-o'></i> <i>" + message + "</i>",
        color: "#5F895F",
        iconSmall: "fa fa-check bounce animated",
        timeout: 4000
    });
};
//function AjaxSave(data, RouteURL, SuccessInfo) {
//    var jsonDate = $.parseJSON(data)
//    if (jsonDate.status == "success") {
//        SuccessBox(SuccessInfo);
//        setTimeout(function () {
//            window.location.href = RouteURL
//        }, 800);
//        //window.location.href = "/Employee/EmployeeList"
//    } else {
//        ErrorBox(jsonDate.message);
//    }
//};

/*
 * Content:操作提示
 * Content:主键
 * RouteURL:Action路由地址
 * Message:操作后提示信息
 * Type:OpTable or OpGen  table操作还是普通操作
 */
function AjaxDelete(Content, PrimaryKey, RouteURL, Message, Type) {
    $.SmartMessageBox({
        title: "Confirmation!",
        content: Content,
        buttons: '[No][Yes]'
    }, function (ButtonPressed) {
        if (ButtonPressed === "Yes") {
            $.post(RouteURL, { "Id": PrimaryKey }, function (data) {
                if (data.code == 1) {
                    SuccessBox(Message);
                    if (Type == "OpTable") {
                        $("[id='" + PrimaryKey + "']").parent().parent().remove();
                    } else {
                         window.location.reload();
                    }
                } else {
                    ErrorBox(data.message);
                }
            })
        }
    });
}

var step_pane3 = $(".step-pane").eq(2).children();
var step_pane4 = $(".step-pane").eq(3).children();

//DuallistBoxAppend
function DuallistBoxAppend(con) {
    initializeDuallistbox.append(con);
    initializeDuallistbox.bootstrapDualListbox('refresh');
}

// 对Date的扩展，将 Date 转化为指定格式的String
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符， 
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字) 
// 例子： 
// (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423 
// (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18 
Date.prototype.Format = function (fmt) { //author: meizz 
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "h+": this.getHours(), //小时 
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}
// eg:pad(100, 4);  // 输出：0100  
function pad(num, n) {
    var len = num.toString().length;
    while (len < n) {
        num = "0" + num;
        len++;
    }
    return num;
}
//绝对时间转换本地时间
function UtcToLocal(d) {
    return Date.UTC(d.getFullYear()
        , d.getMonth()
        , d.getDate()
        , d.getHours()
        , d.getMinutes()
        , d.getSeconds()
        , d.getMilliseconds());
}

function LocalToUtc(d) {
    localTime = d.getTime();
    localOffset = d.getTimezoneOffset() * 60000; //获得当地时间偏移的毫秒数 
    utc = localTime + localOffset; //utc即GMT时间 
    return utc
}

//Date.prototype.format = function (fmt) {
//    var o = {
//        "M+": this.getMonth() + 1,                 //月份 
//        "d+": this.getDate(),                    //日 
//        "h+": this.getHours(),                   //小时 
//        "m+": this.getMinutes(),                 //分 
//        "s+": this.getSeconds(),                 //秒 
//        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
//        "S": this.getMilliseconds()             //毫秒 
//    };
//    if (/(y+)/.test(fmt)) {
//        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
//    }
//    for (var k in o) {
//        if (new RegExp("(" + k + ")").test(fmt)) {
//            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
//        }
//    }
//    return fmt;
//}



