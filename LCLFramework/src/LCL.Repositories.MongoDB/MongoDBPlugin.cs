﻿
using LCL;
using LCL.ComponentModel;
using LCL.MetaModel;
using System.Diagnostics;

namespace LCL.Repositories.MongoDB
{
    public class BundleActivator : LCLPlugin
    {
        public override void Initialize(IApp app)
        {
            Logger.LogInfo("LCL.Repositories.MongoDB Initialize....");
            ServiceLocator.Instance.Register<IMongoDBRepositoryContext, MongoDBRepositoryContext>();
        }
    }
}
