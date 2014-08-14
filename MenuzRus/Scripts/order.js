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
                Checks: $($w).attr("data-checks"),
                DateModified: $($w).attr("data-date")
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

function addTimers() {
    $(".shape").each(function () {
        $(this).find(".timer").countdown({ since: new Date($(this).attr("data-date")), compact: true, format: "HMS" });
    });
}

function addLayout() {
    gridster.remove_all_widgets();
    var serialization = $.parseJSON($("#Floor_Layout").val());

    $.each(serialization, function () {
        var color = "style='color:#999999;'background:transparent;";
        var dm = new Date(this.DateModified);
        var elapsed = dm.getHours() * 60 + dm.getMinutes();
        if (elapsed > 15) {
            color = "style='color:red;'";
        }
        var checks = getChecks(this.Checks);
        var elementName = $.validator.format("<div class='tableNameStatus label label-{1} shadow'>{0} ({2})</div>", this.Name, getStatusColor(this.Status), getStatusName(this.Status));
        var timer = $.validator.format("<div class='timer label label-{1} shadow' {1}></div>", getStatusColor(this.Status), color);
        var element = $.validator.format("<li id='{0}' data-name='{2}' data-type='{1}' data-status='{4}' data-date='{6}' class='shape {1}' onclick='javascript:viewTable({0})'>{3}{5}{7}</li>", this.id, this.Type, this.Name, elementName, this.Status, checks, this.DateModified, timer);
        gridster.add_widget(element, this.X, this.Y, this.Col, this.Row);
        refreshTotal();
        addTimers();
    });
}

function getChecks(checks) {
    var html = "<div style='padding:3px;'>";
    var ids = checks.split('|');
    $.each(ids, function (index, element) {
        if (element != "") {
            html += $.validator.format("<div class='checksBadges label label-success shadow'>#{0}</div>", element);
        }
    })
    html += "</div>";
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