(function ($) {
    $.validator.addMethod('maxsizefrom', function (value, element, params) {
        return params.isUseMaxLength == 'True' && value.length > params.maxLength ? false : true;
    });

    $.validator.unobtrusive.adapters.add('maxsizefrom', [], function (options) {
        var isUseMaxLength = $(options.element).attr('data-dh-is-use-maxlength');
        var maxLength = $(options.element).attr('data-dh-maxlength');

        var params = {
            maxLength: maxLength,
            isUseMaxLength: isUseMaxLength
        };

        options.rules['maxsizefrom'] = params;
        options.messages['maxsizefrom'] = options.message;
    });
}(jQuery));