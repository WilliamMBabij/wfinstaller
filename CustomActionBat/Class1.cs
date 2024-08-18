using Microsoft.Deployment.WindowsInstaller;
using System;
using System.IO;

public class CustomActions
{
    [CustomAction]
    public static ActionResult ModifyBatchFile(Session session)
    {
        try
        {
            session.Log("Begin ModifyBatchFile");

            // Read user input
            string installLocation = session["INSTALLFOLDER"];
            string flags = session["FLAGS"];
            string ipAddresses = session["IP_ADDRESSES"];

            session.Log($"Install Location: {installLocation}");
            session.Log($"Flags: {flags}");
            session.Log($"IP Addresses: {ipAddresses}");

            // Read the template batch file
            string batchContent = File.ReadAllText(Path.Combine(installLocation, "run_wflogon.bat"));

            // Replace placeholders
            batchContent = batchContent.Replace("[INSTALL_LOCATION]", installLocation);
            batchContent = batchContent.Replace("[FLAGS]", flags);
            batchContent = batchContent.Replace("[IP_ADDRESSES]", ipAddresses);

            // Write the modified batch file
            File.WriteAllText(Path.Combine(installLocation, "run_wflogon.bat"), batchContent);

            session.Log("End ModifyBatchFile");

            return ActionResult.Success;
        }
        catch (Exception ex)
        {
            session.Log("ERROR in custom action ModifyBatchFile: " + ex.Message);
            return ActionResult.Failure;
        }
    }
}
