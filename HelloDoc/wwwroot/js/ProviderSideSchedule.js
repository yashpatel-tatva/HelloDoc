var status = localStorage.getItem('status') || 0;
var currentDate = localStorage.getItem('currentDate') ? new Date(localStorage.getItem('currentDate')) : new Date();
if (!localStorage.getItem('status')) localStorage.setItem('status', status);
if (!localStorage.getItem('currentDate')) localStorage.setItem('currentDate', currentDate);

if (status != 0) {
    if (status == 1) {
        $('#pending').prop('checked', true);
    }
    if (status == 2) {
        $('#approved').prop('checked', true);
    }
}


$('#createnewshift').on('click', function () {
    var format = $('input[name="showby"]:checked').attr('id');
    console.log(format);
    $.ajax({
        url: '/AdminArea/AdminProviderTab/CreateShiftPopUp',
        data: { format },
        success: function (result) {
            $('#PopUps').html(result);
            var my = new bootstrap.Modal(document.getElementById('ModalToOpen'));
            my.show();
        },
    });
});

$('#previousbtn').on('click', function () {
    currentDate.setMonth(currentDate.getMonth() - 1);
    GetData(currentDate, status);
});
$('#nextbtn').on('click', function () {
    currentDate.setMonth(currentDate.getMonth() + 1);
    GetData(currentDate, status);
});


$('#selectday').on('change', function () {
    var selectedDate = new Date($('#selectday').val());
    currentDate = selectedDate;
    GetData(currentDate, status);
});

$('input[name="shiftstatus"]').on('click', function () {
    if (status == $(this).val()) {
        status = 0;
        $(this).prop('checked', false);
    }
    else {
        status = $(this).val();
    }
    GetData(currentDate, status);
});

function updateText() {
    var textElement = document.getElementById('timesheet');

    var firstDay = new Date(currentDate.getFullYear(), currentDate.getMonth(), 1);
    var firstDayFormatted = firstDay.toLocaleDateString('en-US', { month: 'short', day: 'numeric' });

    var lastDay = new Date(currentDate.getFullYear(), currentDate.getMonth() + 1, 0);
    var lastDayFormatted = lastDay.toLocaleDateString('en-US', { month: 'short', day: 'numeric', year: 'numeric' });

    textElement.innerText = "Schedule for: "+ firstDayFormatted + ' - ' + lastDayFormatted;
}


function GetData(currentdate, status) {
    localStorage.setItem('status', status);
    localStorage.setItem('currentDate', currentDate);
    var region = 0;
    $.ajax({
        url: '/AdminArea/AdminProviderTab/MonthWiseData',
        data: { datetoshow: currentDate.toISOString(), region: region, status: status },
        type: 'POST',
        success: function (response) {
            updateText();
            $('#timewisedata').html(response);
        }
    });
}

    GetData(currentDate , status)