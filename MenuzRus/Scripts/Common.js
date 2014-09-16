// Sequential number per session
var QZPrint = false;
var uid = (function () { var id = 1; return function () { if (arguments[0] === 1) id = 1; return id++; } })();

$(function () {
    $.blockUI.defaults.message = $("#bowlG");
    $.blockUI.defaults.css = " border: '0px none transparent;";
    //$(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);

    $(".page").sortable();
    setMenu();
});

// Big guid
function guid() {
    var buf = new Uint16Array(8);
    window.crypto.getRandomValues(buf);
    var S4 = function (num) {
        var ret = num.toString(16);
        while (ret.length < 4) {
            ret = "0" + ret;
        };
        return ret;
    };
    return (S4(buf[0]) + S4(buf[1]) + "-" + S4(buf[2]) + "-4" + S4(buf[3]).substring(1) + "-y" + S4(buf[4]).substring(1) + "-" + S4(buf[5]) + S4(buf[6]) + S4(buf[7]));
}

// short guid
function shortGuid() {
    return guid().substring(0, 8);
}

function setMenu() {
    if (window.location.href.indexOf("/Login") == -1)
        $(".menuAlways").show();
    else
        $(".menuAlways").hide();

    if (window.location.href.indexOf("/MenuDesigner") > -1)
        $(".menuDesigner").removeClass("hide");
    else
        $(".menuDesigner").addClass("hide");
};

function deleteImage() {
    $(".preview").attr("src", "");
    $("#ImageUrl").val("");
};

function initImageUpload(element) {
    $(".preview").click(function (e) {
        element.block();
        $("#Image").click();
    });

    $("#Image").change(function (e) {
        preViewImage(e, $(".preview"));
        element.unblock();
    });
}

function preViewImage(e, element) {
    if (e.target.files[0].type.match('image.*')) {
        if (typeof FileReader == "undefined") return true;
        if (e.target.files && e.target.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                element.attr("src", e.target.result);
            };
            reader.readAsDataURL(e.target.files[0]);
            element.unblock();
        }
    }
}

function message(text, type, position) {
    noty({
        text: text,
        layout: position,
        type: type,
        template: '<div class="noty_message"><span class="noty_text"></span><div class="noty_close"></div></div>',
        timeout: "10000",
        maxVisible: 1,
        killer: true,
        closable: true,
        closeOnSelfClick: true,
        closeWith: ["click", "button"]
    });
}

function getStatusName(status) {
    switch (status) {
        case 1:
            return "Open";
            break;
        case 2:
            return "Served";
            break;
        case 3:
            return "Closed";
            break;
        case 4:
            return "Paid";
            break;
    }
}

function getStatusColor(status) {
    switch (status) {
        case 1:
            return "info";
            break;
        case 2:
            return "warning";
            break;
        case 3:
            return "success";
            break;
        case 4:
            return "danger";
            break;
    }
}