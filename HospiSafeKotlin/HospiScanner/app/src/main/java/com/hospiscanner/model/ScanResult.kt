package com.hospiscanner.model

import com.google.gson.GsonBuilder
import com.google.gson.JsonSyntaxException
import com.google.gson.reflect.TypeToken
import java.security.MessageDigest

/**
 * Data class representing the scan result
 */
data class ScanResult(
    val rawData: String,
    val isValidJson: Boolean,
    val jsonData: Map<String, Any>? = null,
    val formattedJson: String? = null,
    val errorMessage: String? = null,
    val dni: String? = null,
    val requiresPinVerification: Boolean = false,
    val medicalReport: MedicalReport? = null,
    val accessPinHash: String? = null
)

/**
 * Utility class for parsing QR code data as JSON
 */
object QRDataParser {
    private val gson = GsonBuilder().setPrettyPrinting().create()
    private val jsonMapType = object : TypeToken<Map<String, Any>>() {}.type
    
    /**
     * Parse the scanned QR code data as JSON
     */
    fun parseQRData(rawData: String): ScanResult {
        return try {
            if (rawData.isBlank()) {
                return ScanResult(
                    rawData = rawData,
                    isValidJson = false,
                    errorMessage = "QR data is empty"
                )
            }

            // Try to parse as JSON
            val jsonMap = gson.fromJson<Map<String, Any>>(rawData, jsonMapType)
            
            if (jsonMap != null) {
                val medicalReport = MedicalReport.fromJsonMap(jsonMap)
                val pinHash = extractPinHash(jsonMap)
                val containsSensitiveData = medicalReport.hasSensitiveMedicalData()

                if (containsSensitiveData && pinHash == null) {
                    return ScanResult(
                        rawData = rawData,
                        isValidJson = false,
                        errorMessage = "Medical QR is missing a PIN hash"
                    )
                }

                // Format JSON for display
                val formattedJson = gson.toJson(jsonMap.filterKeys { it !in PIN_KEYS })
                
                // Extract DNI field (try different possible field names)
                val dni = extractDNI(jsonMap)
                val requiresPinVerification = pinHash != null

                ScanResult(
                    rawData = rawData,
                    isValidJson = true,
                    jsonData = jsonMap,
                    formattedJson = formattedJson,
                    dni = dni,
                    requiresPinVerification = requiresPinVerification,
                    medicalReport = medicalReport,
                    accessPinHash = pinHash
                )
            } else {
                ScanResult(
                    rawData = rawData,
                    isValidJson = false,
                    errorMessage = "Could not parse as JSON object"
                )
            }
        } catch (e: JsonSyntaxException) {
            // Not valid JSON
            ScanResult(
                rawData = rawData,
                isValidJson = false,
                errorMessage = "Invalid JSON format: ${e.message}"
            )
        } catch (e: Exception) {
            // Other parsing errors
            ScanResult(
                rawData = rawData,
                isValidJson = false,
                errorMessage = "Error parsing data: ${e.message}"
            )
        }
    }
    
    /**
     * Extract DNI from JSON data - try different field name variations
     */
    private fun extractDNI(jsonMap: Map<String, Any>): String? {
        // Try different possible field names for DNI
        val possibleKeys = listOf("DNI", "dni", "Dni", "document_id", "documentId")
        
        for (key in possibleKeys) {
            val value = jsonMap[key]
            if (value != null) {
                return value.toString()
            }
        }
        
        return null
    }

    private fun extractPinHash(jsonMap: Map<String, Any>): String? {
        for (key in PIN_HASH_KEYS) {
            val value = jsonMap[key]?.toString()?.trim()
            if (!value.isNullOrBlank()) return value.lowercase()
        }

        for (key in PIN_PLAIN_TEXT_KEYS) {
            val value = jsonMap[key]?.toString()?.trim()
            if (!value.isNullOrBlank()) return hashPin(value)
        }

        return null
    }
    
    /**
     * Hash a PIN for verification. QRs should provide one of the pin hash keys
     * instead of deriving access from a patient identifier.
     */
    fun hashPin(pin: String): String {
        val digest = MessageDigest.getInstance("SHA-256")
            .digest(pin.toByteArray(Charsets.UTF_8))
        return digest.joinToString("") { "%02x".format(it) }
    }

    fun verifyPin(enteredPin: String, expectedHash: String): Boolean {
        return enteredPin.length == 6 &&
            enteredPin.all { it.isDigit() } &&
            hashPin(enteredPin) == expectedHash.lowercase()
    }

    private val PIN_HASH_KEYS = setOf(
        "pin_hash",
        "pinHash",
        "access_pin_hash",
        "accessPinHash",
        "verification_hash",
        "verificationHash"
    )

    private val PIN_PLAIN_TEXT_KEYS = setOf(
        "pin",
        "access_pin",
        "accessPin"
    )

    private val PIN_KEYS = PIN_HASH_KEYS + PIN_PLAIN_TEXT_KEYS
}

private fun MedicalReport.hasSensitiveMedicalData(): Boolean {
    return !dni.isNullOrBlank() ||
        !diagnosis.isNullOrBlank() ||
        !treatment.isNullOrBlank() ||
        !medications.isNullOrBlank() ||
        !allergies.isNullOrBlank()
}
