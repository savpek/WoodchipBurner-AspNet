var app = angular.module("app", []);

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

    var hub = $.connection.screwHub;
    
    $.connection.hub.start().done(function (msg) {
        console.log(msg);
    })
    .fail(function(msg) {
        console.log(msg);
    });

    hub.client.message = function (type, value) {
        if (type === "settingsUpdated") {
            $scope.delay.current = value.delay;
            $scope.workPeriod.current = value.workPeriod;
            $scope.brightnessLimit.current = value.sensorMinimumLimit;
        }
        $rootScope.$apply();
    };

    $scope.setNewValues = function () {
        hub.server.updateSettings({
            delay: $scope.delay.new,
            workPeriod: $scope.workPeriod.new,
            sensorMinimumLimit: $scope.brightnessLimit.new
        });
    }
});