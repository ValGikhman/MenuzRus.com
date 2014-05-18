﻿var model = [];

$(function () {
});

function initChosen() {
    $("select[id^='itemProduct']").chosen({ width: "350px", display_disabled_options: false });
}

function saveItems() {
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
    var jqxhr = $.post("/ItemProduct/SaveAssociatedItems", { "model": model }, "json")
                  .done(function (result) {
                      message("Save successfully.", "success", "center");
                      $(".itemProductClose").click();
                  })
    .fail(function () {
        message("Save item association failed.", "error", "center");
    })
    .always(function () {
        container.unblock();
    });
};

function addAssosiation() {
    var clone = $(".template").clone();
    clone.removeAttr("style").removeClass("template").css("margin-top", "25px");
    $(".container-item").append(clone);
    $(".container-item select:last").chosen({ width: "350px", display_disabled_options: false });
}

function deleteItself(object) {
    object.remove();
    var id = object.find("select").attr("data-value");
    var container = $("#modalEditForm");
    container.block();
    var jqxhr = $.post("/ItemProduct/DeleteItemProduct", { "id": id }, "json")
              .done(function (result) {
                  message("Delete successfully.", "success", "center");
              })
            .fail(function () {
                message("Delete item association failed.", "error", "center");
            })
            .always(function () {
                container.unblock();
            });
}

function toggleProductType(object) {
    if ($(object).text() == "Alternatives")
        $(object).text("Addons")
    else
        $(object).text("Alternatives")
}