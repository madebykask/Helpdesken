function WorkflowStepsBehavior(stepChangedCallback) {

    this.stepChangedCallback = stepChangedCallback;

    this.init = function (params) {
        this.caseId = params.caseId;
        this.templateId = params.templateId;
        this.customerId = params.customerId;
        this.isExtendedCase = params.isExtendedCase;
        this.getWorkFlowStepsUrl = params.getWorkFlowStepsUrl;
        this.selectStepText = params.selectStepText;
        this.saveText = params.saveText;

        this.subscribeUIEvents();

        this.loadWorkflowSteps();
    };

    this.loadWorkflowSteps = function () {
        var self = this;
        var data = {
            caseId: self.caseId,
            templateId: self.templateId
        };

        $('.workflowStepsLoader').show();
        var hasWorkflows = false;

        $.getJSON(self.getWorkFlowStepsUrl, $.param(data), function (res) {
            hasWorkflows = res.items && res.items.length > 0;
            if (hasWorkflows) {
                var options = [];
                if (res.items.length > 0) {
                    options.push('<option value="0">' + self.selectStepText + '</option>');
                }
                $.each(res.items, function (index, item) {
                    options.push('<option data-next-step="' + item.NextStep + '" value="' + item.CaseTemplateId + '">' + item.Name + '</option>');
                });

                var optionsHtml = options.join('');
                $("select.workflows-select").each(function () {
                    $(this).html(optionsHtml);
                });
            }

            //set up UI
            if (self.isExtendedCase) {
                self.setUIForExtendedCase(hasWorkflows);
            } else {
                self.setUIForNormalCase(hasWorkflows);
            }
        }).fail(function (xhr, textStatus, error) {
            var errMsg = error || 'Unknown error';
            ShowToastMessage(errMsg, "Error");
            console.error(errMsg);
        }).done(function() {
            $('.workflowStepsLoader').hide();
        });
    };

    this.setUIForNormalCase = function (hasWorkflows) {
        var self = this;
        if (self.caseId > 0) {
            
            // no save button, show only if hasWorkflows
            if (hasWorkflows) {
                $('div[id^="caseControlPanel"]').show();
                $(".workflowStepsPanel").show();
                $(".workflowStepsPanel2").hide();
            }
            else {
                console.log("no workflows");
                $(".workflowStepsPanel2").show();
                $(".workflowStepsPanel").hide();
            }
        } else {
            // new: save is avaialble, workflows visible only if exist
            $('div[id^="caseControlPanel"]').show();
            $(".save-button-div").show();
            if (hasWorkflows) {
                $(".save-button").val(self.saveText);
                $(".workflowStepsPanel").show();
                $(".workflowStepsPanel2").hide();
            }
            else {
                $(".workflowStepsPanel2").show();
                $(".workflowStepsPanel").hide();
            }
        }
    }

    this.setUIForExtendedCase = function (hasWorkflows) {
        var self = this;
        //save btn is available
        $('div[id^="caseControlPanel"]').show();
        $(".save-button-div").show();

        //show workflows view 
        if (hasWorkflows) {
            $(".save-button").val(self.saveText);
            $(".workflowStepsPanel").show();
            $(".workflowStepsPanel2").hide();
        }
        else {
            $(".workflowStepsPanel2").show();
            $(".workflowStepsPanel").hide();
        }
    }

    this.subscribeUIEvents = function () {
        var self = this;

        $("select.workflows-select").on('change', function () {
            self.handleWorkFlowStepChanged(this);
        });

    }

    this.handleWorkFlowStepChanged = function (el) {
        var that = el;
        var $el = $(el);
        var selectedVal = $el.val();

        $("select.workflows-select").each(function (item, index) {
            if (that !== this)
                $(this).val(selectedVal);
        });

        var nextStepNumber = 0;
        var $selectedStep = $el.find('option:selected'); // todo: same as val() ?
        if ($selectedStep && $selectedStep.length) {
            nextStepNumber = +$selectedStep.data('next-step');
        }

        if (self.stepChangedCallback)
            self.stepChangedCallback({ selectedVal: selectedVal, nextStepNumber: nextStepNumber });
    };

}