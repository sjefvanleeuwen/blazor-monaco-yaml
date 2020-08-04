# Monaco-Editor-Yaml

## Guide to create a release build of monaco-yaml

This release is used in BlazorMonaco; a Blazor implementation of monaco-editor

### Steps

#### Setup

* Open VS Code and open the folder where you want the solution to be
* Open Terminal
* Clone the monaco editor
`$ git clone https://github.com/Microsoft/monaco-editor`
* Go to the folder
`$ cd monaco-editor`
* Install the packages (Requires NPM)
`~/monaco-editor$ npm install .`

#### Test (optional)
* Run rimpleserver 
`~/monaco-editor$ npm run simpleserver`
* In the terminal a url appears; click it of open http://127.0.0.1:8088
* Navigate to the folder `monaco-editor`, then `test`
* You will see a working editor (no yaml yet installed)
* Press <ctrl> + C to stop.

#### Add language

* Install monaco-yaml
`~/monaco-editor$ npm install monaco-yaml`
* Add a reference to monaco-yaml in the project
* Open `~/metadata.js` and use existing configuration for comparison. For monaco-yaml it is:

```,
{
	name: 'monaco-yaml',
	contrib: 'vs/language/yaml/monaco.contribution',
	modulePrefix: 'vs/language/yaml',
	paths: {
		src: '/monaco-yaml/dev',
		'npm/dev': 'node_modules/monaco-yaml/dev',
		'npm/min': 'node_modules/monaco-yaml/min',
		esm: 'node_modules/monaco-yaml/esm',
	}
}
```
* Be sure to check if the references to your files are correct. 
* Run it and chak that it works
`~/monaco-editor$ npm run simpleserver`

#### Export

* Register it in the exports.
`~/monaco-editor/ci/webpackcongfig.cs`
* Add the this entry:
`"yaml.worker": 'monaco-yeaml/esm/yaml.worker',`

#### ~Release build~

Run the release. This gives an error for Yaml:
`~/monaco-editor$ npm run release`

##### Fix 1: 
Non-relative import for unknown module: monaco-editor/esm/vs/editor/editor.worker in <path>\monaco-editor\node_modules\monaco-yaml\esm\yaml.worker.js

Go to that file and change this line:
`import * as worker from 'monaco-editor/esm/vs/editor/editor.worker';`
to:
`import * as worker from 'monaco-editor-core/esm/vs/editor/editor.worker';`

##### Fix 2:
Non-relative import for unknown module: js-yaml in <path>\monaco-editor\node_modules\monaco-yaml\esm\languageservice\parser\yamlParser04.js

Go to that file and change this line:
`import { Schema, Type } from 'js-yaml';`
to:
`import { Schema, Type } from '../../../../js-yaml';`

##### Fix 3:
Non-relative import for unknown module: js-yaml in <path>\monaco-editor\node_modules\monaco-yaml\esm\languageservice\parser\yamlParser07.js

Go to that file and change this line:
`import { Schema, Type } from 'js-yaml';`
to:
`import { Schema, Type } from '../../../../js-yaml';`

#### Release build
~/monaco-editor$ npm run release

The folder ~/monaco-editor/release has been created

## Getting BlazorMonaco to work with the javascript

Follow the instructions on how to include the correct javascript into your project

Copy the min and min-maps folders from the release and copy them in your Blazor project.

Replace loading the YAML with this (wait till monaco is defined to actually address monaco).
The correct javascript is now loaded in about 0.2 seconds after the original call for it to be loaded.

```async function loadYamlWhenMonacoKnown() {
/*!-----------------------------------------------------------------------------
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * monaco-yaml version: 2.4.0(73189a1ab44fa1d7449d4fd595d1ecb5e95b7d66)
 * Released under the MIT license
 * https://github.com/kpdecker/monaco-yaml/blob/master/LICENSE.md
 *-----------------------------------------------------------------------------*/
  console.log("Waiting on monaco...");
  var monacoDefined = false;
  while (!monacoDefined) {
    try {
      monaco;
      monacoDefined = true;
      console.log("monaco loaded!");
    } catch (e) {
      //wait 0.1 second
      console.log("waiting...");
    }
    await wait(100);
  }
  !function(e){if("ob... <-- keep the entire line
  define("vs/editor/e... <-- keep the entire line
  //# sourceMappingURL=../../../min-maps/vs/editor/editor.main.js.map
}

loadYamlWhenMonacoKnown();
```
## Research

Information found on these pages helped make this.

https://stackoverflow.com/questions/50606676/how-do-i-add-a-new-language-syntax-to-monaco-editor
https://github.com/Microsoft/monaco-languages
https://github.com/Microsoft/monaco-editor/blob/master/metadata.js#L68-L70

