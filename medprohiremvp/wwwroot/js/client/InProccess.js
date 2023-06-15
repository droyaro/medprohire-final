//$(document).on('click', '.applicant', function () {
//    var href = $(this).attr('href');
//    $('.applicant.activeapplicant').removeClass('activeapplicant');
//    $(this).addClass('activeapplicant');
//    $('.applicantinfo.active.show').removeClass('active').removeClass('show');
//    $(href).addClass('active').addClass('show');
//});
$(document).on('click', '.proccesshifttable tbody tr', function () {
    var href = $(this).find('.applicant').data('href');
    $(this).parent().find('.applicant.activeapplicant').removeClass('activeapplicant');
    $(this).find('.applicant').addClass('activeapplicant');
    $('.applicantinfo.active.show').removeClass('active').removeClass('show');
    $(href).addClass('active').addClass('show');
});
var datatable = $('.datatable').DataTable({
    "bAutoWidth": false,
    autoWidth: false,
    searching: false,
    info: false,
    pageLength: 5,
    "pagingType": "numbers",
    lengthChange: false,
    "dom": '<"datatablefullwith" <"row "<"col-12" <"bottom">r><" tablebackground nwtb" t>><"float-center datatablepagination pagging" p ><"clear">>',
    "language": {
        "paginate": {
            "previous": '<i class="far fa-angle-left"></i>',
            "next": '<i class="far fa-angle-right"></i>',
        }
    },
    responsive: {
        details: {
            type: 'column',
            renderer: function (api, rowIdx, columns) {
                return false;
            }
        }

    },
    drawCallback: function () {
        var api = this.api();
        $('.datatable').parent().parent().find('.datapageDetails').remove();
        $('.datatable').parent().parent().find('.paginate_button.next').before($('<span>', {
            'text': (api.page() + 1),
            class: 'datapageDetails'
        }));
        if ($('.datatablefullwith .dataTables_paginate li.page-item').length <= 1) {
            $('.datatablefullwith .dataTables_paginate').hide();
        }
    },


});