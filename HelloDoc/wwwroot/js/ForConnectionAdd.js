"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.start().then(function () {
    var groupName = null;
    connection.invoke("AddToGroup", groupName).catch(function (err) {
        return console.error(err.toString());
    });
    connection.invoke("MsgSent").catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});

var sendtoaspid = "";
connection.on("ReceiveMessage", function (fromname, fromid, msg, time  , msgid) {
    sendtoaspid = fromid;
    var printtime = new Date(time);
    var time = printtime.getHours() + ":" + printtime.getMinutes();
    var thisid = $('#sendtoaspid').val();
    if (thisid == fromid) {
        $('.printmsg').append('<div class="spanofrec"><div class="spanofmsg"><span>' + msg + '</span><span class="spanoftime">' + time + '</span></div></div>')
    }
    showNotification(msg + " from " + fromname);
    connection.invoke("MsgSent").catch(function (err) {
        return console.error(err.toString());
    });
});

function showNotification(message) {
    alertify.set('notifier', 'position', 'bottom-left');

    alertify.notify(message, 'success', 5);
    $('.alertify-notifier').on('click', '.ajs-message', function () {
        console.log("clicked")
        $.ajax({
            url: '/Hubs/Home/OpenChatBox',
            data: { sendtoaspid },
            type: 'POST',
            success: function (result) {
                $('#PopUps').html(result);
                var my = new bootstrap.Modal(document.getElementById('ModalToOpen'));
                my.show();
            }
        });
    })

}



