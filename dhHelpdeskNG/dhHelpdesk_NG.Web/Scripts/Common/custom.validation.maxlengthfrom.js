(function($) {
    $.validator.addMethod('maxlengthfrom', function(value, element, params) {
        return value.length > params.maxlength ? false : true;
    });

    $.validator.unobtrusive.adapters.add('maxlengthfrom', ['maxlength'], function(options) {
        var params = {
            maxlength: options.params.maxlength
        };

        options.rules['maxlengthfrom'] = params;
        options.messages['maxlengthfrom'] = options.message;
    });
}(jQuery));