$(function () {
    $("span#popover").popover({
        placement: "right",
        html: true,
        trigger: "hover",
    });
})

/// ****** User ***************///
function collapseUser(thisObject, toggleObject) {
    $(toggleObject).toggle();

    if ($(toggleObject).is(":visible"))
        $(thisObject).removeClass("glyphicon-plus").addClass("glyphicon-minus");
    else
        $(thisObject).removeClass("glyphicon-minus").addClass("glyphicon-plus");
}

function showUserMenu(id, object) {
    $(object).hide();
    $(".btn-group").hide();
    $(".btn-group[data-value=" + id + "]").show();
}

function editUser(id) {
    $(".btn-group").hide();
    var jqxhr = $.get("/User/EditUser/", { id: id })
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

function deleteUser(id) {
    $(".btn-group").hide();
    var name = $(".user[id=user_" + id + "]").html();
    noty({
        layout: "center",
        type: "error",
        killer: true,
        model: true,
        text: "User <em><strong>" + name + "</strong></em> will be deleted.<br />Would you like to continue ?",
        buttons: [{
            addClass: 'btn btn-danger', text: 'Delete', onClick: function ($noty) {
                $noty.close();
                var jqxhr = $.post("/User/DeleteUser/", { id: id })
                                 .done(function (result) {
                                     message("Category successfully deletes.", "success", "center");
                                     window.location = "/Users/Index/";
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