
window.addEventListener("load", function () {
    var stripe = Stripe('pk_live_fvwsjXzOJtbLg6cl79zT5NDX00lsUedvue');
    console.log("Session is is " + session);
    stripe.redirectToCheckout({
        // Make the id field from the Checkout Session creation API response
        // available to this file, so you can provide it as parameter here
        // instead of the {{CHECKOUT_SESSION_ID}} placeholder.
        sessionId: session
    }).then(function (result) {
    // If `redirectToCheckout` fails due to a browser or network
    // error, display the localized error message to your customer
    // using `result.error.message`.
    });
});


