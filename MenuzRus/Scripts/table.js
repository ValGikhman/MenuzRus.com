var tableId;

$(function () {
    setMenu();

    tableId = $("#Table_id").val();

    if ($("#btnTableStatus").text() == "Closed") {
        $("#btnAdd").hide();
        $("#menuTab").remove();

        $("#actionsTab").parent().addClass("active");
        $("#actions-tab").addClass("active");
    }

    $("#checkType li a").click(function () {
        var text = $(this).text();
        $("#btnCheckType").text(text);
        var selected = $(".chosen-select")[0].selectedOptions;
        $.each($(selected), function (index, item) {
            $($.validator.format(".check .tab-pane[data-value={0}]", $(item).val())).attr("data-type", text);
            $($.validator.format(".checks li a[data-value={0}]", $(item).val())).attr("data-type", text);
            updateCheckType($(item).val(), text);
        });
    });

    $("#checkStatus li a").click(function () {
        var split = parseInt($("#slideSplit").val());
        var adjustment = parseInt($("#adjustmentSplit").val());
        var text = $(this).text();
        $("#btnCheckStatus").text(text);

        var selector = $(".chosen-select")[0];
        var selected = selector.selectedOptions;

        $.each($(selected), function (index, item) {
            $($.validator.format(".check .tab-pane[data-value={0}]", $(item).val())).attr("data-status", text);
            $($.validator.format(".checks li a[data-value={0}]", $(item).val())).attr("data-status", text);
            updateCheckStatus($(item).val(), text, adjustment, split);
        });

        toggleObjects(text);
    });

    $("#tableStatus li a").click(function () {
        var text = $(this).text();
        $("#btnTableStatus").text(text);
        if (text == "Open") {
            addNewTableOrder(tableId);
        }
        else {
            updateTableStatus($("#TableOrder_id").val(), $(this).attr("data-value"));
        }
    });

    $("#btnAdd").click(function (e) {
        $(".checks").append($.validator.format("<li><a href='#{0}' data-value='0' data-type='{1}' data-status='{2}' data-toggle='tab'>{0}</a></li>", "New", 'Guest', 'Active'));
        $(".check").append($.validator.format("<div class='tab-pane fade in active' id='New' data-value='0' data-type='{0}' data-status='{1}'></div>", 'Guest', 'Active'));
        $(".checks li:last a").tab("show");
        // Always default to Menu
        $("#menuTab").show().tab("show");
        checkStatusManager("Active");
        $("#btnCheckType").text("Guest");
        $("#btnCheckStatus").text("Active");

        BindEvents();
    });

    $("#btnBack").click(function () {
        var referer = $("#Referer").val();
        window.location = $.validator.format("{0}Order/{1}", root, referer);
    })

    $("#menuTab").click(function () {
        $(this).tab("show");
        $(".check").find(".tab-pane").html("");
        showMenus();
    });

    $("#actionsTab").click(function () {
        $(this).tab("show");
        var active = $(".check").find(".tab-pane.active");
        var checkId = $(active).attr("data-value");
        var type = $(active).attr("data-type");
        var status = $(active).attr("data-status");
        $(".chosen-select").val(checkId).trigger("chosen:updated");
        $("#btnCheckStatus").text(status);
        $("#btnCheckType").text(type);

        showCheckPrint();

        if (webSocket == null) {
            $("#btnPrintChecks").hide();
        }
        else {
            $("#btnPrintChecks").show();
        }
    });

    $("#menuTab").tab("show");

    $("#Menus li a").click(function () {
        $("#btnMenus").text($(this).text());
        getCurrentMenu($(this).attr("data-value"));
        var cookie = $.validator.format("{0}:{1}", $(this).attr("data-value"), $(this).text());
        createCookie("currentMenu", cookie, 0);
    });

    showOrder(tableId);

    $("div#popover").popover({
        animation: false,
        placement: "bottom",
        html: true,
        trigger: "click",
        title: "<h5 class='custom-title' style='margin: 0px;'><span class='glyphicon glyphicon-info-sign'></span> Description </h5>",
        template: "<div class='popover' role='tooltip'><div class='popover-arrow'></div><div class='popover-title alert-info'></div><div class='popover-content'></div></div>"
    });

    $("[data-toggle='popover']").on("shown.bs.popover", function () {
        $(".popover").css("top", 6);
    });
})

function hidePopover() {
    $("div#popover").popover("hide");
}

function addStatusSign(text) {
    var retVal = "";
    if (text == "Ordered") {
        retVal = "<i class='glyphicon glyphicon-usd paid'></i>"
    }
    return retVal;
}

function deleteItself(object) {
    deleteMenu(object);
}

