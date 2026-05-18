package com.hospiscanner.viewmodel.factory

import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import com.hospiscanner.repository.MedicalReportRepository
import com.hospiscanner.viewmodel.ReportDetailViewModel

class ReportDetailViewModelFactory(
    private val repository: MedicalReportRepository,
    private val reportId: Long
) : ViewModelProvider.Factory {
    override fun <T : ViewModel> create(modelClass: Class<T>): T {
        if (modelClass.isAssignableFrom(ReportDetailViewModel::class.java)) {
            @Suppress("UNCHECKED_CAST")
            return ReportDetailViewModel(repository, reportId) as T
        }
        throw IllegalArgumentException("Unknown ViewModel class: ${modelClass.name}")
    }
}
