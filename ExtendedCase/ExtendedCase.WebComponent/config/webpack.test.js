﻿const Webpack = require('webpack');
const WebpackMerge = require('webpack-merge');
const OptimizeCSSAssetsPlugin = require('optimize-css-assets-webpack-plugin');
const cssnano = require('cssnano');
const TerserPlugin = require('terser-webpack-plugin');
const CommonConfig = require('./webpack.common.js');
const Helpers = require('./helpers.js');
const packageJSON = require('../package.json');

// Parse version at the top of your webpack.config
let CONSTANTS = {
  VERSION: JSON.stringify(packageJSON.version),
  MODE: 'prod',
  BASEURL: 'ExtendedCase',
  BASEAPIURL: 'ExtendedCaseApi'
}

const outputDir = Helpers.root('dist');

module.exports = WebpackMerge.smart(CommonConfig({ env: CONSTANTS.MODE,  outputDir: outputDir }), 
{        
        devtool: 'source-map',
        output: {
            path: outputDir,
            publicPath: '/' + CONSTANTS.BASEURL + '/',
            filename: '[name].[hash].js',
            chunkFilename: '[id].[hash].chunk.js'
        },
        
        optimization: {
            minimize: true,
            minimizer: [
                // webpack 4 does minification by default in production mode
                new TerserPlugin({
                    parallel: true,
                    sourceMap: true,
                    terserOptions: {
                      ie8: false,
                      keep_fnames: true,
                      output: {
                        ecma: 5,
                        comments: false,
                      },
                    },
                  }),

                new OptimizeCSSAssetsPlugin({
                    cssProcessor: cssnano,
                    cssProcessorOptions: {
                        discardComments: {
                            removeAll: true
                        }
                    },
                    canPrint: false
                }),
            ]
        },

        module: {
            rules: [
              {
                test: /\.(woff|woff2|eot|ttf|otf)(\?v=\d+\.\d+\.\d+)?$/,
                use: {
                    loader:'url-loader',
                    options: {
                        name: '[name].[ext]',
                        limit: 8192,
                        publicPath: '/'+ CONSTANTS.BASEURL +'/fonts'
                    }
                }
            }, {
                    test: /\.(png|jpe?g|gif|ico)$/,
                    use: [{
                        loader: 'file-loader',
                        options: {
                            name: '[name].[ext]?v=[hash]',
                            publicPath: '/'+ CONSTANTS.BASEURL +'/img'
                        }
                    }],
                    exclude: [Helpers.root(outputDir)]
                }
            ]
        },
        plugins: [
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