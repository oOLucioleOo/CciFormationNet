$(function () {

    $('.video-link').on('click',
        function () {
            var video = '<video height="480" controls="controls" autoplay="autoplay"><source src="' + this.dataset.link + '" type="' + this.dataset.type + '"></video>';
            console.log(video);
            $('#video-player').html(video);
        });

});