$('input[type="tel"]').each(function () {
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
        $(this).val(fullNumber);
    });
});
document.querySelectorAll('.fileToUpload').forEach(function (inputElement) {
    inputElement.addEventListener('change', function () {
        var correspondingCheckbox = this.parentElement.parentElement.querySelector('.fileToUploadCheck');
        if (this.value) {
            correspondingCheckbox.checked = true;
        } else {
            correspondingCheckbox.checked = false;
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
    
    $(this).closest('form').submit();
});