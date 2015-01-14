﻿/*
Used with iframeResizer.js:
This library enables the automatic resizing of the height and width of both same and cross domain iFrames to fit the contained content.
Place iframeResizer.contentWindow.min.js in the page contained within the iframe.
*/

var iframeOptions = {
    log: false,                                                         // Enable console logging
    sizeHeight: true,
    checkOrigin: false,                                                 // Not sure if this works or not, DL
    enablePublicMethods: true,                                          // Enable methods within iframe hosted page
    resizedCallback: function (messageData) {                           // Callback fn when resize is received     
    },
    messageCallback: function (messageData) {                           // Callback fn when message is received

        if (messageData.message === 'cancelCase') {
            var elem = $('#case-action-close');
            location.href = elem.attr('href');
        }
    },
    closedCallback: function (id) {                                     // Callback fn when iFrame is closed
    }
};

var dhform = function (options) {
    "use strict";
    var _this = this;
    _this._isChanged = false;
    _this._options = options || {};
    _this._modalId = 'containerModal';
    _this._loadingTemplate = '<div id="loading" style="width:100%;height:100%;"><img src="/Content/icons/ajax-loader.gif" /></div>';
    _this._formArea;
    _this._formAreaId = 'dh-form-area';
    _this._modalFormAreaId = 'dh-modal-form-area';
    _this.modalTemplate = '<div class="modal fade full-screen" id="containerModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" data-backdrop="static" aria-hidden="true">' +
                             '  <div class="modal-header">' +
                             '  </div>' +
                             '  <div class="modal-body" id="dh-modal-form-area">' +
                             '  </div>' +
                             '  <div class="modal-footer" id="dh-modal-form-area-footer">' +
                             '      <button class="btn" data-dismiss="modal" aria-hidden="true">Close</button>' +
                             '  </div>' +
                             '</div>';

    (function () {
        if (_this._options.modal == false) {
            _this.load({ url: _this._options.url });
        } else {
            $("#openContainer").on("click", function (event) {
                if (_this._options.modal) {
                    event.preventDefault();
                    _this.loadModal({ url: _this._options.url });
                }
            });
        }
    })();
};

dhform.prototype.load = function (options) {
    "use strict";
    var _this = this;

    _this._formArea = $('#' + _this._formAreaId);

    _this._formArea.html(_this._loadingTemplate);

    if (typeof _this._formArea === "undefined" || _this._formArea.length === 0) {
        // error!!!
        return;
    }

    var iframe = _this._formArea.next('iframe');

    if (typeof iframe === "undefined" || iframe.length !== 0) {
        iframe.remove();
    }

    $('<iframe id="test" class="hidden2" scrolling="no" frameBorder="0" width="100%" src="' + options.url + '"></iframe>').appendTo(_this._formArea);

    $('[id*=test]').load(function () {
        $('#test').iFrameResize(iframeOptions);
        _this.progress();
    });
};

dhform.prototype.progress = function () {
    "use strict";
    var _this = this;

    $('#loading').fadeOut(300, function () {
        $(this).remove();
        _this._formArea.find('iframe').removeClass('hidden2');
    });
};

dhform.prototype.loadModal = function (options) {
    "use strict";
    var _this = this;

    var modal = $('#' + _this._modalId);

    if (typeof modal === "undefined" || modal.length !== 0) {
        modal.modal('hide');
        modal.remove();
    }

    $(_this.modalTemplate).appendTo(document.body);

    modal = $('#' + _this._modalId);
    modal.modal("show");

    _this._formArea = $('#' + _this._modalFormAreaId);

    if (typeof _this._formArea === "undefined" || _this._formArea.length === 0) {
        // error!!!
        return;
    }

    _this._formArea.html(_this._loadingTemplate);

    var iframe = _this._formArea.next('iframe');

    if (typeof iframe === "undefined" || iframe.length !== 0) {
        iframe.remove();
    }

    $('<iframe id="test" class="hidden2" scrolling="no" frameBorder="0" width="100%"src="' + options.url + '"></iframe>').appendTo(_this._formArea);

    $('[id*=test]').load(function () {
        $('#test').iFrameResize(iframeOptions);
        _this.progress();
    });
};

dhform.prototype.closeModal = function (options) {
    "use strict";
    var _this = this;

    var modal = $('#' + _this._modalId);

    modal.modal("hide");

    if (_this._isChanged === true) {
        location.reload(false);
    }
};