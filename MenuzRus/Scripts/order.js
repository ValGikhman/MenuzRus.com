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
                Y: wgd.size_y
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
        var elementName = $.validator.format("<div class='tableName label label-default shadow'>{0}</div>", this.Name);
        var element = $.validator.format("<li id='{0}' data-name='{2}' data-type='{1}' class='shape {1}' onclick='javascript:viewTable({0})'>{3}</li>", this.id, this.Type, this.Name, elementName);
        gridster.add_widget(element, this.X, this.Y, this.Col, this.Row);
        refreshTotal();
    });
}