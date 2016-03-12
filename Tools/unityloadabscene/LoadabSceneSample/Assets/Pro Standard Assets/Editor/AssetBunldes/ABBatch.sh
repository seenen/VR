#!/bin/sh

UnityLocation=/Applications/Unity/Unity.app/Contents/MacOS/Unity
ProjectLocation=/project/HeroeSource/GameEditors/Tool_AssetBundles
In=/project/HeroeSource/GameEditors/Tool_AssetBundles/Assets/2dsound
Out=/project/TestOut

$UnityLocation -batchmode -projectPath $ProjectLocation -executeMethod Main.EntryCommand -quit -CustomArgs:Language=en_US,Version=1.02,InFolder=$In,OutFolder=$Out