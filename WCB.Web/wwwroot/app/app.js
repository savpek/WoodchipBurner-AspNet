var app = angular.module("app", []);

app.controller("controlsController", function ($scope, $rootScope, $log) {
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

    $scope.brightnessSensor = {
        lastvalue: 0
    };
    $scope.status = {
        screw: 0
    };

    $scope.airFlow = {
        current: 0,
        new: 50
    };

    var mapCurrentSettings = function(settings) {
        $scope.delay.current = settings.Delay;
        $scope.workPeriod.current = settings.WorkPeriod;
        $scope.brightnessLimit.current = settings.SensorMinimumLimit;
        $scope.airFlow.current = settings.AirFlow;
    }

    var screwHub = $.connection.screwHub;
    var logHub = $.connection.logHub;

    $.connection.hub.start().done(function () {
        screwHub.server.getCurrentSettings().done(function(settings) {
            mapCurrentSettings(settings);
        });
    })
    .fail(function(msg) {
        console.log(msg);
    });

    logHub.client.message = function (msg) {
        $log.info(msg.Message);
    }

    screwHub.client.message = function (type, msg) {
        if (type === "settingsUpdated") {
            mapCurrentSettings(msg);
        }
        if (type === "sensorValue") {
            $scope.brightnessSensor.lastvalue = msg.Value;
        }
        if (type === "screwState") {
            $scope.status.screw = msg;
        }
        if (type === "airState") {
            $scope.status.air = msg;
        }
        $rootScope.$apply();
    };

    $scope.setNewValues = function () {
        screwHub.server.updateSettings({
            delay: $scope.delay.new,
            workPeriod: $scope.workPeriod.new,
            sensorMinimumLimit: $scope.brightnessLimit.new,
            airFlow : $scope.airFlow.new
        });
        screwHub.server.enable();
    }

    $scope.disable = function() {
        screwHub.server.disable();
    }
});