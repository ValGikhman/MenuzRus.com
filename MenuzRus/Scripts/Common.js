var root = location.protocol + '//' + location.host;
$(function () {
    $.blockUI.defaults.message = $("#bowlG");
    $.blockUI.defaults.css = " border: '0px none transparent; ";
    //$(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);

    $(".page").sortable();
    setMenu();
});

function setMenu() {
    if (window.location.href.indexOf("/Login") == -1)
        $(".menuAlways").show();
    else
        $(".menuAlways").hide();

    if (window.location.href.indexOf("/YourMenu") > -1)
        $(".menuDesigner").removeClass("hide").slideDown("fast");
    else
        $(".menuDesigner").addClass("hide").slideDown("fast");
};

function deleteImage() {
    $(".preview").attr("src", "");
    $("#ImageUrl").val("");
};

function initImageUpload(element) {
    $(".preview").click(function (e) {
        element.block();
        $("#Image").click();
    });

    $("#Image").change(function (e) {
        preViewImage(e, $(".preview"));
        element.unblock();
    });
}

function preViewImage(e, element) {
    if (e.target.files[0].type.match('image.*')) {
        if (typeof FileReader == "undefined") return true;
        if (e.target.files && e.target.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                element.attr("src", e.target.result);
            };
            reader.readAsDataURL(e.target.files[0]);
            element.unblock();
        }
    }
}

function message(text, type, position) {
    noty({
        text: text,
        layout: position,
        type: type,
        template: '<div class="noty_message"><span class="noty_text"></span><div class="noty_close"></div></div>',
        timeout: "5000",
        maxVisible: 1,
        killer: true,
        closable: true,
        closeOnSelfClick: true,
        closeWith: ["click", "button"]
    });
}

/// ****** MENU ********///
function editMenu(id, name) {
    if (id == null) id = 0;
    var postData = { id: id, name: name };
    var jqxhr = $.post("/YourMenu/SaveMenu/", postData)
                  .done(function (result) {
                      message("Save successfully.", "success", "topCenter");
                      window.location = "/YourMenu/Index/" + result;
                  })
    .fail(function () {
        message("Save menu failed.", "error", "topCenter");
    })
    .always(function () {
    });
}

function deleteMenu(id) {
    if (id == null) id = 0;
    var postData = { id: id };
    var jqxhr = $.post("/YourMenu/DeleteMenu/", postData)
                  .done(function (result) {
                      message("Menu deleted successfully.", "success", "topCenter");
                      window.location = "/YourMenu";
                  })
    .fail(function () {
        message("Delete menu failed.", "error", "topCenter");
    })
    .always(function () {
    });
}

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
                                     message("Category successfully deletes.", "success", "topCenter");
                                     window.location = "/YourMenu/Index/" + $("#Menu_id").val();
                                 })
                   .fail(function () {
                       message("Delete category failed.", "error", "topCenter");
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