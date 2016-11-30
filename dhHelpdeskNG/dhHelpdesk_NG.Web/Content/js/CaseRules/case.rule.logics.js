$(function () {
    (function ($) {
        window.helpdesk = window.helpdesk || {};
        window.helpdesk.caseRule = window.helpdesk.caseRule || {};
        var params  = window.params;

        var ruleModel = null;
        var $elementsHaveRule = $('.acceptRules');
        var STANDARD_ID = 'standardId';
          
        helpdesk.common = {
            data: function(){
                this.isNullOrUndefined = function(value){
                    if (value == null || value == undefined)
                        return true;
                    
                    return false;
                }
            }            
        }

        helpdesk.caseRule = {
            dataHelper: null,

            init:function() {
                ruleModel = params.ruleModel;
                dataHelper = new helpdesk.common.data();
                $elementsHaveRule.change(function () {
                    var $self = $(this);
                    helpdesk.caseRule.checkRules($self);
                });
            },

            getFieldById: function(fieldId){
                if (dataHelper.isNullOrUndefined(ruleModel) || 
                    dataHelper.isNullOrUndefined(ruleModel.FieldAttributes) ||
                    ruleModel.FieldAttributes.length <= 0)
                    return null;

                fieldId = fieldId.toLowerCase();
                for(var fi=0; fi<ruleModel.FieldAttributes.length; fi++)
                    if (ruleModel.FieldAttributes[fi].FieldId.toLowerCase() == fieldId)
                    {
                        return ruleModel.FieldAttributes[fi];
                    }

                return null;
            },

            getFieldByElement: function ($element) {
                if (dataHelper.isNullOrUndefined($element))
                    return null;

                var fieldId = $element.attr(STANDARD_ID);

                if (fieldId == "")
                    return null;

                return this.getFieldById(fieldId);                
            },

            checkRules: function ($element) {
                var field = this.getFieldByElement($element);
                if (dataHelper.isNullOrUndefined(field))
                    return;

                //alert(field.FieldName);
            }
        }
                
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
       
    helpdesk.caseRule.init();    
});