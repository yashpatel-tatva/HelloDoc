


$(document).ready(function () {
    $('.newtable').DataTable({
        "initComplete": function (settings, json) {
            var table = this.api(); // Get the DataTable instance
            $('#my-search-input').val(settings.oPreviousSearch.sSearch);
            $('#my-search-input').on('keyup', function () {
                var searchValue = $(this).val();
                settings.oPreviousSearch.sSearch = searchValue;
                settings.oApi._fnReDraw(settings);
            });
            $('input[name="requestby"]').on('change', function () {
                var value = $(this).attr('id');
                if (value == 'requestbyAll') {
                    table.column(0).search('').draw();
                } else {
                    table.column(0).search(value).draw();
                }
            });
            $('.drawbyregiondropdown').on('change', function () {
                var value = $(this).val();
                if (value == '1234') {
                    table.columns(1).search('').draw();
                }
                else {
                    table.columns(1).search(value).draw();
                }
            });
        },
        "lengthMenu": [[5, 10, 25, -1], [5, 10, 25, "All"]],
        "pageLength": 5,
        language: {
            oPaginate: {
                sNext: '<i class="bi bi-caret-right-fill text-info"></i>',
                sPrevious: '<i class="bi bi-caret-left-fill text-info"></i>'
            }
        }
    });
    $('.dataTables_filter').hide();
});

$('.gotoaction').click(function (e) {
    e.preventDefault();
    var action = $(this).attr('action');
    var id = $(this).closest('form').data('id');

    $.ajax({
        url: 'AdminArea/Dashboard/' + action,
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
        url: 'AdminArea/Dashboard/' + action,
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


$('.gotoaction').on('click', function (e) {
    var name = $(this).closest('form').attr('value');
    $('.patientname').html(name);
    var id = $(this).closest('form').data('id');
    $('.inputhiddenrequestid').val(id);

});

$('.gotoaction[data-target="#CancelCaseModal"]').on('click', function () {
    $.ajax({
        url: 'AdminArea/Dashboard/GetCaseTags',
        success: function (data) {
            var casetagsdropdown = $('.cancleselect');
            casetagsdropdown.empty();
            casetagsdropdown.append($('<option>', {
                hidden: "hidden",
                value: "1234",
                text: "Reason for Cancellation"
            }))
            $.each(data, function (index, item) {
                casetagsdropdown.append($('<option>', {
                    value: item.casetagid,
                    text: item.name
                }))
            });
        }
    });
});



$('.gotoaction[data-target="#AssignCasemodal"]').on('click', function () {
    $.ajax({
        url: 'AdminArea/Dashboard/GetRegion',
        success: function (data) {
            var regiondropdown = $('.regiondropdown');
            regiondropdown.empty();
            regiondropdown.append($('<option>', {
                value: "",
                text: ""
            }))
            $.each(data, function (index, regions) {
                regiondropdown.append($('<option>', {
                    value: regions.regionid,
                    text: regions.name
                }))
            });
        },
    });
    $.ajax({
        url: 'AdminArea/Dashboard/GetPhysician',
        success: function (data) {
            var physiciandropdown = $('#physiciandropdown');
            physiciandropdown.empty();
            physiciandropdown.append($('<option>', {
                hidden: "hidden",
                value: "invalid",
                text: "Select Physician"
            }))
            $.each(data, function (index, phy) {
                physiciandropdown.append($('<option>', {
                    value: phy.physicianid,
                    text: phy.firstname + " " + phy.lastname
                }))
            });
        }
    });
});
$('.gotoaction[data-target="#TransferCaseModal"]').on('click', function () {
    $.ajax({
        url: 'AdminArea/Dashboard/GetRegion',
        success: function (data) {
            var regiondropdown = $('.transtoregiondropdown');
            regiondropdown.empty();
            regiondropdown.append($('<option>', {
                value: "",
                text: ""
            }))
            $.each(data, function (index, regions) {
                regiondropdown.append($('<option>', {
                    value: regions.regionid,
                    text: regions.name
                }))
            });
        }
    });
    $.ajax({
        url: 'AdminArea/Dashboard/GetPhysician',
        success: function (data) {
            var physiciandropdown = $('#transtophysiciandropdown');
            physiciandropdown.empty();
            physiciandropdown.append($('<option>', {
                hidden: "hidden",
                value: "invalid",
                text: "Select Physician"
            }))
            $.each(data, function (index, phy) {
                physiciandropdown.append($('<option>', {
                    value: phy.physicianid,
                    text: phy.firstname + " " + phy.lastname
                }))
            });
        }
    });
});