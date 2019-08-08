var Webpack = require('webpack');
var WebpackMerge = require('webpack-merge');
var CommonConfig = require('./webpack.common.js');
var Helpers = require('./helpers.js');
const OptimizeCSSAssetsPlugin = require('optimize-css-assets-webpack-plugin');
const cssnano = require('cssnano');
//const ENV = process.env.NODE_ENV = process.env.ENV = 'production';

const outputDir = 'wwwroot';

module.exports = WebpackMerge(CommonConfig({ env: 'prod',  outputDir: outputDir }), 
{    
    devtool: 'source-map', //todo: check other options for prod

    output: {
        path: Helpers.root(outputDir),
        publicPath: '/',
        filename: '[name].[hash].js',
        chunkFilename: '[id].[hash].chunk.js'
    },
    
    optimization: {
        minimizer: [
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

    plugins: [
      new Webpack.NoEmitOnErrorsPlugin(),
      new Webpack.LoaderOptionsPlugin({
          htmlLoader: {
              minimize: false // workaround for ng2 
          }
      })
    ]
});
