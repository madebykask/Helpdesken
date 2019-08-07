// import 'core-js/es6/reflect';
import 'core-js/client/core';
import 'zone.js/dist/zone';
import 'reflect-metadata';
import '@webcomponents/custom-elements/custom-elements.min';
import '@webcomponents/custom-elements/src/native-shim.js'
import '@webcomponents/webcomponentsjs/custom-elements-es5-adapter.js';
declare var ENV: any; // // to avoid compiler error. Using global variable from js.

declare var require: any;

if (ENV === 'production') {
    // Production
 } else {
     // Development and test
     Error['stackTraceLimit'] = Infinity;
     require('zone.js/dist/long-stack-trace-zone');
 };
