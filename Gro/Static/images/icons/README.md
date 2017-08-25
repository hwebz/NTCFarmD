﻿# Sprites
This file contains developer instructions on how to create and use sprite images in this project.

Author: [Viktor Wissing](mailto:viktor.wissing@cgi.com)

## Icon files
For every icon, there should be two files. One for normal screens and one for retina. The filenames should follow this convention:

Normal file: `search.png`<br />
Retina file: `search@2x.png`

## Bundling
In `gulpfile.js`, we define the path for the icons that should be included in the sprites. The bundling is then performed by Gulp, which will create the following three files:

`/images/icons/dist/sprite.png`<br />
`/images/icons/dist/sprite@2x.png`<br />
`/styles/_components.sprites.scss`

## Using sprites in CSS
To use the icons in the stylesheets, simply call the retina mixin like this:

`@include retina-sprite($icon-search-group);`

This will automatically serve the correct icon depending on the screens pixel density.
