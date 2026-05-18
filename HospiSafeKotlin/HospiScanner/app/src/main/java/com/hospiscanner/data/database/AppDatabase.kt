package com.hospiscanner.data.database

import android.content.Context
import androidx.security.crypto.EncryptedSharedPreferences
import androidx.security.crypto.MasterKey
import androidx.room.Database
import androidx.room.Room
import androidx.room.RoomDatabase
import net.sqlcipher.database.SQLiteDatabase
import net.sqlcipher.database.SupportFactory
import java.security.SecureRandom
import android.util.Base64

@Database(
    entities = [MedicalReportEntity::class],
    version = 1,
    exportSchema = true
)
abstract class AppDatabase : RoomDatabase() {
    abstract fun medicalReportDao(): MedicalReportDao

    companion object {
        @Volatile
        private var INSTANCE: AppDatabase? = null

        fun getInstance(context: Context): AppDatabase {
            return INSTANCE ?: synchronized(this) {
                val passphrase = getOrCreatePassphrase(context.applicationContext)
                val factory = SupportFactory(SQLiteDatabase.getBytes(passphrase.toCharArray()))
                INSTANCE ?: Room.databaseBuilder(
                    context.applicationContext,
                    AppDatabase::class.java,
                    "hospiscanner_secure.db"
                )
                    .openHelperFactory(factory)
                    .build()
                    .also { INSTANCE = it }
            }
        }

        private fun getOrCreatePassphrase(context: Context): String {
            val masterKey = MasterKey.Builder(context)
                .setKeyScheme(MasterKey.KeyScheme.AES256_GCM)
                .build()
            val prefs = EncryptedSharedPreferences.create(
                context,
                "secure_database_key",
                masterKey,
                EncryptedSharedPreferences.PrefKeyEncryptionScheme.AES256_SIV,
                EncryptedSharedPreferences.PrefValueEncryptionScheme.AES256_GCM
            )

            prefs.getString(KEY_DATABASE_PASSPHRASE, null)?.let { return it }

            val bytes = ByteArray(32)
            SecureRandom().nextBytes(bytes)
            val passphrase = Base64.encodeToString(bytes, Base64.NO_WRAP)
            prefs.edit().putString(KEY_DATABASE_PASSPHRASE, passphrase).apply()
            return passphrase
        }

        private const val KEY_DATABASE_PASSPHRASE = "database_passphrase"
    }
}