function toggleAll(collapse) {
    if (!collapse) {
        $(".collapsibleSign").removeClass("glyphicon-minus").addClass("glyphicon-plus");
        $(".collapsible").hide();
    }

    else {
        $(".collapsibleSign").removeClass("glyphicon-plus").addClass("glyphicon-minus");
        $(".collapsible").show();
    }
}

function toggleItself(thisObject, toggleObject) {
    var anyText = $(toggleObject).html().replace(/(\r\n|\n|\r|\s)/gm, "");
    if (anyText == "") {
        showMenuProducts(thisObject, toggleObject);
    }

    $(toggleObject).toggle();

    if ($(toggleObject).is(":visible")) {
        $(thisObject).removeClass("glyphicon-plus").addClass("glyphicon-minus");
    }
    else {
        $(thisObject).removeClass("glyphicon-minus").addClass("glyphicon-plus");
    }
}

function toggleCategory(thisObject, toggleObject) {
    $(toggleObject).toggle();

    if ($(toggleObject).is(":visible"))
        $(thisObject).removeClass("glyphicon-plus").addClass("glyphicon-minus");
    else
        $(thisObject).removeClass("glyphicon-minus").addClass("glyphicon-plus");
}

function orderMenuItem(id) {
    var container = $(".menuItems");
    var active = $(".check").find(".tab-pane.active");

    if (active.length == 0) {
        $("#btnAdd").click();
        active = $(".check").find(".tab-pane.active");
    }

    if ($(active).attr("data-status") == "Ordered") {
        var text = "Active";
        var split = parseInt($("#slideSplit").val());
        var adjustment = parseInt($("#adjustmentSplit").val());

        $("#btnCheckStatus").text(text);
        $($.validator.format(".check .tab-pane[data-value={0}]", $(active).attr("data-value"))).attr("data-status", text);
        $($.validator.format(".checks li a[data-value={0}]", $(active).attr("data-value"))).attr("data-status", text);
        updateCheckStatus($(active).attr("data-value"), text, adjustment, split);
    }

    var activeTab = $(".checks").find(".active a");
    var checkId = $(active).attr("data-value");

    if (typeof (checkId) == "undefined") checkId = 0;
    container.block();
    var jqxhr = $.get($.validator.format("{0}Order/OrderMenuItem", root), { "id": id, "checkId": checkId, "tableId": tableId }, "json")
        .done(function (result) {
            $(active).append(result.html);
            var status = $("#btnCheckStatus").text();
            var type = $("#btnCheckType").text();
            var chosen = $(".chosen-select");

            if ($(chosen).find($.validator.format("option[value={0}]", result.checkId)).length == 0) {
                $(chosen).append($.validator.format("<option value='{0}'>#{0}</option>", result.checkId)).val(result.checkId).trigger("chosen:updated");
                toggleObjects(status);
            }

            $(active).attr("id", $.validator.format("Check{0}", result.checkId)).attr("data-value", result.checkId);
            $(active).attr("data-value", result.checkId);
            $(active).attr("data-type", type);
            $(active).attr("data-status", status);
            $(activeTab).attr("href", $.validator.format("#Check{0}", result.checkId));
            $(activeTab).attr("data-value", result.checkId);
            $(activeTab).attr("data-type", type);
            $(activeTab).attr("data-status", status);
            $(activeTab).html($.validator.format("#{0}", result.checkId));
        })
        .fail(function () {
            message("::orderMenuItem:: Failed.", "error", "topCenter");
        })
        .always(function () {
            BindEvents();
            container.unblock();
        });
};

function showOrder(tableId) {
    var container = $(".order");
    container.block();
    var jqxhr = $.get($.validator.format("{0}Order/ShowOrder", root), { "tableId": tableId }, "json")
        .done(function (result) {
            $(".order").html(result);
            var active = $(".checks li:last a");
            if ($(active).length > 0) {
                $(active).tab("show");
                $("#btnCheckType").text($(active).attr("data-type"));
                $("#btnCheckStatus").text($(active).attr("data-status"));
                toggleObjects($(active).attr("data-status"));

                $(".chosen-select").val($(active).attr("data-value")).trigger("chosen:updated");
                checkStatusManager($(active).attr("data-status"));
                if ($("#actionsTab").parent().hasClass("active")) {
                    showCheckPrint();
                }
                else {
                    showMenus($(active).attr("data-value"));
                }
            }
        })
        .fail(function () {
            message("::showOrder:: Failed.", "error", "topCenter");
        })
        .always(function () {
            container.unblock();
            BindEvents();
        });
};

