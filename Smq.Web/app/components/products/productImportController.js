(function (app) {
    app.controller('productImportController', productImportController);

    productImportController.$inject = ['apiService', '$http', 'authenticationService', '$scope', 'notificationService', '$state', 'commonService'];

    function productImportController(apiService, $http, authenticationService, $scope, notificationService, $state, commonService) {

        $scope.files = [];
        $scope.categoryID = 0;
        $scope.ImportProduct = ImportProduct;
        //listen for the file selected event
        $scope.$on("fileSelected", function (event, args) {
            $scope.$apply(function () {
                //add the file object to the scope's files collection
                $scope.files.push(args.file);
            });
        });

        function ImportProduct() {
            authenticationService.setHeader();
            $http({
                method: 'POST',
                url: "/api/product/import",
                headers: { 'Content-Type': undefined },
                transformRequest: function (data) {
                    var formData = new FormData();
                    formData.append("categoryID", angular.toJson(data.categoryID));
                    //now add all of the assigned files
                    for (var i = 0; i < data.files.length; i++) {
                        //add each file to the form data and iteratively name them
                        formData.append("file" + i, data.files[i]);
                    }
                    return formData;
                },
                //Create an object that contains the model and files which will be transformed
                // in the above transformRequest method
                data: { categoryID: $scope.categoryID, files: $scope.files }
            }).then(function (result, status, headers, config) {
                notificationService.displaySuccess(result.data);
                $state.go('products');
            },
            function (data, status, headers, config) {
                notificationService.displayError(data);
            });
        }
        function loadProductCategory() {
            apiService.get('/api/productcategory/getallparents', null, function (result) {
                $scope.productCategories = result.data;
            }, function () {
                console.log('Cannot get list parent');
            });
        }
        loadProductCategory();
    }

})(angular.module('smq.products'));