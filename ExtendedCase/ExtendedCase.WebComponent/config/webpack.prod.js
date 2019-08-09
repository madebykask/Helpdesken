var Webpack = require('webpack');
var WebpackMerge = require('webpack-merge');
var CommonConfig = require('./webpack.common.js');
var Helpers = require('./helpers.js');
const TerserPlugin = require('terser-webpack-plugin');
const OptimizeCSSAssetsPlugin = require('optimize-css-assets-webpack-plugin');
const cssnano = require('cssnano');
//const ENV = process.env.NODE_ENV = process.env.ENV = 'production';

const outputDir = 'dist';

module.exports = WebpackMerge(CommonConfig({ env: 'prod',  outputDir: outputDir }), 
{    
    devtool: 'source-map', 

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
                sourceMap: true,
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
