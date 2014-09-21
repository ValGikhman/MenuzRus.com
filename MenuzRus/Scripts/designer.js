$(function () {
    $("span#popover").popover({
        placement: "right",
        html: true,
        trigger: "hover",
    });

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

function editCategory(id) {
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
    var name = $(".category[data-value=" + id + "]").html();
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
function editItem(id) {
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
    var name = $(".item[data-value=" + id + "]").html();
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

function associateItem(id) {
    var jqxhr = $.get($.validator.format("{0}ItemProduct/ItemProductAssociate/", root), { id: id, type: $("#CategoryType").val() })
.done(function (result) {
    $("#modalEditForm").html(result);
    $(".modalEditForm").modal("show");
})
.fail(function () {
})
.always(function () {
});
}