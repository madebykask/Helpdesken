(function ($) {

    $.validator.addMethod("requiredifnotempty",
        function (value, element, params) {
            var id = "#" + params["dependentproperty"];

            var control = $(id);
            var actualvalue = control.val();

            if ($.trim(actualvalue) !== "")
                return $.validator.methods.required.call(this, value, element, params);

            return true;
        });

    $.validator.unobtrusive.adapters.add("requiredifnotempty", ["dependentproperty"],
        function (options) {
            options.rules["requiredifnotempty"] = {
                dependentproperty: options.params["dependentproperty"]
            };
            options.messages["requiredifnotempty"] = options.message;
        });

})(jQuery)
