
# Prerequisites for ARM Android Maui Builds

This guide provides the setup required to run Android SDK projects in the Rider IDE on a MacBook with an ARM architecture, this was done specifically for the MacBook M1 Pro, but should apply to any ARM architechture MacBook.

## Software to Install

### DMG:
- **[Rider](https://www.jetbrains.com/rider/download/#section=mac)**
- **[Android Studio](https://developer.android.com/studio/)**
- **[Xcode](https://apps.apple.com/us/app/xcode/id497799835?mt=12)**

### Brew:
```bash
brew install --cask android-studio
brew install --cask rider
```
*Note: Xcode is not supported via Homebrew, use the official App Store instead.*

After installing Xcode run this command in your terminal
```
sudo xcode-select -s /Applications/Xcode.app/Contents/Developer
```
- Verify that the path is indeed correct by running the following command
```
xcode-select -p
```
- Run the following command and accept xcode licenses if you havent already
```
sudo xcodebuild -license
```
- Open XCode and download the IOS Simulation Runtime module, this should appear when you try to create a new project and choose the iOS template.

### Java Development Kit (JDK):
As of **22.09.24**, the version used is **17.0.12**.
- [Download JDK](https://www.oracle.com/in/java/technologies/downloads/#java17)

---

## Steps to Create and Run Android MAUI Project

1. Open Rider.
2. Create a new solution using the default Android Maui template.
3. Run the solution. If everything works fine, congratulations!

---

## Steps to Create and Run the Visual Studio Template MAUI Project
This Projects master branch is similair to the Visual Studio Template as of 7/10/2024

1. Pull this project on your macbook
2. Open the solution in Rider and run it

Nuget Package Prerequisites for the Visual Studio Template project is:
- Microsoft.Maui.Controls
- Microsoft.Maui.Controls.Compatibility
- Microsoft.Extensions.Logging.Debug
- Microsoft.NET.ILLink.Tasks

---

## Troubleshooting

If you encounter an error, try following these steps (replace `YOURUSERNAME` with your own username):

1. Download the **Command Line Tools** for Mac from [Android Studio Command Line Tools](https://developer.android.com/studio/).
2. Extract the downloaded file to `/YOURUSERNAME/Library/Android/sdk/cmdline-tools`.
3. Create a folder named `latest`.
   - This should have the path `/YOURUSERNAME/Library/Android/sdk/cmdline-tools/latest`
5. Move the files from `cmdline-tools` to the `latest` folder.

### Folder Structure After Extraction

You should now have the following structure:

```
/YOURUSERNAME/Library/Android/sdk/cmdline-tools/latest
/YOURUSERNAME/Library/Android/sdk/cmdline-tools/latest/bin
```

- If the `bin` folder doesn’t exist, create it.

### JAVA Error

If you have encountered a JAVA error similair to this:
java -v 
	/Users/USERNAME/.asdf/shims/java: line 3: /opt/homebrew/Cellar/asdf/0.13.1/libexec/bin/asdf: 
	No such file or directory /Users/USERNAME/.asdf/shims/java: line 3: exec: /opt/homebrew/Cellar/asdf/0.13.1/libexec/bin/asdf: cannot execute: No such file or directory


Try running the following:
```
asdf reshim
```


### Accept SDK Licenses

1. Using the terminal, navigate to:

   ```bash
   cd /YOURUSERNAME/Library/Android/sdk/cmdline-tools/latest/bin
   ```

2. Run the following command to accept the SDK licenses:

   ```bash
   yes | ./sdkmanager --licenses
   ```

3. Install the Android 34 API:

   ```bash
   ./sdkmanager "platforms;android-34"
   ```



### Final Step
You should now be able to build and run your project successfully.

---

