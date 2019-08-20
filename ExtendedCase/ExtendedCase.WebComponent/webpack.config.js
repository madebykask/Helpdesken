const webpack = require('webpack');

module.exports = env => {
      let configFile = '';
      var mode = env.mode || 'dev';
      console.log('>>>mode is: %s', mode);
      switch (mode) {
        case 'dev':
          configFile = './config/webpack.dev.js';
          break; 

        case 'test':
          configFile = './config/webpack.test.js';
          break;

        case 'test-mobile':
          configFile = './config/webpack.test-mobile.js';
          break;

        case 'prod':
          configFile = './config/webpack.prod.js';
          break;

        case 'dev.test':
          configFile = './config/webpack.dev.test.js';
          break;
      }

    const webpackConfig = require(configFile);
    return webpackConfig;
};