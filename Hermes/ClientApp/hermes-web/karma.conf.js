process.env.CHROME_BIN = require('puppeteer').executablePath();

module.exports = function (config) {
  config.set({
    basePath: '',
    frameworks: ['jasmine', '@angular-devkit/build-angular'],
    plugins: [
      require('karma-jasmine'),
      require('karma-chrome-launcher'),
      require('karma-jasmine-html-reporter'),
      require('@angular-devkit/build-angular/plugins/karma')
    ],
    client: {
      clearContext: false // leave Jasmine Spec Runner output visible in browser
    },
    reporters: ['progress', 'kjhtml'],
    port: 9876,
    colors: true,
    autoWatch: true,
    singleRun: false,
    browsers: ['Chrome_no_sandbox'],
    customLaunchers: {
      Chrome_no_sandbox: {
        base: 'ChromiumHeadless',
        flags: ['--no-sandbox']
      }
    }
  });
};
