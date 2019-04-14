var SORT_ASC = 0;
var SORT_DESC = 1;
var SortAscClass = "icon-chevron-up";
var SortDescClass = "icon-chevron-down";

window.fp = (function($) {

    //ctor
    function FaqsPage() {

        this.selectedCategoryId = '';
        this.sortBy = '';
        this.sortOrder = '';
        this.urls = null;

        this.init = function(params) {
            var self = this;

            //init from params
            this.sortBy = params.sortBy;
            this.sortOrder = params.sortOrder;
            this.urls = params.urls;
            this.languageId = params.languageId;

            //find controls
            this.$faqsTable = $('#faqs_table');
            this.$searchResultsTable = $('#search_results_table');
            this.$searchField = $('#search_field');
            this.$categoriesTree = $('#categories_tree');
            this.$loading = $('.loading-msg');

            //subscribe UI events
            this.subscribeEvents();

            //check
            this.selectedCategoryId = +this.findSelectedCategory();
            if (!isNaN(this.selectedCategoryId) && this.selectedCategoryId > 0) {
                this.сhangeCat(this.selectedCategoryId);
            } else if (params.selectedCategoryId) {
                this.selectedCategoryId = +params.selectedCategoryId;
            }

            if (isNaN(this.selectedCategoryId) || !this.selectedCategoryId) {
                return;
            }

            // load faqs only if category set
            this.loadFaqs();
        };

        //SUBSCRIBE EVENTS
        this.subscribeEvents = function() {
            var self = this;

            $('#tab_1').click(function () {
                if ($(this).hasClass('disabled'))
                    return false;
                $('#add_category_button').hide();
                $('#add_faq_button').show();
            });

            $('#tab_2').click(function () {
                if ($(this).hasClass('disabled'))
                    return false;
                $('#add_category_button').hide();
                $('#add_faq_button').show();
            });

            $('#tab_3').click(function () {
                if ($(this).hasClass('disabled'))
                    return false;
                $('#add_faq_button').hide();
                $('#add_category_button').show();
            });

            $('#show_all_answers_within_category').on('switchChange.bootstrapSwitch', function (event, state) {
                if (!isNaN(self.selectedCategoryId) && self.selectedCategoryId) {
                    self.loadFaqs();
                }
            });

            $('#categories_tree').find('a.tree-node, a.tree-selected-node').click(function () {
                var categoryId = $(this).siblings('input').val();
                self.сhangeCat(+categoryId);
                self.loadFaqs();
            });

            $('#searchBtn').click(function() {
                self.searchFaqs();
            });

            $('#add_faq_button').click(function () {
                if ($(this).find('.btn').hasClass('disabled'))
                    return;
                window.location.href = self.urls.newFaq +  '?categoryId=' + self.selectedCategoryId;
            });

            $('#show_all_answers_within_category_search_checkbox').on('switchChange.bootstrapSwitch', function () {
                var searchText = self.$searchField.val() || '';
                if (searchText.length === 0) {
                    return;
                }
                self.searchFaqs();
            });

            $('#categories_editing_tree').find('a.tree-node, a.tree-selected-node').click(function () {
                var selectedCategoryId = $(this).siblings('input').val();
                window.location.href = self.urls.editCategory + '?id=' + selectedCategoryId + '&languageId=' + self.languageId;
            });

            this.$faqsTable.find('th').each(function(index, el) {
                $(el).click(function(el2) {
                    self.sortFaqs(el2);
                });
            });

            this.$searchResultsTable.find('th').each(function(index, el) {
                $(el).click(function(el2) {
                    self.sortSearchResults(el2);
                });
            });
        };

        this.loadFaqs = function() {
            var self = this;

            var data = {
                categoryId: self.selectedCategoryId,
                sortBy: self.sortBy,
                sortOrder: self.sortOrder,
                now: Date.now()
            };

            var url =
                self.isShowAllAnswersChecked()
                    ? self.urls.getFaqsDetailed
                    : self.urls.getFaqs;

            var $tbody = self.$faqsTable.find('tbody');
            $tbody.html('');

            self.showProgress();

            $.get(url, $.param(data)).done(function(res) {
                $tbody.append(res);
            }).fail(function(err) {
                console.error(err);
            }).always(function() {
                self.hideProgress();
            });
        }

        this.searchFaqs = function() {
            var self = this;
            var txt = this.$searchField.val() || '';
            if (txt.length === 0) {
                return;
            }
                
            var data = {
                pharse: txt.trim(),
                sortBy: self.sortBy,
                sortOrder: self.sortOrder,
                now: Date.now()
            };

            var url =
                self.isShowAllAnswersInSearchChecked()
                    ? self.urls.searchDetailed
                    : self.urls.search;


            var $tbody = self.$searchResultsTable.find('tbody');
            $tbody.html('');

            self.showProgress();

            $.get(url, $.param(data)).done(function(res) {
                $tbody.append(res);
            }).fail(function(err) {
                //todo: error handling
                console.error(err);
            }).always(function() {
                self.hideProgress();
            });
        };

        this.hideProgress = function() {
            this.$loading.hide();
        };

        this.showProgress = function() {
            this.$loading.show();
        };

        this.sortFaqs = function(el) {
            var $el = $(el.target);
            var field = $el.data('field');
            this.setSortClass(field, $el);
            this.loadFaqs();
        };

        this.sortSearchResults = function(el) {
            var $el = $(el.target);
            var field = $el.data('field');
            this.setSortClass(field, $el);
            this.searchFaqs();
        };

        this.setSortClass = function(fieldName, $el) {
            var self = this;
                
            if (fieldName && fieldName === self.sortBy) {
                self.sortOrder = self.sortOrder === SORT_DESC ? SORT_ASC : SORT_DESC;
            } else {
                self.sortBy = fieldName;
                self.sortOrder = SORT_ASC;
            }

            //reset prev classes
            $el.parent().find('i').each(function(index, el) {
                el.className = '';
            });

            var sortCls = +self.sortOrder === SORT_ASC ? SortAscClass : SortDescClass;
            $el.children("i").addClass(sortCls);
        };
          
        this.findSelectedCategory = function() {
            var selected = this.$categoriesTree.find('a.tree-selected-node');
            if (selected && selected.length) {
                var categoryId = +selected.siblings('input').val();
                return categoryId || 0;
            }
            return null;
        }

        this.сhangeCat = function(categoryId) {
            this.selectedCategoryId = categoryId;
            $.post(this.urls.setSelectedCategory, $.param({ value: categoryId }), function(data) {});
        }

        this.isShowAllAnswersChecked = function() {
            return $('#show_all_answers_within_category').is(':checked');
        }

        this.isShowAllAnswersInSearchChecked = function() {
            return $('#show_all_answers_within_category_search_checkbox').is(':checked');
        }
    }
    return new FaqsPage();
})(jQuery);


