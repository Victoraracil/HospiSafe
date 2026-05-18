package com.hospiscanner.view.formatter

import com.hospiscanner.model.MedicalReport

object MedicalReportClipboardFormatter {
    fun format(report: MedicalReport): String {
        val sb = StringBuilder()

        sb.appendLine("============================")
        sb.appendLine("       INFORME MEDICO")
        report.hospital?.let { sb.appendLine("   $it") }
        sb.appendLine("============================")
        sb.appendLine()

        sb.appendLine("INFORMACION DEL PACIENTE")
        sb.appendLine("----------------------------")
        report.patientName?.let { sb.appendLine("Nombre: $it") }
        report.lastName?.let { sb.appendLine("Apellidos: $it") }
        report.dni?.let { sb.appendLine("DNI: $it") }
        report.age?.let { sb.appendLine("Edad: $it") }
        report.gender?.let { sb.appendLine("Genero: $it") }
        report.bloodType?.let { sb.appendLine("Tipo de Sangre: $it") }
        report.room?.let { sb.appendLine("Habitacion: $it") }
        report.bed?.let { sb.appendLine("Cama: $it") }
        report.generatedDate?.let { sb.appendLine("Fecha de Generacion: $it") }
        report.generatedBy?.let { sb.appendLine("Generado Por: $it") }
        sb.appendLine()

        if (report.diagnosis != null || report.treatment != null ||
            report.medications != null || report.allergies != null
        ) {
            sb.appendLine("INFORMACION MEDICA")
            sb.appendLine("----------------------------")
            report.diagnosis?.let { sb.appendLine("Diagnostico: $it") }
            report.treatment?.let { sb.appendLine("Tratamiento: $it") }
            report.medications?.let { sb.appendLine("Medicamentos: $it") }
            report.allergies?.let { sb.appendLine("ALERGIAS: $it") }
            sb.appendLine()
        }

        if (report.doctor != null || report.date != null) {
            sb.appendLine("INFORMACION ADICIONAL")
            sb.appendLine("----------------------------")
            report.doctor?.let { sb.appendLine("Medico: $it") }
            report.date?.let { sb.appendLine("Fecha: $it") }
            sb.appendLine()
        }

        report.additionalNotes?.let {
            sb.appendLine("NOTAS")
            sb.appendLine("----------------------------")
            sb.appendLine(it)
            sb.appendLine()
        }

        if (report.otherFields.isNotEmpty()) {
            sb.appendLine("OTROS DATOS")
            sb.appendLine("----------------------------")
            for ((key, value) in report.otherFields) {
                sb.appendLine("$key: $value")
            }
        }

        return sb.toString()
    }
}
