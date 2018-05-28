window.ChangePasswordDialog =
    (function ($) {
        var _self = this;

        

        this._init = function (settings) {
            var self = this;

            self.settings = settings;

            this.newPasswordField$ = $('#NewPassword');
            this.confirmNewPasswordField$ = $('#ConfirmPassword');
            this.dialogDiv$ = $('#divPasswordDialog');
            this.form$ = $('#changePasswordForm');
            
            //handle submit
            self.form$.on('submit', function (e) {
                e.preventDefault();
                var isValid = self.form$.valid();
                if (isValid) {
                    self.changePassword();
                } 
            });

            //set up validation
            self.passwordValidator = new PasswordValidator(settings.passwordRules);
            self.validator$ = self._setupValidation();

            //reset form on show
            self.dialogDiv$.on('show',
                function() {
                    self.newPasswordField$.val('');
                    self.confirmNewPasswordField$.val('');

                    //clear prev validation errors
                    self.validator$.resetForm();
                    self.form$.find('input[type=password]').removeClass('input-validation-error');
            });
        };

        this.changePassword = function () {

            var self = this;
            $.post(self.settings.PasswordChangeActionUrl,
                {
                    id: $('#id_user').val(),
                    newPassword: self.newPasswordField$.val(),
                    confirmPassword: self.confirmNewPasswordField$.val()
                },
                function (data) {
                    ShowToastMessage(self.settings.translations.PasswordChangedConfirmMessage, "alert");
                    self.dialogDiv$.modal('hide');
                });
        };

        this._setupValidation = function () {
            var self = this;

            //$.validator.messages.required = function (param, input) {
            //    var label = $('label[for=' + input.id + ']');
            //    var template = $.validator.format(self.settings.translations.FieldsNamesRequired);
            //    return template(label.text());
            //}

            $.validator.addMethod("pwdcheck", function (value, element) {
                var res = self.passwordValidator.Validate(value);
                return res.isValid;
            });

            //remove default validator
            self.form$.data('validator', null);
            
            var validator = self.form$.validate({
                errorLabelContainer: "#validation-summary",
                rules: {
                    NewPassword: {
                        required: true,
                        minlength: 8,
                        pwdcheck: true
                    },

                    ConfirmPassword: {
                        required: true,
                        equalTo: '#NewPassword'
                    }
                },

                messages: {
                    NewPassword: {
                        required: self.settings.translations.PasswordRequiredValMessage,
                        minlength: $.validator.format(self.settings.translations.PasswordMinLengthValMessage),
                        pwdcheck: self.settings.translations.StrongPasswordValMessage
                    },

                    ConfirmPassword: {
                        required: self.settings.translations.ConfirmPasswordRequiredValMessage,
                        equalTo: self.settings.translations.ConfirmPasswordMatchValMessage
                    }
                },

                highlight: function (element, errorClass, validClass) {
                    $(element).addClass(errorClass).removeClass(validClass);
                },

                unhighlight: function(element, errorClass, validClass) {
                    $(element).removeClass(errorClass).addClass(validClass);
                }

                // the errorPlacement has to take the table layout into account
                //errorPlacement: function (error, element) {
                //    error.appendTo($('#validation-summary'));
                //}

                //submitHandler: function () {
                //    var isValid = self.form$.valid();
                //    if (isValid) {
                //        self.changePassword();
                //    }
                //    return false;
                //}
            });

            return validator;
        };

        return {
            Init: function (settings) {
                _self._init(settings);
            },

            Show: function () {
                _self.dialogDiv$.modal('show');
            }
        };
    })(jQuery);
