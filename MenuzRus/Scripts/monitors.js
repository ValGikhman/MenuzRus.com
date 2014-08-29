$(function () {
    addTimers();
    setTableStatus();
    setCheckStatus();
})

function setTableStatus() {
    $(".table").each(function () {
        var tableName = $(this).find(".tableNameStatus");
        var text = $(tableName).text();
        var statusName = getStatusName(parseInt($(this).attr("data-status")));
        var statusColor = getStatusColor(parseInt($(this).attr("data-status")));
        $(tableName).addClass($.validator.format("alert-{0}", statusColor));
        $(tableName).text($.validator.format("{0} - {1}", text, statusName));
        $(".timer").addClass($.validator.format("alert-{0}", statusColor));
    });
}

function setCheckStatus() {
    $(".checks").each(function () {
        var text = $(this).text();
        var statusName = getStatusName(parseInt($(this).attr("data-status")));
        var statusColor = getStatusColor(parseInt($(this).attr("data-status")));
        $(this).addClass($.validator.format("alert-{0}", statusColor));
    });
}

function addTimers() {
    $(".table").each(function () {
        var statusName = $(this).attr("data-status");
        if (statusName != "Closed") {
            $(this).find(".timer").countdown({ since: new Date($(this).attr("data-date") + " UTC"), compact: true, format: "HMS", onTick: watchCountdown });
        }
    });
}

function watchCountdown(periods) {
    if ($.countdown.periodsToSeconds(periods) > 900) {
        $(this).addClass("alert-danger");
    }
}