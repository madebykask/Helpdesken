function InvoiceFiles(options) {
    "use strict";

    this.options = $.extend({}, this.getDefaults(), options);
    this.init();

}

InvoiceFiles.prototype = {
    init: function () {
        "use strict";
        var self = this;

        $("#tree > ul > li > span").on("click", function () {
            self._expandYear(this);
        });

        $("#tree > ul > li > ul > li > span").on("click", function () {
            self._expandMonth(this);
        });
    },

    getDefaults: function () {
        "use strict";

        return {

        };
    },

    _expandYear: function(el) {
        "use strict";
        var self = this;

        $("#tree > ul > li > ul").hide();
        $("#tree li").removeClass("selected");
        $(el).siblings("ul").show();
        $(el).parent().addClass("selected");

        self._clearFiles();
    },

    _expandMonth: function(el) {
        "use strict";
        var self = this;

        var li = $(el).parent();

        li.siblings().removeClass("selected");
        li.addClass("selected");

        var year = li.data("year").toString();
        var month = li.data("month").toString();

        self._showFiles(year, month);
    },

    _showFiles: function (year, month) {
        "use strict";
        var self = this;

        var files = self.options.data[year][month];
        var res = "";

        for (var i = 0; i < files.length; i++) {
            res += "<a href='" + self.options.fileUrl + "/" + files[i].Guid + "' target='_blank'>" + files[i].Name + "</a><br/>";
        }

        $("#fileList").html(res);
    },

    _clearFiles: function() {
        "use strict";
        var self = this;

        $("#fileList").html("");
    }
}