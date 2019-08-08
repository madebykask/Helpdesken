const Webpack = require('webpack');
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
  MODE: 'test',
  BASEURL: 'ExtendedCase',
  BASEAPIURL: 'ExtendedCaseApi'
}

const outputDir = 'dist-test';

module.exports = WebpackMerge.smart(CommonConfig({ env: CONSTANTS.MODE,  outputDir: outputDir }), 
{        
        devtool: 'source-map', //todo: check other options for prod
        output: {
            path: Helpers.root(outputDir),
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
                    terserOptions: {
                      ecma: 6,
                      output: {
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
                })
            ]
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