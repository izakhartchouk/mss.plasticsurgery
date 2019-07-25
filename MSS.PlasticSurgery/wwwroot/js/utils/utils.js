(function ($) {
    $.fn.serializeToObject = function () {
        var object = {};
        var array = this.serializeArray();

        $.each(array, function () {
            if (object[this.name]) {
                if (!object[this.name].push) {
                    object[this.name] = [object[this.name]];
                }

                object[this.name].push(this.value || '');
            } else {
                object[this.name] = this.value || '';
            }
        });

        return object;
    };
})(jQuery);