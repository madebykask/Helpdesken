'use strict';
$(function () {
    $('#caseType__WorkingGroup_Id').on('change', function () {
        CaseCascadingSelectlistChange($(this).val(), $('#CaseType_Customer_Id').val(), '/CaseType/ChangeWorkingGroupFilterUser/', '#CaseType_User_Id', 0);
          
    });
});