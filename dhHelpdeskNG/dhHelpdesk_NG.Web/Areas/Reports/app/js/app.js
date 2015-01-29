'use strict';

var reportsApp = angular.module('reportsApp', ['ngRoute']);

reportsApp.config(function($routeProvider){

    var baseUrl = '/Areas/Reports/app';

    $routeProvider
        .when('/', {
            controller: 'reportsController',
            templateUrl: baseUrl + '/templates/reports.html'
        })
        .otherwise({
        redirectTo: '/'
    })
});
