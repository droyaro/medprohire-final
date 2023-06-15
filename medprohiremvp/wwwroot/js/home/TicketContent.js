$('.starrate').barrating({
    theme: 'fontawesome-stars'
});
$('.summernote').summernote({
    height: 200

});
$(".selectpicker").selectpicker({
    style: 'btn-default'
});
$('#AddAnswer').click(function () {
    $('#answerarea').prop('hidden', false);
    $('#AddAnswer').remove();
    $('#Complete').prop('hidden', true);

});
$('#Complete').click(function () {
    $('#CompleteTicket').modal('show');

});
$('#acceptticketcomplete').click(function () {
    var id = $(this).data('ticket_id');
    $.ajax({
        url: "/Account/CompleteTicket",
        type: "Post",
        data:
        {
            ticket_id: id,
        },
        success: function (response) {
            location.reload();
        }
    });
});
$(document).on('change', '.starrate', function () {
    var rate = $(this).val();
    var id = $(this).data('content_id');
    $.ajax({
        url: "/Account/RateTicketContent",
        type: "POST",
        data: {
            id: id,
            rate: rate
        },
        success: function (response) {
            $('#starrate').append('<span class="text-denger"> Content Rated</span>');
        }
    });
});
$('#AddTicketContent').click(function () {
    var content = $('.note-editable')[0].innerHTML;
    var id = $(this).data('id');
    $.ajax({
        url: "/Account/AddContent",
        type: "POST",
        data: {
            id: id,
            content: content
        },
        success: function (response) {
            location.reload();
        }
    });
});
$(".selectpicker").selectpicker({
    style: 'btn-default'
});