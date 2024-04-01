$.ajax({
    url: '/AdminArea/Dashboard/GetRegion',
    success: function (data) {
        var drawbyregiondrop = $('.drawbyregiondrop');
        $.each(data, function (index, regions) {
            drawbyregiondrop.append($('<option>', {
                value: regions.regionid,
                text: regions.name
            }))
        });
    }
});

function getphysician(regionid) {
    $.ajax({
        url: 'AdminArea/Dashboard/GetPhysicianByRegion',
        type: 'POST',
        data: { regionid: regionid },
        success: function (data) {
            var physiciandropdown = $('.drawphysician');
            physiciandropdown.empty();
            physiciandropdown.append($('<option>', {
                hidden: "hidden",
                value: "invalid",
                text: "Select Physician"
            }))
            if (data.length == 0) {
                physiciandropdown.append($('<option>', {
                    value: "invalid",
                    disabled: "disabled",
                    text: "No Physician in this area",
                    style: "color : red"
                }))
            }
            else {
                $.each(data, function (index, phy) {
                    physiciandropdown.append($('<option>', {
                        value: phy.physicianid,
                        text: phy.firstname + " " + phy.lastname
                    }))
                });
            }
        }
    });
}

$('.drawbyregiondrop').on('change', function () {
    var regionid = $(this).val();
    if (regionid != "") {
        getphysician(regionid)
        $('.drawphysician').prop('disabled', false);
        $('.drawbyregiondrop').closest('.form-group').css("border", "1px solid darkgrey");
    }
    else if (regionid == "") {
        $('.drawphysician').prop('disabled', true);
        $('.drawphysician').empty();
        $('.drawphysician').append($('<option>', {
            hidden: "hidden",
            value: "invalid",
            text: "Select Physician"
        }))
        $('.drawbyregiondrop').closest('.form-group').css("border", "1px solid red");
    }
});

$(document).ready(function () {
    $('#startTime, #endTime').on('change', function () {
        var startTime = $('#startTime').val();
        var endTime = $('#endTime').val();

        if (startTime && endTime) {
            if (startTime > endTime) {
                Swal.fire('End time cannot be earlier than start time');
                $('#endTime').val('');
            }
            var parts1 = startTime.split(':');
            var parts2 = endTime.split(':')
            var st = (parseInt(parts1[0]) * 60) + parseInt(parts1[1]);
            var et = (parseInt(parts2[0]) * 60) + parseInt(parts2[1]);
            if ((et - st) < 120) {
                Swal.fire('Shift must be atleast 2 hrs Long');
                $('#endTime').val('');
            }
        }
    });
});
repeatdrop(!$('#repeatonoff').prop('checked'));
$('#repeatonoff').on('change', function () {
    repeatdrop(!$('#repeatonoff').prop('checked'));
});
function repeatdrop(bool) {
    $('.drawrepeat').prop('disabled', bool);
}

$('.drawphysician').on('change', function () {
    if ($(this).val() == "invalid") {
        $('.drawphysician').closest('.form-group').css("border", "1px solid red");
    }
    else {
        $('.drawphysician').closest('.form-group').css("border", "1px solid darkgrey");
    }
})
$('#shiftdate').on('blur', function () {
    var selectedDate = new Date($(this).val());
    var today = new Date();
    today.setHours(0, 0, 0, 0);
    if (selectedDate < today) {
        alert('Selected date cannot be in the past');
        $(this).val('');
        $('#shiftdate').closest('.form-group').css("border", "1px solid red");
    }
    else {
        $('#shiftdate').closest('.form-group').css("border", "1px solid darkgrey");
    }
});
$('#Createconfirmbutton').on('click', function () {
    var region = $('.drawbyregiondrop').val();
    var physician = $('.drawphysician').val();
    var shiftdate = $('#shiftdate').val();
    var starttime = $('#startTime').val();
    var endtime = $('#endTime').val();
    var repeatonoff = $('#repeatonoff').prop('checked');
    var repeatdays = [];
    var format = $('#format').val();
    $('input[name="repeatdays"]:checked').each(function () {
        repeatdays.push($(this).val());
    });
    var repeattimes = $('.drawrepeat').val();

    if (region == 0) {
        $('.drawbyregiondrop').closest('.form-group').css("border", "1px solid red");
    }
    else if (physician == "invalid") {
        $('.drawphysician').closest('.form-group').css("border", "1px solid red");
    }
    else if (shiftdate == "") {
        $('#shiftdate').closest('.form-group').css("border", "1px solid red");
    }
    else if (starttime == "") {
        Swal.fire('Select Start Time');
    }
    else if (endtime == "") {
        Swal.fire('Select End Time');
    }
    else if (repeatonoff == true && repeatdays.length === 0) {
        Swal.fire('Select atleast one Day');
    }
    else {
        console.log(region);
        console.log(physician);
        console.log(shiftdate);
        console.log(starttime);
        console.log(endtime);
        console.log(repeatonoff);
        console.log(repeatdays);
        console.log(repeattimes);
        var model = {
            region : region,
            physician : physician,
            shiftdate : shiftdate,
            starttime : starttime,
            endtime : endtime,
            repeatonoff : repeatonoff,
            repeatdays : repeatdays,
            repeattimes: repeattimes,
            format : format
        }
        console.log(model);
        $.ajax({
            url: 'AdminArea/AdminProviderTab/CreateNewShift',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(model),
            success: function (response) {
                $('#timewisedata').html(response);
                $('#CreateShiftCancelbutton').trigger('click');
            },
            error: function (error) {
                console.error(error);
            }
        });
    }
});



