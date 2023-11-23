// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$.ajaxSetup({ cache: false });


function StartUp() {

    setTimeout(HideMainLoader, 1150);
}


function HideMainLoader() {

    $("div.main-loader").fadeOut(350, function () {
        $("html, body").css("overflow", "auto");
        $("div#main-loader").fadeOut(950);
    });
}


function AlustaUrituseEemaldamine(id, nimetus) {

    $("span#uritus-nimi-delete-modal").html("\"" + nimetus + "\"");
    $("button#uritus-delete-modal-delete-btn").attr("onClick", "EemaldaUritus(" + id + ")");
    $("div#uritus-delete-modal").modal("show");
}


function EemaldaUritus(id) {

    $("div#uritus-delete-modal").modal("hide");

    var loader = $("section#uritused-loader");
    var target = $("div#planeeritud-uritused");

    target.html(loader.html());

    $.post("/Home/EemaldaUritus", { Id: id }, function () {
        setTimeout(function () { target.load("/Home/PlaneeritudUritused"); }, 550);
    });
}

function LisaUritus() {


}


