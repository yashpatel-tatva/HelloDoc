﻿var phoneInputField = document.querySelector("#personalinforditphone");
var phoneInput = window.intlTelInput(phoneInputField, {
    utilsScript: "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
});
var password;
var physicianid = $('#inputhiddenid').val();
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
                url: '/AdminArea/AdminProviderTab/ProviderResetPassword',
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
$('#physicianEdit_Save').on('click', function () {
    console.log($('#physicianEdit_Save').text());
    if ($('#physicianEdit_Save').text() == "Edit") {
        $('.personalinfordit').removeAttr("disabled");
        $('input[name="physicianeditregion"]').removeAttr("disabled");
        $('#physicianEdit_Save').text("Save");
        $('#phyiciansaveandcancel').append('<button type="reset" class="ms-2 btn border-info text-info shadow-none" id="physiciancancel">Cancel</button>');
    }
    else {
        var selectedCountryData = phoneInput.getSelectedCountryData();
        var selectedCountryDialCode = selectedCountryData.dialCode;
        console.log("Selected country dial code: ", selectedCountryDialCode);
        console.log("click");
        var firstname = $('#editfirstname').val();
        var lastname = $('#editlastname').val();
        var email = $('#editemail').val();
        var phonenumber = "+" + selectedCountryDialCode +$('#personalinforditphone').val();
        var medicallicense = $('#editmedical').val();
        var npinumber = $('#editnpi').val();
        var syncemail = $('#editsyncemail').val();
        var selectedregion = [];
        $('input[name="physicianeditregion"]:checked').each(function () {
            selectedregion.push($(this).val());
        });
        var model = {
            PhysicianId: physicianid,
            FirstName: firstname,
            LastName: lastname,
            Email: email,
            Phone: phonenumber,
            MedicalLicense: medicallicense,
            NPINumber: npinumber,
            SynchronizationEmail: syncemail,
            SelectedRegionCB: selectedregion
        }
        console.log(JSON.stringify(model));
        $.ajax({
            url: '/AdminArea/AdminProviderTab/EditProviderPersonal',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(model),
            success: function (response) {
                $('#nav-tabContent').html(response);
            }
        });
    }
});
$('#phyiciansaveandcancel').on('click', '#physiciancancel', function () {
    $('.personalinfordit').prop('disabled', true);
    $('input[name="physicianeditregion"]').prop('disabled', true);
    $('#physicianEdit_Save').text("Edit");
    $(this).css('display', 'none');
});