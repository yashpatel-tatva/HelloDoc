
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
        $(this).val(fullNumber);
    });
});


console.log("admin")
var password;
$('#resetpassword').on('click', function () {
    if ($('#resetpassword').text() == "Reset Password") {
        password = $('#password').val();
        $('#resetpassword').text("Save");
        $('#password').removeAttr("disabled");
        $('#passsaveandcancle').append('<button type="button" class="ms-2 btn border-info text-info shadow-none" id="passcancel">Cancel</button>');
    }
    else {
        const passwordRegex = /^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&-+=()]).{8,}$/;

        const password = $('#password').val();

        if (!passwordRegex.test(password)) {
            $('#password').css("border", "1px solid red");
        }
        else {
            $.ajax({
                url: '/AdminArea/AdminProfile/ResetPassword',
                type: 'POST',
                data: { aspnetid: $('#aspnetid').val(), password: $('#password').val() },
                success: function () {
                    $('#password').prop('disabled', true);
                    $('#resetpassword').text("Reset Password");
                    $('#passcancel').remove();
                    $('#password').css("border", "0px");
                }
            });
        }
    }
});
$('#passsaveandcancle').on('click', '#passcancel', function () {
    console.log('clicked');
    $('#password').val(password);
    $('#password').prop('disabled', true);
    $('#resetpassword').text("Reset Password");
    $(this).remove();
});

var firstname;
var lastname;
var email;
var mobile;
$('#adminEdit_Save').on('click', function () {
    console.log($('#adminEdit_Save').text());
    if ($('#adminEdit_Save').text() == "Edit") {
        $('.adminedit').removeAttr("disabled");
        $('input[name="admineditregion"]').removeAttr("disabled");
        $('#adminEdit_Save').text("Save");
        $('#adminsaveandcancel').append('<button type="reset" class="ms-2 btn border-info text-info shadow-none" id="admincancel">Cancel</button>');
        //firstname = $('#admineditfirstname').val();
        //lastname = $('#admineditlastname').val();
        //email = $('#admineditemail').val();
        //mobile = $('#admineditmobile').val();
    }
    else {
        console.log("click");
        //var selectedCountryData = phoneInput.getSelectedCountryData();
        //var selectedCountryDialCode = selectedCountryData.dialCode;
        //mobile = $('#admineditmobile').val();
        //$('#admineditmobile').val("+" + selectedCountryDialCode + mobile);
        function validateForm() {
            var firstname = $('#admineditfirstname').val();
            var lastname = $('#admineditlastname').val();
            var email = $('#admineditemail').val();
            var mobile = $('#admineditmobile').val();

            if (!firstname) {
                $('#admineditfirstname').css("border", "1px solid red");
                return false;
            }
            if (!lastname) {
                $('#admineditlastname').css("border", "1px solid red");
                return false;
            }
            if (!email) {
                $('#admineditemail').css("border", "1px solid red");
                return false;
            }
            if (!mobile) {
                $('#admineditmobile').css("border", "1px solid red");
                return false;
            }

            return true;
        }
        if (validateForm()) {
            $(this).closest('form').submit();
        }
    }
});
$('#adminsaveandcancel').on('click', '#admincancel', function () {
    //$('#admineditfirstname').val(firstname);
    //$('#admineditlastname').val(lastname);
    //$('#admineditemail').val(email);
    //$('#admineditmobile').val(mobile);
    $('.adminedit').prop('disabled', true);
    $('input[name="admineditregion"]').prop('disabled', true);
    $('#adminEdit_Save').text("Edit");
    $(this).css('display', 'none');
});

var address1;
var address2;
var city;
var zip;
var state;
var billmobile;
$('#addressEdit_Save').on('click', function () {
    if ($('#addressEdit_Save').text() == "Edit") {
        $('.addressedit').removeAttr("disabled");
        $('#addressEdit_Save').text("Save");
        $('#addresssaveandcancel').append('<button type="reset" class="ms-2 btn border-info text-info shadow-none" id="addresscancel">Cancel</button>');
        //address1 = $('#addresseditAddress1').val();
        //address2 = $('#addresseditAddress2').val();
        //city = $('#addresseditCity').val();
        //zip = $('#addresseditZip').val();
        //state = $('#state').val();
        //billmobile = $('#addresseditBillMobile').val();
    }
    else {
        console.log("click");
        $(this).closest('form').submit();
    }
});
$('#addresssaveandcancel').on('click', '#addresscancel', function () {
    //$('#addresseditAddress1').val(address1);
    //$('#addresseditAddress2').val(address2);
    //$('#addresseditCity').val(city);
    //$('#addresseditZip').val(zip);
    //$('#state').val(state);
    //$('#addresseditBillMobile').val(billmobile);
    $('.addressedit').prop('disabled', true);
    $('#addressEdit_Save').text("Edit");
    $(this).css('display', 'none');
});