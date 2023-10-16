"use strict";
var regionControlName = '#Region';
var departmentControlName = '#Department';
var departmentControlId = '#CaseSolution.Department_Id';
var OUControlName = '#OU';
var OUControlId = '#CaseSolution.OU_Id';

var isAbout_RegionControlName = '#IsAbout_Region';
var isAbout_DepartmentControlName = '#IsAbout_Department';
var isAbout_DepartmentControlId = '#CaseSolution.IsAbout_Department_Id';
var isAbout_OUControlName = '#IsAbout_OU';
var isAbout_OUControlId = '#CaseSolution.IsAbout_OU_Id';

// controller methods:
var changeRegion = '/CaseSolution/ChangeRegion';
var changeDepartment = '/CaseSolution/ChangeDepartment/';

var _params = window.params;
var saveDropDownCaption = _params.saveDropDownCaption;

$(function () {
    
    function init() {
        
    }

    $(regionControlName).change(function () {
        //refreshDepartments($(this).val());
    });

    $(departmentControlName).change(function () {
        //refreshOrganizationUnits($(this).val());   
    });

    $(isAbout_RegionControlName).change(function () {       
        //refreshIsAbout_Departments($(this).val());
    });

    $(isAbout_DepartmentControlName).change(function () {        
        //refreshIsAbout_OrganizationUnits($(this).val());
    });

    function refreshDepartments(regionId) {
        $(departmentControlId).val('');
        var ctlOption = departmentControlName + ' option';
        $.post(changeRegion, { 'regionId': regionId }, function (data) {
            $(ctlOption).remove();
            $(departmentControlName).append('<option value="">&nbsp;</option>');
            $(departmentControlName).prop('disabled', true);
            if (data != undefined) {
                for (var i = 0; i < data.list.length; i++) {
                    var item = data.list[i];
                    var option = $("<option value='" + item.id + "'>" + item.name + "</option>");                    
                    $(departmentControlName).append(option);
                }
            }
        }, 'json').always(function () {
            $(departmentControlName).change();
            $(departmentControlName).prop('disabled', false);
        });
    }

    function refreshOrganizationUnits(departmentId) {
        $(OUControlId).val('');
        var ctlOption = OUControlName + ' option';
        $.post(changeDepartment, { 'departmentId': departmentId }, function (data) {
            $(ctlOption).remove();
            $(OUControlName).append('<option value="">&nbsp;</option>');
            $(OUControlName).prop('disabled', true);
            if (data != undefined) {
                for (var i = 0; i < data.list.length; i++) {
                    var item = data.list[i];
                    var option = $("<option value='" + item.id + "'>" + item.name + "</option>");                    
                    $(OUControlName).append(option);
                }
            }
        }, 'json').always(function () {            
            $(OUControlName).prop('disabled', false);
        });
    }

    function refreshIsAbout_Departments(regionId) {
        $(isAbout_DepartmentControlId).val('');
        var ctlOption = isAbout_DepartmentControlName + ' option';
        $.post(changeRegion, { 'regionId': regionId }, function (data) {
            $(ctlOption).remove();
            $(isAbout_DepartmentControlName).append('<option value="">&nbsp;</option>');
            $(isAbout_DepartmentControlName).prop('disabled', true);
            if (data != undefined) {
                for (var i = 0; i < data.list.length; i++) {
                    var item = data.list[i];
                    var option = $("<option value='" + item.id + "'>" + item.name + "</option>");
                    $(isAbout_DepartmentControlName).append(option);
                }
            }
        }, 'json').always(function () {
            $(isAbout_DepartmentControlName).change();
            $(isAbout_DepartmentControlName).prop('disabled', false);
        });
    }

    function refreshIsAbout_OrganizationUnits(departmentId) {

        $(isAbout_OUControlId).val('');
        var ctlOption = isAbout_OUControlName + ' option';
        $.post(changeDepartment, { 'departmentId': departmentId }, function (data) {
            $(ctlOption).remove();
            $(isAbout_OUControlName).append('<option value="">&nbsp;</option>');
            $(isAbout_OUControlName).prop('disabled', true);
            if (data != undefined) {
                for (var i = 0; i < data.list.length; i++) {
                    var item = data.list[i];
                    var option = $("<option value='" + item.id + "'>" + item.name + "</option>");
                    $(isAbout_OUControlName).append(option);
                }
            }
        }, 'json').always(function () {
            $(isAbout_OUControlName).prop('disabled', false);
        });
    }   

    //$('#WorkingGroup').change(function () {
    //    alert();
    //    // filter administrators
    //    CaseCascadingSelectlistChange($(this).val(), $('#CaseSolution_Customer_Id').val(), '/CaseSolution/ChangeWorkingGroupFilterUser/', '#PerformerUser');
    //});

    $('#divFinishingCause ul.dropdown-menu li a').click(function (e) {
        e.preventDefault();
        var val = $(this).attr('value');
        $("#divBreadcrumbs_FinishingCause").text(getBreadcrumbs(this));
        $("#CaseSolution_FinishingCause_Id").val(val).trigger('change');
    });
    
    $('#divCaseType ul.dropdown-menu li a').click(function (e) {
        e.preventDefault();
        var val = $(this).attr('value');
        $("#divBreadcrumbs_CaseType").text(getBreadcrumbs(this));
        $("#CaseSolution_CaseType_Id").val(val).trigger('change');
    });

    $('#divProductArea ul.dropdown-menu li a').click(function (e) {
        e.preventDefault();
        var val = $(this).attr('value');
        $("#divBreadcrumbs_ProductArea").text(getBreadcrumbs(this));
        $("#CaseSolution_ProductArea_Id").val(val).trigger('change');
    });

    $('#divCategory ul.dropdown-menu li a').click(function (e) {
        e.preventDefault();
        var val = $(this).attr('value');
        $("#divBreadcrumbs_Category").text(getBreadcrumbs(this));
        $("#CaseSolution_Category_Id").val(val).trigger('change');
    });
    
    $("select").change(function () {

        if ($(this).attr('elementclass') == 'OptionDropDown') {
            var e = document.getElementById($(this).attr('id'));
            var curVal = e.options[e.selectedIndex].value;
            var resultValue = "";

            switch (curVal) {
                case "1":
                    resultValue = "DisplayField";
                    break;

                case "2":
                    resultValue = "ReadOnly";
                    break;

                case "3":
                    resultValue = "Hide";
                    break;
            }

            var curName = $(this).attr('elementname');

            var hideElement = document.getElementById('hide_' + curName);
            hideElement.setAttribute("value", resultValue);

        }
    });

    /// enable/disable correct choices in schedule  
    function SetDayEnable() {
        document.getElementById("cbo0").removeAttribute('disabled');
        document.getElementById("cbo1").setAttribute('disabled', true);
        document.getElementById("cbo2").setAttribute('disabled', true);
    }

    function SetWeekEnable() {
        document.getElementById("cbo0").setAttribute('disabled', true);
        document.getElementById("cbo1").removeAttribute('disabled');
        document.getElementById("cbo2").removeAttribute('disabled');
    }

    if (document.getElementById("optday").getAttribute('checked') == "'checked'") {
        SetDayEnable();
    } else {
        SetWeekEnable();
    }
    
    $("#optday").click(function () {
        SetDayEnable();
    });

    $("#optthe").click(function () {
        SetWeekEnable();
    });

    var checkScheduleChange = function() {
        if ($('#optScheduleType1').is(':checked')) {
            $(".chDay").toggle($("[name$=ScheduleType]").index($('#optScheduleType1')) == 0);
            $('.chWeek, .chMonth').hide();
        } else if ($('#optScheduleType2').is(':checked')) {
            $('.chDay, .chWeek').toggle($("[name$=ScheduleType]").index($('#optScheduleType2')) == 1);
            $('.chMonth').hide();
        } else if ($('#optScheduleType3').is(':checked')) {
            $('.chDay, .chMonth').toggle($("[name$=ScheduleType]").index($('#optScheduleType3')) == 2);
            $('.chWeek').hide();
        }
    };

    $("#chkSchedule").click(function () {
        $("#scheduleOptions").toggle();
    });

    checkScheduleChange();
    $("[name$=ScheduleType]").change(checkScheduleChange);

    //$("#Casesolution_ShowInsideCase").click(function () {
    //    if ($('#Casesolution_ShowInsideCase').is(':checked')) {
    //        $(".hideOnInsideCase").show();
    //        $("#CaseSolution_ConnectedButton").prop("disabled", false);
    //        $("#savaAndClose").prop("disabled", false);
    //        removeOptionByValue('savaAndClose', '-2'); // remove temporary option
    //    }else{
    //        $("#CaseSolution_ConnectedButton").prop("disabled", true);
    //        $(".hideOnInsideCase").hide();

    //        // Add temporary option 
    //        var opt = document.createElement("option");
    //        opt.value = "-2";
    //        opt.text = saveDropDownCaption;
    //        var elm = document.getElementById("savaAndClose");
    //        elm.options.add(opt);
    //        $("#savaAndClose").val("-2");
    //        $("#savaAndClose").prop("disabled", true);
    //        $("#CaseSolution_ConnectedButton").val("");
    //    }
    //});

    function removeOptionByValue(id, value) {
        var select = document.getElementById(id);
        for (var i = 0, length = select.options.length; i < length; i++) {

            if (select.options[i] && select.options[i].value === value) {
                select.options[i] = null;
            }
        }
    }

    
    init();
});
