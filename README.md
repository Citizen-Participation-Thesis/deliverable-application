# Overview

This repository contains the final version of the application developed as part of the joint master's thesis of Olav Håberg Dimmen and Andreas Oksvold. The application is a prototype mobile augmented reality application for public participation in urban planning built in the Unity framework for Android using ARFoundation.

# Requirements and Disclaimers

The application should be run on an Android device. (It is in theory cross-platform but it has never been built for an iOS device)
The Android device must be an ARCore supported device. In general, this means that it runs Nougat 7.0 or newer with API level 24 or newer. We recommend that the Android is at least Pie 9.0 or newer or API level 28 or newer. A complete list of supported devices can be found here: https://developers.google.com/ar/devices.

Augmented reality is computationally intensive and the application will run better on more powerful devices.

For the purposes of building the application any reasonably modern computer should suffice.
If you believe your computer to be on the weaker side, consider closing all unrelated applications during this tutorial and building process.
Note that the application has only ever been built with Windows and Linux computers, although macOS would also likely work.
For good measure, you should have at least 10 GB of free disk space.

# Build Instructions
Here are the suggested instruction for building application to your device, which consist of 5 steps.
If you are reading this and you are not on GitHub, disregard step 2.
We have included the APK-file, build.apk, of the built application that you may transfer directly to your device if you are familiar with developing for Android.

## 1. Download Unity Hub and Unity Editor

Download the Unity Hub to your computer.
You may follow the instructions provided by Unity here: https://unity.com/download

**Once Unity Hub is downloaded and installed, it is very important that you choose the correct Unity Editor version and modules.**
You are looking for Unity Editor version 2020.3.26f1. If you cannot find that version from the Unity Hub, try the archive: https://unity3d.com/get-unity/download/archive.
When prompted for which modules you want to install along with the editor, you must tick "Android Build Support" including "Android SDK & NDK Tools" and "OpenJDK".

## 2. Clone or Download the Repository

The next step is to clone or download the repository from GitHub.
If you are not familiar with using Git, you can download a ZIP-file of the project by clicking the following link: https://github.com/Citizen-Participation-Thesis/deliverable-application/archive/refs/heads/main.zip.
Unzip the ZIP-file to a convenient location.

## 3. Open the Project in Unity and Set Build Target

In this step you will import the Unity project. Be advised that it may take several minutes to open the project for the first time.

In Unity Hub, navigate to the "Projects" tab.
Locate the "Open" button and click it. You will be prompted by a file manager window.
Locate the folder containing the files you just downloaded, select it and click "open".
The Unity Editor will now begin importing. This may take some time.

When the project has been imported, navigate to the "File" drop-down menu.
Find "Build Settings..." and click it.
You should now see the "Build Settings" window.
In the "Platform" section of the window, it is likely that "PC, Mac & Linux Standalone" is indicated.
However, we want Android to be the build target for our application.
Select the "Android" option in the "Platform" section and find and click the "Switch Platform" button.
Wait until the process completes.

## 4. Build to your Phone

For this step, you will have to enable developer mode on your Android mobile device. Instructions for how to do this is found at: https://developer.android.com/studio/debug/dev-options#enable.
Ensure that the Unity Editor is open on your computer.
Connect your mobile device to your computer with a USB connection.
Once connected, you should see a permissions request on your mobile device. Accept it.

The next task is to build the Addressables system.
In the top row, navigate to the "Windows" drop-down menu.
Find the "Asset Management" option, then "Addressables", and finally "Groups". Click "Groups".
In other words, the path is Window > Asset Management > Addressables > Groups.
You should now see a window called "Addressables Groups".
Go to the "Build" drop-down menu, then the "New Build" option, and finally "Default Build Script". Click "Default Build Script".
In other words, the path is Build > New Build > Default Build Script.

When the Addressables system has been built, the final step is to build the application itself.
Be advised that it may take several minutes for the application to build.
Navigate to the "File" drop-down menu and find "Build Settings...". Click "Build Settings...".
You should now see the "Build Settings" window.
Find and click the "Build and Run".
You should now see a file manager window

## 5. Run the Application and Prepare the QR Code

The application will likely launch on your USB-connected mobile device immediately upon the build finishing.
If it does not, look for the application with the name "mvp-touche-large" in the applications menu on your mobile device.
If you cannot locate the application on your device, verify that the build completed successfully by navigating to the "Console" tab in the Unity Editor and inspect the console output.
Perform step 4 again if the application did not launch automatically and was not on your mobile device.

To use the application you will need to locate the QR code image file.
Open the file manager on your computer.
Navigate to the project folder.
Go to the following sub-folder: Assets > XR > Prefabs. 
Find the PNG-image with the file name "qrcode".
This is the QR code that you will need to scan in order to see the augmented reality content.
You may print the QR code or simply open the PNG on your computer screen and scan it directly.
Follow the instructions in the application to get started.

The augmented reality content will be placed relative to the QR code.
Because you are likely not at the demostration site for which the application was adapted, the augmented reality content will not be placed correctly in the real world.
Perform a 360 degree scan to locate the building.
Should the application loose its tracking or otherwise behave unexpectedly, the only resort is to kill the application process from the application overview screen (the square navigation button on the device navigation bar) and to relaunch it.

## Clean Up

Remember to disable the developer mode setting when you are no longer interested in building the application to your mobile device.
You may follow the same procedure as in the first part of step 4.
