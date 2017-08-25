/// <binding ProjectOpened='watch' />
var exec = require('child_process').exec;

// Plugins
var gulp = require("gulp"),
    plugins = require("gulp-load-plugins")({ pattern: ["*", "gulp-*", "gulp.*"] });     // Lazy loads all plugins           https://github.com/jackfranklin/gulp-load-plugins



// Paths
var basePath = "./Static/",
    baseDestinationPath = basePath + "/dist/",
    bowerPath = "./bower_components/",
    paths = {
        styles: {
            src: basePath + "styles/",
            dist: baseDestinationPath + "styles/"
        },
        scripts: {
            src: basePath + "scripts/",
            vendor: baseDestinationPath + "scripts/vendor/",
            dist: baseDestinationPath + "scripts/"
        },
        sprites: {
            src: basePath + "images/icons/",
            dist: baseDestinationPath + "images/icons/",
            cssBackground: "dist/images/icons/"
        }
    };

// Styles
gulp.task("styles", function () {
    // Configure task here
    var config = {
        srcFile: "styles.scss",
        distFileName: "styles.css",
        distFileSuffix: ".min",
        postCssPlugins: [
            plugins.autoprefixer
        ]
    };

    // Task stream
    return gulp
        .src(paths.styles.src + config.srcFile)
        .pipe(plugins.plumber({ errorHandler: handleError }))		                // Handle errors                    https://github.com/floatdrop/gulp-plumber
        .pipe(plugins.sass().on("error", plugins.sass.logError))                    // Compile sass file                https://github.com/dlmanning/gulp-sass
        .pipe(plugins.postcss(config.postCssPlugins))                               // Run postcss plugins              https://github.com/postcss/gulp-postcss
        .pipe(plugins.size({ title: "Original file size: " }))		                // Show uncompressed file size      https://github.com/sindresorhus/gulp-size
        .pipe(plugins.rename(config.distFileName))		                            // Rename file to *.css             https://github.com/hparra/gulp-rename
        .pipe(gulp.dest(paths.styles.dist))                                             // Save css file
        .pipe(plugins.cssnano())									                // Minify css file                  https://github.com/ben-eb/gulp-cssnano
        .pipe(plugins.rename({ suffix: config.distFileSuffix }))	                // Rename file to *.min.css         https://github.com/hparra/gulp-rename
        .pipe(plugins.size({ title: "Minified file size: " }))		                // Show minified file size          https://github.com/sindresorhus/gulp-size
        .pipe(gulp.dest(paths.styles.dist));	                                        // Save minified css file
});



// Modernizr
gulp.task("modernizr", function () {
    // Configure task here
    var config = {
        srcFiles: [
            paths.scripts.src + "modules.js"
        ]
    };

    // Task stream
    return gulp
        .src(config.srcFiles)
        .pipe(plugins.plumber({ errorHandler: handleError }))		                // Handle errors                    https://github.com/floatdrop/gulp-plumber
        .pipe(plugins.jsvalidate())									                // Validate files for errors        https://github.com/sindresorhus/gulp-jsvalidate
        .pipe(plugins.modernizr("modernizr-custom.js"))                             // Create modernizr build           https://github.com/doctyper/gulp-modernizr
        .pipe(gulp.dest(paths.scripts.vendor + "modernizr-custom.js"));					                        // Save minified js file
});



// Scripts
gulp.task("scripts", function () {
    // Configure task here
    var config = {
        srcFiles: [
            paths.scripts.src + "jquery.filer.min.js",
            paths.scripts.src + "src/common.js",
            paths.scripts.src + "tablesaw.js",
            paths.scripts.src + "src/tablesaw-init.js",
            paths.scripts.src + "src/header.js",
            paths.scripts.src + "src/header-internal.js",
            paths.scripts.src + "src/circle-progress.js",
            paths.scripts.src + "src/agreement-data.js",
            paths.scripts.src + "src/chart.js",
            paths.scripts.src + "src/form-controls.js",
            paths.scripts.src + "src/calculate-delivery-fee.js",
            paths.scripts.src + "src/delivery-note.js",
            paths.scripts.src + "src/meddelanden.js",
            paths.scripts.src + "src/grobarhet-pa-utsade.js",
            paths.scripts.src + "src/message-forms.js",
            paths.scripts.src + "src/mitt-konto.js",
            paths.scripts.src + "src/upload-avatar.js",
            paths.scripts.src + "src/my-profile.js",
            paths.scripts.src + "src/my-machine.js",
            paths.scripts.src + "src/my-company.js",
            paths.scripts.src + "src/my-account.js",
            paths.scripts.src + "src/customer-id.js",
            paths.scripts.src + "src/term-of-use.js",
            paths.scripts.src + "src/registration.js",
            paths.scripts.src + "src/contact-form.js",
            paths.scripts.src + "src/upcoming-delivery-carousel.js",
            paths.scripts.src + "src/search-transport.js",
            paths.scripts.src + "src/validation.js",
            paths.scripts.src + "src/delivery-assurance.js",
            paths.scripts.src + "src/vertical-table.js",
            paths.scripts.src + "src/purchase-agreement.js",
            paths.scripts.src + "src/time-booking.js",
            paths.scripts.src + "src/customer-card.js",
            paths.scripts.src + "src/internal-navigation.js"
        ],
        distFileName: "scripts.js",
        distFileSuffix: ".min"
    };

    // Task stream
    return gulp
        .src(config.srcFiles)
        .pipe(plugins.plumber({ errorHandler: handleError }))		                // Handle errors                    https://github.com/floatdrop/gulp-plumber
        .pipe(plugins.jsvalidate())									                // Validate files for errors        https://github.com/sindresorhus/gulp-jsvalidate
        .pipe(plugins.concat(config.distFileName))					                // Concatenate all script files     https://github.com/contra/gulp-concat
        .pipe(plugins.size({ title: "Original file size: " }))		                // Show original file size          https://github.com/sindresorhus/gulp-size
        .pipe(gulp.dest(paths.scripts.dist))					                        // Save uncompressed file
        .pipe(plugins.uglify())										                // Minify js files                  https://github.com/terinjokes/gulp-uglify
        .pipe(plugins.size({ title: "Minified file size: " }))		                // Show minified file size          https://github.com/sindresorhus/gulp-size
        .pipe(plugins.rename({ suffix: config.distFileSuffix }))	                // Rename file to min.js            https://github.com/hparra/gulp-rename
        .pipe(gulp.dest(paths.scripts.dist));					                        // Save minified js file
});



