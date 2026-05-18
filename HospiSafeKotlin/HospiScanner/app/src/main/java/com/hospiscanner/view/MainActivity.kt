package com.hospiscanner.view

import android.Manifest
import android.content.ClipData
import android.content.ClipboardManager
import android.content.Context
import android.content.Intent
import android.content.pm.PackageManager
import android.os.Bundle
import android.util.Log
import android.view.View
import android.widget.Toast
import androidx.activity.result.contract.ActivityResultContracts
import androidx.activity.viewModels
import androidx.appcompat.app.AlertDialog
import androidx.appcompat.app.AppCompatActivity
import androidx.camera.core.*
import androidx.camera.lifecycle.ProcessCameraProvider
import androidx.core.content.ContextCompat
import androidx.lifecycle.lifecycleScope
import com.google.android.material.textfield.TextInputEditText
import com.google.mlkit.vision.barcode.BarcodeScanning
import com.hospiscanner.R
import com.hospiscanner.data.database.AppDatabase
import com.hospiscanner.databinding.ActivityMainBinding
import com.hospiscanner.repository.MedicalReportRepository
import com.hospiscanner.view.detail.ReportDetailActivity
import com.hospiscanner.view.formatter.MedicalReportClipboardFormatter
import com.hospiscanner.view.history.HistoryActivity
import com.hospiscanner.view.scanner.QrCodeAnalyzer
import com.hospiscanner.viewmodel.ScannerViewModel
import kotlinx.coroutines.launch
import java.util.concurrent.ExecutorService
import java.util.concurrent.Executors

class MainActivity : AppCompatActivity() {
    
    private lateinit var binding: ActivityMainBinding
    private val viewModel: ScannerViewModel by viewModels()
    
    private var cameraProvider: ProcessCameraProvider? = null
    private var camera: Camera? = null
    private var imageAnalyzer: ImageAnalysis? = null
    private lateinit var cameraExecutor: ExecutorService
    private var currentSavedReportId: Long? = null
    private val reportRepository: MedicalReportRepository by lazy {
        MedicalReportRepository(AppDatabase.getInstance(applicationContext).medicalReportDao())
    }
    
    private val barcodeScanner = BarcodeScanning.getClient()
    
    private val requestPermissionLauncher = registerForActivityResult(
        ActivityResultContracts.RequestPermission()
    ) { isGranted ->
        if (isGranted) {
            startCamera()
        } else {
            showPermissionDenied()
        }
    }
    
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityMainBinding.inflate(layoutInflater)
        setContentView(binding.root)
        
        cameraExecutor = Executors.newSingleThreadExecutor()
        
