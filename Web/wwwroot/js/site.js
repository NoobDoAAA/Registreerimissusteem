// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$.ajaxSetup({ cache:false });


function StartUp(){

	setTimeout(HideMainLoader, 650);

	
}


function HideMainLoader(){

	$("div.main-loader").fadeOut(350, function()
	{
		$("html, body").css("overflow", "auto");
		$("div#main-loader").fadeOut(950);
	});
}


