## Design pattern
The design pattern used is [Module Pattern](http://addyosmani.com/resources/essentialjsdesignpatterns/book/#modulepatternjavascript), but feel free to switch to another pattern if you find one that is more suitable for this project..

The most important thing is that you **DO** use a design pattern and don't clutter the project with spaghetti code.

## Namespace
All code should be written inside a module in `modules.js`. **Nothing should be written in the global namespace.**

As the number of modules increases, the modules can preferably be broken out into separate files.

## Bundling
In `gulpfile.js`, we define the files that should be included in the bundle. The bundling is then performed by Gulp. The distribution files are placed in the folder `/scripts/dist/`.

## jQuery & Bootstrap
Both [jQuery](http://jquery.com) and [Bootstrap.js](http://getbootstrap.com/javascript) are loaded from a CDN instead of including it in our bundle. They are both very common libraries that are most likely cached on the visitors computer.

We also provide local fallback copies of the libraries if the CDN is down. These fallback files are located in bower.

## Modernizr
We use [Modernizr](http://modernizr.com) for feature detection, but not the complete build due to it's large filesize. A gulp task called `modernizr` is available for manual run that scans our javascript files and creates a custom modernizr build file with only the required feature detections. This custom build is then included in our javascript bundle.
