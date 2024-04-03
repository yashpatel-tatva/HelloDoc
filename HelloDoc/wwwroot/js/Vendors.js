
$.ajax({
    url: '/AdminArea/AdminPartnersTab/GetProfessionalTypes',
    success: function (data) {
        var professions = $('.professions');
        $.each(data, function (index, p) {
            professions.append($('<option>', {
                value: p.healthprofessionalid,
                text: p.professionname
            }))
        })
    }
})

function getdata(search, profession) {
    $.ajax({
        url: '/AdminArea/AdminPartnersTab/GetVendorsDetail',
        data: { search, profession },
        type: 'POST',
        success: function (response) {
            $('#_Vendordetail').html(response);
        },
    });
}
getdata("", 0);
$('.professions').on('change', function () {
    getdata("", $(this).val());
})

$('#searchInput').on('keyup', function () {
    var search = $(this).val();
    getdata(search, $('.professions').val());
})

