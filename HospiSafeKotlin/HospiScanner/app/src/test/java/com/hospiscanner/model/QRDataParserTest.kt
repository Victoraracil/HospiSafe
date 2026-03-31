package com.hospiscanner.model

import org.junit.Test
import org.junit.Assert.*

/**
 * Unit tests for QRDataParser
 */
class QRDataParserTest {
    
    @Test
    fun parseValidJson_ReturnsValidScanResult() {
        // Given
        val validJson = """{"name":"John Doe","age":30,"city":"New York"}"""
        
        // When
        val result = QRDataParser.parseQRData(validJson)
        
        // Then
        assertTrue(result.isValidJson)
        assertNotNull(result.jsonData)
        assertEquals(validJson, result.rawData)
        assertNull(result.errorMessage)
        assertNotNull(result.formattedJson)
    }
    
    @Test
    fun parseInvalidJson_ReturnsInvalidScanResult() {
        // Given
        val invalidJson = """{"name":"John Doe", "age":30,}""" // Trailing comma
        
        // When
        val result = QRDataParser.parseQRData(invalidJson)
        
        // Then
        assertFalse(result.isValidJson)
        assertNull(result.jsonData)
        assertEquals(invalidJson, result.rawData)
        assertNotNull(result.errorMessage)
    }
    
    @Test
    fun parseNonJsonString_ReturnsInvalidScanResult() {
        // Given
        val plainText = "This is just plain text, not JSON"
        
        // When
        val result = QRDataParser.parseQRData(plainText)
        
        // Then
        assertFalse(result.isValidJson)
        assertNull(result.jsonData)
        assertEquals(plainText, result.rawData)
        assertNotNull(result.errorMessage)
    }
    
    @Test
    fun parseEmptyString_ReturnsInvalidScanResult() {
        // Given
        val emptyString = ""
        
        // When
        val result = QRDataParser.parseQRData(emptyString)
        
        // Then
        assertFalse(result.isValidJson)
        assertEquals(emptyString, result.rawData)
    }
    
    @Test
    fun parseNestedJson_ReturnsValidScanResult() {
        // Given
        val nestedJson = """{"user":{"name":"Jane","email":"jane@example.com"},"status":"active"}"""
        
        // When
        val result = QRDataParser.parseQRData(nestedJson)
        
        // Then
        assertTrue(result.isValidJson)
        assertNotNull(result.jsonData)
        assertEquals(nestedJson, result.rawData)
    }
    
    @Test
    fun parseJsonWithArray_ReturnsValidScanResult() {
        // Given
        val jsonWithArray = """{"items":["apple","banana","orange"],"count":3}"""
        
        // When
        val result = QRDataParser.parseQRData(jsonWithArray)
        
        // Then
        assertTrue(result.isValidJson)
        assertNotNull(result.jsonData)
    }
    
    @Test
    fun parseJsonWithSpecialCharacters_ReturnsValidScanResult() {
        // Given
        val jsonWithSpecialChars = """{"message":"Hello, World! 🌍","emoji":"😀"}"""
        
        // When
        val result = QRDataParser.parseQRData(jsonWithSpecialChars)
        
        // Then
        assertTrue(result.isValidJson)
        assertNotNull(result.jsonData)
    }
}
