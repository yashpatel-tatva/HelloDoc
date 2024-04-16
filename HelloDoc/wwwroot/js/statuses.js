//$(document).ready(function () {
//    $('.newtable').DataTable({
//        "initComplete": function (settings, json) {
//            var table = this.api(); // Get the DataTable instance
//            $('#my-search-input').val(settings.oPreviousSearch.sSearch);
//            $('#my-search-input').on('keyup', function () {
//                var searchValue = $(this).val();
//                settings.oPreviousSearch.sSearch = searchValue;
//                settings.oApi._fnReDraw(settings);
//            });
//            $('input[name="requestby"]').on('change', function () {
//                var value = $(this).attr('id');
//                if (value == 'requestbyAll') {
//                    table.column(0).search('').draw();
//                } else {
//                    table.column(0).search(value).draw();
//                }
//            });
//            $('.drawbyregiondropdown').on('change', function () {
//                var value = $(this).val();
//                if (value == '1234') {
//                    table.columns(1).search('').draw();
//                }
//                else {
//                    table.columns(1).search(value).draw();
//                }
//            });
//        },
//        "lengthMenu": [[5, 10, 25, -1], [5, 10, 25, "All"]],
//        "pageLength": 5,
//        language: {
//            oPaginate: {
//                sNext: '<i class="bi bi-caret-right-fill text-info"></i>',
//                sPrevious: '<i class="bi bi-caret-left-fill text-info"></i>'
//            }
//        }
//    });
//    $('.dataTables_filter').hide();
//});

$('.gotoaction').click(function (e) {
    e.preventDefault();
    var action = $(this).attr('action');
    var id = $(this).closest('form').data('id');

    $.ajax({
        url: '/AdminArea/Dashboard/' + action,
        type: 'GET',
        data: { id: id },
        success: function (result) {

            $('#nav-tabContent').html(result);
        },
        error: function (xhr, status, error) {
            console.error('Error: ' + error);
        },
    });
});

$('.gotopopup').click(function (e) {
    e.preventDefault();
    var action = $(this).attr('action');
    var id = $(this).closest('form').data('id');

    $.ajax({
        url: '/AdminArea/Dashboard/' + action,
        type: 'GET',
        data: { id: id },
        success: function (result) {
            $('#PopUps').html(result);
            var my = new bootstrap.Modal(document.getElementById('ModalToOpen'));
            my.show();
        },
        error: function (xhr, status, error) {
            console.error('Error: ' + error);
        },
    });
});


$('.gotoproviderpopupmodel').click(function (e) {
    e.preventDefault();
    var action = $(this).attr('action');
    var id = $(this).closest('form').data('id');

    $.ajax({
        url: '/ProviderArea/Dashboard/' + action,
        type: 'POST',
        data: { id: id },
        success: function (result) {
            $('#PopUps').html(result);
            var my = new bootstrap.Modal(document.getElementById('ModalToOpen'));
            my.show();
        },
        error: function (xhr, status, error) {
            console.error('Error: ' + error);
        },
    });
});


$('.gotoaction').on('click', function (e) {
    var name = $(this).closest('form').attr('value');
    $('.patientname').html(name);
    var id = $(this).closest('form').data('id');
    $('.inputhiddenrequestid').val(id);

});

$('.gotoproviderpopup').on('click', function () {
    var action = $(this).attr('action');
    var id = $(this).closest('form').data('id');
    var text = "";
    var aftertext = "";
    if (action == "AcceptCase") {
        text = "Accept";
        aftertext = "Accepted"
        Swal.fire({
            title: "Are you sure?",
            text: "You want to " + text + " this!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Yes, " + text + " it!"
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: '/ProviderArea/Dashboard/' + action,
                    data: { id },
                    type: 'POST',
                    success: function (response) {
                        Swal.fire({
                            title: aftertext + "!",
                            text: "Case has been " + aftertext + ".",
                            icon: "success"
                        });
                        $('#nav-tabContent').html(response);
                    }
                });
            }
        });
    }
    else {
        text = "Decline"
        aftertext = "Declined"
        Swal.fire({
            title: "Are you sure?",
            text: "You want to " + text + " this!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Yes, " + text + " it!",
            input: "text",
            inputLabel: "Cancellation Notes",
            inputValidator: (value) => {
                if (!value) {
                    return "You need to write something!";
                }
            }
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: '/ProviderArea/Dashboard/' + action,
                    data: { id, note: result.value },
                    type: 'POST',
                    success: function (response) {
                        Swal.fire({
                            title: aftertext + "!",
                            text: "Case has been " + aftertext + ".",
                            icon: "success"
                        });
                        $('#nav-tabContent').html(response);
                    }
                });
            }
        });
    }

})


$('.housecallbtn').on('click', function () {
    var id = $(this).closest('form').data('id');
    $.ajax({
        url: '/AdminArea/Dashboard/OnHouseOpenEncounter',
        data: { id },
        type: 'POST',
        success: function (result) {
            $('#nav-tabContent').html(result);
            $('#closemodel').trigger('click');
        },
    });
})

$('.gotoactionproviderside').click(function (e) {
    e.preventDefault();
    var action = $(this).attr('action');
    var id = $(this).closest('form').data('id');

    $.ajax({
        url: '/ProviderArea/Dashboard/' + action,
        type: 'GET',
        data: { id: id },
        success: function (result) {
            $('#nav-tabContent').html(result);
        },
        error: function (xhr, status, error) {
            console.error('Error: ' + error);
        },
    });
});