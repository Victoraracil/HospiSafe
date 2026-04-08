# HospiScanner - Features & UI Flow

## App Flow Diagram

```
┌─────────────────────────────────────────┐
│         App Launch (MainActivity)       │
└───────────────┬─────────────────────────┘
                │
                ▼
┌─────────────────────────────────────────┐
│    Check Camera Permission              │
└───────┬────────────────────┬────────────┘
        │                    │
   ✓ Granted            ✗ Not Granted
        │                    │
        ▼                    ▼
┌───────────────┐  ┌─────────────────────┐
│  Start Camera │  │  Permission Request │
│  Preview      │  │  Screen             │
└───────┬───────┘  └──────────┬──────────┘
        │                     │
        │              User Grants/Denies
        │                     │
        │ ◄───────────────────┘
        │
        ▼
┌─────────────────────────────────────────┐
│    Camera Preview + QR Scanning         │
│    ┌─────────────────────────────┐      │
│    │                             │      │
│    │    [Camera Preview]         │      │
│    │                             │      │
│    │      ┌──────────┐          │      │
│    │      │ Scan     │          │      │
│    │      │ Frame    │          │      │
│    │      └──────────┘          │      │
│    │                             │      │
│    └─────────────────────────────┘      │
│    Status: Scanning...                  │
└───────────────┬─────────────────────────┘
                │
         QR Code Detected
                │
                ▼
┌─────────────────────────────────────────┐
│    Process QR Data (ViewModel)          │
│    └─> Parse JSON (Model)               │
└───────┬──────────────┬──────────────────┘
        │              │
   ✓ Valid JSON   ✗ Invalid JSON
        │              │
        ▼              │
    Has DNI?           │
    ┌───┴───┐          │
    │       │          │
  Yes      No          │
    │       │          │
    ▼       ▼          ▼
┌──────┐ ┌──────────────────┐
│ PIN  │ │  Result Screen   │
│Dialog│ │  ✅ Success      │
│      │ │                  │
│Enter │ │  Raw Data        │
│6-digit│ │  Parsed JSON    │
│      │ │                  │
│[Cancel]│ │  [Copy] [Scan]  │
│[Confirm]│ └──────────────┘
└──┬───┘ └───────┬──────────┘
   │             │
PIN OK│    PIN Cancel
   │             │
   ▼             ▼
┌──────────────────┐
│  Result Screen   │
│  ✅ Success      │
│                  │
│  Raw Data        │
│  Parsed JSON     │
│                  │
│  [Copy] [Scan]   │
└───────┬──────────┘
        │
        └─────────┬─────────┘
                  │
          User clicks "Scan Again"
                  │
                  ▼
          Back to Camera Preview
```

## Feature Highlights

### 1. 📸 Camera Integration
- **CameraX API**: Modern, lifecycle-aware camera implementation
- **Real-time Preview**: Live camera feed with scanning overlay
- **Auto-focus**: Automatic focus for optimal QR code detection
- **Portrait Mode**: App locked to portrait orientation for better UX

### 2. 🔍 QR Code Detection
- **ML Kit**: Google's machine learning-powered barcode scanner
- **Fast Detection**: Real-time QR code recognition
- **Multiple Formats**: Supports various QR code types
- **Visual Feedback**: Scan frame overlay guides users

### 3. 📝 JSON Parsing
- **Automatic Validation**: Instant JSON format checking
- **Pretty Formatting**: Readable JSON display with indentation
- **Error Handling**: Clear error messages for invalid JSON
- **Type Flexibility**: Handles nested objects and arrays
- **DNI Detection**: Automatically detects DNI/dni field in JSON

### 4. 🔐 PIN Verification
- **Patient Privacy**: 6-digit PIN verification for patient data
- **DNI-based PIN**: PIN is first 6 digits of patient's DNI
- **Popup Dialog**: Clean, focused verification interface
- **Error Handling**: Clear feedback for incorrect PIN attempts
- **Flexible Field Names**: Supports DNI, dni, Dni, document_id, documentId
- **Optional Feature**: Only activates when DNI field is present

### 5. 🎨 UI/UX Design

#### Color Palette
- **Primary (Blue)**: #2196F3 - Professional, trustworthy
- **Accent (Teal)**: #00BCD4 - Vibrant, energetic
- **Success (Green)**: #4CAF50 - Positive feedback
- **Error (Red)**: #F44336 - Clear warnings
- **Background**: Clean white/dark based on theme

#### Design Principles
- **Material Design 3**: Modern Android design guidelines
- **High Contrast**: Readable text and clear hierarchy
- **Vibrant Colors**: Eye-catching but professional
- **Smooth Transitions**: Polished user experience
- **Dark Mode**: Full dark theme support

