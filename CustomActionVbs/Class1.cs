using System;
using Microsoft.Deployment.WindowsInstaller;
using Microsoft.Win32.TaskScheduler;

namespace CustomActionVBS
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult CreateTaskSchedulerEntry(Session session)
        {
            try
            {
                // Get the installation directory from the session object
                string installDir = session["INSTALLFOLDER"];
                string vbsPath = System.IO.Path.Combine(installDir, "run_wflogon.vbs");

                // Initialize Task Scheduler API
                using (TaskService ts = new TaskService())
                {
                    // Create a new task
                    TaskDefinition td = ts.NewTask();
                    td.RegistrationInfo.Description = "Run VBS at User Logon";

                    // Set task properties
                    td.Triggers.Add(new LogonTrigger());
                    td.Actions.Add(new ExecAction(vbsPath, null, null));

                    // Register the task for all users
                    ts.RootFolder.RegisterTaskDefinition("RunVBSAtLogon", td, TaskCreation.CreateOrUpdate, "SYSTEM", null, TaskLogonType.ServiceAccount);
                }

                return ActionResult.Success;
            }
            catch (Exception ex)
            {
                session.Log("Error: " + ex.Message);
                return ActionResult.Failure;
            }
        }
    }
}
