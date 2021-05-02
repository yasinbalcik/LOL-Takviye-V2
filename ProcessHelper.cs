using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;

internal static class ProcessHelper
{
  private const int SW_HIDE = 0;

  [DllImport("kernel32.dll")]
  private static extern IntPtr OpenThread(
    ProcessHelper.ThreadAccess dwDesiredAccess,
    bool bInheritHandle,
    uint dwThreadId);

  [DllImport("kernel32.dll")]
  private static extern uint SuspendThread(IntPtr hThread);

  [DllImport("kernel32.dll")]
  private static extern int ResumeThread(IntPtr hThread);

  [DllImport("user32.dll")]
  private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

  public static void HideWindow(int processId) => ProcessHelper.ShowWindow(Process.GetProcessById(processId).MainWindowHandle, 0);

  public static void SuspendProcess(int processId)
  {
    foreach (ProcessThread thread in (ReadOnlyCollectionBase) Process.GetProcessById(processId).Threads)
    {
      IntPtr hThread = ProcessHelper.OpenThread(ProcessHelper.ThreadAccess.SUSPEND_RESUME, false, (uint) thread.Id);
      if (hThread == IntPtr.Zero)
        break;
      int num = (int) ProcessHelper.SuspendThread(hThread);
    }
  }

  public static void ResumeProcess(int processId)
  {
    foreach (ProcessThread thread in (ReadOnlyCollectionBase) Process.GetProcessById(processId).Threads)
    {
      IntPtr hThread = ProcessHelper.OpenThread(ProcessHelper.ThreadAccess.SUSPEND_RESUME, false, (uint) thread.Id);
      if (hThread == IntPtr.Zero)
        break;
      ProcessHelper.ResumeThread(hThread);
    }
  }

  public static void KillProcess(int processId) => Process.GetProcessById(processId).Kill();

  public enum Options
  {
    List,
    Kill,
    Suspend,
    Resume,
  }

  [Flags]
  public enum ThreadAccess
  {
    TERMINATE = 1,
    SUSPEND_RESUME = 2,
    GET_CONTEXT = 8,
    SET_CONTEXT = 16, // 0x00000010
    SET_INFORMATION = 32, // 0x00000020
    QUERY_INFORMATION = 64, // 0x00000040
    SET_THREAD_TOKEN = 128, // 0x00000080
    IMPERSONATE = 256, // 0x00000100
    DIRECT_IMPERSONATION = 512, // 0x00000200
  }

  public class Param
  {
    public int PID { get; set; }

    public string Expression { get; set; }

    public ProcessHelper.Options Option { get; set; }
  }
}
