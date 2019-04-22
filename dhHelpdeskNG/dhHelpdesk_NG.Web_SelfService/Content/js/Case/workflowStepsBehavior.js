function WorkflowStepsBehavior(stepChangedCallback) {

    this.stepChangedCallback = stepChangedCallback;

    this.init = function (params) {
        this.caseId = params.caseId;
        this.templateId = params.templateId;
        this.customerId = params.customerId;
        this.canSave = params.canSave;
        this.selectStepText = params.selectStepText;
        this.saveText = params.saveText;

        this.subscribeUIEvents();

        this.loadWorkflowSteps();
    };

    this.loadWorkflowSteps = function () {
        var self = this;
        var data = {
            caseId: self.caseId,
            templateId: self.templateId,
            customerId: self.customerId
        };

        $.getJSON('/Case/GetWorkflowSteps', $.param(data), function (res) {
            var hasWorkflows = false;
            if (res.items && res.items.length) {
                var options = [];
                options.push('<option value="0">' + self.selectStepText + '</option>');
                $.each(res.items, function (index, item) {
                    options.push('<option data-next-step="' + item.NextStep + '" value="' + item.CaseTemplateId + '">' + item.Name + '</option>');
                });

                var optionsHtml = options.join('');
                $("select.workflows-select").each(function () {
                    $(this).html(optionsHtml);
                });

                hasWorkflows = true;
                $(".workflowStepsPanel").show();

                if (self.canSave) {
                    $(".save-button").val(self.saveText);
                }
            }

            if (self.canSave || hasWorkflows) {
                $('div[id^="exCaseControlPanel"]').show();    
            }

        });
    };

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
            self.stepChangedCallback({ selectedVal, nextStepNumber });
    };

}