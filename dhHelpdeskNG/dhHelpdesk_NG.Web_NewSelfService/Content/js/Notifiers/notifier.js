function applyPageBehavior(parameters) {
    $('#region_dropdown').change(function() {
        $.get(parameters.departmentDropDownUrl, { regionId: $(this).val() }, function(departmentDropDownMarkup) {
            $('#department_dropdown').html(departmentDropDownMarkup);
        });
    });

    if (parameters.showManager) {
        (function() {
            $('#manager_textbox').typeahead({
                source: function(query, process) {
                    this.displayNames = [];
                    this.mappings = [];
                    var self = this;

                    $.each(parameters.managers, function(i, manager) {
                        self.mappings[manager.displayName] = manager;
                        self.displayNames.push(manager.displayName);
                    });

                    process(this.displayNames);
                },
                updater: function(item) {
                    var managerId = this.mappings[item].id;
                    $('#manager_id_hidden').val(managerId);
                    return item;
                }
            });
        })();
    }
}