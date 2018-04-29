(function (app) {
    app.controller('postCategoryListController', postCategoryListController);

    postCategoryListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];

    function postCategoryListController($scope, apiService, notificationService, $ngBootbox, $filter) {
        $scope.postCategories = [];
        $scope.page = 0;
        $scope.pagesCount = 0;

        $scope.getPostCagories = getPostCagories;
        $scope.keyword = '';
        $scope.search = search;

        $scope.deletePostCategory = deletePostCategory;
        $scope.selectAll = selectAll;
        $scope.deleteMultiple = deleteMultiple;
        function deleteMultiple() {
            $ngBootbox.confirm('Are you sure you want to delete?').then(function () {
                var listId = [];
                $.each($scope.selected, function (i, item) {
                    listId.push(item.ID);
                });
                var config = {
                    params: {
                        checkedPostCategories: JSON.stringify(listId)
                    }
                }
                apiService.del('/api/postcategory/deletemulti', config, function (result) {
                    notificationService.displaySuccess('Deleted successfully ' + result.data + ' record(s)');
                    search();
                }, function (error) {
                    notificationService.displayError('Deleted faild');
                });
            });
        }


        $scope.isAll = false;
        function selectAll() {
            if ($scope.isAll === false) {
                angular.forEach($scope.postCategories, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            }
            else {
                angular.forEach($scope.postCategories, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
            }
        }

        $scope.$watch("postCategories", function (n, o) {
            var checked = $filter("filter")(n, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnDeletePostCategory').removeAttr('disabled');
            }
            else {
                $('#btnDeletePostCategory').attr('disabled', 'disabled');
            }
        }, true);

        function deletePostCategory(id) {
            $ngBootbox.confirm('Are you sure you want to delete?').then(function () {
                var config = {
                    params: {
                        id: id
                    }
                }
                apiService.del('/api/postcategory/delete', config, function () {
                    notificationService.displaySuccess('Deleted successfully');
                    search();
                }, function () {
                    notificationService.displayError('Delete faild');
                })
            });
        }

        function search() {
            getPostCagories();
        }

        function getPostCagories(page) {
            page = page || 0;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: 10
                }
            }

            apiService.get('/api/postcategory/getall', config, function (result) {
                if (result.data.TotalCount == 0) {
                    notificationService.displayWarning("No Record");
                }
                else {
                    notificationService.displaySuccess('Have ' + result.data.TotalCount + ' Record');
                }
                $scope.postCategories = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
            }, function () {
                console.log('Load postcategory failed.');
            });
        }

        $scope.getPostCagories();
    }
})(angular.module('smq.post_categories'));