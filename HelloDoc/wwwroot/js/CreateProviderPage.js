﻿$('input[type="tel"]').each(function () {
    var iti = window.intlTelInput(this, {
        nationalMode: false,
        initialCountry: "us",
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
document.querySelectorAll('.fileToUpload').forEach(function (inputElement) {
    inputElement.addEventListener('change', function () {
        var correspondingCheckbox = this.parentElement.parentElement.querySelector('.fileToUploadCheck');
        var file = this.files[0];
        if (file) {
            var fileName = file.name;
            var fileExtension = fileName.split('.').pop().toLowerCase();
            if (fileExtension !== 'pdf') {
                alert('Please upload only PDF files.');
                $(this).val('');
                correspondingCheckbox.checked = false;
            }
            else {
                correspondingCheckbox.checked = true;
            }
        }
    });
});
var actualBtn = document.getElementById('actual-btn');
var fileChosen = document.getElementById('file-chosen');

actualBtn.addEventListener('change', function () {
    var filesnames = this.files[0].name;
    for (var i = 1; i < this.files.length; i++) {
        filesnames = filesnames + ' + ' + this.files[i].name;
    }
    fileChosen.textContent = filesnames;
    fileChosen.style.color = "black";
    fileChosen.style.fontSize = "large";
    var fileInput = document.querySelector('#actual-btn');
    var file = fileInput.files[0];
    var reader = new FileReader();
    reader.onloadend = function () {
        var base64String = reader.result;
        $('#photobase64').val(base64String);
        console.log(base64String);
        console.log($('#photobase64').val());
    };
    reader.readAsDataURL(file);
});

function usernamefill() {
    $('#usernamefield').val("MD." + $('#editfirstname').val() + "." + $('#editlastname').val());
}

$('#editfirstname').on('input', function () {
    usernamefill();
});
$('#editlastname').on('input', function () {
    usernamefill();
})



$('#CreateAccountBtn').on('click', function () {
    var id = 0;
    var email = $('#editemail').val();
    if (email == "") {
        $(this).closest('form').valid()
        $(this).closest('form').validate().focusInvalid();
        //Swal.fire("Enter Email");
    } else {
        var submit = checkemail(id, email);
        console.log("click", submit)
        if (submit) {
            console.log("click", submit)
            $(this).closest('form').submit();
        }
    }
});

$('.modal-dialog').draggable({
    handle: ".modal-header"
});

$('#editemail').on('blur', function () {
    var email = $(this).val();
    var id = 0;
    checkemail(id, email);
})

function checkemail(id, email) {
    var cansubmmit = false;
    $.ajax({
        url: '/AdminArea/AdminProviderTab/CheckEmailForPhysician',
        data: { id, email },
        async: false,
        type: 'POST',
        success: function (data) {
            if (data) {
                $('#editemail').val("");
                Swal.fire("Email Already Exist!");
                cansubmmit = !data;
                console.log("if", cansubmmit)
            }
            else {
                cansubmmit = !data;
                console.log("else", cansubmmit)
            }
        }
    });
    console.log("cansuy", cansubmmit)
    return cansubmmit;
}
$('.spanofvalid').bind("DOMSubtreeModified", function () {
    if ($(this).text().trim() !== '') {
        $(this).parents('.py-2').addClass('error');
    } else {
        $(this).parents('.py-2').removeClass('error');
    }
});