        setupObservers()
        setupClickListeners()
        checkCameraPermission()
    }
    
    private fun setupObservers() {
        viewModel.scanResult.observe(this) { result ->
            result?.let {
                showResult(it)
            }
        }
        
        viewModel.pendingScanResult.observe(this) { result ->
            result?.let {
                showPinDialog(it)
            }
        }
        
        viewModel.isScannerActive.observe(this) { isActive ->
            if (isActive) {
                showScanner()
            }
        }
        
        viewModel.errorMessage.observe(this) { error ->
            error?.let {
                Toast.makeText(this, it, Toast.LENGTH_SHORT).show()
                viewModel.clearError()
            }
        }
    }
    
    private fun setupClickListeners() {
        binding.grantPermissionButton.setOnClickListener {
            requestCameraPermission()
        }
        
        binding.scanAgainButton.setOnClickListener {
            viewModel.resetScanner()
        }
        
        binding.copyButton.setOnClickListener {
            confirmCopyToClipboard()
        }

        binding.historyButton.setOnClickListener {
            startActivity(Intent(this, HistoryActivity::class.java))
        }

        binding.detailButton.setOnClickListener {
            val reportId = currentSavedReportId
            if (reportId != null) {
                startActivity(
                    Intent(this, ReportDetailActivity::class.java)
                        .putExtra(ReportDetailActivity.EXTRA_REPORT_ID, reportId)
                )
            } else {
                Toast.makeText(this, getString(R.string.report_not_found), Toast.LENGTH_SHORT).show()
            }
        }
    }
    
    private fun checkCameraPermission() {
        when {
            ContextCompat.checkSelfPermission(
                this,
                Manifest.permission.CAMERA
            ) == PackageManager.PERMISSION_GRANTED -> {
                startCamera()
            }
            else -> {
                showPermissionRequired()
            }
        }
    }
    
    private fun requestCameraPermission() {
        requestPermissionLauncher.launch(Manifest.permission.CAMERA)
    }
    
    private fun startCamera() {
        val cameraProviderFuture = ProcessCameraProvider.getInstance(this)
        
        cameraProviderFuture.addListener({
            try {
                cameraProvider = cameraProviderFuture.get()
                bindCameraUseCases()
            } catch (e: Exception) {
                Log.e(TAG, "Camera initialization failed", e)
                viewModel.onScanError(getString(R.string.error_camera_init))
            }
        }, ContextCompat.getMainExecutor(this))
    }
    
    private fun bindCameraUseCases() {
        val cameraProvider = cameraProvider ?: return
        
        val preview = Preview.Builder()
            .build()
            .also {
                it.setSurfaceProvider(binding.previewView.surfaceProvider)
            }
        
        imageAnalyzer = ImageAnalysis.Builder()
            .setBackpressureStrategy(ImageAnalysis.STRATEGY_KEEP_ONLY_LATEST)
            .build()
            .also {
                it.setAnalyzer(
                    cameraExecutor,
                    QrCodeAnalyzer(
                        barcodeScanner = barcodeScanner,
                        isScannerActive = { viewModel.isScannerActive.value == true },
                        onQrDetected = { qrData ->
                            runOnUiThread {
                                viewModel.processQRCode(qrData)
                            }
                        }
                    )
                )
            }
        
        val cameraSelector = CameraSelector.DEFAULT_BACK_CAMERA
        
        try {
            cameraProvider.unbindAll()
            camera = cameraProvider.bindToLifecycle(
                this,
                cameraSelector,
                preview,
                imageAnalyzer
            )
        } catch (e: Exception) {
            Log.e(TAG, "Use case binding failed", e)
            viewModel.onScanError(getString(R.string.error_camera_init))
        }
    }
    
    private fun showScanner() {
        binding.previewView.visibility = View.VISIBLE
        binding.scanOverlay.visibility = View.VISIBLE
        binding.scanFrame.visibility = View.VISIBLE
        binding.topBar.visibility = View.VISIBLE
        binding.bottomContainer.visibility = View.VISIBLE
        binding.resultLayout.visibility = View.GONE
        binding.permissionLayout.visibility = View.GONE
        binding.progressBar.visibility = View.GONE
        currentSavedReportId = null
        
        // Restart camera if needed
        if (cameraProvider != null) {
            bindCameraUseCases()
        }
    }
    
    private fun showResult(result: com.hospiscanner.model.ScanResult) {
        binding.resultLayout.visibility = View.VISIBLE
        binding.previewView.visibility = View.GONE
        binding.scanOverlay.visibility = View.GONE
        binding.scanFrame.visibility = View.GONE
        binding.topBar.visibility = View.GONE
        binding.bottomContainer.visibility = View.GONE
        
        if (result.isValidJson && result.medicalReport != null) {
            // Show success indicator
            binding.statusIndicator.setBackgroundColor(
                ContextCompat.getColor(this, R.color.success)
            )
            binding.statusIndicatorText.text = getString(R.string.valid_json)

            // Show medical report content
            binding.medicalReportContent.visibility = View.VISIBLE
            binding.errorCard.visibility = View.GONE
            binding.detailButton.visibility = View.VISIBLE

            displayMedicalReport(result.medicalReport)
            saveReportToHistory(result.medicalReport, result.formattedJson ?: result.rawData)
        } else {
            // Show error
            binding.statusIndicator.setBackgroundColor(
                ContextCompat.getColor(this, R.color.error)
            )
            binding.statusIndicatorText.text = getString(R.string.invalid_json_x)
            binding.medicalReportContent.visibility = View.GONE
            binding.errorCard.visibility = View.VISIBLE
            binding.detailButton.visibility = View.GONE
            binding.errorText.text = result.errorMessage ?: getString(R.string.invalid_json)
            binding.rawDataText.text = result.rawData
        }
    }

    private fun saveReportToHistory(report: com.hospiscanner.model.MedicalReport, rawData: String) {
        lifecycleScope.launch {
            val savedId = reportRepository.saveScannedReport(report, rawData)
            if (savedId > 0) {
                currentSavedReportId = savedId
                Toast.makeText(this@MainActivity, getString(R.string.report_saved), Toast.LENGTH_SHORT).show()
            }
        }
    }

    private fun displayMedicalReport(report: com.hospiscanner.model.MedicalReport) {
        // Hospital name in header
        binding.hospitalNameText.text = report.hospital ?: ""
        binding.hospitalNameText.visibility = if (report.hospital != null) View.VISIBLE else View.GONE

        // Patient Name
        if (report.patientName != null) {
            binding.patientNameText.text = report.patientName
            binding.patientNameLayout.visibility = View.VISIBLE
        } else {
            binding.patientNameLayout.visibility = View.GONE
        }

        // Last Name
        if (report.lastName != null) {
            binding.lastNameText.text = report.lastName
            binding.lastNameLayout.visibility = View.VISIBLE
        } else {
            binding.lastNameLayout.visibility = View.GONE
        }

        // DNI
        if (report.dni != null) {
            binding.dniText.text = report.dni
            binding.dniLayout.visibility = View.VISIBLE
        } else {
            binding.dniLayout.visibility = View.GONE
        }

        // Age
        if (report.age != null) {
            binding.ageText.text = report.age
            binding.ageLayout.visibility = View.VISIBLE
        } else {
            binding.ageLayout.visibility = View.GONE
        }

        // Gender
        if (report.gender != null) {
            binding.genderText.text = report.gender
            binding.genderLayout.visibility = View.VISIBLE
        } else {
            binding.genderLayout.visibility = View.GONE
        }

        // Blood Type
        if (report.bloodType != null) {
            binding.bloodTypeText.text = report.bloodType
            binding.bloodTypeLayout.visibility = View.VISIBLE
        } else {
            binding.bloodTypeLayout.visibility = View.GONE
        }

        // Room
        if (report.room != null) {
            binding.roomText.text = report.room
            binding.roomLayout.visibility = View.VISIBLE
        } else {
            binding.roomLayout.visibility = View.GONE
        }

        // Bed
        if (report.bed != null) {
            binding.bedText.text = report.bed
            binding.bedLayout.visibility = View.VISIBLE
        } else {
            binding.bedLayout.visibility = View.GONE
        }

        // Generated Date
        if (report.generatedDate != null) {
            binding.generatedDateText.text = report.generatedDate
            binding.generatedDateLayout.visibility = View.VISIBLE
        } else {
            binding.generatedDateLayout.visibility = View.GONE
        }

        // Generated By
        if (report.generatedBy != null) {
            binding.generatedByText.text = report.generatedBy
            binding.generatedByLayout.visibility = View.VISIBLE
        } else {
            binding.generatedByLayout.visibility = View.GONE
        }

        // Medical Information Card - show only if there's medical data
        val hasMedicalInfo = report.diagnosis != null || report.treatment != null ||
                            report.medications != null || report.allergies != null
        binding.medicalInfoCard.visibility = if (hasMedicalInfo) View.VISIBLE else View.GONE

        // Diagnosis
        if (report.diagnosis != null) {
            binding.diagnosisText.text = report.diagnosis
            binding.diagnosisLayout.visibility = View.VISIBLE
        } else {
            binding.diagnosisLayout.visibility = View.GONE
        }

        // Treatment
        if (report.treatment != null) {
            binding.treatmentText.text = report.treatment
            binding.treatmentLayout.visibility = View.VISIBLE
        } else {
            binding.treatmentLayout.visibility = View.GONE
        }

        // Medications
        if (report.medications != null) {
            binding.medicationsText.text = report.medications
            binding.medicationsLayout.visibility = View.VISIBLE
        } else {
            binding.medicationsLayout.visibility = View.GONE
        }

        // Allergies (highlighted)
        if (report.allergies != null) {
            binding.allergiesText.text = report.allergies
            binding.allergiesLayout.visibility = View.VISIBLE
        } else {
            binding.allergiesLayout.visibility = View.GONE
        }

        // Doctor & Date Card
        val hasDoctorOrDate = report.doctor != null || report.date != null
        binding.doctorDateCard.visibility = if (hasDoctorOrDate) View.VISIBLE else View.GONE

        // Doctor
        if (report.doctor != null) {
            binding.doctorText.text = report.doctor
            binding.doctorLayout.visibility = View.VISIBLE
        } else {
            binding.doctorLayout.visibility = View.GONE
        }

        // Date
        if (report.date != null) {
            binding.dateText.text = report.date
            binding.dateLayout.visibility = View.VISIBLE
        } else {
            binding.dateLayout.visibility = View.GONE
        }

        // Additional Notes
        if (report.additionalNotes != null) {
            binding.notesText.text = report.additionalNotes
            binding.notesCard.visibility = View.VISIBLE
        } else {
            binding.notesCard.visibility = View.GONE
        }

        // Other Fields
        if (report.otherFields.isNotEmpty()) {
            binding.otherFieldsContainer.removeAllViews()

            for ((key, value) in report.otherFields) {
                val fieldLayout = layoutInflater.inflate(
                    R.layout.item_field,
                    binding.otherFieldsContainer,
                    false
                )

                val fieldLabel = fieldLayout.findViewById<android.widget.TextView>(R.id.field_label)
                val fieldValue = fieldLayout.findViewById<android.widget.TextView>(R.id.field_value)

                fieldLabel.text = key
                fieldValue.text = value

                binding.otherFieldsContainer.addView(fieldLayout)
            }

            binding.otherFieldsCard.visibility = View.VISIBLE
        } else {
            binding.otherFieldsCard.visibility = View.GONE
        }
    }
    
    private fun showPermissionRequired() {
        binding.permissionLayout.visibility = View.VISIBLE
        binding.previewView.visibility = View.GONE
        binding.scanOverlay.visibility = View.GONE
        binding.scanFrame.visibility = View.GONE
        binding.topBar.visibility = View.GONE
        binding.bottomContainer.visibility = View.GONE
        binding.resultLayout.visibility = View.GONE
    }
    
    private fun showPermissionDenied() {
        Toast.makeText(
            this,
            getString(R.string.permission_denied),
            Toast.LENGTH_LONG
        ).show()
        showPermissionRequired()
    }
    
    private fun confirmCopyToClipboard() {
        val result = viewModel.scanResult.value ?: return
        if (result.isValidJson && result.medicalReport != null) {
            AlertDialog.Builder(this)
                .setTitle(getString(R.string.copy_sensitive_title))
                .setMessage(getString(R.string.copy_sensitive_message))
                .setPositiveButton(getString(R.string.copy_to_clipboard)) { _, _ ->
                    copyToClipboard(result)
                }
                .setNegativeButton(getString(R.string.cancel), null)
                .show()
        } else {
            copyToClipboard(result)
        }
    }

    private fun copyToClipboard(result: com.hospiscanner.model.ScanResult) {
        val textToCopy = if (result.isValidJson && result.medicalReport != null) {
            MedicalReportClipboardFormatter.format(result.medicalReport)
        } else {
            result.rawData
        }
        
        val clipboard = getSystemService(Context.CLIPBOARD_SERVICE) as ClipboardManager
        val clip = ClipData.newPlainText("Informe Médico", textToCopy)
        clipboard.setPrimaryClip(clip)
        
        Toast.makeText(this, getString(R.string.copied), Toast.LENGTH_SHORT).show()
    }
    
    private fun showPinDialog(result: com.hospiscanner.model.ScanResult) {
        AlertDialog.Builder(this)
            .setTitle(getString(R.string.pin_gate_title))
            .setMessage(getString(R.string.pin_gate_message))
            .setPositiveButton(getString(R.string.verify_identity)) { _, _ ->
                showPinEntryDialog(result)
            }
            .setNegativeButton(getString(R.string.cancel)) { _, _ ->
                viewModel.cancelPinVerification()
            }
            .setCancelable(false)
            .show()
    }

    private fun showPinEntryDialog(result: com.hospiscanner.model.ScanResult) {
        val dialogView = layoutInflater.inflate(R.layout.dialog_pin_verification, null)
        val pinInput = dialogView.findViewById<TextInputEditText>(R.id.pinInput)
        val errorText = dialogView.findViewById<android.widget.TextView>(R.id.errorText)
        
        val dialog = AlertDialog.Builder(this)
            .setView(dialogView)
            .setCancelable(false)
            .create()
        
        dialogView.findViewById<View>(R.id.confirmButton).setOnClickListener {
            val enteredPin = pinInput.text.toString()
            
            when {
                enteredPin.length != 6 -> {
                    errorText.text = getString(R.string.pin_error_length)
                    errorText.visibility = View.VISIBLE
                }
                result.accessPinHash == null -> {
                    errorText.text = getString(R.string.pin_error_no_dni)
                    errorText.visibility = View.VISIBLE
                }
                viewModel.verifyPin(enteredPin) -> {
                    dialog.dismiss()
                }
                else -> {
                    errorText.text = getString(R.string.pin_error_incorrect)
                    errorText.visibility = View.VISIBLE
                    pinInput.text?.clear()
                }
            }
        }
        
        dialogView.findViewById<View>(R.id.cancelButton).setOnClickListener {
            dialog.dismiss()
            viewModel.cancelPinVerification()
        }
        
        dialog.show()
    }
    
    override fun onDestroy() {
        super.onDestroy()
        cameraExecutor.shutdown()
        barcodeScanner.close()
    }
    
    companion object {
        private const val TAG = "MainActivity"
    }
}
