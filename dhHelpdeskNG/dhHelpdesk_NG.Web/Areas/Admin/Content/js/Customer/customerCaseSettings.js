'use strict';

var properties = window.fieldProperties;


$(function () {
    var input = "input[name$='].";
    $(input + properties.ShowOnStartPage).click(function () {
        var elementName = this.name.replace(properties.ShowOnStartPage, properties.ShowExternal);
        var curElement = jQuery('[name="' + elementName + '"]');
        var visibleElement = curElement.filter(':visible').get(0);
        if (this.checked == false && visibleElement != undefined)
            visibleElement.checked = false;
    });
   
});

$(function () {    
    $(input + properties.Label).attr('maxlength', '50');    
    $(input + properties.FieldHelp).attr('maxlength', '200');
});

