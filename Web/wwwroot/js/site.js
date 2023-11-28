// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

$.ajaxSetup({ cache: false });
$.datetimepicker.setLocale("et");


// Eesti isikukoodi valideerimine:
//
function _verifyIdCode(idCode) {

    if (idCode == "" || idCode.length != 11) return false;

    var weights1 = [1, 2, 3, 4, 5, 6, 7, 8, 9, 1];
    var weights2 = [3, 4, 5, 6, 7, 8, 9, 1, 2, 3];
    var sum1, sum2, remainder1, remainder2;

    sum1 = 0;

    for (var i = 0; i < idCode.length - 1; i++) sum1 += idCode.charAt(i) * weights1[i];

    remainder1 = sum1 % 11;

    if (remainder1 < 10) return remainder1 == idCode.charAt(idCode.length - 1) * 1;

    sum2 = 0;

    for (var i = 0; i < idCode.length - 1; i++) sum2 += idCodeidCode.charAt(i) * weights2[i];

    remainder2 = sum2 % 11;

    if (remainder2 < 10) return remainder2 == idCode.charAt(idCode.length - 1) * 1;

    return (idCode.charAt(idCode.length - 1) * 1 == 0);
}


function _visit(url) {

    $("div#main-loader").fadeIn(350, function () {
        location.href = url;
    });
}


function _notEmpty(input) {

    return input.val().length != 0;
}


function _validIdCode(input, form) {

    if (_notEmpty(input)) {

        if (_verifyIdCode(input.val())) {

            $("div#" + input.attr("id") + "-invalid-feedback").html("Peaks olema Isikukood!");
            input.removeClass("is-invalid");
            return true;
        }
        else {

            $("div#" + input.attr("id") + "-invalid-feedback").html("Isikukood on vale, palun kontrollige seda uuesti.");
            input.addClass("is-invalid");
            form.removeClass("was-validated");
            return false;
        }
    }
}


function _emptyUritusForm() {

    $("input#uritus-nimetus").val("");
    $("input#uritus-toimumisaeg").val("");
    $("input#uritus-toimumisekoht").val("");
    $("textarea#uritus-lisainfo").val("");
    $("form#uus-uritus").removeClass("was-validated");
}


function _emptyUusEraisikForm() {

    $("input#uus-eraisik-isikukood").val("");
    $("input#uus-eraisik-eesnimi").val("");
    $("input#uus-eraisik-perekonnanimi").val("");
    $("select#uus-eraisik-makseviis").val("");
    $("form#uus-eraisik").removeClass("was-validated");
}


function _showPlaneeritudUritusedLoader() {

    var loader = $("section#uritused-loader");
    var target = $("div#planeeritud-uritused");

    target.html(loader.html());
}


function _showUrituseAndmedLoader() {

    var loader = $("section#urituse-andmed-loader");
    var target = $("div#urituse-andmed");

    target.html(loader.html());
}


function _showUrituseOsalejadLoader() {

    var loader = $("section#urituse-osalejad-loader");
    var target = $("div#urituse-osalejad");

    target.html(loader.html());
}


function StartUp() {

    setTimeout(HideMainLoader, 1150);

    var now = new Date();
    var month = now.getMonth() + 1;
    var day = now.getDate() + 1;

    var date = now.getFullYear() + "/" + (month < 10 ? "0" : "") + month + "/" + (day < 10 ? "0" : "") + day;

    $("input#uritus-toimumisaeg").datetimepicker({
        format: "d.m.Y H:i",
        minDate: date
    });
}


function HideMainLoader() {

    $("div.main-loader").fadeOut(350, function () {
        $("html, body").css("overflow", "auto");
        $("div#main-loader").fadeOut(950);
    });
}


function VaataUritus(id) {

    _visit("/Home/VaataUritus/" + id);
}


function AlustaUrituseEemaldamine(id, nimetus) {

    $("span#uritus-nimi-delete-modal").html("\"" + nimetus + "\"");
    $("button#uritus-delete-modal-delete-btn").attr("onclick", "EemaldaUritus(" + id + ")");
    $("div#uritus-delete-modal").modal("show");
}


function EemaldaUritus(id) {

    _showPlaneeritudUritusedLoader();

    $("div#uritus-delete-modal").modal("hide");

    var target = $("div#planeeritud-uritused");

    $.post("/Home/EemaldaUritus", { Id: id }, function () {
        setTimeout(function () { target.load("/Home/PlaneeritudUritused"); }, 550);
    });
}


function _validateUritus() {

    var isValid = true;

    isValid = isValid && _notEmpty($("input#uritus-nimetus"));

    isValid = isValid && _notEmpty($("input#uritus-toimumisaeg"));

    isValid = isValid && _notEmpty($("input#uritus-toimumisekoht"));

    return isValid;
}


function LisaUritus() {

    $("form#uus-uritus").addClass("was-validated");

    if (_validateUritus()) {

        var spinner = $("span#uritus-add-modal-spinner");

        spinner.removeClass("d-none");

        $.post("/Home/LisaUritus", $("form#uus-uritus").serialize(),
            function (response) {

                if (response.tehtud) {

                    setTimeout(function () {

                        $("div#uritus-add-modal").modal("hide");

                        _emptyUritusForm();

                        spinner.addClass("d-none");

                        _showPlaneeritudUritusedLoader();

                        var target = $("div#planeeritud-uritused");

                        setTimeout(function () { target.load("/Home/PlaneeritudUritused"); }, 650);

                    }, 850);
                }

            }, "json");
    }
}


function MuudaUritus() {

    $("form#muuda-uritus").addClass("was-validated");

    if (_validateUritus()) {

        var spinner = $("span#uritus-edit-modal-spinner");

        spinner.removeClass("d-none");

        $.post("/Home/MuudaUritus", $("form#muuda-uritus").serialize(),
            function (response) {

                if (response.tehtud) {

                    setTimeout(function () {

                        $("div#uritus-edit-modal").modal("hide");

                        spinner.addClass("d-none");

                        $("form#muuda-uritus").removeClass("was-validated");

                        _showUrituseAndmedLoader();

                        var target = $("div#urituse-andmed");
                        var id = $("input#muuda-uritus-id").val();

                        setTimeout(function () { target.load("/Home/UrituseAndmed", { Id: id }); }, 1250);

                    }, 850);
                }

            }, "json");
    }
}


function _validateEraisik() {

    if (!_validIdCode($("input#uus-eraisik-isikukood"), $("form#uus-eraisik"))) return false;

    var isValid = true;

    isValid = isValid && _notEmpty($("input#uus-eraisik-eesnimi"));

    isValid = isValid && _notEmpty($("input#uus-eraisik-perekonnanimi"));

    isValid = isValid && _notEmpty($("select#uus-eraisik-makseviis"));

    return isValid;
}


function LisaEraisik() {

    $("form#uus-eraisik").addClass("was-validated");

    if (_validateEraisik()) {

        var spinner = $("span#eraisik-add-modal-spinner");

        spinner.removeClass("d-none");

        $.post("/Home/LisaEraisik", $("form#uus-eraisik").serialize(),

            function (response) {

                if (response.tehtud) {

                    setTimeout(function () {

                        $("div#eraisik-add-modal").modal("hide");

                        _emptyUusEraisikForm()

                        spinner.addClass("d-none");

                        _showUrituseOsalejadLoader();

                        var target = $("div#urituse-osalejad");
                        var id = $("input#uus-eraisik-uritus-id").val();

                        setTimeout(function () { target.load("/Home/UrituseOsalejad", { Id: id }); }, 1250);

                    }, 850);
                }

            }, "json");
    }
}
