(function($) {
    $.validator.addMethod('requiredfrom', function(value, element, params) {
        return params.isRequired == 'True' && !value ? false : true;
    });

    $.validator.unobtrusive.adapters.add('requiredfrom', [], function(options) {
        var isRequired = $(options.element).attr('data-dh-is-required');

        var params = {
            isRequired: isRequired
        };

        options.rules['requiredfrom'] = params;
        options.messages['requiredfrom'] = options.message;
    });
}(jQuery));