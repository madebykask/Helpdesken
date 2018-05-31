function PasswordValidator(opt) {
        
    this.ErrorCodes = {
        InvalidLength: 1,
        InvalidChars: 2,
        NoUpperCase: 3,
        NoLowerCase: 4,
        NoDigits: 5,
        NoSpecialChars: 6
    };

    var defaults = 
    {
        MinLength: 8,
        MinUpperCase: 1,
        MinLowerCase: 1,
        MinDigits: 1,
        MinSpecial: 1,
        SpecialChars: '!@#=$&?*' 
    };

    this.options = $.extend({}, defaults, opt || {});
}

PasswordValidator.prototype.createErrorResult = function(errorCode) {
    return {
        isValid: false,
        ErrorCode: errorCode
    };
};

PasswordValidator.prototype.Validate = function (pwd) {

    pwd = pwd || '';

    if (this.options.MinLength && pwd.length < this.options.MinLength) {
        return this.createErrorResult(this.ErrorCodes.InvalidLength);
    }

    var validChars = '';

    //upper case
    var res = false;
    if (this.options.MinUpperCase) {
        res = pwd.match(/[A-Z]/g);
        //console.log(res);
        if (!res || res.length < this.options.MinUpperCase) {
            return this.createErrorResult(this.ErrorCodes.NoUpperCase);
        }
          
        validChars += 'A-Z';
    }
        
    //lower case
    if (this.options.MinLowerCase) {
        res = pwd.match(/[a-z]/g);
        if (!res || res.length < this.options.MinLowerCase) {
            return this.createErrorResult(this.ErrorCodes.NoLowerCase);
        }
        validChars += 'a-z';
    }

    //digits
    if (this.options.MinDigits) {
        res = pwd.match(/[0-9]/g);
        if (!res || res.length < this.options.MinDigits) {
            return this.createErrorResult(this.ErrorCodes.NoDigits);
        }
        validChars += '0-9';            
    }
        
    //special
    if (this.options.MinSpecial && this.options.SpecialChars.length) {
        var regex = new RegExp('[' + this.options.SpecialChars + ']', "g");
        res = pwd.match(regex);
        if (!res || res.length < this.options.MinSpecial) {
            return this.createErrorResult(this.ErrorCodes.NoSpecialChars);
        }
        validChars += this.options.SpecialChars;            
    }

    //invalid chars
    if (validChars.length) {
        //console.log(validChars);
        var regexInvalid = new RegExp('^[^' + validChars + ']+$', "g"); //note negative condition
        if (regexInvalid.test(pwd)) {
            return this.createErrorResult(this.ErrorCodes.InvalidChars);
        }
    }

    return { isValid: true }
}