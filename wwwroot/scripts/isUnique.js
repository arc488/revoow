$().ready(function () {
    console.log(namesInUse);
    $.validator.addMethod("uniqueCompanyName",
        function (value, element) {
            console.log(element);
            console.log(value);
            var result = false;
            $.each(namesInUse, function (index, name) {
                alert(value + ": " + value);
                console.log(name);
                if (value == name) result = true;
            });
            // return true if username is exist in database
            return result;
        },
        "This company name is already taken in use."
    );

    // validate signup form on keyup and submit
    $("#detail").validate({
        rules: {
            "CompanyName": {
                required: true,
                uniqueCompanyName: true
            }
        }
    });
});        