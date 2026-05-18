package com.hospiscanner.view.scanner

import android.util.Log
import androidx.camera.core.ExperimentalGetImage
import androidx.camera.core.ImageAnalysis
import androidx.camera.core.ImageProxy
import com.google.mlkit.vision.barcode.BarcodeScanner
import com.google.mlkit.vision.barcode.common.Barcode
import com.google.mlkit.vision.common.InputImage

class QrCodeAnalyzer(
    private val barcodeScanner: BarcodeScanner,
    private val isScannerActive: () -> Boolean,
    private val onQrDetected: (String) -> Unit
) : ImageAnalysis.Analyzer {

    @ExperimentalGetImage
    override fun analyze(imageProxy: ImageProxy) {
        val mediaImage = imageProxy.image
        if (mediaImage == null || !isScannerActive()) {
            imageProxy.close()
            return
        }

        val image = InputImage.fromMediaImage(
            mediaImage,
            imageProxy.imageInfo.rotationDegrees
        )

        barcodeScanner.process(image)
            .addOnSuccessListener { barcodes ->
                barcodes.firstNotNullOfOrNull { barcode ->
                    barcode.rawValue?.takeIf {
                        barcode.valueType == Barcode.TYPE_TEXT ||
                            barcode.valueType == Barcode.TYPE_URL ||
                            barcode.valueType == Barcode.TYPE_UNKNOWN
                    }
                }?.let(onQrDetected)
            }
            .addOnFailureListener { error ->
                Log.e(TAG, "Barcode scanning failed", error)
            }
            .addOnCompleteListener {
                imageProxy.close()
            }
    }

    private companion object {
        const val TAG = "QrCodeAnalyzer"
    }
}
