$(function () {
})

/// ****** CATEGORY ***************///
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
        modal: true,
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
    editItem(id, 1);
}

function editItem(id, categoryId) {
    var jqxhr = $.get($.validator.format("{0}Item/EditItem/", root), { id: id, type: $("#CategoryType").val() })
    .done(function (result) {
        $("#modalEditForm").html(result);
        if (id == 0) {
            $("#CategoryId").val(categoryId);
        }
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