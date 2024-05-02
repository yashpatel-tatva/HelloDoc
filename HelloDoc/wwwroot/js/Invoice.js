// Get today's date
var today = new Date();
var currentYear = today.getFullYear();
var currentMonth = today.getMonth(); // Note: January is 0, February is 1, and so on.

// Function to add leading zero to single digit numbers
function addLeadingZero(num) {
    return num < 10 ? "0" + num : num;
}

// Populate the dropdown with biweekly periods
var select = document.getElementById("biweekDropdown");
for (var i = 0; i < 5; i++) {
    var year = currentYear;
    var month = currentMonth - i;
    if (month < 0) {
        month += 12;
        year--;
    }

    var option2 = document.createElement("option");
    option2.text = year + "/" + addLeadingZero(month + 1) + "/15 - " + year + "/" + addLeadingZero(month + 1) + "/" + new Date(year, month + 1, 0).getDate();
    option2.value = year + "/" + addLeadingZero(month + 1) + "/15";
    select.add(option2);

    var option1 = document.createElement("option");
    option1.text = year + "/" + addLeadingZero(month + 1) + "/01 - " + year + "/" + addLeadingZero(month + 1) + "/14";
    option1.value = year + "/" + addLeadingZero(month + 1) + "/01";
    select.add(option1);
}


$.ajax({
    url: '/AdminArea/Dashboard/GetPhysicianByRegion',
    type: 'POST',
    data: { regionid: '0' },
    success: function (data) {
        console.log(data);
        var physiciandropdown = $('#physicianid');
        physiciandropdown.empty();
        physiciandropdown.append($('<option>', {
            hidden: "hidden",
            value: "0",
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

var selecteddate = $('#biweekDropdown').val();
var physiciainid = $('#physicianid').val()
$('#physicianid').on('change', function () {
    physiciainid = $(this).val();
    $.ajax({
        url: '/AdminArea/AdminProviderTab/IsBiweekApproved',
        data: { physiciainid, selecteddate },
        type: 'POST',
        success: function (response) {
            if (response == "true") {
                $('#content1').removeClass('d-none')
                getdata(selecteddate);
            } else {
                $('#datatoprintintimesheet').html('<div class="text-muted text-center py-4">' + response + '</div>')
            }
        },
    })
})

function getdata(selecteddate) {
    $.ajax({
        url: '/AdminArea/AdminProviderTab/IsBiweekFinalized',
        data: { physiciainid, selecteddate },
        type: 'POST',
        success: function (response) {
            if (response) {
                $('#finalizetimesheet').addClass('d-none');
            } else {
                $('#finalizetimesheet').removeClass('d-none');
            }
        },
    })
    $.ajax({
        url: '/AdminArea/AdminProviderTab/TimesheetData',
        data: { physiciainid, selecteddate },
        type: 'POST',
        success: function (response) {
            $('#datatoprintintimesheet').html('<div class="text-muted text-center py-4">' + response + '</div>')
            //$('#datatoprintinreimbursement').html('<div class="p-4 text-muted">'+response+'</div>')
        },
    })
    $.ajax({
        url: '/AdminArea/AdminProviderTab/ReimbursementDataView',
        data: { physiciainid, selecteddate },
        type: 'POST',
        success: function (response) {
            //$('#datatoprintintimesheet').html('<div class="p-4 text-muted">' + response + '</div>')
            $('#datatoprintinreimbursement').html('<div class="text-muted text-center py-4">' + response + '</div>')
        },
    })
}

getdata(selecteddate);

$('#biweekDropdown').on('change', function () {
    selecteddate = $(this).val();
    getdata(selecteddate);
})

$('#finalizetimesheet').on('click', function () {
    $.ajax({
        url: '/AdminArea/AdminProviderTab/GetTimeSheet',
        data: { physiciainid, selecteddate },
        type: 'POST',
        success: function (result) {
            $('#nav-tabContent').html(result);
        }
    })
})
