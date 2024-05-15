
$('#SendBtn').on('click', function () {
    var msg = $('#sendmsg').val();
    if (msg == null || msg == "") {

    }
    else {
        var sendid = $('#sendtoaspid').val();
        var currtime = new Date();
        var time = currtime.getHours() + ":" + currtime.getMinutes();
        var read = '<i class="fa-solid fa-check tickmark ms-1"></i>';

        $('.printmsg').append('<div class="spanofsent"><div class="spanofmsg"><span>' + msg + '</span><span class="spanoftime">' + time + read + '</span></div></div>')
        $('.printmsg').scrollTop($('.printmsg')[0].scrollHeight);
        $('#sendmsg').val("");
        connection.invoke("SendMessage", sendid, msg).catch(function (err) {
            return console.error(err.toString());
        });
        connection.invoke("MsgSeen", fromthis).catch(function (err) {
            return console.error(err.toString());
        });

        event.preventDefault();
    }
})

var fromthis = $('#sendtoaspid').val();


connection.invoke("MsgSeen", fromthis).catch(function (err) {
    return console.error(err.toString());
});





var sendid = $('#sendtoaspid').val();

function showmsges() {
    $.ajax({
        url: '/Hubs/Home/ChatHistoryOfthisTwo',
        data: { opp: sendid },
        type: 'POST',
        success: function (data) {
            var current = new Date(data[0].senttime);
            var currentdate = current.getDate() + '-' + current.getMonth() + '-' + current.getFullYear();

            $('.printmsg').append('<div class="d-flex justify-content-center"><span class="p-1 rounded-3 bg-info">' + currentdate  + '</span></div>');
            $.each(data, function (index, result) {

                var rtime = new Date(result.senttime);
                var time = rtime.getHours() + ':' + rtime.getMinutes()
                var read = '<i class="fa-solid fa-check tickmark ms-1"></i>';
                if (result.issent) {
                    read = '<i class="fa-solid fa-check-double tickmark  ms-1"></i>'
                }
                if (result.isread) {
                    read = '<i class="fa-solid fa-check-double tickmark text-info ms-1"></i>'
                }
                var resultcurrent = new Date(result.senttime);
                var resultcurrentdate = resultcurrent.getDate() + '-' + resultcurrent.getMonth() + '-' + resultcurrent.getFullYear()
                if (currentdate != resultcurrentdate) {
                    currentdate = resultcurrentdate;
                    $('.printmsg').append('<div class="d-flex justify-content-center"><span class="p-1 rounded-3 bg-info">' + currentdate + '</span></div>');
                }

                if (result.reciever == sendid) {
                    $('.printmsg').append('<div class="spanofsent"><div class="spanofmsg"><span>' + result.msg + '</span><span class="spanoftime">' + time + read + '</span></div></div>')
                }
                else if (result.sender == sendid) {
                    $('.printmsg').append('<div class="spanofrec"><div class="spanofmsg"><span>' + result.msg + '</span><span class="spanoftime">' + time + '</span></div></div>')
                }
            });

            // Scroll to the bottom
            setTimeout(function () {
                $('.printmsg').scrollTop($('.printmsg')[0].scrollHeight);
            }, 100);
        }
    });

}

showmsges()