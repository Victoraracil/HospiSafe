# PIN Verification Feature

## Overview

The PIN verification feature adds an extra layer of security when scanning QR codes containing patient information. When a QR code includes a DNI (Documento Nacional de Identidad) field, the app will prompt for a 6-digit PIN before displaying the data.

## How It Works

### 1. DNI Field Detection

The parser automatically looks for DNI in the JSON data using multiple field name variations:
- `DNI`
- `dni`
- `Dni`
- `document_id`
- `documentId`

### 2. PIN Generation

The PIN is the **first 6 digits** extracted from the DNI:
- DNI: `12345678` → PIN: `123456`
- DNI: `98765432` → PIN: `987654`
- DNI: `ABC123456789` → PIN: `123456` (only digits are used)

### 3. Verification Flow

```
┌────────────────────────────────────────────┐
│  1. Scan QR Code                           │
│     ↓                                      │
│  2. Parse JSON                             │
│     ↓                                      │
│  3. Check for DNI field                    │
│     ↓                                      │
│  ┌─────────────────────┐                  │
│  │ DNI field present?  │                  │
│  └─────┬──────────┬────┘                  │
│        │ NO       │ YES                    │
│        ↓          ↓                        │
│  ┌─────────┐  ┌────────────────┐         │
│  │ Show    │  │ Show PIN Dialog│         │
│  │ Result  │  │                │         │
│  │ Screen  │  │ User enters PIN│         │
│  └─────────┘  └────────┬───────┘         │
│                        ↓                   │
│                 ┌──────────────┐          │
│                 │ Verify PIN   │          │
│                 └─┬──────────┬─┘          │
│                   │ Correct  │ Incorrect  │
│                   ↓          ↓            │
│            ┌─────────┐  ┌────────────┐   │
│            │ Show    │  │ Show Error │   │
│            │ Result  │  │ Retry      │   │
│            └─────────┘  └────────────┘   │
└────────────────────────────────────────────┘
```

## Implementation Details

### Model Layer (`ScanResult.kt`)

**ScanResult Data Class:**
```kotlin
data class ScanResult(
    val rawData: String,
    val isValidJson: Boolean,
    val jsonData: Map<String, Any>? = null,
    val formattedJson: String? = null,
    val errorMessage: String? = null,
    val dni: String? = null,                    // ← DNI field
    val requiresPinVerification: Boolean = false // ← PIN flag
)
```

**Key Functions:**
- `parseQRData()`: Parses JSON and extracts DNI
- `extractDNI()`: Looks for DNI field with multiple naming variations
- `extractPinFromDNI()`: Extracts first 6 digits from DNI

### ViewModel Layer (`ScannerViewModel.kt`)

**New LiveData:**
- `pendingScanResult`: Holds scan result while waiting for PIN verification

**Key Functions:**
- `processQRCode()`: Routes to PIN dialog if DNI present
- `verifyPin()`: Validates entered PIN against DNI
- `cancelPinVerification()`: Returns to scanner without showing result

### View Layer (`MainActivity.kt`)

**New Observer:**
- `pendingScanResult.observe()`: Shows PIN dialog when result pending

**Key Functions:**
- `showPinDialog()`: Displays PIN verification dialog
- PIN validation with user feedback
- Error handling for incorrect PIN attempts

## UI Components

### PIN Dialog (`dialog_pin_verification.xml`)

**Components:**
- Title: "Verificación PIN"
- Message: "Ingrese el PIN de 6 dígitos del paciente"
- PIN Input: 6-digit numeric field
- Error Text: Shows validation errors
- Cancel Button: Returns to scanner
- Confirm Button: Validates PIN

**String Resources:**
```xml
<string name="pin_verification_title">Verificación PIN</string>
<string name="pin_verification_message">Ingrese el PIN de 6 dígitos del paciente</string>
<string name="pin_hint">PIN (6 dígitos)</string>
<string name="confirm">Confirmar</string>
<string name="cancel">Cancelar</string>
<string name="pin_error_length">El PIN debe tener 6 dígitos</string>
<string name="pin_error_incorrect">PIN incorrecto. Intente nuevamente</string>
<string name="pin_error_no_dni">No se encontró el campo DNI en el código QR</string>
<string name="pin_error_dni_short">El DNI debe tener al menos 6 dígitos</string>
```

## Usage Examples

### Example 1: Patient QR with DNI

**QR Code JSON:**
```json
{
  "patient_id": "P-12345",
  "name": "Juan Pérez",
  "DNI": "12345678",
  "bed": "A-101",
  "status": "admitted",
  "admission_date": "2026-02-18"
}
```

**Flow:**
1. User scans QR code
2. PIN dialog appears
3. User enters: `123456`
4. PIN verified ✓
5. Result screen shows patient data

### Example 2: Equipment QR without DNI

**QR Code JSON:**
```json
{
  "equipment_id": "EQ-778899",
  "type": "Wheelchair",
  "location": "Floor 3",
  "status": "available"
}
```

**Flow:**
1. User scans QR code
2. No PIN dialog (no DNI field)
3. Result screen shows equipment data immediately

### Example 3: Incorrect PIN Attempt

**QR Code JSON:**
```json
{
  "patient_id": "P-67890",
  "name": "María García",
  "dni": "98765432",
  "bed": "B-205"
}
```

**Flow:**
1. User scans QR code
2. PIN dialog appears
3. User enters: `111111` (incorrect)
4. Error message: "PIN incorrecto. Intente nuevamente"
5. User can retry or cancel

## Security Considerations

### Advantages
- **Patient Privacy**: Prevents unauthorized viewing of patient data
- **Simple Authentication**: Easy for authorized staff to remember (first 6 digits of DNI)
- **Optional**: Only activates when DNI field is present
- **Flexible**: Supports multiple DNI field naming conventions

### Limitations
- **Local Validation**: PIN validation happens on device (no server verification)
- **Known Secret**: PIN is based on a somewhat predictable pattern
- **No Lockout**: No limit on PIN attempts (by design for medical emergency access)

### Recommendations for Production
1. Consider adding server-side validation
2. Implement audit logging for PIN attempts
3. Add optional biometric authentication
4. Consider time-limited access tokens
5. Implement role-based access control

## Testing

### Test Cases

1. **Valid PIN Entry**
   - Scan QR with DNI: "12345678"
   - Enter PIN: "123456"
   - Expected: Access granted ✓

2. **Invalid PIN Entry**
   - Scan QR with DNI: "12345678"
   - Enter PIN: "654321"
   - Expected: Error message shown

3. **Short PIN**
   - Scan QR with DNI: "12345678"
   - Enter PIN: "123"
   - Expected: "El PIN debe tener 6 dígitos"

4. **Cancel PIN Dialog**
   - Scan QR with DNI
   - Press "Cancelar"
   - Expected: Return to scanner

5. **No DNI Field**
   - Scan QR without DNI
   - Expected: Show result immediately, no PIN dialog

6. **DNI with Letters**
   - Scan QR with DNI: "ABC123456DEF"
   - Expected: PIN extracted as "123456"

### Sample QR Codes

See [TEST_CASES.md](TEST_CASES.md) for complete list of test QR codes with DNI fields.

## Future Enhancements

1. **Biometric Authentication**: Add fingerprint/face authentication option
2. **PIN Complexity**: Support variable length PINs or custom PIN rules
3. **Offline/Online Mode**: Server validation when connected
4. **Access Logging**: Track who accessed patient data and when
5. **Time-based PINs**: PINs that change based on time for extra security
6. **Custom Field Selection**: Allow configuration of which field to use for PIN
7. **Multiple Verification Factors**: Combine PIN with other authentication methods