function saveItem(element) {
    object = $(element).find("input");
    var checkId = $(".check").find(".tab-pane.active").attr("data-value");
    var productId = $(element).parent().attr("data-value");
    var knopaId = $(object).attr("data-value");
    var type = $(object).attr("data-type");

    var container = $(".order");
    container.block();
    var jqxhr = $.post($.validator.format("{0}Order/SaveItem", root), { "checkId": checkId, "productId": productId, "knopaId": knopaId, "type": type }, "json")
        .done(function (result) {
        })
        .fail(function () {
            message("::saveItem:: Failed.", "error", "topCenter");
        })
        .always(function () {
            container.unblock();
            BindEvents();
        });
};

function deleteMenu(object) {
    var container = $(".layout");
    container.block();
    var jqxhr = $.get($.validator.format("{0}Order/DeleteMenu", root), { "id": $(object).attr("data-value") }, "json")
        .done(function (result) {
            $(object).fadeOut("slow", function () {
                object.remove();
            });
        })
        .fail(function () {
            message("::deleteMenu:: Failed.", "error", "topCenter");
        })
        .always(function () {
            container.unblock();
        });
}

function showMenuProducts(object, addTo) {
    var parent = $(object).parent().parent();
    var menuId = $(parent).attr("data-value");
    parent.block();
    var jqxhr = $.get($.validator.format("{0}Order/ShowMenuProducts", root), { "menuId": menuId }, "json")
            .done(function (result) {
                $(addTo).html(result);
                BindEvents();
            })
    .fail(function () {
        message("::showMenuProducts:: Failed.", "error", "topCenter");
    })
    .always(function () {
        parent.unblock();
    });
}

function showCheckPrint() {
    var container = $(".order");
    container.block();
    var active = $(".check").find(".tab-pane.active");
    var split = parseInt($("#slideSplit").val());
    var adjustment = parseInt($("#adjustmentSplit").val());
    var checkId = $(active).attr("data-value");
    var checkType = $(active).attr("data-type");

    var jqxhr = $.get($.validator.format("{0}Order/ShowCheckPrint", root), { "checkId": checkId, "type": checkType, "split": split, "adjustment": adjustment }, "json")
        .done(function (result) {
            $(active).html(result);
        })
        .fail(function () {
            message("::showCheckPrint:: Failed.", "error", "topCenter");
        })
        .always(function () {
            $(container).unblock();
        });
}

function getCheckPrint(checkId) {
    var tab = $("#html2Print");
    var split = parseInt($("#slideSplit").val());
    var adjustment = parseInt($("#adjustmentSplit").val());
    var type = $(".check").find(".tab-pane.active").attr("data-type");
    var status = $(".check").find(".tab-pane.active").attr("data-status");

    var jqxhr = $.get($.validator.format("{0}Order/ShowCheckPrint", root), { "checkId": checkId, "type": type, "status": status, "split": split, "adjustment": adjustment }, "json")
        .done(function (result) {
            $(tab).append($.validator.format("{0}", result));
        })
        .fail(function () {
            message("::getCheckPrint:: Failed.", "error", "topCenter");
        })
        .always(function () {
        });
}

function showMenus(checkId) {
    if (typeof (checkId) == "undefined" || checkId == null) {
        checkId = $(".check").find(".tab-pane.active").attr("data-value");
        if (typeof (checkId) == "undefined" || checkId == null) {
            return;
        }
    }

    var container = $(".order");
    container.block();

    var jqxhr = $.get($.validator.format("{0}Order/ShowMenus", root), { "checkId": checkId }, "json")
        .done(function (result) {
            $(".check").find(".tab-pane.active").html(result.html);
            var active = $(".checks li:last a");
            if ($(active).length > 0) {
                toggleObjects($(active).attr("data-status"));
            }
        })
        .fail(function () {
            message("::showMenus:: Failed.", "error", "topCenter");
        })
        .always(function () {
            container.unblock();
        });
}

function updateTableStatus(tableOrderId, status) {
    var jqxhr = $.post($.validator.format("{0}Order/UpdateTableStatus", root), { "tableOrderId": tableOrderId, "status": status }, "json")
        .done(function (result) {
            message("Status changed", "success", "topCenter");
        })
        .fail(function () {
            message("::updateTableStatus:: Failed.", "error", "topCenter");
        })
        .always(function () {
            $(".layout").block();
            window.location.reload();
        });
}

function updateCheckType(id, type) {
    var container = $(".layout");
    container.block();

    var jqxhr = $.post($.validator.format("{0}Order/UpdateCheckType", root), { "checkId": id, "type": type }, "json")
        .done(function (result) {
            message("Type changed", "success", "topCenter");
        })
        .fail(function () {
            message("::updateCheckType:: Failed.", "error", "topCenter");
        })
        .always(function () {
            container.unblock();
        });
}

