
// <div id="dhform-area"></div>

var dhform = function (options) {
    "use strict";
    var _this = this;
    _this._isChanged = false;
    _this._options = options || {};
    _this._id = 'containerModal';
    _this._formAreaId = 'dh-form-area';

    _this.modalTemplate = '<div class="modal fade" id="containerModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" data-backdrop="static" aria-hidden="true">' +
                             '  <div class="modal-header">' +
                             '      <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>' +
                             '      <h3>Container header</h3>' +
                             '  </div>' +
                             '  <div class="modal-body" id="dh-form-area">' +
                             '  </div>' +
                             '  <div class="modal-footer">' +
                             '      <h3>Container footer</h3>' +
                             '      <button class="btn" data-dismiss="modal" aria-hidden="true">Close</button>' +
                             '  </div>' +
                             '</div>';

    (function () {
        $("#openContainer").on("click", function (event) {
            if (_this._options.modal) {
                event.preventDefault();
                _this.loadModal({ url: _this._options.url + "/" });
            } else {
                _this.load({ url: _this._options.url + "/" });
            }
        });
    })();
};

dhform.prototype.load = function (options) {
    "use strict";
    var _this = this;

    var formArea = $('#' + _this._formAreaId);

    if (typeof formArea === "undefined" || formArea.length === 0) {
        // error!!!

        return;
    }

    var iframe = formArea.next('iframe');


    if (typeof iframe === "undefined" || iframe.length !== 0) {
        iframe.remove();
    }

    $('<iframe src="' + options.url + '"></iframe>').appendTo(formArea);

};

dhform.prototype.loadModal = function (options) {
    "use strict";
    var _this = this;

    var modal = $('#' + _this._id);

    if (typeof modal === "undefined" || modal.length !== 0) {
        modal.modal('hide');
        modal.remove();
    }

    $(_this.modalTemplate).appendTo(document.body);
    modal = $('#' + _this._id);
    modal.modal("show");

    var formArea = $('#' + _this._formAreaId);

    if (typeof formArea === "undefined" || formArea.length === 0) {
        // error!!!

        return;
    }

    $('<iframe src="' + options.url + '"></iframe>').appendTo(formArea);
};

dhform.prototype.closeModal = function (options) {
    "use strict";
    var _this = this;

    var $modal = $('#' + _this._id);

    $modal.modal("hide");

    if (_this._isChanged === true) {
        location.reload(false);
    }
};