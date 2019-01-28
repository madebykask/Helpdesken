'use strict';
var gulp = require('gulp'),
    //merge = require('merge'),
    //fs = require('fs'),
    del = require('del'),
    path = require('path'),
    gutil = require('gulp-util'),
    webpack = require('webpack')
//WebpackDevServer = require('webpack-dev-server');

//gulp.task('webpack:build', function (callback) {
//    // modify some webpack config options
//    var myConfig = Object.create(require('./config/webpack.prod.js'));
//    myConfig.plugins = myConfig.plugins.concat(
//        //new webpack.DefinePlugin({
//        //    "process.env": {
//        //        // This has effect on the react lib size
//        //        "NODE_ENV": JSON.stringify('production')
//        //    }
//        //}),
//        //new webpack.optimize.DedupePlugin(),
//        //new webpack.optimize.UglifyJsPlugin()
//    );

//    // run webpack
//    webpack(myConfig, function (err, stats) {
//        if (err) throw new gutil.PluginError('webpack:build', err);
//        gutil.log('[webpack:build]', stats.toString({
//            colors: true
//        }));
//        callback();
//    });
//});

//////////////////////////////////////////////////////////////test environment config for dhsolution test server
gulp.task('clean-wwwroot-test', function () {
    return del(['./wwwroot-test/**/*']).then(paths => {
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

gulp.task('build-test', ['clean-wwwroot-test', 'webpack:build-test']);
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
gulp.task('build-dev-test', ['clean-wwwroot', 'webpack:build-dev-test']);
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////


////////////////////////////////////////////////////////////////////////////////////////////////// local dev env

gulp.task('clean-wwwroot', function () {
    return del(['./wwwroot/**/*']).then(paths => {
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

gulp.task('build-dev', ['clean-wwwroot', 'webpack:build-dev']);

gulp.task('build-dev-watch', ['webpack:build-dev'], function () {
    gulp.watch(['src/**/*'], ['webpack:build-dev']);
});
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
var defaultTasks = ['build-test'];
gulp.task('default', defaultTasks);
//gulp.task('run-server', function() {
//    connect.server({
//        root: 'wwwroot',
//        livereload: true,
//        port: 9009
//    });
//});

//gulp.task('build-and-run', ['build-dev-watch','run-server']);

//gulp.task('webpack-dev-server', function (callback) {
//    // modify some webpack config options
//    var myConfig = Object.create(require('./config/webpack.dev.js'));
//    myConfig.devtool = 'eval';

//    // Start a webpack-dev-server
//    new WebpackDevServer(webpack(myConfig), {
//        publicPath: '/' + myConfig.output.publicPath,
//        stats: {
//            colors: true
//        }
//    }).listen(8080, 'localhost', function (err) {
//        if (err) throw new gutil.PluginError('webpack-dev-server', err);
//        gutil.log('[webpack-dev-server]', 'http://localhost:8080/webpack-dev-server/index.html');
//    });
//});
