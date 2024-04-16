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
        $(this).val(fullNumber.replace(" ",""));
    });
});

$('#SendLinkSubmitBtn').on('click', function () {
    console.log('clicked');
    var firstname = $('#sendlinkfirstname').val();
    var lastname = $('#sendlinklastname').val();
    var mobile = $('#sendlinkmobile').val();
    var email = $('#sendlinkemail').val();
    var flage = true;
    console.log(email);
    var regex = /^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$/;

    if (email == null || email == "" || !regex.test(email)) {
        $('#sendlinkemail').css("border", "1px solid red");
        flage = false;
    }
    if (mobile == null || mobile == "") {
        $('#sendlinkmobile').css("border", "1px solid red");
        flage = false;
    }
    if (flage == true) {
        $.ajax({
            url: '/AdminArea/Dashboard/SendEmailFromSendLinkPopUp',
            type: 'POST',
            data: { firstname: firstname, lastname: lastname, email: email, mobile: mobile },
            success: function () {
                Swal.fire({
                    position: "top-end",
                    icon: "success",
                    title: "Email Sent",
                    showConfirmButton: false,
                    timer: 1000
                });
                setTimeout(function () {
                    location.reload();
                }, 1000);
            },
            error: function (error) {
                console.error('Error:', error);
            }
        });
    }
});