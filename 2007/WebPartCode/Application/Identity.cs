using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace SharePointKnowledgeBase.Application
{
    public sealed class Identity : IDisposable
    {
        private static WindowsIdentity appPoolIdentity;
        private WindowsImpersonationContext context;
        private WindowsImpersonationContext selfContext;

        private Identity()
        {
            try
            {
                this.selfContext = WindowsIdentity.Impersonate(IntPtr.Zero);
                this.context = AppPoolIdentity.Impersonate();
            }
            catch
            {
                this.UndoImpersonation();
                throw;
            }
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool CloseHandle(IntPtr handle);
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing_)
        {
            if (disposing_)
            {
                this.UndoImpersonation();
            }
        }

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool DuplicateToken(IntPtr hToken_, int impersonationLevel_, ref IntPtr hNewToken_);
        public static Identity ImpersonateAppPool()
        {
            return new Identity();
        }

        public void UndoImpersonation()
        {
            if (this.context != null)
            {
                this.context.Undo();
                this.context = null;
            }
            if (this.selfContext != null)
            {
                this.selfContext.Undo();
                this.selfContext = null;
            }
        }

        private static WindowsIdentity AppPoolIdentity
        {
            get
            {
                lock (typeof(Identity))
                {
                    if (appPoolIdentity == null)
                    {
                        IntPtr token = WindowsIdentity.GetCurrent().Token;
                        if (token == IntPtr.Zero)
                        {
                            throw new ApplicationException("Unable to fetch AppPool's identity token !");
                        }
                        if (!DuplicateToken(token, 2, ref token))
                        {
                            throw new Win32Exception(Marshal.GetLastWin32Error(), "Unable to duplicate AppPool's identity token !");
                        }
                        if (token == IntPtr.Zero)
                        {
                            throw new ApplicationException("Unable to duplicate AppPool's identity token !");
                        }
                        appPoolIdentity = new WindowsIdentity(token);
                        CloseHandle(token);
                    }
                    return appPoolIdentity;
                }
            }
        }

        public static string CurrentUserName
        {
            get
            {
                return WindowsIdentity.GetCurrent().Name;
            }
        }
    }
}