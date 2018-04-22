/// <reference path="~/Assets/admin/libs/angular/angular.js" />

(function (app) {
    app.controller('orderListController', orderListController);
    orderListController.$inject = ['$scope', 'apiService', 'notificationService'];
    function orderListController($scope, apiService, notificationService) {
        $scope.orders = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.getOrders = getOrders;
        $scope.keyword = '';
        $scope.search = search;

        function getOrders(page) {
            page = page || 0;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: 10
                }
            }
            apiService.get('/api/order/getall', config, function (result) {
                if (result.data.TotalCount == 0) {
                    notificationService.displayWarning("No Record");
                }
                else {
                    notificationService.displaySuccess('Have ' + result.data.TotalCount + ' Record');
                }
                $scope.orders = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
            }, function () {
                console.log('Load productcategory failed.');
            });
        }
        function search() {
            getOrders();
        }
        $scope.getOrders();
    }
})(angular.module('smq.orders'));