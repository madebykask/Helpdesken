var Webpack = require('webpack');
var HtmlWebpackPlugin = require('html-webpack-plugin');
var CopyWebpackPlugin = require('copy-webpack-plugin');
var ExtractTextPlugin = require('extract-text-webpack-plugin');
var Helpers = require('./helpers.js');

const AngularCompilerPlugin = require('@ngtools/webpack').AngularCompilerPlugin;

module.exports = {
    entry: {
        ecapp: './src/main.ts',
        ecpolyfills: './src/polyfills.ts',
        // ecvendor: './src/vendor.ts'
    },
    //output: {
    //    filename: './dist/[name].js',
    //},
    resolve: {
        extensions: ['.ts', '.js']
    },
    module: {
        rules: [
            {
                test: /\.ts$/,
                loaders: ['@ngtools/webpack'],
                exclude: [Helpers.root('dist'), Helpers.root('dist-test')]
            },
            {
                test: /\.html$/,
                use: 'html-loader',
                exclude: [Helpers.root('dist'), Helpers.root('dist-test')]
            },
            {
                test: /\.woff(\?v=\d+\.\d+\.\d+)?$/,
                use: 'url-loader?limit=10000&minetype=application/font-woff',
            }, {
                test: /\.woff2(\?v=\d+\.\d+\.\d+)?$/,
                use: 'url-loader?limit=10000&minetype=application/font-woff2',
            }, {
                test: /\.ttf(\?v=\d+\.\d+\.\d+)?$/,
                use: 'url-loader?limit=10000&minetype=application/octet-stream',
            }, {
                test: /\.eot(\?v=\d+\.\d+\.\d+)?$/,
                use: 'file-loader',
            }, {
                test: /\.svg(\?v=\d+\.\d+\.\d+)?$/,
                use: 'url-loader?limit=10000&minetype=image/svg+xml',
            },
            {
                test: /\.(png|jpe?g|gif|ico)$/,
                use: [
                    {
                        loader: 'file-loader',
                        options: {
                            name: '[name].[ext]?v=[hash]',
                            outputPath: 'img/'
                        }
                    }
                ],
                exclude: [Helpers.root('dist'), Helpers.root('dist-test')]
            },
             { //this rule will only be used for any vendors
                 test: /\.css$/,
                 use: ['css-to-string-loader', 'css-loader'],
                 include: Helpers.root('node_modules')
             },
            {
                test: /\.css$/,
                exclude: [ Helpers.root('src', 'app'), Helpers.root('node_modules') ],
                use: ExtractTextPlugin.extract({ fallback: 'raw-loader', use: ['css-to-string-loader', 'css-loader'] })
            },
            {
                test: /\.css$/,
                include: Helpers.root('src', 'app'),
                use: ['css-to-string-loader', 'css-loader']
            },
            {
              test: /\.scss$/,
              use: ['css-to-string-loader', 'css-loader', 'sass-loader']
            }
            /* {
                test: /\.scss$/,
                include: Helpers.root('src', 'styles', 'css'),
                use: ExtractTextPlugin.extract({
                    fallback: 'style-loader',
                    use: [
                      { loader: 'css-loader', options: { importLoaders: 1 } },
                      { loader: 'sass-loader?sourceMap' }
                    ]
                })
            } */

        ]
    },
    plugins: [
        //new CopyWebpackPlugin([{ from: 'src/styles/images/*', to: 'img/[name].[ext]' }]),
        //new Webpack.LoaderOptionsPlugin({
        //    debug: true
        //}),
        new AngularCompilerPlugin({
            tsConfigPath: './tsconfig.json',
            entryModule: Helpers.root('src', 'app.module#AppModule')
        }),
      // Workaround for angular/angular#11580
      new Webpack.ContextReplacementPlugin(
        // The (\\|\/) piece accounts for path separators in *nix and Windows
        /angular(\\|\/)core(\\|\/)@angular/,
        Helpers.root('./src'), // location of your src
        {} // a map of your routes
      ),

        new Webpack.ProvidePlugin({
            jQuery: 'jquery',
            $: 'jquery'
        }),
    ]
};