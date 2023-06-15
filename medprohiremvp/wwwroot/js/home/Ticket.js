
$('.datatable').DataTable({
    searching: false,
    info: false,
    pageLength: 10,
    "pagingType": "numbers",
    lengthChange: false,
    "dom": '<"d-flex view-pager"><"bottom">r<"tablebackground nwtb" t><"p-2 mx-auto pagging"<p>><"clear">',
    "language": {
        "paginate": {
            "previous": '<i class="far fa-angle-left"></i>',
            "next": '<i class="far fa-angle-right"></i>',
        }
    },
    drawCallback: function () {
        var api = this.api();
        $('.datatable').parent().parent().find('.peycheckpageDetails').remove();
        $('.datatable').parent().parent().find('.paginate_button.next').before($('<span>', {
            'text': (api.page() + 1),
            class: 'peycheckpageDetails'
        }));

    }


});
