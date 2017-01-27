$(function () {
    $("#comments").listnav({
        includeAll: true,
        includeOther: false,
        flagDisabled: true,
        noMatchText: 'Nothing matched your filter, please click another letter.',
        showCounts: true,
        cookieName: "my-main-list",
        onClick: function (letter) { },
        prefixes: ["the", "a"]
    });

    $(".ln-letters").addClass("center");
    $(".ln-letters a").addClass("btn btn-success");

    $("#btnCancel").click(function () {
        $(".modalEditForm").modal("hide");
    })

    $("#btnSave").click(function () {
        var container = $("#modalEditForm");
        var text = $("#txtNewComment").val();
        var url = $.validator.format("{0}Comments/Save", root);
        container.block();
        $.ajax({
            async: true,
            url: url,
            data: { "commentText": text },
            type: "POST",
            success: function (result) {
                getAllComments();
            },
            error: function (e) {
                message("::CommentSave:: Failed.", "error", "topCenter");
            }
        });
    });

    refreshComments();
});

function toggleComment(element) {
    var container = $("#commentsList");
    var url;
    var parentId = $("#parentId").val();
    var commentType = $("#commentType").val();
    if (element.checked) {
        url = $.validator.format("{0}Comments/SaveComment", root);
    }
    else {
        url = $.validator.format("{0}Comments/DeleteComment", root);
    }
    container.block();
    $.ajax({
        async: true,
        url: url,
        data: { "id": element.id, "parentId": parentId, "type": commentType },
        type: "POST",
        success: function (result) {
            container.unblock();
            refreshComments();
        },
        error: function (e) {
            message("::toggleComment:: Failed.", "error", "topCenter");
        }
    });
};

function toggleSaveButton(tabSelected) {
    $("#btnSave").addClass("hide");
    if (tabSelected == "AddNew") {
        $("#btnSave").removeClass("hide");
    }
    return false;
};

function refreshComments() {
    var values = $(".commentButton:checked").map(function () {
        return this.value;
    }).get();

    $("#showComments").html(values.join(" | "));
};

function getAllComments() {
    var jqxhr = $.get($.validator.format("{0}Comments/GetComments/", root), { "parentId": $("#parentId").val(), "type": $("#commentType").val() })
        .done(function (result) {
            $("#modalEditForm").html(result);
        })
        .fail(function () {
        })
        .always(function () {
        });
}