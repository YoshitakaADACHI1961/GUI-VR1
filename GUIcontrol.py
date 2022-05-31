#
# 脈拍発生装置　Ver．0.2
#
import sys, shutil, os, time
import tkinter as tk

# constants
PULSE_STEP = 0.04025
PULSE_MAX = 1.205
PULSE_INITIAL = 0.752
PULSE_MIN = 0.4
ON_TIME_INITIAL = 0.1
ON_TIME_MIN = 0.01
ON_TIME_STEP = 0.01

# グローバル変数
run_mode = 'STOP'
interval = PULSE_INITIAL
on_time = ON_TIME_INITIAL

#
# 制御ファイルの修正とコピー
#
def ChangeControlFile():
    global run_mode,interval,on_time
    record = run_mode +','+ str(round(interval,3)) +','+ str(round(on_time,3))
    origin_file = open('RaspberryPiControlOrigin.txt', mode='w')
    origin_file.write(record)
    origin_file.close()
    shutil.copy('RaspberryPiControlOrigin.txt','RaspberryPiControl.txt')

#
# ボタンが押されるとここが呼び出される
#
#def DeleteEntryValue(event):
#    EditBox1.delete(0, tk.END)
#    EditBox2.delete(0, tk.END)

# 周期を長くする
def IntervalUp(event):
    global interval, run_mode
    EditBox1.delete(0, tk.END)
    interval = interval - PULSE_STEP
#    print('interval2:'+str(interval))   
    if interval < PULSE_MIN:
        interval = PULSE_MIN
    if run_mode == 'START':
        EditBox1.insert(tk.END,'> '+str(round(60/interval)))
    else:
        EditBox1.insert(tk.END,str(round(60/interval)))
    ChangeControlFile()

# 周期を短くする
def IntervalDown(event):
    global interval, run_mode
    EditBox1.delete(0, tk.END)
    interval = interval + PULSE_STEP
#    print('interval1:'+str(interval))   
    if interval > PULSE_MAX:
        interval = PULSE_MAX
    if run_mode == 'START':
        EditBox1.insert(tk.END,'> '+str(round(60/interval)))
    else:
        EditBox1.insert(tk.END,str(round(60/interval)))
    ChangeControlFile()

# ONの時間を増やす
def OnTimeUp(event):
    global on_time
    EditBox2.delete(0, tk.END)
    on_time = on_time + ON_TIME_STEP
    if on_time > interval/2.0:
        on_time = interval/2.0
    EditBox2.insert(tk.END,str(round(on_time,2)))
    ChangeControlFile()

# ONの時間を減らす
def OnTimeDown(event):
    global on_time
    EditBox2.delete(0, tk.END)
    on_time = on_time - ON_TIME_STEP
    if on_time < ON_TIME_MIN:
        on_time = ON_TIME_MIN
    EditBox2.insert(tk.END,str(round(on_time,2)))
    ChangeControlFile()

# 脈拍発生の開始
def PulseStart(event):
    global run_mode
    EditBox1.delete(0, tk.END)
    EditBox1.insert(tk.END,'> '+str(round(60/interval)))
    run_mode = 'START'
    ChangeControlFile()

# 脈拍発生の中断
def PulseStop(event):
    global run_mode
    EditBox1.delete(0, tk.END)
    EditBox1.insert(tk.END,str(round(60/interval)))
    run_mode = 'STOP'
    ChangeControlFile()

# 脈拍発生装置の終了
def PulseControlFin(event):
    global run_mode, interval, on_time
    interval = PULSE_INITIAL
    on_time = ON_TIME_INITIAL
    run_mode = 'FIN'
    ChangeControlFile()
    sys.exit()


# 画面の制御
root = tk.Tk()
root.title("Pulse Generator Ver. 0.1")
root.geometry("400x350")
root.resizable(False, False)
txt = tk.Text(root)
txt.pack()

#１行目　周波数　（周期を周波数に変換して表示）
Static1 = tk.Label(text='Interval', foreground='#00ff00', background='#001122')
Static1.place(x=10, y=10)
EditBox1 = tk.Entry(width=30)
EditBox1.insert(tk.END,str(round(60/interval)))
EditBox1.place(x=100, y=10)

# ２行目　ONの時間
Static2 = tk.Label(text='ON time', foreground='#00ff00', background='#001122')
Static2.place(x=10, y=60)
EditBox2 = tk.Entry(width=30)
EditBox2.insert(tk.END,str(round(on_time,2)))
EditBox2.place(x=100, y=60)

#ボタン
Button1 = tk.Button(text='Frequency DOWN', width=20)
Button1.bind("<Button-1>",IntervalDown) 
Button1.place(x=10, y=120)

Button2 = tk.Button(text='Frequency UP', width=20)
Button2.bind("<Button-1>",IntervalUp) 
Button2.place(x=200, y=120)

Button3 = tk.Button(text='ON time DOWN', width=20)
Button3.bind("<Button-1>",OnTimeDown) 
Button3.place(x=10, y=160)

Button4 = tk.Button(text='ON time UP', width=20)
Button4.bind("<Button-1>",OnTimeUp) 
Button4.place(x=200, y=160)

Button5 = tk.Button(text='START', width=20)
Button5.bind("<Button-1>",PulseStart)
Button5.place(x=10, y=220)

Button6 = tk.Button(text='STOP', width=20)
Button6.bind("<Button-1>",PulseStop)
Button6.place(x=200, y=220)

Button7 = tk.Button(text='FIN', width=30)
Button7.bind("<Button-1>",PulseControlFin)
Button7.place(x=100, y=300)

# ファイルの書き込みとコピー
#UTfolder (file://DYNABOOK-HARMAN/Users/y-ada/Documents/UTfolder)
#UTfolder ('C:/Users/Yoshitaka ADACHI/Documents/UTfolder')

os.chdir('C:/Users/y-ada/Documents/UTfolder')
print('Change to C:/Users/y-ada/Documents/UTfolder')
ChangeControlFile()

#
# イベント待ち
#
root.mainloop()