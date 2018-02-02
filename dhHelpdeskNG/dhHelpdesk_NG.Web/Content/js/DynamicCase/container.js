
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
    _this.modalTemplate = '<div class="modal fade" id="containerModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" data-backdrop="static" aria-hidden="true">' +
                             '  <div class="modal-header">' +
                             '  </div>' +
                             '  <div class="modal-body" id="dh-modal-form-area">' +
                             '  </div>' +
                             '  <div class="modal-footer" id="dh-modal-form-area-footer">' +
                             '      <button class="btn" data-dismiss="modal" aria-hidden="true">Close</button>' +
                             '  </div>' +
                             '</div>';

    
    // Used with iframeResizer.js:
    // This library enables the automatic resizing of the height and width of both same and cross domain iFrames to fit the contained content.
    // Place iframeResizer.contentWindow.min.js in the page contained within the iframe.
    _this.iframeOptions = {
        log: false,                                                         // Enable console logging
        sizeHeight: true,
        checkOrigin: false,                                                 // Not sure if this works or not, DL
        enablePublicMethods: true,                                          // Enable methods within iframe hosted page
        resizedCallback: function (messageData) {                           // Callback fn when resize is received     
        },
        bodyMargin: '0 0 200px 0',
        messageCallback: function (data) {                                  // Callback fn when message is received
            //data:{iframe,message}
            _this.processMessage(data);
        },
        closedCallback: function (id) {                                     // Callback fn when iFrame is closed
        }
    };

    (function () {
        if (_this._options.modal == 0) {
            $('#loadContainer').on('click', function (event) {
                event.preventDefault();
                var curState = $(this).attr("data-state");
                if (curState == "") {
                    _this.load({ url: _this._options.url });
                    $(this).attr("data-state", "loaded");
                }
            });
        } else if (_this._options.modal == 1) {
            $('#openContainer').on('click', function (event) {
                event.preventDefault();
                _this.loadModal({ url: _this._options.url });
            });

            $(document).on('hidden', '#' + _this._modalId, function () {
                _this.closeModal();
            });
        }
        else if (_this._options.modal == 2) {
            _this._formAreaId = 'dh-form-on-case-area';
            // _this._options.url = _this._options.url.substr(1);
            _this.load({ url: _this._options.url });
        }
    })();
};

dhform.prototype.processMessage = function(messageData) {
    "use strict";
    
    var message = messageData && messageData.message;
    
    //keep for compatibility in case eForms have not been updated yet with new msg format (ect.js)
    if (message === 'cancelCase') { 
        cancelCase();
        return;
    }

    if (message && message.type) {
        var msgType = message.type || '';

        if (msgType === 'cancelCase') {
            this.cancelCase();
        }

        if (msgType === 'refreshData') {
            // NOTE:you can use this handler to proces refresh data from eForm app
            // but its already handled in EditPage.prototype.refreshCasePage by checking IsCaseDataChanged session flag on server
        }
    }
};

dhform.prototype.cancelCase = function () {
    var elem = $('#case-action-close');
    location.href = elem.attr('href');
};


dhform.prototype.updateCaseStatus = function () {
    var caseId = $('#case__Id').val();
    
    $.getJSON('/Cases/GetStateSecondary/' + caseId)
        .done(function (res) {
            if (res && res.Id) {
                $("#case__StateSecondary_Id").val(res.Id);
                var subStateName$ = $("#subStateName");
                if (subStateName$.length)
                    subStateName$.val(res.StateSecondaryName);
            }
        }).fail(function (err) {
            console.error(err);
        });
}

dhform.prototype.progress = function () {
    "use strict";
    var _this = this;

    $('#loading').fadeOut(300, function () {
        $(this).remove();
        _this._formArea.find('iframe').removeClass('hidden2');
    });
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

    if (iframe.length !== 0) {
        iframe.remove();
    }

    $('<iframe id="test" class="hidden2" scrolling="no" frameBorder="0" width="100%" src="' + options.url + '"></iframe>').appendTo(_this._formArea);

    $('[id*=test]').load(function () {
        $('#test').iFrameResize(_this.iframeOptions);
        _this.progress();
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
    modal.css({
        'width': function () {
            return ($(document).width() * .9) + 'px';
        },
        'margin-left': function () {
            return -($(this).width() / 2);
        }
    });
    modal.modal("show");

    _this._formArea = $('#' + _this._modalFormAreaId);

    if (typeof _this._formArea === "undefined" || _this._formArea.length === 0) {
        // error!!!
        return;
    }

    _this._formArea.html(_this._loadingTemplate);

    var iframe = _this._formArea.next('iframe');

    if (iframe.length !== 0) {
        iframe.remove();
    }

    $('<iframe id="test" class="hidden2" scrolling="no" frameBorder="0" width="100%"src="' + options.url + '"></iframe>').appendTo(_this._formArea);

    $('[id*=test]').load(function () {
        $('#test').iFrameResize(_this.iframeOptions);
        _this.progress();
    });
};

dhform.prototype.closeModal = function () {
    "use strict";
    var _this = this;

    var modal = $('#' + _this._modalId);

    modal.modal("hide");

    window.scrollTo(0, 0);
    location.reload(true);

    //Not implemented
    //if (_this._isChanged === true) {
    //    location.reload(false);
    //}
};
