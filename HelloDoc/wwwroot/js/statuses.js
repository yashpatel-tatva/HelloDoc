

console.log("status.js here");

var table = $('.newtable').DataTable({
    dom: 'lrtip',
    "pageLength": 5,
    "language": {
        "info": "_START_ - _END_ of _TOTAL_",
        "paginate": {
            "next": '<i class="fa-solid fa-angle-right"></i>', // Right arrow icon
            "previous": '<i class="fa-solid fa-angle-left"></i>' // Left arrow icon
        }
    }
});
console.log(table);// Initialize your DataTable


$('input[type="search"]').on('keyup', function () {
    table.search(this.value).draw();
    console.log("status.js here");

});



$('input[name="requestby"]').on('change', function () {
    var value = $(this).attr('id');
    console.log(value);
    if (value == 'requestbyAll') {
        table.columns(0).search('').draw();
    }
    else {
        table.columns(0).search(value).draw();
    }
});
$('.drawbyregiondropdown').on('change', function () {
    var value = $(this).val();
    console.log(value);
    if (value == '1234') {
        table.columns(1).search('').draw();
    }
    else {
        table.columns(1).search(value).draw();
    }
});


$('.gotoaction').click(function (e) {
    e.preventDefault();
    console.log("clicked");
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


$('.gotomodel').on('click', function (e) {
    console.log("clicked")
    var name = $(this).closest('form').attr('value');
    console.log(name);
    $('.patientname').html(name);
    var id = $(this).closest('form').data('id');

    $('.inputhiddenrequestid').val(id);
    console.log(id);

});

$('.gotomodel[data-target="#CancelCaseModal"]').on('click', function () {
    console.log("clickascaed");
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



$('.gotomodel[data-target="#AssignCasemodal"]').on('click', function () {
    console.log("clickascaed");
    debugger;
    $.ajax({
        url: 'AdminArea/Dashboard/GetRegion',
        success: function (data) {
            var regiondropdown = $('.regiondropdown');
            regiondropdown.empty();
            console.log(data);
            regiondropdown.append($('<option>', {
                value: "",
                text: ""
            }))
            console.log(regiondropdown);
            $.each(data, function (index, regions) {
                regiondropdown.append($('<option>', {
                    value: regions.regionid,
                    text: regions.name
                }))
            });
            console.log(regiondropdown);
        }
    });
    $.ajax({
        url: 'AdminArea/Dashboard/GetPhysician',
        success: function (data) {
            var physiciandropdown = $('#physiciandropdown');
            physiciandropdown.empty();
            console.log(data);
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
            console.log(physiciandropdown);

        }
    });
});
$('.gotomodel[data-target="#TransferCaseModal"]').on('click', function () {
    console.log("clickascaed");
    debugger;
    $.ajax({
        url: 'AdminArea/Dashboard/GetRegion',
        success: function (data) {
            var regiondropdown = $('.transtoregiondropdown');
            regiondropdown.empty();
            console.log(data);
            regiondropdown.append($('<option>', {
                value: "",
                text: ""
            }))
            console.log(regiondropdown);
            $.each(data, function (index, regions) {
                regiondropdown.append($('<option>', {
                    value: regions.regionid,
                    text: regions.name
                }))
            });
            console.log(regiondropdown);
        }
    });
    $.ajax({
        url: 'AdminArea/Dashboard/GetPhysician',
        success: function (data) {
            var physiciandropdown = $('#transtophysiciandropdown');
            physiciandropdown.empty();
            console.log(data);
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
            console.log(physiciandropdown);

        }
    });
});