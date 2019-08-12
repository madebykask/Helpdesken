const Webpack = require('webpack');
const CleanWebpackPlugin = require('clean-webpack-plugin');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const AngularCompilerPlugin = require('@ngtools/webpack').AngularCompilerPlugin;
//const CopyPlugin = require('copy-webpack-plugin');
const StatsPlugin = require('stats-webpack-plugin');
const Helpers = require('./helpers.js');

module.exports = args => {
    console.log(">>> args: " + JSON.stringify(args));

    const outputDir = args.outputDir;
    const isDevMode = args.env === 'dev' || args.env === 'dev-test';
    const tsconfigFile = isDevMode ? './tsconfig.json' : './tsconfig.aot.json';
    console.log('>>> tsconfigFile: %s', tsconfigFile);

    return {
        node: {process: false }, //keep it fo ie11 issue: https://github.com/angular/angular/issues/24769
        
        mode: isDevMode ? 'development' : 'production', // webpack4 required
        
        entry: {
            ecapp: isDevMode ? './src/main.ts' : './src/main.aot.ts',
            ecpolyfills: './src/polyfills.ts',
            //ecvendor: './src/vendor.ts'
        },

        resolve: {
            extensions: ['.ts', '.js']
        },

        //todo: new webpack4 settings instead of CommonsChunkPlugin: https://webpack.js.org/plugins/split-chunks-plugin/
         optimization: {
            runtimeChunk: false,
            noEmitOnErrors: true,
            //splitChunks: {
            //    cacheGroups: {
            //     default: false,
            //     commons: {
            //        test: /[\\/]node_modules[\\/]/,
            //        name: 'ecvendor',
            //        chunks: 'all',
            //        minChunks: 2
            //      },
            //    }
            //},
        },

        module: {
            rules: [
                {
                    test: /\.ts$/,
                    loaders: ['@ngtools/webpack'],
                    exclude: [Helpers.root('dist'), Helpers.root('dist-test')]
                }, {
                    test: /\.html$/,
                    use: 'html-loader',
                    exclude: [Helpers.root('dist'), Helpers.root('dist-test')]
                }, {
                    test: /\.(woff|woff2|eot|ttf|otf)(\?v=\d+\.\d+\.\d+)?$/,
                    use: {
                        loader:'url-loader',
                        options: {
                            name: '[name].[ext]',
                            limit: 8192,
                            outputPath: '/fonts/'
                        }
                    }
                }, {
                    test: /\.svg(\?v=\d+\.\d+\.\d+)?$/,
                    use: 'url-loader?limit=10000&minetype=image/svg+xml',
                }, {
                    test: /\.(png|jpe?g|gif|ico)$/,
                    use: [
                        {
                            loader: 'file-loader',
                            options: {
                                name: '[name].[ext]?v=[hash]',
                                outputPath: '/img/'
                            }
                        }
                    ],
                    exclude: [Helpers.root('dist'), Helpers.root('dist-test')]
                }, {
                    test: /\.css$/,
                    use: [
                        'css-to-string-loader',
                        { loader: 'css-loader', options: { sourceMap: isDevMode } }
                    ]
                }, {
                    test: /\.scss$/,
                    use: [
                        'css-to-string-loader', 
                        { loader: 'css-loader', options: { sourceMap: isDevMode } },
                        { loader: 'sass-loader', options: { sourceMap: isDevMode } }
                    ]
                }
            ]
        },
        plugins: [
            new CleanWebpackPlugin(['*.*'], {
                root: Helpers.root(outputDir),
                verbose: true
            }),

            // will be extracted and copied from css
            //new CopyPlugin([{ from: 'src/styles/images/*', to: 'img/[name].[ext]' }]),
            //new CopyPlugin([{ from: 'src/styles/fonts/*', to: 'fonts/[name].[ext]' }]),

            //new Webpack.LoaderOptionsPlugin({
            //    debug: true
            //}),

            // no css to reference at the moment
            /*new MiniCssExtractPlugin({
                filename: '[name].[hash].css',
                chunkFilename: '[id].[hash].css' 
            }),
            */
            
            new AngularCompilerPlugin({
                tsConfigPath: tsconfigFile,
                entryModule: Helpers.root('src', 'app.module#AppModule'),
                sourceMap: isDevMode
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
        
            new StatsPlugin('stats.json', 'verbose'),

            new HtmlWebpackPlugin({
                title: 'Extended case element',
                template: "./public/test.ejs",
                filename: "./test.html",
                inject: false,
                chunks: ['ecapp', 'ecpolyfills'],
                head: {
                    js: ['ecpolyfills']
                },
                body: {
                    js: ['ecapp']
                }
            }),
        ]
    }
};