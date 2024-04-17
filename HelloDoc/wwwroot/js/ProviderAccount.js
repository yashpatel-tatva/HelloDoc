$('.spanofvalid').bind("DOMSubtreeModified", function () {
    if ($(this).text().trim() !== '') {
        $(this).parents('.py-2').addClass('error');
    } else {
        $(this).parents('.py-2').removeClass('error');
    }
});
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

var nameregex = /^[a-zA-Z]+$/i
var emailregex = /^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/;

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
            Swal.fire("Enter Valid Password");
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
                    Swal.fire({
                        position: "top-end",
                        icon: "success",
                        title: "Saved",
                        showConfirmButton: false,
                        timer: 1000
                    });
                }
            });
        }
    }
});

$('#passsaveandcancle').on('click', '#passcancel', function () {
    $('#password').prop('disabled', true);
    $('#resetpassword').text("Reset Password");
    $(this).css('display', 'none');
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
        var firstname = $('#editfirstname').val();
        var lastname = $('#editlastname').val();
        var email = $('#editemail').val();
        var phonenumber = $('#personalinforditphone').val();
        var medicallicense = $('#editmedical').val();
        var npinumber = $('#editnpi').val();
        var syncemail = $('#editsyncemail').val();
        var selectedregion = [];
        $('input[name="physicianeditregion"]:checked').each(function () {
            selectedregion.push($(this).val());
        });
        function validateForm() {
            var firstname = $('#editfirstname').val();
            var lastname = $('#editlastname').val();
            var email = $('#editemail').val();
            var phonenumber = $('#personalinforditphone').val();

            if (!firstname || !nameregex.test(firstname)) {
                $('#editfirstname').css("border", "1px solid red");
                return false;
            }
            if (!lastname || !nameregex.test(lastname)) {
                $('#editlastname').css("border", "1px solid red");
                return false;
            }
            if (!email || !emailregex.test(email)) {
                $('#editemail').css("border", "1px solid red");
                return false;
            } else {
                var boolas = checkemail(physicianid, email);
                return boolas;
            }
            if (!phonenumber) {
                $('#personalinforditphone').css("border", "1px solid red");
                return false;
            }

            return true;
        }

        if (validateForm()) {
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
            var error = $('.spanofvalid').text().trim() !== '';
            console.log(error)
            if (!error) {
                $.ajax({
                    url: '/AdminArea/AdminProviderTab/EditProviderPersonal',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(model),
                    success: function (response) {
                        $('#nav-tabContent').html(response);
                        Swal.fire({
                            position: "top-end",
                            icon: "success",
                            title: "Saved",
                            showConfirmButton: false,
                            timer: 1000
                        });
                    }
                });
            }
        }
    }
});
$('#phyiciansaveandcancel').on('click', '#physiciancancel', function () {
    $('.personalinfordit').prop('disabled', true);
    $('input[name="physicianeditregion"]').prop('disabled', true);
    $('#physicianEdit_Save').text("Edit");
    $(this).css('display', 'none');
});

$('#addressEdit_Save').on('click', function () {
    if ($('#addressEdit_Save').text() == "Edit") {
        $('.addressedit').removeAttr("disabled");
        $('#addressEdit_Save').text("Save");
        $('#addresssaveandcancel').append('<button type="reset" class="ms-2 btn border-info text-info shadow-none" id="addresscancel">Cancel</button>');
    }
    else {
        var address1 = $('#addresseditAddress1').val();
        var address2 = $('#addresseditAddress2').val();
        var city = $('#addresseditCity').val();
        var zip = $('#addresseditZip').val();
        var state = $('#state').val();
        var billmobile = $('#addresseditBillMobile').val();
        var model = {
            PhysicianId: physicianid,
            Address1: address1,
            Address2: address2,
            City: city,
            Zip: zip,
            RegionID: state,
            BusinessPhone: billmobile,
        }
        $.ajax({
            url: '/AdminArea/AdminProviderTab/EditProviderMailingInfo',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(model),
            success: function (response) {
                $('#nav-tabContent').html(response);
                Swal.fire({
                    position: "top-end",
                    icon: "success",
                    title: "Saved",
                    showConfirmButton: false,
                    timer: 1000
                });
            }
        });
    }
});
$('#addresssaveandcancel').on('click', '#addresscancel', function () {
    $('.addressedit').prop('disabled', true);
    $('#addressEdit_Save').text("Edit");
    $(this).css('display', 'none');
});


$('#adminnoteEdit_Save').on('click', function () {
    if ($('#adminnoteEdit_Save').text() == "Edit") {
        $('.bnamewebnoteedit').removeAttr("disabled");
        $('#adminnoteEdit_Save').text("Save");
        $('#adminnotesaveandcancel').append('<button type="reset" class="ms-2 btn border-info text-info shadow-none" id="adminnotecancel">Cancel</button>');
    }
    else {
        var adminnote = $('#editadminnotes').val();
        var businessname = $('#authbusinessname').val();
        var businessweb = $('#authbusinessweb').val();

        $.ajax({
            url: '/AdminArea/AdminProviderTab/EditProviderAdminNote',
            type: 'POST',
            data: { physicianid, adminnote, businessname, businessweb },
            success: function (response) {
                $('#nav-tabContent').html(response);
                Swal.fire({
                    position: "top-end",
                    icon: "success",
                    title: "Saved",
                    showConfirmButton: false,
                    timer: 1000
                });
            }
        });
    }
});
$('#adminnotesaveandcancel').on('click', '#adminnotecancel', function () {
    $('.bnamewebnoteedit').prop('disabled', true);
    $('#adminnoteEdit_Save').text("Edit");
    $(this).css('display', 'none');
});


