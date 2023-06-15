$(document).on('click', '#add-provider', function () {
    var index = $(".bprovider tr").length;
    var selects = $(".bprovider tr td").find('select');
    var array=[];
    selects.each(function () {
        array.push($(this).val());
    });

    var isvalid = true;

    if (isvalid) {
        $.ajax({
            url: "/ClinicalInstitution/_Client_specialties",
            type: "POST",
            data: {
                model: index,
                specIds: array
            },
            success: function (response) {

                $(".bprovider").append(response);
                $(".bprovider tr:last").find('select').each(function () { $(this).addClass('selectpicker'); });
                $('.selectpicker').selectpicker({

                    style: 'btn-default flat'
                });
                $('.selectpicker').selectpicker('refresh');
              
            }

        });
    }

});