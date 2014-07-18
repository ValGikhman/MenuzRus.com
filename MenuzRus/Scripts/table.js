var tableId;

$(function () {
    tableId = $("#TableId").val();

    $("#btnAdd").click(function (e) {
        $(".checks").append($.validator.format("<li><a href='#{0}' data-value='0' data-toggle='tab'>{0}{1}</a></li>", "New", addCloseButton()));
        $(".check").append($.validator.format("<div class='tab-pane fade in active' id='New' data-value='0'></div>"));
        $(".checks li:last a").tab("show");
        BindEvents();
    });

    $("#btnBack").click(function () {
        window.location = $.validator.format("{0}Order/Tables", root);
    })

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
    var container = $(".order");
    container.block();
    toggleAll(false);
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
            $(active).append(result);
            var activeTab = $(".checks").find(".active a");
            var checkId = $(active).find(".panel").attr("data-check");
            $(active).attr("id", $.validator.format("Check{0}", checkId)).attr("data-value", checkId);
            $(activeTab).attr("href", $.validator.format("#Check{0}", checkId));
            $(activeTab).attr("data-value", checkId);
            $(activeTab).html($.validator.format("Check#{0}{1}", checkId, addCloseButton()));
            BindEvents();
        })
        .fail(function () {
            message("Failed.", "error", "topCenter");
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
            if ($(active).length > 0) {
                $(active).tab("show");
                showMenus($(active).attr("data-value"));
            }
        })
        .fail(function () {
            message("Failed.", "error", "topCenter");
        })
        .always(function () {
            container.unblock();
            BindEvents();
        });
};

function saveItem(element) {
    object = $(element).find("input");
    var productId = $(element).parent().attr("data-value");
    var knopaId = $(object).attr("data-value");
    var type = $(object).attr("data-type");

    var container = $(".order");
    container.block();
    var jqxhr = $.post($.validator.format("{0}Order/SaveItem", root), { "productId": productId, "knopaId": knopaId, "type": type }, "json")
        .done(function (result) {
            $(".order").html();
        })
        .fail(function () {
            message("Failed.", "error", "topCenter");
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
            message("Failed.", "error", "topCenter");
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
            message("Failed.", "error", "topCenter");
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
    var jqxhr = $.post($.validator.format("{0}Order/DeleteCheck", root), { "checkId": checkId }, "json")
        .done(function (result) {
            $(tabSelector).parent().fadeOut("slow", function () {
                $(tabSelector).parent().parent().remove();
                $(divSelector).remove();
            });
        })
        .fail(function () {
            message("Failed.", "error", "topCenter");
        })
        .always(function () {
            $(".checks li:last a").tab("show");
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
        message("Failed.", "error", "topCenter");
    })
    .always(function () {
        parent.unblock();
    });
}

function BindEvents() {
    $(".checks").on("click", "a", function (e) {
        var checkId = $(this).attr("data-value");
        if (checkId != "0") {
            showMenus(checkId);
        }
    })

    $("button.close").on("click", function () {
        var object = $(this);
        noty({
            layout: "center",
            type: "error",
            killer: true,
            model: true,
            text: "<strong>" + $(object).parent().text().replace(/\t\r\n\s/g, '') + "</strong></em> will be deleted.<br />Would you like to continue ?",
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