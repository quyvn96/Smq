(function () {
    angular.module('smq.orders', ['smq.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider.state('orders', {
            url: "/orders",
            parent: 'base',
            templateUrl: "/app/components/orders/orderListView.html",
            controller: "orderListController"
        });
    }
})();