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


function _notEmpty(input) {

    return input.val().length != 0;
}


function _emptyUritusForm() {

    $("input#urituse-nimetus").val("");
    $("input#urituse-toimumisaeg").val("");
    $("input#urituse-toimumisekoht").val("");
    $("textarea#urituse-lisainfo").val("");
    $("form#uus-uritus").removeClass("was-validated");
}


function _showPlaneeritudUritusedLoader() {

    var loader = $("section#uritused-loader");
    var target = $("div#planeeritud-uritused");

    target.html(loader.html());
}


function StartUp() {

    setTimeout(HideMainLoader, 1150);

    var now = new Date();
    var month = now.getMonth() + 1;
    var day = now.getDate() + 1;

    var date = now.getFullYear() + "/" + (month < 10 ? "0" : "") + month + "/" + (day < 10 ? "0" : "") + day;

    $("input#urituse-toimumisaeg").datetimepicker({
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


function LisaUritus() {

    $("form#uus-uritus").addClass("was-validated");

    var isValid = true;

    isValid = isValid && _notEmpty($("input#urituse-nimetus"));

    isValid = isValid && _notEmpty($("input#urituse-toimumisaeg"));

    isValid = isValid && _notEmpty($("input#urituse-toimumisekoht"));

    if (isValid) {

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

                        setTimeout(function () { target.load("/Home/PlaneeritudUritused"); }, 750);

                    }, 550);
                }

            }, "json");
    }
}


