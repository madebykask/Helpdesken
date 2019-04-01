var Webpack = require('webpack');
var WebpackMerge = require('webpack-merge');
var ExtractTextPlugin = require('extract-text-webpack-plugin');
var CommonConfig = require('./webpack.common.js');
var Helpers = require('./helpers.js');

//const ENV = process.env.NODE_ENV = process.env.ENV = 'production';

module.exports = WebpackMerge(CommonConfig, {
    devtool: 'source-map',

    output: {
        path: Helpers.root('wwwroot'),
        publicPath: '/',
        filename: '[name].[hash].js',
        chunkFilename: '[id].[hash].chunk.js'
    },

    plugins: [
      new Webpack.NoEmitOnErrorsPlugin(),
      new Webpack.optimize.UglifyJsPlugin({ // https://github.com/angular/angular/issues/10618
          mangle: {
              keep_fnames: true
          }
      }),
      new ExtractTextPlugin('[name].[hash].css'),
      //new webpack.DefinePlugin({
      //    'process.env': {
      //        'ENV': JSON.stringify(ENV)
      //    }
      //}),
      new Webpack.LoaderOptionsPlugin({
          htmlLoader: {
              minimize: false // workaround for ng2
          }
      })
    ]
});
