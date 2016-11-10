
$(function () {
    (function ($) {

        window.selfService = window.selfService || {};
        window.selfService.faq = window.selfService.faq || {};

        var $normalTreeNode = $('.faq-node');
        var $expandableTreeNode = $('.faq-expand');
        var $collapsableTreeNode = $('.faq-collapse');
        
        var fontBoldClass;

        selfService.faq.init = function () {      
            fontBoldClass = 'font-bold';
        };
                
        $normalTreeNode.click(function () {            
            selfService.faq.selectNode($(this));
        });

        $expandableTreeNode.click(function () {
            selfService.faq.expandNode(this);
        });

        $collapsableTreeNode.click(function () {
            selfService.faq.collapseNode(this);
        });

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
        }
       
        selfService.faq.loadFaqList = function (id) {
                    
        }

    })($);
       
    selfService.faq.init();
});