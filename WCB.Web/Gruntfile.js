module.exports = function (grunt) {
    grunt.initConfig({
        bower: {
            install: {
                options: {
                    targetDir: "bower_components",
                    layout: "byComponent",
                    cleanTargetDir: true
                }
            }
        },
        concat: {
            options: {
            },
            bower: {
                files: {
                    "wwwroot/bower_concat.js": [
                        "bower_components/jquery/dist/jquery.js",
                        "bower_components/angular/angular.js",
                        "bower_components/bootstrap/dist/js/bootstrap.js",
                        "bower_components/signalr/jquery.signalR.js"],
                    "wwwroot/bower_concat.css": ["bower_components/**/*.css", "!bower_components/**/*.min.css"]
                },
                nonull: true
            }
        }
    });

    grunt.registerTask("default", ["bower:install", "concat:bower"]);
    grunt.loadNpmTasks("grunt-bower-task");
    grunt.loadNpmTasks("grunt-contrib-concat");
};