package com.hospiscanner.repository

import androidx.lifecycle.LiveData
import com.hospiscanner.data.database.MedicalReportDao
import com.hospiscanner.data.database.MedicalReportEntity
import com.hospiscanner.model.MedicalReport

class MedicalReportRepository(
    private val dao: MedicalReportDao
) {
    val reports: LiveData<List<MedicalReportEntity>> = dao.observeReports()

    fun searchReports(query: String): LiveData<List<MedicalReportEntity>> {
        return if (query.isBlank()) reports else dao.searchReports(query.trim())
    }

    fun observeReport(id: Long): LiveData<MedicalReportEntity?> = dao.observeReport(id)

    suspend fun saveScannedReport(report: MedicalReport, rawData: String): Long {
        val entity = MedicalReportEntity.fromReport(
            report = report,
            rawData = rawData,
            scannedAt = System.currentTimeMillis()
        )
        val duplicate = dao.findDuplicate(entity.dni, entity.reportDateKey)
        if (duplicate != null) return duplicate.id

        return dao.insert(entity)
    }
}
