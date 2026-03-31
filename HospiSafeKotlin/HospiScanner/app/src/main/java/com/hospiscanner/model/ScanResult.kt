package com.hospiscanner.model

import com.google.gson.Gson
import com.google.gson.JsonSyntaxException

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
    val medicalReport: MedicalReport? = null
)

/**
 * Utility class for parsing QR code data as JSON
 */
object QRDataParser {
    private val gson = Gson()
    
    /**
     * Parse the scanned QR code data as JSON
     */
    fun parseQRData(rawData: String): ScanResult {
        return try {
            // Try to parse as JSON
            val jsonMap = gson.fromJson(rawData, Map::class.java) as? Map<String, Any>
            
            if (jsonMap != null) {
                // Format JSON for display
                val formattedJson = gson.toJson(jsonMap)
                    .replace(",", ",\n")
                    .replace("{", "{\n  ")
                    .replace("}", "\n}")
                
                // Extract DNI field (try different possible field names)
                val dni = extractDNI(jsonMap)
                val requiresPinVerification = dni != null && dni.filter { it.isDigit() }.length >= 6
                
                // Parse medical report
                val medicalReport = MedicalReport.fromJsonMap(jsonMap)

                ScanResult(
                    rawData = rawData,
                    isValidJson = true,
                    jsonData = jsonMap,
                    formattedJson = formattedJson,
                    dni = dni,
                    requiresPinVerification = requiresPinVerification,
                    medicalReport = medicalReport
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
    
    /**
     * Extract the first 6 digits from DNI for PIN verification
     */
    fun extractPinFromDNI(dni: String): String {
        // Extract only digits from the DNI
        val digits = dni.filter { it.isDigit() }
        // Return first 6 digits
        return digits.take(6)
    }
}
