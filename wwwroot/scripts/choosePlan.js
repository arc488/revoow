var $accountTypeInput = $('#Input_AccountType');

var addclass = 'shadow';
var $cols = $('.card-pricing').click(function (e) {
    var plan = $(this).find('#PlanValue').val();
    $cols.removeClass(addclass);
    $(this).addClass(addclass);
    $('button').removeClass("btn-primary").addClass('btn-outline-secondary');
    $(this).find('button').addClass("btn-primary").removeClass('btn-outline-secondary');
    $accountTypeInput.val(plan).change();

});

if ($("#Input_CurrentType").length){
    var currentPlan = $("#Input_CurrentType").val();
  $cols.filter(function( index ) {
    return $( this ).find( "#PlanValue" ).val() == currentPlan;
  }).remove();
};
