function LoadFeedbackEdit(parameters, translations) {
    "use strict";

    var selectedOptionId;
    var selectedOptionImg;
    var hiddenImgElement;

    $("#languageList").on("change", function() {
                var selectedItem = $(this).val();
                window.location.href = parameters.FeedbackEditUrl +
                    "?feedbackId=" +
                    parameters.QuestionnaireId +
                    "&languageId=" +
                    selectedItem;
            });

    $("#Addbtn").on("click", function(event) {
                var optionPos = $("#optionPos").val();
                var optionText = $("#optionText").val();
                var optionValue = $("#optionValue").val();
                var optionIcon = $("#optionIcon").val();

                if (optionPos < 0 || optionPos === "") {
                    ShowToastMessage("Invalid Position !!! ", "error");
                    return;
                }

                if (optionText === "") {
                    ShowToastMessage("Invalid Alternative !!! ", "error");
                    return;
                }

                if (optionValue < 0 || optionValue === "") {
                    ShowToastMessage("Invalid Value !!!", "error");
                    return;
                }

                var qOptions = $(".question_option");
                if (qOptions.length > 0) {
                    for (var i = 0; i < qOptions.length; i++) {
                        if (optionValue !== '0' && $(qOptions[i]).val().trim() === optionValue.trim()) {
                            ShowToastMessage(translations.OptionUnique, "error");
                            event.preventDefault();
                            return;
                        }
                    }
                }

                var imgSource = "";
                if (selectedOptionImg)
                    imgSource = selectedOptionImg.attr("src").split(",")[1];

                $.ajax({
                    type: "POST",
                    url: parameters.AddOptionUrl,
                    data: {
                        feedbackId: parameters.FeedbcakId,
                        questionId: parameters.QuestionId,
                        languageId: parameters.LanguageId,
                        optionPos: optionPos,
                        optionText: optionText,
                        optionValue: optionValue,
                        optionIcon: optionIcon,
                        IconSrc: imgSource
                    },
                    success: function(result) {
                        if (result && result.success) {
                            window.location.href = parameters.FeedbackEditUrl +
                                '?feedbackId=' +
                                result.FeedbackId +
                                '&languageId=' +
                                result.LanguageId;
                        }
                    }
                });
            });

    function formatIcon(icon) {
        if (!icon.id) {
            return icon.text;
        }
        if (icon.id === parameters.FeedBackIconId) {
            return $('<span class="load_image">' + icon.text + '</span>');
        }
        var $icon = $(
            '<span><img src="' + parameters.UrlContentImage + icon.element.value.toLowerCase() + '" class="" /> </span>'
        );
        return $icon;
    };

    function formatSelectedIcon(data) {
        if (data.id === parameters.FeedBackIconId) {
            return $('<img class="loaded-image" />');
        }
        return $(
            '<span><img src="' + parameters.UrlContentImage + data.element.value.toLowerCase() + '" class="" /> </span>'
        );
    }

    $("#feedback_edit_form").on("submit", function(event) {
                function unique(array) {
                    var resArray = [];
                    $.each(array,
                        function(i, item) {
                            if ($.inArray(item, resArray) === -1 || item === '0') {
                                resArray.push(item);
                            }
                        });
                    return resArray;
                }

                var qOptions = $(".question_option");
                if (qOptions.length > 0) {
                    var initialLength = qOptions.length;
                    var qOptionsVal = qOptions.map(function() {
                        return $(this).val();
                    });
                    var uniqueLength = unique(qOptionsVal).length;
                    if (uniqueLength !== initialLength) {
                        event.preventDefault();
                        event.stopImmediatePropagation();
                        ShowToastMessage(translations.OptionUnique, "error");
                        return false;
                    }
                }
                return true;
            });

    $(".question_icons").select2({
            templateResult: formatIcon,
            templateSelection: formatSelectedIcon,
            width: '85%',
            minimumResultsForSearch: Infinity // hides search box
        });

    $(".question_icons").on("select2:closing", function(e) {
                if ($(this).val() === parameters.FeedBackIconId) {
                    $("#load_emoj_file").click();
                    var parent = $(this).parent();
                    var imgElement = parent.find("img.loaded-image");
                    hiddenImgElement = parent.find("input.iconSrc");
                    selectedOptionImg = imgElement;
                    var optionId = parent.parent().find(".option-id");
                    selectedOptionId = optionId.val();
                }
    });

    $("#load_emoj_file").on("change", function (event) {

        var target = event.target || event.srcElement;
        if (target.value.length === 0) {
            if (selectedOptionImg)
                selectedOptionImg.attr('src', "");
        }

                if (this.files && this.files[0]) {
                    if (this.files[0].size > 2048) {
                        ShowToastMessage(translations.MaxSize, "error");
                        return;
                    }
                    var reader = new FileReader();
                    reader.onload = function(e) {
                        if (selectedOptionId) {
                            $.ajax({
                                type: "POST",
                                url: parameters.UrlSaveIcon,
                                data: {
                                    OptionId: selectedOptionId,
                                    Src: e.target.result.split(",")[1]
                                },
                                success: function(result) {
                                    if (result && result.success) {
                                        selectedOptionImg.attr('src', e.target.result);
                                        selectedOptionImg.width(24);
                                        selectedOptionImg.height(24);
                                        hiddenImgElement.val(e.target.result);
                                    }
                                }
                            });
                        } else {
                            selectedOptionImg.attr('src', e.target.result);
                            selectedOptionImg.width(24);
                            selectedOptionImg.height(24);
                            hiddenImgElement.val(e.target.result);
                        }
                    }
                    reader.readAsDataURL(this.files[0]);
                }
            });

    $(document).ready(function() {
            var iconSources = $(".iconSrc");
            iconSources.each(function(index) {
                var src = $(this).val();
                if (src) {
                    var img = $(this).parent().find(".loaded-image");
                    img.attr('src', src);
                    img.width(24);
                    img.height(24);
                }
            });
        });
}