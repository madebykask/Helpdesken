/* This is only used in Norway. Next country to implement should use Data Admin B so this file will not be used /TAN 2016-05-05 */
var core = core || {};

// Fix: Prevent to change saved JobTitle by SetDefJobTitle method S.G
// TODO: Should do SetDefJobTitle only after "Selectize.OnClick()"
var maxLocalJobTitleDefCount = 2;
var SetDefCount = 0;
var SetDefCountReportsToLineManager = 0;

$(document).ready(function () {
    core.baseUrl = $("#core_baseUrl").val();
    core.area = $("#core_area").val();
    core.controller = $("#core_controller").val();
    core.formGuid = $("#formGuid").val();
});

function getXML(xmlFile, url) {
    var path = window.location.protocol + '//';
    path = path + window.location.host + '/';
    path = core.baseUrl + '/FormLibContent/Xmls/' + core.area  + xmlFile;

    $.ajax({
        type: "GET",
        url: path,
        dataType: "xml",
        success: url,
        error: function (jqXHR, textStatus, errorThrown) {
            alert(jqXHR.responseText);
            alert(textStatus);
            alert(errorThrown);
        }
    });
}

//add check if "select" or "search-select" for both baseSelector and targetSelector
//target - add items to select
function narrowingWithXML(xmlFile,xmlFileAllOptions, $baseSelector, $targetSelector) {
    var path = window.location.protocol + '//';
    path = path + window.location.host + '/';
    path = core.baseUrl + '/FormLibContent/Xmls/' + core.area + xmlFile;

    $.ajax({
        type: "GET",
        url: path,
        dataType: "xml",
        success: function (xml) {

            var show = '';

            var baseIsSelectize = $baseSelector.hasClass('search-select');
            var targetIsSelectize = $targetSelector.hasClass('search-select');
                        
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
                if (targetIsSelectize) {
                    var options = $targetSelector[0].selectize.options;
                    var optionsLength = Object.keys(options).length;
                    if (optionsLength == 0) {
                        getAllOptions(xmlFileAllOptions, $targetSelector, targetValue);
                    }
                }
                else {
                    var optionsLength = $targetSelector.find('option').length;
                    if (optionsLength == 0) {
                        getAllOptions(xmlFileAllOptions, $targetSelector, targetValue);
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
};

//add check if "select" or "search-select" for both baseSelector and targetSelector
function defaultWithXML(xmlFile, $baseSelector, $targetSelector) {
    var path = window.location.protocol + '//';
    path = path + window.location.host + '/';
    path = core.baseUrl + '/FormLibContent/Xmls/' + core.area + xmlFile;

    $.ajax({
        type: "GET",
        url: path,
        dataType: "xml",
        success: function (xml) {

            var baseIsSelectize = $baseSelector.hasClass('search-select');
    
            var base = '';
            if (baseIsSelectize) {
                base = $($baseSelector[0].selectize.getOption($baseSelector[0].selectize.getValue())).text();
            }
            else {
            base =  $baseSelector.find('option:selected').text();
            }

            base = base.toLowerCase();

            var foundMatch = false;
            
            $(xml).find('dependent').each(function () {
                var $sel = $(this);
                show = '';

                var baseCompare = $sel.find('selected').text();
                baseCompare = baseCompare.toLowerCase();

                show = $sel.find('show').text();

             
                if (baseCompare == base) {

                    if (show != '') {

                        foundMatch = true;

                        var n = show.indexOf("#");

                        if (n == -1) {
                            if ($targetSelector[0].selectize)
                                $targetSelector[0].selectize.setValue(show);
                            else
                                $targetSelector.val(show);
                        } else {
                            var res = show.substring(0, n);
                            if ($targetSelector[0].selectize)
                                $targetSelector[0].selectize.setValue(res);
                            else
                                $targetSelector.val(show);
                        }
                        return;
                    }
                }
            });

            //If there is no match in xml - empty value
            if (foundMatch == false) {
                if ($targetSelector[0].selectize) {
                    $targetSelector[0].selectize.setValue('');
                }
                else {
                    $targetSelector.val('');
                }
            }

        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(jqXHR.responseText);
            alert(textStatus);
            alert(errorThrown);
        }
    });
};

function getAllOptions(xmlFile, $targetSelector, targetValue) {
    var path = window.location.protocol + '//';
    path = path + window.location.host + '/';
    path = core.baseUrl + '/FormLibContent/Xmls/' + core.area + xmlFile;
    $.ajax({
        type: "GET",
        url: path,
        dataType: "xml",
        success: function (xml) {
            
            var targetIsSelectize = $targetSelector.hasClass('search-select');
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

//Två selectorer (BU_Function)
function narrowingWithXML2Base(xmlFile, xmlFileAllOptions, $baseSelector, $base2Selector, $targetSelector) {
    var path = window.location.protocol + '//';
    path = path + window.location.host + '/';
    path = core.baseUrl + '/FormLibContent/Xmls/' + core.area + xmlFile;

    $.ajax({
        type: "GET",
        url: path,
        dataType: "xml",
        success: function (xml) {

            var show = '';

            var baseIsSelectize = $baseSelector.hasClass('search-select');
            var base2IsSelectize = $base2Selector.hasClass('search-select');

            var targetIsSelectize = $targetSelector.hasClass('search-select');

            var base = '';
            if (baseIsSelectize) {
                base = $($baseSelector[0].selectize.getOption($baseSelector[0].selectize.getValue())).text().toLowerCase();
            }
            else {
                base = $baseSelector.find('option:selected').text().toLowerCase();
            }

            var base2 = '';
            if (base2IsSelectize) {
                base2 = $($base2Selector[0].selectize.getOption($base2Selector[0].selectize.getValue())).text().toLowerCase();
            }
            else {
                base2 = $base2Selector.find('option:selected').text().toLowerCase();
            }

            //put toghether
            base = base + "_" + base2;

            var targetValue;
            var selectize_tags;

            targetValue = $targetSelector.val();

     

            //clear list
            $targetSelector.val('');

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
                if (targetIsSelectize) {
                    var options = $targetSelector[0].selectize.options;
                    var optionsLength = Object.keys(options).length;
                    if (optionsLength == 0) {
                        getAllOptions(xmlFileAllOptions, $targetSelector, targetValue);
                    }
                }
                else {
                    var optionsLength = $targetSelector.find('option').length;
                    if (optionsLength == 0) {
                        getAllOptions(xmlFileAllOptions, $targetSelector, targetValue);
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
};