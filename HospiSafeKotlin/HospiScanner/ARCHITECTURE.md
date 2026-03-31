# HospiScanner Architecture

## Overview

HospiScanner is built using the **MVVM (Model-View-ViewModel)** architecture pattern, which provides clear separation of concerns and makes the codebase maintainable, testable, and scalable.

## Architecture Diagram

```
┌─────────────────┐
│      View       │ ← User Interface (MainActivity, XML Layouts)
│  (Activities/   │
│    Fragments)   │
└────────┬────────┘
         │ observes LiveData
         │ user actions
         ▼
┌─────────────────┐
│   ViewModel     │ ← Presentation Logic (ScannerViewModel)
│  (Business      │
│    Logic)       │
└────────┬────────┘
         │ uses
         │
         ▼
┌─────────────────┐
│     Model       │ ← Data & Business Rules (ScanResult, QRDataParser)
│  (Data Layer)   │
└─────────────────┘
```

## Components

### 1. Model Layer

**Location**: `com.hospiscanner.model`

**Files**:
- `ScanResult.kt`: Data class representing a scanned QR code result
- `QRDataParser`: Utility object for parsing QR data as JSON

**Responsibilities**:
- Define data structures
- Implement business logic for JSON parsing
- Handle data validation
- No UI dependencies

**Example**:
```kotlin
data class ScanResult(
    val rawData: String,
    val isValidJson: Boolean,
    val jsonData: Map<String, Any>? = null,
    val formattedJson: String? = null,
    val errorMessage: String? = null
)
```

### 2. ViewModel Layer

**Location**: `com.hospiscanner.viewmodel`

**Files**:
- `ScannerViewModel.kt`: Manages scanner state and coordinates data flow

**Responsibilities**:
- Manage UI state using LiveData
- Process QR code data
- Handle scanning lifecycle
- Provide data to the View
- Survive configuration changes (screen rotation)

**LiveData Exposed**:
- `scanResult`: Contains the parsed scan result
- `isScannerActive`: Indicates if scanner is active
- `errorMessage`: Contains error messages for the UI

**Example**:
```kotlin
class ScannerViewModel : ViewModel() {
    private val _scanResult = MutableLiveData<ScanResult?>()
    val scanResult: LiveData<ScanResult?> = _scanResult
    
    fun processQRCode(rawData: String) {
        val result = QRDataParser.parseQRData(rawData)
        _scanResult.value = result
    }
}
```

### 3. View Layer

**Location**: `com.hospiscanner.view`

**Files**:
- `MainActivity.kt`: Main activity handling UI and camera
- `activity_main.xml`: Layout defining the UI structure

**Responsibilities**:
- Display UI
- Handle user interactions
- Observe ViewModel LiveData
- Manage camera lifecycle
- Request permissions
- No business logic

**Key Features**:
- Camera preview using CameraX
- QR code detection using ML Kit
- Real-time result display
- Permission handling UI

## Data Flow

### Scanning Flow

1. **User launches app** → View checks camera permission
2. **Permission granted** → View initializes CameraX
3. **Camera detects QR code** → ML Kit processes image
4. **QR detected** → View calls `ViewModel.processQRCode(data)`
5. **ViewModel processes** → Calls `QRDataParser.parseQRData()`
6. **Model parses JSON** → Returns `ScanResult`
7. **ViewModel updates LiveData** → `_scanResult.value = result`
8. **View observes change** → UI updates with result

### Reset Flow

1. **User clicks "Scan Again"** → View calls `ViewModel.resetScanner()`
2. **ViewModel resets state** → Sets `_scanResult = null`, `_isScannerActive = true`
3. **View observes changes** → Hides result, shows camera

## Key Design Patterns

### 1. Observer Pattern
- View observes ViewModel's LiveData
- Automatic UI updates when data changes
- Lifecycle-aware observations

```kotlin
viewModel.scanResult.observe(this) { result ->
    result?.let { showResult(it) }
}
```

### 2. Single Responsibility Principle
- Each class has one clear purpose
- Model: Data and parsing
- ViewModel: State management
- View: UI rendering

### 3. Dependency Injection (Manual)
- ViewModel created using `viewModels()` delegate
- Model objects are stateless utilities
- Easy to replace with DI framework (Hilt/Dagger)

## Technology Stack

### Android Components
- **CameraX**: Modern camera API
- **ML Kit Barcode Scanning**: QR code detection
- **Lifecycle Components**: ViewModel, LiveData
- **View Binding**: Type-safe view access

### Libraries
- **Gson**: JSON parsing
- **Material Components**: UI design

## Benefits of MVVM in HospiScanner

1. **Separation of Concerns**: Clear boundaries between UI, logic, and data
2. **Testability**: ViewModel and Model can be unit tested without UI
3. **Maintainability**: Easy to understand and modify
4. **Configuration Changes**: ViewModel survives screen rotation
5. **Reactive UI**: LiveData ensures UI is always in sync with data

## File Organization

```
com.hospiscanner/
├── model/
│   └── ScanResult.kt          # Data models and parsing logic
├── viewmodel/
│   └── ScannerViewModel.kt    # Presentation logic
└── view/
    └── MainActivity.kt         # UI and user interaction

res/
├── layout/
│   └── activity_main.xml      # UI layout
├── values/
│   ├── colors.xml             # Color definitions
│   ├── strings.xml            # String resources
│   └── themes.xml             # App theme
└── drawable/
    └── scan_frame.xml         # Scan frame drawable
```

## Testing Strategy

### Unit Tests (Model Layer)
- Test JSON parsing logic
- Test data validation
- No Android dependencies

### ViewModel Tests
- Test LiveData updates
- Test business logic
- Mock Model layer

### UI Tests (View Layer)
- Test user interactions
- Test permission flow
- Test camera integration

## Future Enhancements

1. **Repository Pattern**: Add a Repository layer for data source abstraction
2. **Dependency Injection**: Integrate Hilt for automatic DI
3. **Use Cases/Interactors**: Add domain layer for complex business logic
4. **Coroutines**: Add for asynchronous operations
5. **Room Database**: Persist scan history
6. **Navigation Component**: For multi-screen navigation

## References

- [Android Architecture Components](https://developer.android.com/topic/architecture)
- [Guide to app architecture](https://developer.android.com/topic/architecture/intro)
- [MVVM Pattern](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93viewmodel)
