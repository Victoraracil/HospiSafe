# Implementation Summary

## Current Security Model

HospiScanner scans QR payloads, parses medical JSON, and stores validated reports in local encrypted history. Sensitive reports require an explicit PIN credential in the QR payload before any patient data is displayed.

## Key Changes

- Medical QRs with DNI or clinical fields are rejected when no PIN hash is present.
- PIN verification uses SHA-256 hashes from QR fields such as `pin_hash`, `pinHash`, `access_pin_hash`, or `accessPinHash`.
- The scanner no longer derives PINs from patient identifiers.
- Room is opened with SQLCipher using a locally generated passphrase stored in `EncryptedSharedPreferences`.
- Encrypted installs use `hospiscanner_secure.db` so older plaintext databases are not opened as encrypted files.
- Android backup and data extraction rules exclude app data.
- Release builds enable minification and resource shrinking.
- QR analysis and clipboard formatting were split out of `MainActivity`.
- Copying a medical report to the clipboard now requires confirmation.
- Stored report JSON is sanitized so PIN credential fields are not persisted in history.

## Main Components

- `QRDataParser`: JSON parsing, sensitive-data gating, PIN hashing, and PIN verification.
- `ScannerViewModel`: scanner state and pending PIN verification flow.
- `QrCodeAnalyzer`: CameraX/ML Kit barcode analysis.
- `MedicalReportClipboardFormatter`: text export for confirmed clipboard copies.
- `AppDatabase`: encrypted Room database factory.

## Validation

Unit tests cover valid/invalid JSON, sensitive medical payloads without PIN hashes, sensitive payloads with PIN hashes, and explicit hash verification.
