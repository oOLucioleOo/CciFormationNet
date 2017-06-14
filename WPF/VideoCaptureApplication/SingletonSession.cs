using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoCaptureApplication
{
    public sealed class SingletonSession
    {
        private static readonly object syncRoot = new object();
        private static volatile SingletonSession instance;
        public static SingletonSession Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new SingletonSession();
                        }
                    }
                }
                return instance;
            } 
        }
        #region Propriétés d'instances
        public USER ConnectedUser { get; set; }
        public bool IsConnected { get; set; }
        #endregion
    }
}
