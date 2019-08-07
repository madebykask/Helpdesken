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
  MODE: 'test',
  BASEURL: 'ExtendedCase',
  BASEAPIURL: 'ExtendedCaseApi'
}

module.exports = WebpackMerge.smart(CommonConfig,
    {
        devtool: 'source-map',
        output: {
            path: Helpers.root('dist-test'),
            publicPath: '/' + CONSTANTS.BASEURL + '/',
            filename: '[name].[hash].js',
            chunkFilename: '[id].[hash].chunk.js'
        },
        module: {
            rules: [
                {
                    test: /\.(png|jpe?g|gif|ico)$/,
                    use: [{
                        loader: 'file-loader',
                        options: {
                            name: '[name].[ext]?v=[hash]',
                            outputPath: 'img/',
                            publicPath: CONSTANTS.BASEURL +'/'
                        }
                    }],
                    exclude: [Helpers.root('dist'), Helpers.root('dist-test')]
                }
            ]
        },
        plugins: [
            new Webpack.optimize.UglifyJsPlugin({
                compress: { warnings: false },
                sourceMap: true,
                test: /(?:(vendor|polyfills|app)[\.\d\w]*\.js)+/i
            }),
            new ExtractTextPlugin('[name].[hash].css'),
            new Webpack.DefinePlugin({
                ENV: JSON.stringify(CONSTANTS.MODE),
                AppSettings: JSON.stringify({
                    'apiHost': '/' + CONSTANTS.BASEAPIURL,
                    'showDebugProxyModel': false,
                    'debugMode': false
                })
            })
        ]
});