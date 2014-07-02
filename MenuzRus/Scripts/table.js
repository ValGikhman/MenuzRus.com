var checkNum = 0;
$(function () {
    $(".checks").on("click", "a", function (e) {
    })
    .on("click", "button.close", function () {
        checkNum--;
        var tabSelector = $(this);
        var divSelector = $.validator.format("div{0}", $(this).parent().attr("href"));
        $(tabSelector).parent().fadeOut("slow", function () {
            $(tabSelector).parent().parent().remove();
            $(divSelector).remove();
            refreshCheckNames();
            $(".checks li:first a").tab("show");
        });
    });

    $("#btnAdd").click(function (e) {
        checkNum++;
        $(".checks").append($.validator.format("<li><a href='#Check{0}' data-toggle='tab'></a></li>", checkNum));
        $(".check").append($.validator.format("<div class='tab-pane fade in active' id='Check{0}' data-value='0'></div>", checkNum));
        $(".checks li:last a").tab("show");
        refreshCheckNames();
    });

    $("#btnBack").click(function () {
        window.location = $.validator.format("{0}Order/Tables", root);
    })

    $("#btnSave").click(function () {
        SaveOrders();
    })
})

function refreshCheckNames() {
    $(".checks li a").each(function (index, value) {
        index++;
        $(this).html($.validator.format("Check#{0}<button class='close btn btn-primary' title='Remove this check' type='button'><i class='glyphicon glyphicon-remove-circle'></button>", index));
    })
}

function deleteItself(object) {
    $(object).fadeOut("slow", function () {
        object.remove();
    });
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
    $(toggleObject).toggle();

    if ($(toggleObject).is(":visible"))
        $(thisObject).removeClass("glyphicon-plus").addClass("glyphicon-minus");
    else
        $(thisObject).removeClass("glyphicon-minus").addClass("glyphicon-plus");
}

function getProducts(id) {
    var container = $(".order");

    var active = $(".check").find(".active");
    if (active.length == 0) {
        $("#btnAdd").click();
        active = $(".check").find(".active");
    }
    var orderId = $(active).attr("data-value");
    var tableId = $("#TableId").val();
    if (typeof (orderId) == "undefined") orderId = 0;

    container.block();
    var jqxhr = $.get($.validator.format("{0}Order/ShowOrderItem", root), { "id": id, "uid": uid(), "orderId": orderId, "tableId": tableId }, "json")
.done(function (result) {
    toggleAll(false);
    $(active).append(result);
})
.fail(function () {
    message("Failed.", "error", "center");
})
.always(function () {
    container.unblock();
});
};

function SaveMenu() {
    var container = $(".container-order");
    container.block();
    var jqxhr = $.post($.validator.format("{0}Order/SaveMenu", root), { "TableId": tableId, "model": $.toJSON(model) }, "json")
                  .done(function (result) {
                  })
    .fail(function () {
        message("Failed.", "error", "center");
    })
    .always(function () {
        container.unblock();
    });
}

function SaveOrders() {
    var tableId = $("#TableId").val();
    var checks = new Array();
    $(".tab-content.check .tab-pane").each(function (index, element) {
        MenuItems = new Array();
        var menus = $.validator.format("#{0} .container-order-item", element.id)
        $(menus).each(function (i, item) {
            productItems = new Array();
            var associations = $(item).find(".panel").find(".association").find(".associationItem");
            $(associations).each(function (i, association) {
                var assosiationId = $(association).attr("data-value");
                var Items = $(association).find(".knopa.active").find("input");
                addOnsItems = new Array();
                $(Items).each(function (i, productItem) {
                    var type = $(productItem).attr("type");
                    addOnsItems.push({ "id": $(productItem).attr("value"), "type": ((type == "checkbox") ? 1 : 2) });
                });
                productItems.push({ "id": $(association).attr("data-value"), "Items": addOnsItems });
            });

            var menuModel = { "id": $(item).attr("id"), "Products": productItems };
            MenuItems.push(menuModel);
        })
        checks.push({ "id": $(element).attr("data-value"), "Menus": MenuItems });
    });

    var model = {
        "tableId": tableId,
        "Checks": checks
    };

    var container = $(".container-order");
    container.block();
    var jqxhr = $.post($.validator.format("{0}Order/SaveOrders", root), { "TableId": tableId, "model": $.toJSON(model) }, "json")
                  .done(function (result) {
                  })
    .fail(function () {
        message("Failed.", "error", "center");
    })
    .always(function () {
        container.unblock();
    });
}