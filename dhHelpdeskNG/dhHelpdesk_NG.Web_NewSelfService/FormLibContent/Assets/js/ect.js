//Selection

// Globals
var isEmbed = window != window.parent;
var site = site || {};
site.baseUrl = "";
var disableTypeahead = 1;
// stolen from boostrap-datepicker.js DL...
// Extend for general functions
var APIGlobal = {
    DateTime: {
        parseFormat: function (format) {
            var separator = format.match(/[.\/\-\s].*?/),
                parts = format.split(/\W+/);
            if (!separator || !parts || parts.length === 0) {
                throw new Error("Invalid date format.");
            }
            return { separator: separator, parts: parts };
        },
        parseDate: function (date, format) {
            var parts = date.split(format.separator),
                date = new Date(),
                val;
            date.setHours(0);
            date.setMinutes(0);
            date.setSeconds(0);
            date.setMilliseconds(0);
            if (parts.length === format.parts.length) {
                var year = date.getFullYear(), day = date.getDate(), month = date.getMonth();
                for (var i = 0, cnt = format.parts.length; i < cnt; i++) {
                    val = parseInt(parts[i], 10) || 1;
                    switch (format.parts[i]) {
                        case 'dd':
                        case 'd':
                            day = val;
                            date.setDate(val);
                            break;
                        case 'mm':
                        case 'm':
                            month = val - 1;
                            date.setMonth(val - 1);
                            break;
                        case 'yy':
                            year = 2000 + val;
                            date.setFullYear(2000 + val);
                            break;
                        case 'yyyy':
                            year = val;
                            date.setFullYear(val);
                            break;
                    }
                }
                date = new Date(year, month, day, 0, 0, 0);
            }
            return date;
        },
        formatDate: function (date, format) {
            var val = {
                d: date.getDate(),
                m: date.getMonth() + 1,
                yy: date.getFullYear().toString().substring(2),
                yyyy: date.getFullYear()
            };
            val.dd = (val.d < 10 ? '0' : '') + val.d;
            val.mm = (val.m < 10 ? '0' : '') + val.m;
            var date = [];
            for (var i = 0, cnt = format.parts.length; i < cnt; i++) {
                date.push(val[format.parts[i]]);
            }
            return date.join(format.separator);
        },
        dateInRange: function (start, end, months) {
            start.setMonth(start.getMonth() + months);
            var b = start <= end;
            return b;
        }
    }
};

// Functions jQuery

jQuery.fn.selectText = function () {
    var doc = document, element = this[0], range, selection;

    if (doc.body.createTextRange) {
        range = document.body.createTextRange();
        range.moveToElementText(element);
        range.select();
    } else if (window.getSelection) {
        selection = window.getSelection();
        range = document.createRange();
        range.selectNodeContents(element);
        selection.removeAllRanges();
        selection.addRange(range);
    }
};

// DH+ specific

var reload = function (cancelCase) {
    if (window.parent.cancelCase != undefined && cancelCase)
        window.parent.cancelCase(3, 0);
    else if (!isEmbed) {
        if (cancelCase) {
            window.open('', '_self', '');
            window.opener = self; window.close();
        }
    }
    var action = $('form').attr('action');
    location.href = action;

    //location.href = location.href;
};

var multi = function () {
    var multi = $('.multi');
    if (multi.length > 0) {
        multi.change(function () {
            var t = $(this);
            var id = t.attr('id');
            var notice = $('#notice_' + id);

            if (notice.length > 0) {
                if (t.is(':checked'))
                    notice.show();
                else
                    notice.hide();
            }
        });

        multi.change();
    }
};

var validate = {
    run: function (iState) {
        $(".asterisk").hide();
        if (iState !== undefined && iState != '' && $('#validate_' + iState).length > 0) {
            var json = $.parseJSON($('#validate_' + iState).val());

            for (var i = 0; i < json.length; i++) {
                if (json[i].Rules.required == "True")
                    $(".asterisk_" + json[i].Name).show();
            }
        }
    }
};

