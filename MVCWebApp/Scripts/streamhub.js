$(function () {

    // Without the generated proxy
    var connection = $.hubConnection('http://localhost:63315/signalr');
    var streamHubProxy = connection.createHubProxy('streamHub');
    streamHubProxy.on('sendMessage', function (name, message) {
        console.log(name + ' ' + message);
        $('#discussion').append("<li>" + name + " : " + message + "</li>");
    });
    var channel = connection.start();

    channel.done(function () {
        // Wire up Send button to call SendMessage on the server.
        $('#sendMessage').click(function () {
            streamHubProxy.invoke('sendMessage', $('#displayname').val(), $('#message').val());
            $('#message').val('').focus();
        });
    });

    channel.fail(function (a, b, c) {
        console.log('ERROR');
    });

});