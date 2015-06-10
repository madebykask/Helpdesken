'use strict';

var properties = window.fieldProperties;

var input = "input[name$='].";
$(function () {
    $(input + properties.ShowOnStartPage).click(function () {
        var elementName = this.name.replace(properties.ShowOnStartPage, properties.ShowExternal);
        var curElement = jQuery('[name="' + elementName + '"]');
        console.log('catch me');
        var $requiredElement = $(this).parents('tr').find('[name$="Required"]:checkbox').not(':disabled');
        var visibleElement = curElement.filter(':visible').get(0);
        if (this.checked == false && visibleElement != undefined) {
            visibleElement.checked = false;
            $requiredElement.prop('checked', false);
        }
    });

    $(input + properties.ShowOnStartPage).click(function () {
        var _this = $(this);
        var elemLocked = _this.closest('tr').find('select[name$=".Locked"]');
        if (!_this[0].checked) {
            elemLocked.prop('disabled', true);
            elemLocked.find('option:eq(0)').prop('selected', true);
        } else {
            elemLocked.prop('disabled', false);
        }
    });

    $('select[name$=".Locked"]').each(function (i, e) {
        var _this = $(this);
        var elemShowOnstartPage = _this.closest('tr').find('input[name$=".ShowOnStartPage"]');
        if (!elemShowOnstartPage[0].checked) {
            _this.prop('disabled', true);
            _this.find('option:eq(0)').prop('selected', true);
        } else {
            _this.prop('disabled', false);
        }

    });

    $(input + properties.ShowExternal).click(function () {
        var elementName = this.name.replace(properties.ShowExternal, properties.ShowOnStartPage);
        var curElement = jQuery('[name="' + elementName + '"]');
        var visibleElement = curElement.filter(':visible').get(0);
        if (visibleElement != undefined && visibleElement.checked == false) {
            this.checked = false;
        }
    });

    $('input[name$=".Required"]').on('click', function (ev) {
        var $mainEl = $(this).parents('tr').find('[name$=".ShowOnStartPage"]:checkbox');
        if (!$mainEl.prop('checked')) {
            ev.preventDefault();
            return false;
        }
        return true;
    });
});

$(function () {    
    $(input + properties.Label).attr('maxlength', '50');    
    $(input + properties.FieldHelp).attr('maxlength', '200');
});

