var gridster;
var grid_size = 50;
var grid_margin = 10;
var block_params = {
    max_width: 3,
    max_height: 3
};
$(function () {
    gridster = $(".gridster ul").gridster({
        widget_margins: [grid_margin, grid_margin],
        widget_base_dimensions: [grid_size, grid_size],
        avoid_overlapped_widgets: true,
        helper: "clone",
        resize: {
            enabled: true
        },
        serialize_params: function ($w, wgd) {
            floorId = $("#Floor_id").val();
            return {
                TableId: $($w).attr("id"),
                Type: $($w).attr("data-type"),
                Name: $($w).attr("data-name"),
                FloorId: floorId,
                Col: wgd.col,
                Row: wgd.row,
                X: wgd.size_x,
                Y: wgd.size_y
            };
        }
    }).data("gridster");
    $(".floorArea").resizable({ grid: [1000, 1] });

    $("#btnSave").on("click", function () {
        saveTables();
    });

    addLayout();

    $("#Floor_id").change(function () {
        window.location = $.validator.format("{0}Floor/Index/", root) + $(this).val();
    })

    $("#btnNewFloor").click(function () {
        $("#Floor_id").val(0);
        $("#Floor_Name").val("");
        $(".floorTitle").html("New floor");
        $(".floorEditForm").modal("show");
    })

    $("#btnEditFloor").click(function () {
        $(".floorTitle").html("Edit floor");
        $(".floorEditForm").modal("show");
    })

    $("#btnDeleteFloor").click(function () {
        noty({
            layout: "center",
            type: "error",
            killer: true,
            model: true,
            text: "Floor <em><strong>" + $("#Floor_Name").val() + "</strong></em> will be deleted.<br />Would you like to continue ?",
            buttons: [{
                addClass: "btn btn-danger", text: "Delete", onClick: function ($noty) {
                    $noty.close();
                    deleteFloor($("#Floor_id").val());
                }
            },
              {
                  addClass: "btn btn-default", text: "Cancel", onClick: function ($noty) {
                      $noty.close();
                  }
              }
            ]
        });
    })

    $("#btnCancelFloor").click(function () {
        $("#Floor_Name").val($("#Floor_id option:selected").text());
    })

    $("#btnSaveFloor").click(function () {
        $(".floorEditForm").modal("hide");
        editFloor($("#Floor_id").val(), $("#Floor_Name").val());
    })

    $("#btnSaveTable").click(function () {
        var element = $($.validator.format("#{0}", $("#currentTableId").val()));
        element.removeClass(element.attr("data-type"));
        element.addClass($("#ddlShape").val());
        element.attr("data-type", $("#ddlShape").val());
        element.attr("data-name", $("#TableName").val());
        element.children(".tableName").html(element.attr("data-name"));
        $(".tableEditForm").modal("hide");
    })
});

function addLayout() {
    gridster.remove_all_widgets();
    var serialization = $.parseJSON($("#Floor_Layout").val());

    $.each(serialization, function () {
        var deleteButton = "<span onclick='javascript:editTable($(this).parent().parent());' class='editTable glyphicon glyphicon-pencil'></span>";
        var editButton = "<span onclick='javascript:deleteTable($(this).parent().parent());' class='deleteTable glyphicon glyphicon-trash'></span>";
        var plusButton = "<span onclick='javascript:copyTable($(this).parent().parent());' class='copyTable glyphicon glyphicon-plus'></span>";
        var toolbar = $.validator.format("<div class='toolbar hide shadow'>{0}{1}{2}</div>", deleteButton, editButton, plusButton);
        var elementName = $.validator.format("<div class='tableName label label-default shadow'>{0}</div>", this.Name);
        var element = $.validator.format("<li id='{0}' data-name='{2}' data-type='{1}' class='shape {1}' onmouseleave='javascript:showTools(this, false)' onmouseover='javascript:showTools(this, true)'>{3}{4}</li>", this.id, this.Type, this.Name, toolbar, elementName);
        gridster.add_widget(element, this.X, this.Y, this.Col, this.Row);
        refreshTotal();
    });
}

function addNewTable(style) {
    var name = uid();
    var deleteButton = "<span onclick='javascript:editTable($(this).parent().parent());' class='editTable glyphicon glyphicon-pencil'></span>";
    var editButton = "<span onclick='javascript:deleteTable($(this).parent().parent());' class='deleteTable glyphicon glyphicon-trash'></span>";
    var plusButton = "<span onclick='javascript:copyTable($(this).parent().parent());' class='copyTable glyphicon glyphicon-plus'></span>";
    var toolbar = $.validator.format("<div class='toolbar hide shadow'>{0}{1}{2}</div>", deleteButton, editButton, plusButton);
    var elementName = $.validator.format("<div class='label label-default shadow'>{0}</div>", name);
    var element = $.validator.format("<li id='{4}' data-name='{1}' data-type='{0}' data-sizex='1' data-sizey='1' class='shape {0}' onmouseleave='javascript:showTools(this, false)' onmouseover='javascript:showTools(this, true)'>{2}{3}</li>", style, name, toolbar, elementName, shortGuid());
    gridster.add_widget(element, 1, 1);
    refreshTotal();
}

function deleteTable(element) {
    gridster.remove_widget(element);
    refreshTotal();
}

function copyTable(element) {
    var newElement = element.clone();
    gridster.add_widget(newElement);
    newElement.attr("data-type", element.attr("data-type"));
    newElement.attr("data-name", element.attr("data-name"));
    newElement.attr("data-sizex", element.attr("data-sizex"));
    newElement.attr("data-sizey", element.attr("data-sizey"));
    newElement.attr("id", shortGuid());
    refreshTotal();
}

function editTable(element) {
    $(".tableTitle").html("Edit table");
    $("#ddlShape").val(element.attr("data-type"));
    $("#TableName").val(element.attr("data-name"));
    $("#currentTableId").val(element.attr("id"));
    $(".tableEditForm").modal("show");
}

function showTools(element, show) {
    if (show) {
        $(element).children(".toolbar").removeClass("hide");
    }
    else {
        $(element).children(".toolbar").addClass("hide");
    }
}

function saveTables() {
    var container = $(".container-floor");
    container.block();
    var postData = JSON.stringify(gridster.serialize());
    var jqxhr = $.post($.validator.format("{0}Floor/SaveTables", root), { "tables": postData }, "json")
                  .done(function (result) {
                      message("Save successfully.", "success", "topCenter");
                  })
    .fail(function () {
        message("Save tables failed.", "error", "topCenter");
    })
    .always(function () {
        container.unblock();
    });
}

function refreshTotal() {
    $(".tables.badge").html(gridster.$widgets.length);
}
/// ****** FLOOR ********///
function editFloor(id, name) {
    if (id == null) id = 0;
    var postData = { id: id, name: name };
    var jqxhr = $.post($.validator.format("{0}Floor/SaveFloor/", root), postData)
                  .done(function (result) {
                      message("Save successfully.", "success", "topCenter");
                      window.location = $.validator.format("{0}Floor/Index/", root) + result;
                  })
    .fail(function () {
        message("Save floor failed.", "error", "topCenter");
    })
    .always(function () {
    });
}

function deleteFloor(id) {
    if (id == null) id = 0;
    var postData = { id: id };
    var jqxhr = $.post($.validator.format("{0}Floor/DeleteFloor/", root), postData)
                  .done(function (result) {
                      message("Menu deleted successfully.", "success", "topCenter");
                      window.location = $.validator.format("{0}Floor", root);
                  })
    .fail(function () {
        message("Delete floor failed.", "error", "topCenter");
    })
    .always(function () {
    });
}