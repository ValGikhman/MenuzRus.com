var tableId;

$(function () {
    tableId = $("#TableId").val();

    $("#btnAdd").click(function (e) {
        $(".checks").append("<li><a href='#New' data-toggle='tab'>New<button class='close btn btn-primary' title='Remove this check' type='button'><i class='glyphicon glyphicon-remove-circle' /></button></a></li>");
        $(".check").append($.validator.format("<div class='tab-pane fade in active' id='New' data-value='0'></div><button class='close btn btn-primary' title='Remove this check' type='button'><i class='glyphicon glyphicon-remove-circle'></button>"));
        $(".checks li:last a").tab("show");
    });

    $("#btnBack").click(function () {
        window.location = $.validator.format("{0}Order/Tables", root);
    })

    showOrder(tableId);
})

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
    var container = $(".order");
    container.block();

    var active = $(".check").find(".active");
    if (active.length == 0) {
        $("#btnAdd").click();
        active = $(".check").find(".active");
    }
    var orderId = $(active).attr("data-value");
    if (typeof (orderId) == "undefined") orderId = 0;

    container.block();
    var jqxhr = $.post($.validator.format("{0}Order/OrderMenuItem", root), { "id": id, "orderId": orderId, "tableId": tableId }, "json")
        .done(function (result) {
            toggleAll(false);
            $(active).append(result);
            BindEvents();
        })
        .fail(function () {
            message("Failed.", "error", "center");
        })
        .always(function () {
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
            $(active).tab("show");
            showMenus($(active).attr("data-value"));
        })
        .fail(function () {
            message("Failed.", "error", "center");
        })
        .always(function () {
            container.unblock();
            BindEvents();
        });
};

function showMenus(checkId) {
    var jqxhr = $.get($.validator.format("{0}Order/ShowMenus", root), { "checkId": checkId }, "json")
        .done(function (result) {
            $(".check").find(".active").html(result);
        })
        .fail(function () {
            message("Failed.", "error", "center");
        })
        .always(function () {
        });
}

function deleteMenu(object) {
    var container = $(".container-order");
    container.block();
    var jqxhr = $.post($.validator.format("{0}Order/DeleteMenu", root), { "id": $(object).attr("data-value") }, "json")
        .done(function (result) {
            $(object).fadeOut("slow", function () {
                object.remove();
            })
        })
        .fail(function () {
            message("Failed.", "error", "center");
        })
        .always(function () {
            container.unblock();
        });
}

function deleteCheck(object) {
    var container = $(".container-order");
    container.block();

    var tabSelector = $(object);
    var divSelector = $.validator.format("div{0}", $(object).parent().attr("href"));

    var jqxhr = $.post($.validator.format("{0}Order/DeleteCheck", root), { "checkId": $(divSelector).attr("data-value") }, "json")
        .done(function (result) {
            $(tabSelector).parent().fadeOut("slow", function () {
                $(tabSelector).parent().parent().remove();
                $(divSelector).remove();
                $(".checks li:last a").tab("show");
            });
        })
        .fail(function () {
            message("Failed.", "error", "center");
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
            })
    .fail(function () {
        message("Failed.", "error", "center");
    })
    .always(function () {
        parent.unblock();
    });
}

function BindEvents() {
    $(".checks").on("click", "a", function (e) {
        showMenus($(this).attr("data-value"));
    })

    $("button.close").on("click", function () {
        deleteCheck($(this));
    });
}