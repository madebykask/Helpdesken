if (window.BR == null)
    BR = {};


$(function () {
    _customerId = window.Params.customerId;
    _ruleId = window.Params.ruleId;
    _ruleSection = window.Params.ruleSectionId;
    _conditionSection = window.Params.conditionSectionId;
    _actionSection = window.Params.actionSectionId;
    _ruleTemplatePath = window.Params.ruleTemplatePath;
    _conditionSecTemplatePath = window.Params.conditionSecTemplatePath;
    _conditionTemplatePath = window.Params.conditionTemplatePath;
    _actionTemplatePath = window.Params.actionTemplatePath;
        
    BR.Models = {
        RuleViewModel: function (){
            this.Id = 1;
            this.Name = "Rule Name 1";            
            this.Event = null;
            this.Sequence = null;
            this.ContinueOnSuccess = true;
            this.ContinueOnError = true;
            this.Status = null;            
        },

        ConditionModel: function () {
            this.Id = 1;
            this.Field = null;
            this.FromValue = null;
            this.ToValue = null;
            this.Sequence = null;
            this.Status = null;
        },

        ConditionSecModel: function () {                      
            this.Conditions = [];

            this.AddCondition = function (condition) {
                this.Conditions.push(condition);
            }
        },

        ActionViewModel: function () {
            this.Id = null;
            this.Field = null;
            this.FromValue = null;
            this.ToValue = null;
            this.Sequence = null;
            this.Status = null;
        },

        RuleSectionModel: function (sectionId, templatePath) {
            RuleSec = this;
            this.SectionCaption = "";
            this.RuleTemplate = null;
            this.Model = new BR.Models.RuleViewModel();

            this.ConditionSection = null;
            this.ActionSection = null;

            var loadRuleTemplate = function (templatePath) {
                return $.get(templatePath, function (ruleTemplate) {
                    RuleSec.RuleTemplate = $.templates("rule", ruleTemplate);
                });
            };

            loadRuleTemplate(templatePath).then(function () {
                $(sectionId).html(RuleSec.RuleTemplate.render(RuleSec.Model));
            });
            
        },

        ConditionSectionModel: function (sectionId, templatePath) {
            ConditionSec = this;
            this.SectionCaption = "";
            this.ConditionSecTemplate = null;
            this.ConditionRowsTemplate = null;
            this.Model = null;

            var loadConditionSecTemplate = function (templatePath) {
                return $.get(templatePath, function (conditionSecTemplate) {
                    ConditionSec.ConditionSecTemplate = $.templates("ruleConditionSec", conditionSecTemplate);
                });
            };

            var loadConditionRowsTemplate = function (templatePath) {
                return $.get(templatePath, function (conditionRowsTemplate) {
                    ConditionSec.ConditionRowsTemplate = $.templates("ruleConditionRows", conditionRowsTemplate);
                });
            };

            

            loadConditionTemplate(templatePath).then(function () {
                ConditionSec.Model = new BR.Models.ConditionSecModel();
                $(sectionId).html(ConditionSec.ConditionTemplate.render(ConditionSec.Model));

                var newCon = new BR.Models.ConditionModel();
                newCon.Id = 1;
                newCon.Field = "R1";
                newCon.FromValue = "F1";
                newCon.ToValue = "T1";
                ConditionSec.Model.AddCondition(newCon);                
            });
            
            
        },

        ActionSectionModel: function (sectionId, templatePath) {
            this.SectionCaption = "";
            this.Actions = [];

            this.AddAction = function (action) {
                this.Actions.push(action);
                this.Draw();
            };

            this.DrawSection = function () {
            }

        }
    }

    BR.Views = {
        BusinessRulePage: function () {
            this.CustomerId = _customerId;
            this.Id = _ruleId;
            this.RuleSection = new BR.Models.RuleSectionModel(_ruleSection, _ruleTemplatePath);
            this.RuleSection.ConditionSection = new BR.Models.ConditionSectionModel(_conditionSection, _conditionSecTemplatePath);
            //this.RuleSection.ActionSection = new BR.Models.ActionSectionModel(_actionSection, _actionTemplatePath);

            this.Initialize = function() {
                
            };

            this.SavePage = function () {
                // Save to database
            };
        }
    }

    BR.Controllers = {        
        GenerateBR: function () {
            var BRPage = new BR.Views.BusinessRulePage();
        }        
    }   

    var loadPage = function () {        
        var p = BR.Controllers.GenerateBR();
    };    

    loadPage();
});