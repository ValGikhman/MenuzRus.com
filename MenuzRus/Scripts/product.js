$(function () {
    $("#Menu_id").change(function () {
        window.location = $.validator.format("{0}Product/Index/{1}", root, $(this).val());
    })

    $("span#popover").popover({
        placement: "right",
        html: true,
        trigger: "hover",
    });
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
    $(".btn-group").hide();
    $(".btn-group[data-value=" + id + "]").show();
}

function editCategory(id) {
    $(".btn-group").hide();
    var jqxhr = $.get($.validator.format("{0}Category/EditCategory/", root), { id: id })
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
    $(".btn-group").hide();
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
                                     message("Category successfully deletes.", "success", "center");
                                     window.location = $.validator.format("{0}Product/Index/{1}", root, $("#Menu_id").val());
                                 })
                   .fail(function () {
                       message("Delete category failed.", "error", "center");
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
    $(".btn-group").hide();
    $(".btn-group[data-value=" + id + "]").show();
}

function editItem(id) {
    $(".btn-group").hide();
    var jqxhr = $.get($.validator.format("{0}Item/EditItem/", root), { id: id })
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
    $(".btn-group").hide();
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
                                  message("Deleted successfully.", "success", "center");
                                  window.location = $.validator.format("{0}Product/Index/", root) + $("#Menu_id").val();
                              })
                .fail(function () {
                    message("Delete item failed.", "error", "center");
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