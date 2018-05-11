/// <reference path="~/Assets/admin/libs/angular/angular.js" />

(function (app) {
    app.controller('feedbackListController', feedbackListController);
    feedbackListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox'];
    function feedbackListController($scope, apiService, notificationService, $ngBootbox) {
        $scope.feedbacks = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.getFeedbacks = getFeedbacks;
        $scope.totalQuantity = 0;
        $scope.total = 0;
        $scope.keyword = '';
        $scope.search = search;

        $scope.deleteFeedback = deleteFeedback;
        $scope.exportExcelFeedback = exportExcelFeedback;
        $scope.updateFeedback = updateFeedback;

        function getFeedbacks(page) {
            page = page || 0;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: 10
                }
            }
            apiService.get('/api/feedback/getall', config, function (result) {
                if (result.data.TotalCount == 0) {
                    notificationService.displayWarning("No Record");
                }
                else {
                    notificationService.displaySuccess('Have ' + result.data.TotalCount + ' Record');
                }
                $scope.feedbacks = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
            }, function () {
                console.log('Load productcategory failed.');
            });
        }
        function deleteFeedback(id) {
            $ngBootbox.confirm('Are you sure you want to delete?').then(function () {
                var config = {
                    params: {
                        id: id
                    }
                }
                apiService.del('/api/feedback/delete', config, function () {
                    notificationService.displaySuccess('Deleted successfully');
                    search();
                }, function () {
                    notificationService.displayError('Deleted faild');
                })
            });
        }
        function updateFeedback(id,status) {
                var config = {
                    params: {
                        id: id,
                        status: status
                    }
                }
                apiService.get('api/feedback/updatestatus', config, function () {
                    notificationService.displaySuccess('Updated successfully');
                    search();
                }, function () {
                    notificationService.displayError('Updated faild');
                });
        }

        function exportExcelFeedback() {
            var config = {
                params: {
                    filter: $scope.keyword
                }
            }
            apiService.get('/api/feedback/exportfeedback', config, function (response) {
                if (response.status = 200) {
                    window.location.href = response.data.Message;
                }
            }, function (error) {
                notificationService.displayError(error);

            });
        }
        function search() {
            getFeedbacks();
        }
        $scope.getFeedbacks();
    }
})(angular.module('smq.feedbacks'));