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

