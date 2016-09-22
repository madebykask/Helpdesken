if (window.BR == null)
    BR = {};


$(function () {
   
    BR.VModels = {
        ConditionVModel: function () {
            this.Id = null;
            this.Field = null;
            this.FromValue = null;
            this.ToValue = null;
            this.Sequence = null;
            this.Status = null;            
        },

        ActionVModel: function () {
            this.Id = null;
            this.Field = null;
            this.FromValue = null;
            this.ToValue = null;
            this.Sequence = null;
            this.Status = null;
        },

        RuleVModel: function () {
            this.Id = null;
            this.Name = "";
            this.ModuleId = null;
            this.EventId = null;
            this.Sequence = null;
            this.ContinueOnSuccess = true;
            this.ContinueOnError = true;
            this.Status = null;

            this.ConditionSection = new BR.Models.ConditionVModel();
            this.ActionSection = new BR.Models.ActionVModel();
        }
    }

    BR.Controller = {
        RuleTemplate: null,
        RuleConditionTemplate: null,

        Rule: function () {
            _container = null;            

            this.Initialize = function () {
                this.Container = $($("#ruleArea").html(BR.Controller.RuleTemplate.render(this.GetViewModel())));
            },

            this.GetViewModel = function () {
                var model = new BR.Models.RuleModel();
                model.Id = 1;
                model.ModuleId = 1;
                model.EventId = 1;
                model.Name = "Rule1";
                model.Sequence = 1;
                model.ContinueOnSuccess = true;
                model.ContinueOnError = true;                
                model.Status = 1;

                model.ConditionSection.Field = "tt";
                //model.ActionSection = null;

                return model;
            }
        },        
    }

    var loadRuleTemplate = function () {
        return $.get("/areas/admin/content/templates/businessrule/rule.tmpl.html", function (ruleTemplate) {
            BR.Controller.RuleTemplate = $.templates("rule", ruleTemplate);
        });
    };

    var loadRuleConditionTemplate = function () {
        return $.get("/areas/admin/content/templates/businessrule/rule-condition.tmpl.html", function (ruleConditionTemplate) {
            BR.Controller.RuleConditionTemplate = $.templates("conditionSection", ruleConditionTemplate);
        });
    };

    var loadRulePage = function () {        
        loadRuleTemplate()
            .then(loadRuleConditionTemplate)
            .then(function () {
                var rule = new BR.Controller.Rule();            
                rule.Initialize();            
        });
    };    

    loadRulePage();
});