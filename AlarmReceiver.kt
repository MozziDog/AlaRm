package com.example.alarm405

import android.app.NotificationChannel
import android.app.NotificationManager
import android.app.PendingIntent
import android.content.BroadcastReceiver
import android.content.Context
import android.content.Intent
import android.media.MediaPlayer
import android.media.RingtoneManager
import android.widget.Toast
import androidx.core.app.NotificationCompat
import androidx.core.app.NotificationManagerCompat


class AlarmReceiver: BroadcastReceiver() {

    companion object {
        const val NOTIFICATION_ID = 100
        const val NOTIFICATION_CHANNEL_ID = "1000"
    }

    override fun onReceive(context: Context, intent: Intent) {
        createNotificationChannel(context)
        notifyNotification(context)
        Toast.makeText(context, "good morning", Toast.LENGTH_SHORT).show()
        val action = intent.action
        if(action.equals("android.intent.action.BOOT_COMPLETED")){
            val i = Intent(context, MainActivity::class.java)
            i.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK)
            context.startActivity(i)
        }
        val alarmUri = RingtoneManager.getDefaultUri(RingtoneManager.TYPE_NOTIFICATION)
        if (alarmUri == null) {
            val mp = MediaPlayer.create(context, R.raw.wake_up_song)
            mp.start()
        }
    }

    private fun createNotificationChannel(context: Context) {

        val notificationChannel = NotificationChannel(
            NOTIFICATION_CHANNEL_ID,
            "기상 알람",
            NotificationManager.IMPORTANCE_HIGH
        )

        NotificationManagerCompat.from(context).createNotificationChannel(notificationChannel)
    }

    private fun notifyNotification(context: Context) {
        val tapResultIntent = Intent(context, MainActivity::class.java).apply {
            flags = Intent.FLAG_ACTIVITY_CLEAR_TASK
        }
        val pendingIntent: PendingIntent = PendingIntent.getActivity(
            context,
            0,
            tapResultIntent,
            PendingIntent.FLAG_UPDATE_CURRENT
        )
        with(NotificationManagerCompat.from(context)) {
            val build = NotificationCompat.Builder(context, NOTIFICATION_CHANNEL_ID)
                .setContentTitle("알람")
                .setContentText("일어날 시간입니다.")
                .setAutoCancel(true)
                .setContentIntent(pendingIntent)
                .setSmallIcon(R.drawable.ic_launcher_foreground)
                .setPriority(NotificationCompat.PRIORITY_HIGH)

            notify(NOTIFICATION_ID, build.build())
        }
        val intent1 = Intent(context, MainActivity::class.java)
        intent1.putExtra("change", true)
    }
}