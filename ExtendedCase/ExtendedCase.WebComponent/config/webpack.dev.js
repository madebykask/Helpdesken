﻿const Webpack = require("webpack");
const WebpackMerge = require("webpack-merge");
const CommonConfig = require("./webpack.common.js");
const Helpers = require("./helpers.js");
const packageJSON = require("../package.json");

// Parse version at the top of your webpack.config
const CONSTANTS = {
  VERSION: JSON.stringify(packageJSON.version),
  MODE: "dev",
};

const outputDir = Helpers.root("dist");

module.exports = WebpackMerge(
  CommonConfig({ env: CONSTANTS.MODE, outputDir: outputDir }),
  {
    devtool: "source-map",
    output: {
      path: outputDir,
      publicPath: "/",
      filename: "[name].[hash].js",
      chunkFilename: "[id].[hash].chunk.js",
    },

    plugins: [
      new Webpack.ProgressPlugin(),

      new Webpack.DefinePlugin({
        ENV: JSON.stringify(CONSTANTS.MODE),
        AppSettings: JSON.stringify({
          apiHost: "https://localhost:447/extendedcase-api",
          showDebugProxyModel: true,
          debugMode: true,
        }),
      }),
    ],
  }
);
