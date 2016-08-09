var uid = (function () { var id = 1; return function () { if (arguments[0] === 1) id = 1; return id++; } })();
var webSocket;
var printerKitchen = "";
var printers = "";
var printerPOS = "";
var printerKitchenWidth = "";
var printerPOSWidth = "";
var alertsDelay = 60 * 1000;

$(function () {
    $.blockUI.defaults.message = $("#bowlG");
    $.blockUI.defaults.css = " border: '0px none transparent;";

    $(".page").sortable();

    getAlertsCount();
    window.setInterval(refreshActions, alertsDelay);
    $("#printerImage").on("click", function () {
        message($.validator.format("Kitchen Printer:{0}<br/>POS Printer:{1}", printerKitchen, printerPOS), "warning", "topLeft");
    });
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
    if (e.target.files[0].type.match("image.*")) {
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
        closeWith: ["click", "button"],
        animation: {
            open: { height: 'toggle' },
            close: { height: 'toggle' },
            easing: 'swing',
            speed: 500 // opening & closing animation speed
        },
    });
}

function buzz(title, message) {
    noty({
        text: title,
        layout: "topRight",
        type: "warning",
        killer: false,
        modal: true,
        template: '<div class="noty_message"><span class="noty_text" style="text-align:left;"></span></div>',
        animation: {
            open: { height: 'toggle' },
            close: { height: 'toggle' },
            easing: 'swing',
            speed: 500 // opening & closing animation speed
        },
        callback: {
            onShow: function ($noty) {
                this.$buttons.append(message).css("text-align", "");
            },
            afterShow: function () {
            },
            onClose: function () {
            },
            afterClose: function () {
            }
        },
        buttons: [
            {
                addClass: 'btn btn-warning', text: 'OK', onClick: function ($noty) {
                    readMessage($(this).parent());
                    $noty.close();
                }
            }
        ]
    });
}

function readMessage(element) {
    var id = $(element).find("div").attr("id");
    var jqxhr = $.get($.validator.format("{0}Alert/ReadAlert", root), { "id": id }, "json")
        .done(function (result) {
            refreshMessageBadge(result);
        })
    .fail(function () {
        message("::readMessage:: Failed.", "error", "topCenter");
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

function refreshActions() {
    getAlertsCount();
};

function printKitchenOrders() {
    var jqxhr = $.get($.validator.format("{0}Order/KitchenOrders2Print", root))
        .done(function (result) {
            $.each(result.rows, function (i, e) {
                printKitchenOrder(e.id);
            })
        })
        .fail(function () {
            message("::printKitchenOrders:: Failed.", "error", "topCenter");
        });
}

function printKitchenOrder(id) {
    var jqxhr = $.get($.validator.format("{0}Order/KitchenOrder2Print", root), { "id": id }, "json")
        .done(function (result) {
            printData(result, printerKitchen);
            updateKitchenOrderPrintStatus(id);
        })
        .fail(function () {
            message("::printKitchenOrder:: Failed.", "error", "topCenter");
        });
}

function updateKitchenOrderPrintStatus(id) {
    var jqxhr = $.post($.validator.format("{0}Order/UpdateKitchenOrderPrintStatus", root), { "id": id }, "json")
        .done(function (result) {
        })
        .fail(function () {
            message("::updateKitchenOrderPrintStatus:: Failed.", "error", "topCenter");
        });
}

function getAlertsCount() {
    var jqxhr = $.get($.validator.format("{0}Alert/GetAlertsCount", root))
        .done(function (result) {
            refreshMessageBadge(result);
        })
        .fail(function () {
            message("::getAlertsCount:: Failed.", "error", "topCenter");
        })
        .always(function () {
        });
};

// TODO: Delete
function getPrinters() {
    var jqxhr = $.get($.validator.format("{0}Home/GetPrinters", root))
        .done(function (result) {
            printerPOS = result.printerPOS;
            printerKitchen = result.printerKitchen;
            printerPOSWidth = result.printerPOSWidth;
            printerKitchenWidth = result.printerKitchenWidth;
        })
        .fail(function () {
            message("::getPrinters:: Failed.", "error", "topCenter");
        })
        .always(function () {
        });
};

function getAlerts() {
    var jqxhr = $.get($.validator.format("{0}Alert/GetAlerts", root))
        .done(function (result) {
            var alerts = result.alerts;
            $.each(alerts, function (i, e) {
                var title = $.validator.format("<strong>Check#{0}</strong>", e.CheckId);
                var message = $.validator.format("<div id='{0}' style='display:inline; float:right'>{1} is ready.</div>", e.id, e.Item);
                buzz(title, message);
            });
        })
        .fail(function () {
            message("::getAlerts:: Failed.", "error", "topCenter");
        })
        .always(function () {
        });
};

function refreshMessageBadge(count) {
    if (count == 0) {
        $(".alertsCount").hide();
    }
    else {
        $(".alertsCount").show();
        $(".alertsCount").html(count);
    }
};

function printData(data, name) {
    if (WSPrint) {
        WSPrint.printHTML(data, name);
    }
    else {
        message("::printData:: Printing is not connected.", "error", "topCenter");
    }
}