var root = location.protocol + '//' + location.host;
$(function () {
    $.blockUI.defaults.message = $("#bowlG");
    $.blockUI.defaults.css = " border: '0px none transparent; ";
    //$(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);

    $(".page").sortable();
    setMenu();
});

function setMenu() {
    if (window.location.href.indexOf("/Login") == -1)
        $(".menuAlways").show();
    else
        $(".menuAlways").hide();
    if (window.location.href.indexOf("/YourMenu") > -1)
        $(".menuDesigner").show();
    else
        $(".menuDesigner").hide();
};

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

function message(text, type, position) {
    noty({
        text: text,
        layout: position,
        type: type,
        template: '<div class="noty_message"><span class="noty_text"></span><div class="noty_close"></div></div>',
        timeout: "5000",
        maxVisible: 1,
        killer: true,
        closable: true,
        closeOnSelfClick: true,
        closeWith: ["click", "button"]
    });
}