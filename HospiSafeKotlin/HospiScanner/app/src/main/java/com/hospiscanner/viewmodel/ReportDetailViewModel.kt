package com.hospiscanner.viewmodel

import androidx.lifecycle.LiveData
import androidx.lifecycle.ViewModel
import com.hospiscanner.data.database.MedicalReportEntity
import com.hospiscanner.repository.MedicalReportRepository

class ReportDetailViewModel(
    repository: MedicalReportRepository,
    reportId: Long
) : ViewModel() {
    val report: LiveData<MedicalReportEntity?> = repository.observeReport(reportId)
}
