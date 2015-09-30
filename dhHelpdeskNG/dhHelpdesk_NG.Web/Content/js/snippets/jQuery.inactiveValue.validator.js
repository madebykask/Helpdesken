/**
* This is a plugin for jQuery validator.
* This plugin does not allows to choose OPTION with "inactive" CSS class in SELECT element.
* WARN: tested only with single value (!multiple)
*/
(function($) {
    $.validator.addMethod('only_active', function (value, element, params) {
        var res = !$(element).find('option:selected').hasClass('inactive');
        return res;
    });
})(jQuery);