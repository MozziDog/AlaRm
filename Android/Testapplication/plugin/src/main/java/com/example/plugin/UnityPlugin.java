package com.example.plugin;

import android.app.Activity;
import android.app.AlarmManager;
import android.app.PendingIntent;
import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.content.pm.ResolveInfo;
import android.os.Build;
import android.widget.Toast;

import androidx.annotation.RequiresApi;

import com.unity3d.player.UnityPlayer;
import java.util.Calendar;
import java.util.List;

public class UnityPlugin {
    private static UnityPlugin _instance;

    private static Activity _context;

    public static UnityPlugin instance() {
        if (_instance == null) {
            _instance = new UnityPlugin();
            _context = UnityPlayer.currentActivity;
        }
        return _instance;
    }

    public String getPackageName() {
        return _context.getPackageName();
    }

    public void showToast(final String text) {
        _context.runOnUiThread(new Runnable() {
            public void run() {
                Toast.makeText((Context)UnityPlugin._context, text, Toast.LENGTH_LONG).show();
            }
        });
    }

    @RequiresApi(api = Build.VERSION_CODES.S)
    private void setAlarm(int id, int hour, int minute) {
        Calendar calendar = Calendar.getInstance();
        calendar.set(Calendar.HOUR_OF_DAY, hour);
        calendar.set(Calendar.MINUTE, minute);
        calendar.set(Calendar.SECOND, 0);
        if (calendar.before(Calendar.getInstance()))
            calendar.add(5, 1);
        Intent intent = new Intent("com.example.testapplication.ALARM_START");

        // 암시적 intent는 백그라운드에서 실행 불가능 -> 명시적 intent로 변환
        // com.example.testapplication.ALARM_START를 수신하는 receiver가 달린 패키지를 모두 찾아서 intent에 이를 명시
        PackageManager packageManager = _context.getPackageManager();
        List<ResolveInfo> infos = packageManager.queryBroadcastReceivers(intent, 0);
        for (ResolveInfo info : infos) {
            ComponentName cn = new ComponentName(info.activityInfo.packageName,
                    info.activityInfo.name);
            intent.setComponent(cn);
        }
        PendingIntent pendingIntent = PendingIntent.getBroadcast((Context) _context, id, intent, PendingIntent.FLAG_UPDATE_CURRENT | PendingIntent.FLAG_MUTABLE);
        AlarmManager.AlarmClockInfo alarmClockInfo = new AlarmManager.AlarmClockInfo(calendar.getTimeInMillis(), pendingIntent);
        AlarmManager alarmManager = (AlarmManager) _context.getSystemService(Context.ALARM_SERVICE);
        alarmManager.setAlarmClock(alarmClockInfo, pendingIntent);
    }

    @RequiresApi(api = Build.VERSION_CODES.S)
    private void cancelAlarm(int id) {
        PendingIntent pendingIntent = PendingIntent.getBroadcast((Context)_context, id, new Intent("com.example.testapplication.ALARM_START"), PendingIntent.FLAG_NO_CREATE | PendingIntent.FLAG_MUTABLE);
        if (pendingIntent != null)
            pendingIntent.cancel();
    }
}
