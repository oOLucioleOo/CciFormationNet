$(function () {


    // Without the generated proxy
    var connection = $.hubConnection('http://localhost:63315/signalr');
    var streamHubProxy = connection.createHubProxy('streamHub');

    streamHubProxy.on("notify", function (title, message, type) {
        toastr[type](message, title);
    });
    
    var channel = connection.start();
    channel.done(function () {
        console.log("StreamHub connection ok");
    });

    channel.fail(function () {
        console.log("StreamHub connection failed");
    });

});