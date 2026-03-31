# Building HospiScanner

This guide will help you build and run the HospiScanner Android application.

## Prerequisites

### Required Software
1. **Android Studio** (Hedgehog 2023.1.1 or later)
   - Download from: https://developer.android.com/studio
   
2. **Java Development Kit (JDK)** 17 or later
   - Bundled with Android Studio or download from: https://adoptium.net/

3. **Android SDK**
   - API Level 24 (Android 7.0) minimum
   - API Level 34 (Android 14) target
   - Installed automatically with Android Studio

### Optional
- Physical Android device with camera (recommended for testing)
- Android Emulator with camera support

## Setup Instructions

### 1. Clone the Repository
```bash
git clone https://github.com/aleeecsitoo/HospiScanner.git
cd HospiScanner
```

### 2. Open in Android Studio
1. Launch Android Studio
2. Click "Open an Existing Project"
3. Navigate to the cloned `HospiScanner` directory
4. Click "OK"
5. Wait for Gradle sync to complete

### 3. Configure SDK
Android Studio should automatically download the required SDK versions. If not:
1. Go to `Tools` → `SDK Manager`
2. Under `SDK Platforms`, ensure Android 14.0 (API 34) is installed
3. Under `SDK Tools`, ensure these are installed:
   - Android SDK Build-Tools
   - Android SDK Platform-Tools
   - Android Emulator
4. Click "Apply" and "OK"

## Building the App

### Using Android Studio (Recommended)

#### Debug Build
1. Click the "Run" button (green play icon) or press `Shift+F10`
2. Select your target device (physical device or emulator)
3. Wait for build to complete
4. App will automatically install and launch

#### Release Build
1. Go to `Build` → `Generate Signed Bundle / APK`
2. Select `APK`
3. Create a new keystore or use an existing one
4. Fill in keystore details
5. Select `release` build variant
6. Click "Finish"
7. Find the APK in `app/build/outputs/apk/release/`

### Using Command Line

#### Debug Build
```bash
# On Linux/Mac
./gradlew assembleDebug

# On Windows
gradlew.bat assembleDebug
```
The APK will be at: `app/build/outputs/apk/debug/app-debug.apk`

#### Release Build
```bash
# On Linux/Mac
./gradlew assembleRelease

# On Windows
gradlew.bat assembleRelease
```
The APK will be at: `app/build/outputs/apk/release/app-release-unsigned.apk`

#### Install on Connected Device
```bash
# On Linux/Mac
./gradlew installDebug

# On Windows
gradlew.bat installDebug
```

## Running Tests

### Unit Tests
```bash
# On Linux/Mac
./gradlew test

# On Windows
gradlew.bat test
```

### View Test Results
Test results will be available at:
`app/build/reports/tests/testDebugUnitTest/index.html`

## Troubleshooting

### Gradle Sync Failed
**Solution:**
1. Ensure you have internet connection
2. Try `File` → `Invalidate Caches / Restart`
3. Delete `.gradle` folder and sync again

### SDK Not Found
**Solution:**
1. Open `File` → `Project Structure`
2. Under `SDK Location`, verify the Android SDK path
3. Click `Apply` and `OK`
4. Sync project again

### Build Tools Version Issue
**Solution:**
Edit `app/build.gradle.kts` and adjust the `compileSdk` and `targetSdk` versions to match your installed SDK.

### Camera Not Working on Emulator
**Solution:**
1. Use a physical device for best camera testing
2. Or configure emulator with webcam support:
   - Go to AVD Manager
   - Edit your emulator
   - Under "Advanced Settings" → "Camera", select "Webcam"

### Dependencies Download Failed
**Solution:**
1. Check your internet connection
2. Try using a VPN if behind a firewall
3. Clear Gradle cache:
   ```bash
   rm -rf ~/.gradle/caches
   ```

## Project Structure

```
HospiScanner/
├── app/
│   ├── build.gradle.kts          # App-level build configuration
│   ├── src/
│   │   ├── main/
│   │   │   ├── java/com/hospiscanner/
│   │   │   │   ├── model/        # Data models
│   │   │   │   ├── view/         # Activities/UI
│   │   │   │   └── viewmodel/    # ViewModels
│   │   │   ├── res/              # Resources (layouts, strings, etc.)
│   │   │   └── AndroidManifest.xml
│   │   └── test/                 # Unit tests
│   └── proguard-rules.pro        # ProGuard rules for release builds
├── gradle/
│   └── wrapper/                  # Gradle wrapper files
├── build.gradle.kts              # Project-level build configuration
├── settings.gradle.kts           # Project settings
├── gradle.properties             # Gradle properties
└── README.md                     # Project documentation
```

## Build Variants

### Debug
- Debuggable
- Includes debug symbols
- No code obfuscation
- Larger APK size

### Release
- Not debuggable
- ProGuard optimization
- Code obfuscation
- Smaller APK size
- Requires signing key

## Performance Tips

1. **Enable Build Cache**
   ```properties
   # In gradle.properties
   org.gradle.caching=true
   ```

2. **Increase Heap Size**
   ```properties
   # In gradle.properties
   org.gradle.jvmargs=-Xmx4096m -XX:MaxMetaspaceSize=512m
   ```

3. **Use Build Analyzer**
   - After build, click "Build Analyzer" in Android Studio
   - Identifies slow tasks and suggests optimizations

## Signing the Release APK

### Generate Keystore
```bash
keytool -genkey -v -keystore my-release-key.jks -alias my-key-alias -keyalg RSA -keysize 2048 -validity 10000
```

### Sign APK
```bash
jarsigner -verbose -sigalg SHA256withRSA -digestalg SHA-256 -keystore my-release-key.jks app-release-unsigned.apk my-key-alias
```

### Verify Signature
```bash
jarsigner -verify -verbose -certs app-release-unsigned.apk
```

## Device Requirements

### Minimum Requirements
- Android 7.0 (API 24) or higher
- Camera hardware
- 50 MB free storage

### Recommended Requirements
- Android 10.0 (API 29) or higher
- Rear camera with autofocus
- 100 MB free storage

## Building for Production

1. **Update Version**
   - Edit `app/build.gradle.kts`
   - Increment `versionCode` and update `versionName`

2. **Enable ProGuard**
   ```kotlin
   buildTypes {
       release {
           isMinifyEnabled = true
           proguardFiles(
               getDefaultProguardFile("proguard-android-optimize.txt"),
               "proguard-rules.pro"
           )
       }
   }
   ```

3. **Test Thoroughly**
   - Test on multiple devices
   - Test all features
   - Check performance

4. **Build Release APK**
   ```bash
   ./gradlew assembleRelease
   ```

5. **Sign APK**
   - Use Android Studio's signing wizard, or
   - Use command line with jarsigner

## Additional Resources

- [Android Developer Guide](https://developer.android.com/guide)
- [CameraX Documentation](https://developer.android.com/training/camerax)
- [ML Kit Barcode Scanning](https://developers.google.com/ml-kit/vision/barcode-scanning/android)
- [Material Design Guidelines](https://material.io/design)
- [Gradle Build Configuration](https://docs.gradle.org/current/userguide/userguide.html)

## Support

For build issues or questions:
1. Check this guide's troubleshooting section
2. Review the [README.md](README.md) and [ARCHITECTURE.md](ARCHITECTURE.md)
3. Open an issue on GitHub with details about your build environment

## License

See [LICENSE](LICENSE) file for details.
