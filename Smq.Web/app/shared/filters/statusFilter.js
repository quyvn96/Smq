(function (app) {
    app.filter('statusFilter', function () {
        return function (input) {
            if (input == true)
                return 'Hiệu lực';
            else
                return 'Khóa';
        }
    });
})(angular.module('smq.common'));