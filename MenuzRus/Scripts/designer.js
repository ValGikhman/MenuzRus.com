$(function () {
    $(".menu li a").click(function () {
        $("#btnMenu").text($(this).text());
        window.location = $.validator.format("{0}Product/Index/{1}", root, $(this).attr("data-value"));
    });

    $("span#popover").popover({
        placement: "right",
        html: true,
        trigger: "hover",
    });

    if ($("#floor").has("li").length == 0) {
        $("#btnNewCategory").hide();
    }
    else {
        $("#btnNewCategory").show();
    }

    if ($(".table tbody").has(".categoryRow").length == 0) {
        $("#btnNewItem").hide();
    }
    else {
        $("#btnNewItem").show();
    }
})

/// ****** CATEGORY ***************///
function collapseCategory(thisObject, toggleObject) {
    $(toggleObject).toggle();

    if ($(toggleObject).is(":visible"))
        $(thisObject).removeClass("glyphicon-plus").addClass("glyphicon-minus");
    else
        $(thisObject).removeClass("glyphicon-minus").addClass("glyphicon-plus");
}

function showCategoryMenu(id, object) {
    $(object).hide();
    $(".btn-group.popup-category").hide();
    $(".btn-group.popup-category[data-value=" + id + "]").show();
}

function editCategory(id) {
    $(".btn-group.popup-category").hide();
    var jqxhr = $.get($.validator.format("{0}Category/EditCategory/", root), { id: id, type: $("#CategoryType").val() })
                  .done(function (result) {
                      $("#modalEditForm").html(result);
                      $(".modalEditForm").modal("show");
                  })
    .fail(function () {
    })
    .always(function () {
        $('.glyphicon-cog').show();
    });
}

function deleteCategory(id) {
    $(".btn-group.popup-category").hide();
    var name = $(".category[id=category_" + id + "]").html();
    noty({
        layout: "center",
        type: "error",
        killer: true,
        model: true,
        text: "Category <em><strong>" + name + "</strong></em> will be deleted.<br />Would you like to continue ?",
        buttons: [{
            addClass: 'btn btn-danger', text: 'Delete', onClick: function ($noty) {
                $noty.close();
                var jqxhr = $.post($.validator.format("{0}Category/DeleteCategory/", root), { id: id })
                                 .done(function (result) {
                                     message("Category successfully deletes.", "success", "topCenter");
                                     window.location = $.validator.format("{0}Product/Index/{1}", root, $("#Menu_id").val());
                                 })
                   .fail(function () {
                       message("Delete category failed.", "error", "topCenter");
                   })
                   .always(function () {
                       $('.glyphicon-cog').show();
                   });
            }
        },
          {
              addClass: 'btn btn-default', text: 'Cancel', onClick: function ($noty) {
                  $noty.close();
              }
          }
        ]
    });
}

/// ****** ITEMS ***************///
function showItemMenu(id, object) {
    $(object).hide();
    $(".btn-group.popup-item").hide();
    $(".btn-group.popup-item[data-value=" + id + "]").show();
}

function editItem(id) {
    $(".btn-group.popup-item").hide();
    var jqxhr = $.get($.validator.format("{0}Item/EditItem/", root), { id: id, type: $("#CategoryType").val() })
                  .done(function (result) {
                      $("#modalEditForm").html(result);
                      $(".modalEditForm").modal("show");
                  })
    .fail(function () {
    })
    .always(function () {
        $('.glyphicon-cog').show();
    });
}

function deleteItem(id) {
    $(".btn-group.popup-item").hide();
    var name = $(".item[id=item_" + id + "]").html();
    noty({
        layout: "center",
        type: "error",
        killer: true,
        model: true,
        text: "Item <em><strong>" + name + "</strong></em> will be deleted.<br />Would you like to continue ?",
        buttons: [{
            addClass: 'btn btn-danger', text: "Delete", onClick: function ($noty) {
                $noty.close();
                var jqxhr = $.post($.validator.format("{0}Item/DeleteItem/", root), { id: id })
                              .done(function (result) {
                                  message("Deleted successfully.", "success", "topCenter");
                                  window.location = $.validator.format("{0}Product/Index/", root) + $("#Menu_id").val();
                              })
                .fail(function () {
                    message("Delete item failed.", "error", "topCenter");
                })
                .always(function () {
                    $('.glyphicon-cog').show();
                });
            }
        },
          {
              addClass: 'btn btn-default', text: 'Cancel', onClick: function ($noty) {
                  $noty.close();
              }
          }
        ]
    });
}