var Webpack = require('webpack');
var WebpackMerge = require('webpack-merge');
var ExtractTextPlugin = require('extract-text-webpack-plugin');
var CommonConfig = require('./webpack.common.js');
var Helpers = require('./helpers.js');
var HtmlWebpackPlugin = require('html-webpack-plugin');
const packageJSON = require('../package.json');

// Parse version at the top of your webpack.config
let CONSTANTS = {
    VERSION: JSON.stringify(packageJSON.version),
    MODE: 'dev'
  }  

module.exports = WebpackMerge(CommonConfig,
{
    devtool: 'source-map',

    output: {
        path: Helpers.root('wwwroot'),
        publicPath: '/',
        filename: '[name].[hash].js',
        chunkFilename: '[id].[hash].chunk.js'
    },
    plugins: [
        new ExtractTextPlugin('[name].[hash].css'),
        new Webpack.DefinePlugin({
            ENV: JSON.stringify(CONSTANTS.MODE),
            AppSettings: JSON.stringify({
                'apiHost': 'http://localhost:8090',
                'showDebugProxyModel': true,
                'debugMode': true
            })
        }),
        new HtmlWebpackPlugin({
            template: 'src/template.ejs',
            filename: 'index.html',
            inject: false,
            chunks: ['app', 'vendor', 'polyfills'],
            head: {
                js: ['polyfills'],
                css: ['vendor', 'app'],
                customCss: []
            },
            body: {
                js: ['vendor', 'app'],
                css: []
            },
            baseUrl: '/',
            version: CONSTANTS.VERSION,
            mode: CONSTANTS.MODE
        })
    ]
});