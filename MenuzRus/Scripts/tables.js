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

function viewTable(id) {
    window.location = $.validator.format("{0}Order/Table/{1}", root, id);
}

function refreshTotal() {
    $(".tables.badge").html($("#tables li:not([data-type='filler'])").length);
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
    $(".floorArea ul").empty();
    var tables = $("#Floor_Layout").val();

    if (tables != "") {
        var serialization = $.parseJSON(tables);

        $.each(serialization, function () {
            var selector = $.validator.format("#{0}", this.id);
            var statusName = getStatusName(this.Status);
            var checks = getChecks(this.Checks);
            var elementName = $.validator.format("<div class='tableNameStatus label alert-{1} shadow'>{0} ({2})</div>", this.Name, getStatusColor(this.Status), statusName);
            var timer = $.validator.format("<div class='timer label alert-{0} shadow'></div>", getStatusColor(this.Status));

            var element;

            if (this.Type == "filler") {
                // Do not hang anything on filler
                element = $.validator.format("<li id='{0}' data-type='{1}' class='tables shape {1}'>{2}</li>", this.id, this.Type, this.Name);
            }
            else {
                element = $.validator.format("<li id='{0}' data-name='{2}' data-type='{1}' data-status='{4}' data-date='{6}' class='tables shape {1}' onclick='javascript:viewTable({0})'>{3}{5}{7}</li>", this.id, this.Type, this.Name, elementName, statusName, checks, this.DateModified, timer);
            }

            $("#tables").append(element);
            $(selector).css("top", this.Top).css("left", this.Left).css("width", this.Width).css("height", this.Height).css("position", "absolute");
            refreshTotal();
            addTimers();

            $(".floorArea").width($("#Floor_Width").val()).height($("#Floor_Height").val());
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
            html += $.validator.format("<div class='checksBadges label label-{2} colorBlack shadow' data-status='{1}'>#{0}</div>", id, status, getStatusColor(parseInt(status)));
        }
    })
    html += "</div>";
    return html;
}