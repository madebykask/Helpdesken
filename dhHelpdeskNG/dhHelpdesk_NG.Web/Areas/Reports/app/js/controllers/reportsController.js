/**
 * Created by user on 29.01.2015.
 */

reportsApp.controller('reportsController', function($scope, reportsService){

    reportsService.getReportsData(function(data){
        $scope.translations = data.Translations;
        $scope.reports = data.Reports;
    });

});