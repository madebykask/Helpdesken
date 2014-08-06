function applyComputerShortInfoBehavior(parameters) {
    if (!parameters.url) throw new Error('url must be specified.');
    window.showComputerShortInfo = function (id) {

        if (!id) {
            return;
        }

        $.get(parameters.url, { computerId: id }, function (data) {
            $("#computer_short_info_dialog_container").html(data);
            $("#computer_short_info_select_div").parent().appendTo("#computer_short_info_dialog_container");
            $('#computer_short_info_select_div').dialog('open');
        });
    };
};