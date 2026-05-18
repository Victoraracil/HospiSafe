package com.hospiscanner.viewmodel

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.switchMap
import com.hospiscanner.data.database.MedicalReportEntity
import com.hospiscanner.repository.MedicalReportRepository

class HistoryViewModel(
    private val repository: MedicalReportRepository
) : ViewModel() {
    private val query = MutableLiveData("")

    val reports: LiveData<List<MedicalReportEntity>> = query.switchMap { value ->
        repository.searchReports(value)
    }

    fun updateSearch(text: String) {
        query.value = text
    }
}
