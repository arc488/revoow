var $SubscriptionTypeInput = $('#Input_SubscriptionType');

var addclass = 'shadow';
var $cols = $('.card-pricing').click(function (e) {
    var plan = $(this).find('#PlanValue').val();
    $cols.removeClass(addclass);
    $(this).addClass(addclass);
    $("button[name='choosePlanButton']").removeClass("btn-primary").addClass('btn-outline-secondary');
    $(this).find("button[name='choosePlanButton']").addClass("btn-primary").removeClass('btn-outline-secondary');
    $SubscriptionTypeInput.val(plan).change();

});

if ($("#currentPlanValue").length && $("#isCanceled").val() !== "True"){
    var currentPlan = $("#currentPlanValue").val();
  $cols.filter(function( index ) {
    return $( this ).find( "#PlanValue" ).val() === currentPlan;
  }).remove();
};

if ($("#isCanceled").val() === "True") {
    $cols
        .filter(function (index) {
            return $(this).find("#PlanValue").val() === "0";
        })
        .remove();
}

