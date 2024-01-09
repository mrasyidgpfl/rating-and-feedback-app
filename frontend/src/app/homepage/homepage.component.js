angular.module('myApp', ['ionic'])

.controller('MyCtrl', function($scope) {
  
  $scope.ratings = [{ name: 'Service', number: '4.5' }, { name: 'Food', number: '3.25987' }, { name: 'Speed', number: '1.25987' }];
 
  $scope.getStars = function(rating) {
    // Get the value
    var val = parseFloat(rating);
    // Turn value into number/100
    var size = val/6*100;
    return size + '%';
  }
  
});