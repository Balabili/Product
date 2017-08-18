(function () {
    var self = this;
    self.utility = {};
    self.utility.ajax = function (actionUrl, type, data, successCallBack, errorCallBack) {
        $.ajax({
            url: actionUrl,
            type: type,
            data: data,
            success: successCallBack,
            error: errorCallBack,
        });
    }
    return self;
})()