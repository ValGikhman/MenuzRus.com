$(function () {
    if ($("#menuList").has("li").length > 0) {
        $("#btnEditMenu").show();
        $("#btnDeleteMenu").show();
        $("#btnMenuIt").show();
        $("#btnNewCategory").show();
        $("#btnNewItem").show();
    }
    else {
        $("#btnEditMenu").hide();
        $("#btnDeleteMenu").hide();
        $("#btnMenuIt").hide();
        $("#btnNewCategory").hide();
        $("#btnNewItem").hide();
    }

    /// ********** APPLY IMAGES ************///
    $(".img-wall").click(function () {
        var bg = "url(" + $(this).attr("src").replace("thumbnails/", "") + ") repeat";
        $(".wall").css("background", bg)
        var wallbackground = {
            Type: "wallbackground",
            Value: $(this).attr("src")
        };

        var jqxhr = $.post($.validator.format("{0}/MenuDesigner/SaveSettings/", root), wallbackground)
                  .done(function (result) {
                      message("Wall background saved successfully.", "success", "topCenter");
                  })
                  .fail(function (e) {
                      message("Wall background was not saved.", "error", "topCenter");
                  })
                  .always(function () {
                  });
    })

    $(".img-page").click(function () {
        var bg = "url(" + $(this).attr("src").replace("thumbnails/", "preview/") + ") no-repeat";
        $(".page").css("background", bg);
        var pagebackground = {
            Type: "pagebackground",
            Value: $(this).attr("src")
        };

        var jqxhr = $.post($.validator.format("{0}MenuDesigner/SaveSettings/", root), pagebackground)
                  .done(function (result) {
                      message("Page background saved successfully.", "success", "topCenter");
                  })
                  .fail(function () {
                      message("Page background was not saved.", "error", "topCenter");
                  })
                  .always(function () {
                  });
    });

    $(".category-group").sortable({
        stop: function (e, ui) {
            var cats = [];
            $(this).children().each(function (i) {
                var li = $(this);
                cats.push(li.attr("data-value"));
            });

            var postData = { ids: cats.join(), type: "Category" };
            var jqxhr = $.post($.validator.format("{0}MenuDesigner/SaveOrder/", root), postData)
                          .done(function (result) {
                              message("Sort order saved successfully.", "success", "topCenter");
                          })
            .fail(function () {
                message("Sort order was not saved.", "error", "topCenter");
            })
            .always(function () {
            });
        }
    }).disableSelection();

    $(".items-group").sortable({
        stop: function (e, ui) {
            var items = [];
            $(this).children().each(function (i) {
                var li = $(this);
                items.push(li.attr("data-value"));
            });

            var postData = { ids: items.join(), type: "Items" };
            var jqxhr = $.post($.validator.format("{0}MenuDesigner/SaveOrder/", root), postData)
                          .done(function (result) {
                              message("Sort order saved successfully.", "success", "topCenter");
                          })
            .fail(function () {
                message("Sort order was not saved.", "error", "topCenter");
            })
            .always(function () {
            });
        }
    }).disableSelection();

    $(".menu li a").click(function () {
        $("#btnMenu").text($(this).text());
        window.location = $.validator.format("{0}MenuDesigner/Index/{1}", root, $(this).attr("data-value"));
    });

    $("#btnNewMenu, #newMenu").click(function () {
        $("#Menu_id").val(0);
        $("#Menu_Name").val("");
        $(".menuTitle").html("New menu");
        $(".menuEditForm").modal("show");
    })

    $("#btnEditMenu, #editMenu").click(function () {
        $(".menuTitle").html("Edit menu");
        $(".menuEditForm").modal("show");
    })

    $("#btnDeleteMenu, #deleteMenu").click(function () {
        noty({
            layout: "center",
            type: "error",
            killer: true,
            model: true,
            text: "Menu <em><strong>" + $("#Menu_Name").val() + "</strong></em> will be deleted.<br />Would you like to continue ?",
            buttons: [{
                addClass: 'btn btn-danger', text: 'Delete', onClick: function ($noty) {
                    $noty.close();
                    deleteMenu($("#Menu_id").val());
                }
            },
              {
                  addClass: 'btn btn-default', text: 'Cancel', onClick: function ($noty) {
                      $noty.close();
                  }
              }
            ]
        });
    })

    $("#btnCancelMenu").click(function () {
        $("#Menu_Name").val($("#Menu_id option:selected").text());
    })

    $("#btnSaveMenu").click(function () {
        $(".menuEditForm").modal("hide");
        editMenu($("#Menu_id").val(), $("#Menu_Name").val());
    })

    $("#btnMenuIt").click(function () {
        window.open($.validator.format("{0}Menu/Index/", root) + $("#Menu_id").val(), "");
    })

    $("#btnSettings, #btnCloseSetting").click(function () {
        $(".panel-setting-body").toggle();
    })

    $("#btnShowHiddenItems").on("click", function () {
        var showItemsData = {
            Type: "ShowHiddenItems",
            Value: !$(this).hasClass("active")
        };
        saveSettings(showItemsData);
        if (showItemsData.Value)
            $(".inActiveItem").show();
        else
            $(".inActiveItem").hide();
    });

    $(".slider.font").slider({
        range: "max",
        min: 6,
        max: 36,
        animate: true,
        slide: function (event, ui) {
            var klass = "";
            switch (this.id) {
                case "CategoryFontSize":
                    klass = ".category";
                    break;
                case "CategoryDescriptionFontSize":
                    klass = ".categoryDescription";
                    break;
                case "ItemFontSize":
                    klass = ".item";
                    break;
                case "ItemDescriptionFontSize":
                    klass = ".itemDescription";
                    break;
                case "PriceFontSize":
                    klass = ".price";
                    break;
            }
            $(klass).css("font-size", ui.value);
            $($.validator.format("#{0}Badge", this.id)).html(ui.value);
        },
        stop: function (event, ui) {
            var fontsizeData = {
                Type: this.id,
                Value: ui.value
            };
            saveSettings(fontsizeData);
        }
    });

    $(".color-box").colpick({
        colorScheme: "light",
        layout: "hex",
        submit: 1,
        color: "ff8800",
        onChange: function (hsb, hex, rgb, el) {
            $(el).css("background-color", "#" + hex);
        },
        onSubmit: function (hsb, hex, rgb, el) {
            $(el).colpickHide();
            var klass = "";
            switch (el.id) {
                case "CategoryColor":
                    klass = ".category";
                    break;
                case "CategoryDescriptionColor":
                    klass = ".categoryDescription";
                    break;
                case "ItemColor":
                    klass = ".item";
                    break;
                case "ItemDescriptionColor":
                    klass = ".itemDescription";
                    break;
                case "PriceColor":
                    klass = ".price";
                    break;
            }

            var colorData = {
                Type: el.id,
                Value: "#" + hex
            };
            $(klass).css("color", colorData.Value);

            saveSettings(colorData);
        }
    });

    $(".categoryMenu").popover({
        placement: "top",
        html: true,
        trigger: "click",
        content: $(".btn-group.category").html(),
    });

    $(".itemMenu").popover({
        placement: "top",
        html: true,
        trigger: "click",
        content: $(".btn-group.item").html(),
    });

    applySettings();
})

