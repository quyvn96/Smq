(function (app) {
    app.controller('postListController', postListController);

    postListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];

    function postListController($scope, apiService, notificationService, $ngBootbox, $filter) {
        $scope.posts = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.getPosts = getPosts;
        $scope.keyword = '';

        $scope.search = search;

        $scope.deletePost = deletePost;

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
                        checkedProducts: JSON.stringify(listId)
                    }
                }
                apiService.del('/api/post/deletemulti', config, function (result) {
                    notificationService.displaySuccess('Deleted successfuly' + result.data + ' record(s).');
                    search();
                }, function (error) {
                    notificationService.displayError('Deleted faild');
                });
            });
        }

        $scope.isAll = false;
        function selectAll() {
            if ($scope.isAll === false) {
                angular.forEach($scope.posts, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            } else {
                angular.forEach($scope.posts, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
            }
        }

        $scope.$watch("posts", function (n, o) {
            var checked = $filter("filter")(n, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnDeletePost').removeAttr('disabled');
            } else {
                $('#btnDeletePost').attr('disabled', 'disabled');
            }
        }, true);

        function deletePost(id) {
            $ngBootbox.confirm('Are you sure you want to delete?').then(function () {
                var config = {
                    params: {
                        id: id
                    }
                }
                apiService.del('/api/post/delete', config, function () {
                    notificationService.displaySuccess('Deleted successfully');
                    search();
                }, function () {
                    notificationService.displayError('Deleted faild');
                })
            });
        }

        function search() {
            getPosts();
        }

        function getPosts(page) {
            page = page || 0;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: 20
                }
            }
            apiService.get('/api/post/getall', config, function (result) {
                if (result.data.TotalCount == 0) {
                    notificationService.displayWarning('Not found record.');
                }
                $scope.posts = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
            }, function () {
                console.log('Load post failed.');
            });
        }

        $scope.getPosts();
    }
})(angular.module('smq.posts'));