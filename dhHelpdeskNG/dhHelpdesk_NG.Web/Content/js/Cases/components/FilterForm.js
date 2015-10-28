'use strict';

function FilterForm() {
};

/// initial state of search form
FilterForm.prototype.init = function (opt) {
    var me = this;
    me.opt = opt || {};
    me.$el = opt.$el;
    me.$filterFormContent = me.$el.find('.hideable');
    if (me.$el == null) {
        throw Error('me.$el should be JQuery-like DOM element');
    }

    me.controlsMap = me.initControlsMap();
    me.$filteredMarker = $('#icoFilter');
    me.$btnExpandFilter = $("#btnMore");
    me.$expandIcon = $('#icoPlus');
    me.$caseFilterType = $('#lstfilterCaseProgress');
    me.$btnResetFilter = me.$el.find('.btn-reset');
    me.$btnClearFilter = me.$el.find('.btn-clear');
    me.$searchField = me.$el.find('#txtFreeTextSearch');
    me.initiatorField = me.$el.find('#CaseInitiatorFilter');
    me.$searchOnlyInMyCases = $('#SearchInMyCasesOnly');

    /************** EVENTS BINDING ************************/
    me.$searchField.keydown(function (ev) {
        if (ev.keyCode == 13) {
            ev.preventDefault();
            me.onSearchClick.call(me);
            return false;
        }
    });
    
    me.initiatorField.keydown(function (ev) {
        if (ev.keyCode == 13) {
            ev.preventDefault();
            me.onSearchClick.call(me);
            return false;
        }
    });

    me.$btnResetFilter.click(function(ev) {
        ev.preventDefault();
        me.reset();
        $(this).parent('.btn-group.open').removeClass('open');
        return false;
    });

    me.$btnClearFilter.click(function(ev) {
        ev.preventDefault();
        me.clear();
        $(this).parents('.btn-group.open').removeClass('open');
        return false;
    });

    me.$btnExpandFilter.click(function (e) {
        e.preventDefault();
        me.toggleFilter.call(me, !me.$filterFormContent.is(':visible'));
        return false;
    });

    me.$searchOnlyInMyCases.on('switchChange.bootstrapSwitch', function () {
        if ($(this).prop('checked')) {
            me.clear();
        } else {
            me.reset();
        }
    });

    $('.submit').on('click', function (ev) {
        ev.preventDefault();
        me.onSearchClick.call(me);
        return false;
    });

    $('.input-append.date').datepicker({
        format: 'yyyy-mm-dd',
        autoclose: true,
        clearBtn: true,
        weekStart: 1
    });

    if (me.isFilterEmpty()) {
        me.$filteredMarker.hide();
        me.toggleFilter(false);
    } else {
        me.$filteredMarker.show();
        me.toggleFilter(true);
    }
};

/**
* @public
*/
FilterForm.prototype.getSavedSeacrhCaseTypeValue = function() {
    return parseInt($.cookie('caseoverveiew.filter.searchCaseType'), 10);
};

/**
* @public
*/
FilterForm.prototype.saveSeacrhCaseTypeValue = function() {
    var me = this;
    $.cookie('caseoverveiew.filter.searchCaseType', me.getSearchCaseType());
};

/**
* @public
* @returns int
*/
FilterForm.prototype.getSearchCaseType = function() {
    var me = this;
    var res = parseInt(me.$caseFilterType.val(), 10);
    if (!isNaN(res)) {
        return res;
    }

    throw new Error('unknow value in caseFilterType control');
};

/**
* @public
* Forms filter data to send through AJAX
*/
FilterForm.prototype.getFilterToSend = function () {
    var me = this;
    return me.$el.serializeArray();
};

/**
* @public
* @chainable
*/
FilterForm.prototype.clear = function () {
    var me = this;
    $.each(me.controlsMap, function(fieldId, control) {
        control.clear();
    });
    return me;
};

/**
* @param { bool } forceShow
*/
FilterForm.prototype.toggleFilter = function (forceShow) {
    var me = this;
    if ((forceShow != null && forceShow !== false)
        || (forceShow == null && me.$filterFormContent.is(':visible'))) {
        me.$filterFormContent.show();
        me.$expandIcon.removeClass('icon-plus-sign').addClass('icon-minus-sign');
    } else {
        me.$filterFormContent.hide();
        me.$expandIcon.removeClass('icon-minus-sign').addClass('icon-plus-sign');
    }
};