/// ****** SETTINGS ***************///
function applySettings() {
    var bg = $.validator.format("url({1}Images/Backgrounds/Pages/preview/{0}) no-repeat", $("input[name='Settings[PageBackground]']").val(), root);
    if (bg != "")
        $(".page").css("background", bg);

    bg = $.validator.format("url({1}Images/Backgrounds/Wall/{0}) repeat", $("input[name='Settings[WallBackground]']").val(), root);
    if (bg != "")
        $(".wall").css("background", bg);

    // Color
    var kolor = $("input[name='Settings[CategoryColor]']").val();
    if (kolor != "")
        $(".category").css("color", kolor);

    kolor = $("input[name='Settings[CategoryDescriptionColor]']").val();
    if (kolor != "")
        $(".categoryDescription").css("color", kolor);

    kolor = $("input[name='Settings[ItemColor]']").val();
    if (kolor != "")
        $(".item").css("color", kolor);

    kolor = $("input[name='Settings[ItemDescriptionColor]']").val();
    if (kolor != "")
        $(".itemDescription").css("color", kolor);

    kolor = $("input[name='Settings[PriceColor]']").val();
    if (kolor != "")
        $(".price").css("color", kolor);

    // Font Size
    var fontsize = $("input[name='Settings[CategoryFontSize]']").val();
    if (fontsize != "") {
        $(".category").css("font-size", $.validator.format("{0}px", fontsize));
        $("#CategoryFontSize").slider("value", fontsize);
    }

    fontsize = $("input[name='Settings[CategoryDescriptionFontSize]']").val();
    if (fontsize != "") {
        $(".categoryDescription").css("font-size", $.validator.format("{0}px", fontsize));
        $("#CategoryDescriptionFontSize").slider("value", fontsize);
    }

    fontsize = $("input[name='Settings[ItemFontSize]']").val();
    if (fontsize != "") {
        $(".item").css("font-size", $.validator.format("{0}px", fontsize));
        $("#ItemFontSize").slider("value", fontsize);
    }

    fontsize = $("input[name='Settings[ItemDescriptionFontSize]']").val();
    if (fontsize != "") {
        $(".itemDescription").css("font-size", $.validator.format("{0}px", fontsize));
        $("#ItemDescriptionFontSize").slider("value", fontsize);
    }

    fontsize = $("input[name='Settings[PriceFontSize]']").val();
    if (fontsize != "") {
        $(".price").css("font-size", $.validator.format("{0}px", fontsize));
        $("#PriceFontSize").slider("value", fontsize);
    }

    // Others
    var showHiddenItems = $("input[name='Settings[ShowHiddenItems]']").val();
    if (showHiddenItems == "true") {
        $("#btnShowHiddenItems").addClass("active");
        $(".inActiveItem").show();
    }
    else {
        $(".inActiveItem").hide();
    }
}
/// ****** MENU ********///
function editMenu(id, name) {
    if (id == null) id = 0;
    var postData = { id: id, name: name };
    var jqxhr = $.post($.validator.format("{0}MenuDesigner/SaveMenu/", root), postData)
                  .done(function (result) {
                      message("Save successfully.", "success", "topCenter");
                      window.location = $.validator.format("{0}MenuDesigner/Index/", root) + result;
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
    var jqxhr = $.post($.validator.format("{0}MenuDesigner/DeleteMenu/", root), postData)
                  .done(function (result) {
                      message("Menu deleted successfully.", "success", "topCenter");
                      window.location = $.validator.format("{0}MenuDesigner", root);
                  })
    .fail(function () {
        message("Delete menu failed.", "error", "topCenter");
    })
    .always(function () {
    });
}

/// ****** CATEGORY ***************///
function addCategory() {
    editCategory(0);
}

function editCategory(object) {
    var id = $(object).parent().parent().parent().attr("data-value");
    $(".categoryMenu").popover('hide');
    $(".itemMenu").popover('hide');
    var jqxhr = $.get($.validator.format("{0}Category/EditCategory/", root), { id: id, type: $("#CategoryType").val() })
              .done(function (result) {
                  $("#modalEditForm").html(result);
                  $(".modalEditForm").modal("show");
              })
.fail(function () {
})
.always(function () {
});
}

function deleteCategory(object) {
    var id = $(object).parent().parent().parent().attr("data-value");
    $(".categoryMenu").popover('hide');
    $(".itemMenu").popover('hide');
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
                                     window.location = $.validator.format("{0}MenuDesigner/Index/{1}", root, $("#Menu_id").val());
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

function saveSettings(obj) {
    var jqxhr = $.post($.validator.format("{0}MenuDesigner/SaveSettings/", root), obj)
      .done(function (result) {
          var text = result; // Will contain error if exception
          if (result == "OK")
              text = "Settings successfully saved.";
          message(text, "success", "topCenter");
      })
      .fail(function () {
          message("Settings was not saved.", "error", "topCenter");
      })
      .always(function () {
      });
}

/// ****** ITEMS ***************///
function addItem() {
    editItem(0);
}

function editItem(object) {
    var id = $(object).parent().parent().parent().attr("data-value");
    $(".categoryMenu").popover('hide');
    $(".itemMenu").popover('hide');
    var jqxhr = $.get($.validator.format("{0}Item/EditItem/", root), { id: id, type: $("#CategoryType").val() })
                  .done(function (result) {
                      $("#modalEditForm").html(result);
                      $(".modalEditForm").modal("show");
                  })
    .fail(function () {
    })
    .always(function () {
    });
}

function associateItem(object) {
    var id = $(object).parent().parent().parent().attr("data-value");
    $(".categoryMenu").popover('hide');
    $(".itemMenu").popover('hide');
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

function deleteItem(object) {
    var id = $(object).parent().parent().parent().attr("data-value");
    $(".categoryMenu").popover('hide');
    $(".itemMenu").popover('hide');
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
                                  if (result == "OK") {
                                      message("Deleted successfully.", "success", "topCenter");
                                      window.location = $.validator.format("{0}MenuDesigner/Index/", root) + $("#Menu_id").val();
                                  }
                              })
                .fail(function () {
                    message("Delete item failed.", "error", "topCenter");
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