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
        helper: "clone",
        resize: {
            enabled: true
        },
        serialize_params: function ($w, wgd) {
            return {
                TableId: $($w).attr("id"),
                Type: $($w).attr("data-type"),
                Name: $($w).attr("data-name"),
                Col: wgd.col,
                Row: wgd.row,
                X: wgd.size_x,
                Y: wgd.size_y
            };
        }
    }).data("gridster");
    $(".floorArea").resizable({ grid: [1000, 1] });

    $("#btnSerialize").on("click", function () {
        saveTables();
    });

    $("#Floor_id").change(function () {
        window.location = "/Floor/Index/" + $(this).val();
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
});

function addTable(style) {
    var name = uid();
    var deleteButton = "<span onclick='javascript:editTable($(this).parent());' class='editTable glyphicon glyphicon-pencil'></span>";
    var editButton = "<span onclick='javascript:deleteTable($(this).parent());' class='deleteTable glyphicon glyphicon-trash'></span>";
    var elementName = $.validator.format("<span class='label label-default'>{0}</span>", name);
    var element = $.validator.format("<li id='{4}' data-name='{5}' data-type='{0}' data-sizex='1' data-sizey='1' class='shape {0}'>{1}{2}{3}</li>", style, editButton, deleteButton, elementName, shortGuid(), name);
    gridster.add_widget(element, 1, 1);
    refreshTotal();
}

function deleteTable(element) {
    gridster.remove_widget(element);
    refreshTotal();
}

function saveTables() {
    var postData = JSON.stringify(gridster.serialize());
    var jqxhr = $.post("/Floor/SaveTables", { "tables": postData }, "json")
                  .done(function (result) {
                      message("Save successfully.", "success", "center");
                  })
    .fail(function () {
        message("Save tables failed.", "error", "center");
    })
    .always(function () {
    });
}

function refreshTotal() {
    $(".tables.badge").html(gridster.$widgets.length);
}
/// ****** FLOOR ********///
function editFloor(id, name) {
    if (id == null) id = 0;
    var postData = { id: id, name: name };
    var jqxhr = $.post("/Floor/SaveFloor/", postData)
                  .done(function (result) {
                      message("Save successfully.", "success", "center");
                      window.location = "/Floor/Index/" + result;
                  })
    .fail(function () {
        message("Save floor failed.", "error", "center");
    })
    .always(function () {
    });
}

function deleteFloor(id) {
    if (id == null) id = 0;
    var postData = { id: id };
    var jqxhr = $.post("/Floor/DeleteFloor/", postData)
                  .done(function (result) {
                      message("Menu deleted successfully.", "success", "center");
                      window.location = "/Floor";
                  })
    .fail(function () {
        message("Delete floor failed.", "error", "center");
    })
    .always(function () {
    });
}