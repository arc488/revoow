var time = 30;
var counter;
var secondsLeft = document.getElementById('secondsLeft');

function startCounter() {
    time = 30;
    counter = setInterval(decreaseSecond, 1000);
}

function decreaseSecond() {
    time = time - 1;
    secondsLeft.value = time;
    if (time <= 0) {
        clearInterval(counter);
    }
}

const mediaSource = new MediaSource();
mediaSource.addEventListener("sourceopen", handleSourceOpen, false);
let mediaRecorder;
var recordedBlobs;
let sourceBuffer;

const startRecordingClassName = "fa fa-circle fa-lg";
const stopRecordingClassName = "fa fa-stop fa-lg";

const errorMsgElement = document.querySelector("span#errorMsg");
const recordButton = document.querySelector("button#record");
const uploadButton = document.querySelector("input#upload");
const reviewerNameInput = document.querySelector("input#reviewerName");
const pageIdInput = document.querySelector("input#pageId");
const companyNameInput = document.querySelector("input#companyName");
const gumVideo = document.querySelector("video#gum");
const blobLengthInput = document.querySelector("#blobLength");
var isRecording = false;

recordButton.addEventListener("click", () => {
    var icon = recordButton.children[0];
    if (icon.className === startRecordingClassName) {
        startRecording();
        startCounter();
    } else {
        stopRecording();
        icon.className = startRecordingClassName;
        playButton.disabled = false;
        uploadButton.disabled = false;
        clearInterval(counter);
    }
});

const playButton = document.querySelector("button#play");
playButton.addEventListener("click", () => {
    const superBuffer = new Blob(recordedBlobs, { type: "video/webm" });
    recordButton.disabled = true;
    gumVideo.src = null;
    gumVideo.srcObject = null;
    gumVideo.src = window.URL.createObjectURL(superBuffer);
    gumVideo.controls = true;
    gumVideo.play();
});

uploadButton.addEventListener("click", () => {

    if ($("form[name='upload']").valid()) {
        console.log("Form upload is valid");
        uploadButton.disabled = true;
        disableCamera();
        var blob = new Blob(recordedBlobs, { type: "video/webm" });
        var formData = new FormData();

        formData.append("video-blob", blob);
        formData.append("ratingValue", ratingValue);
        formData.append("reviewerName", reviewerNameInput.value);
        formData.append("pageId", pageIdInput.value);
        formData.append("companyName", companyNameInput.value);

        var request = new XMLHttpRequest();
        request.open("POST", "/Page/Upload");
        request.send(formData);
        handleRequestSent(request);
    } else {
        console.log("Form upload is not valid");
    }

});
function disableCamera() {
    window.stream.getTracks().forEach(function (track) {
        track.stop();
    });
}
function handleRequestSent(request) {
    request.addEventListener("readystatechange", function () {
        console.log(this);
        if (this.readyState === 4) {
            var responseData = JSON.parse(this.responseText);
            console.log(responseData.url);
            redirectToPage(responseData.url);
        };
    });
}

function redirectToPage(redirectUrl) {
    window.location.href = redirectUrl;
}

function handleSourceOpen(event) {
    console.log("MediaSource opened");
    sourceBuffer = mediaSource.addSourceBuffer('video/webm; codecs="vp8"');
    console.log("Source buffer: ", sourceBuffer);
}

function handleDataAvailable(event) {
    console.log("handleDataAvailable", event);
    blobLengthInput.value = recordedBlobs.length;
    if (event.data && event.data.size > 0) {
        recordedBlobs.push(event.data);
    }
}

function startRecording() {
    recordedBlobs = [];
    let options = { mimeType: "video/webm;codecs=vp9" };
    if (!MediaRecorder.isTypeSupported(options.mimeType)) {
        console.error(`${options.mimeType} is not Supported`);
        errorMsgElement.innerHTML = `${options.mimeType} is not Supported`;
        options = { mimeType: "video/webm;codecs=vp8" };
        if (!MediaRecorder.isTypeSupported(options.mimeType)) {
            console.error(`${options.mimeType} is not Supported`);
            errorMsgElement.innerHTML = `${options.mimeType} is not Supported`;
            options = { mimeType: "video/webm" };
            if (!MediaRecorder.isTypeSupported(options.mimeType)) {
                console.error(`${options.mimeType} is not Supported`);
                errorMsgElement.innerHTML = `${options.mimeType} is not Supported`;
                options = { mimeType: "" };
            }
        }
    }

    try {
        mediaRecorder = new MediaRecorder(window.stream, options);
    } catch (e) {
        console.error("Exception while creating MediaRecorder:", e);
        errorMsgElement.innerHTML = `Exception while creating MediaRecorder: ${JSON.stringify(
            e
        )}`;
        return;
    }

    console.log("Created MediaRecorder", mediaRecorder, "with options", options);
    recordButton.children[0].className = stopRecordingClassName;
    playButton.disabled = true;
    uploadButton.disabled = true;
    mediaRecorder.onstop = event => {
        console.log("Recorder stopped: ", event);
        console.log("Recorded Blobs: ", recordedBlobs);
    };
    mediaRecorder.ondataavailable = handleDataAvailable;
    mediaRecorder.start(10); // collect 10ms of data
    console.log("MediaRecorder started", mediaRecorder);
}

function stopRecording() {
    mediaRecorder.stop();
}

function handleSuccess(stream) {
    recordButton.disabled = false;
    gumVideo.controls = false;
    console.log("getUserMedia() got stream:", stream);
    window.stream = stream;
    gumVideo.srcObject = stream;
}

async function init(constraints) {
    try {
        const stream = await navigator.mediaDevices.getUserMedia(constraints);
        handleSuccess(stream);
    } catch (e) {
        console.error("navigator.getUserMedia error:", e);
        errorMsgElement.innerHTML = `navigator.getUserMedia error:${e.toString()}`;
    }
}

document.querySelector("button#start").addEventListener("click", async () => {
    const hasEchoCancellation = false;
    const constraints = {
        audio: {
            echoCancellation: { exact: hasEchoCancellation }
        },
        video: {
            width: 1280,
            height: 720
        }
    };
    console.log("Using media constraints:", constraints);
    await init(constraints);
});
