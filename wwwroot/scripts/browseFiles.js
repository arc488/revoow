const fileInput = document.querySelector("input#fileInput");
const fileName = document.querySelector("#fileName");
var fileMessage = "Uploaded file: ";

fileInput.addEventListener("change", () => {
    var str = fileInput.value.toString();
    var n = str.lastIndexOf("\\");
    var result = str.substring(n + 1);
    fileName.innerHTML = fileMessage.concat(result);
});