function updateCheckStatus(id, status, adjustment, split) {
    var container = $(".layout");
    container.block();

    var type = $(".check").find(".tab-pane.active").attr("data-type");
    var jqxhr = $.post($.validator.format("{0}Order/UpdateCheckStatus", root), { "checkId": id, "status": status, "adjustment": adjustment, "split": split, "type": type }, "json")
        .done(function (result) {
            checkStatusManager(status);
            message("Status changed", "success", "topCenter");
        })
        .fail(function () {
            message("::updateCheckStatus:: Failed.", "error", "topCenter");
        })
        .always(function () {
            container.unblock();
        });
}

function addNewTableOrder(tableId) {
    var jqxhr = $.post($.validator.format("{0}Order/AddNewTableOrder", root), { "tableId": tableId }, "json")
        .done(function (result) {
            message("New order started", "success", "topCenter");
        })
        .fail(function () {
            message("::AddNewTableOrder:: Failed.", "error", "topCenter");
        })
        .always(function () {
            $(".layout").block();
            window.location.reload();
        });
}

function toggleObjects(text) {
    var active = $(".checks").find(".active a");
    var activeTab = $(".check").find(".tab-pane.active");

    $(active).find(".paid").remove();
    $(activeTab).find(".btnDelete").hide();
    $("#btnDeleteSelectedChecks").hide();
    $("#menuTab").show();

    if (text == "Active") {
        $(activeTab).find(".btnDelete").show();
        $("#btnDeleteSelectedChecks").show();
    }
    else if (text == "Ordered") {
        $(active).append(addStatusSign(text));
    }
    else if (text == "Paid") {
        $("#menuTab").hide();
        $("#actionsTab").tab("show");
    }
    else if (text == "Ready") {
        $("#menuTab").hide();
        $("#actionsTab").tab("show");
    }
}

function BindEvents() {
    $(".checks").on("click", "a", function (e) {
        $(this).tab("show");
        var checkId = $(this).attr("data-value");
        $("#btnCheckType").text($(this).attr("data-type"));
        $("#btnCheckStatus").text($(this).attr("data-status"));
        toggleObjects($(this).attr("data-status"));
        checkStatusManager($(this).attr("data-status"));

        $(".chosen-select").val(checkId).trigger("chosen:updated");
        var html = $(".check").find(".tab-pane.active").html().replace(/\n/g, "").replace(/\s/g, "");
        if (html == "") {
            if ($("#actionsTab").parent().hasClass("active")) {
                showCheckPrint();
            }
            else {
                showMenus(checkId);
            }
        }
    });
}

function checkStatusManager(status) {
    $("#checkStatus li").show();

    switch (status) {
        case "Active": {
            $("#checkStatus li[data-value='Active']").hide();
            break;
        }
        case "Ordered": {
            $("#checkStatus li[data-value='Active']").hide();
            $("#checkStatus li[data-value='Ordered']").hide();
            break;
        }
        case "Ready": {
            // Can be returned to  Ordered, if some stuff is added or comments were made
            //$("#checkStatus li[data-value='Ordered']").addClass("disabled");
            $("#checkStatus li[data-value='Active']").hide();
            $("#checkStatus li[data-value='Ready']").hide();
            break;
        }
        case "Paid": {
            $("#checkStatus li[data-value='Active']").hide();
            $("#checkStatus li[data-value='Ordered']").hide();
            $("#checkStatus li[data-value='Ready']").hide();
            $("#checkStatus li[data-value='Paid']").hide();
            break;
        }
        case "Cancelled": {
            $("#checkStatus li[data-value='Active']").hide();
            $("#checkStatus li[data-value='Ordered']").hide();
            $("#checkStatus li[data-value='Ready']").hide();
            $("#checkStatus li[data-value='Paid']").hide();
            $("#checkStatus li[data-value='Cancelled']").hide();
            break;
        }
    }
}

function getCurrentMenu(id) {
    var container = $(".menu");
    container.block();

    var jqxhr = $.get($.validator.format("{0}Order/GetCurrentMenu", root), { "id": id }, "json")
        .done(function (result) {
            $("#menu-tab").html(result);
        })
        .fail(function () {
            message("::getCurrentMenu:: Failed.", "error", "topCenter");
        })
        .always(function () {
            container.unblock();
        });
}

function setMenu() {
    var cookies = readCookie("currentMenu");
    if (cookies) {
        var array = cookies.split(':');
        getCurrentMenu(array[0]);
        $("#btnMenus").text(array[1]);
    }
}