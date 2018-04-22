(function (app) {
    app.controller('revenueStatisticController', revenueStatisticController);

    revenueStatisticController.$inject = ['$scope', 'apiService', 'notificationService', '$filter'];

    function revenueStatisticController($scope, apiService, notificationService, $filter) {
        $scope.filterData = filterData;
        function filterData() {
            $scope.tabledata = [];
            $scope.labels = [];
            $scope.series = ['Revenue', 'Profit'];

            $scope.chartdata = [];
            function getStatistic() {
                if ($scope.fromdate != undefined && $scope.todate != undefined)
                {
                    var config = {
                        param: {
                            fromDate: $filter('date')($scope.fromdate, "dd-MM-yyyy"),
                            toDate: $filter('date')($scope.todate, "dd-MM-yyyy")
                        }
                    }
                    apiService.get('/api/statistic/getrevenues?fromDate=' + config.param.fromDate + "&toDate=" + config.param.toDate, null, function (response) {
                        $scope.tabledata = response.data;
                        var labels = [];
                        var chartData = [];
                        var revenues = [];
                        var benefits = [];
                        $.each(response.data, function (i, item) {
                            labels.push($filter('date')(item.Date, 'dd-MM-yyyy'));
                            revenues.push(item.Revenues);
                            benefits.push(item.Benefit);
                        });
                        chartData.push(revenues);
                        chartData.push(benefits);

                        $scope.chartdata = chartData;
                        $scope.labels = labels;
                    }, function (response) {
                        notificationService.displayError('Cannot load data');
                    });
                }
                else {
                    notificationService.displayError('Cannot load data');
                }
            }

            getStatistic();
        }
    }

})(angular.module('smq.statistics'));