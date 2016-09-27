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
        
    BR.StyleManager = {
                        
        setFieldStyle: function(onChangeEvent){
            var fieldClass = '.chosen-single-select.dropDownField';
            $(fieldClass).chosen({
                width: "100%",
                height: "100px",
                placeholder_text_single: "select an item",
                search_contains: true,
                'no_results_text': '?',
            }).on('change', function(evt, params) {
                onChangeEvent(this, params);                
            });                
        },

        setFieldDataStyle: function(){
            var fieldDataClass = '.chosen-select.dropDownFieldData';
            $(fieldDataClass).chosen({
                width: "100%",
                height: "100px",
                placeholder_text: "select an item",
                search_contains: true,
                'no_results_text': '?',
            });
        },

        setCheckBoxStyle: function(){
            var checkBox = '.switchcheckbox';
            $(checkBox).bootstrapSwitch('onText', 'Yes');
            $(checkBox).bootstrapSwitch('offText', 'No');
            $(checkBox).bootstrapSwitch('size', 'small');
            $(checkBox).bootstrapSwitch('onColor', 'success');
        }
    }

    BR.Templates = {
        RulePageTemplate: null,
        RuleTemplate: null,
        ConditionTemplate: null,
        ConditionRowsTemplate: null
    }

    BR.Models = {

        RulePageViewModel: function (ruleId, customerId) {
            this.Id = ruleId;
            this.CustomerId = customerId;

            this.RuleSection = [];
            this.ConditionSection = [];
            this.ActionSection = [];            
        },

        RuleSecViewModel: function (sectionId) {            
            this.SectionCaption = "";
            this.Id = 1;
            this.Name = "Rule Name 1";
            this.Event = null;
            this.Sequence = null;
            this.ContinueOnSuccess = true;
            this.ContinueOnError = true;
            this.Status = null;                      
        },

        ConditionSecViewModel: function () {
            this.SectionCaption = "";
            this.Conditions = [];

            this.AddCondition = function (condition) {
                this.Conditions.push(condition);
            }
        },

        ConditionRowModel: function () {
            this.Id = 1;
            this.Field = null;
            this.FromValue = null;
            this.ToValue = null;
            this.Sequence = null;
            this.Status = null;
        },

        ActionViewModel: function () {
            this.Id = null;
            this.Field = null;
            this.FromValue = null;
            this.ToValue = null;
            this.Sequence = null;
            this.Status = null;
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
                                               
            var loadRulePageTemplate = function () {
                return $.get(_rulePageAreaTemplatePath, function (rulePageTemplate) {
                    BR.Templates.RulePageTemplate = $.templates("rulePageTemp", rulePageTemplate);
                });
            };

            var loadRuleTemplate = function () {                
                return $.get(_ruleSecTemplatePath, function (ruleSecTemplate) {
                    BR.Templates.RuleTemplate = $.templates("ruleSecTemp", ruleSecTemplate);
                });
            };

            var loadConditionTemplate = function () {                
                return $.get(_conditionSecTemplatePath, function (conditionSecTemplate) {
                    BR.Templates.ConditionTemplate = $.templates("conditionSecTemp", conditionSecTemplate);
                });
            };

            var loadConditionRowsTemplate = function () {
                return $.get(_conditionRowsTemplatePath, function (conditionRowsTemplate) {
                    BR.Templates.ConditionRowsTemplate = $.templates("conditionRows", conditionRowsTemplate);
                });
            };
            
            var loadAllTemplates = function() {
                loadRulePageTemplate()
                    .then(loadRuleTemplate)
                    .then(loadConditionTemplate)
                    .then(loadConditionRowsTemplate)
                    .then(function () {

                        var rulePageModel = new BR.Models.RulePageViewModel(_ruleId, _customerId);
                        
                        
                        var ruleSecModel = new BR.Models.RuleSecViewModel();
                        rulePageModel.RuleSection = [];
                        rulePageModel.RuleSection.push(ruleSecModel);

                        var conditionSecModel = new BR.Models.ConditionSecViewModel();
                        conditionSecModel.SectionCaption = 'Conditions';
                        rulePageModel.ConditionSection = [];
                        
                        var conditionRowModel = new BR.Models.ConditionRowModel();
                        conditionRowModel.Id = 1;
                        conditionSecModel.Conditions.push(conditionRowModel);

                        var conditionRowModel = new BR.Models.ConditionRowModel();
                        conditionRowModel.Id = 2;
                        conditionSecModel.Conditions.push(conditionRowModel);

                        rulePageModel.ConditionSection.push(conditionSecModel);

                        rulePage.drawPage(rulePageModel);                        
                    });
            };

            this.drawPage = function(pageModel){
                $(_rulePageAreaId).html(BR.Templates.RulePageTemplate.render(pageModel));
                this.drawRuleSection(pageModel.RuleSection);
                this.drawConditionSection(pageModel.ConditionSection);
            }

            this.drawRuleSection = function (models) {                
                if (BR.Templates.RuleTemplate != null)
                    $(_ruleSectionId).html(BR.Templates.RuleTemplate.render(models));
            }

            this.drawConditionSection = function (models) {
                if (models.length > 0) {
                    for (var mo = 0; mo < models.length; mo++) {
                        var model = models[mo];
                        if (BR.Templates.ConditionTemplate != null) {
                            var container = $($(_conditionSectionId).html(BR.Templates.ConditionTemplate.render(model)));

                            var rows = container.find(".rule-condition-rows");
                            rows.html('');

                            for (var i = 0; i < model.Conditions.length; i++) {
                                var row = $(BR.Templates.ConditionRowsTemplate.render(model.Conditions[i]));
                                rows.append(row);
                            }
                        }
                    }
                }

                BR.StyleManager.setFieldStyle(this.onChangeField);
                BR.StyleManager.setFieldDataStyle();
                BR.StyleManager.setCheckBoxStyle();
               
            }

            this.Initialize = function () {
                loadAllTemplates();
            };

            this.onChangeField = function (sender, params) {
                var selectedValue = params.selected;
                

                var tt = $(sender).chosen();
                var selectedId = sender.selectedIndex;
                alert(selectedValue);
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