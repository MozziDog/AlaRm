package com.example.testapplication;

import android.app.Notification;
import android.app.NotificationChannel;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.os.Build;
import android.util.Log;
import android.widget.Toast;

import androidx.core.app.NotificationCompat;
import androidx.core.app.NotificationManagerCompat;

public class MyReceiver extends BroadcastReceiver {

        final int NOTIFICATION_ID = 100;
        final String NOTIFICATION_CHANNEL_ID = "1000";
        final String RECEIVER = "MyReceiver";

    @Override public void onReceive(Context context, Intent intent) {
        Log.d(RECEIVER, "수신 받음!!!!!!!!!!!!!!!");

        /*
        val intent2 = Intent(context, MainActivity::class.java)
        intent2.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_SINGLE_TOP or Intent.FLAG_ACTIVITY_CLEAR_TOP)
        if(Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            context.startForegroundService(intent2)
        } else {
            startActivity(context, intent2, null)
        }
        */

        createNotificationChannel(context);
        notifyNotification(context);
        Toast.makeText(context, "good morning", Toast.LENGTH_SHORT).show();

        // 디버그 용이성을 위해 진동은 잠시 배제
        // val vibrator = context.getSystemService(Context.VIBRATOR_SERVICE) as Vibrator
        // val effect = VibrationEffect.createOneShot(150, VibrationEffect.DEFAULT_AMPLITUDE)
        // vibrator.vibrate(effect)
    }

    private void createNotificationChannel(Context context) {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) { // 호환성 고려
            NotificationChannel notificationChannel = new NotificationChannel(
                    NOTIFICATION_CHANNEL_ID,
                    "기상 알람",
                    NotificationManager.IMPORTANCE_HIGH
            );

            NotificationManagerCompat.from(context).createNotificationChannel(notificationChannel);
        }
        else
        {
            Log.e(RECEIVER, "빌드 버전 너무 낮음");
        }
    }

    //@SuppressLint("SuspiciousIndentation")
    private void notifyNotification(Context context) {
        Intent tapResultIntent = new Intent(context, com.example.testapplication.MainActivity.class);
        tapResultIntent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_CLEAR_TASK);

        PendingIntent pendingIntent = PendingIntent.getActivity(
                context,
                0,
                tapResultIntent,
                PendingIntent.FLAG_UPDATE_CURRENT | PendingIntent.FLAG_MUTABLE
        );
        // 여기서부터 추가한 내용
        Intent fullscreenIntent = new Intent(context, com.example.testapplication.MainActivity.class);
        fullscreenIntent.setAction("fullscreen_activity");
        fullscreenIntent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_CLEAR_TASK);
        PendingIntent fullscreenPendingIntent = PendingIntent.getActivity(context, 0, fullscreenIntent,
                PendingIntent.FLAG_UPDATE_CURRENT | PendingIntent.FLAG_IMMUTABLE);
        // 여기까지
        NotificationCompat.Builder build = new NotificationCompat.Builder(context, NOTIFICATION_CHANNEL_ID)
                .setContentTitle("알람")
                .setContentText("일어날 시간입니다.")
                .setSmallIcon(R.drawable.ic_launcher_background)
                .setAutoCancel(true)
                .setContentIntent(pendingIntent)        // 이 부분이 fullscreenPendingIntent가 아니여도 되는가?
                .setCategory(NotificationCompat.CATEGORY_ALARM)
                .setVisibility(NotificationCompat.VISIBILITY_PUBLIC)    // 추가
                .setPriority(NotificationCompat.PRIORITY_MAX)          // High에서 MAX로 변경
                .setFullScreenIntent(fullscreenPendingIntent, true);
        NotificationManagerCompat.from(context).notify(NOTIFICATION_ID, build.build());
    }
}