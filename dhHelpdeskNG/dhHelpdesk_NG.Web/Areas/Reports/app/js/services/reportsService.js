/**
 * Created by user on 29.01.2015.
 */

reportsApp.factory('reportsService', function($http){
    return {
        getReportsData: function(callback){
            return $http.get('/Reports/Report/Reports').success(callback);
        }
    };
});