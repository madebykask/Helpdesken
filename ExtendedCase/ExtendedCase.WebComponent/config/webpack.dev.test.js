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
  MODE: 'dev',
}

const outputDir = Helpers.root('dist');;

module.exports = WebpackMerge.smart(CommonConfig({ env: CONSTANTS.MODE,  outputDir: outputDir }), 
{
  devtool: 'source-map',
  output: {
    path: outputDir,
    publicPath: '/',
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

  plugins: [
      new Webpack.DefinePlugin({
          ENV: JSON.stringify(CONSTANTS.MODE),
          AppSettings: JSON.stringify({
              'apiHost': 'http://localhost:8090',
              'showDebugProxyModel': true,
              'debugMode': false
          })
      })
  ]
});