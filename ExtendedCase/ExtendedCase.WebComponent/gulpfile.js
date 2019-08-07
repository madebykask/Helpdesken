'use strict';
var gulp = require('gulp'),
    //merge = require('merge'),
    //fs = require('fs'),
    del = require('del'),
    path = require('path'),
    gutil = require('gulp-util'),
    webpack = require('webpack')

//////////////////////////////////////////////////////////////test environment config for dhsolution test server
gulp.task('clean-dist-test', function () {
    return del(['./dist-test/**/*']).then(paths => {
        console.log('Deleted files and folders:\n', paths.join('\n'));
    });
});

gulp.task('webpack:build-test', function(callback) {
    var config = Object.create(require('./config/webpack.test.js'));
    config.devtool = 'sourcemap';
    webpack(config, function (err, stats) {
        if (err) throw new gutil.PluginError('webpack:build-test', err);
        gutil.log('[webpack:build-test]', stats.toString({
            colors: true
        }));
        callback();
    });

});

gulp.task('build-test', ['clean-dist-test', 'webpack:build-test']);
///////////////////////////////////////////////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////test environment config, but for local dev use


gulp.task('webpack:build-dev-test', function (callback) {
    var config = Object.create(require('./config/webpack.dev.test.js'));
    config.devtool = 'sourcemap';
    webpack(config, function (err, stats) {
        if (err) throw new gutil.PluginError('webpack:build-dev-test', err);
        gutil.log('[webpack:build-dev-test]', stats.toString({
            colors: true
        }));
        callback();
    });

});
gulp.task('build-dev-test', ['clean-dist', 'webpack:build-dev-test']);
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////


////////////////////////////////////////////////////////////////////////////////////////////////// local dev env

gulp.task('clean-dist', function () {
    return del(['./dist/**/*']).then(paths => {
        console.log('Deleted files and folders:\n', paths.join('\n'));
    });
});

// modify some webpack config options
var myDevConfig = Object.create(require('./config/webpack.dev.js'));
myDevConfig.devtool = 'sourcemap';

// create a single instance of the compiler to allow caching
var devCompiler = webpack(myDevConfig);

gulp.task('webpack:build-dev', function (callback) {
    // run webpack
    devCompiler.run(function (err, stats) {
        if (err) throw new gutil.PluginError('webpack:build-dev', err);
        gutil.log('[webpack:build-dev]', stats.toString({
            colors: true
        }));
        callback();
    });
});

gulp.task('build-dev', ['clean-dist', 'webpack:build-dev']);

gulp.task('build-dev-watch', ['webpack:build-dev'], function () {
    gulp.watch(['src/**/*'], ['webpack:build-dev']);
});
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
var defaultTasks = ['build-test'];
gulp.task('default', defaultTasks);

