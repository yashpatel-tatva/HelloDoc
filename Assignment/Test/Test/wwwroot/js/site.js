$('input[type="tel"]').each(function () {
    var iti = window.intlTelInput(this, {
        nationalMode: false,
        utilsScript: "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js"
    });
    $(this).on('blur', function () {
        var fullNumber = iti.getNumber();
        var countryCode = iti.getSelectedCountryData().dialCode;
        if (fullNumber.startsWith("+" + countryCode + "+" + countryCode)) {
            fullNumber = fullNumber.replace("+" + countryCode + "+", "+");
        }
        console.log(fullNumber);
        $(this).val(fullNumber.replace(" ", ""));
    });
});




$('#addpatient').on('click', function () {
    var id = $(this).data('id');
    $.ajax({
        url: '/Home/OpenPatientModel',
        data: { id },
        type: 'POST',
        success: function (result) {
            $('#PopUps').html(result);
            var my = new bootstrap.Modal(document.getElementById('ModalToOpen'));
            my.show();
        }
    })
})


var currentpage = 1;
var pagesize = $('#pagesizedropdown').val();
var search = $('#search').val();

console.log(currentpage, pagesize, search);

currentpage = 1;
filter(currentpage, pagesize, search)

$("#search").on('keyup', function () {
    search = $(this).val();
    currentpage = 1
    filter(currentpage, pagesize, search)
})
$('#pagesizedropdown').on('change', function () {
    pagesize = $('#pagesizedropdown').val();
    currentpage = 1
    filter(currentpage, pagesize, search)
});
$('.dataTables_paginate').on('click', '.paginate_button', function () {
    currentpage = $(this).data('id');
    filter($(this).data('id'), pagesize, search);
});
$('.paginate_Previousbutton').on('click', function () {
    if (currentpage != 1) {
        currentpage = currentpage - 1;
        filter(currentpage, pagesize, search)
    }
});
$('.paginate_Nextbutton').on('click', function () {
    if (buttoncount != currentpage) {
        currentpage = currentpage + 1;
        filter(currentpage, pagesize, search)
    }
});



function filterwithoutpagination(search) {
    $.ajax({
        url: '/Home/PatientCountbyFilter',
        type: 'POST',
        data: {search},
        success: function (data) {
            printbuttons(data);
            $('.paginate_button').removeClass("current");
            console.log(currentpage);
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
        $('#tothisrequest').html(data);
    }
    else {
        $('.paginate_Nextbutton').removeClass("d-none");
        if (data == 0) {
            $('#tothisrequest').html(0);
        }
        else {
            $('#tothisrequest').html(((currentpage) * pagesize));
        }
    }
    if (data == 0) {
        $('#fromthisrequest').html(0);
    }
    else {
        $('#fromthisrequest').html((((currentpage - 1) * pagesize) + 1));
    }
    $('#totalcountofrequest').html(data);
}
function filter(currentpage, pagesize, search) {
    filterwithoutpagination(search);
    $.ajax({
        url: '/Home/PatientData',
        type: 'POST',
        data: { currentpage, pagesize, search },
        success: function (response) {
            $('#dashtables').html(response);
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
}

