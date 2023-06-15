$(document).on('click', '#ReadMore', function () {
    var shiftid = $(this).data('clientid');
    $.ajax({
        url: "/Applicant/_pickedshiftdetail",
        type: "POST",
        data: {
            id: shiftid
        },
        success: function (response) {
            $("#myModal2").find('.modal-body').empty();
            $("#myModal2").find('.modal-body').append(response);
            $("#myModal2").modal('show');
        }
    });
    
});
$(".selectpicker").selectpicker({
    style: 'btn-default'
});