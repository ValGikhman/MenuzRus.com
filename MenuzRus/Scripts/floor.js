var dragSettings = {
    grid: [20, 20],
    containment: ".floorArea",
    cursor: "move",
    span: true,
    scroll: true,
    scrollSensitivity: 100
};

var resizeSettings = { grid: [20, 20] };

$(function () {
    addLayout();

    $("#tables").draggable(dragSettings);

    $(".floorArea").resizable(resizeSettings);

    $("#btnSave").on("click", function () {
        saveTables();
    });

    $("#floor li a").click(function () {
        $("#btnFloor").text($(this).text());
        window.location = $.validator.format("{0}Floor/Index/{1}", root, $(this).attr("data-value"));
    });

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
        saveFloor($("#Floor_id").val(), $("#Floor_Name").val());
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

    if ($("#Floor_id").val() == 0) {
        $(".addMenu").hide();
    }
    else {
        $(".addMenu").show();
    }
});

function addLayout() {
    $(".floorArea ul").empty();
    var tables = $("#Floor_Layout").val();

    if (tables != "") {
        var serialization = $.parseJSON(tables);

        $.each(serialization, function () {
            var selector = $.validator.format("#{0}", this.id);
            var deleteButton = "<span onclick='javascript:editTable($(this).parent().parent());' class='editTable glyphicon glyphicon-pencil'></span>";
            var editButton = "<span onclick='javascript:deleteTable($(this).parent().parent());' class='deleteTable glyphicon glyphicon-trash'></span>";
            var plusButton = "<span onclick='javascript:copyTable($(this).parent().parent());' class='copyTable glyphicon glyphicon-plus'></span>";
            var toolbar = $.validator.format("<div class='toolbar hide shadow'>{0}{1}{2}</div>", deleteButton, editButton, plusButton);
            var elementName = $.validator.format("<div class='tableName label label-default shadow'>{0}</div>", this.Name);
            var element = $.validator.format("<li id='{0}' data-name='{2}' data-type='{1}' class='tables shape {1}' onmouseleave='javascript:showTools(this, false)' onmouseover='javascript:showTools(this, true)'>{3}{4}</li>", this.id, this.Type, this.Name, toolbar, elementName);
            $("#tables").append(element);
            $(selector).css("top", this.Top).css("left", this.Left).css("width", this.Width).css("height", this.Height).css("position", "absolute").resizable(resizeSettings).draggable(dragSettings);
        });
        refreshTotal();
    }
}

function addNewTable(style) {
    var name = uid();
    var id = shortGuid();
    var selector = $.validator.format("#new-{0}", id);
    var deleteButton = "<span onclick='javascript:editTable($(this).parent().parent());' class='editTable glyphicon glyphicon-pencil'></span>";
    var editButton = "<span onclick='javascript:deleteTable($(this).parent().parent());' class='deleteTable glyphicon glyphicon-trash'></span>";
    var plusButton = "<span onclick='javascript:copyTable($(this).parent().parent());' class='copyTable glyphicon glyphicon-plus'></span>";
    var toolbar = $.validator.format("<div class='toolbar hide shadow'>{0}{1}{2}</div>", deleteButton, editButton, plusButton);
    var elementName = $.validator.format("<div class='tableName label label-default shadow'>{0}</div>", name);
    var element = $.validator.format("<li id='new-{0}' data-name='{1}' data-type='{2}' class='tables shape {2}' onmouseleave='javascript:showTools(this, false)' onmouseover='javascript:showTools(this, true)'>{3}{4}</li>", id, name, style, toolbar, elementName);
    $("#tables").append(element);
    $(selector).css("width", "100px").css("height", "100px").resizable(resizeSettings).draggable(dragSettings);
    refreshTotal();
}

function deleteTable(element) {
    $(element).remove();
    refreshTotal();
}

function copyTable(element) {
    var newElement = element.clone();
    $(".floorArea ul").append(element);
    newElement.attr("name", name = uid());
    newElement.attr("id", $.validator.format("new-{0}", shortGuid()));
    newElement.attr("data-type", element.attr("data-type"));
    newElement.attr("data-name", element.attr("data-name"));
    $(newElement).find(".tableName").html(element.attr("data-name"));
    $("#tables").append(newElement);
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
    var postData;
    var values = "";
    var selector = $("#tables li");

    var id;
    var name;
    var type;

    var top = 0;
    var left = 0;
    var width = 0;
    var height = 0;

    container.block();
    values += "[";

    $.each(selector, function (index, item) {
        if ($(item).attr("id").indexOf("new-") == 0) {
            $(item).attr("id", "0");
        }

        id = $(item).attr("id");
        name = $(item).attr("data-name");
        type = $(item).attr("data-type");

        top = $(item).css("top").replace("px", "");
        left = $(item).css("left").replace("px", "");
        width = $(item).css("width").replace("px", "");
        height = $(item).css("height").replace("px", "");

        prefix = index > 0 ? ", " : "";
        values += $.validator.format("{0}{'id':'{1}', 'Name':'{2}', 'Type':'{3}', 'Top':'{4}', 'Left':'{5}', 'Width':'{6}', 'Height':'{7}'}", prefix, id, name, type, top, left, width, height);
    });

    values += "]";

    var jqxhr = $.post($.validator.format("{0}Floor/SaveTables", root), { "model": values }, "json")
                  .done(function (result) {
                      message("Save successfully.", "success", "topCenter");
                      window.location = $.validator.format("{0}Floor/Index/", root) + result;
                  })
    .fail(function () {
        message("Save tables failed.", "error", "topCenter");
    })
    .always(function () {
        container.unblock();
    });
}

function refreshTotal() {
    $(".tables.badge").html($("#tables li").length);
}
/// ****** FLOOR ********///
function saveFloor(id, name) {
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