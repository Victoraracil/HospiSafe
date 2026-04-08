package com.hospiscanner.model

/**
 * Data class representing a medical report parsed from QR code
 */
data class MedicalReport(
    val patientName: String?,
    val lastName: String?,
    val dni: String?,
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
    val otherFields: Map<String, String> = emptyMap()
) {
    companion object {
        /**
         * Parse medical report from JSON map
         */
        fun fromJsonMap(jsonMap: Map<String, Any>): MedicalReport {
            // Helper function to safely get string value
            fun getValue(vararg keys: String): String? {
                for (key in keys) {
                    val value = jsonMap[key]
                    if (value != null) {
                        return value.toString()
                    }
                }
                return null
            }

            // Extract known fields
            val fullName = getValue("nombre", "name", "patient_name", "patientName", "Nombre", "Name")
            val lastNameField = getValue("apellidos", "apellido", "lastname", "last_name", "surname", "Apellidos", "Apellido")

            // Extract first name and last name
            var patientName: String? = null
            var lastName: String? = null

            if (lastNameField != null) {
                // If there's a separate lastName field, use it
                lastName = lastNameField
                patientName = fullName
            } else if (fullName != null) {
                // Try to split the full name into first name and last name
                val nameParts = fullName.trim().split("\\s+".toRegex())
                if (nameParts.size > 1) {
                    patientName = nameParts[0]
                    lastName = nameParts.drop(1).joinToString(" ")
                } else {
                    patientName = fullName
                }
            }

            val dni = getValue("DNI", "dni", "Dni", "document_id", "documentId", "cedula", "ci")
            val age = getValue("edad", "age", "Edad", "Age")
            val gender = getValue("sexo", "gender", "genero", "Sexo", "Gender", "Genero")
            val bloodType = getValue("tipo_sangre", "blood_type", "bloodType", "tipoSangre", "grupo_sanguineo")
            val diagnosis = getValue("diagnostico", "diagnosis", "Diagnostico", "Diagnosis")
            val treatment = getValue("tratamiento", "treatment", "Tratamiento", "Treatment")
            val medications = getValue("medicamentos", "medications", "medicinas", "Medicamentos", "Medications")
            val allergies = getValue("alergias", "allergies", "Alergias", "Allergies")
            val doctor = getValue("doctor", "medico", "Doctor", "Medico", "physician")
            val date = getValue("fecha", "date", "Fecha", "Date", "fecha_ingreso", "admission_date")
            val hospital = getValue("hospital", "Hospital", "centro_medico", "medical_center")
            val room = getValue("habitacion", "room", "Habitacion", "Room", "sala")
            val bed = getValue("cama", "bed", "Cama", "Bed")
            val additionalNotes = getValue("notas", "notes", "observaciones", "observations", "comentarios", "comments")
            val generatedDate = getValue("fecha_generacion", "generated_date", "generatedDate", "fecha_creacion", "creation_date")
            val generatedBy = getValue("generado_por", "generated_by", "generatedBy", "creado_por", "created_by")

            // Collect other fields that weren't mapped
            val knownKeys = setOf(
                "nombre", "name", "patient_name", "patientName", "Nombre", "Name",
                "apellidos", "apellido", "lastname", "last_name", "surname", "Apellidos", "Apellido",
                "DNI", "dni", "Dni", "document_id", "documentId", "cedula", "ci",
                "edad", "age", "Edad", "Age",
                "sexo", "gender", "genero", "Sexo", "Gender", "Genero",
                "tipo_sangre", "blood_type", "bloodType", "tipoSangre", "grupo_sanguineo",
                "diagnostico", "diagnosis", "Diagnostico", "Diagnosis",
                "tratamiento", "treatment", "Tratamiento", "Treatment",
                "medicamentos", "medications", "medicinas", "Medicamentos", "Medications",
                "alergias", "allergies", "Alergias", "Allergies",
                "doctor", "medico", "Doctor", "Medico", "physician",
                "fecha", "date", "Fecha", "Date", "fecha_ingreso", "admission_date",
                "hospital", "Hospital", "centro_medico", "medical_center",
                "habitacion", "room", "Habitacion", "Room", "sala",
                "cama", "bed", "Cama", "Bed",
                "notas", "notes", "observaciones", "observations", "comentarios", "comments",
                "fecha_generacion", "generated_date", "generatedDate", "fecha_creacion", "creation_date",
                "generado_por", "generated_by", "generatedBy", "creado_por", "created_by"
            )

            val otherFields = jsonMap
                .filterKeys { it !in knownKeys }
                .mapValues { it.value.toString() }

            return MedicalReport(
                patientName = patientName,
                lastName = lastName,
                dni = dni,
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
    }
}

