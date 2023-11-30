﻿// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

$.ajaxSetup({ cache: false });
$.datetimepicker.setLocale("et");


// Eesti isikukoodi valideerimine:
//
function _verifyIsikukood(idCode) {

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

    location.href = url;
}


function _notEmpty(input) {

    return input.val().length != 0;
}


function _setInvalidIdCode(input, form) {

    input.addClass("is-invalid");
    form.removeClass("was-validated");
    $("div#" + input.attr("id") + "-invalid-feedback").html("Isikukood on vale, palun kontrollige seda uuesti.");
}


function _validIdCode(input, form) {

    if (_notEmpty(input)) {

        if (!input.val().match(/^\d+$/)) {

            _setInvalidIdCode(input, form);
            return false;
        }
        else {
            if (_verifyIsikukood(input.val())) {

                $("div#" + input.attr("id") + "-invalid-feedback").html("Peaks olema Isikukood!");
                input.removeClass("is-invalid");
                return true;
            }
            else {

                _setInvalidIdCode(input, form);
                return false;
            }
        }
    }
}


function _validOsavotjateArv(input, form) {

    if (input.val().match(/^\d+$/)) {

        $("div#" + input.attr("id") + "-invalid-feedback").html("Peaks olema Ettevõttest tulevate osavõtjate arv!");
        input.removeClass("is-invalid");
        return true;
    }
    else {

        input.addClass("is-invalid");
        form.removeClass("was-validated");
        $("div#" + input.attr("id") + "-invalid-feedback").html("Ettevõttest tulevate osavõtjate arv on vale, palun kontrollige seda uuesti.");
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

    $("input#uus-eraisik-eesnimi").val("");
    $("input#uus-eraisik-perekonnanimi").val("");
    $("input#uus-eraisik-isikukood").val("");
    $("select#uus-eraisik-makseviis").val("");
    $("textarea#uus-eraisik-lisainfo").val("");
    $("form#uus-eraisik").removeClass("was-validated");
}


function _emptyUusEttevoteForm() {

    $("input#uus-ettevote-nimi").val("");
    $("input#uus-ettevote-registrikood").val("");
    $("input#uus-ettevote-osavotjatearv").val("");
    $("select#uus-ettevote-makseviis").val("");
    $("textarea#uus-ettevote-lisainfo").val("");
    $("form#uus-ettevote").removeClass("was-validated");
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

    var form = $("form#uus-uritus");

    form.addClass("was-validated");

    if (_validateUritus()) {

        var spinner = $("span#uritus-add-modal-spinner");

        spinner.removeClass("d-none");

        $.post("/Home/LisaUritus", form.serialize(),
            function (response) {

                if (response.tehtud) {

                    setTimeout(function () {

                        $("div#uritus-add-modal").modal("hide");
                        spinner.addClass("d-none");

                        _emptyUritusForm();
                        _showPlaneeritudUritusedLoader();

                        var target = $("div#planeeritud-uritused");

                        setTimeout(function () { target.load("/Home/PlaneeritudUritused"); }, 650);

                    }, 850);
                }
            }, "json");
    }
}


function MuudaUritus() {

    var form = $("form#muuda-uritus");

    form.addClass("was-validated");

    if (_validateUritus()) {

        var spinner = $("span#uritus-edit-modal-spinner");

        spinner.removeClass("d-none");

        $.post("/Home/MuudaUritus", form.serialize(),
            function (response) {

                if (response.tehtud) {

                    setTimeout(function () {

                        $("div#uritus-edit-modal").modal("hide");
                        form.removeClass("was-validated");
                        spinner.addClass("d-none");

                        _showUrituseAndmedLoader();

                        var target = $("div#urituse-andmed");
                        var id = $("input#muuda-uritus-id").val();

                        setTimeout(function () { target.load("/Home/UrituseAndmed", { Id: id }); }, 1250);

                    }, 850);
                }
            }, "json");
    }
}


function _validateUusEraisik() {

    if (!_validIdCode($("input#uus-eraisik-isikukood"), $("form#uus-eraisik"))) return false;

    var isValid = true;

    isValid = isValid && _notEmpty($("input#uus-eraisik-eesnimi"));
    isValid = isValid && _notEmpty($("input#uus-eraisik-perekonnanimi"));
    isValid = isValid && _notEmpty($("select#uus-eraisik-makseviis"));

    return isValid;
}


function LisaEraisik() {

    var form = $("form#uus-eraisik");

    form.addClass("was-validated");

    if (_validateUusEraisik()) {

        var spinner = $("span#eraisik-add-modal-spinner");

        spinner.removeClass("d-none");

        $.post("/Home/LisaEraisik", form.serialize(),

            function (response) {

                if (response.tehtud) {

                    setTimeout(function () {

                        $("div#eraisik-add-modal").modal("hide");
                        spinner.addClass("d-none");

                        _emptyUusEraisikForm();
                        _showUrituseOsalejadLoader();

                        var target = $("div#urituse-osalejad");
                        var id = $("input#uus-eraisik-uritus-id").val();

                        setTimeout(function () { target.load("/Home/UrituseOsalejad", { Id: id, tabNr: 1 }); }, 1250);

                    }, 850);
                }
            }, "json");
    }
}


function _validateEttevote() {

    if (!_validOsavotjateArv($("input#uus-ettevote-osavotjatearv"), $("form#uus-ettevote"))) return false;

    var isValid = true;

    isValid = isValid && _notEmpty($("input#uus-ettevote-nimi"));
    isValid = isValid && _notEmpty($("input#uus-ettevote-registrikood"));
    isValid = isValid && _notEmpty($("select#uus-ettevote-makseviis"));

    return isValid;
}


function LisaEttevote() {

    var form = $("form#uus-ettevote");

    form.addClass("was-validated");

    if (_validateEttevote()) {

        var spinner = $("span#ettevote-add-modal-spinner");

        spinner.removeClass("d-none");

        $.post("/Home/LisaEttevote", form.serialize(),

            function (response) {

                if (response.tehtud) {

                    setTimeout(function () {

                        $("div#ettevote-add-modal").modal("hide");
                        spinner.addClass("d-none");

                        _emptyUusEttevoteForm();
                        _showUrituseOsalejadLoader();

                        var target = $("div#urituse-osalejad");
                        var id = $("input#uus-ettevote-uritus-id").val();

                        setTimeout(function () { target.load("/Home/UrituseOsalejad", { Id: id, tabNr: 2 }); }, 1250);

                    }, 850);
                }
            }, "json");
    }
}


function AlustaEraisikuEemaldamine(id, nimi) {

    $("span#eraisik-nimi-delete-modal").html("\"" + nimi + "\"");
    $("button#eraisik-delete-modal-delete-btn").attr("onclick", "EemaldaEraisik(" + id + ")");
    $("div#eraisik-delete-modal").modal("show");
}


function EemaldaEraisik(id) {

    _showUrituseOsalejadLoader();

    $("div#eraisik-delete-modal").modal("hide");

    $.post("/Home/EemaldaEraisik", { Id: id }, function () {

        var target = $("div#urituse-osalejad");
        id = $("input#uus-eraisik-uritus-id").val();

        setTimeout(function () { target.load("/Home/UrituseOsalejad", { Id: id, tabNr: 1 }); }, 550);
    });
}


function AlustaEttevoteEemaldamine(id, nimi) {

    $("span#ettevote-nimi-delete-modal").html("\"" + nimi + "\"");
    $("button#ettevote-delete-modal-delete-btn").attr("onclick", "EemaldaEttevote(" + id + ")");
    $("div#ettevote-delete-modal").modal("show");
}


function EemaldaEttevote(id) {

    _showUrituseOsalejadLoader();

    $("div#ettevote-delete-modal").modal("hide");

    $.post("/Home/EemaldaEttevote", { Id: id }, function () {

        var target = $("div#urituse-osalejad");
        id = $("input#uus-ettevote-uritus-id").val();

        setTimeout(function () { target.load("/Home/UrituseOsalejad", { Id: id, tabNr: 2 }); }, 550);
    });
}


function _showEraisikEditLoader() {

    var loader = $("section#osalejad-edit-loader");
    var target = $("div#eraisik-edit-modal-content");

    target.html(loader.html());
}


function AlustaEraisikuMuutmine(id) {

    var button = $("button#eraisik-edit-modal-edit-btn");
    var target = $("div#eraisik-edit-modal-content");

    button.removeAttr("onclick");

    _showEraisikEditLoader();

    $("div#eraisik-edit-modal").modal("show");

    setTimeout(function () {
        target.load("/Home/EraisikuAndmed", { Id: id },
            function () { button.attr("onclick", "MuudaEraisik()"); });
    }, 550);
}


function _validateEraisikEdit() {

    if (!_validIdCode($("input#eraisik-edit-isikukood"), $("form#eraisik-edit"))) return false;

    var isValid = true;

    isValid = isValid && _notEmpty($("input#eraisik-edit-eesnimi"));
    isValid = isValid && _notEmpty($("input#eraisik-edit-perekonnanimi"));

    return isValid;
}


function MuudaEraisik() {

    var form = $("form#eraisik-edit");

    form.addClass("was-validated");

    if (_validateEraisikEdit()) {

        var spinner = $("span#eraisik-edit-modal-spinner");

        spinner.removeClass("d-none");

        $.post("/Home/MuudaEraisik", form.serialize(),

            function (response) {

                if (response.tehtud) {

                    setTimeout(function () {

                        $("div#eraisik-edit-modal").modal("hide");
                        spinner.addClass("d-none");

                        _showUrituseOsalejadLoader();

                        var target = $("div#urituse-osalejad");
                        var id = $("input#uus-eraisik-uritus-id").val();

                        setTimeout(function () { target.load("/Home/UrituseOsalejad", { Id: id, tabNr: 1 }); }, 1250);

                    }, 850);
                }
            }, "json");
    }
}


function _showEttevoteEditLoader() {

    var loader = $("section#osalejad-edit-loader");
    var target = $("div#ettevote-edit-modal-content");

    target.html(loader.html());
}


function AlustaEttevoteMuutmine(id) {

    var button = $("button#ettevote-edit-modal-edit-btn");
    var target = $("div#ettevote-edit-modal-content");

    button.removeAttr("onclick");

    _showEttevoteEditLoader();

    $("div#ettevote-edit-modal").modal("show");

    setTimeout(function () {
        target.load("/Home/EttevoteAndmed", { Id: id },
            function () { button.attr("onclick", "MuudaEttevote()"); });
    }, 550);
}


function _validateEttevoteEdit() {

    if (!_validOsavotjateArv($("input#ettevote-edit-osavotjatearv"), $("form#ettevote-edit"))) return false;

    var isValid = true;

    isValid = isValid && _notEmpty($("input#ettevote-edit-nimi"));
    isValid = isValid && _notEmpty($("input#ettevote-edit-registrikood"));
    isValid = isValid && _notEmpty($("select#ettevote-edit-makseviis"));

    return isValid;
}


function MuudaEttevote() {

    var form = $("form#ettevote-edit");

    form.addClass("was-validated");

    if (_validateEttevoteEdit()) {

        var spinner = $("span#ettevote-edit-modal-spinner");

        spinner.removeClass("d-none");

        $.post("/Home/MuudaEttevote", form.serialize(),

            function (response) {

                if (response.tehtud) {

                    setTimeout(function () {

                        $("div#ettevote-edit-modal").modal("hide");
                        spinner.addClass("d-none");

                        _showUrituseOsalejadLoader();

                        var target = $("div#urituse-osalejad");
                        var id = $("input#uus-ettevote-uritus-id").val();

                        setTimeout(function () { target.load("/Home/UrituseOsalejad", { Id: id, tabNr: 2 }); }, 1250);

                    }, 850);
                }
            }, "json");
    }
}