// Sprites
gulp.task("sprites", function () {
    // Configure task here
    var config = {
        srcFileFormat: "*.png",
        srcRetinaFilter: "*@2x.png",
        distNormalImage: "sprite.png",
        distRetinaImage: "sprite@2x.png",
        distCss: "_components.sprites.scss",
        distCssVarPrefix: "icon-",
        cssFormat: "scss_retina"
    };

    // Task stream
    var stream = gulp
        .src(paths.sprites.src + config.srcFileFormat)
        .pipe(plugins.plumber({ errorHandler: handleError }))					    // Handle errors                        https://github.com/floatdrop/gulp-plumber
        .pipe(plugins.spritesmith({                                                 // Generate sprites                     https://github.com/twolfson/gulp.spritesmith
            imgName: config.distNormalImage,									    // Filename of normal sprite
            imgPath: paths.sprites.cssBackground + config.distNormalImage,			// Path to normal sprite in css
            retinaImgName: config.distRetinaImage,								    // Filename of retina sprite
            retinaImgPath: paths.sprites.cssBackground + config.distRetinaImage,	// Path to retina sprite in css
            retinaSrcFilter: [paths.sprites.src + config.srcRetinaFilter],		    // Retina filter
            cssName: config.distCss,											    // Name of css/less/scss file
            cssFormat: config.cssFormat,										    // Css format
            cssVarMap: function (sprite) {										    // Variable format for css file
                sprite.name = config.distCssVarPrefix + sprite.name;
            }
        }));

    // CSS stream
    var cssStream = stream.css
        .pipe(plugins.plumber({ errorHandler: handleError }))                       // Handle errors                        https://github.com/floatdrop/gulp-plumber
        .pipe(gulp.dest(paths.styles.src));			                                    // Save css file

    // Image stream
    var imgStream = stream.img
        .pipe(plugins.plumber({ errorHandler: handleError }))                       // Handle errors                        https://github.com/floatdrop/gulp-plumber
        .pipe(plugins.imagemin())                                                   // Optimize images                      https://github.com/sindresorhus/gulp-imagemin
        .pipe(gulp.dest(paths.sprites.dist));					                        // Save sprite images

    // Merge css and image stream
    return plugins.mergeStream(imgStream, cssStream);                                   // Merge streams                        https://github.com/grncdr/merge-stream
});



// Watch for new/changed/deleted files
gulp.task("watch", function () {
    // Configure task here
    var config = {
        includeCss: "*.scss",
        includeJs: "*.js"
    }

    // Watch commands
    gulp.watch(paths.styles.src + "**/" + config.includeCss, ["styles"]);	                    // Watch for changes in less files
    gulp.watch(paths.scripts.src + "**/" + config.includeJs, ["scripts"]);                      // Watch for changes in js files
});

gulp.task("webpack", function (cb) {
    exec("webpack --progress --colors --config webpack.config.production.js", function (err, stdout, stderr) {
        console.log(stdout);
        console.log(stderr);
        cb(err);
    });
});

// Default task
gulp.task("default", ["styles", "scripts", "sprites"]);                                 // Default gulp task

gulp.task("build:prod", ["default", "webpack"]);

// Error handling function
var handleError = function (err) {
    plugins.util.log(err);                                                              // Show error messages                  https://github.com/gulpjs/gulp-util
    this.emit("end");
};
