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
}

function saveInventoryItems() {
    var model = [];
    $(".items:not(.template)").each(function (i, e) {
        var values = "";
        $.each(e.selectedOptions, function (index, item) {
            values += $.validator.format("{0}:{1}:{2},", $(e).attr("data-value"), item.value, $(e).parent().find("button").text());
        });
        if (values != "")
            model.push(values);
    })

    var container = $("#modalEditForm");
    container.block();
    model = JSON.stringify(model);
    var jqxhr = $.post($.validator.format("{0}ItemProduct/SaveInventoryAssociatedItems", root), { "model": model }, "json")
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