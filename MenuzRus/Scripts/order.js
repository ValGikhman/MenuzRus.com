var gridster;
var grid_size = 50;
var grid_margin = 10;
var block_params = {
    max_width: 3,
    max_height: 3
};
$(function () {
    addLayout();

    $("#Floor_id").change(function () {
        window.location = $.validator.format("{0}Order/Tables/{1}", root, $(this).val());
    })
});

function initGridster() {
    gridster = $(".gridster ul").gridster({
        widget_margins: [grid_margin, grid_margin],
        widget_base_dimensions: [grid_size, grid_size],
        avoid_overlapped_widgets: true,
        helper: "clone",
        resize: {
            enabled: false
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
                Y: wgd.size_y,
                Status: $($w).attr("data-status"),
                Checks: $($w).attr("data-checks")
            };
        }
    }).data("gridster").disable();
}

function viewTable(id) {
    window.location = $.validator.format("{0}Order/Table/{1}", root, id);
}

function refreshTotal() {
    $(".tables.badge").html(gridster.$widgets.length);
}

function addLayout() {
    gridster.remove_all_widgets();
    var serialization = $.parseJSON($("#Floor_Layout").val());

    $.each(serialization, function () {
        var dermo = getChecks(this.Checks);
        var elementName = $.validator.format("<div class='tableName label label-{1} shadow'>{0} ({2})</div>", this.Name, getStatusColor(this.Status), getStatusName(this.Status));
        var element = $.validator.format("<li id='{0}' data-name='{2}' data-type='{1}' data-status='{4}' class='shape {1}' onclick='javascript:viewTable({0})'>{5}{3}</li>", this.id, this.Type, this.Name, elementName, this.Status, dermo);
        gridster.add_widget(element, this.X, this.Y, this.Col, this.Row);
        refreshTotal();
    });
}

function getChecks(checks) {
    var html = "";
    var ids = checks.split('|');
    $.each(ids, function (index, element) {
        if (element != "") {
            html += $.validator.format("<div class='checksBadges label label-success shadow'>#{0}</div>", element);
        }
    })
    return html;
}

function getStatusName(status) {
    switch (status) {
        case 1:
            return "Open";
            break;
        case 2:
            return "Working";
            break;
        case 3:
            return "Served";
            break;
        case 4:
            return "Closed";
            break;
    }
}

function getStatusColor(status) {
    switch (status) {
        case 1:
            return "default";
            break;
        case 2:
            return "warning";
            break;
        case 3:
            return "success";
            break;
        case 4:
            return "default";
            break;
    }
}