/**
* @public
* Resets filter to values that user set in "Settings" tab
*/
FilterForm.prototype.reset = function() {
    var me = this;
    var fSettings = me.opt.filter;
    if (fSettings == null || fSettings.length == 0) {
        me.clear();
    } else {
        $.each(fSettings, function (idx, field) {
            var control = me.getControlByFieldName(field.attrName);
            if (control != null) {
                control.setValue(field.value);
            }
        });
    }
};


/**
* Resolves whether filter form fields is empty
* @returns { bool } 
*/
FilterForm.prototype.isFilterEmpty = function () {
    var me = this;
    var res = true;
    $.each(me.controlsMap, function(id, control) {
        res = res && control.isValueEmpty();
        return res;
    });
    return res;
};


/*********** PRIVATE *****************/
FilterForm.prototype.getControlByFieldName = function(fieldName) {
    var me = this;
    return me.controlsMap[fieldName];
};

/**
* @private
* Key halndler on "Search" button
*/
FilterForm.prototype.onSearchClick = function () {
    var me = this;
    var searchStr = me.$searchField.val();
    if (me.opt.onBeforeSearch()) {
        if (searchStr.length > 0 && searchStr[0] === '#') {
            /// if looking by case number - set case state filter to "All"
            me.$caseFilterType.val(-1);
            /// and reset search filter
            me.$searchOnlyInMyCases.bootstrapSwitch('state', false);
            me.clear();
        }
        if (me.isFilterEmpty()) {
            $('#icoFilter').hide();
        } else {
            $('#icoFilter').show();
        }
        me.opt.onSearch();
    }
};

/**
* @private
*/
FilterForm.prototype.initControlsMap = function() {
    var me = this;
    var res = {};
    var controlsId = ['CaseInitiatorFilter', 'lstFilterRegion', 'lstfilterDepartment', 'lstfilterUser',
        'hidFilterCaseTypeId', 'hidFilterProductAreaId', 'lstfilterCategory',
        'lstfilterWorkingGroup', 'lstfilterResponsible', 'lstfilterPerformer',
        'lstfilterPriority', 'lstfilterStatus', 'lstfilterStateSecondary',
        'hidFilterClosingReasonId',
        'CaseRegistrationDateStartFilter', 'CaseRegistrationDateEndFilter',
        'CaseWatchDateStartFilter', 'CaseWatchDateEndFilter',
        'CaseClosingDateStartFilter', 'CaseClosingDateEndFilter', 'lstfilterCaseRemainingTime'
    ];
    $.each(controlsId, function(id, controlName) {
        var control;
        var $el;
        var searchEl = '[name=' + controlName + ']';
        switch(controlName) {
            case 'CaseInitiatorFilter':
            case 'txtFreeTextSearch':
                $el = me.$el.find(searchEl);
                if (!window.is$ElEmpty($el)) {
                    control = CreateInstance(BaseField, { $el: $el });    
                }
                break;
            case 'lstFilterRegion':
            case 'lstfilterDepartment':
            case 'lstfilterUser':
            case 'lstfilterCategory':
            case 'lstfilterWorkingGroup':
            case 'lstfilterResponsible':
            case 'lstfilterPerformer':
            case 'lstfilterPriority':
            case 'lstfilterStatus':
            case 'lstfilterStateSecondary':
                $el = me.$el.find(searchEl);
                if (!window.is$ElEmpty($el)) {
                    control = CreateInstance(JQueryChosenField, { $el: $el });
                }
                break;
            case 'hidFilterCaseTypeId':
            case 'hidFilterProductAreaId':
            case 'hidFilterClosingReasonId':
                $el = me.$el.find(searchEl).parent('.btn-group');
                if (!window.is$ElEmpty($el)) {
                    control =  CreateInstance(DropdownButtonField, { $el: $el });
                }
                break;
            case 'CaseRegistrationDateStartFilter':
            case 'CaseRegistrationDateEndFilter':
            case 'CaseWatchDateStartFilter':
            case 'CaseWatchDateEndFilter':
            case 'CaseClosingDateStartFilter':
            case 'CaseClosingDateEndFilter':
                $el = me.$el.find(searchEl).parent('.input-append.date.dateie');
                if (!window.is$ElEmpty($el)) {
                    control = CreateInstance(DateField, { $el: $el });
                }
                break;
            case 'lstfilterCaseRemainingTime':
                $el = me.$el.find(searchEl);
                if (!window.is$ElEmpty($el)) {
                    control = CreateInstance(JQueryChosenField, { $el: $el });
                }
                break;
            default:
                throw Error('Create UI is not registered for control "' + controlName + '" in initControlsMap()');
        }
        
        if (control != null) {
            res[controlName] = control;
        }
    });
    return res;
};
