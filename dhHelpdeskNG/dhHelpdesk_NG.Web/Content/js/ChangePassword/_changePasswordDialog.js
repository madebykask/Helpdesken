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
                return false;
            });

            //set up validation
            self.passwordValidator = new PasswordValidator(settings.ComplexPasswordRules);
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
                    id: self.settings.UserId,
                    newPassword: self.newPasswordField$.val(),
                    confirmPassword: self.confirmNewPasswordField$.val()
                },
                function (data) {
                    if (data.isSuccess) {
                        self.dialogDiv$.modal('hide');
                        self.ShowToastMessage(self.settings.Translations.PasswordChangedConfirmMessage, "alert");

                        if (self.successCallback) {
                            setTimeout(function() {
                                var pwd = self.newPasswordField$.val();
                                self.successCallback(pwd);
                            }, 500);
                        }
                    } else {
                        $('#validation-summary').html(self.settings.Translations.PasswordChangeErrorMessage);
                    }
                });
        };

        this._setupValidation = function () {
            var self = this;
            
            //check password strong policy
            $.validator.addMethod("pwdcheck", function (value, element) {
                var res = self.passwordValidator.Validate(value);
                return res.isValid;
            });

            //check if new password is different from the existing
            $.validator.addMethod("checkUnique", function (value, element) {

                var isUnique = false;

                var data = {
                    userId: self.settings.UserId,
                    pwd: value
                };
                
                //run sync to get validation result 
                $.ajax({
                    url: self.settings.CheckPasswordUniqueActionUrl,
                    type: 'POST',
                    data: $.param(data),
                    dataType: "json",
                    async: false
                }).done(function(res) {
                    //console.log('>> ajax.isUnique: ' + (res.isUnique ? 'true' : 'false'));
                    isUnique = res.isUnique;
                });
                
                //console.log('>> func.result: ' + (isUnique ? 'true' : 'false'));
                return isUnique;
            });

            //remove default validator - issue with default unobtrusive valdiator. 
            self.form$.data('validator', null);
            
            var validator = self.form$.validate({
                errorLabelContainer: "#validation-summary",
                rules: {
                    NewPassword: {
                        required: true,
                        minlength: self.settings.PasswordMinLength,
                        pwdcheck: self.settings.UseComplexPassword,
                        checkUnique: true
                    },

                    ConfirmPassword: {
                        required: true,
                        equalTo: '#NewPassword'
                    }
                },

                messages: {
                    NewPassword: {
                        required: self.settings.Translations.PasswordRequiredValMessage,
                        minlength: $.validator.format(self.settings.Translations.PasswordMinLengthValMessage),
                        pwdcheck: self.settings.Translations.StrongPasswordValMessage,
                        checkUnique: self.settings.Translations.PasswordEqualsExistingValMessage
                    },

                    ConfirmPassword: {
                        required: self.settings.Translations.ConfirmPasswordRequiredValMessage,
                        equalTo: self.settings.Translations.ConfirmPasswordMatchValMessage
                    }
                },

                highlight: function (element, errorClass, validClass) {
                    $(element).addClass(errorClass).removeClass(validClass);
                },

                unhighlight: function(element, errorClass, validClass) {
                    $(element).removeClass(errorClass).addClass(validClass);
                }
            });

            return validator;
        };


        this.ShowToastMessage = function(message, msgType) {
            $().toastmessage('showToast', {
                text: message,
                sticky: false,
                position: 'top-center',
                type: msgType,
                closeText: '',
                stayTime: 3000,
                inEffectDuration: 1000,
                close: function () {
                    //console.log("toast is closed ...");
                }
            });
        }

        return {
            Init: function (settings) {
                _self._init(settings);
            },

            SetSuccessCallback : function(callback) {
                _self.successCallback = callback;
            },

            Show: function () {
                _self.dialogDiv$.modal('show');
            }
        };
    })(jQuery);
