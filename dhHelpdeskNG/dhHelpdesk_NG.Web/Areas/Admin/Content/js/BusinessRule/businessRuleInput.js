if (window.BR == null)
    BR = {};


$(function () {
    _ruleId = window.Params.ruleId;
    _customerId = window.Params.customerId;
    
    _rulePageAreaId = window.Params.rulePageAreaId;
    _rulePageAreaTemplatePath = window.Params.rulePageAreaTemplatePath;

    _ruleSectionId = window.Params.ruleSectionId;
    _ruleSecTemplatePath = window.Params.ruleSecTemplatePath;

    _conditionSectionId = window.Params.conditionSectionId;
    _conditionSecTemplatePath = window.Params.conditionSecTemplatePath;
    _conditionRowsTemplatePath = window.Params.conditionRowsTemplatePath;

    _actionSectionId = window.Params.actionSectionId;            
    _actionTemplatePath = window.Params.actionTemplatePath;
        
    BR.Models = {

        RulePageViewModel: function (ruleId, customerId) {
            this.Id = ruleId;
            this.CustomerId = customerId;

            this.RuleSection = [];
            this.ConditionSection = [];
            this.ActionSection = [];
        },

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

        RuleSectionModel: function (sectionId, template) {
            RuleSec = this;
            this.SectionCaption = "";
            this.RuleTemplate = null;
            this.Model = new BR.Models.RuleViewModel();
                        
            loadRuleTemplate(templatePath).then(function () {
                $(sectionId).html(RuleSec.RuleTemplate.render(RuleSec.Model));
                $('#setValue').on('click', function () {
                    RuleSec.Model.Name = 'mine';                    
                    $(sectionId).html(RuleSec.RuleTemplate.render(RuleSec.Model));
                });
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

            

            loadConditionSecTemplate(templatePath).then(function () {
                ConditionSec.Model = new BR.Models.ConditionSecModel();
                $(sectionId).html(ConditionSec.ConditionSecTemplate.render(ConditionSec.Model));                

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
            rulePage = this;
            

            this.RulePageTemplate = null;
            this.RuleTemplate = null;
            this.ConditionTemplate = null;
            this.ConditionRowsTemplate = null;

            this.RuleSection = null; 
            //this.ConditionSection = new BR.Models.ConditionSectionModel(_conditionSection, _conditionSecTemplatePath);
            //this.RuleSection.ActionSection = new BR.Models.ActionSectionModel(_actionSection, _actionTemplatePath);

            var loadRulePageTemplate = function () {
                return $.get(_rulePageAreaTemplatePath, function (rulePageTemplate) {
                    rulePage.RulePageTemplate = $.templates("rulePageSec", rulePageTemplate);
                });
            };

            var loadRuleTemplate = function () {                
                return $.get(_ruleSecTemplatePath, function (ruleSecTemplate) {
                    rulePage.RuleTemplate = $.templates("ruleSec", ruleSecTemplate);
                });
            };

            var loadConditionTemplate = function () {                
                return $.get(_conditionSecTemplatePath, function (conditionSecTemplate) {
                    rulePage.ConditionTemplate = $.templates("conditionSec", conditionSecTemplate);
                });
            };

            var loadConditionRowsTemplate = function () {
                return $.get(_conditionRowsTemplatePath, function (conditionRowsTemplate) {
                    rulePage.ConditionRowsTemplate = $.templates("conditionRows", conditionRowsTemplate);
                });
            };

            var loadTemplates = function() {
                loadRulePageTemplate()
                    .then(loadRuleTemplate())
                    .then(loadConditionTemplate())
                    .then(loadConditionRowsTemplate())
                    .then(function () {
                        $(_rulePageAreaId).html(rulePage.RulePageTemplate.render(BR.Models.RulePageViewModel(_ruleId, _customerId)));

                        //rulePage.RuleSection = new BR.Models.RuleSectionModel(_rule, _ruleTemplatePath);
                    });
            };

            this.Initialize = function () {
                loadTemplates();
            };

            this.SavePage = function () {
                // Save to database
            };
        }
    }

    BR.Controllers = {        
        GenerateBR: function () {
            var BRPage = new BR.Views.BusinessRulePage();
            BRPage.Initialize();
        }        
    }   

    var loadPage = function () {        
        var p = BR.Controllers.GenerateBR();
    };

    loadPage();    
});