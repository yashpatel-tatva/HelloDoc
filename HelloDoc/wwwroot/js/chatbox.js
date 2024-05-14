
$('#SendBtn').on('click', function () {
    var msg = $('#sendmsg').val();
    var sendid = $('#sendtoaspid').val();
    var currtime = new Date();
    var time = currtime.getHours() + ":" + currtime.getMinutes();
    showmsges()
    connection.invoke("SendMessage", sendid, msg).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
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
            $.each(data, function (index, result) {

                var rtime = new Date(result.senttime);
                var time = rtime.getHours() + ':' + rtime.getMinutes()
                var read = '<i class="fa-solid fa-check"></i>';
                if (result.issent) {
                    read = '<i class="fa-solid fa-check-double"></i>'
                }
                if (result.isread) {
                    read = '<i class="fa-solid fa-check-double" style="color: #74C0FC;"></i>'
                }

                if (result.reciever == sendid) {
                    $('.printmsg').append('<div class="spanofsent"><div class="spanofmsg"><span>' + result.msg + '</span><span class="spanoftime">' + time + read + '</span></div></div>')
                }
                else if (result.sender == sendid) {
                    $('.printmsg').append('<div class="spanofrec"><div class="spanofmsg"><span>' + result.msg + '</span><span class="spanoftime">' + time + '</span></div></div>')
                }
            });
        }
    })
}

showmsges()