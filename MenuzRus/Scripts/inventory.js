$(function () {
});

function addInventoryAssosiation() {
    var clone = $(".template").clone();
    clone.removeAttr("style").removeClass("template").css("margin-top", "25px");
    $(".container-item").append(clone);
    $(".container-item select:last").chosen({ width: "250px", display_disabled_options: false });
    $(".chosen-container").addClass("shadow");
}

function initInventoryChosen() {
    $("select[id^='itemInventory']").chosen({ width: "250px", display_disabled_options: false }).addClass("shadow");
    $(".chosen-container").addClass("shadow");
}

function deleteInventoryAssociation(object) {
    $(object).fadeOut("slow", function () {
        object.remove();
    });

    var id = object.find("select").attr("data-value");
    var container = $("#modalEditForm");
    container.block();
    var jqxhr = $.post($.validator.format("{0}Inventory/DeleteInventoryAssociation", root), { "id": id }, "json")
              .done(function (result) {
                  message("Delete successfully.", "success", "topCenter");
              })
            .fail(function () {
                message("Delete item association failed.", "error", "topCenter");
            })
            .always(function () {
                container.unblock();
            });
}

function saveInventoryItems() {
    var model = [];
    $(".items:not(.template)").each(function (i, e) {
        var values = "";
        // id, Inventory, Qty
        values += $.validator.format("{0}:{1}:{2},", $(e).find("select").attr("data-value"), $(e).find("select").val(), $(e).find("#Quantity").val());

        if (values != "")
            model.push(values);
    })

    var container = $("#modalEditForm");
    container.block();
    model = JSON.stringify(model);
    var jqxhr = $.post($.validator.format("{0}Inventory/SaveInventoryAssociation", root), { "model": model }, "json")
                  .done(function (result) {
                      message("Save successfully.", "success", "topCenter");
                      $(".itemInventoryClose").click();
                  })
    .fail(function () {
        message("Save inventory item association failed.", "error", "topCenter");
    })
    .always(function () {
        container.unblock();
    });
};