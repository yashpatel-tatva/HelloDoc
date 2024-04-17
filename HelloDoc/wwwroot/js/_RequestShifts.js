$('input[name="selectall"]').on('change', function () {
    $('input[name="selectedshift"]').prop("checked", $('input[name="selectall"]').prop("checked"))
    if ($('input[name="selectall"]').prop("checked") == false) {
        $('#deleteselected').prop("disabled", true);
        $('#approveselected').prop("disabled", true);
    }
    else {
        $('#deleteselected').prop("disabled", false);
        $('#approveselected').prop("disabled", false);
    }
});
$('input[name="selectedshift"]').on('change', function () {
    if ($('input[name="selectedshift"]').length === $('input[name="selectedshift"]:checked').length) {
        $('input[name="selectall"]').prop("checked", true);
    }
    else {
        $('input[name="selectall"]').prop("checked", false);
    }
    if ($('input[name="selectedshift"]:checked').length === 0) {
        $('#deleteselected').prop("disabled", true);
        $('#approveselected').prop("disabled", true);
    }
    else {
        $('#deleteselected').prop("disabled", false);
        $('#approveselected').prop("disabled", false);
    }
});

//var region = localStorage.getItem('region') || 0;
//var currentDate = localStorage.getItem('currentDate') ? new Date(localStorage.getItem('currentDate')) : new Date();
//var showby = localStorage.getItem('showby') || $('input[name="showby"]:checked').attr('id');
//var currentpage = $('.current.paginate_button').data('id');
//$("#deleteselected").on('click', function () {
//    var shiftdetailsid = [];
//    $.each($('input[name="selectedshift"]:checked'), function (index, item) {
//        shiftdetailsid.push($(this).data('id'));
//    });
//    $.ajax({
//        url: '/AdminArea/AdminProviderTab/DeleteSelecetdShifts',
//        data: { shiftdetailsid: shiftdetailsid, datetoshow: currentDate.toISOString(), region: region, showby: showby, currentpage: currentpage },
//        type : 'POST' ,
//        success: function (response) {
//            $('#_RequestedShifts').html(response);
//            Swal.fire({
//                position: "top-end",
//                icon: "success",
//                title: "Shifts Deleted",
//                showConfirmButton: false,
//                timer: 1000
//            });
//        },
//    })
//})
