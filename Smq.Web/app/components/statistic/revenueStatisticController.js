(function (app) {
    app.controller('revenueStatisticController', revenueStatisticController);

    revenueStatisticController.$inject = ['apiService', '$scope','notificationService','$filter'];

    function revenueStatisticController(apiService, $scope, notificationService, $filter) {
        $scope.tabledata = [];
        $scope.labels = [];
        $scope.series = ['Doanh số', 'Lợi nhuận'];

        $scope.chartdata = [];
        function getStatistic() {
            var config = {
                param: {
                    fromDate: '01/01/2015',
                    toDate:'01/01/2018'
                }
            }
            apiService.get('api/statistic/getrevenue?fromDate=' + config.param.fromDate + "&toDate=" + config.param.toDate + '', null, function (response) {
                $scope.tabledata = response.data;
                var lables = [];
                var chartData = [];
                var revenues = [];
                var benefits = [];
                $.each(response.data, function (i,item) {
                    lables.push($filter('date')(item.Date,'dd/MM/yyyy'));
                    revenues.push(item.Revenues);
                    benefits.push(item.Benefit);
                });
                chartData.push(revenues);
                chartData.push(benefits);

                $scope.chartdata = chartData;
                $scope.labels = lables;
            }, function (response) {
                notificationService.displayError("Không thể tải dữ liệu");
            });
        }
        getStatistic();
    }

})(angular.module('smq.statistics'));