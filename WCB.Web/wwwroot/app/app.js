﻿var app = angular.module("myNoteApp", []);

app.controller("myNoteCtrl", function ($scope) {
    $scope.message = "Hei moi!";
    $scope.left = function () { return 100 - $scope.message.length; };
    $scope.clear = function () { $scope.message = ""; };
    $scope.save = function () { alert("Note Saved"); };
});