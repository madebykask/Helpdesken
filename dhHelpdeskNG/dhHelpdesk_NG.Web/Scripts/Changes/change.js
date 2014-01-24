function onReady(data) {
    $('#save_and_close_button').click(function () {
        $('#change_form').submit();
        window.location.href = data.afterDeleteUrl;
    });

    $('#delete_button').click(function () {
        $.post(data.deleteUrl, { id: data.id });
    });
}