var globalTypeAheadOptions = {
    items: 10,
    minLength: 3,
    source: function (query, process) {
        if (disableTypeahead == 0) {
            disableTypeahead == 1;
            return;
        }
        return $.ajax({
            url: site.baseUrl + '/search/globalview',
            type: 'post',
            data: { query: query, customerId: $('#CustomerId').val() },
            dataType: 'json',
            success: function (result) {
                var resultList = jQuery.map(result, function (item) {
                    var aItem = {
                        id: item.Id
                        , num: item.EmployeeNumber
                        , name: item.Name + ' ' + item.Surname
                        , firstname: item.Name
                        , lastname: item.Surname
                        , company: item.Company
                        , companyId: item.CompanyId
                        , unit: item.Unit
                        , unitId: item.UnitId
                        , department: item.Department
                        , _function: item.Function
                        , caseNumber: item.CaseNumber
                        , regTime: item.RegTime
                        , email: item.Email
                    };
                    return JSON.stringify(aItem);
                });

                return process(resultList);
            }
        });
    },

    matcher: function (obj) {
        var item = JSON.parse(obj);
        return ~item.num.toLowerCase().indexOf(this.query.toLowerCase())
            || ~item.name.toLowerCase().indexOf(this.query.toLowerCase());
    },

    sorter: function (items) {
        var beginswith = [], caseSensitive = [], caseInsensitive = [], item;
        while (aItem = items.shift()) {
            var item = JSON.parse(aItem);
            if (!item.num.toLowerCase().indexOf(this.query.toLowerCase())) beginswith.push(JSON.stringify(item));
            else if (~item.num.indexOf(this.query)) caseSensitive.push(JSON.stringify(item));
            else caseInsensitive.push(JSON.stringify(item));
        }

        return beginswith.concat(caseSensitive, caseInsensitive);
    },

    highlighter: function (obj) {
        var item = JSON.parse(obj);
        var query = this.query.replace(/[\-\[\]{}()*+?.,\\\^$|#\s]/g, '\\$&');

        var result = item.num + ' - ' + item.name;
        return result.replace(new RegExp('(' + query + ')', 'ig'), function ($1, match) {
            return '<strong>' + match + '</strong>';
        });
    },

    updater: function (obj) {
        var item = JSON.parse(obj);

        var notice = $('.typeahead-notice').hide();

        if (item.caseNumber != undefined && item.caseNumber != '') { 
            notice.text(notice.text().replace('#', item.caseNumber));
            notice.show();
        }

        if ($('#formGuid').length > 0) {

            $.ajax({
                url: site.baseUrl + '/search/EmployeesExtendedInfo',
                type: 'post',
                data: { formGuid: $('#formGuid').val(), employeenumber: item.num },
                dataType: 'json',
                success: function (result) {

                    var resultList = jQuery.map(result, function (extendeditem) {
                        var type = $('#' + extendeditem.FormFieldName).attr('type');
                        if (type != 'text') {

                            $('#' + extendeditem.FormFieldName).val(extendeditem.FormFieldValue);

                            $('#' + extendeditem.FormFieldName + ' option').filter(function () {
                                return $(this).text() == extendeditem.FormFieldValue;
                            }).prop('selected', true);

                            if ($('#' + extendeditem.FormFieldName)[0] != undefined && $('#' + extendeditem.FormFieldName)[0].selectize) {
                                $('#' + extendeditem.FormFieldName)[0].selectize.setValue(extendeditem.FormFieldValue);
                            }

                            if (extendeditem.FormFieldName == 'NewCompany')
                                changeNewCompany(true);                 

                            if (extendeditem.FormFieldName == 'EmploymentCategory' && extendeditem.FormFieldValue == 'Permanent') {
                                $('#date_ContractEndDate').datepicker("destroy");
                                $('#date_ContractEndDate').addClass("disabled");
                                $('#ContractEndDate').prop('disabled', true);
                            }

                        } else {
                            $('input[name="' + extendeditem.FormFieldName + '"]').val(extendeditem.FormFieldValue);
                        }

                        $('#emOLD_' + extendeditem.FormFieldName).text(extendeditem.FormFieldValue);
                        $('#OLD_' + extendeditem.FormFieldName).val(extendeditem.FormFieldValue);
                    });
                }
            });
        }

        $('input[name="Company"]').val(item.company);
        $('#emOLD_Company').text(item.company);
        $('#OLD_Company').val(item.company);

        $('input[name="BusinessUnit"]').val(item.unit);
        $('#emOLD_BusinessUnit').text(item.unit);
        $('#OLD_BusinessUnit').val(item.unit);

        $('input[name="ServiceArea"]').val(item._function);
        $('#emOLD_ServiceArea').text(item._function);
        $('#OLD_ServiceArea').val(item._function);

        $('input[name="Department"]').val(item.department);
        $('#emOLD_Department').text(item.department);
        $('#OLD_Department').val(item.department);

        $('input[name="FirstName"]').val(item.firstname);
        $('#emOLD_FirstName').text(item.firstname);
        $('#OLD_FirstName').val(item.firstname);

        $('input[name="LastName"]').val(item.lastname);
        $('#emOLD_LastName').text(item.lastname);
        $('#OLD_LastName').val(item.lastname);

        $('input[name="IKEAEmailAddress"]').val(item.email);
        $('#emOLD_IKEAEmailAddress').text(item.email);
        $('#OLD_IKEAEmailAddress').val(item.email);

        $('input[name="IKEANetworkID"]').val(item.num);
        $('#emOLD_IKEANetworkID').text(item.num);
        $('#OLD_IKEANetworkID').val(item.num);

        return item.num;
    }
};


var globalEmailTypeAheadOptions = {
    items: 10,
    minLength: 3,
    source: function (query, process) {
        return $.ajax({
            url: site.baseUrl + '/search/globalview',
            type: 'post',
            data: { query: query, customerId: $('#CustomerId').val() },
            dataType: 'json',
            success: function (result) {
                var resultList = jQuery.map(result, function (item) {
                    var aItem = {
                        id: item.Id
                        , num: item.EmployeeNumber
                        , name: item.Name + ' ' + item.Surname
                        , firstname: item.Name
                        , lastname: item.Surname
                        , company: item.Company
                        , companyId: item.CompanyId0
                        , unit: item.Unit
                        , unitId: item.UnitId
                        , department: item.Department
                        , _function: item.Function
                        , caseNumber: item.CaseNumber
                        , regTime: item.RegTime
                        , email: item.Email
                    };
                    return JSON.stringify(aItem);
                });

                return process(resultList);
            }
        });
    },

    matcher: function (obj) {
        var item = JSON.parse(obj);
        return ~item.num.toLowerCase().indexOf(this.query.toLowerCase())
            || ~item.name.toLowerCase().indexOf(this.query.toLowerCase());
    },

    sorter: function (items) {
        var beginswith = [], caseSensitive = [], caseInsensitive = [], item;
        while (aItem = items.shift()) {
            var item = JSON.parse(aItem);
            if (!item.num.toLowerCase().indexOf(this.query.toLowerCase())) beginswith.push(JSON.stringify(item));
            else if (~item.num.indexOf(this.query)) caseSensitive.push(JSON.stringify(item));
            else caseInsensitive.push(JSON.stringify(item));
        }

        return beginswith.concat(caseSensitive, caseInsensitive);
    },

    highlighter: function (obj) {
        var item = JSON.parse(obj);
        var query = this.query.replace(/[\-\[\]{}()*+?.,\\\^$|#\s]/g, '\\$&');

        var result = item.email;
        return result.replace(new RegExp('(' + query + ')', 'ig'), function ($1, match) {
            return '<strong>' + match + '</strong>';
        });
    },

    updater: function (obj) {
        var item = JSON.parse(obj);

        var notice = $('.typeahead-notice').hide();

        if (item.caseNumber != undefined && item.caseNumber != '') {
            notice.text(notice.text().replace('#', item.caseNumber));
            notice.show();
        }

        if ($('#formGuid').length > 0) {
            $.ajax({
                url: site.baseUrl + '/search/EmployeesExtendedInfo',
                type: 'post',
                data: { formGuid: $('#formGuid').val(), employeenumber: item.num },
                dataType: 'json',
                success: function (result) {
                    var resultList = jQuery.map(result, function (extendeditem) {
                        var type = $('#' + extendeditem.FormFieldName).attr('type');
                        if (type != 'text') {
                            $('#' + extendeditem.FormFieldName).val(extendeditem.FormFieldValue);

                            $('#' + extendeditem.FormFieldName + ' option').filter(function () {
                                return $(this).text() == extendeditem.FormFieldValue;
                            }).prop('selected', true);

                            if (extendeditem.FormFieldName == 'NewCompany')
                                changeNewCompany(true);

                            if (extendeditem.FormFieldName == 'EmploymentCategory' && extendeditem.FormFieldValue == 'Permanent') {
                                $('#date_ContractEndDate').datepicker("destroy");
                                $('#date_ContractEndDate').addClass("disabled");
                                $('#ContractEndDate').prop('disabled', true);
                            }

                        } else {
                            $('input[name="' + extendeditem.FormFieldName + '"]').val(extendeditem.FormFieldValue);
                        }                       
                        $('#emOLD_' + extendeditem.FormFieldName).text(extendeditem.FormFieldValue);
                        $('#OLD_' + extendeditem.FormFieldName).val(extendeditem.FormFieldValue);
                    });
                }
            });
        }

        $('input[name="Co-WorkerGlobalviewID"]').val(item.num);

        $('input[name="Company"]').val(item.company);
        $('#emOLD_Company').text(item.company);
        $('#OLD_Company').val(item.company);

        $('input[name="BusinessUnit"]').val(item.unit);
        $('#emOLD_BusinessUnit').text(item.unit);
        $('#OLD_BusinessUnit').val(item.unit);

        $('input[name="ServiceArea"]').val(item._function);
        $('#emOLD_ServiceArea').text(item._function);
        $('#OLD_ServiceArea').val(item._function);

        $('input[name="Department"]').val(item.department);
        $('#emOLD_Department').text(item.department);
        $('#OLD_Department').val(item.department);

        $('input[name="FirstName"]').val(item.firstname);
        $('#emOLD_FirstName').text(item.firstname);
        $('#OLD_FirstName').val(item.firstname);

        $('input[name="LastName"]').val(item.lastname);
        $('#emOLD_LastName').text(item.lastname);
        $('#OLD_LastName').val(item.lastname);

        $('input[name="IKEAEmailAddress"]').val(item.email);
        $('#emOLD_IKEAEmailAddress').text(item.email);
        $('#OLD_IKEAEmailAddress').val(item.email);

        $('input[name="IKEANetworkID"]').val(item.num);
        $('#emOLD_IKEANetworkID').text(item.num);
        $('#OLD_IKEANetworkID').val(item.num);

        return item.email;
    }
};


var globalNameTypeAheadOptions = {
    items: 10,
    minLength: 3,
    source: function (query, process) {
        if (disableTypeahead == 0) {
            disableTypeahead == 1;
            return;
        }
        return $.ajax({
            url: site.baseUrl + '/search/globalview',
            type: 'post',
            data: { query: query, customerId: $('#CustomerId').val() },
            dataType: 'json',
            success: function (result) {
                var resultList = jQuery.map(result, function (item) {
                    var aItem = {
                        id: item.Id
                        , num: item.EmployeeNumber
                        , name: item.Name + ' ' + item.Surname
                        , firstname: item.Name
                        , lastname: item.Surname
                        , company: item.Company
                        , companyId: item.CompanyId
                        , unit: item.Unit
                        , unitId: item.UnitId
                        , department: item.Department
                        , _function: item.Function
                        , caseNumber: item.CaseNumber
                        , regTime: item.RegTime
                        , email: item.Email
                    };
                    return JSON.stringify(aItem);
                });

                return process(resultList);
            }
        });
    },

    matcher: function (obj) {
        var item = JSON.parse(obj);
        return ~item.num.toLowerCase().indexOf(this.query.toLowerCase())
            || ~item.name.toLowerCase().indexOf(this.query.toLowerCase());
    },

    sorter: function (items) {
        var beginswith = [], caseSensitive = [], caseInsensitive = [], item;
        while (aItem = items.shift()) {
            var item = JSON.parse(aItem);
            if (!item.num.toLowerCase().indexOf(this.query.toLowerCase())) beginswith.push(JSON.stringify(item));
            else if (~item.num.indexOf(this.query)) caseSensitive.push(JSON.stringify(item));
            else caseInsensitive.push(JSON.stringify(item));
        }

        return beginswith.concat(caseSensitive, caseInsensitive);
    },

    highlighter: function (obj) {
        var item = JSON.parse(obj);
        var query = this.query.replace(/[\-\[\]{}()*+?.,\\\^$|#\s]/g, '\\$&');

        var result = item.name;
        return result.replace(new RegExp('(' + query + ')', 'ig'), function ($1, match) {
            return '<strong>' + match + '</strong>';
        });
    },

    updater: function (obj) {
        var item = JSON.parse(obj);

        var notice = $('.typeahead-notice').hide();

        if (item.caseNumber != undefined && item.caseNumber != '') {
            notice.text(notice.text().replace('#', item.caseNumber));
            notice.show();
        }

        if ($('#formGuid').length > 0) {
            $.ajax({
                url: site.baseUrl + '/search/EmployeesExtendedInfo',
                type: 'post',
                data: { formGuid: $('#formGuid').val(), employeenumber: item.num },
                dataType: 'json',
                success: function (result) {
                    var resultList = jQuery.map(result, function (extendeditem) {
                        var type = $('#' + extendeditem.FormFieldName).attr('type');
                        if (type != 'text') {
                            $('#' + extendeditem.FormFieldName).val(extendeditem.FormFieldValue);

                            $('#' + extendeditem.FormFieldName + ' option').filter(function () {
                                return $(this).text() == extendeditem.FormFieldValue;
                            }).prop('selected', true);

                            if (extendeditem.FormFieldName == 'NewCompany')
                                changeNewCompany(true);

                            if (extendeditem.FormFieldName == 'EmploymentCategory' && extendeditem.FormFieldValue == 'Permanent') {
                                $('#date_ContractEndDate').datepicker("destroy");
                                $('#date_ContractEndDate').addClass("disabled");
                                $('#ContractEndDate').prop('disabled', true);
                            }

                        } else {
                            $('input[name="' + extendeditem.FormFieldName + '"]').val(extendeditem.FormFieldValue);
                        }                        
                        $('#emOLD_' + extendeditem.FormFieldName).text(extendeditem.FormFieldValue);
                        $('#OLD_' + extendeditem.FormFieldName).val(extendeditem.FormFieldValue);
                    });
                }
            });
        }

        $('input[name="Co-WorkerGlobalviewID"]').val(item.num);
        $('input[name="Co-WorkerID"]').val(item.num);

        $('input[name="Company"]').val(item.company);
        $('#emOLD_Company').text(item.company);
        $('#OLD_Company').val(item.company);

        $('input[name="BusinessUnit"]').val(item.unit);
        $('#emOLD_BusinessUnit').text(item.unit);
        $('#OLD_BusinessUnit').val(item.unit);

        $('input[name="ServiceArea"]').val(item._function);
        $('#emOLD_ServiceArea').text(item._function);
        $('#OLD_ServiceArea').val(item._function);

        $('input[name="Department"]').val(item.department);
        $('#emOLD_Department').text(item.department);
        $('#OLD_Department').val(item.department);

        $('input[name="FirstName"]').val(item.firstname);
        $('#emOLD_FirstName').text(item.firstname);
        $('#OLD_FirstName').val(item.firstname);

        $('input[name="LastName"]').val(item.lastname);
        $('#emOLD_LastName').text(item.lastname);
        $('#OLD_LastName').val(item.lastname);

        $('input[name="IKEAEmailAddress"]').val(item.email);
        $('#emOLD_IKEAEmailAddress').text(item.email);
        $('#OLD_IKEAEmailAddress').val(item.email);

        $('input[name="IKEANetworkID"]').val(item.num);
        $('#emOLD_IKEANetworkID').text(item.num);
        $('#OLD_IKEANetworkID').val(item.num);


        return item.firstname;
    }
};


var globalLastNameTypeAheadOptions = {
    items: 10,
    minLength: 3,
    source: function (query, process) {
        if (disableTypeahead == 0) {
            disableTypeahead == 1;
            return;
        }
        return $.ajax({
            url: site.baseUrl + '/search/globalview',
            type: 'post',
            data: { query: query, customerId: $('#CustomerId').val() },
            dataType: 'json',
            success: function (result) {
                var resultList = jQuery.map(result, function (item) {
                    var aItem = {
                        id: item.Id
                        , num: item.EmployeeNumber
                        , name: item.Name + ' ' + item.Surname
                        , firstname: item.Name
                        , lastname: item.Surname
                        , company: item.Company
                        , companyId: item.CompanyId
                        , unit: item.Unit
                        , unitId: item.UnitId
                        , department: item.Department
                        , _function: item.Function
                        , caseNumber: item.CaseNumber
                        , regTime: item.RegTime
                        , email: item.Email
                    };
                    return JSON.stringify(aItem);
                });

                return process(resultList);
            }
        });
    },

    matcher: function (obj) {
        var item = JSON.parse(obj);
        return ~item.num.toLowerCase().indexOf(this.query.toLowerCase())
            || ~item.name.toLowerCase().indexOf(this.query.toLowerCase());
    },

    sorter: function (items) {
        var beginswith = [], caseSensitive = [], caseInsensitive = [], item;
        while (aItem = items.shift()) {
            var item = JSON.parse(aItem);
            if (!item.num.toLowerCase().indexOf(this.query.toLowerCase())) beginswith.push(JSON.stringify(item));
            else if (~item.num.indexOf(this.query)) caseSensitive.push(JSON.stringify(item));
            else caseInsensitive.push(JSON.stringify(item));
        }

        return beginswith.concat(caseSensitive, caseInsensitive);
    },

    highlighter: function (obj) {
        var item = JSON.parse(obj);
        var query = this.query.replace(/[\-\[\]{}()*+?.,\\\^$|#\s]/g, '\\$&');

        var result = item.lastname + ' ' + item.firstname;
        return result.replace(new RegExp('(' + query + ')', 'ig'), function ($1, match) {
            return '<strong>' + match + '</strong>';
        });
    },

    updater: function (obj) {
        var item = JSON.parse(obj);

        var notice = $('.typeahead-notice').hide();

        if (item.caseNumber != undefined && item.caseNumber != '') {
            notice.text(notice.text().replace('#', item.caseNumber));
            notice.show();
        }

        if ($('#formGuid').length > 0) {
            $.ajax({
                url: site.baseUrl + '/search/EmployeesExtendedInfo',
                type: 'post',
                data: { formGuid: $('#formGuid').val(), employeenumber: item.num },
                dataType: 'json',
                success: function (result) {
                    var resultList = jQuery.map(result, function (extendeditem) {
                        var type = $('#' + extendeditem.FormFieldName).attr('type');
                        if (type != 'text') {
                            $('#' + extendeditem.FormFieldName).val(extendeditem.FormFieldValue);

                            $('#' + extendeditem.FormFieldName + ' option').filter(function () {
                                return $(this).text() == extendeditem.FormFieldValue;
                            }).prop('selected', true);

                            if (extendeditem.FormFieldName == 'NewCompany')
                                changeNewCompany(true);

                            if (extendeditem.FormFieldName == 'EmploymentCategory' && extendeditem.FormFieldValue == 'Permanent') {
                                $('#date_ContractEndDate').datepicker("destroy");
                                $('#date_ContractEndDate').addClass("disabled");
                                $('#ContractEndDate').prop('disabled', true);
                            }                       

                        } else {
                            $('input[name="' + extendeditem.FormFieldName + '"]').val(extendeditem.FormFieldValue);
                        }                     
                        $('#emOLD_' + extendeditem.FormFieldName).text(extendeditem.FormFieldValue);
                        $('#OLD_' + extendeditem.FormFieldName).val(extendeditem.FormFieldValue);
                    });
                }
            });
        }

        $('input[name="Co-WorkerGlobalviewID"]').val(item.num);
        $('input[name="Co-WorkerID"]').val(item.num);

        $('input[name="Company"]').val(item.company);
        $('#emOLD_Company').text(item.company);
        $('#OLD_Company').val(item.company);

        $('input[name="BusinessUnit"]').val(item.unit);
        $('#emOLD_BusinessUnit').text(item.unit);
        $('#OLD_BusinessUnit').val(item.unit);

        $('input[name="ServiceArea"]').val(item._function);
        $('#emOLD_ServiceArea').text(item._function);
        $('#OLD_ServiceArea').val(item._function);

        $('input[name="Department"]').val(item.department);
        $('#emOLD_Department').text(item.department);
        $('#OLD_Department').val(item.department);

        $('input[name="FirstName"]').val(item.firstname);
        $('#emOLD_FirstName').text(item.firstname);
        $('#OLD_FirstName').val(item.firstname);

        $('input[name="LastName"]').val(item.lastname);
        $('#emOLD_LastName').text(item.lastname);
        $('#OLD_LastName').val(item.lastname);

        $('input[name="IKEAEmailAddress"]').val(item.email);
        $('#emOLD_IKEAEmailAddress').text(item.email);
        $('#OLD_IKEAEmailAddress').val(item.email);

        $('input[name="IKEANetworkID"]').val(item.num);
        $('#emOLD_IKEANetworkID').text(item.num);
        $('#OLD_IKEANetworkID').val(item.num);


        return item.lastname;
    }
};

var typeAheadOptions = {
    items: 10,
    minLength: 3,
    source: function (query, process) {
        return $.ajax({
            url: site.baseUrl + '/search/globalview',
            type: 'post',
            data: { query: query, customerId: $('#CustomerId').val(), searchKey: $('#Unit').val() },
            dataType: 'json',
            success: function (result) {
                var resultList = jQuery.map(result, function (item) {
                    var aItem = {
                        id: item.Id
                        , num: item.EmployeeNumber
                        , name: item.Name + ' ' + item.Surname
                        , firstname: item.Name
                        , lastname: item.Surname
                        , company: item.Company
                        , companyId: item.CompanyId
                        , unit: item.Unit
                        , unitId: item.UnitId
                        , caseNumber: item.CaseNumber
                        , regTime: item.RegTime
                    };
                    return JSON.stringify(aItem);
                });

                return process(resultList);
            }
        });
    },

    matcher: function (obj) {
        var item = JSON.parse(obj);
        return ~item.num.toLowerCase().indexOf(this.query.toLowerCase())
            || ~item.name.toLowerCase().indexOf(this.query.toLowerCase());
    },

    sorter: function (items) {
        var beginswith = [], caseSensitive = [], caseInsensitive = [], item;
        while (aItem = items.shift()) {
            var item = JSON.parse(aItem);
            if (!item.num.toLowerCase().indexOf(this.query.toLowerCase())) beginswith.push(JSON.stringify(item));
            else if (~item.num.indexOf(this.query)) caseSensitive.push(JSON.stringify(item));
            else caseInsensitive.push(JSON.stringify(item));
        }

        return beginswith.concat(caseSensitive, caseInsensitive);
    },

    highlighter: function (obj) {
        var item = JSON.parse(obj);
        var query = this.query.replace(/[\-\[\]{}()*+?.,\\\^$|#\s]/g, '\\$&');

        var result = item.num + ' - ' + item.name;

        return result.replace(new RegExp('(' + query + ')', 'ig'), function ($1, match) {
            return '<strong>' + match + '</strong>';
        });
    },

    updater: function (obj) {
        var item = JSON.parse(obj);

        var notice = $('.typeahead-notice').hide();

        if (item.caseNumber != undefined && item.caseNumber != '') {
            notice.text(notice.text().replace('#', item.caseNumber));
            notice.show();
        }

        $('input[name="EmployeeFirstName"]').val(item.firstname);
        $('input[name="EmployeeLastName"]').val(item.lastname);

        return item.num;
    }
};

// Poland hiring Specific

var homeCostCenterQuery = function (query, $homeCostCenter) {

    $.ajax({
        url: site.baseUrl + '/poland/hiring/costcenter',
        type: 'post',
        data: { query: query, node: $homeCostCenter.attr('id') },
        dataType: 'json',
        success: function (result) {
            $homeCostCenter.val(result.Value);
        }
    });
};

var narrowDownInit = function () {

    var init = $('#narrowDownInit');
    if (init.length > 0) {

        //var homeCostCenter = $('#HomeCostCenter');
        //if (homeCostCenter.length > 0) {

        //    var depart = $('#Department');
        //    var source = depart.data('source');

        //    depart.typeahead({
        //        source: source,
        //        updater: function (item) {
        //            if (homeCostCenter.length > 0)
        //                homeCostCenterQuery(item, homeCostCenter);
        //            return item;
        //        }
        //    });

        //    if (depart.val() !== "" && homeCostCenter.val() === "")
        //        homeCostCenterQuery(depart.val(), homeCostCenter);
        //}

        var employeeUnit = $('#EmployeeUnit');
        var url = init.attr('url');

        $('#LineManager').typeahead({
            minLength: 1,
            source: function (query, process) {
                var node = this.$element.attr('data-node');
                var dependent = $('#Unit').val();
                if (employeeUnit.length > 0)
                    dependent = employeeUnit.val();
                return $.ajax({
                    url: url,
                    type: 'post',
                    data: { query: query, node: node, dependentAttribute: 'Unit', dependentAttributeValue: dependent },
                    dataType: 'json',
                    success: function (result) {

                        var resultList = jQuery.map(result, function (item) {
                            return item;
                        });

                        return process(resultList);
                    }
                });
            }
        });

        //$('#CrossChargeCostCentre').typeahead({
        //    minLength: 1,
        //    source: function (query, process) {

        //        var node = this.$element.attr('data-node');
        //        var dependent = $('#Department').val();

        //        return $.ajax({
        //            url: url,
        //            type: 'post',
        //            data: { query: query, node: node, dependentAttribute: 'Department', dependentAttributeValue: dependent },
        //            dataType: 'json',
        //            success: function (result) {

        //                var resultList = jQuery.map(result, function (item) {
        //                    return item;
        //                });

        //                return process(resultList);
        //            }
        //        });
        //    }
        //});

        //$('#TECApprover').typeahead({
        //    minLength: 1,
        //    source: function (query, process) {

        //        var node = this.$element.attr('data-node');
        //        var dependent = $('#ReportsToLineManager').val();

        //        return $.ajax({
        //            url: url,
        //            type: 'post',
        //            data: { query: query, node: node, dependentAttribute: 'ReportsToLineManager', dependentAttributeValue: dependent },
        //            dataType: 'json',
        //            success: function (result) {

        //                var resultList = jQuery.map(result, function (item) {
        //                    return item;
        //                });

        //                return process(resultList);
        //            }
        //        });
        //    }
        //});

        //$('#HomeCostCentre').typeahead({
        //    minLength: 1,
        //    source: function (query, process) {

        //        var node = this.$element.attr('data-node');
        //        var dependent;
        //        var hiredependent = $('#Department').val();
        //        var changedependent = $('#NewDepartment').val();

        //        var ci = $('#HomeCostCentre').attr("customid");
        //        if (ci != 'changeModule')
        //            dependent = hiredependent;
        //        else
        //            dependent = changedependent;

        //        return $.ajax({
        //            url: url,
        //            type: 'post',
        //            data: { query: query, node: node, dependentAttribute: 'Department', dependentAttributeValue: dependent },
        //            dataType: 'json',
        //            success: function (result) {

        //                var resultList = jQuery.map(result, function (item) {
        //                    return item;
        //                });

        //                return process(resultList);
        //            }
        //        });
        //    }
        //});

        $('#ReportsToLineManager').typeahead({
            minLength: 1,
            source: function (query, process) {

                var node = this.$element.attr('data-node');
                var dependent = $('#BusinessUnit').val();

                return $.ajax({
                    url: url,
                    type: 'post',
                    data: { query: query, node: node, dependentAttribute: 'BusinessUnit', dependentAttributeValue: dependent },
                    dataType: 'json',
                    success: function (result) {

                        var resultList = jQuery.map(result, function (item) {
                            return item;
                        });

                        return process(resultList);
                    }
                });
            }
        });

        var homeCostCenter = $('#HomeCostCenter');
        var depart = $('#Department');

        depart.typeahead({
            minLength: 1,
            source: function (query, process) {
                var node = this.$element.attr('data-node');
                var dependent = $('#Unit').val();
                if (employeeUnit.length > 0)
                    dependent = employeeUnit.val();
                return $.ajax({
                    url: url,
                    type: 'post',
                    data: { query: query, node: node, dependentAttribute: 'Unit', dependentAttributeValue: dependent },
                    dataType: 'json',
                    success: function (result) {

                        var resultList = jQuery.map(result, function (item) {
                            return item;
                        });

                        return process(resultList);
                    }
                });
            },
            updater: function (item) {
                if (homeCostCenter.length > 0)
                    homeCostCenterQuery(item, homeCostCenter);
                return item;
            }
        });

        if (homeCostCenter.length > 0) {
            if (depart.val() !== "" && homeCostCenter.val() === "")
                homeCostCenterQuery(depart.val(), homeCostCenter);
        }
    }


};

var familyMembers = function () {

    var emptyElements = function () {
        return $('[class^=familyMember]')
                                    .filter(function () { return $(this).css('display') !== 'none'; })
                                    .find(':text, :radio, :checkbox, select')
                                    .filter(function () {
                                        return $(this).val() == '';
                                    });
    };

    $('[class^=familyMember]').hide();
    $('.familyMember').show();
    var counter = parseInt($('#FamilyMembers').val());

    var max = 5;

    var elements = $('[class^=familyMember]').find(':text, :radio, :checkbox, select');

    var enabled = elements.eq(0).is(':enabled');

    for (var i = 1; i <= counter; i++)
        $('.familyMember' + i).show();

    if (emptyElements().length == 0 && (counter < max) && enabled)
        $('#addFamilyMemberTr').show();
    else
        $('#addFamilyMemberTr').hide();

    var elements = $('[class^=familyMember]').find(':text, :radio, :checkbox, select');

    elements.change(function () {
        if (emptyElements().length == 0 && (counter < max) && enabled)
            $('#addFamilyMemberTr').show();
        else
            $('#addFamilyMemberTr').hide();
    });

    $('#addFamilyMember').click(function (e) {
        e.preventDefault();
        counter++;
        $('#FamilyMembers').val(counter);
        if (counter <= max)
            $('[class=familyMember' + counter + ']').show();
        $('#addFamilyMemberTr').hide();
    });
};



//var terminationPayment = function () {

//    var emptyElements = function () {

//        return $('[class^=terminationPayment]')
//                                    .filter(function () { return $(this).css('display') !== 'none'; })
//                                    .find(':text, :radio, :checkbox, select')
//                                    .filter(function () {
//                                        return $(this).val() == '';
//                                    });
//    };

//    $('[class^=terminationPayment]').hide();
//    $('.terminationPayment').show();
//    var counter = parseInt($('#TerminationPayments').val());

//    var max = 3;

//    var elements = $('[class^=terminationPayment]').find(':text, :radio, :checkbox, select');

//    var enabled = elements.eq(0).is(':enabled');

//    for (var i = 1; i <= counter; i++)
//        $('.terminationPayment' + i).show();

//    if (emptyElements().length == 0 && (counter < max) && enabled)
//        $('#addTerminationPaymenttr').show();
//    else
//        //$('#addTerminationPaymenttr').hide();

//    var elements = $('[class^=terminationPayment]').find(':text, :radio, :checkbox, select');

//    elements.change(function () {
//        if (emptyElements().length == 0 && (counter < max) && enabled)
//            $('#addTerminationPaymenttr').show();
//        else
//            $('#addTerminationPaymenttr').hide();
//    });

//    $('#addTerminationPayment').click(function (e) {
//        e.preventDefault();
//        counter++;
//        $('#TerminationPayments').val(counter);
//        if (counter <= max)
//            $('[class=terminationPayment' + counter + ']').show();
//        //$('#addTerminationPaymenttr').hide();
//    });
//};

var employeeDocuments = function () {
    var emptyElements = function () {
        return $('[class^=employeeDocument]')
                                    .filter(function () { return $(this).css('display') !== 'none'; })
                                    .find(':text, :radio, :checkbox, select')
                                    .filter(function () {
                                        return $(this).val() == '';
                                    });
    };

    $('[class^=employeeDocument]').hide();
    $('.employeeDocument').show();
    var counter = parseInt($('#EmployeeDocuments').val());

    var max = 3;

    var elements = $('[class^=employeeDocument]').find(':text, :radio, :checkbox, select');

    var enabled = elements.eq(0).is(':enabled');

    for (var i = 1; i <= counter; i++)
        $('.employeeDocument' + i).show();

    if (emptyElements().length == 0 && (counter < max) && enabled)
        $('#addEmployeeDocumentTr').show();
    else
        $('#addEmployeeDocumentTr').hide();

    var elements = $('[class^=employeeDocument]').find(':text, :radio, :checkbox, select');

    elements.change(function () {
        if (emptyElements().length == 0 && (counter < max) && enabled)
            $('#addEmployeeDocumentTr').show();
        else
            $('#addEmployeeDocumentTr').hide();
    });

    $('#addEmployeeDocument').click(function (e) {
        e.preventDefault();
        counter++;
        $('#EmployeeDocuments').val(counter);
        if (counter <= max)
            $('[class*=employeeDocument' + counter + ']').show();
        $('#addEmployeeDocumentTr').hide();
    });
};

var dependantFamilyMembers = function () {

    var emptyElements = function () {
        return $('[class^=dependantFamilyMember]')
                                    .filter(function () { return $(this).css('display') !== 'none'; })
                                    .find(':text, :radio, :checkbox, select')
                                    .filter(function () {
                                        return $(this).val() == '';
                                    });
    };

    $('[class^=dependantFamilyMember]').hide();
    $('.dependantFamilyMember').show();
    var counter = parseInt($('#DependantFamilyMembers').val());

    var max = 3;

    var elements = $('[class^=dependantFamilyMember]').find(':text, :radio, :checkbox, select');

    var enabled = elements.eq(0).is(':enabled');

    for (var i = 1; i <= counter; i++)
        $('.dependantFamilyMember' + i).show();

    if (emptyElements().length == 0 && (counter < max) && enabled)
        $('#addDependantFamilyMembersTr').show();
    else
        $('#addDependantFamilyMembersTr').hide();

    var elements = $('[class^=dependantFamilyMember]').find(':text, :radio, :checkbox, select');

    elements.change(function () {
        if (emptyElements().length == 0 && (counter < max) && enabled)
            $('#addDependantFamilyMembersTr').show();
        else
            $('#addDependantFamilyMembersTr').hide();
    });

    $('#addDependantFamilyMember').click(function (e) {
        e.preventDefault();
        counter++;

        $('#DependantFamilyMembers').val(counter);
        if (counter <= max)
            $('[class*=dependantFamilyMember' + counter + ']').show();
        $('#addDependantFamilyMembersTr').hide();
    });
};

var dependantsDocuments = function () {

    var emptyElements = function () {
        return $('[class^=globalMobilityDependantDocument]')
                                    .filter(function () { return $(this).css('display') !== 'none'; })
                                    .find(':text, :radio, :checkbox, select')
                                    .filter(function () {
                                        return $(this).val() == '';
                                    });
    };

    $('[class^=globalMobilityDependantDocument]').hide();
    $('.globalMobilityDependantDocument').show();
    var counter = parseInt($('#DependantsDocuments').val());

    var max = 3;

    var elements = $('[class^=globalMobilityDependantDocument]').find(':text, :radio, :checkbox, select');

    var enabled = elements.eq(0).is(':enabled');

    for (var i = 1; i <= counter; i++)
        $('.globalMobilityDependantDocument' + i).show();

    if (emptyElements().length == 0 && (counter < max) && enabled)
        $('#addGlobalMobilityDependantDocumentTr').show();
    else
        $('#addGlobalMobilityDependantDocumentTr').hide();

    var elements = $('[class^=globalMobilityDependantDocument]').find(':text, :radio, :checkbox, select');

    elements.change(function () {
        if (emptyElements().length == 0 && (counter < max) && enabled)
            $('#addGlobalMobilityDependantDocumentTr').show();
        else
            $('#addGlobalMobilityDependantDocumentTr').hide();
    });

    $('#addGlobalMobilityDependantDocument').click(function (e) {
        e.preventDefault();
        counter++;

        $('#DependantsDocuments').val(counter);
        if (counter <= max)
            $('[class*=globalMobilityDependantDocument' + counter + ']').show();
        $('#addGlobalMobilityDependantDocumentTr').hide();
    });
};


var allowances = function () {

    var emptyElements = function () {
        return $('[class^=allowance]')
                                    .filter(function () { return $(this).css('display') !== 'none'; })
                                    .find(':text, :radio, :checkbox, select')
                                    .filter(function () {
                                        return $(this).val() == '';
                                    });
    };

    $('[class^=allowance]').hide();
    $('.allowance').show();
    var counter = parseInt($('#Allowances').val());

    var max = 3;

    var elements = $('[class^=allowance]').find(':text, :radio, :checkbox, select');

    var enabled = elements.eq(0).is(':enabled');

    for (var i = 1; i <= counter; i++)
        $('.allowance' + i).show();

    if (emptyElements().length == 0 && (counter < max) && enabled) {
        $('#addAllowanceTr').show();

    }
    else
        $('#addAllowanceTr').hide();

    var elements = $('[class^=allowance]').find(':text, :radio, :checkbox, select');

    elements.change(function () {
        if (emptyElements().length == 0 && (counter < max) && enabled)
            $('#addAllowanceTr').show();
        else
            $('#addAllowanceTr').hide();
    });

    $('#addAllowance').click(function (e) {
        e.preventDefault();
        counter++;

        $('#Allowances').val(counter);
        if (counter <= max)
            $('[class=allowance' + counter + ']').show();
        $('#addAllowanceTr').hide();
    });
};

var deductions = function () {

    var emptyElements = function () {
        return $('[class^=deduction]')
                                    .filter(function () { return $(this).css('display') !== 'none'; })
                                    .find(':text, :radio, :checkbox, select')
                                    .filter(function () {
                                        return $(this).val() == '';
                                    });
    };

    $('[class^=deduction]').hide();
    $('.deduction').show();
    var counter = parseInt($('#Deductions').val());

    var max = 3;

    var elements = $('[class^=deduction]').find(':text, :radio, :checkbox, select');

    var enabled = elements.eq(0).is(':enabled');

    for (var i = 1; i <= counter; i++)
        $('.deduction' + i).show();

    if (emptyElements().length == 0 && (counter < max) && enabled)
        $('#addDeductionTr').show();
    else
        $('#addDeductionTr').hide();

    var elements = $('[class^=deduction]').find(':text, :radio, :checkbox, select');

    elements.change(function () {
        if (emptyElements().length == 0 && (counter < max) && enabled)
            $('#addDeductionTr').show();
        else
            $('#addDeductionTr').hide();
    });

    $('#addDeduction').click(function (e) {
        e.preventDefault();
        counter++;

        $('#Deductions').val(counter);
        if (counter <= max)
            $('[class=deduction' + counter + ']').show();
        $('#addDeductionTr').hide();
    });
};

var education = function () {

    var emptyElements = function () {
        return $('[class^=education]')
                                    .filter(function () { return $(this).css('display') !== 'none'; })
                                    .find(':text, :radio, :checkbox, select')
                                    .filter(function () {
                                        return $(this).val() == '';
                                    });
    };

    $('[class^=education]').hide();
    $('.education').show();
    var counter = parseInt($('#Educations').val());

    var max = 3;

    var elements = $('[class^=education]').find(':text, :radio, :checkbox, select');

    var enabled = elements.eq(0).is(':enabled');

    for (var i = 1; i <= counter; i++)
        $('.education' + i).show();

    if (emptyElements().length == 0 && (counter < max) && enabled)
        $('#addEducationTr').show();
    else
        $('#addEducationTr').hide();

    var elements = $('[class^=education]').find(':text, :radio, :checkbox, select');

    elements.change(function () {
        if (emptyElements().length == 0 && (counter < max) && enabled)
            $('#addEducationTr').show();
        else
            $('#addEducationTr').hide();
    });

    $('#addEducation').click(function (e) {
        e.preventDefault();
        counter++;

        $('#Educations').val(counter);
        if (counter <= max)
            $('[class=education' + counter + ']').show();
        $('#addEducationTr').hide();
    });
};


var benefits = function () {

    var emptyElements = function () {
        return $('[class^=benefits]')
                                    .filter(function () { return $(this).css('display') !== 'none'; })
                                    .find(':text, :radio, :checkbox, select')
                                    .filter(function () {
                                        return $(this).val() == '';
                                    });
    };

    $('[class^=benefits]').hide();
    $('.benefits').show();
    var counter = parseInt($('#Benefits').val());

    var max = 3;

    var elements = $('[class^=benefits]').find(':text, :radio, :checkbox, select');

    var enabled = elements.eq(0).is(':enabled');

    for (var i = 1; i <= counter; i++)
        $('.benefits' + i).show();

    if (emptyElements().length == 0 && (counter < max) && enabled)
        $('#addBenefitsTr').show();
    else
        $('#addBenefitsTr').hide();

    var elements = $('[class^=benefits]').find(':text, :radio, :checkbox, select');

    elements.change(function () {
        if (emptyElements().length == 0 && (counter < max) && enabled)
            $('#addBenefitsTr').show();
        else
            $('#addBenefitsTr').hide();
    });

    $('#addBenefit').click(function (e) {
        e.preventDefault();
        counter++;

        $('#Benefits').val(counter);
        if (counter <= max)
            $('[class=benefits' + counter + ']').show();
        $('#addBenefitsTr').hide();
    });
};

var otherpreviousemployers = function () {

    var emptyElements = function () {
        return $('[class^=otherpreviousemployers]')
                                    .filter(function () { return $(this).css('display') !== 'none'; })
                                    .find(':text, :radio, :checkbox, select')
                                    .filter(function () {
                                        return $(this).val() == '';
                                    });
    };

    $('[class^=otherpreviousemployers]').hide();
    $('.otherpreviousemployers').show();
    var counter = parseInt($('#OtherPreviousEmployers').val());

    var max = 3;

    var elements = $('[class^=otherpreviousemployers]').find(':text, :radio, :checkbox, select');

    var enabled = elements.eq(0).is(':enabled');

    for (var i = 1; i <= counter; i++)
        $('.otherpreviousemployers' + i).show();

    if (emptyElements().length == 0 && (counter < max) && enabled)
        $('#addOtherPreviousEmployersTr').show();
    else
        $('#addOtherPreviousEmployersTr').hide();

    var elements = $('[class^=otherpreviousemployers]').find(':text, :radio, :checkbox, select');

    elements.change(function () {
        if (emptyElements().length == 0 && (counter < max) && enabled)
            $('#addOtherPreviousEmployersTr').show();
        else
            $('#addOtherPreviousEmployersTr').hide();
    });

    $('#addOtherPreviousEmployers').click(function (e) {
        e.preventDefault();
        counter++;

        $('#OtherPreviousEmployers').val(counter);
        if (counter <= max)
            $('[class=otherpreviousemployers' + counter + ']').show();
        $('#addOtherPreviousEmployersTr').hide();
    });
};



var absencesmulti = function () {

    var emptyElements = function () {
        return $('[class^=absence]')
                                    .filter(function () { return $(this).css('display') !== 'none'; })
                                    .find(':text, :radio, :checkbox, select')
                                    .filter(function () {
                                        return $(this).val() == '';
                                    });
    };

    $('[class^=absence]').hide();
    $('.absence').show();
    var counter = parseInt($('#AbsencesMulti').val());

    var max = 3;

    var elements = $('[class^=absence]').find(':text, :radio, :checkbox, select');

    var enabled = elements.eq(0).is(':enabled');

    for (var i = 1; i <= counter; i++)
        $('.absence' + i).show();

    if (emptyElements().length == 0 && (counter < max) && enabled)
        $('#addAbsenceTr').show();
    else
        $('#addAbsenceTr').hide();

    var elements = $('[class^=absence]').find(':text, :radio, :checkbox, select');

    elements.change(function () {
        if (emptyElements().length == 0 && (counter < max) && enabled)
            $('#addAbsenceTr').show();
        else
            $('#addAbsenceTr').hide();
    });

    $('#addAbsence').click(function (e) {
        e.preventDefault();
        counter++;

        $('#AbsencesMulti').val(counter);
        if (counter <= max)
            $('[class=absence' + counter + ']').show();
        $('#addAbsenceTr').hide();
    });
};



var detailsonglobalcommuting = function () {

    var emptyElements = function () {
        return $('[class^=detailsonglobalcommuting]')
                                    .filter(function () { return $(this).css('display') !== 'none'; })
                                    .find(':text, :radio, :checkbox, select')
                                    .filter(function () {
                                        return $(this).val() == '';
                                    });
    };

    $('[class^=detailsonglobalcommuting]').hide();
    $('.detailsonglobalcommuting').show();
    var counter = parseInt($('#DetailsOnGlobalCommutings').val());

    var max = 3;

    var elements = $('[class^=detailsonglobalcommuting]').find(':text, :radio, :checkbox, select');

    var enabled = elements.eq(0).is(':enabled');

    for (var i = 1; i <= counter; i++)
        $('.detailsonglobalcommuting' + i).show();

    if (emptyElements().length == 0 && (counter < max) && enabled)
        $('#addDetailsOnGlobalCommutingTr').show();
    else
        $('#addDetailsOnGlobalCommutingTr').hide();

    var elements = $('[class^=detailsonglobalcommuting]').find(':text, :radio, :checkbox, select');

    elements.change(function () {
        if (emptyElements().length == 0 && (counter < max) && enabled)
            $('#addDetailsOnGlobalCommutingTr').show();
        else
            $('#addDetailsOnGlobalCommutingTr').hide();
    });

    $('#addDetailsOnGlobalCommuting').click(function (e) {
        e.preventDefault();
        counter++;

        $('#DetailsOnGlobalCommutings').val(counter);
        if (counter <= max)
            $('[class=detailsonglobalcommuting' + counter + ']').show();
        $('#addDetailsOnGlobalCommutingTr').hide();
    });
};

var terminationpayments = function () {

    var emptyElements = function () {
        return $('[class^=terminationpayments]')
                                    .filter(function () { return $(this).css('display') !== 'none'; })
                                    .find(':text, :radio, :checkbox, select')
                                    .filter(function () {
                                        return $(this).val() == '';
                                    });
    };

    $('[class^=terminationpayments]').hide();
    $('.terminationpayments').show();
    var counter = parseInt($('#TerminationPayments').val());

    var max = 3;

    var elements = $('[class^=terminationpayments]').find(':text, :radio, :checkbox, select');

    var enabled = elements.eq(0).is(':enabled');

    for (var i = 1; i <= counter; i++)
        $('.terminationpayments' + i).show();

    if (emptyElements().length == 0 && (counter < max) && enabled)
        $('#addTerminationPaymentsTr').show();
    else
        $('#addTerminationPaymentsTr').hide();

    var elements = $('[class^=terminationpayments]').find(':text, :radio, :checkbox, select');

    elements.change(function () {
        if (emptyElements().length == 0 && (counter < max) && enabled)
            $('#addTerminationPaymentsTr').show();
        else
            $('#addTerminationPaymentsTr').hide();
    });

    $('#addTerminationPaymentsTr').click(function (e) {
        e.preventDefault();
        counter++;
        $('#TerminationPayments').val(counter);
        if (counter <= max)
            $('[class=terminationpayments' + counter + ']').show();
        $('#addTerminationPaymentsTr').hide();
    });
};


/*
var servicerequestpriority = function () {

    $('#date_ProcessBeforeDate').not(".disabled").datepicker("destroy");

    var format = APIGlobal.DateTime.parseFormat('dd.mm.yyyy');
    var nowTemp = new Date();
    var now = new Date(nowTemp.getFullYear(), nowTemp.getMonth(), nowTemp.getDate(), 0, 0, 0, 0);
    var endDate = new Date(nowTemp.getFullYear(), nowTemp.getMonth(), nowTemp.getDate() + 3, 0, 0, 0, 0);
    var sRDateUrgent = new Date(nowTemp.getFullYear(), nowTemp.getMonth(), nowTemp.getDate() + 1, 0, 0, 0, 0);
    var sRDate = new Date(nowTemp.getFullYear(), nowTemp.getMonth(), nowTemp.getDate() - 3, 0, 0, 0, 0);

    if ($('#ServiceRequestPriority').val() == 'Urgent') {
        var visibledaate = APIGlobal.DateTime.formatDate(sRDateUrgent, format);
        $('#ProcessBeforeDate').val(visibledaate);

        $('#date_ProcessBeforeDate').not(".disabled").datepicker(
        {
            onRender: function (ev) {
                return ev.valueOf() >= endDate.valueOf() || ev.valueOf() < now.valueOf() ? 'disabled' : '';
            }
        });
    }
    else {
        var visibledate = APIGlobal.DateTime.formatDate(sRDate, format);
        $('#ProcessBeforeDate').val(visibledate);
        $('#date_ProcessBeforeDate').not(".disabled").datepicker(
        {
            onRender: function (ev) {
                return ev.valueOf() > now.valueOf() ? 'disabled' : '';
            }
        });
    }
};
*/

var cache = {};

var init = function () {

    // Prevent submit form on enter
    $(window).keydown(function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            return false;
        }
    });

    /*
    var f = $('#LockedFields').val();

    if (f != null) {       
        var fi = [];
        fi = f.split(';');
        for (var l = 1 ; l < fi.length - 1 ; l++) {
            $('#' + fi[l]).attr('disabled', true);
            $('#date_' + fi[l]).addClass("disabled");
        }
    }
    */

    narrowDownInit();
    familyMembers();
    employeeDocuments();
    dependantFamilyMembers();
    dependantsDocuments();
    allowances();
    deductions();
    education();
    otherpreviousemployers();
    terminationpayments();
    absencesmulti();
    multi();
    detailsonglobalcommuting();
    benefits();
    


    var activeTab = $("#activeTab");

    $(".search-select").selectize();
    $('.typeahead').typeahead(typeAheadOptions);
    $('#Co-WorkerGlobalviewID').typeahead(globalTypeAheadOptions);
    $('#IKEAEmailAddress').typeahead(globalEmailTypeAheadOptions);
    $('#FirstName').typeahead(globalNameTypeAheadOptions);
    $('#LastName').typeahead(globalLastNameTypeAheadOptions);

    //var iconFlag = '<i class="icon-flag"></i>';
    $('select').each(function (i) {
        cache[$(this).attr('id')] = $(this).html();
        /*
        val = $(this).attr('id');

        var txt = $(this).find('option:selected').text();

        if ($('#' + val).val() !== $('#OLD_' + val).val() &&
                $('#OLD_' + val).parent().find('.icon-flag').length == 0) {
            $('#OLD_' + val).parent().prepend($(iconFlag));
        }

        if (txt == $('#OLD_' + val).val() &&
             $('#OLD_' + val).parent().find('.icon-flag').length > 0) {
            $('#OLD_' + val).parent().find('.icon-flag').remove();
        }*/
    });

    /*
    $('input').each(function (i) {
        val = $(this).attr('id');
        if ($('#' + val).val() !== $('#OLD_' + val).val() &&
          $('#OLD_' + val).parent().find('.icon-flag').length == 0) {
            $('#OLD_' + val).parent().prepend($(iconFlag));
        }
    });
    */

    if ($("#NewToIKEA").val() == "") {
        disableTypeahead = 0;
    }

    $("#NewToIKEA").on('change', function () {

        if ($('#CustomerId').val() != '31' && ($('#formGuid').length > 0)) {
            $('#Co-WorkerID').typeahead(globalTypeAheadOptions);
            if ($(this).val() == 'Re-Hire') {
                disableTypeahead = 1;
            }
            else {
                disableTypeahead = 0;
                if ($('#Co-WorkerID').val() != "") {
                    $.ajax({
                        url: site.baseUrl + '/search/EmployeesExtendedInfo',
                        type: 'post',
                        data: { formGuid: $('#formGuid').val(), employeenumber: $('#Co-WorkerID').val() },
                        dataType: 'json',
                        success: function (result) {
                            var resultList = jQuery.map(result, function (extendeditem) {
                                var type = $('#' + extendeditem.FormFieldName).attr('type');
                                if (type != 'text') {
                                    $('#' + extendeditem.FormFieldName).val("");
                                }
                                $('input[name="' + extendeditem.FormFieldName + '"]').val("");
                            });
                        }
                    });
                    $('#Co-WorkerID').val("");
                    $('#FirstName').val("");
                    $('#LastName').val("");
                }
            }
        }
    });

    /*

    $('#ServiceRequestPriority').change(function () {
        $('#date_ProcessBeforeDate').not(".disabled").datepicker("destroy");

        var format = APIGlobal.DateTime.parseFormat('dd.mm.yyyy');
        var nowTemp = new Date();
        var now = new Date(nowTemp.getFullYear(), nowTemp.getMonth(), nowTemp.getDate(), 0, 0, 0, 0);
        var endDate = new Date(nowTemp.getFullYear(), nowTemp.getMonth(), nowTemp.getDate() + 3, 0, 0, 0, 0);
        var sRDateUrgent = new Date(nowTemp.getFullYear(), nowTemp.getMonth(), nowTemp.getDate() + 1, 0, 0, 0, 0);
        var sRDate = new Date(nowTemp.getFullYear(), nowTemp.getMonth(), nowTemp.getDate() - 3, 0, 0, 0, 0);

        if ($(this).val() == 'Urgent') {
            var visibledaate = APIGlobal.DateTime.formatDate(sRDateUrgent, format);
            $('#ProcessBeforeDate').val(visibledaate);

            $('#date_ProcessBeforeDate').not(".disabled").datepicker(
            {
                onRender: function (ev) {
                    return ev.valueOf() >= endDate.valueOf() || ev.valueOf() < now.valueOf() ? 'disabled' : '';
                }
            });
        }
        else {
            var visibledate = APIGlobal.DateTime.formatDate(sRDate, format);
            $('#ProcessBeforeDate').val(visibledate);
            $('#date_ProcessBeforeDate').not(".disabled").datepicker(
            {
                onRender: function (ev) {
                    return ev.valueOf() > now.valueOf() ? 'disabled' : '';
                }
            });
        }
    });

    */

    $('.nav-tabs a').click(function (e) {
        e.preventDefault();
        $(this).tab('show');
        var url = $(this).attr("data-url");
        var hash = this.hash;

        if (uploader != null && hash == '#attachments') {
            uploader.refresh();
        }

        if (url != undefined) {
            $.post(url, function (data) {
                $(hash).find("#gvList").html(data.View);
                initGV();
            });
        }
        activeTab.val($(this).attr('href'));
    });

    $('a.navigate').click(function (e) {
        e.preventDefault();
        $('.nav-tabs a[href="' + $(this).attr('href') + '"]').click();
        return false;
    });

    if (activeTab.length > 0)
        $('.nav-tabs a[href="' + activeTab.val() + '"]').click();

    // global view
    var initGV = function () {
        var copy = $('.copy');
        if (copy.length > 0) {
            copy.on('click', function (e) {
                e.preventDefault();
                $(this).closest('tr').find('.copy-me').selectText();
            });
        }
    }

    initGV();

    // datepicker

    $(document).on("click", ".date", function () {
        $(this).not(".disabled").datepicker('show');
    });

    // Specific to Netherlands for hiringdate. Customer requirements changed. Saved for reference
    // Only enables 1st and 15th day
    //$('#date_ContractStartDate').datepicker(
    //{
    //    onRender: function (ev) {
    //        if (ev.getDate() === 1 || ev.getDate() === 15)
    //            return '';
    //        return 'disabled';
    //    }
    //});

    // Specific to Netherlands, enables only dates after ContractStartDate
    var nowTemp = new Date();
    var now = new Date(nowTemp.getFullYear(), nowTemp.getMonth(), nowTemp.getDate(), 0, 0, 0, 0);
    if (document.getElementById("ContractStartDate")) {
        var contractstartdate = new Date(document.getElementById('ContractStartDate').value);
        $('#date_AllowancesValidFrom, #date_ContractEndDate').not(".disabled").datepicker({
            onRender: function (date) {

                return date.valueOf() < contractstartdate.valueOf() ? 'disabled' : '';
            }
        });
    }
    if (document.getElementById("ChangeValidFrom")) {
        var changevalidfrom = new Date(document.getElementById('ChangeValidFrom').value);
        $('#date_AllowancesValidFrom, #date_ContractEndDate, #date_DeductionsValidFrom').not(".disabled").datepicker({
            onRender: function (date) {
                return date.valueOf() < changevalidfrom.valueOf() ? 'disabled' : '';
            }
        });
    }



    // Built for Global, sets LockCDSAccountFrom date to same date as last day of employment
    var LastDayOfEmployment = $('#date_LastDayOfEmployment');
    LastDayOfEmployment.not(".disabled").datepicker()
            .on('changeDate', function (e) {
                document.getElementById('LockCDSAccountFrom').value = document.getElementById('LastDayOfEmployment').value;

                if (document.getElementById('PaymentDate')) {
                    document.getElementById('PaymentDate').value = document.getElementById('LastDayOfEmployment').value;

                    for (var i = 2; i <= 3; i++) {
                        var PaymentDate = "PaymentDate" + [i].toString();


                        document.getElementById(PaymentDate).value = document.getElementById('LastDayOfEmployment').value;

                    }
                }
                else {
                    if (document.getElementById('TerminationPaymentDate')) {
                        document.getElementById('TerminationPaymentDate').value = document.getElementById('LastDayOfEmployment').value;

                        for (var i = 2; i <= 3; i++) {
                            var PaymentDate = "TerminationPaymentDate" + [i].toString();


                            document.getElementById(PaymentDate).value = document.getElementById('LastDayOfEmployment').value;

                        }
                    }
                }

                //this rows is specific to Complete Termination Details
                if (document.getElementById('DeductionDate')) {

                    document.getElementById('DeductionDate').value = document.getElementById('LastDayOfEmployment').value;

                    for (var i = 2; i <= 3; i++) {

                        var DeductionDate = "DeductionDate" + [i].toString();


                        document.getElementById(DeductionDate).value = document.getElementById('LastDayOfEmployment').value;
                    }
                }

            });




    var noticeHiringDate = $('#notice_HiringDate');
    var dateHiringDate = $('#date_HiringDate, #date_ContractStartDate, #date_LastDayOfEmployment');
    var noticeHiringDateWrong = $('#notice_HiringDateWrong');
    if (noticeHiringDate.length > 0 && dateHiringDate.length > 0) {
        var startDate = new Date();
        startDate.setHours(0, 0, 0, 0);

        dateHiringDate.not(".disabled").datepicker()
            .on('changeDate', function (e) {


                if (!(e.date.getDate() === 1 || e.date.getDate() === 15)) {
                    noticeHiringDateWrong.show();
                } else {
                    noticeHiringDateWrong.hide();
                }
                if (e.date.valueOf() < startDate.valueOf()) {
                    noticeHiringDate.show();
                } else {
                    noticeHiringDate.hide();
                }
            });
    }


    var noticeFutureDate = $('#notice_ValidUntilDate, #notice_FutureDate');
    var dateDatePicker = $('#date_DocumentValidUntil, #date_ChangeValidFrom');
    if (noticeFutureDate.length > 0 && dateHiringDate.length > 0) {
        var startDate = new Date();
        startDate.setHours(0, 0, 0, 0);

        dateDatePicker.not(".disabled").datepicker()
            .on('changeDate', function (e) {
                if (e.date.valueOf() < startDate.valueOf()) {

                    noticeFutureDate.show();
                } else {

                    noticeFutureDate.hide();
                }
            });
    }




    // Specific to dateofbirth
    // Only enables past dates. So that users cannot add employees that is not born.
    var nowTemp = new Date();
    var now = new Date(nowTemp.getFullYear(), nowTemp.getMonth(), nowTemp.getDate(), 0, 0, 0, 0);
    $('#date_DateOfBirth, #date_FamilyMemberDateOfBirth, #date_DependantsDateOfBirth').not(".disabled").datepicker(
    {
        onRender: function (ev) {
            return ev.valueOf() > now.valueOf() ? 'disabled' : '';
        }
    });


    // effective date

    var noticeEffectiveDate = $('#notice_EffectiveDate');
    var dateEffectiveDate = $('#date_EffectiveDate');

    if (noticeEffectiveDate.length > 0 && dateEffectiveDate.length > 0) {
        var startDate = new Date();
        startDate.setHours(0, 0, 0, 0);

        dateEffectiveDate.not(".disabled").datepicker()
            .on('changeDate', function (e) {
                if (e.date.valueOf() < startDate.valueOf()) {
                    noticeEffectiveDate.show();
                } else {
                    noticeEffectiveDate.hide();
                }
            });


    }

    // predefined functions 

    //var cache = {};
    //$('select').each(function (i) {
    //    cache[$(this).attr('id')] = $(this).html();
    //});

    var predefined = $('.predefined');

    predefined.change(function (e) {
        
        var $this = $(this);
        var value = $this.val();
        var hidden = $('.predefined_' + $this.attr('id'));

        hidden.each(function (i) {

            var hit = false;
            var json = $.parseJSON($(this).text());
            var dependent = $('#' + json.predefined.name);
            var selectCache = $(cache[json.predefined.name]);

            // might give performance issues, other solutions might be creating of separate method for only search-select predefined elemens with using native api from selectize http://stackoverflow.com/questions/20458585/add-item-to-input-programmatically
            
            if (dependent[0] != undefined) {
                if (dependent.selectize()) {
                    dependent[0].selectize.destroy();
                }
            }

            for (var i = 0; i < json.predefined.dependent.length; i++) {

                if ($.trim(json.predefined.dependent[i].selected) == $.trim(value)) {

                    dependent.find('option').remove();

                    if (json.predefined.dependent[i].show !== "") {
                        var options = json.predefined.dependent[i].show.split(',');

                        dependent.append('<option value="" selected></option>');

                        selectCache.each(function () {
                            var opt = $(this);

                            for (var i = 0; i < options.length; i++) {
                                if ($.trim(opt.val()) == $.trim(options[i]))
                                    dependent.append(opt);
                            }
                        });

                    }

                    hit = true;
                    break;
                }
            }

            if (hit)
                dependent.change();

            if (value === "" || !hit) {

                dependent.find('option').remove();
                selectCache.each(function () {
                    var opt = $(this);
                    dependent.append(opt);
                });
            }

            var childControl = document.getElementById(json.predefined.name);

            // might give performance issues, other solutions might be creating of separate method for only search-select predefined elemens with using native api from selectize http://stackoverflow.com/questions/20458585/add-item-to-input-programmatically
            if (dependent.hasClass("search-select")) {
                dependent.selectize();
            }

        });

    });

    predefined.each(function () {
        $(this).change();
    });


    //$('#HasEvent').val('false');

    validate.run($('#actionState').val());

};

// plupload
var uploader;
var initUpload = function () {

    uploader = new plupload.Uploader({
        runtimes: 'gears,html5,flash,silverlight,browserplus,html4',
        browse_button: 'pickfiles',
        container: 'container',
        max_file_size: '30mb',
        url: site.baseUrl + '/files/upload',
        flash_swf_url: site.baseUrl + '/assets/plupload/plupload.flash.swf',
        silverlight_xap_url: site.baseUrl + '/assets/plupload/plupload.silverlight.xap'
    });

    uploader.bind('Init', function (up, params) { /*$('#filelist').html("<div>Current runtime: " + params.runtime + "</div>");*/ });

    $('#uploadfiles').click(function (e) {
        uploader.start();
        e.preventDefault();
    });

    uploader.init();

    uploader.bind('FilesAdded', function (up, files) {

        up.files.splice();

        $.each(files, function (i, file) {
            $('#filelist').prepend(
                '<div id="' + file.id + '">' +
                file.name + ' (' + plupload.formatSize(file.size) + ') <b></b>' +
            ' <a href="" class="remove btn btn-link btn-mini"><i class="icon-trash"></i></a>' +
            ' <input type="hidden" name="uploads" value="' + file.name + '"/></div>');

            $('#' + file.id + ' a.remove').first().click(function (e) {
                e.preventDefault();
                up.removeFile(file);
                $('#' + file.id).remove();
                if (up.files.length == 0) {
                    $('#uploadfiles').css('display', 'none');
                }
            });

        });

        up.refresh(); // Reposition Flash/Silverlight
    });

    uploader.bind('UploadProgress', function (up, file) { /*$('#' + file.id + " b").html(file.percent + "%");*/ });

    uploader.bind('Error', function (up, err) {
        $('#filelist').prepend("<div>Error: " + err.code +
            ", Message: " + err.message +
            (err.file ? ", File: " + err.file.name : "") +
            "</div>"
        );

        up.refresh(); // Reposition Flash/Silverlight
    });

    uploader.bind('FileUploaded', function (up, file) { /*$('#' + file.id + " b").html("100%");*/ });
};

//var f = "";
//var hasChange = "";
var actionStateChanged = function () {

    /*f = $('#LockedFields').val();

    if (f != null) {     
        var fi = [];
        fi = f.split(';');
        for (var l = 1 ; l < fi.length - 1 ; l++) {
            $('#' + fi[l]).prop('disabled', false);
            $('#date_' + fi[l]).removeAttr("disabled");
        }
    }

    hasChange = $('#FirstEventName').val();
    */
    var form = $('form');
    var formData = form.serialize();
    var url = form.attr('action');

        $.ajax({
            type: "POST",
            url: url,
            data: formData
        })
        .done(function (data) {
            if (data.View != '') {
                $('form').replaceWith(data.View);
                init();
                
                InitIntegration();

                if ($('#pickfiles').length > 0) {
                    initUpload();
                }
            } else {
                reload(data.CancelCase);
            }

        }); 
};

$(document).on('submit', 'form', function (e) {
    /*var f = $('#LockedFields').val();

    if (f != null) {
        $('#ChangeType').prop('disabled', false);       
        var fi = [];
        fi = f.split(';');
        for (var l = 1 ; l < fi.length - 1 ; l++) {
            $('#' + fi[l]).prop('disabled', false);
            $('#date_' + fi[l]).removeAttr("disabled");
        }
    }    

    allFieldname = $('#FirstEventName').val();
    var lastvalidation = $('#LastValidation').val();
    var val = $('#actionState').val();
    if (allFieldname != '' && allFieldname != undefined) {

        var url = '';
        var ajaxInfo = $('#ajaxInfo');
        if (ajaxInfo.length > 0) {
            url = ajaxInfo.attr('url');
        }

        return $.ajax({
            url: site.baseUrl + url + 'ChangeRules/',
            type: 'post',
            data: { id: val, eventFieldName: allFieldname },
            dataType: 'json',
            success: function (result) {
                $('#validate_' + val).val(result);
                $('#LastValidation').val(result);
                validate.run(val);
            }
        });
    }*/

    var elem = $('#navigation').find(':button, select').addClass("disabled").attr('readonly', 'readonly');

    if ($('#filelist').length > 0 && uploader != undefined) {
        if (uploader.files.length > 0) {
            uploader.bind('StateChanged', function () {
                if (uploader.files.length === (uploader.total.uploaded + uploader.total.failed)) {
                    $('form')[0].submit();
                }
            });
            uploader.start();
            return false;
        }
    }
});

$(document).on('click', '#actionStateChange', function (e) {

    //var hasChange = $('#FirstEventName').val();
    var form = $('form');
    var url = form.attr('action');
    /*
    if ((url.toString().indexOf("ChangeTermsConditions") > -1) && hasChange == "")
    {
        alert("It will not possible to go furthure in this process as you didn't make any changes.");
        return;
    }
    */
    
    if ($('#actionState').val() != '') {

        if ($('#actionState').val() == '99') {
            if (!confirm($('#cancelRequest').text()))
                return;
        }

        var elem = $('#navigation').find(':button, select').addClass("disabled").attr('readonly', 'readonly');

        var proceed = true;

        if ($('#filelist').length > 0 && uploader != undefined) {
            if (uploader.files.length > 0) {
                uploader.bind('StateChanged', function () {
                    if (uploader.files.length === (uploader.total.uploaded + uploader.total.failed)) {
                        proceed = true
                        actionStateChanged()
                    } else {
                        proceed = false;
                    }
                });

                uploader.start();
            }
        }

        if (proceed)
            actionStateChanged();
    }

    e.preventDefault();
});

$(document).on('click', '.print', function (e) {
    e.preventDefault();

    var $this = $(this);
    var url = $this.attr('href');
    var concludedOn = $this.closest('tr').find('.concludedOn').val();

    $.ajax({
        type: "POST",
        url: url,
        data: { concludedOn: concludedOn }
    })
        .done(function (data) {
            if (data.Exception)
                location.href = location.href;
            else {
                $this.closest('tr').find('td').eq(2).text(data.Result);
                url = url.replace('print', 'contract');
                window.open(url);
            }
        });
});

$(document).on('click', '.btn-delete-file', function (e) {
    e.preventDefault();

    var t = $(this);
    var url = t.attr('href');

    $.ajax({
        type: "POST",
        url: url
    })
        .done(function (data) {
            t.parent().remove();
            if ($('#attachmentCounter').length > 0)
                $('#attachmentCounter').html($('.btn-delete-file').length);
        });
});

$(function () {
    init();
    if ($('#pickfiles').length > 0)
        initUpload();
});