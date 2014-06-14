var checkNum = 0;
$(function () {
    $(".checks").on("click", "a", function (e) {
        e.preventDefault;
        $($(this).attr("id")).load($.validator.format("{0}/Order/ShowOrder/{1}", root, 1), function () {
            $(this).tab("show");
        });
    })
    .on("click", "button.close", function () {
        var element = $(this);
        noty({
            layout: "center",
            type: "error",
            killer: true,
            model: true,
            text: "Check will be deleted.<br />Would you like to continue ?",
            buttons: [{
                addClass: "btn btn-danger", text: "Delete", onClick: function ($noty) {
                    checkNum--;
                    var anchor = $(element).siblings("a");
                    $(anchor.attr("href")).remove();
                    $(element).parent().remove();
                    $noty.close();
                    $(".checks li:last a").tab("show");
                    refreshCheckNames();
                }
            },
              {
                  addClass: "btn btn-default", text: "Cancel", onClick: function ($noty) {
                      $noty.close();
                  }
              }
            ]
        });
    });

    $("#btnAdd").click(function (e) {
        checkNum++;
        $(".checks").append($.validator.format("<li><a id='#Check{0}' href='#Check{0}' data-toggle='tab'></a></li>", checkNum));
        $(".check").append($.validator.format("<div class='tab-pane fade in active' id='Check{0}'></div>", checkNum));
        $(".checks li:last a").tab("show");
        refreshCheckNames();
    });
})

function refreshCheckNames() {
    $(".checks li a").each(function (index, value) {
        index++;
        $(this).html($.validator.format("Check#{0}<button class='close btn btn-primary' title='Remove this check' type='button'><i class='glyphicon glyphicon-remove-circle'></button>", index));
    })
}

function getAsosiation(id) {
    var container = $("#menuItems");
    var active = $(".check").find(".active");
    container.block();
    var jqxhr = $.get("/Order/ShowOrderItem", { "id": id }, "json")
                  .done(function (result) {
                      $(active).append(result);
                  })
    .fail(function () {
        message("Failed.", "error", "center");
    })
    .always(function () {
        container.unblock();
    });
};