var selectstatus = "all";
var patientname;
var selecttype = 0;
var fromdate;
var todate;
var providername;
var emailid;
var mobile;

var pagesize = 5;
var currentpage = 1;
var order = true;
var buttoncount;


function getdata(selectstatus, patientname, selecttype, fromdate, todate, providername, emailid, mobile, currentpage, pagesize, order) {
    filterDatawithoutpagination(selectstatus, patientname, selecttype, fromdate, todate, providername, emailid, mobile)
    $.ajax({
        url: '/AdminArea/AdminRecordsTab/SearchRecordsdata',
        data: { selectstatus: selectstatus, patientname: patientname, selecttype: selecttype, fromdate: fromdate, todate: todate, providername: providername, emailid: emailid, mobile: mobile, currentpage: currentpage, pagesize: pagesize, order: order },
        type: 'POST',
        success: function (response) {
            $('#_records').html(response);
            if (order) {
                $('.bi').addClass("bi-arrow-up")
                $('.bi').removeClass("bi-arrow-down")
            }
            else {
                $('.bi').addClass("bi-arrow-down")
                $('.bi').removeClass("bi-arrow-up")
            }
        }
    });
}



getdata("all", "", 0, "", "", "", "", "", 1, pagesize, order);


$('#submitdate').on('click', function () {
    selectstatus = $('#selectstatus').val();
    patientname = $('#patientname').val();
    selecttype = $('#selecttype').val();
    fromdate = $('#fromdate').val();
    todate = $('#todate').val();
    providername = $('#providername').val();
    emailid = $('#emailid').val();
    mobile = $('#mobile').val();
    currentpage = 1;
    getdata(selectstatus, patientname, selecttype, fromdate, todate, providername, emailid, mobile, currentpage, pagesize, order);
})
$('#clearbtn').on('click', function () {
    selectstatus = "all";
    patientname = "";
    selecttype = 0;
    fromdate = "";
    todate = "";
    providername = "";
    emailid ="";
    mobile = "";
    currentpage = 1;
    getdata(selectstatus, patientname, selecttype, fromdate, todate, providername, emailid, mobile, currentpage, pagesize, order);
});
$('.dataTables_paginate').on('click', '.paginate_button', function () {
    currentpage = $(this).data('id');
    getdata(selectstatus, patientname, selecttype, fromdate, todate, providername, emailid, mobile, currentpage, pagesize, order);
});
$('.paginate_Previousbutton').on('click', function () {
    if (currentpage != 1) {
        currentpage = currentpage - 1;
        getdata(selectstatus, patientname, selecttype, fromdate, todate, providername, emailid, mobile, currentpage, pagesize, order);
    }
});
$('.paginate_Nextbutton').on('click', function () {
    if (buttoncount != currentpage) {
        currentpage = currentpage + 1;
        getdata(selectstatus, patientname, selecttype, fromdate, todate, providername, emailid, mobile, currentpage, pagesize, order);
    }
});

function filterDatawithoutpagination(selectstatus, patientname, selecttype, fromdate, todate, providername, emailid, mobile) {
    $.ajax({
        url: '/AdminArea/AdminRecordsTab/SearchRecordsCount',
        type: 'POST',
        data: { selectstatus: selectstatus, patientname: patientname, selecttype: selecttype, fromdate: fromdate, todate: todate, providername: providername, emailid: emailid, mobile: mobile },
        success: function (data) {
            printbuttons(data);
            $('.paginate_button').removeClass("current");
            $('.paginate_button[data-id="' + currentpage + '"]').addClass("current");
        }
    });
}

function printbuttons(data) {
    buttoncount = Math.ceil(data / pagesize);
    $('.dataTables_paginate').html("");
    var btn = Math.min(5, buttoncount);
    var j = currentpage;
    if (buttoncount == 0) {
        $('.dataTables_paginate').html("No Records Found");
    }
    else if (buttoncount <= 5) {
        for (var i = 1; i <= btn; i++) {
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + i + '">' + i + '</div > ');
        }
    }
    else {
        if (currentpage >= buttoncount - 3) {
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + 1 + '">' + 1 + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_buttonNaN" data-id="' + '...' + '">' + '...' + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + (buttoncount - 3) + '">' + (buttoncount - 3) + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + (buttoncount - 2) + '">' + (buttoncount - 2) + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + (buttoncount - 1) + '">' + (buttoncount - 1) + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + buttoncount + '">' + buttoncount + '</div > ');
        }
        else if (currentpage < 4) {
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + 1 + '">' + 1 + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + 2 + '">' + 2 + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + 3 + '">' + 3 + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + 4 + '">' + 4 + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_buttonNaN" >' + '...' + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + buttoncount + '">' + buttoncount + '</div > ');
        }
        else {
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + 1 + '">' + 1 + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_buttonNaN" data-id="' + '...' + '">' + '...' + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + (currentpage - 1) + '">' + (currentpage - 1) + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + currentpage + '">' + currentpage + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + (currentpage + 1) + '">' + (currentpage + 1) + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_buttonNaN" data-id="' + '...' + '">' + '...' + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + buttoncount + '">' + buttoncount + '</div > ');
        }
    }
    if (currentpage == 1) {
        $('.paginate_Previousbutton').addClass("d-none");
    }
    else {
        $('.paginate_Previousbutton').removeClass("d-none");
    }
    if (buttoncount == currentpage) {
        $('.paginate_Nextbutton').addClass("d-none");
        $('#tothisshift').html(data);
    }
    else {
        $('.paginate_Nextbutton').removeClass("d-none");
        if (data == 0) {
            $('#tothisshift').html(0);
        }
        else {
            $('#tothisshift').html(((currentpage) * pagesize));
        }
    }
    if (data == 0) {
        $('#fromthisshift').html(0);
    }
    else {
        $('#fromthisshift').html((((currentpage - 1) * pagesize) + 1));
    }
    $('#totalcountofshift').html(data);
}


$('#exportall').on('click', function () {
    $.ajax({
        url: '/AdminArea/AdminRecordsTab/SearchRecordsdatatoexcle',
        data: { selectstatus: selectstatus, patientname: patientname, selecttype: selecttype, fromdate: fromdate, todate: todate, providername: providername, emailid: emailid, mobile: mobile, order: order },
        type: 'POST',
        success: function (base64String) {
            var byteCharacters = atob(base64String);
            var byteNumbers = new Array(byteCharacters.length);
            for (var i = 0; i < byteCharacters.length; i++) {
                byteNumbers[i] = byteCharacters.charCodeAt(i);
            }
            var byteArray = new Uint8Array(byteNumbers);
            var blob = new Blob([byteArray], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            var date = new Date();
            var day = String(date.getDate()).padStart(2, '0');
            var month = String(date.getMonth() + 1).padStart(2, '0'); //January is 0!
            var year = date.getFullYear();
            var hours = date.getHours();
            var minutes = date.getMinutes();
            var fileName = 'All_Records_' + day + '_' + month + '_' + year + '_' + hours + '_' + minutes + '.xlsx';
            link.download = fileName;
            link.click();
        }
    });
});