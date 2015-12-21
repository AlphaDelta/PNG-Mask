<img src="https://i.imgur.com/NnrDw0q.png" />

## Overview

PNG Mask is a steganography tool for Windows to hide data in PNG files.

## Features

* Multiple steganography methods.
* Hides images, messages, binary files, and URL lists inside images.
* Can password protect hidden files for security using an XOR one-time-pad.
* Licensed under the [WTFPL](http://www.wtfpl.net/txt/copying/) allowing you to use, modify, and/or distribute any code or images from this project however you wish.

## Test Images

| Image         | [<img src="https://i.imgur.com/xmXZNK9.png" />](https://i.imgur.com/xmXZNK9.png) | [<img src="https://i.imgur.com/RhIqvQO.png" />](https://i.imgur.com/RhIqvQO.png) | [<img src="https://i.imgur.com/5z0gEge.png" />](https://i.imgur.com/u0h1VSK.png) | [<img src="https://i.imgur.com/8as6XyR.png" />](https://i.imgur.com/8as6XyR.png) |
|---------------|--------------|-------------|----------------------|----------------------|
| **Password**  | *None*       | *None*      | *None*               | ABC                  |
| **Data Type** | Hidden image | Hidden text | Hidden binary (WEBM) | Hidden link index    |
| **Method**    | XOR EOF      | XOR tEXt    | XOR IDAT             | XOR EOF              |