### 6. 🔐 Permission Management
- **Runtime Permissions**: Requests camera permission when needed
- **Clear Messaging**: Explains why permission is required
- **Graceful Handling**: User-friendly permission denial flow
- **Easy Recovery**: Simple button to re-request permission

### 7. 📋 Result Management
- **Dual Display**: Shows both raw and parsed data
- **Status Indicators**: Color-coded success/error states
- **Copy Function**: One-tap clipboard copy
- **Scan Again**: Quick return to scanning mode

## Screen Layouts

### Scanner Screen
```
┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓
┃ HospiScanner                         ┃ ← Blue header
┃ Point your camera at a QR code       ┃
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
┃                                       ┃
┃         Camera Preview                ┃
┃                                       ┃
┃           ╔═══════════╗              ┃ ← Teal scan frame
┃           ║           ║              ┃
┃           ║           ║              ┃
┃           ╚═══════════╝              ┃
┃                                       ┃
┃                                       ┃
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
┃      ⟳  Scanning...                  ┃ ← Status bar
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
```

### Result Screen (Valid JSON)
```
┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓
┃ Scan Result                          ┃ ← Blue header
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓
┃      ✓ Valid JSON                    ┃ ← Green indicator
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓
┃ Raw Data:                            ┃
┃ {"name":"John","age":30}             ┃
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓
┃ Parsed JSON:                         ┃
┃ {                                    ┃
┃   "name": "John",                    ┃
┃   "age": 30                          ┃
┃ }                                    ┃
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓
┃  [Copy to Clipboard]  [Scan Again]  ┃ ← Action buttons
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
```

### Result Screen (Invalid JSON)
```
┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓
┃ Scan Result                          ┃ ← Blue header
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓
┃      ✗ Invalid JSON                  ┃ ← Red indicator
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓
┃ Raw Data:                            ┃
┃ This is not JSON                     ┃
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓
┃ Error:                               ┃
┃ Invalid JSON format: Expected value  ┃
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓
┃  [Copy to Clipboard]  [Scan Again]  ┃ ← Action buttons
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
```

## User Interactions

### 1. First Launch
```
User opens app
    → App checks camera permission
    → Permission not granted
    → Shows permission request screen
    → User taps "Grant Permission"
    → System permission dialog appears
    → User grants permission
    → Camera starts
```

### 2. Scanning Flow (without DNI)
```
Camera preview active
    → User points at QR code
    → ML Kit detects code
    → ViewModel processes data
    → Model parses JSON
    → Result screen appears
    → User views result
```

### 3. Scanning Flow (with DNI - PIN verification)
```
Camera preview active
    → User points at QR code with DNI field
    → ML Kit detects code
    → ViewModel processes data
    → Model parses JSON and extracts DNI
    → PIN dialog appears
    → User enters 6-digit PIN
    → PIN validated against first 6 digits of DNI
    → If correct: Result screen appears
    → If incorrect: Error message shown, retry
    → User can cancel to return to scanner
```

### 4. Copy to Clipboard
```
Result screen displayed
    → User taps "Copy to Clipboard"
    → Data copied to clipboard
    → Toast notification: "Copied to clipboard"
    → User can paste in other apps
```

### 5. Scan Again
```
Result screen displayed
    → User taps "Scan Again"
    → ViewModel resets state
    → Camera preview returns
    → Ready for new scan
```

## Technical Features

### MVVM Architecture Benefits
- **Separation of Concerns**: Clean code organization
- **Testability**: Easy unit testing
- **Maintainability**: Clear structure
- **Lifecycle Awareness**: Survives configuration changes

### LiveData Advantages
- **Reactive UI**: Automatic updates
- **Lifecycle Safe**: No memory leaks
- **Observer Pattern**: Clean communication

### CameraX Benefits
- **Lifecycle Aware**: Automatic management
- **Consistent API**: Works across devices
- **Modern**: Latest camera features

## Performance Considerations

- **Single Thread Executor**: Efficient camera processing
- **Keep Latest Strategy**: Prevents frame buildup
- **Lifecycle Binding**: Automatic cleanup
- **View Binding**: Type-safe, no findViewById
- **Minimal UI Updates**: Only when needed

## Future Enhancements

1. **Scan History**: Save previous scans
2. **Export Options**: Share or export results
3. **Batch Scanning**: Multiple QR codes
4. **Custom Themes**: User-selected colors
5. **Sound Feedback**: Audio confirmation
6. **Flashlight Toggle**: Low-light scanning
7. **Manual Input**: Type QR data
8. **QR Generation**: Create QR codes from JSON
