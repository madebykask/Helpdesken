"use strict";

/**
* Management select with subselect in organization units.
* NOTE: multiselects are not supported
*/
$(function () {
    var firstLevelData = [], secondLevelData = [];    
    var miscParameters = window.parameters;  

    /**
    * Generates options HTLM by array
    * @param { object[] } data_array
    * @param { string|null } parent_id_of_selected
    * @param { bool } is_put_empty
    */
    function generateOptions(dataArray, selectedId, isPutEmpty) {
        var res = [];
        var i;
        var len = dataArray.length;
        if (isPutEmpty) {
            res.push(['<option value="" ', selectedId == null ? 'selected' : '', '></option>']);
        }
        for (i = 0; i < len; i++) {
            res.push(['<option value="', dataArray[i].id, '" ', selectedId === dataArray[i].id ? 'selected' : '', '>', dataArray[i].name, '</option>'].join(''));
        }
        return res.join('');
    }    

    if (miscParameters.showManager) {
        (function () {
            $('#manager_textbox').typeahead({
                source: function (query, process) {
                    this.displayNames = [];
                    this.mappings = [];
                    var self = this;

                    $.each(miscParameters.managers, function (i, manager) {
                        self.mappings[manager.displayName] = manager;
                        self.displayNames.push(manager.displayName);
                    });

                    process(this.displayNames);
                },
                updater: function (item) {
                    var managerId = this.mappings[item].id;
                    $('#manager_id_hidden').val(managerId);
                    return item;
                }
            });
        })();
    }

    $('#region_dropdown').change(function () {
        $('#departmentId').val('');
        $('#department_dropdown').val('').html('');
        $('#department_dropdown').prop('disabled', true);
        $.get('/Notifiers/DepartmentDropDown', { regionId: $(this).val() }, function (departmentDropDownMarkup) {
            $('#department_dropdown').html($(departmentDropDownMarkup).html());
            $('#department_dropdown').trigger('change');
        }).always(function () {
            $('#department_dropdown').prop('disabled', false);
        });

    });

    $('#department_dropdown').change(function () {
        $('#OrganizationUnitId').val('');
        $('#organizationUnit_dropdown').val('').html('');
        $.get('/Notifiers/OrganizationUnitDropDown', { departmentId: $(this).val() }, function (organizationUnitDropDownMarkup) {
            $('#organizationUnit_dropdown').html($(organizationUnitDropDownMarkup).html());
            //$('#organizationUnit_dropdown').trigger('change');
        });
    });
    
});