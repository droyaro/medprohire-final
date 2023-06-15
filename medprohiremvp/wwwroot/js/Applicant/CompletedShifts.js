$(document).ready(function () {
    $('span.pie').peity('pie', {
        fill: ['#1a85c7', '#d7d7d7', '#ffffff'],
        height: 30,
        radius: 20,
        width: 30

    });
    var datatable = $('.datatable').DataTable({
        searching: false,
        info: false,
        pageLength: 10,
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
  
});
$('.activeshiftview').click(function () {
    var id = $(this).data('id');
    $.ajax({
        url: "/Applicant/_activeshiftdetail",
        type: "POST",
        data: {
            id: id
        },
        success: function (response) {
            $("#ActiveShiftModal").find('.modal-body').empty();
            $("#ActiveShiftModal").find('.modal-body').append(response);
            $("#ActiveShiftModal").modal('show');
        }
    });
});