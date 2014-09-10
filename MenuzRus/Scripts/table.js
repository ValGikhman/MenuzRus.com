var tableId;

$(function () {
    tableId = $("#Table_id").val();
    if ($("#btnTableStatus").text() == "Closed") {
        $(".layout").block();
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
        var text = $(this).text();
        $("#btnCheckStatus").text(text);
        var selected = $(".chosen-select")[0].selectedOptions;
        $.each($(selected), function (index, item) {
            $($.validator.format(".check .tab-pane[data-value={0}]", $(item).val())).attr("data-status", text);
            $($.validator.format(".checks li a[data-value={0}]", $(item).val())).attr("data-status", text);
            updateCheckStatus($(item).val(), text);
        });
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
        $(".checks").append($.validator.format("<li><a href='#{0}' data-value='0' data-toggle='tab'>{0}{1}</a></li>", "New", addCloseButton()));
        $(".check").append($.validator.format("<div class='tab-pane fade in active' id='New' data-value='0'></div>"));
        $(".checks li:last a").tab("show");
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
    });

    $("#printTab").click(function () {
        $(this).tab("show");

        var tabs = $(".check").find(".tab-pane");
        $(tabs).html("");

        var checkId = $(".check").find(".tab-pane.active").attr("data-value");

        $(".chosen-select").val(checkId).trigger("chosen:updated");

        showCheckPrint();
    });

    $("#propertiesTab").click(function () {
        $(this).tab("show");

        var tabs = $(".check").find(".tab-pane");
        $(tabs).html("");

        var checkId = $(".check").find(".tab-pane.active").attr("data-value");

        $(".check").find(".tab-pane").html("");
        $(".chosen-select").val(checkId).trigger("chosen:updated");

        showMenus(checkId);
    });

    $("#menuTab").tab("show");
    $("#propertiesTab").tab("show");

    showOrder(tableId);
})

function addCloseButton() {
    return "<button class='close btn btn-primary' title='Remove this check' type='button'><i class='glyphicon glyphicon-remove-circle' /></button>";
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

    if ($(toggleObject).is(":visible"))
        $(thisObject).removeClass("glyphicon-plus").addClass("glyphicon-minus");
    else
        $(thisObject).removeClass("glyphicon-minus").addClass("glyphicon-plus");
}

function orderMenuItem(id) {
    var container = $(".menuItems");
    container.block();
    //toggleAll(false);
    var active = $(".check").find(".tab-pane.active");
    if (active.length == 0) {
        $("#btnAdd").click();
        active = $(".check").find(".tab-pane.active");
    }

    var activeTab = $(".checks").find(".active a");
    var checkId = $(active).attr("data-value");

    if (typeof (checkId) == "undefined") checkId = 0;
    container.block();
    var jqxhr = $.get($.validator.format("{0}Order/OrderMenuItem", root), { "id": id, "checkId": checkId, "tableId": tableId }, "json")
        .done(function (result) {
            $(active).append(result.html);
            $(active).attr("id", $.validator.format("Check{0}", result.checkId)).attr("data-value", result.checkId);
            $(active).attr("data-value", result.checkIdheckId);
            $(activeTab).attr("href", $.validator.format("#Check{0}", result.checkId));
            $(activeTab).attr("data-value", result.checkIdheckId);
            $(activeTab).html($.validator.format("#{0}{1}", result.checkId, addCloseButton()));
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
                showMenus($(active).attr("data-value"));
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
    var container = $(".container-order");
    var checkId = $(".check").find(".tab-pane.active").attr("data-value");
    container.block();
    var jqxhr = $.get($.validator.format("{0}Order/DeleteMenu", root), { "id": $(object).attr("data-value"), "checkId": checkId }, "json")
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

function deleteCheck(object) {
    var tabSelector = $(object);
    var divSelector = $.validator.format("div{0}", $(object).parent().attr("href"));
    var checkId = $(divSelector).attr("data-value");
    if (checkId == "0") {
        $(tabSelector).parent().fadeOut("slow", function () {
            $(tabSelector).parent().parent().remove();
            $(divSelector).remove();
            var active = $(".checks li:last a");
            if ($(active).length > 0) {
                $(active).tab("show");
                showMenus($(active).attr("data-value"));
            }
        });
        return;
    }
    var container = $(".container-order");
    container.block();
    var jqxhr = $.get($.validator.format("{0}Order/DeleteCheck", root), { "checkId": checkId }, "json")
        .done(function (result) {
            $(tabSelector).parent().fadeOut("slow", function () {
                $(tabSelector).parent().parent().remove();
                $(divSelector).remove();
                active = $(".checks li:last a");
                if ($(active).length > 0) {
                    $(active).tab("show");
                    showMenus($(active).attr("data-value"));
                }
            });
        })
        .fail(function () {
            message("::deleteCheck:: Failed.", "error", "topCenter");
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
    var split = parseInt($("#slideSplit").slider("value"));
    var adjustment = parseInt($("#adjustmentSplit").slider("value"));
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
    var split = parseInt($("#slideSplit").slider("value"));
    var adjustment = parseInt($("#adjustmentSplit").slider("value"));
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
            $(".container-order").block();
            window.location.reload();
        });
}

function updateCheckType(id, type) {
    var jqxhr = $.post($.validator.format("{0}Order/UpdateCheckType", root), { "checkId": id, "type": type }, "json")
        .done(function (result) {
            message("Type changed", "success", "topCenter");
        })
        .fail(function () {
            message("::updateCheckType:: Failed.", "error", "topCenter");
        })
        .always(function () {
        });
}

function updateCheckStatus(id, status) {
    var adjustment = parseInt($("#adjustmentSplit").slider("value"));
    var type = $(".check").find(".tab-pane.active").attr("data-type");
    var jqxhr = $.post($.validator.format("{0}Order/UpdateCheckStatus", root), { "checkId": id, "status": status }, "json")
        .done(function (result) {
            message("Status changed", "success", "topCenter");
        })
        .fail(function () {
            message("::updateCheckStatus:: Failed.", "error", "topCenter");
        })
        .always(function () {
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
            $(".container-order").block();
            window.location.reload();
        });
}
function BindEvents() {
    $(".checks").on("click", "a", function (e) {
        $(this).tab("show");
        var checkId = $(this).attr("data-value");
        $("#btnCheckType").text($(this).attr("data-type"));
        $("#btnCheckStatus").text($(this).attr("data-status"));

        $(".chosen-select").val(checkId).trigger("chosen:updated");
        var html = $(".check").find(".tab-pane.active").html().replace(/\n/g, "").replace(/\s/g, "");
        if (html == "") {
            if ($("#printTab").parent().hasClass("active")) {
                showCheckPrint();
            }
            else {
                showMenus(checkId);
            }
        }
    })

    $("button.close").on("click", function () {
        var object = $(this);
        noty({
            layout: "center",
            type: "error",
            killer: true,
            model: true,
            text: "<strong>" + $(object).parent().text().replace(/\t\r\n\s/g, "") + "</strong></em> will be deleted.<br />Would you like to continue ?",
            buttons: [{
                addClass: 'btn btn-danger', text: 'Delete', onClick: function ($noty) {
                    $noty.close();
                    deleteCheck(object);
                }
            },
              {
                  addClass: 'btn btn-default', text: 'Cancel', onClick: function ($noty) {
                      $noty.close();
                  }
              }
            ]
        });
    });
}