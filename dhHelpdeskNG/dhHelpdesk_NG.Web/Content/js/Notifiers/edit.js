"use strict";

/**
* Management select with subselect in organization units.
* NOTE: multiselects are not supported
*/
$(function () {
    var firstLevelData = [], secondLevelData = [];
    var organizationData = window.organizationData || [];
    var parameters = window.displaySettings;
    var miscParameters = window.parameters;

    /**
    * Searches items by id
    * @params { object[] } data_array 
    * @params { string } id_to_search
    * returns { object[] }
    */
    function searchOUByParentId(dataArray, idToSearch) {
        var i = 0;
        var res = [];
        var len = dataArray.length;
        if (len == 0) {
            return res;
        }
        for (i = 0; i < len; i++) {
            if (dataArray[i]['parent_id'] === idToSearch) {
                res.push(dataArray[i]);
            }
        }
        return res;
    }

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

    function onParentSelectChanged(parentId, selectedId) {
        var subItems = searchOUByParentId(secondLevelData, parentId);
        var GEN_EMPTY_OPTION = true;

        if (subItems.length > 0) {
            $('#sub_organization_unit_dropdown').html(generateOptions(subItems, selectedId, GEN_EMPTY_OPTION)).show();
            
        } else {
            $('#sub_organization_unit_dropdown').hide();
        }
        if (selectedId == null) {
            $('#OrganizationUnitId').val(parentId);
        } else {
            $('#OrganizationUnitId').val(selectedId);
        }
    }

    /**
    * Intializes select and subselect for organization unit 
    * @param { object[] } data
    * @param { string } selectedId
    * @param { object } paramters
    */
    function initOrganizationUnitControls(data, selectedId, parameters) {
        var selectedInLevel = 1;
        var parentSelectedId;
        firstLevelData = [];
        secondLevelData = [];

        $.each(data, function (idx, el) {
            /// force to select first found sub-item if field marked as required
            if (selectedId == null && parameters.Required) {
                selectedId = el.id;
            }

            if (el.parent_id == '') {
                firstLevelData.push(el);
            } else {
                secondLevelData.push(el);
                if (el.id === selectedId) {
                    selectedInLevel = 2;
                    parentSelectedId = el.parent_id;
                }
            }
        });
        if (selectedInLevel === 1) {
            $('#organization_unit_dropdown').html(generateOptions(firstLevelData, selectedId, !parameters.Required));
            onParentSelectChanged(selectedId, null);
        } else {
            $('#organization_unit_dropdown').html(generateOptions(firstLevelData, parentSelectedId, !parameters.Required));
            onParentSelectChanged(parentSelectedId, selectedId);
        }
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

    function FetchOrgnaizationUnits(regionId, departmentId) {
        initOrganizationUnitControls([], null, parameters);
        $.get('/Notifiers/OrganizationUnits', {
            regionId: regionId,
            departmentId: departmentId
        }, function (data) {
            if (data) {
                initOrganizationUnitControls(data, null, parameters);
            }
        });
    }

    $('#region_dropdown').change(function () {
        $('#OrganizationUnitId').val('');
        $('#organization_unit_dropdown').val('').html('');
        $('#sub_organization_unit_dropdown').val('').html('').hide();
        $.get(parameters.departmentDropDownUrl, { regionId: $(this).val() }, function (departmentDropDownMarkup) {
            if ($('#department_dropdown').length === 0) {
                FetchOrgnaizationUnits($('#region_dropdown').val(), '');
            } else {
                $('#department_dropdown').html($(departmentDropDownMarkup).html());
                $('#department_dropdown').trigger('change');
            }
        });
    });

    if (!$('#organization_unit_dropdown').is(':visible')) {
        return;
    }

    /// events
    $('#department_dropdown').change(function() {
        FetchOrgnaizationUnits($('#region_dropdown').val(), $('#department_dropdown').val());
    });
    $('#organization_unit_dropdown').on('change', function() {
        onParentSelectChanged($('#organization_unit_dropdown').val());
    });
    $('#sub_organization_unit_dropdown').on('change', function() {
        var subItemId = $('#sub_organization_unit_dropdown').val();
        if (subItemId !== '') {
            $('#OrganizationUnitId').val(subItemId);
        } else {
            $('#OrganizationUnitId').val($('#organization_unit_dropdown').val());
        }
    });
    var initValue = $('#OrganizationUnitId').val();
    if (initValue === '') {
        initValue = null;
    }
    initOrganizationUnitControls(organizationData, initValue, parameters);
});