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
function WarningBox(message) {
    $.smallBox({
        title: "Warning!",
        content: "<i class='fa fa-warning'></i> <i>" + message + "</i>",
        color: "#DAA520",
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

function showTheTime(now) {
    //var now = new Date();

    return showTheHours(now.getHours()) +
    showZeroFilled(now.getMinutes()) + showZeroFilled(now.getSeconds()) + showAmPm();
    //将几个函数的结果连接起来，构造出要在页面上显示的时间值，然后放入showTime的innerHTML属性。 

    function showTheHours(theHour) { //此函数用来显示小时字段
        if (show24Hour() || (theHour > 0 && theHour < 13)) {
            return theHour;
        }
        //如果用户希望显示为24小时制，或者时间的小时部分大于0小于13，那么直接返回小时值theHour
        if (theHour == 0) {
            return 12;
        }
        return theHour - 12;
        //大于等于13小于24的theHour减去12
    }

    function showZeroFilled(inValue) { //此函数为10以下的加0
        if (inValue > 9) {
            return ":" + inValue;
        }
        return ":0" + inValue;
    }

    function showAmPm() { //此函数在12小时制后面加上AM或PM。如果show24Hour返回为true，它返回空
        if (show24Hour()) {
            return "";
        }
        if (now.getHours() < 12) {
            return " AM";
        }
        return " PM";
    }
    function show24Hour() {//根据用户在页面上选择的单选按钮返回一个值，若选中show24，返回true。
        //return document.getElementById("show24").checked;
        return false;
    }
}

/*!
 *方法：isEastEarthTime
 *判断一个时间是在东半球还是西半球
 *@param 
 *@author  Aaron
 */
function isEastEarthTime() {
    var now = new Date();
    var timeZone = now.getTimezoneOffset();
    if (timeZone < 0) {
        return true;
    }
    else {
        return false;
    }
}

/*!
 *方法：isDayLightTime
 *判断一个时间是否在夏令时
 *@param 
 *@author  Aaron
 */
function isDayLightTime() {
    var now = new Date();
    var start = new Date();
    //得到一年的开始时间
    start.setMonth(0);
    start.setDate(1);
    start.setHours(0);
    start.setMinutes(0);
    start.setSeconds(0);

    var middle = new Date(start.getTime());
    middle.setMonth(6);
    // 如果年始和年中时差相同，则认为此国家没有夏令时
    if ((middle.getTimezoneOffset() - start.getTimezoneOffset()) == 0) {
        return false;
    }

    var margin = 0;
    //判断当前用户在东半球还是西半球
    if (isEastEarthTime()) {
        margin = start.getTimezoneOffset();
    }
    else {
        margin = middle.getTimezoneOffset();
    }
    if (now.getTimezoneOffset() == margin) {
        return true;
    }
    return false;
}

// Extend the default Number object with a formatMoney() method:
// usage: someVar.formatMoney(decimalPlaces, symbol, thousandsSeparator, decimalSeparator)
// defaults: (2, "$", ",", ".")
Number.prototype.formatMoney = function (places, symbol, thousand, decimal) {
    places = !isNaN(places = Math.abs(places)) ? places : 2;
    symbol = symbol !== undefined ? symbol : "$";
    thousand = thousand || ",";
    decimal = decimal || ".";
    var number = this,
        negative = number < 0 ? "-" : "",
        i = parseInt(number = Math.abs(+number || 0).toFixed(places), 10) + "",
        j = (j = i.length) > 3 ? j % 3 : 0;
    return symbol + negative + (j ? i.substr(0, j) + thousand : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + thousand) + (places ? decimal + Math.abs(number - i).toFixed(places).slice(2) : "");
};
//获取地址栏参数
function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
};
//功能：根据用户输入的Email跳转到相应的电子邮箱首页
function gotoEmail($mail) {
    $t = $mail.split('@')[1];
    $t = $t.toLowerCase();
    if ($t == '163.com') {
        return 'mail.163.com';
    } else if ($t == 'vip.163.com') {
        return 'vip.163.com';
    } else if ($t == '126.com') {
        return 'mail.126.com';
    } else if ($t == 'qq.com' || $t == 'vip.qq.com' || $t == 'foxmail.com') {
        return 'mail.qq.com';
    } else if ($t == 'gmail.com') {
        return 'mail.google.com';
    } else if ($t == 'sohu.com') {
        return 'mail.sohu.com';
    } else if ($t == 'tom.com') {
        return 'mail.tom.com';
    } else if ($t == 'vip.sina.com') {
        return 'vip.sina.com';
    } else if ($t == 'sina.com.cn' || $t == 'sina.com') {
        return 'mail.sina.com.cn';
    } else if ($t == 'tom.com') {
        return 'mail.tom.com';
    } else if ($t == 'yahoo.com.cn' || $t == 'yahoo.cn') {
        return 'mail.cn.yahoo.com';
    } else if ($t == 'tom.com') {
        return 'mail.tom.com';
    } else if ($t == 'yeah.net') {
        return 'www.yeah.net';
    } else if ($t == '21cn.com') {
        return 'mail.21cn.com';
    } else if ($t == 'hotmail.com') {
        return 'www.hotmail.com';
    } else if ($t == 'sogou.com') {
        return 'mail.sogou.com';
    } else if ($t == '188.com') {
        return 'www.188.com';
    } else if ($t == '139.com') {
        return 'mail.10086.cn';
    } else if ($t == '189.cn') {
        return 'webmail15.189.cn/webmail';
    } else if ($t == 'wo.com.cn') {
        return 'mail.wo.com.cn/smsmail';
    } else if ($t == '139.com') {
        return 'mail.10086.cn';
    } else {
        return '';
    }
};
$(".btn_actemail").click(function () {
    var uurl = $(".hide_email").val();
    uurl = gotoEmail(uurl);
    if (uurl != "") {
        window.open("http://" + uurl);
        //$(".toopen").attr("href", "http://" + uurl);
        //$(".toopen")[0].click();
    } else {
        alert("Sorry! Do not find the corresponding email address, please log in to check email!");
    }
});

//获得某月的天数　　 
function getMonthDays(paraYear, paraMonth) {
    var monthStartDate = new Date(paraYear, paraMonth-1, 1);
    var monthEndDate = new Date(paraYear, paraMonth, 1);
    var days = (monthEndDate - monthStartDate) / (1000 * 60 * 60 * 24);
    return days;
}
//获得某月的开始日期　　 
function getMonthStartDate(paraYear, paraMonth) {
    var monthStartDate = new Date(paraYear, paraMonth-1, 1);
    return monthStartDate.Format("yyyy-M-d");
};
//获得某月的结束日期　　 
function getMonthEndDate(paraYear, paraMonth) {
    var monthEndDate = new Date(paraYear, paraMonth-1, getMonthDays(paraYear, paraMonth));
    return monthEndDate.Format("yyyy-M-d");
};
//获得某季度的开始日期　　 
function getQuarterStartDate(paraYear, paraSeason) {
    switch (paraSeason) {
        case "1": return paraYear + "-01-01";
        case "2": return paraYear + "-04-01";
        case "3": return paraYear + "-07-01";
        case "4": return paraYear + "-10-01";
    }
};
//获得某季度的结束日期　　 
function getQuarterEndDate(paraYear, paraSeason) {
    switch (paraSeason) {
        case "1": return paraYear + "-03-31";
        case "2": return paraYear + "-06-30";
        case "3": return paraYear + "-09-30";
        case "4": return paraYear + "-12-31";
    }
};

//报表筛选条件
$('.reportSelection-dtpStartDate').val((new Date()).Format("yyyy-MM-dd"));
$('.reportSelection-dtpEndDate').val((new Date()).Format("yyyy-MM-dd"));

$('.reportSelection-ChooseYear').change(function () {
    var year = $(this).val();
    if (year <= 0) {
        $('.reportSelection-Quarter').val("").attr("disabled", true);
        $('.reportSelection-Month').val("").attr("disabled", true);
    } else {
        $('.reportSelection-Quarter').removeAttr("disabled");
        $('.reportSelection-Month').removeAttr("disabled");
    }
});
$('.reportSelection-Month').change(function () {
    $(".reportSelection-Quarter").val("");
    var dtpStartDate = (new Date()).Format("yyyy-MM-dd");
    var dtpEndDate = (new Date()).Format("yyyy-MM-dd");
    var Month = $(this).val();
    var year = $('.reportSelection-ChooseYear').val();
    if (Month > 0) {
        dtpStartDate = getMonthStartDate(year, Month)
        dtpEndDate = getMonthEndDate(year, Month)
    }
    $('.reportSelection-dtpStartDate').val(dtpStartDate);
    $('.reportSelection-dtpEndDate').val(dtpEndDate);
});
$('.reportSelection-Quarter').change(function () {
    $(".reportSelection-Month").val("");
    var dtpStartDate = (new Date()).Format("yyyy-MM-dd");
    var dtpEndDate = (new Date()).Format("yyyy-MM-dd");
    var Quarter = $(this).val();

    var year = $('.reportSelection-ChooseYear').val();
    if (Quarter > 0) {
        dtpStartDate = getQuarterStartDate(year, Quarter)
        dtpEndDate = getQuarterEndDate(year, Quarter)
    }
    $('.reportSelection-dtpStartDate').val(dtpStartDate);
    $('.reportSelection-dtpEndDate').val(dtpEndDate);
});

$('.Export-Excel').click(function () {
    console.log("hha");
    $(window.parent.document).contents().find("#RdlcView")[0].contentWindow.$find('ReportViewer1').exportReport('EXCELOPENXML');
});
$('.Export-PDF').click(function () {
    $(window.parent.document).contents().find("#RdlcView")[0].contentWindow.$find('ReportViewer1').exportReport('PDF');
});
$('.Export-Word').click(function () {
    $(window.parent.document).contents().find("#RdlcView")[0].contentWindow.$find('ReportViewer1').exportReport('WORDOPENXML');
});
$('.Export-Print').click(function () {
    $("#RdlcView").contents().find("div[id^='VisibleReportContentReportViewer1']").print({
        globalStyles: false,
        mediaPrint: false,
        stylesheet: null,
        noPrintSelector: ".no-print",
        iframe: true,
        append: null,
        prepend: null,
        manuallyCopyFormValues: true,
        deferred: $.Deferred()
    });
});
//高亮闪烁 eg: shake($('#txtHrsRegular'),"BlinkColor",10);
function shake(ele, cls, times) {
    var i = 0, t = false, o = ele.attr("class") + " ", c = "", times = times || 2;
    if (t) return;
    t = setInterval(function () {
        i++;
        c = i % 2 ? o + cls : o;
        ele.attr("class", c);
        if (i == 2 * times) {
            clearInterval(t);
            ele.removeClass(cls);
        }
    }, 200);
};


12345678
