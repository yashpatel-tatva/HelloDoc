var role = $('#selectrole').val();
var rname = $('#rname').val();
var email = $('#emailid').val();
var createddate = $('#createddate').val();
var sentdate = $('#sentdate').val();
console.log(role, rname, email, createddate, sentdate);

if (createddate !== "") {
    createddate = new Date(createddate).toISOString();
}

if (sentdate !== "") {
    sentdate = new Date(sentdate).toISOString();
}

console.log(role, rname, email, createddate, sentdate);

function getdata(role, rname, email, createddate, sentdate) {
    console.log(role, rname, email, createddate, sentdate);
    $.ajax({
        url: '/AdminArea/AdminRecordsTab/EmailLogsdata',
        data: { role: role, rname: rname, email: email, createddate: createddate, senddate: sentdate },
        type: 'POST',
        success: function (response) {
            $('#_records').html(response);
        }
    });
}

getdata(0, "ya", "ya",  "ya", "ya");

$('#selectrole').on('change', function () {
    role = $(this).val();
    getdata(role, "ya", "ya", "ya", "ya")
});