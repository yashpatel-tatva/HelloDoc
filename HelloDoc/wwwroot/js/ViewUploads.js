$('.deletebtn').on('click', function () {
    var id = $(this).val();
    $.ajax({
        type: 'POST',
        url: 'AdminArea/Dashboard/Delete',
        data: { id: id },
        success: function (result) {
            Swal.fire({
                position: "top-end",
                icon: "success",
                title: "File Deleted",
                showConfirmButton: false,
                timer: 1000
            });
            $('#nav-tabContent').html(result);
        },
        error: function (xhr, status, error) {
            console.error('Error: ' + error);
        },
    });
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
        $(this).val(fullNumber);
    });
});
$('.deleteall').on('click', function () {
    $('.childCheckbox:checked').each(function () {
        var id = $(this).val();
        $.ajax({
            type: 'POST',
            url: 'AdminArea/Dashboard/Delete',
            data: { id: id },
            success: function (result) {
                Swal.fire({
                    position: "top-end",
                    icon: "success",
                    title: "Files Deleted",
                    showConfirmButton: false,
                    timer: 1000
                });
                $('#nav-tabContent').html(result);
            },
            error: function (xhr, status, error) {
                console.error('Error: ' + error);
            },
        });
    });
});
$('.downloadall').on('click', function () {
    if ($('.childCheckbox:checked').length === 0) {
        alert("Please select at least one item.");
    }
    else {
        $(this).closest('form').attr('action', '/AdminArea/Dashboard/Download');
        $(this).closest('form').submit();
    }
});
$('.sendemail').on('click', function () {
    if ($('.childCheckbox:checked').length === 0) {
        alert("Please select at least one item.");
    }
    else {

        var formData = new FormData();
        $('input[name="downloadselect"]:checked').each(function () {
            formData.append('RequestWiseFileId', $(this).val());
        });

        formData.append('RequestsId', $('.RequestsId').val());

        $.ajax({
            url: 'AdminArea/Dashboard/SendMail',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                Swal.fire({
                    position: "top-end",
                    icon: "success",
                    title: "Files Sent",
                    showConfirmButton: false,
                    timer: 1000
                });
                $('#nav-tabContent').html(response);
            },
            error: function (error) {
                console.error('Error uploading files:', error);
            }
        });
    }
});

$('.deleteall').on('click', function () {
    if ($('.childCheckbox:checked').length === 0) {
        alert("Please select at least one item.");
    }
});

$(document).ready(function () {
    $('#dtBasicExample1').DataTable({
        info: false,
        ordering: true,
        paging: false,
        searching: false,
        autoWidth: false
    });
});
$('.viewfile').on('click', function () {
    var id = $(this).val();
    var requestid = $('#hiddenrequestid').val();
    $.ajax({
        type: 'POST',
        url: 'AdminArea/Dashboard/ViewFile',
        data: { id: id },
        success: function (result) {
            window.open('/Documents/' + requestid + "/" + result);
        },
        error: function (xhr, status, error) {
            console.error('Error: ' + error);
        },
    });
});
var beforeemail;
var beforemobile;
$('#Edit_Save').on('click', function () {
    if ($('#Edit_Save').text() == "Edit") {
        $('#Edit_Save').html("Save");
        $('#Close_Cancle').html("Cancel")
        $('.CloseCasePatientMobile').removeAttr('disabled');
        $('.CloseCasePatientEmail').removeAttr('disabled');
        beforeemail = $("input[type='email']").val();
        beforemobile = $("input[type='tel']").val();
    }
    else if ($('#Edit_Save').text() == "Save") {
        var requestid = $('#inputhiddenrequestid').val();
        var email = $("input[type='email']").val();
        var phone = $("input[type='tel']").val();
        var model = {
            requestid: requestid,
            patientemail: email,
            patientmobile: phone,
            pageredirectto: "CloseCase"
        };


        $.ajax({
            type: 'POST',
            url: 'AdminArea/Dashboard/EditEmailPhone',
            contentType: 'application/json',
            data: JSON.stringify(model),
            success: function (response) {
                Swal.fire({
                    position: "top-end",
                    icon: "success",
                    title: "Email and Mobile Edited",
                    showConfirmButton: false,
                    timer: 1000
                });
                $('#nav-tabContent').html(response);
            },
            error: function (error) {
                console.error('Error saving admin notes:', error);
            }
        });
    }
});
$('#Close_Cancle').on('click', function () {
    if ($('#Edit_Save').text() == "Edit") {
        var requestid = $('#inputhiddenrequestid').val();
        $.ajax({
            url: '/AdminArea/Dashboard/CloseCaseSubmit',
            type: 'POST',
            data: { requestid: requestid },
            success: function (result) {
                Swal.fire({
                    position: "top-end",
                    icon: "success",
                    title: "Case Closed",
                    showConfirmButton: false,
                    timer: 1000
                });
                setTimeout(function () {
                    location.reload();
                }, 1000);
            },
            error: function (xhr, status, error) {
                console.error('Error: ' + error);
            },
        });
    }
    else if ($('#Edit_Save').text() == "Save") {
        $('#Edit_Save').html("Edit");
        $('#Close_Cancle').html("Close Case")
        $('.CloseCasePatientMobile').prop('disabled', true);
        $('.CloseCasePatientEmail').prop('disabled', true);
        $("input[type='email']").val(beforeemail);
        $("input[type='tel']").val(beforemobile);
    }
});