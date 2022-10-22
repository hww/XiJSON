# XiJSON _Stupid simple serialization for Unity 3D_

![](https://img.shields.io/badge/unity-2018.3%20or%20later-green.svg)
[![⚙ Build and Release](https://github.com/hww/XiJSON/actions/workflows/ci.yml/badge.svg)](https://github.com/hww/XiJSON/actions/workflows/ci.yml)
[![openupm](https://img.shields.io/npm/v/com.hww.xijson?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.hww.xijson/)
[![](https://img.shields.io/github/license/hww/XiJSON.svg)](https://github.com/hww/XiJSON/blob/master/LICENSE)
[![semantic-release: angular](https://img.shields.io/badge/semantic--release-angular-e10079?logo=semantic-release)](https://github.com/semantic-release/semantic-release)

Simple asstes renaming tool by [hww](https://github.com/hww)

## Introduction

Fast data serialization solution for Unity. Has been used in two commercial products for redemption machines.

Uses Unity's built-in JSON serializer, but does simple data sanitization using regular expressions.

## Install

The package is available on the openupm registry. You can install it via openupm-cli.

```bash
openupm add com.hww.xijson
```
You can also install via git url by adding this entry in your manifest.json

```bash
"com.hww.xijson": "https://github.com/hww/XiJSON.git#upm"
```

## Usage 

For MonoBehaviour, just inherince your class from JsonBehaviour. For ScriptableObject, just inherince from JsonObject.
After that the importing and exporting will read and write data to the StreamingAssets folder.

