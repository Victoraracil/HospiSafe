package com.hospiscanner.view.history

import android.view.LayoutInflater
import android.view.ViewGroup
import androidx.recyclerview.widget.DiffUtil
import androidx.recyclerview.widget.ListAdapter
import androidx.recyclerview.widget.RecyclerView
import com.hospiscanner.R
import com.hospiscanner.data.database.MedicalReportEntity
import com.hospiscanner.databinding.ItemReportHistoryBinding
import java.text.SimpleDateFormat
import java.util.Date
import java.util.Locale

class ReportHistoryAdapter(
    private val onReportClick: (MedicalReportEntity) -> Unit
) : ListAdapter<MedicalReportEntity, ReportHistoryAdapter.ReportViewHolder>(DiffCallback) {

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ReportViewHolder {
        val binding = ItemReportHistoryBinding.inflate(
            LayoutInflater.from(parent.context),
            parent,
            false
        )
        return ReportViewHolder(binding, onReportClick)
    }

    override fun onBindViewHolder(holder: ReportViewHolder, position: Int) {
        holder.bind(getItem(position))
    }

    class ReportViewHolder(
        private val binding: ItemReportHistoryBinding,
        private val onReportClick: (MedicalReportEntity) -> Unit
    ) : RecyclerView.ViewHolder(binding.root) {

        fun bind(report: MedicalReportEntity) {
            val context = binding.root.context
            val fullName = listOfNotNull(report.patientName, report.lastName)
                .joinToString(" ")
                .ifBlank { context.getString(R.string.patient_without_name) }

            binding.patientNameText.text = fullName
            binding.analysisTypeText.text = report.analysisType
            binding.reportDateText.text = report.date ?: report.generatedDate ?: context.getString(R.string.date_not_available)
            binding.scannedAtText.text = context.getString(
                R.string.scanned_at_format,
                scanDateFormatter.format(Date(report.scannedAt))
            )
            binding.statusChip.text = report.status
            binding.statusChip.setChipBackgroundColorResource(
                if (report.status == "Alergias") R.color.allergy_container else R.color.success_container
            )
            binding.statusChip.setTextColor(
                context.getColor(if (report.status == "Alergias") R.color.allergy else R.color.success_dark)
            )
            binding.root.setOnClickListener { onReportClick(report) }
        }

        companion object {
            private val scanDateFormatter = SimpleDateFormat("dd/MM/yyyy HH:mm", Locale.getDefault())
        }
    }

    private object DiffCallback : DiffUtil.ItemCallback<MedicalReportEntity>() {
        override fun areItemsTheSame(oldItem: MedicalReportEntity, newItem: MedicalReportEntity): Boolean {
            return oldItem.id == newItem.id
        }

        override fun areContentsTheSame(oldItem: MedicalReportEntity, newItem: MedicalReportEntity): Boolean {
            return oldItem == newItem
        }
    }
}
