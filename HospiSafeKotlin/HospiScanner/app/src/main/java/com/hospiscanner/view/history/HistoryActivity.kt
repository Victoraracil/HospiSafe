package com.hospiscanner.view.history

import android.content.Intent
import android.os.Bundle
import android.view.View
import androidx.activity.viewModels
import androidx.appcompat.app.AppCompatActivity
import androidx.core.widget.doAfterTextChanged
import androidx.recyclerview.widget.LinearLayoutManager
import com.hospiscanner.data.database.AppDatabase
import com.hospiscanner.databinding.ActivityHistoryBinding
import com.hospiscanner.repository.MedicalReportRepository
import com.hospiscanner.view.detail.ReportDetailActivity
import com.hospiscanner.viewmodel.HistoryViewModel
import com.hospiscanner.viewmodel.factory.HistoryViewModelFactory

class HistoryActivity : AppCompatActivity() {
    private lateinit var binding: ActivityHistoryBinding

    private val viewModel: HistoryViewModel by viewModels {
        val dao = AppDatabase.getInstance(applicationContext).medicalReportDao()
        HistoryViewModelFactory(MedicalReportRepository(dao))
    }

    private val adapter = ReportHistoryAdapter { report ->
        startActivity(
            Intent(this, ReportDetailActivity::class.java)
                .putExtra(ReportDetailActivity.EXTRA_REPORT_ID, report.id)
        )
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        window.setFlags(
            android.view.WindowManager.LayoutParams.FLAG_SECURE,
            android.view.WindowManager.LayoutParams.FLAG_SECURE
        )
        binding = ActivityHistoryBinding.inflate(layoutInflater)
        setContentView(binding.root)

        setupToolbar()
        setupList()
        setupSearch()
        observeReports()
    }

    private fun setupToolbar() {
        binding.backButton.setOnClickListener { finish() }
    }

    private fun setupList() {
        binding.historyRecyclerView.layoutManager = LinearLayoutManager(this)
        binding.historyRecyclerView.adapter = adapter
        binding.historyRecyclerView.itemAnimator?.changeDuration = 180
    }

    private fun setupSearch() {
        binding.searchInput.doAfterTextChanged {
            viewModel.updateSearch(it?.toString().orEmpty())
        }
    }

    private fun observeReports() {
        viewModel.reports.observe(this) { reports ->
            adapter.submitList(reports)
            val isEmpty = reports.isEmpty()
            binding.emptyState.visibility = if (isEmpty) View.VISIBLE else View.GONE
            binding.historyRecyclerView.visibility = if (isEmpty) View.GONE else View.VISIBLE
        }
    }
}
