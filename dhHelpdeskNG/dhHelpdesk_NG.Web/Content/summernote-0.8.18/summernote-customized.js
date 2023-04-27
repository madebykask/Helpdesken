/**
 *
 * copyright [year] [your Business Name and/or Your Name].
 * email: your@email.com
 * license: Your chosen license, or link to a license file.
 *
 */
(function (factory) {
    /* Global define */
    if (typeof define === 'function' && define.amd) {
        // AMD. Register as an anonymous module.
        define(['jquery'], factory);
    } else if (typeof module === 'object' && module.exports) {
        // Node/CommonJS
        module.exports = factory(require('jquery'));
    } else {
        // Browser globals
        factory(window.jQuery);
    }
}(function ($) {

    $.extend(true, $.summernote.lang, {
        'en-US': { /* US English(Default Language) */
            imgZoomPlugin: {
                exampleText: 'Popup image',
                dialogTitle: 'Popup image',
                okButton: 'OK'
            }
        }
    });

    $.extend($.summernote.options, {
        imgZoomPlugin: {
            icon: '<i class="icon-zoom-in"></i>',
            tooltip: 'Popup image'
        }
    });



    $.extend($.summernote.plugins, {
        /**
         *  @param {Object} context - context object has status of editor.
         */
        'imgZoomPlugin': function (context) {

            var self = this,

                // ui has renders to build ui elements
                // for e.g. you can create a button with 'ui.button'
                ui = $.summernote.ui,
                $note = context.layoutInfo.note,

                // contentEditable element
                $editor = context.layoutInfo.editor,
                $editable = context.layoutInfo.editable,
                $toolbar = context.layoutInfo.toolbar,

                // options holds the Options Information from Summernote and what we extended above.
                options = context.options,

                // lang holds the Language Information from Summernote and what we extended above.
                lang = options.langInfo;

            context.memo('button.imgZoomPlugin', function () {

                // Here we create a button
                var button = ui.button({

                    // icon for button
                    contents: options.imgZoomPlugin.icon,

                    // tooltip for button
                    tooltip: lang.imgZoomPlugin.tooltip,

                    // Keep button from being disabled when in CodeView
                    codeviewKeepButton: true,

                    click: function (e) {
                        var $img = $($editable.data('target'));

                        $.magnificPopup.open({
                            type: 'image',
                            items: {
                                src: $img[0].src
                            },
                            callbacks: {
                            imageLoadComplete: changeImgSize
                        }
                        });

                    //context.invoke('imgZoomPlugin.show');
                    }
                });
                return button.render();
            });

            function changeImgSize() {

                var img = this.content.find('img');
                var imgHeight = img[0].height;

                if (imgHeight > screen.height - 300) {

                    $('.mfp-content').css({ 'margin-top': '3%', 'max-width': '70%' });
                }
            }

            ////// This events will be attached when editor is initialized.
            //this.events = {
            //    // This will be called after modules are initialized.
            //    'summernote.init': function (we, e) {
            //        console.log('summernote initialized', we, e);
            //    },
            //    // This will be called when user releases a key on editable.
            //    'summernote.keyup': function (we, e) {
            //        console.log('summernote keyup', we, e);
            //    }
            //};

            // This methods will be called when editor is destroyed by $('..').summernote('destroy');
            // You should remove elements on `initialize`.
            this.destroy = function () {
                this.$panel.remove();
                this.$panel = null;
            };
        }
    });
}));