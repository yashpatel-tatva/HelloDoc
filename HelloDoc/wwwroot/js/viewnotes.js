$('#NoteSaveSubmitButton').on('click', function () {

    var requestid = $('#hiddenrequestid').val();
    var Notes = $('#AdminNotes').val();

    var model = {
        requestid: requestid,
        AdminNotes: Notes
    };


    $.ajax({
        url: 'AdminArea/Dashboard/SaveAdminNotes',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(model),
        success: function (response) {
            $('#nav-tabContent').html(response);
        },
        error: function (error) {
            console.error('Error saving admin notes:', error);
        }
    });

});
