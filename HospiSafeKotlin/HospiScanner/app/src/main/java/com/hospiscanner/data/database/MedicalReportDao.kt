package com.hospiscanner.data.database

import androidx.lifecycle.LiveData
import androidx.room.Dao
import androidx.room.Insert
import androidx.room.OnConflictStrategy
import androidx.room.Query

@Dao
interface MedicalReportDao {
    @Query("SELECT * FROM medical_reports ORDER BY scannedAt DESC")
    fun observeReports(): LiveData<List<MedicalReportEntity>>

    @Query(
        """
        SELECT * FROM medical_reports
        WHERE patientName LIKE '%' || :query || '%'
            OR lastName LIKE '%' || :query || '%'
            OR dni LIKE '%' || :query || '%'
            OR analysisType LIKE '%' || :query || '%'
            OR diagnosis LIKE '%' || :query || '%'
        ORDER BY scannedAt DESC
        """
    )
    fun searchReports(query: String): LiveData<List<MedicalReportEntity>>

    @Query("SELECT * FROM medical_reports WHERE id = :id LIMIT 1")
    fun observeReport(id: Long): LiveData<MedicalReportEntity?>

    @Query("SELECT * FROM medical_reports WHERE dni = :dni AND reportDateKey = :dateKey LIMIT 1")
    suspend fun findDuplicate(dni: String, dateKey: String): MedicalReportEntity?

    @Insert(onConflict = OnConflictStrategy.IGNORE)
    suspend fun insert(report: MedicalReportEntity): Long
}
