"use strict";

$(function () {
    $('#WorkingGroup').change(function () {
        // filter administrators
        CaseCascadingSelectlistChange($(this).val(), $('#CaseSolution_Customer_Id').val(), '/Cases/ChangeWorkingGroupFilterUser/', '#PerformerUser');
    });

    $('#divFinishingCause ul.dropdown-menu li a').click(function (e) {
        e.preventDefault();
        var val = $(this).attr('value');
        $("#divBreadcrumbs_FinishingCause").text(getBreadcrumbs(this));
        $("#CaseSolution_FinishingCause_Id").val(val);
    });

    $('#divCaseType ul.dropdown-menu li a').click(function (e) {
        e.preventDefault();
        var val = $(this).attr('value');
        $("#divBreadcrumbs_CaseType").text(getBreadcrumbs(this));
        $("#CaseSolution_CaseType_Id").val(val);
    });

    $('#divProductArea ul.dropdown-menu li a').click(function (e) {
        e.preventDefault();
        var val = $(this).attr('value');
        $("#divBreadcrumbs_ProductArea").text(getBreadcrumbs(this));
        $("#CaseSolution_ProductArea_Id").val(val);
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
});
