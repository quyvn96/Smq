(function (app) {
    app.controller('revenueStatisticController', revenueStatisticController);

    revenueStatisticController.$inject = ['$scope', 'apiService', 'notificationService', '$filter'];

    function revenueStatisticController($scope, apiService, notificationService, $filter) {
        $scope.filterData = filterData;
        $scope.exportExcelStatistic = exportExcelStatistic;
        function filterData() {
            $scope.tabledata = [];
            $scope.labels = [];
            $scope.series = ['Doanh thu', 'Lợi nhuận'];

            $scope.chartdata = [];
            function getStatistic() {
                if ($scope.fromdate != undefined && $scope.todate != undefined) {
                    var config = {
                        param: {
                            fromDate: $filter('date')($scope.fromdate, "yyyy-MM-dd"),
                            toDate: $filter('date')($scope.todate, "yyyy-MM-dd")
                        }
                    }
                    apiService.get('/api/statistic/getrevenues?fromDate=' + config.param.fromDate + "&toDate=" + config.param.toDate, null, function (response) {
                        $scope.tabledata = response.data;
                        var labels = [];
                        var chartData = [];
                        var revenues = [];
                        var benefits = [];
                        $.each(response.data, function (i, item) {
                            labels.push($filter('date')(item.Date, "yyyy-MM-dd"));
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
        };
        function exportExcelStatistic() {
            var config = {
                params: {
                    fromDate: $filter('date')($scope.fromdate, "yyyy-MM-dd"),
                    toDate: $filter('date')($scope.todate, "yyyy-MM-dd")
                }
            }
            apiService.get('/api/statistic/exportstatistic', config, function (response) {
                if (response.status = 200) {
                    window.location.href = response.data.Message;
                }
            }, function (error) {
                notificationService.displayError(error);

            });
        }
    }

})(angular.module('smq.statistics'));