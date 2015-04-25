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

    $scope.brightnessTime = {
        current: 0,
        new: 0
    };

    $scope.sensorLimit = {
        current: 0,
        limit: 0
    }

    var mapCurrentSettings = function(settings) {
        $scope.delay.current = settings.Delay;
        $scope.workPeriod.current = settings.WorkPeriod;
        $scope.brightnessLimit.current = settings.SensorMinimumLimit;
        $scope.brightnessTime.current = settings.SensorLimitTimeTreshold;

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

    var sensorFeecbackSeries = new TimeSeries();
    var screwFeedbackSeries = new TimeSeries();
    var airFeedbackSeries = new TimeSeries();

    screwHub.client.message = function (type, msg) {
        if (type === "settingsUpdated") {
            mapCurrentSettings(msg);
        }
        if (type === "sensorValue") {
            $scope.brightnessSensor.lastvalue = msg.Value;
            sensorFeecbackSeries.append(new Date().getTime(), msg.Value);
        }
        if (type === "screwState") {
            $scope.status.screw = msg;
            screwFeedbackSeries.append(new Date().getTime(), msg * 100);
        }
        if (type === "airState") {
            $scope.status.air = msg;
            airFeedbackSeries.append(new Date().getTime(), msg * 100);
        }
        if (type === "sensorLimit") {
            $scope.sensorLimit = Math.floor(msg.CurrentTotal) + "/" + msg.LimitSeconds;
        }
        $rootScope.$apply();
    };


    var smoothie = new SmoothieChart({
        grid: {
            strokeStyle: 'rgb(125, 0, 0)',
            lineWidth: 1, millisPerLine: 1000, verticalSections: 6
        },
        labels: { fillStyle: 'rgb(255, 255, 255)' },
        millisPerPixel:100,
        timestampFormatter: SmoothieChart.timeFormatter,
        interpolation: 'step'
    });

    smoothie.addTimeSeries(sensorFeecbackSeries,
      { strokeStyle: 'rgb(0, 255, 0)', lineWidth: 2 });
    smoothie.addTimeSeries(screwFeedbackSeries,
      { strokeStyle: 'rgb(255, 255, 255)', lineWidth: 2 });
    smoothie.addTimeSeries(airFeedbackSeries,
      { strokeStyle: 'rgb(120, 120, 255)', lineWidth: 2 });

    smoothie.streamTo(document.getElementById("mycanvas"));

    $scope.setNewValues = function () {
        screwHub.server.updateSettings({
            delay: $scope.delay.new,
            workPeriod: $scope.workPeriod.new,
            sensorMinimumLimit: $scope.brightnessLimit.new,
            airFlow: $scope.airFlow.new,
            SensorLimitTimeTreshold: $scope.brightnessTime.new
        });
        screwHub.server.enable();
    }

    $scope.disable = function() {
        screwHub.server.disable();
    }
});