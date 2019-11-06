// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function AjaxRequest(controllerAction, data, isAsyn, callback, errorCallback, method, contentType) {
    jQuery.ajaxSettings.traditional = true;
    return $.ajax({
        cache: false,
        type: method == null ? 'post' : method,
        data: data,
        url: controllerAction,
        async: isAsyn,
        dataType: 'json',
        traditional: true,
        success: callback,
        error: errorCallback,
        contentType: (typeof contentType !== 'undefined') ? contentType : 'application/x-www-form-urlencoded; charset=UTF-8'
    });
}