﻿using System;
using System.Collections.Generic;

namespace UnityTools
{
    public abstract class AssetsReplacer : IDisposable
    {
        public abstract AssetsReplacementType GetReplacementType();

        public abstract int GetFileID();
        public abstract long GetPathID();
        public abstract AssetClassID GetClassID();
        public abstract ushort GetMonoScriptID();
        public abstract void SetMonoScriptID(ushort scriptId);

        public abstract bool GetPropertiesHash(out Hash128 propertiesHash);
        public abstract bool SetPropertiesHash(Hash128 propertiesHash);
        public abstract bool GetScriptIDHash(out Hash128 scriptIdHash);
        public abstract bool SetScriptIDHash(Hash128 scriptIdHash);
        public abstract bool GetTypeInfo(out ClassDatabaseFile file, out ClassDatabaseType type);
        public abstract bool SetTypeInfo(ClassDatabaseFile file, ClassDatabaseType type, bool localCopy);
        public abstract bool GetPreloadDependencies(out List<AssetPPtr> preloadList);
        public abstract bool SetPreloadDependencies(List<AssetPPtr> preloadList);
        public abstract bool AddPreloadDependency(AssetPPtr dependency);

        public abstract long GetSize();
        public abstract long Write(EndianWriter writer);
        public abstract long WriteReplacer(EndianWriter writer);

        public abstract void Dispose();
    }
}
