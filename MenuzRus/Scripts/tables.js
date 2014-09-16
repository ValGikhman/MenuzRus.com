var gridster;
var grid_size = 50;
var grid_margin = 10;
var block_params = {
    max_width: 3,
    max_height: 3
};
$(function () {
    addLayout();

    $("#floor li a").click(function () {
        $("#btnFloor").text($(this).text());
        window.location = $.validator.format("{0}Order/Tables/{1}", root, $(this).attr("data-value"));
    });
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
    $(".tables").html(gridster.$widgets.length);
}

function watchCountdown(periods) {
    if ($.countdown.periodsToSeconds(periods) > 900) {
        $(this).addClass("alert-danger");
    }
}

function addTimers() {
    $(".shape").each(function () {
        var statusName = $(this).attr("data-status");
        if (statusName != "Closed") {
            $(this).find(".timer").countdown({ since: new Date($(this).attr("data-date") + " UTC"), compact: true, format: "HMS", onTick: watchCountdown });
        }
    });
}

function addLayout() {
    gridster.remove_all_widgets();
    var tables = $("#Floor_Layout").val();
    if (tables != "") {
        var serialization = $.parseJSON(tables);

        $.each(serialization, function () {
            var statusName = getStatusName(this.Status);
            var checks = getChecks(this.Checks);
            var elementName = $.validator.format("<div class='tableNameStatus label alert-{1} shadow'>{0} ({2})</div>", this.Name, getStatusColor(this.Status), statusName);
            var timer = $.validator.format("<div class='timer label alert-{0} shadow'></div>", getStatusColor(this.Status));
            var element = $.validator.format("<li id='{0}' data-name='{2}' data-type='{1}' data-status='{4}' data-date='{6}' class='shape {1}' onclick='javascript:viewTable({0})'>{3}{5}{7}</li>", this.id, this.Type, this.Name, elementName, statusName, checks, this.DateModified, timer);
            gridster.add_widget(element, this.X, this.Y, this.Col, this.Row);
            refreshTotal();
            addTimers();
        });
    }
}

function getChecks(checks) {
    var html = "<div style='padding:3px;'>";
    var ids = checks.split('|');
    $.each(ids, function (index, element) {
        if (element != "") {
            value = element.split(':');
            id = value[0];
            status = value[1];
            html += $.validator.format("<div class='checksBadges label alert-{2} shadow' data-status='{1}'>#{0}</div>", id, status, getStatusColor(parseInt(status)));
        }
    })
    html += "</div>";
    return html;
}