
$(function () {
    (function ($) {
        window.selfService = window.selfService || {};
        window.selfService.faq = window.selfService.faq || {};
        var params  = window.params;

        var $normalTreeNode = $('.faq-node');
        var $expandableTreeNode = $('.faq-expand');
        var $collapsableTreeNode = $('.faq-collapse');
        var faqRowsPlace = '#accordion';
        var defaultCategoryId = 0;
        var answerCaption = '';
        var internalAnswerCaption = '';
        var urlCaption = '';
        var fontBoldClass;
        var downloadFileUrl = '';
        var baseFilePath = '';
        var $faqSearchText = $('#faq-Search');
        var $faqSearchButton = $('#btn-faq-Search');

        var hierarchyData = [];

        selfService.faq.init = function () {      
            fontBoldClass = 'faq-font-bold';                        
            hierarchyData = params.hierarchyData || [];
            answerCaption = params.answerCaption ||'';
            internalAnswerCaption = params.internalAnswerCaption || '';
            urlCaption = params.URLCaption || '';

            downloadFileUrl = params.downloadFileUrl || '';            

            if (hierarchyData.length > 0)            
                defaultCategoryId = hierarchyData[0].Id;

            if (defaultCategoryId > 0)
                this.selectNode($('#node-' + defaultCategoryId));            
        };
           
        $faqSearchButton.click(function () {
            var phrase = $faqSearchText.val();
            if (phrase != '')
                selfService.faq.searchFor(phrase.toLowerCase());
        });

        $faqSearchText.keypress(function (e) {
            if (e.which == 13) {
                $faqSearchButton.click();
            }
        });

        $normalTreeNode.click(function () {            
            selfService.faq.selectNode($(this));
        });

        $expandableTreeNode.click(function () {
            selfService.faq.expandNode(this);
        });

        $collapsableTreeNode.click(function () {
            selfService.faq.collapseNode(this);
        });

        selfService.faq.searchFor = function (phrase) {
            clearFaqs();
            searchFaqs(phrase);
        };


        selfService.faq.expandNode = function (node) {            
            $(node).toggle();
            $(node).next().toggle();                       
            $(node).parent().parent().children().last().toggle();

            selfService.faq.selectNode($(node).next());
        };

        selfService.faq.collapseNode = function (node) {            
            $(node).toggle();
            $(node).prev().toggle();            
            $(node).parent().parent().children().last().toggle();

            selfService.faq.selectNode($(node).prev());
        };
               
        selfService.faq.selectNode = function (node) {
            var selecteds = document.getElementsByClassName(fontBoldClass);
            for (var i = 0; i < selecteds.length; i++) {
                $(selecteds[i]).removeClass(fontBoldClass);
            }
            
            $(node).addClass(fontBoldClass);
            clearFaqs();
            $faqSearchText.val('');
            var catId = $(node).attr('id');
            loadFaqList(catId);
        }
       
        var searchFaqs = function (phrase) {
            if (hierarchyData == undefined || hierarchyData == null)
                return;

            var faqs = getFaqsByPhrase(hierarchyData, phrase);
            if (faqs == undefined || faqs == null)
                return;

            drawFaqRows(faqs);
        }

        var getFaqsByPhrase = function (dataToSearch, phrase) {
            if (dataToSearch == undefined || dataToSearch == null)
                return;

            var ret = [];
            for (var i = 0; i < dataToSearch.length; i++) {
                var data = dataToSearch[i];
                if (data.FaqRows != null && data.FaqRows.length > 0) {
                    for (var j = 0; j < data.FaqRows.length; j++) {
                        var faq = data.FaqRows[j];
                        if (faq.Question.toLowerCase().indexOf(phrase) >= 0 ||
                            faq.Answer.toLowerCase().indexOf(phrase) >= 0 ||
                            faq.InternalAnswer.toLowerCase().indexOf(phrase) >= 0)
                            ret.push(faq);
                    }
                }
                if (data.SubCategories != null && data.SubCategories.length > 0) {
                    var subFaqs = getFaqsByPhrase(data.SubCategories, phrase);
                    if (subFaqs != null && subFaqs.length > 0) {
                        for (var j = 0; j < subFaqs.length; j++) {
                            ret.push(subFaqs[j]);
                        }
                    }
                }
            }
            return ret;
        }

        var loadFaqList = function (categoryId) {
            if (hierarchyData == undefined || hierarchyData == null)
                return;

            var faqs = getFaqsForCategory(hierarchyData, categoryId);
            if (faqs == undefined || faqs == null)
                return;

            drawFaqRows(faqs);
        }

        var getFaqsForCategory = function (dataToSearch, categoryId) {
            if (dataToSearch == undefined || dataToSearch == null)
                return;

            for (var i = 0; i < dataToSearch.length; i++) {
                var data = dataToSearch[i];
                if ('node-' + data.Id == categoryId)
                {
                    return data.FaqRows;
                }
                if (data.SubCategories != null && data.SubCategories.length > 0)
                {
                    var subFaqs = getFaqsForCategory(data.SubCategories, categoryId);
                    if (subFaqs != null && subFaqs.length>0)
                    {
                        return subFaqs;
                    }                    
                }
            }
            return null;
        }

        var clearFaqs = function() {
            $(faqRowsPlace).empty();
        }

        var drawFaqRows = function (faqs) {
            if (faqs == undefined || faqs == null)
                return;
                        
            for (var i = 0; i < faqs.length; i++) {
                var faq = faqs[i];                

                if (faq.Question != '' || faq.Answer != '') {
                    var faqDetails = dateToDisplayDate(faq.CreatedDate) + '<br /><br />' +
                                    '<b>' + answerCaption + '</b><br />' + faq.Answer + '<br /><br />';
                    //                                    '<b>' + internalAnswerCaption + '</b><br/>' + faq.InternalAnswer.replace(/\</g, "").replace(/\</g, "") + '<br /><br />' +
                    if (faq.Url1) {
                        var url1 = faq.Url1;
                        if (url1.indexOf("http://") < 0 && url1.indexOf("https://") < 0) {
                            url1 = "http://" + url1;
                        }
                        faqDetails = faqDetails + '<b>' + urlCaption + '</b><br/>' + '<a href="' + url1 + '">' + faq.Url1 + '<a/>' + '<br />';
                    }
                    if (faq.Url2) {
                        var url2 = faq.Url2;
                        if (url2.indexOf("http://") < 0 && url2.indexOf("https://") < 0) {
                            url2 = "http://" + url2;
                        }
                        faqDetails = faqDetails + '<b>' + urlCaption + '</b><br/>' + '<a href="' + url2 + '">' + faq.Url2 + '<a/>' + '<br /><br />';
                    }

                    faqDetails = faqDetails.replace(/(?:\r\n|\r|\n)/g, '<br />');

                    var faqQuestion = faq.Question != '' ? faq.Question : "-";
                    faqQuestion = faqQuestion.replace(/\</g, "").replace(/\</g, "");

                    var faqFiles = '';
                    if (faq.Files != null && faq.Files.length > 0) {
                        for (var j = 0; j < faq.Files.length; j++) {
                            var file = faq.Files[j];                            
                            var downloadLink = downloadFileUrl + '?faqId=' + faq.Id + '&fileName=' + file.FileName;
                            faqFiles += '<a href="' + downloadLink + '" style="cursor: pointer;">' +
                                        '<span class="glyphicon glyphicon-file"></span>&nbsp;' + file.FileName + '</a>';
                            faqFiles += '<br />';
                        }
                    }

                    var row = '';
                    row += '<div class="panel panel-default">';
                    row += '<div class="panel-heading faqph" role="tab" id="heading-"' + faq.Id + '>';
                    row += '<h5 class="panel-title">';
                    row += '<a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapse-'+ faq.Id + '" aria-expanded="true" aria-controls="collapseOne">';
                    row += faqQuestion;
                    row += '</a>';
                    row += '</h5>';
                    row += '</div>';
                    row += '<div id="collapse-' + faq.Id + '" class="panel-collapse collapse" role="tabpanel" aria-labelledby="heading-"' + faq.Id + '">';
                    row += '<div class="panel-body">';
                    row += faqDetails;
                    row += '</br>';
                    row += faqFiles;
                    row += '</div>';
                    row += '</div>';
                    row += '</div>';
                    
                    $(faqRowsPlace).append(row);
                }
            }
            
        }

        var dateToDisplayDate = function (date) {
            var displayDate = "";
            if (date != null) {
                var _date = new Date(parseFloat(date.substr(6)));
                displayDate = _date.getFullYear() + "-" +
                              padLeft((_date.getMonth() + 1), 2, '0') + "-" +
                              padLeft(_date.getDate(), 2, '0');
            }
            return displayDate;
        }

         var padLeft = function (value, totalLength, padChar) {
            var valLen = value.toString().length;
            var diff = totalLength - valLen;
            if (diff > 0) {
                for (i = 0; i < diff; i++)
                    value = padChar + value;
            }
            return value;
         }
                     
    })($);
       
    selfService.faq.init();
});