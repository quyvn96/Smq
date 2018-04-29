(function (app) {
    app.controller('postCategoryAddController', postCategoryAddController);

    postCategoryAddController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService'];

    function postCategoryAddController(apiService, $scope, notificationService, $state, commonService) {
        $scope.postCategory = {
            CreatedDate: new Date(),
            Status: true,
        }

        $scope.GetSeoTitle = GetSeoTitle;
        function GetSeoTitle() {
            $scope.postCategory.Alias = commonService.getSeoTitle($scope.postCategory.Name);
        }
        $scope.AddPostCategory = AddPostCategory;

        function AddPostCategory() {
            apiService.post('/api/postcategory/add', $scope.postCategory,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + 'Added successfully.');
                    $state.go('post_categories');
                }, function (error) {
                    notificationService.displayError('Updated faild.');
                });
        }
        function loadParentCategory() {
            apiService.get('/api/postcategory/getallparents', null, function (result) {
                $scope.parentCategories = result.data;
            }, function () {
                console.log('Cannot get list parent');
            });
        }

        loadParentCategory();
    }

})(angular.module('smq.post_categories'));