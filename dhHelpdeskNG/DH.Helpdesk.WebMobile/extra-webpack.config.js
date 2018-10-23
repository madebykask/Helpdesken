'use strict';

//const webpack = require('webpack');
const MomentLocalesPlugin = require('moment-locales-webpack-plugin');

// https://webpack.js.org/plugins/context-replacement-plugin/
module.exports = {
    plugins: [
        new MomentLocalesPlugin({
            localesToKeep: ['en', 'sv','pl','de','nl','nb','nn','fi','ru','zh-cn','fr','da','cs','it','ko','ja','lt','lv','ca','es','is','hu','sk','sl'],
        }),
        // new webpack.ContextReplacementPlugin(/moment[\\\/]locale$/, /^\.\/(en|sv|pl|de|nl|nb|nn|fi|ru|zh-cn|fr|da|cs|it|ko|ja|lt|lv|ca|es|is|hu|sk|sl)$/)
    ]
};