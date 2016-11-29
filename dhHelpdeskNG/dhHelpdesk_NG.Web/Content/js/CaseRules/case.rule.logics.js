$(function () {
    (function ($) {
        window.helpdesk = window.helpdesk || {};
        window.helpdesk.caseRule = window.helpdesk.caseRule || {};
        var params  = window.params;

        var ruleModel = null;
        var $elementsHaveRule = $('.acceptRules');

        helpdesk.caseRule.init = function () {
            ruleModel = params.ruleModel;
            $elementsHaveRule.change(function () {
                helpdesk.caseRule.checkRules();
            });
        };
          
        helpdesk.caseRule.checkRules = function () {
            alert('checked');
        };

        var func1 = function () {
            //return $.get("/Translation/GetCaseFieldsForTranslation", {
            //    curTime: Date.now
            //}, function (data) {
                
            //});
        };
        

        var testFunc = function () {
            func1()            
            .then(function () {
                
            });
        }

    })($);
       
    //helpdesk.caseRule.init();
});