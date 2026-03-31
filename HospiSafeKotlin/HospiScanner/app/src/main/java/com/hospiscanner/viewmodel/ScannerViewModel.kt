package com.hospiscanner.viewmodel

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import com.hospiscanner.model.QRDataParser
import com.hospiscanner.model.ScanResult

/**
 * ViewModel for managing QR scanner state and data (MVVM pattern)
 */
class ScannerViewModel : ViewModel() {
    
    // LiveData for scan result
    private val _scanResult = MutableLiveData<ScanResult?>()
    val scanResult: LiveData<ScanResult?> = _scanResult
    
    // LiveData for pending scan result (before PIN verification)
    private val _pendingScanResult = MutableLiveData<ScanResult?>()
    val pendingScanResult: LiveData<ScanResult?> = _pendingScanResult
    
    // LiveData for scanner state
    private val _isScannerActive = MutableLiveData(true)
    val isScannerActive: LiveData<Boolean> = _isScannerActive
    
    // LiveData for error messages
    private val _errorMessage = MutableLiveData<String?>()
    val errorMessage: LiveData<String?> = _errorMessage
    
    /**
     * Process scanned QR code data
     */
    fun processQRCode(rawData: String) {
        try {
            val result = QRDataParser.parseQRData(rawData)
            
            if (result.requiresPinVerification) {
                // Store result for PIN verification
                _pendingScanResult.value = result
                _isScannerActive.value = false
            } else {
                // No PIN required, show result directly
                _scanResult.value = result
                _isScannerActive.value = false
            }
        } catch (e: Exception) {
            _errorMessage.value = "Error processing QR code: ${e.message}"
        }
    }
    
    /**
     * Verify PIN and show result if correct
     */
    fun verifyPin(enteredPin: String): Boolean {
        val pending = _pendingScanResult.value ?: return false
        val dni = pending.dni ?: return false
        
        val correctPin = QRDataParser.extractPinFromDNI(dni)
        
        return if (enteredPin == correctPin) {
            // PIN is correct, show the result
            _scanResult.value = pending
            _pendingScanResult.value = null
            true
        } else {
            // PIN is incorrect
            false
        }
    }
    
    /**
     * Cancel PIN verification and return to scanner
     */
    fun cancelPinVerification() {
        _pendingScanResult.value = null
        _isScannerActive.value = true
    }
    
    /**
     * Reset scanner to scan again
     */
    fun resetScanner() {
        _scanResult.value = null
        _pendingScanResult.value = null
        _isScannerActive.value = true
        _errorMessage.value = null
    }
    
    /**
     * Handle scanning error
     */
    fun onScanError(error: String) {
        _errorMessage.value = error
    }
    
    /**
     * Clear error message
     */
    fun clearError() {
        _errorMessage.value = null
    }
    
    override fun onCleared() {
        super.onCleared()
        _scanResult.value = null
        _pendingScanResult.value = null
        _errorMessage.value = null
    }
}
