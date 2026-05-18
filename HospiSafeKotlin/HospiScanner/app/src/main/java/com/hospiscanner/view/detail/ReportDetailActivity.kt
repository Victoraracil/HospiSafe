package com.hospiscanner.view.detail

import android.os.Bundle
import android.view.View
import androidx.activity.viewModels
import androidx.appcompat.app.AppCompatActivity
import com.hospiscanner.R
import com.hospiscanner.data.database.AppDatabase
import com.hospiscanner.data.database.MedicalReportEntity
import com.hospiscanner.databinding.ActivityReportDetailBinding
import com.hospiscanner.databinding.ItemDetailFieldBinding
import com.hospiscanner.repository.MedicalReportRepository
import com.hospiscanner.viewmodel.ReportDetailViewModel
import com.hospiscanner.viewmodel.factory.ReportDetailViewModelFactory
import java.text.SimpleDateFormat
import java.util.Date
import java.util.Locale

class ReportDetailActivity : AppCompatActivity() {
    private lateinit var binding: ActivityReportDetailBinding

    private val reportId: Long by lazy { intent.getLongExtra(EXTRA_REPORT_ID, -1L) }

    private val viewModel: ReportDetailViewModel by viewModels {
        val dao = AppDatabase.getInstance(applicationContext).medicalReportDao()
        ReportDetailViewModelFactory(MedicalReportRepository(dao), reportId)
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        window.setFlags(
            android.view.WindowManager.LayoutParams.FLAG_SECURE,
            android.view.WindowManager.LayoutParams.FLAG_SECURE
        )
        binding = ActivityReportDetailBinding.inflate(layoutInflater)
        setContentView(binding.root)

        binding.backButton.setOnClickListener { finish() }
        observeReport()
    }

    private fun observeReport() {
        viewModel.report.observe(this) { report ->
            if (report == null) {
                binding.notFoundState.visibility = View.VISIBLE
                binding.detailContent.visibility = View.GONE
            } else {
                binding.notFoundState.visibility = View.GONE
                binding.detailContent.visibility = View.VISIBLE
                bindReport(report)
            }
        }
    }

    private fun bindReport(report: MedicalReportEntity) {
        val fullName = listOfNotNull(report.patientName, report.lastName)
            .joinToString(" ")
            .ifBlank { getString(R.string.patient_without_name) }

        binding.patientHeroName.text = fullName
        binding.patientHeroMeta.text = getString(
            R.string.report_meta_format,
            report.dni.ifBlank { getString(R.string.dni_not_available) },
            report.analysisType
        )
        binding.statusChip.text = report.status
        binding.scanDateText.text = getString(
            R.string.scanned_at_format,
            scanDateFormatter.format(Date(report.scannedAt))
        )

        binding.patientFields.removeAllViews()
        addField(binding.patientFields, getString(R.string.dni), report.dni.ifBlank { null })
        addField(binding.patientFields, getString(R.string.age), report.age)
        addField(binding.patientFields, getString(R.string.gender), report.gender)
        addField(binding.patientFields, getString(R.string.blood_type), report.bloodType)
        addField(binding.patientFields, getString(R.string.room), report.room)
        addField(binding.patientFields, getString(R.string.bed), report.bed)

        bindSection(binding.diagnosisGroup, binding.diagnosisText, report.diagnosis)
        bindSection(binding.treatmentGroup, binding.treatmentText, report.treatment)
        bindSection(binding.medicationsGroup, binding.medicationsText, report.medications)
        bindSection(binding.allergiesGroup, binding.allergiesText, report.allergies)

        binding.clinicalCard.visibility = if (
            report.diagnosis.isNullOrBlank() &&
            report.treatment.isNullOrBlank() &&
            report.medications.isNullOrBlank() &&
            report.allergies.isNullOrBlank()
        ) View.GONE else View.VISIBLE

        binding.doctorFields.removeAllViews()
        addField(binding.doctorFields, getString(R.string.doctor), report.doctor)
        addField(binding.doctorFields, getString(R.string.date), report.date)
        addField(binding.doctorFields, getString(R.string.generated_date), report.generatedDate)
        addField(binding.doctorFields, getString(R.string.generated_by), report.generatedBy)
        binding.doctorCard.visibility = if (binding.doctorFields.childCount == 0) View.GONE else View.VISIBLE

        bindSection(binding.notesCard, binding.notesText, report.additionalNotes)
    }

    private fun addField(parent: android.view.ViewGroup, label: String, value: String?) {
        if (value.isNullOrBlank()) return
        val fieldBinding = ItemDetailFieldBinding.inflate(layoutInflater, parent, false)
        fieldBinding.fieldLabel.text = label
        fieldBinding.fieldValue.text = value
        parent.addView(fieldBinding.root)
    }

    private fun bindSection(group: View, textView: android.widget.TextView, value: String?) {
        val visible = !value.isNullOrBlank()
        group.visibility = if (visible) View.VISIBLE else View.GONE
        textView.text = value.orEmpty()
    }

    companion object {
        const val EXTRA_REPORT_ID = "extra_report_id"
        private val scanDateFormatter = SimpleDateFormat("dd/MM/yyyy HH:mm", Locale.getDefault())
    }
}
