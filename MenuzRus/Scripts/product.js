$(function () {
    $("#Menu_id").change(function () {
        window.location = "/Product/Index/" + $(this).val();
    })
})

/// ****** CATEGORY ***************///
function showCategoryMenu(id) {
    $(".btn-group.category").css("display", "none");
    $(".btn-group.item").css("display", "none");
    $(".btn-group.category[data-value=" + id + "]").css("display", "inline");
}

function editCategory(id) {
    $(".btn-group.category").css("display", "none");
    $(".btn-group.item").css("display", "none");
    var jqxhr = $.get("/Category/EditCategory/", { id: id })
                  .done(function (result) {
                      $("#modalEditForm").html(result);
                      $(".modalEditForm").modal("show");
                  })
    .fail(function () {
    })
    .always(function () {
    });
}

function deleteCategory(id) {
    $(".btn-group.category").css("display", "none");
    $(".btn-group.item").css("display", "none");
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
                var jqxhr = $.post("/Category/DeleteCategory/", { id: id })
                                 .done(function (result) {
                                     message("Category successfully deletes.", "success", "center");
                                     window.location = "/Product/Index/" + $("#Menu_id").val();
                                 })
                   .fail(function () {
                       message("Delete category failed.", "error", "center");
                   })
                   .always(function () {
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
function showItemMenu(id) {
    $(".btn-group.category").css("display", "none");
    $(".btn-group.item").css("display", "none");
    $(".btn-group.item[data-value=" + id + "]").css("display", "inline");
}

function editItem(id) {
    $(".btn-group.category").css("display", "none");
    $(".btn-group.item").css("display", "none");
    var jqxhr = $.get("/Item/EditItem/", { id: id })
                  .done(function (result) {
                      $("#modalEditForm").html(result);
                      $(".modalEditForm").modal("show");
                  })
    .fail(function () {
    })
    .always(function () {
    });
}

function deleteItem(id) {
    $(".btn-group.category").css("display", "none");
    $(".btn-group.item").css("display", "none");
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
                var jqxhr = $.post("/Item/DeleteItem/", { id: id })
                              .done(function (result) {
                                  message("Deleted successfully.", "success", "center");
                                  window.location = "/Product/Index/" + $("#Menu_id").val();
                              })
                .fail(function () {
                    message("Delete item failed.", "error", "center");
                })
                .always(function () {
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