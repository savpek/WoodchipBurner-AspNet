﻿var app = angular.module("app", []);

app.controller("controlsController", function ($scope, $rootScope) {
    $scope.delay = {
        current: 3,
        new: 2
    };

    $scope.brightnessLimit = {
        current: 30,
        new: 20
    };
    $scope.workPeriod = {
        current: 2,
        new: 3
    };
    // Declare a proxy to reference the hub. 
    var hub = $.connection.delayHub;
    
    $.connection.hub.start().done(function (msg) {
        console.log(msg);
    })
    .fail(function(msg) {
        console.log(msg);
    });

    hub.client.message = function(msg) {
        console.log(msg);
    };
});