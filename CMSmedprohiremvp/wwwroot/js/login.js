$(document).ready(function () {
    var date = new Date().getTimezoneOffset();
    var name = date.toString();
    $('#TimeOffset').val(name);
});