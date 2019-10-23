function InitInventoryUserSearch() {

    $('#user_dialog_textbox').typeahead(getComputerUserInventorySearchOptions());

    function generateRandomKey() {
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
                .toString(16)
                .substring(1);
        }

        return s4() + '-' + s4() + '-' + s4();
    }

    function getComputerUserInventorySearchOptions() {
        var options = {
            items: 20,
            minLength: 2,
            source: function(query, process) {
                var lastInitiatorSearchKey = generateRandomKey();
                return $.ajax({
                    url: '/workstation/SearchComputerUsers',
                    type: 'post',
                    data: { query: query, searchKey: lastInitiatorSearchKey },
                    dataType: 'json',
                    success: function(result) {
                        if (result.searchKey !== lastInitiatorSearchKey)
                            return;

                        var resultList = jQuery.map(result.result,
                            function(item) {
                                var aItem = {
                                    id: item.Id,
                                    num: item.UserId,
                                    name: item.FirstName + ' ' + item.SurName,
                                    fname: item.FirstName,
                                    sname: item.SurName,
                                    email: item.Email,
                                    phone: item.Phone,
                                    mobilephone: item.MobilePhone,
                                    regionname: item.Region,
                                    departmentname: item.Department,
                                    unit: item.Unit,
                                    name_family: item.SurName + ' ' + item.FirstName
                                };
                                return JSON.stringify(aItem);

                            });

                        return process(resultList);
                    }
                });
            },

            matcher: function(obj) {
                var item = JSON.parse(obj);
                return ~item.name.toLowerCase().indexOf(this.query.toLowerCase()) ||
                    ~item.name_family.toLowerCase().indexOf(this.query.toLowerCase()) ||
                    ~item.num.toLowerCase().indexOf(this.query.toLowerCase()) ||
                    ~item.phone.toLowerCase().indexOf(this.query.toLowerCase()) ||
                    ~item.email.toLowerCase().indexOf(this.query.toLowerCase());
            },

            sorter: function(items) {
                var beginswith = [], caseSensitive = [], caseInsensitive = [], item;
                while (aItem = items.shift()) {
                    var item = JSON.parse(aItem);
                    if (!item.num
                        .toLowerCase()
                        .indexOf(this.query.toLowerCase())) beginswith.push(JSON.stringify(item));
                    else if (~item.num.indexOf(this.query)) caseSensitive.push(JSON.stringify(item));
                    else caseInsensitive.push(JSON.stringify(item));
                }

                return beginswith.concat(caseSensitive, caseInsensitive);
            },

            highlighter: function(obj) {
                var item = JSON.parse(obj);
                var orgQuery = this.query;
                if (item.departmentname == null)
                    item.departmentname = "";
                if (item.regionname == null)
                    item.regionname = "";
                var query = this.query.replace(/[\-\[\]{}()*+?.,\\\^$|#\s]/g, '\\$&');
                var result = item.name + ' - ' + item.num + ' - ' + item.phone + ' - ' + item.email + ' - ' + item.departmentname;
                var resultBy_NameFamily = item.name_family + ' - ' + item.num + ' - ' + item.phone + ' - ' + item.email + ' - ' + item.departmentname;

                if (result.toLowerCase().indexOf(orgQuery.toLowerCase()) > -1)
                    return result.replace(new RegExp('(' + query + ')', 'ig'),
                        function($1, match) {
                            return '<strong>' + match + '</strong>';
                        });
                else
                    return resultBy_NameFamily.replace(new RegExp('(' + query + ')', 'ig'),
                        function($1, match) {
                            return '<strong>' + match + '</strong>';
                        });

            },

            updater: function(obj) {
                var item = JSON.parse(obj);

                $('#user_dialog_textbox').val(item.num);
                $('#user_dialog_userId').val(item.id);

                if (item.fname !== "" && item.fname != null)
                    $('#firstname_dialog_td').text(item.fname);
                else
                    $('#firstname_dialog_td').text("");

                if (item.sname !== "" && item.sname != null)
                    $('#surname_dialog_td').text(item.sname);
                else
                    $('#surname_dialog_td').text("");

                if (item.regionname != null)
                    $('#region_dialog_td').text(item.regionname);
                else
                    $('#region_dialog_td').text("");

                if (item.departmentname != null)
                    $('#department_dialog_td').text(item.departmentname);
                else
                    $('#department_dialog_td').text("");

                if (item.unit != null)
                    $('#unit_dialog_td').text(item.unit);
                else
                    $('#unit_dialog_td').text("");

                return item.num;
            }
        };

        return options;
    }
}