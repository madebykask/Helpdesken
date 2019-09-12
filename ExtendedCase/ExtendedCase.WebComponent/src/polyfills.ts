// import 'core-js/es6/reflect';
import 'zone.js/dist/zone';
import 'reflect-metadata';
import '@webcomponents/custom-elements/custom-elements.min';
import '@webcomponents/custom-elements/src/native-shim.js'
import '@webcomponents/webcomponentsjs/custom-elements-es5-adapter.js';
import 'url-search-params-polyfill';
/* required for attachShadow or createShadowDom for IE(no CSS encapsulation) */
// import '@webcomponents/webcomponentsjs/webcomponents-bundle.js'
/* required for attachShadow for IE(no CSS encapsulation) */
// import '@webcomponents/shadydom' 

/* if (!Element.prototype.matches) {
  Element.prototype.matches = (<any>Element.prototype).msMatchesSelector ||
    Element.prototype.webkitMatchesSelector;
} */
declare let ENV: any; // // to avoid compiler error. Using global variable from js.

declare let require: any;

if (ENV === 'production') {
    // Production
 } else {
     // Development and test
     Error['stackTraceLimit'] = Infinity;
     require('zone.js/dist/long-stack-trace-zone');
 };
