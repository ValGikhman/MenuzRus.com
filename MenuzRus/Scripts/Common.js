var root = location.protocol + '//' + location.host;
$(function () {
    $.blockUI.defaults.message = $("#bowlG");
    $.blockUI.defaults.css = " border: '0px none transparent; ";
    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);

    $(".tree li:has(ul)").addClass("parent_li");
    $(".tree li.parent_li > span").on("click", function (e) {
        var children = $(this).parent("li.parent_li").find(" > ul > li");
        if (children.is(":visible")) {
            children.hide("fast");
            //.find(" > i").addClass("icon-plus-sign").removeClass("icon-minus-sign");
        } else {
            children.show("fast");
            //$(this).find(" > i").addClass("icon-minus-sign").removeClass("icon-plus-sign");
        }
    });

    $("#menu-toggle").click(function (e) {
        e.preventDefault();
        $("#wrapper").toggleClass("active");
    });

    $(".page").sortable();
});

function deleteImage() {
    $(".preview").attr("src", "");
    $("#ImageUrl").val("");
};

function initImageUpload(element) {
    $(".preview").click(function (e) {
        element.block();
        $("#Image").click();
    });

    $("#Image").change(function (e) {
        preViewImage(e, $(".preview"));
        element.unblock();
    });
}

function preViewImage(e, element) {
    if (e.target.files[0].type.match('image.*')) {
        if (typeof FileReader == "undefined") return true;
        if (e.target.files && e.target.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                element.attr("src", e.target.result);
            };
            reader.readAsDataURL(e.target.files[0]);
            element.unblock();
        }
    }
}

function saveSettings() {
}