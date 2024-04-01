//$.ajax({
//    url: '/AdminArea/AdminProviderTab/SetAllPhysican',
//    success: function () { }
//});













var status = 0;
var region = 0;






$.ajax({
    url: '/AdminArea/Dashboard/GetRegion',
    success: function (data) {
        var drawbyregiondropdown = $('.drawbyregiondropdown');
        $.each(data, function (index, regions) {
            drawbyregiondropdown.append($('<option>', {
                value: regions.regionid,
                text: regions.name
            }))
        });
    }
});

$('#createnewshift').on('click', function () {
    var format = $('input[name="showby"]:checked').attr('id');
    console.log(format);
    $.ajax({
        url: '/AdminArea/AdminProviderTab/CreateShiftPopUp',
        data: {format},
        success: function (result) {
            $('#PopUps').html(result);
            var my = new bootstrap.Modal(document.getElementById('ModalToOpen'));
            my.show();
        },
    });
});

$('.drawbyregiondropdown').on('change', function () {
    region = $('.drawbyregiondropdown').val();
    GetData(showby, currentDate, region, status);
});

$('input[name="shiftstatus"]').on('click', function () {
    if (status == $(this).val()) {
        status = 0;
        $(this).prop('checked' , false);
    }
    else {
        status = $(this).val();
    }
    GetData(showby, currentDate, region, status);
});

var currentDate = new Date();
var showby = $('input[name="showby"]:checked').attr('id');
GetData(showby, currentDate, region, status);

$('input[name="showby"]').on('change', function () {
    showby = $('input[name="showby"]:checked').attr('id');
    GetData(showby, currentDate, region, status);
})
function updateText() {
    var textElement = document.getElementById('timesheet');
    if (showby == "DayWiseData") {
        var options = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
        textElement.innerText = currentDate.toLocaleDateString('en-US', options);
    } else if (showby == "WeekWiseData") {
        var startOfWeek = new Date(currentDate);
        startOfWeek.setDate(startOfWeek.getDate() - startOfWeek.getDay());
        var endOfWeek = new Date(startOfWeek);
        endOfWeek.setDate(endOfWeek.getDate() + 6);
        var options = { year: 'numeric', month: 'short', day: 'numeric' };
        textElement.innerText = startOfWeek.toLocaleDateString('en-US', options) + ' - ' + endOfWeek.toLocaleDateString('en-US', options);
    } else if (showby == "MonthWiseData") {
        textElement.innerText = currentDate.toLocaleString('default', { month: 'long' }) + ' ' + currentDate.getFullYear();
    }
}

$('#previousbtn').on('click', function () {
    if (showby == "DayWiseData") {
        currentDate.setDate(currentDate.getDate() - 1);
    } else if (showby == "WeekWiseData") {
        currentDate.setDate(currentDate.getDate() - 7);
    } else if (showby == "MonthWiseData") {
        currentDate.setMonth(currentDate.getMonth() - 1);
    }
    GetData(showby, currentDate, region, status);
});
$('#nextbtn').on('click', function () {
    if (showby == "DayWiseData") {
        currentDate.setDate(currentDate.getDate() + 1);
    } else if (showby == "WeekWiseData") {
        currentDate.setDate(currentDate.getDate() + 7);
    } else if (showby == "MonthWiseData") {
        currentDate.setMonth(currentDate.getMonth() + 1);
    }
    GetData(showby, currentDate, region, status);
});


$('#selectday').on('change', function () {
    var selectedDate = new Date($('#selectday').val());
    currentDate = selectedDate;
    GetData(showby, currentDate, region, status);
});


function GetData(showby, currentDate, region, status) {
    if (showby == "DayWiseData") {
        $('#selectday').attr('type', 'date');
    } else if (showby == "WeekWiseData") {
        $('#selectday').attr('type', 'date');
    } else if (showby == "MonthWiseData") {
        $('#selectday').attr('type', 'month');
    }
    $.ajax({
        url: '/AdminArea/AdminProviderTab/' + showby,
        data: { datetoshow: currentDate.toISOString(), region: region, status: status },
        type: 'POST',
        success: function (response) {
            updateText();
            $('#timewisedata').html(response);
        }
    });
}

///////////

$('.events').on('click', function () {
    console.log($(this).data('id'));
})