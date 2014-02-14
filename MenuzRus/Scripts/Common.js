var root = location.protocol + '//' + location.host;
$(function () {
    $.blockUI.defaults.message = $("#bowlG");
    $.blockUI.defaults.css = " border: '0px none transparent; ";

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
    $(".preview, .preview1, .preview2").attr("src", "");
    $("#ImageUrl, #ImageUrl1, #ImageUrl2").val("");
};

function initImageUpload() {
    $(".preview").click(function (e) {
        $(".preview").block();
        $("#Image").click();
    });

    $("#Image").change(function (e) {
        $(".imagerow").block();
        preViewImage(e, $(".preview"));
        $(".imagerow").unblock();
    });
}

function initImageUploadCustomer() {
    $(".preview1").click(function (e) {
        $(".preview1").block();
        $("#Image1").click();
    });

    $(".preview2").click(function (e) {
        $(".preview2").block();
        $("#Image2").click();
    });

    $("#Image1").change(function (e) {
        preViewImage(e, $(".preview1"));
    });

    $("#Image2").change(function (e) {
        preViewImage(e, $(".preview2"));
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