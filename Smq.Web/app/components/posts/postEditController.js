(function (app) {
    app.controller('postEditController', postEditController);

    postEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService', '$stateParams'];

    function postEditController(apiService, $scope, notificationService, $state, commonService, $stateParams) {
        $scope.post = {};
        $scope.ckeditorOptions = {
            languague: 'vi',
            height: '200px'
        }
        $scope.UpdatePost = UpdatePost;
        $scope.GetSeoTitle = GetSeoTitle;

        function GetSeoTitle() {
            $scope.post.Alias = commonService.getSeoTitle($scope.post.Name);
        }

        function loadPostDetail() {
            apiService.get('/api/post/getbyid/' + $stateParams.id, null, function (result) {
                console.log(result.data);
                $scope.post = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
        function UpdatePost() {
            apiService.put('/api/post/update', $scope.post,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' Updated successfully.');
                    $state.go('posts');
                }, function (error) {
                    notificationService.displayError('Updated faild.');
                });
        }
        function loadPostCategory() {
            apiService.get('/api/postcategory/getallparents', null, function (result) {
                $scope.postCategories = result.data;
            }, function () {
                console.log('Cannot get list parent');
            });
        }
        $scope.ChooseImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {
                    $scope.post.Image = fileUrl;
                })
            }
            finder.popup();
        }
        loadPostCategory();
        loadPostDetail();
    }

})(angular.module('smq.posts'));