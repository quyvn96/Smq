﻿/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('smq.post_categories', ['smq.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider.state('post_categories', {
            url: "/post_categories",
            parent: 'base',
            templateUrl: "/app/components/post_categories/postCategoryListView.html",
            controller: "postCategoryListController"
        })
            .state('add_post_category', {
                url: "/add_post_category",
                parent: 'base',
                templateUrl: "/app/components/post_categories/postCategoryAddView.html",
                controller: "postCategoryAddController"
            })
            .state('edit_post_category', {
                url: "/edit_post_category/:id",
                parent: 'base',
                templateUrl: "/app/components/post_categories/postCategoryEditView.html",
                controller: "postCategoryEditController"
            });
    }
})();