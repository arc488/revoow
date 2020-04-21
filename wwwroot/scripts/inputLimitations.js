function alphanumericSpaceOnly() {
    var charCode = (event.which) ? event.which : event.keyCode;
    if ((charCode >= 48 && charCode <= 57) || (charCode >= 65 && charCode <= 90) || (charCode >= 95 && charCode <= 122) || charCode == 32) { return true; }
    else { return false; }
}

function alphanumericOnly() {
    var charCode = (event.which) ? event.which : event.keyCode;
    if ((charCode >= 48 && charCode <= 57) || (charCode >= 65 && charCode <= 90) || (charCode >= 95 && charCode <= 122) ) { return true; }
    else { return false; }
}