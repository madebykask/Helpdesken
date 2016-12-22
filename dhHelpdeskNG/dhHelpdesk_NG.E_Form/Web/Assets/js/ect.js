
// Globals
var isEmbed = window != window.parent;
var site = site || {};
site.baseUrl = "";

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
    location.reload();
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
        if (iState !== undefined && iState != '') {
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
        return $.ajax({
            url: site.baseUrl + '/search/globalviewforcustomer',
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
        
        $('input[name="BusinessUnit"]').val(item.unit);
        $('input[name="Department"]').val(item.company);
        $('input[name="FirstName"]').val(item.firstname);
        $('input[name="LastName"]').val(item.lastname);
        $('input[name="IKEAEmailAddress"]').val(item.email);
        $('input[name="IKEANetworkID"]').val(item.num);


        return item.num;
    }
};


var globalEmailTypeAheadOptions = {
    items: 10,
    minLength: 3,
    source: function (query, process) {
        return $.ajax({
            url: site.baseUrl + '/search/globalviewforcustomer',
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

        $('input[name="Co-WorkerGlobalviewID"]').val(item.num);
        $('input[name="BusinessUnit"]').val(item.unit);
        $('input[name="Department"]').val(item.company);
        $('input[name="FirstName"]').val(item.firstname);
        $('input[name="LastName"]').val(item.lastname);
        $('input[name="IKEAEmailAddress"]').val(item.email);
        $('input[name="IKEANetworkID"]').val(item.num);

        return item.email;
    }
};


var globalNameTypeAheadOptions = {
    items: 10,
    minLength: 3,
    source: function (query, process) {
        return $.ajax({
            url: site.baseUrl + '/search/globalviewforcustomer',
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

        $('input[name="Co-WorkerGlobalviewID"]').val(item.num);
        $('input[name="BusinessUnit"]').val(item.unit);
        $('input[name="Department"]').val(item.company);
        $('input[name="FirstName"]').val(item.firstname);
        $('input[name="LastName"]').val(item.lastname);
        $('input[name="IKEAEmailAddress"]').val(item.email);
        $('input[name="IKEANetworkID"]').val(item.num);


        return item.firstname;
    }
};


var globalLastNameTypeAheadOptions = {
    items: 10,
    minLength: 3,
    source: function (query, process) {
        return $.ajax({
            url: site.baseUrl + '/search/globalviewforcustomer',
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

        $('input[name="Co-WorkerGlobalviewID"]').val(item.num);
        $('input[name="BusinessUnit"]').val(item.unit);
        $('input[name="Department"]').val(item.company);
        $('input[name="FirstName"]').val(item.firstname);
        $('input[name="LastName"]').val(item.lastname);
        $('input[name="IKEAEmailAddress"]').val(item.email);
        $('input[name="IKEANetworkID"]').val(item.num);


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
            data: { query: query, searchKey: $('#Unit').val() },
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
        console.log(counter);
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
        console.log(counter);
        $('#Educations').val(counter);
        if (counter <= max)
            $('[class=education' + counter + ']').show();
        $('#addEducationTr').hide();
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
        console.log(counter);
        $('#OtherPreviousEmployers').val(counter);
        if (counter <= max)
            $('[class=otherpreviousemployers' + counter + ']').show();
        $('#addOtherPreviousEmployersTr').hide();
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
        console.log(counter);
        $('#TerminationPaymentsTr').val(counter);
        if (counter <= max)
            $('[class=terminationpayments' + counter + ']').show();
        $('#addTerminationPaymentsTr').hide();
    });
};


var absence = function () {

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
    //var counter = parseInt($('#Absences').val());
    var counter = 1;

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
        console.log(counter);
        $('#Absences').val(counter);
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
        console.log(counter);
        $('#DetailsOnGlobalCommutings').val(counter);
        if (counter <= max)
            $('[class=detailsonglobalcommuting' + counter + ']').show();
        $('#addDetailsOnGlobalCommutingTr').hide();
    });
};



var init = function () {

    narrowDownInit();
    familyMembers();
    allowances();
    deductions();
    education();
    otherpreviousemployers();
    terminationpayments();
    absence();
    multi();
    detailsonglobalcommuting();

    var activeTab = $("#activeTab");

    $('.typeahead').typeahead(typeAheadOptions);
    $('#Co-WorkerGlobalviewID').typeahead(globalTypeAheadOptions);
    $('#IKEAEmailAddress').typeahead(globalEmailTypeAheadOptions);
    $('#FirstName').typeahead(globalNameTypeAheadOptions);
    $('#LastName').typeahead(globalLastNameTypeAheadOptions);



    $('.nav-tabs a').click(function (e) {
        e.preventDefault();
        $(this).tab('show');
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
    var copy = $('.copy');
    if (copy.length > 0) {
        copy.on('click', function (e) {
            e.preventDefault();
            $(this).closest('tr').find('.copy-me').selectText();
        });
    }

    // datepicker

    $(document).on("click", ".date", function () {
        $(this).not(".disabled").datepicker('show');
    });

    // Specific to Netherlands for hiringdate
    // Only enables 1st and 15th day
    $('#date_ContractStartDate').datepicker(
    {
        onRender: function (ev) {
            if (ev.getDate() === 1 || ev.getDate() === 15)
                return '';
            return 'disabled';
        }
    });

    // Specific to Netherlands, enables only dates after ContractStartDate
    var nowTemp = new Date();
    var now = new Date(nowTemp.getFullYear(), nowTemp.getMonth(), nowTemp.getDate(), 0, 0, 0, 0);
    var contractstartdate = new Date(document.getElementById('ContractStartDate').value);
    $('#date_AllowancesValidFrom').datepicker({
        onRender: function (date) {
            return date.valueOf() < contractstartdate.valueOf() ? 'disabled' : '';
        }
    });


    // Built for Global, sets LockCDSAccountFrom date to same date as last day of employment
    var LastDayOfEmployment = $('#date_LastDayOfEmployment');   
    LastDayOfEmployment.datepicker()
            .on('changeDate', function (e) {
                document.getElementById('LockCDSAccountFrom').value = document.getElementById('LastDayOfEmployment').value;


                //this rows is specific to Complete Termination Details
                document.getElementById('PaymentDate').value = document.getElementById('LastDayOfEmployment').value;
                document.getElementById('DeductionDate').value = document.getElementById('LastDayOfEmployment').value;

                for (var i = 2; i <= 3; i++) {
                    var PaymentDate = "PaymentDate" + [i].toString();
                    var DeductionDate = "DeductionDate" + [i].toString();

                    document.getElementById(PaymentDate).value = document.getElementById('LastDayOfEmployment').value;
                    document.getElementById(DeductionDate).value = document.getElementById('LastDayOfEmployment').value;
                }
            });



    //Specific to Netherlands. Shows a notice about hiring date being in the past.
    var noticeHiringDate = $('#notice_HiringDate');
    var dateHiringDate = $('#date_HiringDate, #date_ContractStartDate');
    if (noticeHiringDate.length > 0 && dateHiringDate.length > 0) {
        var startDate = new Date();
        startDate.setHours(0, 0, 0, 0);

        dateHiringDate.datepicker()
            .on('changeDate', function (e) {
                if (e.date.valueOf() < startDate.valueOf()) {
                    noticeHiringDate.show();
                } else {
                    noticeHiringDate.hide();
                }
            });
    }


    var noticeFutureDate = $('#notice_ValidUntilDate');
    var dateDatePicker = $('#date_DocumentValidUntil');
    if (noticeFutureDate.length > 0 && dateHiringDate.length > 0) {
        var startDate = new Date();
        startDate.setHours(0, 0, 0, 0);

        dateDatePicker.datepicker()
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
    $('#date_DateOfBirth, #date_FamilyMemberDateOfBirth, #date_DependantsDateOfBirth').datepicker(
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

        dateEffectiveDate.datepicker()
            .on('changeDate', function (e) {
                if (e.date.valueOf() < startDate.valueOf()) {
                    noticeEffectiveDate.show();
                } else {
                    noticeEffectiveDate.hide();
                }
            });


    }

    // predefined functions 

    var cache = {};
    $('select').each(function (i) {
        cache[$(this).attr('id')] = $(this).html();
    });

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

        });

    });

    predefined.each(function () {
        $(this).change();
    });

    validate.run($('#actionState').val());

};

init();

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
                initUpload();
            }
        } else {
            reload(data.CancelCase);
        }
    });
};

$(document).on('submit', 'form', function (e) {

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
                location.reload();
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
        });
});

if ($('#pickfiles').length > 0)
    initUpload();