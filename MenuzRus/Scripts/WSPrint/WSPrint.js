var WSPrint = {
    open: function () {
        if (!webSocket) {
            webSocket = new WS(
                function (on) {
                    if (on) {
                        $("#printerImage").show();
                    }
                    else {
                        var jqxhr = $.get($.validator.format("{0}Home/NoPrinters", root), "json")
                            .done(function () {
                            })
                            .fail(function () {
                                message("::noPrinters:: Failed.", "error", "topCenter");
                            })
                            .always(function () {
                                $("#printerImage").hide();
                            });
                    }
                },

                function (m) {
                    var j = JSON.parse(m)
                    if (j.status == "OK=getPrinters") {
                        printers = JSON.parse(j.message);
                        printers.splice(0, 0, "None")
                        WSPrint.sendPrinters();
                    }
                }
            )
            return;
        }
    },

    getPrinters: function () {
        if (printers == "") {
            webSocket.getPrinters();
        }
    },

    sendPrinters: function () {
        if (printers != "") {
            var jqxhr = $.get($.validator.format("{0}Home/SendPrinters", root), { "model": JSON.stringify(printers) }, "json")
            .done(function (result) {
                printerPOS = result.printerPOS;
                printerKitchen = result.printerKitchen;
                printerPOSWidth = result.printerPOSWidth;
                printerKitchenWidth = result.printerKitchenWidth;
            })
            .fail(function () {
                message("::sendPrinters:: Failed.", "error", "topCenter");
            })
            .always(function () {
            });
        }
    },

    printHTML: function (html, printer) {
        webSocket.printHtml(html, printer);
    }
}

function WS(onstate, onmessage) {
    ws = new WebSocket("ws://localhost:9001/json");
    ws.onopen = function (event) {
        if (onstate && (typeof onstate == "function")) {
            onstate(true);
            WSPrint.getPrinters();
        }
    }
    ws.onclose = function (event) {
        if (onstate && (typeof onstate == "function")) {
            onstate(false);
        }
    }
    ws.onmessage = function (event) {
        if (onstate && (typeof onstate == "function")) {
            onmessage(event.data);
        }
    };

    this.send = function (data) {
        if (ws) {
            ws.send(data);
        }
    }

    this.getPrinters = function () {
        this.send(JSON.stringify({ "action": "getPrinters", "params": "" }));
    }

    this.printHtml = function (data, device) {
        var sap = JSON.stringify({ printer: device, html: data })
        this.send(JSON.stringify({ action: "setAndPrint", params: sap }));
    }
}

// Attaching WSPrinter
WSPrint.open();