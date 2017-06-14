$(function () {

    $.ajaxPrefilter(function (options) {
        if (!options.beforeSend) {
            options.beforeSend = function (xhr) {
                xhr.setRequestHeader("Access-Control-Allow-Origin", "*");
            }
        }
    });

    // Without the generated proxy
    var connection = $.hubConnection("http://localhost:63315");
    var streamHubProxy = connection.createHubProxy("streamHub");

    streamHubProxy.on("notify", function (title, message, type) {
        toastr[type](message, title);
    });
    
    var channel = connection.start({jsonp: true});
    channel.done(function () {
        console.log("StreamHub connection ok");
    });

    channel.fail(function (error) {
        console.log("StreamHub connection failed");
        console.log(error);
    });

});