$('#uploadphoto').on('click', function () {
    var fileInput = document.querySelector('#actual-btn');
    var file = fileInput.files[0];
    var reader = new FileReader();
    reader.onloadend = function () {
        var base64String = reader.result;
        $.ajax({
            url: '/AdminArea/AdminProviderTab/EditProviderPhoto',
            type: 'POST',
            data: { physicianid, base64String },
            success: function (response) {
                $('#nav-tabContent').html(response);
                Swal.fire({
                    position: "top-end",
                    icon: "success",
                    title: "Photo Uploaded",
                    showConfirmButton: false,
                    timer: 1000
                });
            }
        });
    };
    reader.readAsDataURL(file);
});
$('#uploadsign').on('click', function () {
    var fileInput = document.querySelector('#actual-btn1');
    var file = fileInput.files[0];
    var reader = new FileReader();
    reader.onloadend = function () {
        var base64String = reader.result;
        $.ajax({
            url: '/AdminArea/AdminProviderTab/EditProviderSign',
            type: 'POST',
            data: { physicianid, base64String },
            success: function (response) {
                $('#nav-tabContent').html(response);
                Swal.fire({
                    position: "top-end",
                    icon: "success",
                    title: "Sign Uploaded",
                    showConfirmButton: false,
                    timer: 1000
                });
            }
        });
    };
    reader.readAsDataURL(file);
});

$('#createsign').on('click', function () {
    $.ajax({
        url: '/AdminArea/AdminProviderTab/OpenSignPadPopUp',
        type: 'POST',
        data: { physicianid },
        success: function (result) {
            $('#_SignPad').html(result);
            var my = new bootstrap.Modal(document.getElementById('ModalToOpen'));
            my.show();
        }
    });
});

$('.fileuploadbtn').on('click', function () {
    $('#SelectFileToUpload').data('url', $(this).data('url'));
    $('#SelectFileToUpload').click();
});

$('#SelectFileToUpload').on('change', function () {
    var formData = new FormData();
    formData.append('physicianid', physicianid);
    formData.append('file', this.files[0]);
    $.ajax({
        url: $(this).data('url'),
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (data) {
            $('#nav-tabContent').html(data);
            Swal.fire({
                position: "top-end",
                icon: "success",
                title: "File Uploaded",
                showConfirmButton: false,
                timer: 1000
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $('#errormessageforfile').text(jqXHR.responseText);
        }
    });
});

$('.fileviewbtn').on('click', function () {
    var resultdata = $(this).data('url');
    var result;
    if (resultdata == "/AdminArea/AdminProviderTab/ViewICAdoc") {
        result = "AgreementDoc";
    } else if (resultdata == "/AdminArea/AdminProviderTab/ViewBackDocdoc") {
        result = "BackgroundDoc";
    } else if (resultdata == "/AdminArea/AdminProviderTab/VeiwCredentialdoc") {
        result = "CredentialDoc";
    } else if (resultdata == "/AdminArea/AdminProviderTab/ViewNDAdoc") {
        result = "NonDisclosureDoc";
    } else if (resultdata == "/AdminArea/AdminProviderTab/ViewLicensedoc") {
        result = "LicenseDoc";
    }


    window.open('/PhysicianDocuments/' + physicianid + "/" + result + '.pdf');
    //$.ajax({
    //    type: 'POST',
    //    url: $(this).data('url'),
    //    data: { physicianid },
    //    success: function (result) {
    //    },
    //    error: function (xhr, status, error) {
    //        console.error('Error: ' + error);
    //    },
    //});
});

$('#DeleteAccount').on('click', function () {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/AdminArea/AdminProviderTab/DeleteProviderAccount',
                data: { physicianid },
                type: 'POST',
                success: function (response) {
                    Swal.fire({
                        title: "Deleted!",
                        text: "Account has been deleted.",
                        icon: "success"
                    });
                    $('#nav-tabContent').html(response);
                }
            });
        }
    });
});

$('#editemail').on('blur', function () {
    var email = $(this).val();
    var id = physicianid;
    checkemail(id, email);
})

function checkemail(id, email) {
    var cansubmmit = false;
    $.ajax({
        url: '/AdminArea/AdminProviderTab/CheckEmailForPhysician',
        data: { id, email },
        type: 'POST',
        async: false,
        success: function (data) {
            if (data) {
                $('#editemail').val("");
                Swal.fire("Email Already Exist!");
                cansubmmit = !data;
            }
            else {
                cansubmmit = !data;
            }
        }
    });
    return cansubmmit;
}


$('#requestadmin').on('click', function () {
    var physicianid = $('#inputhiddenid').val();
    $.ajax({
        url: '/ProviderArea/ProviderProfile/RequesttoadminPopup',
        data: { physicianid },
        type: 'POST',
        success: function (result) {
            $('#PopUps').html(result);
            var my = new bootstrap.Modal(document.getElementById('ModalToOpen'));
            my.show();
        }
    })
})