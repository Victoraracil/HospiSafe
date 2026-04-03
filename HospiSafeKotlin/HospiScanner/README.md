# HospiScanner

Un scanner de QR con parsing en Json para la app HospiSafe

## 📱 Features

- **QR Code Scanning**: Real-time camera-based QR code scanning using ML Kit
- **JSON Parsing**: Automatic JSON parsing and validation of scanned QR codes
- **PIN Verification**: 6-digit PIN verification for patient privacy (based on DNI)
- **MVVM Architecture**: Clean separation of concerns with Model-View-ViewModel pattern
- **Professional UI**: Modern Material Design with vibrant colors (Blue & Teal theme)
- **Permission Handling**: Smooth camera permission request flow
- **Result Display**: Beautiful presentation of scan results with JSON formatting
- **Copy to Clipboard**: Easy copying of scanned data
- **Dark Mode Support**: Automatic dark theme support

## 🏗️ Architecture

This app follows the **MVVM (Model-View-ViewModel)** architecture pattern:

- **Model** (`ScanResult.kt`, `QRDataParser`): Data classes and business logic for JSON parsing
- **ViewModel** (`ScannerViewModel.kt`): Manages UI state and business logic
- **View** (`MainActivity.kt`, layouts): UI components and user interaction

## 🎨 Design

The app features a professional and vibrant color scheme:

- **Primary Color**: Vibrant Blue (#2196F3)
- **Accent Color**: Vibrant Teal (#00BCD4)
- **Success Color**: Vibrant Green (#4CAF50)
- **Error Color**: Vibrant Red (#F44336)

## 📋 Requirements

- Android Studio Arctic Fox or later
- Android SDK 24 (Android 7.0) or higher
- Gradle 8.2+
- Kotlin 1.9.20+

## 🚀 Getting Started

1. Clone the repository:
```bash
git clone https://github.com/aleeecsitoo/HospiScanner.git
cd HospiScanner
```

2. Open the project in Android Studio

3. Build and run the app on an Android device or emulator (camera required)

## 📦 Dependencies

- **AndroidX Core & AppCompat**: Core Android libraries
- **Material Components**: Material Design UI components
- **CameraX**: Modern camera API for Android
- **ML Kit Barcode Scanning**: Google's ML Kit for QR code detection
- **Gson**: JSON parsing library
- **Lifecycle Components**: ViewModel and LiveData for MVVM

## 🔒 Permissions

The app requires the following permissions:

- `CAMERA`: To access the device camera for QR code scanning

## 💡 Usage

1. **Launch the app**: The camera preview will start automatically after granting camera permission
2. **Scan a QR code**: Point your camera at a QR code
3. **PIN Verification** (if QR contains DNI field):
   - A popup dialog will appear requesting a 6-digit PIN
   - Enter the first 6 digits of the patient's DNI
   - Press "Confirmar" to verify or "Cancelar" to return to scanning
4. **View results**: The app will automatically detect and parse the QR code
   - If the QR code contains valid JSON, it will be parsed and displayed in a formatted view
   - If the QR code is not valid JSON, the raw data will be displayed
5. **Copy data**: Use the "Copy to Clipboard" button to copy the scanned data
6. **Scan again**: Press "Scan Again" to scan another QR code

### 🔐 PIN Verification Feature

When scanning a QR code that contains patient information with a DNI field, the app will automatically:

1. Detect the DNI field in the JSON data (supports: `DNI`, `dni`, `Dni`, `document_id`, `documentId`)
2. Show a PIN verification dialog
3. Verify the entered 6-digit PIN against the first 6 digits of the DNI
4. Only show the patient information if the PIN is correct

**Example QR Code JSON:**
```json
{
  "patient_id": "P-12345",
  "name": "John Doe",
  "DNI": "12345678",
  "bed": "A-101",
  "status": "admitted"
}
```
For this example, the correct PIN would be: `123456`

## 📸 Screenshots

*(Screenshots will be added once the app is built and tested)*

## 🛠️ Build

To build the app:

```bash
./gradlew build
```

To build and install on a connected device:

```bash
./gradlew installDebug
```

## 🧪 Testing

To run tests:

```bash
./gradlew test
```

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👥 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## 🙏 Acknowledgments

- Built with Android CameraX and ML Kit
- Uses Material Design guidelines
- Follows Android best practices and MVVM architecture
