(function (app) {
    app.controller('productCategoryListController', productCategoryListController);

    productCategoryListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];

    function productCategoryListController($scope, apiService, notificationService, $ngBootbox, $filter) {
        $scope.productCategories = [];
        $scope.page = 0;
        $scope.pagesCount = 0;

        $scope.getProductCagories = getProductCagories;
        $scope.keyword = '';
        $scope.search = search;

        $scope.deleteProductCategory = deleteProductCategory;
        $scope.selectAll = selectAll;
        $scope.deleteMultiple = deleteMultiple;
        function deleteMultiple() {
            var listId = [];
            $.each($scope.selected, function (i,item) {
                listId.push(item.ID);
            });
            var config = {
                params: {
                    checkedProductCategories:JSON.stringify(listId)
                }
            }
            apiService.del('/api/productcategory/deletemulti', config, function (result) {
                notificationService.displaySuccess('Deleted successfully ' + result.data + ' record(s)');
                search();
            }, function (error) {
                notificationService.displayError('Deleted faild');
            });
        }


        $scope.isAll = false;
        function selectAll() {
            if ($scope.isAll === false) {
                angular.forEach($scope.productCategories, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            }
            else {
                angular.forEach($scope.productCategories, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
            }
        }

        $scope.$watch("productCategories", function (n, o) {
            var checked = $filter("filter")(n, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnDelete').removeAttr('disabled');
            }
            else {
                $('#btnDelete').attr('disabled', 'disabled');
            }
        }, true);

        function deleteProductCategory(id) {
            $ngBootbox.confirm('Are you sure you want to delete?').then(function () {
                var config = {
                    params: {
                        id: id
                    }
                }
                apiService.del('/api/productcategory/delete', config, function () {
                    notificationService.displaySuccess('Deleted successfully');
                    search();
                }, function () {
                    notificationService.displayError('Delete faild');
                })
            });
        }

        function search() {
            getProductCagories();
        }

        function getProductCagories(page) {
            page = page || 0;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize:10
                }
            }

            apiService.get('/api/productcategory/getall', config, function (result) {
                if (result.data.TotalCount == 0) {
                    notificationService.displayWarning("No Record");
                }
                else {
                    notificationService.displaySuccess('Have '+result.data.TotalCount+' Record');
                }
                $scope.productCategories = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
            }, function () {
                console.log('Load productcategory failed.');
            });
        }

        $scope.getProductCagories();
    }
})(angular.module('smq.product_categories'));