package com.hospiscanner.data.database

import androidx.room.Entity
import androidx.room.Index
import androidx.room.PrimaryKey
import com.google.gson.Gson
import com.google.gson.reflect.TypeToken
import com.hospiscanner.model.MedicalReport

@Entity(
    tableName = "medical_reports",
    indices = [
        Index(value = ["dni", "reportDateKey"], unique = true),
        Index(value = ["scannedAt"])
    ]
)
data class MedicalReportEntity(
    @PrimaryKey(autoGenerate = true)
    val id: Long = 0,
    val patientName: String?,
    val lastName: String?,
    val dni: String,
    val age: String?,
    val gender: String?,
    val bloodType: String?,
    val diagnosis: String?,
    val treatment: String?,
    val medications: String?,
    val allergies: String?,
    val doctor: String?,
    val date: String?,
    val hospital: String?,
    val room: String?,
    val bed: String?,
    val additionalNotes: String?,
    val generatedDate: String?,
    val generatedBy: String?,
    val otherFieldsJson: String,
    val rawData: String,
    val reportDateKey: String,
    val analysisType: String,
    val status: String,
    val scannedAt: Long
) {
    fun toMedicalReport(): MedicalReport {
        val mapType = object : TypeToken<Map<String, String>>() {}.type
        val otherFields = runCatching {
            Gson().fromJson<Map<String, String>>(otherFieldsJson, mapType)
        }.getOrDefault(emptyMap())

        return MedicalReport(
            patientName = patientName,
            lastName = lastName,
            dni = dni.takeIf { it.isNotBlank() },
            age = age,
            gender = gender,
            bloodType = bloodType,
            diagnosis = diagnosis,
            treatment = treatment,
            medications = medications,
            allergies = allergies,
            doctor = doctor,
            date = date,
            hospital = hospital,
            room = room,
            bed = bed,
            additionalNotes = additionalNotes,
            generatedDate = generatedDate,
            generatedBy = generatedBy,
            otherFields = otherFields
        )
    }

    companion object {
        fun fromReport(report: MedicalReport, rawData: String, scannedAt: Long): MedicalReportEntity {
            val dateKey = report.date ?: report.generatedDate ?: "sin-fecha"
            val dniKey = report.dni?.trim().orEmpty()
            val analysisType = report.otherFields["tipo_analisis"]
                ?: report.otherFields["analysis_type"]
                ?: report.otherFields["tipoAnalisis"]
                ?: report.diagnosis?.lineSequence()?.firstOrNull()?.take(42)
                ?: "Informe medico"

            return MedicalReportEntity(
                patientName = report.patientName,
                lastName = report.lastName,
                dni = dniKey,
                age = report.age,
                gender = report.gender,
                bloodType = report.bloodType,
                diagnosis = report.diagnosis,
                treatment = report.treatment,
                medications = report.medications,
                allergies = report.allergies,
                doctor = report.doctor,
                date = report.date,
                hospital = report.hospital,
                room = report.room,
                bed = report.bed,
                additionalNotes = report.additionalNotes,
                generatedDate = report.generatedDate,
                generatedBy = report.generatedBy,
                otherFieldsJson = Gson().toJson(report.otherFields),
                rawData = rawData,
                reportDateKey = dateKey,
                analysisType = analysisType,
                status = if (report.allergies.isNullOrBlank()) "Validado" else "Alergias",
                scannedAt = scannedAt
            )
        }
    }
}
