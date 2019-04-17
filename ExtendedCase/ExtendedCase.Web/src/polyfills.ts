// import 'es6-shim';
import 'core-js/client/core';
import 'zone.js/dist/zone';
import 'reflect-metadata';
declare var ENV: any; // // to avoid compiler error. Using global variable from js.

if (ENV === 'production') {
    // Production
 } else {
     // Development and test
     Error['stackTraceLimit'] = Infinity;
     require('zone.js/dist/long-stack-trace-zone');
 };
