(function () {
    angular.module('smq.feedbacks', ['smq.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider.state('feedbacks', {
            url: "/feedbacks",
            parent: 'base',
            templateUrl: "/app/components/feedbacks/feedbackListView.html",
            controller: "feedbackListController"
        });
    }
})();