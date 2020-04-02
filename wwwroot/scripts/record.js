function PostBlob(blob) {
    // FormData
    var formData = new FormData();
    formData.append('video-filename', fileName);
    formData.append('video-blob', blob);

    // progress-bar
    var hr = document.createElement('hr');
    container.appendChild(hr);
    var strong = document.createElement('strong');
    strong.id = 'percentage';
    strong.innerHTML = 'Video upload progress: ';
    container.appendChild(strong);
    var progress = document.createElement('progress');
    container.appendChild(progress);

    // POST the Blob using XHR2
    xhr('/Page/PostRecordedAudioVideo', formData, progress, percentage, function (fName) {
        container.appendChild(document.createElement('hr'));
        var mediaElement = document.createElement('video');

        var source = document.createElement('source');
        source.src = document.location.origin + '/uploads/' + fName.replace(/"/g, '');
        source.type = 'video/webm; codecs="vp8, vorbis"';

        mediaElement.appendChild(source);

        mediaElement.controls = true;
        container.appendChild(mediaElement);
        mediaElement.play();

        progress.parentNode.removeChild(progress);
        strong.parentNode.removeChild(strong);
        hr.parentNode.removeChild(hr);
    });
}

var record = document.getElementById('record');
var stop = document.getElementById('stop');
var deleteFiles = document.getElementById('delete');

var preview = document.getElementById('preview');

var container = document.getElementById('container');

var recordVideo;
record.onclick = function () {
    record.disabled = true;

    navigator.getUserMedia = navigator.getUserMedia || navigator.mozGetUserMedia || navigator.webkitGetUserMedia;
    navigator.getUserMedia({
        audio: true,
        video: true
    }, function (stream) {
        preview.src = stream;
        preview.play();

        recordVideo = RecordRTC(stream, {
            type: 'video'
        });

        recordVideo.startRecording();

        stop.disabled = false;
    }, function (error) {
        alert(error.toString());
    });
};



function xhr(url, data, progress, percentage, callback) {
    var request = new XMLHttpRequest();
    request.onreadystatechange = function () {
        if (request.readyState == 4 && request.status == 200) {
            callback(request.responseText);
        }
    };

    if (url.indexOf('/Page/DeleteFile') == -1) {
        request.upload.onloadstart = function () {
            percentage.innerHTML = 'Upload started...';
        };

        request.upload.onprogress = function (event) {
            progress.max = event.total;
            progress.value = event.loaded;
            percentage.innerHTML = 'Upload Progress ' + Math.round(event.loaded / event.total * 100) + "%";
        };

        request.upload.onload = function () {
            percentage.innerHTML = 'Saved!';
        };
    }

    request.open('POST', url);
    request.send(data);
}
