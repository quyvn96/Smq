(function (app) {
    app.filter('statusFilter', function () {
        return function (input) {
            if (input == true)
                return 'Mở';
            else
                return 'Khóa';
        }
    });
})(angular.module('smq.common'));