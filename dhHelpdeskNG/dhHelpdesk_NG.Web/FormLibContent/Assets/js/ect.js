/// <reference path="spin.js" />
/// <reference path="spin.js" />
//Selection

// Globals
var isEmbed = window != window.parent;
var site = site || {};
site.baseUrl = "";
var disableTypeahead = 1;

var core2 = core2 || {};


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
            end.setMonth(end.getMonth() - months);

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
    var jump = false;
    if ('parentIFrame' in window)
        jump = true;

    // DL: used with iframeResizer.contentWindow.min.js
    //if (jump && cancelCase) {
    //    window.parentIFrame.sendMessage('cancelCase');
    //}

    if (jump) {
        if (cancelCase) {
            window.parentIFrame.sendMessage({ type: 'cancelCase' });
        } else {
            //refresh hd case status
            window.parentIFrame.sendMessage({ type: 'refreshData', caseStatus: 1 });
        }
    }

    if (!jump && window.parent.cancelCase != undefined && cancelCase) {
        window.parent.cancelCase(3, 0);
    }
    else if (!isEmbed) {
        if (cancelCase) {
            window.open('', '_self', '');
            window.opener = self; window.close();
        }
    }

    var action = $('form').attr('action');
    location.href = action;
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

//Future/DAB for "Global" customer (UK, IE, NL, KR and NO)
//Get #CustomerId from ajaxinfo in _GlobalNavRev.cshtml
//Istället för att lägga till på 69 olika ställen.
//Kolla #AllCoWorkers
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
            data: { query: query, customerId: $('#CustomerId').val(), allCoWorkers: $('#AllCoWorker').length > 0 },
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
                        , iKEANetworkID: item.IKEANetworkID
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
                    setCurrentRecord(result);
                }
            });
        }

        $('input[name="Company"]').val(item.company);
        $('#emOLD_Company').text(item.company);
        $('#OLD_Company').val(item.company);

        $('input[name="BusinessUnit"]').val(item.unit);
        $('#emOLD_BusinessUnit').text(item.unit);
        $('#OLD_BusinessUnit').val(item.unit);

        //check that element exist
        if ($('#BusinessUnitId').length) {
            //1. Set UnitId to hidden field
            //2. Trigger change event
            $('#BusinessUnitId').val(item.unitId).trigger('change');
        }

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

        $('input[name="IKEANetworkID"]').val(item.iKEANetworkID);
        $('#emOLD_IKEANetworkID').text(item.iKEANetworkID);
        $('#OLD_IKEANetworkID').val(item.iKEANetworkID);

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
            //  data: { query: query, customerId: $('#CustomerId').val(), formFieldName: "IKEAEmailAddress" },
            data: { query: query, customerId: $('#CustomerId').val(), allCoWorkers: $('#AllCoWorker').length > 0, formFieldName: "IKEAEmailAddress" },
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
                        , iKEANetworkID: item.IKEANetworkID
                    };

                    return JSON.stringify(aItem);
                });

                return process(resultList);
            }
        });
    }
    ,

    matcher: function (obj) {
        var item = JSON.parse(obj);
        return ~item.num.toLowerCase().indexOf(this.query.toLowerCase())
            || ~item.email.toLowerCase().indexOf(this.query.toLowerCase());
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
                    setCurrentRecord(result);
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

        //check that element exist
        if ($('#BusinessUnitId').length) {
            //1. Set UnitId to hidden field
            //2. Trigger change event
            $('#BusinessUnitId').val(item.unitId).trigger('change');
        }

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

        $('input[name="IKEANetworkID"]').val(item.iKEANetworkID);
        $('#emOLD_IKEANetworkID').text(item.iKEANetworkID);
        $('#OLD_IKEANetworkID').val(item.iKEANetworkID);

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
            data: { query: query, customerId: $('#CustomerId').val(), allCoWorkers: $('#AllCoWorker').length > 0, formFieldName: "FirstName" },
            dataType: 'json',
            success: function (result) {
                var resultList = jQuery.map(result, function (item) {
                    var aItem = {
                        id: item.Id
                        , num: item.EmployeeNumber
                        , name: item.Name
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
                        , iKEANetworkID: item.IKEANetworkID
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

        var result = item.firstname + ' ' + item.lastname;
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
                    setCurrentRecord(result);
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

        //check that element exist
        if ($('#BusinessUnitId').length) {
            //1. Set UnitId to hidden field
            //2. Trigger change event
            $('#BusinessUnitId').val(item.unitId).trigger('change');
        }

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

        $('input[name="IKEANetworkID"]').val(item.iKEANetworkID);
        $('#emOLD_IKEANetworkID').text(item.iKEANetworkID);
        $('#OLD_IKEANetworkID').val(item.iKEANetworkID);

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
            data: { query: query, customerId: $('#CustomerId').val(), allCoWorkers: $('#AllCoWorker').length > 0, formFieldName: "LastName" },
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
                        , iKEANetworkID: item.IKEANetworkID
                        //, watchdate: item.watchdate
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
            || ~item.lastname.toLowerCase().indexOf(this.query.toLowerCase());
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
                    setCurrentRecord(result);
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

        //check that element exist
        if ($('#BusinessUnitId').length) {
            //1. Set UnitId to hidden field
            //2. Trigger change event
            $('#BusinessUnitId').val(item.unitId).trigger('change');
        }

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

        $('input[name="IKEANetworkID"]').val(item.iKEANetworkID);
        $('#emOLD_IKEANetworkID').text(item.iKEANetworkID);
        $('#OLD_IKEANetworkID').val(item.iKEANetworkID);

        return item.lastname;
    }
};

var globalNetworkIdTypeAheadOptions = {
    items: 10,
    minLength: 2,
    source: function (query, process) {
        if (disableTypeahead == 0) {
            disableTypeahead == 1;
            return;
        }
        return $.ajax({
            url: site.baseUrl + '/search/globalview',
            type: 'post',
            data: { query: query, customerId: $('#CustomerId').val(), allCoWorkers: $('#AllCoWorker').length > 0, formFieldName: "IKEANetworkID" },
            dataType: 'json',
            success: function (result) {
                var resultList = jQuery.map(result, function (item) {
                    var aItem = {
                        id: item.Id
                        , num: item.EmployeeNumber
                        , name: item.Name
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
                        , iKEANetworkID: item.IKEANetworkID
                    };
                    return JSON.stringify(aItem);
                });

                return process(resultList);
            }
        });
    },

    matcher: function (obj) {
        var item = JSON.parse(obj);
        return ~item.iKEANetworkID.toLowerCase().indexOf(this.query.toLowerCase())
            || ~item.iKEANetworkID.toLowerCase().indexOf(this.query.toLowerCase());
    },

    sorter: function (items) {
        var beginswith = [], caseSensitive = [], caseInsensitive = [], item;
        while (aItem = items.shift()) {
            var item = JSON.parse(aItem);
            if (!item.iKEANetworkID.toLowerCase().indexOf(this.query.toLowerCase())) beginswith.push(JSON.stringify(item));
            else if (~item.iKEANetworkID.indexOf(this.query)) caseSensitive.push(JSON.stringify(item));
            else caseInsensitive.push(JSON.stringify(item));
        }

        return beginswith.concat(caseSensitive, caseInsensitive);
    },

    highlighter: function (obj) {
        var item = JSON.parse(obj);
        var query = this.query.replace(/[\-\[\]{}()*+?.,\\\^$|#\s]/g, '\\$&');

        var result = item.iKEANetworkID
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
                    setCurrentRecord(result);
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

        //check that element exist
        if ($('#BusinessUnitId').length) {
            //1. Set UnitId to hidden field
            //2. Trigger change event
            $('#BusinessUnitId').val(item.unitId).trigger('change');
        }

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

        $('input[name="IKEANetworkID"]').val(item.iKEANetworkID);
        $('#emOLD_IKEANetworkID').text(item.iKEANetworkID);
        $('#OLD_IKEANetworkID').val(item.iKEANetworkID);


        return item.iKEANetworkID;
    }
};

function setCurrentRecord(result) {

    var resultList = jQuery.map(result, function (extendeditem) {

        //FIX, only set current value if formfieldvalue has value //TAN
        if (extendeditem.FormFieldValue.length > 0) {

            var element = $('#' + extendeditem.FormFieldName);
            var type = $('#' + extendeditem.FormFieldName).attr('type');

            var defaultbyformfieldidentifier = '1';

            //check that attribute exist
            if (element.attr('data-defaultbyformfieldidentifier') != undefined) {
                defaultbyformfieldidentifier = element.data('defaultbyformfieldidentifier');
            }

            //only set if 
            if (defaultbyformfieldidentifier == '1') {
                if (extendeditem.FormFieldName.toLowerCase() == 'DateOfBirth'.toLowerCase() && extendeditem.FormFieldValue.length > 0) {
                    $('#date_DateOfBirth').datepicker('setValue', extendeditem.FormFieldValue);
                }
                else if (extendeditem.FormFieldName.toLowerCase() == 'ContractEndDate'.toLowerCase() && extendeditem.FormFieldValue.length > 0) {
                    $('#date_ContractEndDate').datepicker('setValue', extendeditem.FormFieldValue);
                }
                else {
                    if (type != 'text') {

                        //NO - narrowing down on change terms & conditions, therefore we need to wait... 
                        //TODO: Change this solution...
                        if (extendeditem.FormFieldName.toLowerCase() == 'PSGroup'.toLowerCase()) {

                            setTimeout(function () {

                                $('#' + extendeditem.FormFieldName).val(extendeditem.FormFieldValue);

                                $('#' + extendeditem.FormFieldName + ' option').filter(function () {
                                    return $(this).text() == extendeditem.FormFieldValue;
                                }).prop('selected', true);

                                $('#PSGroup').trigger("change");

                            }, 500);

                        }
                        else {
                            $('#' + extendeditem.FormFieldName).val(extendeditem.FormFieldValue);
                            $('#' + extendeditem.FormFieldName + ' option').filter(function () {
                                return $(this).text() == extendeditem.FormFieldValue;
                            }).prop('selected', true);

                        }

                        if ($('#' + extendeditem.FormFieldName)[0] != undefined && $('#' + extendeditem.FormFieldName)[0].selectize) {

                            //Used in NO - change terms conditions
                            if (extendeditem.FormFieldName.toLowerCase() == 'PSGroup'.toLowerCase() || extendeditem.FormFieldName.toLowerCase() == 'ReportsToLineManager'.toLowerCase() || extendeditem.FormFieldName.toLowerCase() == 'PositionTitle'.toLowerCase() || extendeditem.FormFieldName.toLowerCase() == 'JobTitle'.toLowerCase()) {
                                setTimeout(function () {
                                    $('#' + extendeditem.FormFieldName)[0].selectize.setValue(extendeditem.FormFieldValue);
                                }, 500);

                                //dont need timeout, set directly
                                if (extendeditem.FormFieldName.toLowerCase() == 'JobTitle'.toLowerCase()) {
                                    try {
                                        $('#employeeSearch_JobTitle').val(extendeditem.FormFieldValue);
                                    } catch (e) {
                                    }
                                }
                            }
                            else {
                                $('#' + extendeditem.FormFieldName)[0].selectize.setValue(extendeditem.FormFieldValue);
                            }
                        }

                        if (extendeditem.FormFieldName == 'NewCompany')
                            changeNewCompany(true);

                        if (extendeditem.FormFieldName == 'WorkedCompany' | extendeditem.FormFieldName == 'WorkedCompany2' | extendeditem.FormFieldName == 'WorkedCompany3' | extendeditem.FormFieldName == 'WorkedCompany4' | extendeditem.FormFieldName == 'WorkedCompany5') {
                            setCompanyValue(extendeditem.FormFieldName, extendeditem.FormFieldId, true);
                        }


                        if (extendeditem.FormFieldName == 'EmploymentCategory' && extendeditem.FormFieldValue == 'Permanent') {
                            $('#date_ContractEndDate').datepicker("destroy");
                            $('#date_ContractEndDate').addClass("disabled");
                            $('#ContractEndDate').prop('disabled', true);
                        }

                    } else {
                        $('input[name="' + extendeditem.FormFieldName + '"]').val(extendeditem.FormFieldValue);
                    }
                }
            }
        }

        $('#emOLD_' + extendeditem.FormFieldName).text(extendeditem.FormFieldValue);
        $('#OLD_' + extendeditem.FormFieldName).val(extendeditem.FormFieldValue);
    });
}

//Polen har inte #ajaxinfo - använder _Navigation.cshtml. 
//Här får vi ha kvar #CustomerId
// Special Type Ahead function for Poland
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

        // DL 20141712 - Function (PL) narrowing down old Unit unless New Employee Unit exists.
        var funct = $('#Function');
        funct.typeahead({
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

    //var max = 5;
    //get info from hidden, if hidden don´t exist. choose default value
    var max = parseInt($('#MultipleEntrySectionsNr').val()) || 5;

    //Hide button if we already show all available
    if (counter >= max && max >= 5) {
        $('#addFamilyMember').hide();
    }

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
        counter = parseInt($('#FamilyMembers').val()) || 1;
        counter++;

        if (counter <= max) {
            $('#FamilyMembers').val(counter);
            $('[class*=familyMember' + counter + ']').show();
            $('#addFamilyMemberTr').hide();

            //Hide button if we already show all available
            if (counter >= max && max >= 5) {
                $('#addFamilyMember').hide();
            }
        }
    });
};

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
    var counter = parseInt($('#EmployeeDocuments').val()) || 1;

    //get info from hidden, if hidden don´t exist. choose default value
    var max = parseInt($('#MultipleEntrySectionsNr').val()) || 5;

    //Hide button if we already show all available
    if (counter >= max && max >= 5) {
        $('#addEmployeeDocument').hide();
    }


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
        counter = parseInt($('#EmployeeDocuments').val()) || 1;
        counter++;

        if (counter <= max) {
            $('#EmployeeDocuments').val(counter);
            $('[class*=employeeDocument' + counter + ']').show();
            $('#addEmployeeDocumentTr').hide();

            //Hide button if we already show all available
            if (counter >= max && max >= 5) {
                $('#addEmployeeDocument').hide();
            }
        }
    });
};

var PermanentCommunicationTypes = function () {

    var emptyElements = function () {
        return $('[class^=PermanentCommunicationType]')
                                    .filter(function () { return $(this).css('display') !== 'none'; })
                                    .find(':text, :radio, :checkbox, select')
                                    .filter(function () {
                                        return $(this).val() == '';
                                    });
    };

    $('[class^=PermanentCommunicationType]').hide();
    $('.PermanentCommunicationType').show();
    var counter = parseInt($('#PermanentCommunicationtypes').val()) || 1;;

    //var max = 5;
    //get info from hidden, if hidden don´t exist. choose default value
    var max = parseInt($('#MultipleEntrySectionsNr').val()) || 4;

    //Hide button if we already show all available
    if (counter >= max && max >= 5) {
        $('#addPermanentCommunicationType').hide();
    }

    var elements = $('[class^=PermanentCommunicationType]').find(':text, :radio, :checkbox, select');

    var enabled = elements.eq(0).is(':enabled');

    for (var i = 1; i <= counter; i++)
        $('.PermanentCommunicationType' + i).show();

    if (emptyElements().length == 0 && (counter < max) && enabled)
        $('#addPermanentCommunicationtypesTr').show();
    else
        $('#addPermanentCommunicationtypesTr').hide();

    var elements = $('[class^=PermanentCommunicationType]').find(':text, :radio, :checkbox, select');

    elements.change(function () {
        if (emptyElements().length == 0 && (counter < max) && enabled)
            $('#addPermanentCommunicationtypesTrr').show();
        else
            $('#addPermanentCommunicationtypesTrr').hide();
    });

    $('#addPermanentCommunicationType').click(function (e) {
        e.preventDefault();
        counter = parseInt($('#PermanentCommunicationtypes').val()) || 1;
        counter++;

        if (counter <= max) {
            $('#PermanentCommunicationtypes').val(counter);
            $('[class*=PermanentCommunicationType' + counter + ']').show();
            $('#addPermanentCommunicationtypesTr').hide();

            //Hide button if we already show all available
            if (counter >= max && max >= 5) {
                $('#addPermanentCommunicationType').hide();
            }

        }
    });
};

var EmergencyCommunicationTypes = function () {

    var emptyElements = function () {
        return $('[class^=EmergencyCommunicationType]')
                                    .filter(function () { return $(this).css('display') !== 'none'; })
                                    .find(':text, :radio, :checkbox, select')
                                    .filter(function () {
                                        return $(this).val() == '';
                                    });
    };

    $('[class^=EmergencyCommunicationType]').hide();
    $('.EmergencyCommunicationType').show();
    var counter = parseInt($('#EmergencyCommunicationtypes').val()) || 1;;

    //var max = 5;
    //get info from hidden, if hidden don´t exist. choose default value
    var max = parseInt($('#MultipleEntrySectionsNr').val()) || 4;

    //Hide button if we already show all available
    if (counter >= max && max >= 5) {
        $('#addEmergencyCommunicationType').hide();
    }

    var elements = $('[class^=EmergencyCommunicationType]').find(':text, :radio, :checkbox, select');

    var enabled = elements.eq(0).is(':enabled');

    for (var i = 1; i <= counter; i++)
        $('.EmergencyCommunicationType' + i).show();

    if (emptyElements().length == 0 && (counter < max) && enabled)
        $('#addEmergencyCommunicationtypesTr').show();
    else
        $('#addEmergencyCommunicationtypesTr').hide();

    var elements = $('[class^=EmergencyCommunicationType]').find(':text, :radio, :checkbox, select');

    elements.change(function () {
        if (emptyElements().length == 0 && (counter < max) && enabled)
            $('#addEmergencyCommunicationtypesTrr').show();
        else
            $('#addEmergencyCommunicationtypesTrr').hide();
    });

    $('#addEmergencyCommunicationType').click(function (e) {
        e.preventDefault();
        counter = parseInt($('#EmergencyCommunicationtypes').val()) || 1;
        counter++;

        if (counter <= max) {
            $('#EmergencyCommunicationtypes').val(counter);
            $('[class*=EmergencyCommunicationType' + counter + ']').show();
            $('#addEmergencyCommunicationtypesTr').hide();

            //Hide button if we already show all available
            if (counter >= max && max >= 5) {
                $('#addEmergencyCommunicationType').hide();
            }

        }
    });
};

var HomeCommunicationTypes = function () {

    var emptyElements = function () {
        return $('[class^=HomeCommunicationType]')
                                    .filter(function () { return $(this).css('display') !== 'none'; })
                                    .find(':text, :radio, :checkbox, select')
                                    .filter(function () {
                                        return $(this).val() == '';
                                    });
    };

    $('[class^=HomeCommunicationType]').hide();
    $('.HomeCommunicationType').show();
    var counter = parseInt($('#HomeCommunicationtypes').val()) || 1;;

    //var max = 5;
    //get info from hidden, if hidden don´t exist. choose default value
    var max = parseInt($('#MultipleEntrySectionsNr').val()) || 4;

    //Hide button if we already show all available
    if (counter >= max && max >= 5) {
        $('#addHomeCommunicationType').hide();
    }

    var elements = $('[class^=HomeCommunicationType]').find(':text, :radio, :checkbox, select');

    var enabled = elements.eq(0).is(':enabled');

    for (var i = 1; i <= counter; i++)
        $('.HomeCommunicationType' + i).show();

    if (emptyElements().length == 0 && (counter < max) && enabled)
        $('#addHomeCommunicationtypesTr').show();
    else
        $('#addHomeCommunicationtypesTr').hide();

    var elements = $('[class^=HomeCommunicationType]').find(':text, :radio, :checkbox, select');

    elements.change(function () {
        if (emptyElements().length == 0 && (counter < max) && enabled)
            $('#addHomeCommunicationtypesTrr').show();
        else
            $('#addHomeCommunicationtypesTrr').hide();
    });

    $('#addHomeCommunicationType').click(function (e) {
        e.preventDefault();
        counter = parseInt($('#HomeCommunicationtypes').val()) || 1;
        counter++;

        if (counter <= max) {
            $('#HomeCommunicationtypes').val(counter);
            $('[class*=HomeCommunicationType' + counter + ']').show();
            $('#addHomeCommunicationtypesTr').hide();

            //Hide button if we already show all available
            if (counter >= max && max >= 5) {
                $('#addHomeCommunicationType').hide();
            }

        }
    });
};

var ENHomeCommunicationTypes = function () {

    var emptyElements = function () {
        return $('[class^=ENHomeCommunicationType]')
                                    .filter(function () { return $(this).css('display') !== 'none'; })
                                    .find(':text, :radio, :checkbox, select')
                                    .filter(function () {
                                        return $(this).val() == '';
                                    });
    };

    $('[class^=ENHomeCommunicationType]').hide();
    $('.ENHomeCommunicationType').show();
    var counter = parseInt($('#ENHomeCommunicationtypes').val()) || 1;;

    //var max = 5;
    //get info from hidden, if hidden don´t exist. choose default value
    var max = parseInt($('#MultipleEntrySectionsNr').val()) || 4;

    //Hide button if we already show all available
    if (counter >= max && max >= 5) {
        $('#addENHomeCommunicationType').hide();
    }

    var elements = $('[class^=ENHomeCommunicationType]').find(':text, :radio, :checkbox, select');

    var enabled = elements.eq(0).is(':enabled');

    for (var i = 1; i <= counter; i++)
        $('.ENHomeCommunicationType' + i).show();

    if (emptyElements().length == 0 && (counter < max) && enabled)
        $('#addENHomeCommunicationtypesTr').show();
    else
        $('#addENHomeCommunicationtypesTr').hide();

    var elements = $('[class^=ENHomeCommunicationType]').find(':text, :radio, :checkbox, select');

    elements.change(function () {
        if (emptyElements().length == 0 && (counter < max) && enabled)
            $('#addENHomeCommunicationtypesTrr').show();
        else
            $('#addENHomeCommunicationtypesTrr').hide();
    });

    $('#addENHomeCommunicationType').click(function (e) {
        e.preventDefault();
        counter = parseInt($('#ENHomeCommunicationtypes').val()) || 1;
        counter++;

        if (counter <= max) {
            $('#ENHomeCommunicationtypes').val(counter);
            $('[class*=ENHomeCommunicationType' + counter + ']').show();
            $('#addENHomeCommunicationtypesTr').hide();

            //Hide button if we already show all available
            if (counter >= max && max >= 5) {
                $('#addENHomeCommunicationType').hide();
            }

        }
    });
};

var WorkCommunicationTypes = function () {

    var emptyElements = function () {
        return $('[class^=WorkCommunicationType]')
                                    .filter(function () { return $(this).css('display') !== 'none'; })
                                    .find(':text, :radio, :checkbox, select')
                                    .filter(function () {
                                        return $(this).val() == '';
                                    });
    };

    $('[class^=WorkCommunicationType]').hide();
    $('.WorkCommunicationType').show();
    var counter = parseInt($('#WorkCommunicationtypes').val()) || 1;;

    //var max = 5;
    //get info from hidden, if hidden don´t exist. choose default value
    var max = parseInt($('#MultipleEntrySectionsNr').val()) || 4;

    //Hide button if we already show all available
    if (counter >= max && max >= 5) {
        $('#addWorkCommunicationType').hide();
    }

    var elements = $('[class^=WorkCommunicationType]').find(':text, :radio, :checkbox, select');

    var enabled = elements.eq(0).is(':enabled');

    for (var i = 1; i <= counter; i++)
        $('.WorkCommunicationType' + i).show();

    if (emptyElements().length == 0 && (counter < max) && enabled)
        $('#addWorkCommunicationtypesTr').show();
    else
        $('#addWorkCommunicationtypesTr').hide();

    var elements = $('[class^=WorkCommunicationType]').find(':text, :radio, :checkbox, select');

    elements.change(function () {
        if (emptyElements().length == 0 && (counter < max) && enabled)
            $('#addWorkCommunicationtypesTrr').show();
        else
            $('#addWorkCommunicationtypesTrr').hide();
    });

    $('#addWorkCommunicationType').click(function (e) {
        e.preventDefault();
        counter = parseInt($('#WorkCommunicationtypes').val()) || 1;
        counter++;

        if (counter <= max) {
            $('#WorkCommunicationtypes').val(counter);
            $('[class*=WorkCommunicationType' + counter + ']').show();
            $('#addWorkCommunicationtypesTr').hide();

            //Hide button if we already show all available
            if (counter >= max && max >= 5) {
                $('#addWorkCommunicationType').hide();
            }

        }
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
    var counter = parseInt($('#DependantFamilyMembers').val()) || 1;;

    //var max = 5;
    //get info from hidden, if hidden don´t exist. choose default value
    var max = parseInt($('#MultipleEntrySectionsNr').val()) || 5;

    //Hide button if we already show all available
    if (counter >= max && max >= 5) {
        $('#addDependantFamilyMember').hide();
    }

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
        counter = parseInt($('#DependantFamilyMembers').val()) || 1;
        counter++;

        if (counter <= max) {
            $('#DependantFamilyMembers').val(counter);
            $('[class*=dependantFamilyMember' + counter + ']').show();
            $('#addDependantFamilyMembersTr').hide();

            //Hide button if we already show all available
            if (counter >= max && max >= 5) {
                $('#addDependantFamilyMember').hide();
            }

        }
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
    var counter = parseInt($('#DependantsDocuments').val()) || 1;

    //var max = 5;
    //get info from hidden, if hidden don´t exist. choose default value
    var max = parseInt($('#MultipleEntrySectionsNr').val()) || 5;

    //Hide button if we already show all available
    if (counter >= max && max >= 5) {
        $('#addGlobalMobilityDependantDocument').hide();
    }


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
        counter = parseInt($('#DependantsDocuments').val()) || 1;
        counter++;


        if (counter <= max) {
            $('#DependantsDocuments').val(counter);
            $('[class*=globalMobilityDependantDocument' + counter + ']').show();
            $('#addGlobalMobilityDependantDocumentTr').hide();

            //Hide button if we already show all available
            if (counter >= max && max >= 5) {
                $('#addGlobalMobilityDependantDocument').hide();
            }

        }
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

    //var max = 3;
    //get info from hidden, if hidden don´t exist. choose default value
    var max = parseInt($('#MultipleEntrySectionsNr').val()) || 5;

    //Hide button if we already show all available
    if (counter >= max && max >= 5) {
        $('#addAllowance').hide();
    }

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
        if (counter <= max) {
            $('[class=allowance' + counter + ']').show();
            $('#addAllowanceTr').hide();


            //Hide button if we already show all available
            if (counter >= max && max >= 5) {
                $('#addAllowance').hide();
            }

        }
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

    //var max = 5;
    //get info from hidden, if hidden don´t exist. choose default value
    var max = parseInt($('#MultipleEntrySectionsNr').val()) || 5;

    //Hide button if we already show all available
    if (counter >= max && max >= 5) {
        $('#addDeduction').hide();
    }

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

        //Hide button if we already show all available
        if (counter >= max && max >= 5) {
            $('#addDeduction').hide();
        }
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

    var max = 5;

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

var costcentres = function () {

    var emptyElements = function () {
        return $('[class^=costcentres]')
                                    .filter(function () { return $(this).css('display') !== 'none'; })
                                    .find(':text, :radio, :checkbox, select')
                                    .filter(function () {
                                        return $(this).val() == '';
                                    });
    };

    $('[class^=costcentres]').hide();
    $('.costcentres').show();
    var counter = parseInt($('#MultiCostcentres').val());

    //var max = 5;

    //get info from hidden, if hidden don´t exist. choose default value
    var max = parseInt($('#MultipleEntrySectionsNr').val()) || 3;

    //Hide button if we already show all available
    if (counter >= max && max >= 5) {
        $('#addBenefit').hide();
    }


    var elements = $('[class^=costcentres]').find(':text, :radio, :checkbox, select');

    var enabled = elements.eq(0).is(':enabled');

    for (var i = 1; i <= counter; i++)
        $('.costcentres' + i).show();

    if (emptyElements().length == 0 && (counter < max) && enabled)
        $('#addCostcentresTr').show();
    else
        $('#addCostcentresTr').hide();

    var elements = $('[class^=costcentres]').find(':text, :radio, :checkbox, select');

    elements.change(function () {
        if (emptyElements().length == 0 && (counter < max) && enabled)
            $('#addCostcentresTr').show();
        else
            $('#addCostcentresTr').hide();
    });

    $('#addCostCentre').click(function (e) {
        e.preventDefault();
        counter++;

        $('#MultiCostcentres').val(counter);
        if (counter <= max)
            $('[class=costcentres' + counter + ']').show();
        $('#addCostcentresTr').hide();

        //Hide button if we already show all available
        if (counter >= max && max >= 5) {
            $('#addCostCentre').hide();
        }
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

    //var max = 3;

    //get info from hidden, if hidden don´t exist. choose default value
    var max = parseInt($('#MultipleEntrySectionsNr').val()) || 3;

    //Hide button if we already show all available
    if (counter >= max && max >= 5) {
        $('#addBenefit').hide();
    }

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

    //$('#addBenefit').click(function (e) {
    //    e.preventDefault();
    //    counter++;

    //    $('#Benefits').val(counter);
    //    if (counter <= max)
    //        $('[class=benefits' + counter + ']').show();
    //    $('#addBenefitsTr').hide();
    //});

    $('#addBenefit').click(function (e) {
        e.preventDefault();
        counter++;

        $('#Benefits').val(counter);
        if (counter <= max)
            $('[class=benefits' + counter + ']').show();
        $('#addBenefitsTr').hide();

        //Hide button if we already show all available
        if (counter >= max && max >= 5) {
            $('#addBenefit').hide();
        }
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

    //var max = 5;
    //get info from hidden, if hidden don´t exist. choose default value
    var max = parseInt($('#MultipleEntrySectionsNr').val()) || 5;

    //Hide button if we already show all available
    if (counter >= max && max >= 5) {
        $('#addAbsence').hide();
    }

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
        if (counter <= max) {
            $('[class=absence' + counter + ']').show();
            $('#addAbsenceTr').hide();

            //Hide button if we already show all available
            if (counter >= max && max >= 5) {
                $('#addAbsence').hide();
            }
        }
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

    //var max = 3;    
    //get info from hidden, if hidden don´t exist. choose default value
    var max = parseInt($('#MultipleEntrySectionsNr').val()) || 5;

    //Hide button if we already show all available
    if (counter >= max && max >= 5) {
        $('#addDetailsOnGlobalCommuting').hide();
    }

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
        counter = parseInt($('#DetailsOnGlobalCommutings').val()) || 1;
        counter++;

        $('#DetailsOnGlobalCommutings').val(counter);
        if (counter <= max)
            $('[class*=detailsonglobalcommuting' + counter + ']').show();
        $('#addDetailsOnGlobalCommutingTr').hide();

        //Hide button if we already show all available
        if (counter >= max && max >= 5) {
            $('#addDetailsOnGlobalCommuting').hide();
        }
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

    //var max = 5;
    //get info from hidden, if hidden don´t exist. choose default value
    var max = parseInt($('#MultipleEntrySectionsNr').val()) || 5;

    //Hide button if we already show all available
    if (counter >= max && max >= 5) {
        $('#addTerminationPayment').hide();
    }

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

    $('#addTerminationPayment').click(function (e) {
        e.preventDefault();
        counter++;
        $('#TerminationPayments').val(counter);

        if (counter <= max)
            $('[class=terminationpayments' + counter + ']').show();
        $('#addTerminationPaymentsTr').hide();

        //Hide button if we already show all available
        if (counter >= max && max >= 5) {
            $('#addTerminationPayment').hide();
        }
    });
};

var UnreturnedItem = function () {

    var emptyElements = function () {
        return $('[class^=UnreturnedItem]')
                                    .filter(function () { return $(this).css('display') !== 'none'; })
                                    .find(':text, :radio, :checkbox, select')
                                    .filter(function () {
                                        return $(this).val() == '';
                                    });
    };

    $('[class^=UnreturnedItem]').hide();
    $('.UnreturnedItem').show();
    var counter = parseInt($('#UnreturnedItemsMulti').val());

    //var max = 5;
    //get info from hidden, if hidden don´t exist. choose default value
    var max = parseInt($('#MultipleEntrySectionsNr').val()) || 5;

    //Hide button if we already show all available
    if (counter >= max && max >= 5) {
        $('#addUnreturnedItem').hide();
    }

    var elements = $('[class^=UnreturnedItem]').find(':text, :radio, :checkbox, select');

    var enabled = elements.eq(0).is(':enabled');

    for (var i = 1; i <= counter; i++)
        $('.UnreturnedItem' + i).show();

    if (emptyElements().length == 0 && (counter < max) && enabled)
        $('#addUnreturnedItemTr').show();
    else
        $('#addUnreturnedItemTr').hide();

    var elements = $('[class^=UnreturnedItem]').find(':text, :radio, :checkbox, select');

    elements.change(function () {
        if (emptyElements().length == 0 && (counter < max) && enabled)
            $('#addUnreturnedItemTr').show();
        else
            $('#addUnreturnedItemTr').hide();
    });

    $('#addUnreturnedItem').click(function (e) {
        e.preventDefault();
        counter++;
        $('#UnreturnedItemsMulti').val(counter);
        if (counter <= max)
            $('[class=UnreturnedItem' + counter + ']').show();
        $('#addUnreturnedItemTr').hide();

        //Hide button if we already show all available
        if (counter >= max && max >= 5) {
            $('#addUnreturnedItem').hide();
        }
    });
};

var adddocument = function () {

    var emptyElements = function () {
        return $('[class^=DocumentsSection]')
                                    .filter(function () { return $(this).css('display') !== 'none'; })
                                    .find(':text, :radio, :checkbox, select')
                                    .filter(function () {
                                        return $(this).val() == '';
                                    });
    };

    $('[class^=DocumentsSection]').hide();
    $('.DocumentsSection').show();
    var counter = parseInt($('#Documents').val());

    //var max = 2;

    //get info from hidden, if hidden don´t exist. choose default value
    var max = parseInt($('#MultipleEntrySectionsNr').val()) || 5;

    //Hide button if we already show all available
    if (counter >= max && max >= 5) {
        $('#addDocuments').hide();
    }

    var elements = $('[class^=DocumentsSection]').find(':text, :radio, :checkbox, select');

    var enabled = elements.eq(0).is(':enabled');

    for (var i = 1; i <= counter; i++)
        $('.DocumentsSection' + i).show();

    if (emptyElements().length == 0 && (counter < max) && enabled)
        $('#addDocumentTr').show();
    else
        $('#addDocumentTr').hide();

    var elements = $('[class^=DocumentsSection]').find(':text, :radio, :checkbox, select');

    elements.change(function () {
        if (emptyElements().length == 0 && (counter < max) && enabled)
            $('#addDocumentTr').show();
        else
            $('#addDocumentTr').hide();
    });

    $('#addDocuments').click(function (e) {
        e.preventDefault();
        counter++;

        $('#Documents').val(counter);
        if (counter <= max) {
            $('[class=DocumentsSection' + counter + ']').show();
            $('#addDocumentTr').hide();


            //Hide button if we already show all available
            if (counter >= max && max >= 5) {
                $('#addDocuments').hide();
            }

        }
    });
};

var bankaccount = function () {

    var emptyElements = function () {
        return $('[class^=bankaccount]')
                                    .filter(function () { return $(this).css('display') !== 'none'; })
                                    .find(':text, :radio, :checkbox, select')
                                    .filter(function () {
                                        return $(this).val() == '';
                                    });
    };

    $('[class^=bankaccount]').hide();
    $('.bankaccount').show();

    var counter = parseInt($('#BankAccounts').val());

    var max = 2;

    //Hide button if we already show all available
    if (counter >= max) {
        $('#addBank').hide();
    }

    var elements = $('[class^=bankaccount]').find(':text, :radio, :checkbox, select');

    var enabled = elements.eq(0).is(':enabled');

    for (var i = 1; i <= counter; i++)
        $('.bankaccount' + i).show();

    //if (emptyElements().length == 0 && (counter < max) && enabled)
    //    $('#addBankAccountTr').show();
    //else
    //    $('#addBankAccountTr').hide();

    var elements = $('[class^=bankaccount]').find(':text, :radio, :checkbox, select');

    //elements.change(function () {
    //    if (emptyElements().length == 0 && (counter < max) && enabled)
    //        $('#addEducationTr').show();
    //    else
    //        $('#addEducationTr').hide();
    //});

    $('#addBank').click(function (e) {
        e.preventDefault();
        counter++;

        $('#BankAccounts').val(counter);
        if (counter <= max) {
            $('[class=bankaccount' + counter + ']').show();

            //Hide button if we already show all available
            if (counter >= max) {
                $('#addBank').hide();
            }
        }

        //
        SetPayeeDefault();
        //$('#addEducationTr').hide();
    });
};

var cache = {};

//Global spinner, spin.js
var globalSpinner = new Indicator();

function Indicator() {
    var target;
    var spinner;

    this.initialize = function () {
        var opts = {
            lines: 11, // The number of lines to draw
            length: 18, // The length of each line
            width: 6, // The line thickness
            radius: 25, // The radius of the inner circle
            corners: 1, // Corner roundness (0..1)
            rotate: 0, // The rotation offset
            direction: 1, // 1: clockwise, -1: counterclockwise
            color: '#000', // #rgb or #rrggbb or array of colors
            speed: 1, // Rounds per second
            trail: 47, // Afterglow percentage
            shadow: false, // Whether to render a shadow
            hwaccel: false, // Whether to use hardware acceleration
            className: 'spinner', // The CSS class to assign to the spinner
            zIndex: 2000000000, // The z-index (defaults to 2000000000)
            top: '50%', // Top position relative to parent
            left: '50%' // Left position relative to parent
        };
        target = document.getElementById('spinnerDiv');
        spinner = new Spinner(opts);
    };
    this.start = function () {
        if (!spinner) {
            this.initialize();
        }
        spinner.stop();
        spinner.spin(target);
    };
    this.stop = function () {
        if (spinner) {
            spinner.stop();
        }
    };
}

$(document).ajaxStart(function () {
    globalSpinner.start();
});

$(document).ajaxComplete(function () {
    globalSpinner.stop();
});

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

var tabs = function (e) {

    var activeTab = $("#activeTab");

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
};

var setActiveTab = function () {

    $('.nav-tabs a').on('click', tabs);

    var activeTab = $("#activeTab");

    if (activeTab.length > 0)
        $('.nav-tabs a[href="' + activeTab.val() + '"]').click();
};

var initAutoComplete = function () 
{
    var ect = new Ect();
    ect.getAutoComplete('Country');
    ect.getAutoComplete('Nationality');
    ect.getAutoComplete('LineManager');
    ect.getAutoComplete('PositionTitle');
    ect.getAutoComplete('PrimarySite');
    ect.getAutoComplete('CostCentre');
    ect.getAutoComplete('JobTitle');
    ect.getAutoComplete('Language');
    ect.getAutoComplete('TecApprover');
    ect.getAutoComplete('LineManagerJobTitle');
}

var init = function () {
    // Initiate globalspinner for ajax requests
    globalSpinner.stop();

    initAutoComplete();

    // Prevent submit form on enter
    $(window).keydown(function (event) {
        // Make it Possible to USE enter in Internal lognote
        if (event.target.tagName != 'TEXTAREA') {
            if (event.keyCode == 13) {
                event.preventDefault();
                return false;
            }
        }
    });

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
    UnreturnedItem();
    adddocument();
    bankaccount();
    costcentres();
    PermanentCommunicationTypes();
    EmergencyCommunicationTypes();
    HomeCommunicationTypes();
    ENHomeCommunicationTypes();
    WorkCommunicationTypes();

    InitIntegration();

    //Sort by text
    $('.search-select').selectize({sortField: [{ field: 'text' }]});
  
    $('.typeahead').typeahead(typeAheadOptions);
    $('#Co-WorkerGlobalviewID').typeahead(globalTypeAheadOptions);
    $('#IKEAEmailAddress').typeahead(globalEmailTypeAheadOptions);
    $('#FirstName').typeahead(globalNameTypeAheadOptions);
    $('#LastName').typeahead(globalLastNameTypeAheadOptions);
    $('#IKEANetworkID').typeahead(globalNetworkIdTypeAheadOptions);

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

    if ($("#NewToIKEA").length > 0 && ($("#NewToIKEA").val() != 'Re-Hire' || $("#NewToIKEA").val() != 'Transfer Between Business Units')) {
        disableTypeahead = 0;
    }

    $("#NewToIKEA").on('change', function () {

        if ($('#formGuid').length > 0) {
            $('#Co-WorkerID').typeahead(globalTypeAheadOptions);
            if ($(this).val() == 'Re-Hire' || $(this).val() == 'Transfer Between Business Units') {
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

    setActiveTab();

    // DD 2015-01-08: Don't know what this is for?
    $('a.navigate').click(function (e) {
        e.preventDefault();
        $('.nav-tabs a[href="' + $(this).attr('href') + '"]').click();
        return false;
    });

    initGV();

    // datepicker
    $(document).on("click", ".date", function () {
        $(this).not(".disabled").datepicker('show');
    });

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

                if (document.getElementById('LockCDSAccountFrom')) {
                    document.getElementById('LockCDSAccountFrom').value = document.getElementById('LastDayOfEmployment').value;
                }
                if (document.getElementById('LockCDSAccountFrom')) {
                    document.getElementById('LastDayWorked').value = document.getElementById('LastDayOfEmployment').value;
                }
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

                            if (document.getElementById(PaymentDate)) {
                                document.getElementById(PaymentDate).value = document.getElementById('LastDayOfEmployment').value;
                            }

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

            if (!json.predefined.dependent)
                return;

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

    validate.run($('#actionState').val());
};

// plupload
var uploader;
var initUpload = function () {

    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,html4',
        browse_button: 'pickfiles',
        container: 'container',
        max_file_size: '5mb',
        url: site.baseUrl + '/files/upload',
        flash_swf_url: site.baseUrl + '/FormLibContent/Assets/plupload/plupload.flash.swf'
        //,silverlight_xap_url: site.baseUrl + '/FormLibContent/assets/plupload/plupload.silverlight.xap'
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

    uploader.bind('UploadProgress', function (up, file) { $('#' + file.id + " b").html(file.percent + "%"); });

    uploader.bind('Error', function (up, err) {

        switch (err.code) {
            case plupload.FILE_EXTENSION_ERROR:
                break;

            case plupload.FILE_SIZE_ERROR:
                break;

            case plupload.FILE_DUPLICATE_ERROR:
                break;

            case self.FILE_COUNT_ERROR:
                break;

            case plupload.IMAGE_FORMAT_ERROR:
                break;

            case plupload.IMAGE_MEMORY_ERROR:
                break;

            case plupload.HTTP_ERROR:
                break;
        }

        $('#filelist').prepend("<div>Error: " + err.code + ", Message: " + err.message + (err.file ? ", File: " + err.file.name : "") + "</div>");

        up.refresh(); // Reposition Flash/Silverlight
    });

    uploader.bind('FileUploaded', function (up, file) { $('#' + file.id + " b").html("100%"); });
};

var actionStateChanged = function () {
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
            if ($('#pickfiles').length > 0) {
                //this is in integration.js instead -- 52554
                //initUpload();
            }
            if (data.Uploads != null) {
                if (data.Uploads.length > 0) {
                    $.each(data.Uploads, function () {
                        var uploadItem = '<div>' + this + '<b></b> <a href=""'
                        + 'class="removeTempFile btn btn-link btn-mini"><i class="icon-trash"></i></a>'
                        + '<input type="hidden" name="uploads" value="' + this + '"></div>';
                        $('#filelist').append(uploadItem);
                    });

                }
            }
        } else {
            reload(data.CancelCase);
        }
        globalSpinner.stop();
    });
};

$(document).on('submit', 'form', function (e) {

    var elem = $('#navigation').find(':button, select').addClass("disabled").attr('readonly', 'readonly').attr('disabled', 'disabled');

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
    //SG To avoid disable both buttons while actionState is not chosen Roll Out #53866
    if ($('#actionState').val() != '') {
        $('#actionStateChange').attr("disabled", true);

        // SG to disable save button while form is loading HD case : 52790
        $('#btnGlobalSave').attr("disabled", true);
    }

    var form = $('form');
    var url = form.attr('action');

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

$(document).on('click', '.changeprint', function (e) {
    e.preventDefault();

    var $this = $(this);
    var url = $this.attr('href');

    $.ajax({
        type: "POST",
        url: url,
        data: { query: $('#changeform').serialize() }
    })
        .done(function (data) {
            if (data.Exception)
                location.href = location.href;
            else {
                url = url.replace('ChangePrint', 'contract');
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
    //if ($('#pickfiles').length > 0)
    //this is in integration.js instead - 52554
    //initUpload();

    $(document).on('click', '.removeTempFile', function (e) {
        e.preventDefault();
        removeTempFile(e);
    });
});

function removeTempFile(e) {
    $(e.target).parent().parent().remove();
};

function GetXMLandCallback(country, xmlFile, Callback, $BaseSelector, $DefaultSelector, MultiId, lockedbyUserGroup, isTabReadOnly) {
    var path = window.location.protocol + '//';
    path = path + window.location.host + '/';
    path = site.baseUrl + '/FormLibContent/Xmls/' + country + '/' + xmlFile;

    $.ajax({
        type: "GET",
        url: path,
        dataType: "xml",
        success: function (xml) {
            Callback(xml, $BaseSelector, $DefaultSelector, MultiId, lockedbyUserGroup, isTabReadOnly);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(jqXHR.responseText);
            alert(textStatus);
            alert(errorThrown);
        }
    });
};

function SetDefault(xml, $BaseSelector, $DefaultSelector) {
    $(xml).find('defaults').find('default').each(function () {
        var xmlBaseVal = $(this).find('baseVal').text();
        var BaseVal = $BaseSelector.val();
        if (xmlBaseVal.toLowerCase() == BaseVal.toLowerCase()) {
            var defaultVal = $(this).find('defaultVal').text();
            $DefaultSelector.val(defaultVal);
            return;
        }
    });
};

function SetHideAndClear(xml, $BaseSelector, $DefaultSelector, MultiId) {
    if (typeof MultiId === undefined) {
        MultiId = "";
    }

    $(xml).find('hides').find('hide').each(function () {
        $(this).find('hideSelector').each(function () {
            thisXMLNode = $(this);
            var xmlHideSelectorVal = thisXMLNode.text();
            var HideSelector = "";
            if (thisXMLNode.attr('type').toLowerCase() == 'id') {
                HideSelector = $('#' + xmlHideSelectorVal + MultiId);
                HideSelector.parent().parent().show();
            }
            else if (thisXMLNode.attr('type').toLowerCase() == 'class') {
                HideSelector = $('.' + xmlHideSelectorVal + MultiId);
                HideSelector.show();
            }
        });
    });
    $(xml).find('hides').find('hide').each(function () {
        var xmlHideVal = $(this).find('hideVal').text();
        var BaseVal = "";
        if (IsElementSourceDB($BaseSelector)) {
            if (IsSelectorSelectize($BaseSelector)) {
                BaseVal = $($BaseSelector[0].selectize.getOption($BaseSelector[0].selectize.getValue())).text()
            }
            else {
                BaseVal = $BaseSelector.children(':selected').val();
            }
        }
        else {
            BaseVal = $BaseSelector.children(':selected').val();
        }
        if (BaseVal == "" || BaseVal == null) {
            BaseVal = $BaseSelector.val();
            if (BaseVal == "" || BaseVal == null) {
                BaseVal = "";
            }
        }
        if (xmlHideVal.toLowerCase() == BaseVal.toLowerCase()) {
            $(this).find('hideSelector').each(function () {
                var thisXMLNode = $(this);
                var xmlHideSelectorVal = thisXMLNode.text();
                var HideSelector = "";
                if (thisXMLNode.attr('type').toLowerCase() == 'id') {
                    HideSelector = $('#' + xmlHideSelectorVal + MultiId);
                    HideSelector.parent().parent().hide(); //if id, we need to hide the TR
                }
                else if (thisXMLNode.attr('type').toLowerCase() == 'class') {
                    HideSelector = $('.' + xmlHideSelectorVal + MultiId);
                    HideSelector.hide(); //if class, hide class
                }
            });
        }
    });
};


//S.G : Added  lockedbyUserGroup, isTabReadOnly becasuse it sets the disable to false while it is read only which is not correct.
//This "Set Lock" Not such a good solution for date fields.
//It is not working for date fields it just disables the text field for dates and not a date picker.
//only tested for normal select and text... Currently no support for selectize/search-select
function SetLock(xml, $BaseSelector, $LockSelector, lockedbyUserGroup, isTabReadOnly) {

    var LockField = false;
    $(xml).find('locks').find('lock').each(function () {
        var xmlBaseVal = $(this).find('lockVal').text();
        var BaseVal = $BaseSelector.val();
        if (xmlBaseVal.toLowerCase() == BaseVal.toLowerCase()) {
            LockField = true;
        }
    });

    if (LockField) {
        // To make the value empty if the field will be disable
        $LockSelector.val("");
        $LockSelector.prop('disabled', true);
    }
    else {
          // If the Tabs are read only it shouldn't happen any thing.
        if (!lockedbyUserGroup && !isTabReadOnly) {
            return;
        }
        else
            $LockSelector.prop('disabled', false);
    }
};

var initOrgDataSection = function ($companySelector, $buSelector, $fuSelector, $depSelector, $costCentreSelector) {
    var ajaxInfo = $('#ajaxInfo');
    if (ajaxInfo.length > 0) {
        var baseurl = ajaxInfo.attr('url');
        var customerId = ajaxInfo.attr('customerId');
        var LoadCompany = function ($selector) {
            var url = baseurl + 'GetAllCompanies' + '?customerId=' + customerId;
            LoadOrgData(url, $selector);
        };
        var LoadBU = function (companyId, $selector) {
            if (companyId == "" || companyId == null) {
                companyId = 0;
            }
            var url = baseurl + 'GetAllBusinessUnits' + '?customerId=' + customerId + '&companyId=' + companyId;
            LoadOrgData(url, $selector);
        };
        var LoadFunction = function (buId, $selector) {
            if (buId == "" || buId == null) {
                buId = 0;
            }
            var url = baseurl + 'GetAllFunctions' + '?businessUnitId=' + buId;
            LoadOrgData(url, $selector);
        };
        var LoadDepartments = function (functionId, $selector) {
            if (functionId == "" || functionId == null) {
                functionId = 0;
            }
            var url = baseurl + 'GetAllDepartments' + '?functionId=' + functionId;
            LoadOrgData(url, $selector);
        };
        var LoadCostCentres = function (departmentId, $selector) {
            if (departmentId == "" || departmentId == null) {
                departmentId = 0;
            }
            var url = baseurl + 'GetCostCentresByDepartment' + '?departmentId=' + departmentId;
            LoadOrgData(url, $selector);
        };
        LoadCompany($companySelector);
        $companySelector.change(function () {
            LoadBU($(this).val(), $buSelector)
        });
        $buSelector.change(function () {
            LoadFunction($(this).val(), $fuSelector);
        });
        $fuSelector.change(function () {
            LoadDepartments($(this).val(), $depSelector);
        });
        $depSelector.change(function () {
            LoadCostCentres($(this).val(), $costCentreSelector);
        });
    }
};

function LoadOrgData(url, $selector) {
    var option = function (value, text) {
        var optionRow = '<option value="' + value + '">' + text + '</option>';
        return optionRow;
    };
    var AddSelectizeOptions = function (data, CurrentVal) {
        var selectizeSelector = $selector[0].selectize;
        selectizeSelector.clearOptions();
        var items = data.map(function (x) { return { text: x.Name, value: x.Id }; });
        selectizeSelector.addOption(items);
        selectizeSelector.setValue(CurrentVal);
    };
    var AddSelectOptions = function (data, CurrentVal) { 
        $selector.empty()
        var options = "";
        options += option('', '');
        $.each(data, function () {
            options += option(this.Id, this.Name)
        });
        $selector.append(options);
        $selector.val(CurrentVal);
        $selector.change();
    };
    var ClearAndReset = function (CurrentVal) {
        if (IsSelectorSelectize($selector)) {
            var selectizeSelector = $selector[0].selectize;
            selectizeSelector.clearOptions();
            selectizeSelector.setValue(CurrentVal);
            $selector.change();
        }
        else {
            $selector.empty();
            $selector.val(CurrentVal);
            $selector.change();
        }
    };
    var thisUrl = url + '&ie=' + Date.now();
    $.getJSON(thisUrl, function (data) {
        
        var CurrentVal = "";
        if (IsElementSourceDB($selector)) {
            CurrentVal = $selector.parent().find('input[type=hidden]').val(); //printed by database="source"
        }
        else {
            CurrentVal = $selector.val();
        }

        if (data) {
            if (IsSelectorSelectize($selector)) {
                AddSelectizeOptions(data, CurrentVal);
            }
            else {
                AddSelectOptions(data, CurrentVal);
            }
        }
        else if (data == false) {
            ClearAndReset(CurrentVal);
        }
    });
};

//put this in some type of function called selectorHelper? does not need to be globally declared?
function IsSelectorSelectize($selector) {
    if ($selector.hasClass('search-select')) {
        return true;
    }
    else {
        return false;
    }
};

//put this in some type of function called selectorHelper? does not need to be globally declared?
function IsElementSourceDB($selector) {
    if ($selector.parent().find('input[type=hidden]')) {
        return true;
    }
    else {
        return false;
    }
};

//this can maybe be part of a datehelper class?
function getChosenDate($dateSelector) {
    var DateMonthYear = $dateSelector.val().split(".");
    var chosenDate = new Date(DateMonthYear[2], DateMonthYear[1] - 1, DateMonthYear[0]);
    return chosenDate;
};

///only works for date at the moment
//put this in some type of function called selectorHelper? does not need to be globally declared
function selectorNotice($selector, show) {
    var noticeSelector = "";
    if ($selector.parent().hasClass('date')) {
        noticeSelector = $selector.parent().parent().find('[id^=notice_]');
    }

    if (show == true) {
        noticeSelector.show();
    }
    else {
        noticeSelector.hide();
    }
};

//put this in a bigger function called datehelper? does not need to be globally declared?
function validateFirstDayOfMonth($dateSelector) {
    var chosenDate = getChosenDate($dateSelector);
    var thisDate = chosenDate.getDate();
    if (!(thisDate == 1)) {
        return true;
    }
    else {
        return false;
    }
};


$(document).ready(function () {

    //uppercase on input
    $('.upper-case').bind('keyup', function () {
        var val = $(this).val().toUpperCase();
        $(this).val(val);
    });

});


function Ect() {
    core2.baseUrl = $("#core_baseUrl").val();
    core2.area = $("#core_area").val();
    core2.controller = $("#core_controller").val();
    core2.formGuid = $("#formGuid").val();
};

Ect.prototype.getRelations = function (xmlFile, xmlFileAllOptions, $baseSelector, $targetSelector) {

    var path = window.location.protocol + '//';
    path = path + window.location.host + '/';
    path = core2.baseUrl + '/FormLibContent/Xmls/' + core2.area + "/Relations/" + xmlFile;

    var self = this;

    $.ajax({
        type: "GET",
        url: path,
        dataType: "xml",
        success: function (xml) {

            var show = '';

            var baseIsSelectize = $baseSelector[0].selectize; //$baseSelector.hasClass('selectize');
            var targetIsSelectize = $targetSelector[0].selectize; // $targetSelector.hasClass('selectize');

            

            
            var base = '';
            if (baseIsSelectize) {
                base = $($baseSelector[0].selectize.getOption($baseSelector[0].selectize.getValue())).text().toLowerCase();
            }
            else {
                base = $baseSelector.find('option:selected').text().toLowerCase();
            }

            var targetValue;
            var selectize_tags;

            targetValue = $targetSelector.val();
            //clear list
            $targetSelector.val('');
            //console.log('clear' + $targetSelector.attr('id'));

            //Clear list
            if (targetIsSelectize) {
                selectize_tags = $targetSelector[0].selectize
                selectize_tags.clearOptions();
            }
            else {
                $targetSelector.find('option').remove();
            }
            var foundMatch = false;

            $(xml).find('dependent').each(function () {
                var $sel = $(this);
                show = '';

                var baseCompare = $sel.find('selected').text().toLowerCase();
                show = $sel.find('show').text();
                
                if (baseCompare == base) {
                    if (show != '') {
                        
                        foundMatch = true;

                        //temp, replace spaces that occours in the beginning of comma separation.
                        show = show.replace(/\, /g, ",");

                        var optionsarray = show.split(',');
                        optionsarray.unshift('');

                        $.each(optionsarray, function (key, val) {
                            // search for value and replace it
                            // optionsarray[key] = val.replace('[COMMA]', '&#44;');
                            optionsarray[key] = val.replace('[COMMA]', ',');
                        })

                        var items = optionsarray.map(function (x) { return { text: x, value: x }; });

                        if (targetIsSelectize) {
                            selectize_tags.addOption(items);
                            if (targetValue != '') {
                                selectize_tags.setValue(targetValue);
                            }
                            else
                                selectize_tags.setValue('');
                            return;
                        }
                        else {

                            $.each(items, function (key, item) {
                                $targetSelector
                                    .append($("<option></option>")
                                    .attr("value", item.value)
                                    .text(item.text));
                            });

                            //set value
                            try {
                                var attrId = $targetSelector.attr('id');
                                var selectedValue = $('#hidden_' + attrId);

                                //First time
                                if (selectedValue.val() != '') {
                                    $targetSelector.val(selectedValue.val());
                                    selectedValue.val('');
                                }

                            } catch (e) {
                            }

                            return;
                        }
                    }

                }
            });

            //Get all options if there is no options in select/search-select
            if (xmlFileAllOptions != '' && foundMatch == false) {

                console.log('no match, fetch all');
                
                if (targetIsSelectize) {
                    var options = $targetSelector[0].selectize.options;
                    var optionsLength = Object.keys(options).length;
                    if (optionsLength == 0) {
                        self.getAll(xmlFileAllOptions, $targetSelector, targetValue);
                    }
                }
                else {
                    var optionsLength = $targetSelector.find('option').length;
                    if (optionsLength == 0) {
                        self.getAll(xmlFileAllOptions, $targetSelector, targetValue);
                    }
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(jqXHR.responseText);
            alert(textStatus);
            alert(errorThrown);
        }
    });
}

Ect.prototype.getAll = function (xmlFile, $targetSelector, targetValue) {
    
    var path = window.location.protocol + '//';
    path = path + window.location.host + '/';
    path = core2.baseUrl + '/FormLibContent/Xmls/' + core2.area + '/Data/' + xmlFile;
    $.ajax({
        type: "GET",
        url: path,
        dataType: "xml",
        success: function (xml) {

            var targetIsSelectize = $targetSelector.hasClass('selectize');
            var optionsArray = [''];

            //var items = data.map(function (x) { return { text: x.Name, value: x.Id }; });
            $(xml).find('option').each(function () {
                var a = $(this).text();
                if (!(a == 'undefined' || a == '')) {
                    optionsArray.push($(this).text());
                }
            });
            var items = optionsArray.map(function (x) { return { text: x, value: x }; });
            
            if (targetIsSelectize) {
                var selectize_tags = $targetSelector[0].selectize;
                selectize_tags.addOption(items);

                if (targetValue != '') {
                    selectize_tags.setValue(targetValue);
                }
                else
                    selectize_tags.setValue('');
                return;
            }
            else {
                //ADD OPTIONS TO SELECT
                $.each(items, function (key, item) {
                    $targetSelector
                        .append($("<option></option>")
                        .attr("value", item.value)
                        .text(item.text));
                });

                //set value
                try {
                    var attrId = $targetSelector.attr('id');
                    var selectedValue = $('#hidden_' + attrId);

                    //First time
                    if (selectedValue.val() != '') {
                        $targetSelector.val(selectedValue.val());
                        selectedValue.val('');
                    }

                } catch (e) {
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(jqXHR.responseText);
            alert(textStatus);
            alert(errorThrown);
        }
    });

}

Ect.prototype.getAutoComplete = function (entityName) {
    
    entityName = entityName.toLowerCase();

    if ($('.autocomplete-' + entityName)[0]) {
    
        var selectize_options_primarysite = {
            sortField: [{ field: 'value', direction: 'asc' }, { field: '$score' }],
            persist: false,
            valueField: 'value',
            labelField: 'text',
            searchField: 'text',
            create: false,
            preload: false,
            render: {
                option: function (item, escape) {

                    return '<div data-value="' + escape(item.value) + '" data-selectable="" class="option">' + escape(item.text) + '</div>';
                }
            },
            load: function (query, callback) {
                    if (!query.length && query.length > 1) return callback();

                    var url = core2.baseUrl + "/" + core2.area + "/" + core2.controller;

                    $.ajax({
                        url: url + '/AutoComplete?entityName=' + entityName + '&query=&formGuid=' + encodeURIComponent(core2.formGuid) + '&=' + Math.random(),
                        type: 'GET',
                        error: function () {
                            callback();
                        },
                        success: function (res) {
                            callback(res);
                        }
                    });
            }
        };

        $('.autocomplete-' + entityName)
            .each(function (e, i) {
                var _this = $(this);
                if (!_this[0].selectize) {
                    _this.selectize(selectize_options_primarysite);
                }
            });
        var url = core2.baseUrl + "/" + core2.area + "/" + core2.controller;
        $.ajax({
            url: url + '/AutoComplete?entityName=' + entityName + '&query=&formGuid=' + encodeURIComponent(core2.formGuid) + '&=' + Math.random(),
            type: 'GET',
            error: function () {
            },
            success: function (res) {
                $('.autocomplete-' + entityName)
                    .each(function (e, i) {
                        if (i.selectize) {
                            i.selectize.addOption(res);
                        }
                    });
            }
        });
    }
   
    if ($('.autocomplete-relation-' + entityName)[0]) {
    
        var selectize_options_primarysite = {
            sortField: [{ field: 'value', direction: 'asc' }, { field: '$score' }],
            persist: false,
            valueField: 'value',
            labelField: 'text',
            searchField: 'text',
            create: false,
            preload: false,
            render: {
                option: function (item, escape) {
                    return '<div data-value="' + escape(item.value) + '" data-selectable="" class="option">' + escape(item.text) + '</div>';
                }
            }
        };

        $('.autocomplete-relation-' + entityName)
            .each(function (e, i) {
                var _this = $(this);
                if (!_this[0].selectize) {
                    _this.selectize(selectize_options_primarysite);
                }
            });

    }
}

Ect.prototype.copyValueFrom = function ($baseSelector, $targetSelector) {

    //check that they exist
    if ($baseSelector[0] && $targetSelector[0]) {

        //Set same value in target as selected in base
        var value = $baseSelector.val();

        var selectedText = '';

        if ($baseSelector[0].selectize) {
            selectedText = $baseSelector[0].selectize.getItem(value).text();
        }
        else if ($baseSelector.is('select')) {
            selectedText = $baseSelector.find('option:selected').text();
        }
        else {
            //Assume that is a text for now
            selectedText = $baseSelector.val();
        }

        selectedText = $.trim(selectedText);

        if ($baseSelector[0].selectize) {


            if ($targetSelector[0].selectize) {
                $targetSelector[0].selectize.setValue(selectedText);
            }
            else {
                $targetSelector.val(selectedText);
            }
        } else {

            if ($targetSelector[0].selectize) {
                $targetSelector[0].selectize.setValue(selectedText);
            }
            else {
                $targetSelector.val(selectedText);
            }
        }

    }

}


Ect.prototype.setValue = function (value, $targetSelector) {
    
    //check that they exist
    if ($targetSelector[0]) {

            if ($targetSelector[0].selectize) {
                var valueExist = $targetSelector[0].selectize.getItem(value).text();

                //Add value if it doesent exist                
                if (valueExist == '')
                    {
                    $targetSelector[0].selectize.addOption({
                        text: value,
                        value: value
                    });
                }

                $targetSelector[0].selectize.setValue(value);
            }
            else {
                $targetSelector.val(value);
            }
    }
}
