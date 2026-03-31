# Sample QR Code Test Cases

This directory contains sample QR code data in JSON format for testing the HospiScanner app.

## Valid JSON Examples

### Patient Information
```json
{
  "patient_id": "P-12345",
  "name": "John Doe",
  "bed": "A-101",
  "status": "admitted",
  "admission_date": "2026-02-18"
}
```

### Patient Information with DNI (for PIN verification)
```json
{
  "patient_id": "P-12345",
  "name": "John Doe",
  "DNI": "12345678",
  "bed": "A-101",
  "status": "admitted",
  "admission_date": "2026-02-18"
}
```
**Note:** When this QR code is scanned, it will prompt for PIN verification. The PIN is the first 6 digits of the DNI: `123456`

### Patient Information with DNI (alternative format)
```json
{
  "patient_id": "P-67890",
  "name": "Maria Garcia",
  "dni": "98765432",
  "bed": "B-205",
  "status": "observation",
  "admission_date": "2026-02-17"
}
```
**Note:** The PIN would be: `987654`

### Medical Record
```json
{
  "record_id": "MR-67890",
  "patient": "Maria Garcia",
  "diagnosis": "Observation",
  "attending_physician": "Dr. Smith",
  "room": "ICU-205"
}
```

### Medication Information
```json
{
  "medication_id": "MED-445566",
  "name": "Paracetamol",
  "dosage": "500mg",
  "frequency": "Every 6 hours",
  "patient_bed": "B-302"
}
```

### Equipment Tracking
```json
{
  "equipment_id": "EQ-778899",
  "type": "Wheelchair",
  "location": "Floor 3",
  "status": "available",
  "last_maintenance": "2026-02-15"
}
```

### Staff Badge
```json
{
  "staff_id": "ST-123456",
  "name": "Nurse Johnson",
  "department": "Emergency",
  "shift": "Night",
  "access_level": "3"
}
```

### Laboratory Sample
```json
{
  "sample_id": "LAB-998877",
  "patient_id": "P-12345",
  "test_type": "Blood Test",
  "collected_by": "Tech Williams",
  "timestamp": "2026-02-18T08:30:00Z"
}
```

## Invalid Examples (for error handling testing)

### Plain Text
```
This is just plain text, not JSON
```

### Malformed JSON
```json
{"name":"Test", "age":30,}
```

### Empty String
```

```

## Testing Instructions

1. **Using a QR Code Generator**:
   - Visit https://www.qr-code-generator.com/ or similar
   - Paste the JSON text above
   - Generate the QR code
   - Save or print the QR code

2. **Testing with the App**:
   - Open HospiScanner app
   - Point camera at the generated QR code
   - Observe the parsing result

3. **Expected Behavior**:
   - ✅ Valid JSON: Should display formatted JSON with green indicator
   - ❌ Invalid JSON: Should show error message with red indicator
   - 📋 Copy feature: Should copy the parsed/raw data to clipboard
