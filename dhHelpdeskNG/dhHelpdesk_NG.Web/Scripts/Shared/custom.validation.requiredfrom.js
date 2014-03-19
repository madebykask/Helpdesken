(function($) {
    $.validator.addMethod('requiredfrom', function(value, element, params) {
        return params.isRequired == 'True' && !value ? false : true;
    });

    $.validator.unobtrusive.adapters.add('requiredfrom', ['isrequired'], function(options) {
        var params = {
            isRequired: options.params.isrequired
        };

        options.rules['requiredfrom'] = params;
        options.messages['requiredfrom'] = options.message;
    });
}(jQuery));