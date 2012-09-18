require.config({
  //ASSIGN SHORTCUTS FOR EASY LOADING AND VERSION ABSTRACTION
  paths: {
    jquery: 'libs/jquery-1.8.1.min',
    underscore: 'libs/underscore-min',
    can: 'libs/can.jquery.min',
    kendoall: 'libs/kendoui/js/kendo.all.min',
    kendoweb: 'libs/kendoui/js/kendo.web.min',
    kendomobile: 'libs/kendoui/js/kendo.mobile.min',
    kendodataviz: 'libs/kendoui/js/kendo.dataviz.min',
    jquerymobile: 'libs/jquerymobile/jquery.mobile-1.2.0-beta.1.min',
    qTip: 'libs/qtip/jquery.qtip.min',
    blockUI: 'libs/jquery.blockUI',
    ajaxForm: 'libs/jquery.form',
    moment: 'libs/moment.min',
    notifier: 'libs/notifier.mod',
    html5placeholder: 'libs/html5placeholder.mod',
    browserselector: 'libs/css_browser_selector'
  },
  //DECLARE NON-AMD COMPLIANT JS AND DEPENDENCIES
  shim: {
    underscore: {
      deps: [ 'jquery' ],
      exports: '_'
    },
    can: {
      deps: [ 'jquery' ],
      exports: 'can'
    },
    kendoall: {
      deps: [ 'jquery' ],
      exports: 'kendo'
    },
    kendoweb: {
      deps: [ 'jquery' ],
      exports: 'kendo'
    },
    kendomobile: {
      deps: [ 'jquery' ],
      exports: 'kendo'
    },
    kendodataviz: {
      deps: [ 'jquery' ],
      exports: 'kendo'
    },
    moment: {
      deps: [ 'jquery' ],
      exports: 'moment'
    },
    notifier: {
      deps: [ 'jquery' ],
      exports: 'Notifier'
    },
    qTip: ['jquery'],
    blockUI: ['jquery'],
    ajaxForm: ['jquery'],
    html5placeholder: ['jquery']
  }
});

//INITIALIZE APP
require([
  'router',
  'browserselector',
  'html5placeholder'
], function () {
  //ROUTER DEPENDENCY INITIATED AND LISTENING
});