$(document).ready(function () {
    $("#nameError").hide();
    $("#ratingError").hide();
    $("#videoError").hide();

});

$(function () {
    // Initialize form validation on the registration form.
    // It has the name attribute "registration"
    $("form[name='upload']").validate({
        // Specify validation rules
        errorPlacement: function (error, element) {
            //Custom position: first name
            if (element.attr("name") == "reviewerName") {
                console.log(error);
                $("#nameError").show().text(error[0].textContent);
            }
            //Custom position: second name
            else if (element.attr("name") == "ratingValue") {
                console.log(error);
                $("#ratingError").show().text(error[0].textContent);
            }
            else if (element.attr("name") == "blobLength") {
                $("#videoError").show().text(error[0].textContent);
            }
            // Default position: if no match is met (other fields)
            else {
                console.log("Error boxes not found");
            }
        },
        ignore: "",
        rules: {
            // The key name on the left side is the name attribute
            // of an input field. Validation rules are defined
            // on the right side
            reviewerName: {
                required: true,
                minlength: 2
            },
            blobLength: {
                required: true,
                number: true,
                min: 1
            },

            ratingValue: "required",
        },
        // Specify validation error messages
        messages: {
            ratingValue: "Please give us a rating",
            reviewerName: {
                required: "Please enter your first name",
                minlength: "Your name must be at least 2 characters long"
            },
            blobLength: {
                required: "Please record a short video",
                minlength: "Please record a short video"
            },
        },
        // Make sure the form is submitted to the destination defined
        // in the "action" attribute of the form when valid
        //submitHandler: function (form) {
        //    form.submit();
        //}
    });
});