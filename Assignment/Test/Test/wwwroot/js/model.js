$('#save').on('click', function () {
    if ($('input[name="gender"]:checked').length === 0) {
        Swal.fire("Select Gender!");
    }
    else if ($('#age').val() == 0) {
        Swal.fire("Add Age!");
    }
    else if ($('#age').val() >= 100) {
        Swal.fire("Enter Valid!");
        $('#age').val("")
    }
    else {
        $(this).closest('form').submit();
    }
})


$('#email').on('blur', function () {
    var email = $(this).val();
    var id = $('#id').val();
    $.ajax({
        url: '/Home/CheckEmail',
        data: { email, id },
        type: 'POST',
        success: function (data) {
            if (data) {
                Swal.fire("Email already Exist!");
                $('#email').val("");
            }
            else {
            }
        }
    })
})


$.ajax({
    url: 'Home/GetDiesesData',
    type: 'POST',
    success: function (list) {
        var dieses = $('#listofDieses');
        dieses.empty();
        $.each(list, function (index, item) {
            dieses.append($('<option>', {
                value: item,
                text: item
            }))
        });
    }
})


$('#Dieses').on('blur', function () {
    var Dieses = $(this).val();
    $.ajax({
        url: 'Home/GetDoctorData',
        type: 'POST',
        data: { Dieses },
        success: function (list) {
            var doctor = $('#listofDoctor');
            doctor.empty();
            $.each(list, function (index, item) {
                doctor.append($('<option>', {
                    value: item,
                    text: item
                }))
            });
        }
    })
})