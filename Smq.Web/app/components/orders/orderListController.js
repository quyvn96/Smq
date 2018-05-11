/// <reference path="~/Assets/admin/libs/angular/angular.js" />

(function (app) {
    app.controller('orderListController', orderListController);
    orderListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox'];
    function orderListController($scope, apiService, notificationService, $ngBootbox) {
        $scope.orders = [];
        $scope.orderDetails = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.getOrders = getOrders;
        $scope.viewOrderDetail = viewOrderDetail;
        $scope.totalQuantity = 0;
        $scope.total = 0;
        $scope.keyword = '';
        $scope.search = search;

        $scope.deleteOrder = deleteOrder;
        $scope.exportExcelOrder = exportExcelOrder;
        $scope.deleteOrderDetails = deleteOrderDetails;
        $scope.updateOrderDetails = updateOrderDetails;
        $scope.validationQuantity = validationQuantity;
        $scope.updateOrderStatus = updateOrderStatus;

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
        function deleteOrder(id) {
            $ngBootbox.confirm('Are you sure you want to delete?').then(function () {
                var config = {
                    params: {
                        id: id
                    }
                }
                apiService.del('/api/order/delete', config, function () {
                    notificationService.displaySuccess('Deleted successfully');
                    search();
                }, function () {
                    notificationService.displayError('Deleted faild');
                })
            });
        }
        function deleteOrderDetails(orderId,productId) {
            $ngBootbox.confirm('Are you sure you want to delete?').then(function () {
                var config = {
                    params: {
                        orderId: orderId,
                        productId: productId
                    }
                }
                apiService.del('/api/order/deleteorderdetails', config, function () {
                    notificationService.displaySuccess('Deleted successfully');
                    viewOrderDetail(orderId);
                }, function () {
                    notificationService.displayError('Deleted faild');
                })
            });
        }
        function updateOrderDetails(orderId, productId, quantity) {
                var config = {
                    params: {
                        orderId: orderId,
                        productId: productId,
                        quantity: quantity
                    }
                }
                apiService.get('api/order/updateorderdetails', config, function () {
                    notificationService.displaySuccess('Updated successfully');
                    viewOrderDetail(orderId);
                }, function () {
                    notificationService.displayError('Updated faild');
                });
        }
        function viewOrderDetail(id,page) {
            page = page || 0;
            var config = {
                params: {
                    id: id,
                    page: page,
                    pageSize: 10
                }
            }
            apiService.get('/api/order/getorderdetailbyid', config, function (result) {
                if (result.data.TotalCount == 0) {
                    notificationService.displayWarning("No Record");
                }
                else {
                    notificationService.displaySuccess('Have ' + result.data.TotalCount + ' Record');
                }
                $scope.orderDetails = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
                $scope.totalQuantity = result.data.TotalQuantity;
                $scope.total = result.data.Total;
            }, function () {
                console.log('Load orderDetail failed.');
            });
        }

        function exportExcelOrder(id) {
            var config = {
                params: {
                    id: id
                }
            }
            apiService.get('/api/order/exportexcel', config, function (response) {
                if (response.status = 200) {
                    window.location.href = response.data.Message;
                }
            }, function (error) {
                notificationService.displayError(error);

            });
        }
        function validationQuantity(quantity, orderId, productId) {
            var config = {
                params: {
                    orderId: orderId,
                    productId: productId
                }
            }
            apiService.get('/api/order/getquantity', config, function (response) {
                if (response.status = 200) {
                    var currentQuantity = response.data[0];
                    if (currentQuantity < quantity) {
                            notificationService.displayError('You only input values less current value');
                            $("#quantity-orderdetail-" + orderId + "-" + productId + "").val(currentQuantity);
                            return;
                      }
                }
            }, function (error) {
                notificationService.displayError(error);

            });
        }
        function updateOrderStatus(id, status) {
            var config = {
                params: {
                    id: id,
                    status: status
                }
            }
            apiService.get('api/order/updateorderstatus', config, function () {
                notificationService.displaySuccess('Updated successfully');
                search();
            }, function () {
                notificationService.displayError('Updated faild');
            });
        }
        function search() {
            getOrders();
        }
        $scope.getOrders();
    }
})(angular.module('smq.orders'));