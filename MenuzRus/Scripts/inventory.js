$(function () {
});

function addInventoryAssosiation() {
    var clone = $(".template").clone();
    clone.removeAttr("style").removeClass("template").css("margin-top", "25px");
    $(".container-item").append(clone);
    $(".container-item select:last").chosen({ width: "350px", display_disabled_options: false });
}