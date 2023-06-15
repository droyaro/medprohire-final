$(document).ready(function () {
    var date = new Date().getTimezoneOffset();
var name=date.toString();
    $.ajax({
        url: "/Home/GetTime",
        type: "POST",
        data: { time: date },
        success: function (response) {
           
        }
    });
});